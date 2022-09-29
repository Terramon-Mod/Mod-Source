#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Commands
{
    public class Debug : ModCommand
    {
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                caller.Reply(Usage);
            }

            switch (args[0])
            {
                case "npc":
                    var max = Main.maxNPCs;
                    if (args.Length >= 2)
                    {
                        int.TryParse(args[1], out max);
                        max = Math.Min(max, Main.maxNPCs);
                    }

                    if (Main.netMode == NetmodeID.Server)
                    {
                        for (int i = 0; i < max; i++)
                        {
                            Console.WriteLine($"{i:D3} = {Main.npc[i].GivenName}");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < max; i++)
                        {
                            caller.Reply($"{i:D3} = {Main.npc[i].GivenName}");
                        }
                    }
                    break;
                case "proj":
                    var maxp = Main.maxProjectiles;
                    if (args.Length >= 2)
                    {
                        int.TryParse(args[1], out max);
                        maxp = Math.Min(max, Main.maxProjectiles);
                    }
                    if (Main.netMode == NetmodeID.Server)
                    {
                        for (int i = 0; i < maxp; i++)
                        {
                            Console.WriteLine($"{i:D3} = {Main.projectile[i].Name}");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < maxp; i++)
                        {
                            caller.Reply($"{i:D3} = {Main.projectile[i].Name}");
                        }
                    }
                    break;
            }
        }

        public override string Command => "d";
        public override string Usage => "/d [npc/poj]";
        public override CommandType Type => CommandType.Chat | CommandType.Console;
    }
}
#endif