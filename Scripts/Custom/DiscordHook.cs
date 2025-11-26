using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Server.Misc;

namespace Server.Custom.DiscordHook
{
    public enum SentFrom
    {
        GeneralChat,
        Staff
    }

    public class DiscordHook
    {
        public static string webhook = string.Empty;

        public static void Initialize()
        {
            webhook = Config.Get("Discord.DiscordWebHook", string.Empty);

            if (string.IsNullOrEmpty(webhook))
            {
                Config.Set("Discord.DiscordWebHook", "");
                Config.Save();
                return;
            }

            EventSink.Shutdown += new ShutdownEventHandler(On_Shutdown);
            EventSink.Crashed += new CrashedEventHandler(On_Crash);
            EventSink.PlayerDeath += new PlayerDeathEventHandler(On_PlayerDeath);

            ThreadPool.QueueUserWorkItem(new DiscordHookWorker(null, "```The server is online!```").SendTheMessage);
        }

        public static void SendMessageToDiscord(Mobile from, string message)
        {
            ThreadPool.QueueUserWorkItem(new DiscordHookWorker(from, message).SendTheMessage);
        }

        public static void On_Shutdown(ShutdownEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new DiscordHookWorker(null, "```The server has shutdown!```").SendTheMessage);
        }

        public static void On_Crash(CrashedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new DiscordHookWorker(null, "```On no, the server has crashed!```").SendTheMessage);
        }

        private static readonly string[] deathMessages = new string[]
        {
            "```{0} was turned into a fine red mist by [{1}].```",
            "```{0} learned the hard way that [{1}] doesn't play fair.```",
            "```{0} tried to fight [{1}]... it didn't end well.```",
            "```{0} underestimated [{1}] and paid the ultimate price.```",
            "```{0} is now a permanent resident of the afterlife, courtesy of [{1}].```",
            "```{0} thought they were invincible... [{1}] proved them wrong.```",
            "```{0} got a one-way ticket to the graveyard, thanks to [{1}].```",
            "```{0} was deleted from existence by [{1}].```",
            "```{0} tried to run, but [{1}] was faster.```",
            "```{0} became [{1}]'s latest trophy.```",
            "```{0} zigged when they should have zagged... [{1}] took advantage.```",
            "```{0} is now a cautionary tale about messing with [{1}].```",
            "```{0} forgot to dodge, and [{1}] didn't forget to attack.```",
            "```{0} is now a ghost, haunting [{1}]'s nightmares.```",
            "```{0} tried their best, but [{1}]'s best was better.```",
            "```{0} took a swing at [{1}] and got a dirt nap in return.```",
            "```{0} bet their life against [{1}]... and lost.```",
            "```{0} became a smear on the battlefield, courtesy of [{1}].```",
            "```{0} faced [{1}] with courage — and zero survival instinct.```",
            "```{0} is now part of the scenery, thanks to [{1}].```",
            "```{0} walked into [{1}]'s eye sight and never walked out.```",
            "```{0} challenged [{1}] to a duel... big mistake.```",
            "```{0} just couldn't keep up with [{1}]'s kill streak.```",
            "```{0}'s final words were 'This'll be easy.' [{1}] proved otherwise.```",
            "```{0} forgot that [{1}] always plays for keeps.```"
        };

        public static void On_PlayerDeath(PlayerDeathEventArgs e)
        {
            if (e.Mobile == null || e.Killer == null) return;

            string messageTemplate = deathMessages[RandomImpl.Next(deathMessages.Length)];
            string deathMessage = string.Format(messageTemplate, e.Mobile.Name, e.Killer.Name);
            ThreadPool.QueueUserWorkItem(new DiscordHookWorker(null, deathMessage).SendTheMessage);
        }

        public static string quickJson(string m, string u)
        {
            return "{\"content\": \"" + m + "\",\"username\": \"" + u + "\"}";
        }
    }

    public class DiscordHookWorker
    {
        string from;
        string message;
        SentFrom sentFrom;

        public DiscordHookWorker(Mobile from, string message)
        {
            if (from != null)
            {
                sentFrom = from.AccessLevel > AccessLevel.Player ? SentFrom.Staff : SentFrom.GeneralChat;
                this.from = from.Name;
            }
            else
            {
                sentFrom = SentFrom.GeneralChat;
                this.from = ServerList.ServerName;
            }
            this.message = message;
        }
        public void SendTheMessage(object threadContext)
        {
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(DiscordHook.webhook);
                Request.KeepAlive = false;
                Request.ProtocolVersion = HttpVersion.Version11;
                Request.Method = "POST";

                string MsgContents = "";

                switch (sentFrom)
                {
                    case SentFrom.GeneralChat:
                        MsgContents = message;
                        break;
                    case SentFrom.Staff:
                        MsgContents = "`[Staff]`" + message;
                        break;
                }
                MsgContents = DiscordHook.quickJson(MsgContents, from);

                byte[] content = Encoding.UTF8.GetBytes(MsgContents);

                Request.ContentType = "application/json; charset=UTF-8";
                Request.Accept = "application/json";
                Request.ContentLength = content.Length;
                Stream rStream = Request.GetRequestStream();
                rStream.Write(content, 0, content.Length);
                rStream.Close();
                new ManualResetEvent(true).Set();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
