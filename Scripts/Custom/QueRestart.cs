using Server;
using Server.Commands;
using Server.Misc;
using Server.Network;
using System;

namespace Bittiez
{
    public class QueRestart
    {
        private static bool willRestart = false;
        public static void Initialize()
        {
            CommandSystem.Register("QueRestart", AccessLevel.GameMaster, new CommandEventHandler(restart_OnCommand));
        }

        [Usage("QueRestart")]
        [Description("Restarts the server as soon as no players are online")]
        private static void restart_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Restart will occur when the last player logs out.");

            if(willRestart) return;

            EventSink.Logout += new LogoutEventHandler(onLogout);
            willRestart = true;
        }

        public static void onLogout(LogoutEventArgs e)
        {
            Bittiez.Tools.Start_Timer_Delayed_Call(TimeSpan.FromSeconds(5), CheckRestart);
        }

        private static void CheckRestart()
        {
            int count = 0;
            foreach (NetState state in NetState.Instances)
            {
                if (state.Mobile != null)
                    count++;
            }
            if (count < 1)
            {
                AutoSave.Save();
                World.Broadcast(0x22, true, "The server is restarting.");
                Core.Kill(true);
            }
        }
    }
}
