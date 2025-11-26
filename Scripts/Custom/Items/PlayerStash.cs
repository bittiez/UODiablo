using Server.Accounting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Custom.Items
{
    [FlipableAttribute(0xE43, 0xE42)]
    internal class PlayerStash : Item
    {
        [Constructable]
        public PlayerStash()
        {
            ItemID = 0xE43;
            Name = "Stash";
            Movable = false;
        }

        public PlayerStash(Serial serial) : base(serial)
        {
        }

        public override bool Decays => false;

        public override void OnDoubleClick(Mobile from)
        {
            if (!AccessCheck(from))
            {
                return;
            }

            from.BankBox.Open();

            if (AccountGold.Enabled && from.Account is Account)
            {
                from.SendLocalizedMessage(1155855, String.Format("{0:#,0}\t{1:#,0}",
                    from.Account.TotalPlat,
                    from.Account.TotalGold), 0x3BC);

                from.SendLocalizedMessage(1155848, String.Format("{0:#,0}", ((Account)from.Account).GetSecureAccountAmount(from)), 0x3BC);
            }
            else
            {
                // Thy current bank balance is ~1_AMOUNT~ gold.
                from.SendLocalizedMessage(1042759, Banker.GetBalance(from).ToString("#,0"));
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Your personal stash");
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            list.Add(new WithdrawlContextEntry(from, this));

            var entry = new OpenBankEntry(this, from);
            list.Add(entry);
        }

        private bool AccessCheck(Mobile from)
        {
            if (from == null || !from.Alive || !InRange(from, 5))
            {
                return false;
            }

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

        internal class WithdrawlContextEntry : ContextMenuEntry
        {
            public WithdrawlContextEntry(Mobile from, Item item) : base(1062986)
            {
                From = from;
                Item = item;
                Enabled = item.InRange(from, 2);
            }

            public override void OnClick()
            {
                base.OnClick();

                if (Item.InRange(From, 2))
                {
                    From.SendMessage("How much would you like to withdrawl?");
                    From.BeginPrompt((e, response) =>
                    {
                        if (int.TryParse(response, out int amount))
                        {
                            var pack = From.Backpack;

                            if (pack == null || pack.Deleted || !(pack.TotalWeight < pack.MaxWeight) ||
                                             !(pack.TotalItems < pack.MaxItems))
                            {
                                // Your backpack can't hold anything else.
                                From.SendLocalizedMessage(1048147);
                            }
                            else if (amount > 0)
                            {
                                var box = From.Player ? From.BankBox : From.FindBankNoCreate();

                                if (box == null || !Banker.Withdraw(From, amount))
                                {
                                    // Ah, art thou trying to fool me? Thou hast not so much gold!
                                    From.SendLocalizedMessage(500384);
                                }
                                else
                                {
                                    pack.DropItem(new Gold(amount));

                                    // Thou hast withdrawn gold from thy account.
                                    From.SendLocalizedMessage(1010005);
                                }
                            }
                        }
                        else
                        {
                            From.SendMessage("That was not a valid response.");
                        }
                    }, false);
                }
            }

            public Mobile From { get; }
            public Item Item { get; }
        }

        internal class OpenBankEntry : ContextMenuEntry
        {

            public OpenBankEntry(Item item, Mobile from)
                : base(6105, 12)
            {
                Stash = item;
                Enabled = item.InRange(from, 2);
            }

            public Item Stash { get; }

            public override void OnClick()
            {
                if (!Owner.From.CheckAlive())
                    return;

                Owner.From.BankBox.Open();

                if (Core.TOL && Owner.From is PlayerMobile)
                {
                    Owner.From.CloseGump(typeof(BankerGump));
                    Owner.From.SendGump(new BankerGump((PlayerMobile)Owner.From));
                }
            }
        }
    }
}
