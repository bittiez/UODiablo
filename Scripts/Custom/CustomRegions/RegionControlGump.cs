#region References

using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;

#endregion

namespace Server.Gumps
{
    public class RegionControlGump : Gump
    {
        private readonly RegionControl m_Controller;

        public RegionControlGump(RegionControl r) : base(25, 25)
        {
            m_Controller = r;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddBackground(23, 32, 412, 256, 9270);
            AddAlphaRegion(19, 29, 418, 263);
            AddButton(55, 46, 5569, 5570, (int)Buttons.SpellButton, GumpButtonType.Reply, 0);
            AddButton(55, 128, 5581, 5582, (int)Buttons.SkillButton, GumpButtonType.Reply, 0);
            AddButton(50, 205, 7006, 7006, (int)Buttons.AreaButton, GumpButtonType.Reply, 0);
            AddButton(353, 205, 7006, 7006, (int)Buttons.TextAreaButton, GumpButtonType.Reply, 0);

            AddLabel(152, 70, 1152, "Edit Restricted Spells");
            AddLabel(152, 153, 1152, "Edit Restricted Skills");
            AddLabel(152, 220, 1152, "<- Target Region Area");
            AddLabel(152, 250, 1152, "Type Region Area ->");
            AddImage(353, 54, 3953);
            AddImage(353, 180, 3955);
        }

        private enum Buttons
        {
            SpellButton = 1,
            SkillButton,
            AreaButton,
            TextAreaButton
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Controller == null || m_Controller.Deleted)
            {
                return;
            }

            Mobile m = sender.Mobile;

            switch (info.ButtonID)
            {
                case 1:
                    {
                        //m_Controller.SendRestrictGump( m, RestrictType.Spells );
                        m.CloseGump(typeof(SpellRestrictGump));
                        m.SendGump(new SpellRestrictGump(m_Controller.RestrictedSpells));

                        m.CloseGump(typeof(RegionControlGump));
                        m.SendGump(new RegionControlGump(m_Controller));
                        break;
                    }
                case 2:
                    {
                        //m_Controller.SendRestrictGump( m, RestrictType.Skills );

                        m.CloseGump(typeof(SkillRestrictGump));
                        m.SendGump(new SkillRestrictGump(m_Controller.RestrictedSkills));

                        m.CloseGump(typeof(RegionControlGump));
                        m.SendGump(new RegionControlGump(m_Controller));
                        break;
                    }
                case 3:
                    {
                        m.CloseGump(typeof(RegionControlGump));
                        m.SendGump(new RegionControlGump(m_Controller));

                        m.CloseGump(typeof(RemoveAreaGump));

                        m.SendGump(new RemoveAreaGump(m_Controller));

                        m_Controller.ChooseArea(m);
                        break;
                    }
                case 4:
                    {
                        m.Prompt = new SizePrompt(m_Controller, m);
                        break;
                    }
            }
        }
    }

    public class SizePrompt : Prompt
    {
        public SizePrompt(RegionControl control, Mobile mobile, Point3D? point3D = null)
        {
            Control = control;
            Point3D = point3D;

            mobile.SendMessage($"Please type the -{(Point3D == null ? "first" : "second")}- location as x, y, z:");
        }

        public RegionControl Control { get; }
        public Point3D? Point3D { get; }

        // This method is called when the player enters their response
        public override void OnResponse(Mobile from, string text)
        {
            string[] split = text.Split(',');

            if (split.Length != 3)
            {
                from.SendMessage("You need to type the location as x, y, z.");
            }
            else
            {
                for (int i = 0; i < split.Length; i++)
                {
                    split[i] = split[i].Trim();
                }

                if (int.TryParse(split[0], out int x) && int.TryParse(split[1], out int y) && int.TryParse(split[2], out int z))
                {
                    if(Point3D == null)
                    {
                        from.Prompt = new SizePrompt(Control, from, new Point3D(x, y, z));
                    }
                    else
                    {
                        Point3D end = new Point3D(x, y, z);
                        Point3D start = (Point3D)Point3D;

                        Utility.FixPoints(ref start, ref end);

                        Control.DoChooseArea(from, from.Map, start, end);
                    }
                }
                else
                {
                    from.SendMessage("You need to type the location as x, y, z.");
                }
            }
        }
    }
}
