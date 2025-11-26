using Server;
using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Bittiez.FlexCap
{
	public class FlexCommand
	{
		public static void Initialize()
		{
			CommandSystem.Register("Caps", AccessLevel.Player, new CommandEventHandler(stats_OnCommand));
		}

		private static void stats_OnCommand(CommandEventArgs e)
		{
			Mobile m = e.Mobile;
			if (m.HasGump(typeof(FlexGump)))
				m.CloseGump(typeof(FlexGump));
			m.SendGump(new FlexGump(m));
		}
	}
}

namespace Server.Gumps
{
	public class FlexGump : Gump
	{
		private Mobile caller;

		public FlexGump(Mobile mobile)
			: base(50, 50)
		{
			caller = mobile;

			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;

			string skillcap = caller.SkillsCap.ToString();
			string skilltotal = caller.SkillsTotal.ToString();
			TimeSpan total = ((Account)mobile.Account).TotalGameTime;

			AddPage(0);
			AddBackground(0, 0, 250, 200, 3500);
			AddPage(1);


			int y = 0;
			AddLabel(110, 6 + (20 * y), 10, string.Format("FlexCap"));

			AddLabel(15, 23 + (20 * y), 10, string.Format("Stat Cap: {0}", caller.StatCap));
			y++;
			AddLabel(15, 23 + (20 * y), 10, string.Format("Skill Cap: {0}", skillcap.Insert(skillcap.Length - 1, ".")));
			y++;
			y++;
			y++;
			y++;
			AddLabel(15, 23 + (20 * y), 10, string.Format("Total Stats: {0}", caller.RawStatTotal));
			y++;
			AddLabel(15, 23 + (20 * y), 10, string.Format("Total Skills: {0}", skilltotal.Insert(skilltotal.Length - 1, ".")));
			y++;
			AddLabel(15, 23 + (20 * y), 10, string.Format("Current Age: {0} {1}", Math.Round(total.TotalMinutes, 0), "minutes"));

		}
	}
}
