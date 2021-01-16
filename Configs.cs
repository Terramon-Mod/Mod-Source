using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel;
using Terramon.UI.SidebarParty;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Terramon
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Battle Config")]

        [Label("Battle Text Speed")]
        [Tooltip("Change the speed of text in battles.")]
        [DefaultValue(BattleTextSpeed.Fast)]
        // Using StringEnumConverter, Enums are read and written as strings rather than the numerical value of the Enum. This makes the config file more readable, but prone to errors if a player manually modifies the config file.
        [JsonConverter(typeof(StringEnumConverter))]
        public BattleTextSpeed textSpeed { get; set; }


        public override void OnChanged()
        {
            UISidebar uISidebar = ModContent.GetInstance<TerramonMod>().UISidebar;
            if (uISidebar != null)
            {
                if (!TerramonMod.ShowHelpButton)
                    uISidebar.RemoveChild(uISidebar.choose);
                else
                    uISidebar.Append(uISidebar.choose);
            }
        }
        public enum BattleTextSpeed
        {
            Slow,
            Regular,
            Fast,
            Instant
        }
    }
}