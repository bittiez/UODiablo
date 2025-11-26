using System;
using Server;
using Server.Gumps;
using Server.Network;
using Bittiez.ArrowPM;

namespace Bittiez.ArrowPM
{
    public class MessageGump : Gump
    {
        Mobile Recipient, Sender;
        private PersonalMessage PM;

        public MessageGump(PersonalMessage Message, bool Show_Buttons)
            : base(SETTINGS.MessageGump_X, SETTINGS.MessageGump_Y)
        {
            #region Gump Stuff
            Recipient = Message.Recipient;
            Sender = Message.Sender;
            PM = Message;
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


            AddLabel(5, 3, 0, string.Format("From: {0}", Message.Sender.RawName));
            AddLabel(5, 21, 0, string.Format("Date: {0}", Message.Date.ToShortDateString()));

            AddHtml(5, 46, SETTINGS.MessageGump_W - 10, SETTINGS.MessageGump_H - 79, @Message.Message, true, true);
            if (Show_Buttons)
            {
                AddButton(5, SETTINGS.MessageGump_H - 30, 2445, 2445, 1000, GumpButtonType.Reply, 1000);
                AddLabel(45, SETTINGS.MessageGump_H - 30, 0, @"Save");

                AddButton(SETTINGS.MessageGump_W - 113, SETTINGS.MessageGump_H - 30, 2445, 2445, 0, GumpButtonType.Reply, 0);
                AddLabel((SETTINGS.MessageGump_W - 113) + 30, SETTINGS.MessageGump_H - 30, 0, @"Discard");
            }
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

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0: { break; }
                #region Buttons
                case 1000:
                    {
                        PersonalMessageDeed d = new PersonalMessageDeed(PM);
                        from.AddToBackpack(d);
                        break;
                    }
                #endregion

            }
        }
    }

    public class PersonalMessage
    {
        #region Privates
        private Mobile m_Sender, m_Recipient;
        private DateTime m_Date;
        private string m_Message;
        #endregion
        #region Publics
        public Mobile Sender { get { return m_Sender; } set { m_Sender = value; } }
        public Mobile Recipient { get { return m_Recipient; } set { m_Recipient = value; } }
        public DateTime Date { get { return m_Date; } set { m_Date = value; } }
        public string Message { get { return m_Message; } set { m_Message = value; } }
        #endregion
        public PersonalMessage(Mobile sender, Mobile recipient, DateTime date, string message)
        {
            Sender = sender;
            Recipient = recipient;
            Date = date;
            Message = message;
        }
    }

    public class PersonalMessageDeed : Item
    {
        private PersonalMessage PM;
        public PersonalMessageDeed(PersonalMessage Message)
            : base(0x14F0)
        {
            PM = Message;
            Name = "A Personal Message";
        }
        public PersonalMessageDeed(Serial serial) { }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(string.Format("From: {0}<br>Date: {1}", PM.Sender.RawName, PM.Date.ToShortDateString()));
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new MessageGump(PM, false));
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
                PM = new PersonalMessage(s,r,d,m);
            }
        }
    }
}