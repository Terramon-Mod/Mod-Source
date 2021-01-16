using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
// ReSharper disable ParameterHidesMember
// ReSharper disable LocalVariableHidesMember


namespace Terramon.Players
{
    public class PokeMorphPlayer : ModPlayer        //morph stuff here
    {
        public bool pikachuMorph = false;
        public bool bulbasaurMorph = false;

        public override void FrameEffects()
        {
            if (pikachuMorph)
            {
                player.legs = mod.GetEquipSlot("pikachu_Legs", EquipType.Legs);
                player.body = mod.GetEquipSlot("pikachu_Body", EquipType.Body);
                player.head = mod.GetEquipSlot("pikachu_Head", EquipType.Head);
            }
            if (bulbasaurMorph)
            {
                player.legs = mod.GetEquipSlot("bulbasaur_Legs", EquipType.Legs);
                player.body = mod.GetEquipSlot("bulbasaur_Body", EquipType.Body);
                player.head = mod.GetEquipSlot("bulbasaur_Head", EquipType.Head);
            }
        }

        public override void SetControls()      //to do things like left click attack we need to stop item usages in GlobalItem then when Main.MouseLeft goes true to just do things
        {
            base.SetControls();
        }

        public override void PostUpdateEquips()     
        {
            if (pikachuMorph)
            {
                //buffs here
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (pikachuMorph)
                genGore = false;
            if (bulbasaurMorph)
                genGore = false;
            return true;
        }
    }
}