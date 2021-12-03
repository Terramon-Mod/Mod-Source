using Microsoft.Xna.Framework;
using Razorwing.Framework.Graphics;
using Razorwing.Framework.Localisation;
using Razorwing.Framework.Utils;
using System;
using Terramon.Players;
using Terramon.UI;
using Terraria;
using Terraria.ID;

namespace Terramon.Pokemon.Moves
{
    public class ShootMove : DamageMove
    {
        public override string MoveName => "Shoot";
        public override string MoveDescription => "A powerful move. Launches a firebird projectile at the target.";
        public override int Damage => 80;
        public override int Accuracy => 100;
        public override Target Target => Target.Opponent;
        public override int Cooldown => 60 * 1; //Once per second
        public override PokemonType MoveType => PokemonType.Normal;

        public override int AutoUseWeight(ParentPokemon mon, Vector2 pos, TerramonPlayer player)
        {
            NPC target = GetNearestNPC(pos);
            if (target == null)
                return 0;
            return 30;
        }

        public override bool PerformInWorld(ParentPokemon mon, Vector2 pos, TerramonPlayer player)
        {
            NPC target = GetNearestNPC(pos);
            if (target == null)
                return false;

            player.Attacking = true;
            Vector2 vel = (target.position + (target.Size / 2)) - (mon.projectile.position + (mon.projectile.Size / 2));
            var l = vel.Length();
            vel += target.velocity * (l / 100);//Make predict shoot
            vel.Normalize(); //Direction
            vel *= 15; //Speed
            Projectile.NewProjectile((mon.projectile.position + (mon.projectile.Size / 2)), vel, ProjectileID.DD2PhoenixBowShot, 20, 1f, player.whoAmI);
            return true;
        }

        public const string PROJID_KEY = "move.projID";
        public override bool AnimateTurn(ParentPokemon mon, ParentPokemon target, TerramonPlayer player, PokemonData attacker,
            PokemonData deffender, BattleState state, bool opponent)
        {
            if (AnimationFrame == 1) //At initial frame we pan camera to attacker
            {
                TerramonMod.ZoomAnimator.ScreenPosX(mon.projectile.position.X + 12, 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(mon.projectile.position.Y, 500, Easing.OutExpo);
            }
            else if (AnimationFrame == 140) //Move animation begin after 140 frames
            {
                BattleMode.UI.splashText.SetText("");

                Vector2 vel = (target.projectile.position + (target.projectile.Size / 2)) - (mon.projectile.position + (mon.projectile.Size / 2));
                vel.Normalize();
                vel *= 15;
                int id = Projectile.NewProjectile(mon.projectile.position, vel,
                    ProjectileID.DD2PhoenixBowShot, 0, 0);
                Main.projectile[id].maxPenetrate = 99;
                Main.projectile[id].penetrate = 99;
                Main.projectile[id].tileCollide = false;

                if (attacker.CustomData.ContainsKey(PROJID_KEY))
                {
                    attacker.CustomData[PROJID_KEY] = id.ToString();
                }
                else
                {
                    attacker.CustomData.Add(PROJID_KEY, id.ToString());
                }
            }
            else if (AnimationFrame == 260)//At Last frame we destroy new proj
            {
                InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                var id = int.Parse(attacker.CustomData[PROJID_KEY]);
                if (PostTextLoc.Args.Length >= 4)//If we can extract damage number
                    CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]);//Print combat text at attacked mon position
                Main.projectile[id].timeLeft = 0;
                Main.projectile[id].active = false;
                BattleMode.queueEndMove = true;
            }
            else if (AnimationFrame > 140 && AnimationFrame < 261)
            {
                var id = int.Parse(attacker.CustomData[PROJID_KEY]);
                //Vector2 vel = (target.projectile.position + (target.projectile.Size / 2)) - (mon.projectile.position + (mon.projectile.Size / 2));
                //var l = vel.Length();
                //vel.Normalize();
                //Main.projectile[id].position = mon.projectile.position + (vel * (l * (AnimationFrame / 120)));
                Main.projectile[id].position = Interpolation.ValueAt(AnimationFrame, mon.projectile.position, target.projectile.position, 140, 260,
                    Easing.Out);
                TerramonMod.ZoomAnimator.ScreenPosX(Main.projectile[id].position.X, 1, Easing.None);
                TerramonMod.ZoomAnimator.ScreenPosY(Main.projectile[id].position.Y, 1, Easing.None);
            }

            // This should be at the very bottom of AnimateTurn() in every move.
            if (BattleMode.moveEnd)
            {
                AnimationFrame = 0;
                BattleMode.moveEnd = false;
                return false;
            }

            // IGNORE EVERYTHING BELOW WHEN MAKING YOUR OWN MOVES.
            if (AnimationFrame > 1810) return false;

            return true;
        }
    }
}