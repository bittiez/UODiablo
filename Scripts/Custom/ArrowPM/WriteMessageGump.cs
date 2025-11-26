using System;
using Server;
using Server.Gumps;
using Server.Network;
using Bittiez.ArrowPM;
using System.Collections.Generic;

namespace Bittiez.ArrowPM
{
    public class WriteMessageGump : Gump
    {
        public WriteMessageGump(string Mess, string To)
            : base(SETTINGS.MessageGump_X, SETTINGS.MessageGump_Y)
        {
            #region Gump Stuff
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            #endregion
            #region Start_Gump
            AddPage(0);
            AddBackground(0, 0, SETTINGS.MessageGump_W, SETTINGS.MessageGump_H, 9350);
            AddPage(1);
            #endregion

            AddLabel(5, 3, 0, string.Format("To: "));
            AddTextEntry(25, 3, SETTINGS.MessageGump_W - 30, 20, 38, 0, To);

            AddTextEntry(5, 46, SETTINGS.MessageGump_W - 10, SETTINGS.MessageGump_H - 79, 38, 1, Mess);
            AddButton(5, SETTINGS.MessageGump_H - 30, 2445, 2445, 1000, GumpButtonType.Reply, 1000);
            AddLabel(42, SETTINGS.MessageGump_H - 30, 0, @"Send");

            AddButton(SETTINGS.MessageGump_W - 113, SETTINGS.MessageGump_H - 30, 2445, 2445, 1001, GumpButtonType.Reply, 1001);
            AddLabel((SETTINGS.MessageGump_W - 113) + 40, SETTINGS.MessageGump_H - 30, 0, @"Save");
            AddLogo();
        }
        private void AddLogo()
        {
            AddLabel(SETTINGS.MessageGump_W - 60, 5, 0, @"ArrowPM");
            AddItem(SETTINGS.MessageGump_W - 70, 25, 0xF3F);
            AddItem(SETTINGS.MessageGump_W - 60, 25, 0xF3F);
            AddItem(SETTINGS.MessageGump_W - 50, 25, 0xF3F);
            AddItem(SETTINGS.MessageGump_W - 40, 25, 0xF3F);
        }

        public static void CloseMG(Mobile Sender)
        {
            if (Sender.HasGump(typeof(WriteMessageGump)))
                Sender.CloseGump(typeof(WriteMessageGump));
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0: { break; }
                #region Buttons
                case 1000:
                    {
                        string who = info.TextEntries[0].Text;
                        string message = info.TextEntries[1].Text;

                        Mobile Sender = from;

                        List<Mobile> MC = PMCommand.MessCandis(who);

                        if (MC.Count > SETTINGS.Max_Name_Canididates)
                        {
                            Sender.SendMessage(SETTINGS.Error_Message_Hue, SETTINGS.Too_Many_Matches);
                            CloseMG(Sender);
                            Sender.SendGump(new WriteMessageGump(message, who));
                            return;
                        }

                        if (MC.Count < 1)
                        {
                            Sender.SendMessage(SETTINGS.Error_Message_Hue, SETTINGS.No_Matches);
                            CloseMG(Sender);
                            Sender.SendGump(new WriteMessageGump(message, who));
                            return;
                        }

                        if (MC[0].AccessLevel > SETTINGS.Top_Access && !(Sender.AccessLevel > AccessLevel.Player))
                        {
                            Sender.SendMessage(SETTINGS.Error_Message_Hue, SETTINGS.Above_Top_Access);
                            CloseMG(Sender);
                            Sender.SendGump(new WriteMessageGump(message, who));
                            return;
                        }

                        PersonalMessage PM = new PersonalMessage(Sender, MC[0], DateTime.Now, message);
                        MC[0].SendGump(new MessageGump(PM, true));
                        Sender.SendMessage(SETTINGS.Regular_Hue, SETTINGS.Message_Sent);
						PMCommand.OnMessageSent(Sender);
                        break;
                    }
                case 1001:
                    {
                        string who = info.TextEntries[0].Text;
                        string message = info.TextEntries[1].Text;

                        Mobile Sender = from;

                        List<Mobile> MC = PMCommand.MessCandis(who);

                        if (MC.Count > SETTINGS.Max_Name_Canididates)
                        {
                            Sender.SendMessage(SETTINGS.Error_Message_Hue, SETTINGS.Too_Many_Matches);
                            CloseMG(Sender);
                            Sender.SendGump(new WriteMessageGump(message, who));
                            return;
                        }

                        if (MC.Count < 1)
                        {
                            Sender.SendMessage(SETTINGS.Error_Message_Hue, SETTINGS.No_Matches);
                            CloseMG(Sender);
                            Sender.SendGump(new WriteMessageGump(message, who));
                            return;
                        }

                        PersonalMessage PM = new PersonalMessage(from, MC[0], DateTime.Now, message);
                        PMSaveDeed PSD = new PMSaveDeed(PM);
                        from.AddToBackpack(PSD);
                        break;
                    }
                #endregion

            }
        }
    }

    public class PMSaveDeed : Item
    {
        private PersonalMessage PM;
        public PMSaveDeed(PersonalMessage Message)
            : base(0x14F0)
        {
            PM = Message;
            Name = "A Saved Message";
        }
        public PMSaveDeed(Serial serial) { }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(string.Format("Date: {0}", PM.Date.ToShortDateString()));
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new WriteMessageGump(PM.Message, PM.Recipient.RawName));
			this.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            //v 0
            writer.Write(PM.Sender);
            writer.Write(PM.Recipient);
            writer.Write(PM.Message);
            writer.Write(PM.Date);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int v = reader.ReadInt();

            if (v >= 0)
            {
                Mobile s, r;
                string m;
                DateTime d;

                s = reader.ReadMobile();
                r = reader.ReadMobile();
                m = reader.ReadString();
                d = reader.ReadDateTime();
                PM = new PersonalMessage(s, r, d, m);
            }
        }
    }
}