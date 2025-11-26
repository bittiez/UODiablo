using Server.Misc;

namespace Server.Custom.Skyfly.UODisc.Commands.Custom
{
    [Command]
    public class RestartCommand : ICommand
    {
        public bool IsDisabled { get; set; }

        public string Command => "Restart";

        public AccessLevel AccessLevel => AccessLevel.GameMaster;

        public CommandType CommandType => CommandType.Private;

        public string Description => "Restart the server";

        public string Usage => "Type {prefix}Restart";

        public int MinParameters => 0;

        public void Invoke(CommandHandler handler, CommandEventArgs args)
        {
            DClient.DiscordLog("```Server restarting soon..```");
            AutoRestart.Restart();
        }
    }
}
