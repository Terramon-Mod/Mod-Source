using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Razorwing.Framework.Localisation;
using Terramon.Players;
using Terraria;

namespace Terramon.Pokemon.Moves
{
    public class HealMove : BaseMove
    {
        public override string MoveName => "Heal";
        public override string MoveDescription => "A healing move. It heals 35% of the user's max HP.";

        public override Target Target => Target.Trainer;

        public override int Cooldown => 20 * 60; //20 sec cooldown

        public HealMove()
        {
            PostTextLoc = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("moves.healText", "{0} healed by {1}")));
        }

        public override int AutoUseWeight(ParentPokemon mon, Vector2 target, TerramonPlayer player)
        {
            Player pl = Main.player[player.whoAmI];
            if (!(pl.statLife < pl.statLifeMax - (100 * (pl.statLifeMax / 500f))))
                return 0;
            return (int)Math.Round(100 * ((float)pl.statLife / pl.statLifeMax)); // The less hp left, the more chance to cast
        }

        public override bool PerformInWorld(ParentPokemon mon, Vector2 target, TerramonPlayer player)
        {
            Player pl = Main.player[player.whoAmI];
            if (pl.statLife < pl.statLifeMax - (100 * ((float)pl.statLifeMax / 500f))) //The more hp player have the more hp threshold
            {
                pl.HealEffect(200 * (pl.statLifeMax / 500));
                pl.statLife += 200;
                return true;
            }
            return false;
        }

        public override bool PerformInBattle(ParentPokemon mon, ParentPokemon target, TerramonPlayer player, PokemonData attacker,
            PokemonData deffender, BaseMove move)
        {
            var heal = (int) (attacker.MaxHP * 0.35f);
            var d = attacker.HP;
            attacker.HP += heal;
            d = attacker.HP - d;
            CombatText.NewText(mon.projectile.Hitbox, CombatText.HealLife, d, true);
            PostTextLoc.Args = new object[] {attacker.PokemonName, d};
            return true;
        }

        public override bool AnimateTurn(ParentPokemon mon, ParentPokemon target, TerramonPlayer player, PokemonData attacker,
            PokemonData deffender, BattleState state, bool opponent)
        {
            return false;
        }
    }
}
