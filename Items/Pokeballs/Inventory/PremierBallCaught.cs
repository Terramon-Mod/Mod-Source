using System.Collections.Generic;
using System.Linq;
using Razorwing.Framework.Localisation;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Inventory
{
    public class PremierBallCaught : BaseCaughtClass
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Premier Ball");
            Tooltip.SetDefault(pokeballTooltip?.Value ?? "");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (pokeName.Value != PokemonName)
            {
                pokeName = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(PokemonName));
            }

            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");
            if (nameLine != null) nameLine.text = "Premier Ball (" + PokemonName + (isShiny ? " ✦)" : ")");

            pokeballTooltip.Args = new object[] { isShiny ? pokeName.Value + " ✦" : pokeName.Value };
            tooltips.Find(x => x.Name == "Tooltip0").text = pokeballTooltip.Value;
            base.ModifyTooltips(tooltips);
        }
    }
}