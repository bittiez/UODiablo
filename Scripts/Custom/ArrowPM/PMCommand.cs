using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server;
using Server.Network;

namespace Bittiez.ArrowPM
{
	public class PMCommand
	{
		public const String Version = "1.1.0";
		public static Dictionary<String, DateTime> MessagesDelayTracker = new Dictionary<String, DateTime>();
		public static void Initialize()
		{
			foreach (string pre in SETTINGS.COMMAND_PREFIX)
				CommandSystem.Register(pre, AccessLevel.Player, new CommandEventHandler(On_PM));

			ConsoleWrite(ConsoleColor.Blue, "ArrowPM Version " + Version);
		}

		private static void ConsoleWrite(ConsoleColor Color, string Text)
		{
			ConsoleColor o = Console.ForegroundColor;
			Console.ForegroundColor = Color;
			Console.WriteLine(Text);
			Console.ForegroundColor = o;
		}

		public static void On_PM(CommandEventArgs e)
		{
			Mobile Sender = e.Mobile;

			if (!CanSendMessage(Sender))
			{
				Sender.SendMessage(SETTINGS.Error_Message_Hue, SETTINGS.Too_Soon);
				return;
			}

			if (e.ArgString == null || e.ArgString == "")
			{
				Sender.SendGump(new WriteMessageGump("Write your message here", ""));
				return;
			}

			string Recipient = e.GetString(0);
			string Message = "";

			if (e.Arguments.Length < 2)
			{
				Sender.SendGump(new WriteMessageGump("Write your message here", Recipient));
				return;
			}

			if (e.Arguments.Length > 1)
				Message = e.ArgString.Substring(Recipient.Length + 1, e.ArgString.Length - Recipient.Length - 1);

			List<Mobile> MC = MessCandis(Recipient);

			if (MC.Count > SETTINGS.Max_Name_Canididates)
			{
				Sender.SendMessage(SETTINGS.Error_Message_Hue, SETTINGS.Too_Many_Matches);
				return;
			}

			if (MC.Count < 1)
			{
				Sender.SendMessage(SETTINGS.Error_Message_Hue, SETTINGS.No_Matches);
				return;
			}

			PersonalMessage PM = new PersonalMessage(Sender, MC[0], DateTime.Now, Message);
			MC[0].SendGump(new MessageGump(PM, true));
			Sender.SendMessage(SETTINGS.Regular_Hue, SETTINGS.Message_Sent);
			OnMessageSent(Sender);
		}

		public static bool CanSendMessage(Mobile who)
		{
			DateTime lastMessage = DateTime.MinValue;
			if (MessagesDelayTracker.TryGetValue(who.Account.Username, out lastMessage))
				if (lastMessage > DateTime.UtcNow - SETTINGS.Delay_Between_Messages)
					return false;
			return true;
		}

		public static void OnMessageSent(Mobile sender)
		{
			if (MessagesDelayTracker.ContainsKey(sender.Account.Username))
			{
				MessagesDelayTracker[sender.Account.Username] = DateTime.UtcNow;
			}
			else
			{
				MessagesDelayTracker.Add(sender.Account.Username, DateTime.UtcNow);
			}
		}

		public static List<Mobile> MessCandis(string name)
		{
			List<Mobile> MC = new List<Mobile>();
			List<Mobile> OP = List_Connected_Players();
			foreach (Mobile m in OP)
				if (m.RawName.ToLower().Contains(name.ToLower()))
					MC.Add(m);
			return MC;
		}

		private static List<Mobile> List_Connected_Players()
		{
			List<Mobile> Players = new List<Mobile>();

			foreach (NetState ns in NetState.Instances)
				if (ns.Mobile != null)
					Players.Add(ns.Mobile);
			return Players;
		}
	}
}
