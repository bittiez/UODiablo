// Treasure Chest Pack - Version 0.99H
// By Nerun

using System;

namespace Server.Items
{
    public abstract class BaseTreasureChestMod : LockableContainer
    {
        private ChestTimer m_DeleteTimer;
        public bool IsChestDeleteTimerStarted { get { return m_DeleteTimer != null; } }

        private bool m_OpenedOnce = false;
        /// <summary>
        /// Has this been opened yet? true if yes, false if no
        /// </summary>
        public bool OpenedOnce { get { return m_OpenedOnce; } }
        //public override bool Decays { get{ return true; } }
        //public override TimeSpan DecayTime{ get{ return TimeSpan.FromMinutes( Utility.Random( 10, 15 ) ); } }
        public override int DefaultGumpID { get { return 0x42; } }
        public override int DefaultDropSound { get { return 0x42; } }
        public override Rectangle2D Bounds { get { return new Rectangle2D(20, 105, 150, 180); } }
        public override bool IsDecoContainer { get { return false; } }

        public BaseTreasureChestMod(int itemID) : base(itemID)
        {
            Locked = true;
            Movable = false;

            Key key = (Key)FindItemByType(typeof(Key));

            if (key != null)
                key.Delete();

            if (Core.SA)
                RefinementComponent.Roll(this, 1, 0.08);

            if (Utility.RandomBool())
            {
                AddItem(Loot.RandomHealOrManaPot(Utility.Random(2)));
            }
        }

        public BaseTreasureChestMod(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);

            writer.Write(m_OpenedOnce);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (!Locked)
                StartDeleteTimer();

            switch (version)
            {
                case 1:
                    m_OpenedOnce = reader.ReadBool();
                    break;
            }
        }

        public override void OnTelekinesis(Mobile from)
        {
            if (CheckLocked(from))
            {
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5022);
                Effects.PlaySound(Location, Map, 0x1F5);
                return;
            }

            base.OnTelekinesis(from);
            Name = "a treasure chest";
            StartDeleteTimer();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (CheckLocked(from))
                return;

            base.OnDoubleClick(from);
            Name = "a treasure chest";
            StartDeleteTimer();
        }

        public override void Open(Mobile from)
        {
            if (!m_OpenedOnce)
            {
                BeforeFirstOpened(from);
            }

            base.Open(from);

            if (!m_OpenedOnce)
            {
                FirstOpened(from);
                m_OpenedOnce = true;
            }
        }

        public virtual void BeforeFirstOpened(Mobile from) { }
        public virtual void FirstOpened(Mobile from) { }

        protected void AddLoot(Item item)
        {
            if (item == null)
                return;

            if (Core.SA && RandomItemGenerator.Enabled)
            {
                int min, max;
                TreasureMapChest.GetRandomItemStat(out min, out max);

                RunicReforging.GenerateRandomItem(item, 0, min, max);
            }

            DropItem(item);
        }

        private void StartDeleteTimer()
        {
            if (m_DeleteTimer == null)
                m_DeleteTimer = new ChestTimer(this);
            else
                m_DeleteTimer.Delay = TimeSpan.FromSeconds(Utility.Random(1, 2));

            m_DeleteTimer.Start();
        }

        private class ChestTimer : Timer
        {
            private BaseTreasureChestMod m_Chest;

            public ChestTimer(BaseTreasureChestMod chest) : base(TimeSpan.FromMinutes(Utility.Random(2, 5)))
            {
                m_Chest = chest;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                m_Chest.Delete();
            }
        }
    }
}
