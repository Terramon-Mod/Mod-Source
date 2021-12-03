﻿using System.Diagnostics;
using Terramon.Players;
using Terramon.Pokemon;
using Terramon.UI;
using Terramon.UI.SidebarParty;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Commands
{
    internal class Wiki : ModCommand
    {
        public override CommandType Type => CommandType.Chat;

        public override string Command => "wiki";

        public override string Description => "Launch the Terramon wiki.";

        public override string Usage => "/wiki";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                TerramonMod terramonMod = ModContent.GetInstance<TerramonMod>();
                terramonMod._battle.SetState(BattleMode.UI = new BattleUI());
                terramonMod.summaryUIInterface.SetState(terramonMod.summaryUI = new AnimatorUI());
                //Process.Start("https://terrariamods.gamepedia.com/Terramon");

            }
        }
    }
}