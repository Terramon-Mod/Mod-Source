using System.Diagnostics;
using Terraria.ModLoader;

namespace Terramon.Commands
{
    internal class Discord : ModCommand
    {
        public override CommandType Type => CommandType.Chat;

        public override string Command => "discord";

        public override string Description => "Join the Terramon Discord server.";

        public override string Usage => "/discord";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                //caller.Reply("Opened Discord link...");
                //Process.Start("https://discord.gg/U8skDEA");
            }
        }
    }
}