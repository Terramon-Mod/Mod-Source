using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Razorwing.Framework.Localisation;
using Terramon.Players;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.Moves
{
    public abstract class DamageMove : BaseMove
    {
        public abstract int Damage { get; }// Perc 200-100

        public virtual int Accuracy => 100;
        public bool Miss => _mrand.Next(100) > Accuracy;
        public virtual bool Special => false;
        public virtual bool MakesContact => false;

        public SoundEffectInstance MoveSound;

        public DamageMove()
        {
            PostTextLoc =
                TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("moves.baseDamageText", "{0} attacked {1} with {2} for {3} damage")));

        }

        public override bool PerformInBattle(ParentPokemon mon, ParentPokemon target, TerramonPlayer player, PokemonData attacker,
            PokemonData deffender, BaseMove move)
        {
            if (player == null)
            {
                BattleMode.UI.splashText.SetText($"The wild {attacker.PokemonName} used {MoveName}!");
            }
            else
            {
                BattleMode.UI.splashText.SetText($"{attacker.PokemonName} used {MoveName}!");
            }
            return true;
        }

        public override void CheckIfAffects(ParentPokemon target, PokemonData deffender, BattleState state, bool opponent)
        {
            if (target.PokemonTypes.Contains(PokemonType.Ghost) && MoveType == PokemonType.Normal ||
            target.PokemonTypes.Contains(PokemonType.Ghost) && MoveType == PokemonType.Fighting ||
            target.PokemonTypes.Contains(PokemonType.Steel) && MoveType == PokemonType.Poison ||
            target.PokemonTypes.Contains(PokemonType.Flying) && MoveType == PokemonType.Ground ||
            target.PokemonTypes.Contains(PokemonType.Normal) && MoveType == PokemonType.Ghost ||
            target.PokemonTypes.Contains(PokemonType.Ground) && MoveType == PokemonType.Electric ||
            target.PokemonTypes.Contains(PokemonType.Dark) && MoveType == PokemonType.Psychic ||
            target.PokemonTypes.Contains(PokemonType.Fairy) && MoveType == PokemonType.Dragon)
            {
                if (opponent) BattleMode.UI.splashText.SetText($"It doesn't affect {deffender.PokemonName}!");
                else if (state == BattleState.BattleWithWild)
                {
                    BattleMode.UI.splashText.SetText($"It doesn't affect the wild {deffender.PokemonName}...");
                }
                else if (state == BattleState.BattleWithPlayer)
                {
                    BattleMode.UI.splashText.SetText($"It doesn't affect the foe's {deffender.PokemonName}...");
                }
                BattleMode.endMoveTimer = -120;
                BattleMode.animWindow = 1600;
                MoveSound?.Stop();
            }
        }

        public float InflictDamage(ParentPokemon mon, ParentPokemon target, TerramonPlayer player, PokemonData attacker,
            PokemonData deffender, BattleState state, bool opponent, bool checkZeroResist = true)
        {
            Mod mod = ModContent.GetInstance<TerramonMod>();

            int attackerphysDefModifier = 0;
            int attackerspDefModifier = 0;
            int attackercritRatioModifier = 0;

            int deffenderphysDefModifier = 0;
            int deffenderspDefModifier = 0;

            bool critical = false;
            bool stab = false;

            if (attacker.CustomData.ContainsKey("CritRatioModifier")) attackercritRatioModifier = int.Parse(attacker.CustomData["CritRatioModifier"]);

            if (deffender.CustomData.ContainsKey("PhysDefModifier")) deffenderphysDefModifier = int.Parse(deffender.CustomData["PhysDefModifier"]);
            if (deffender.CustomData.ContainsKey("SpDefModifier")) deffenderspDefModifier = int.Parse(deffender.CustomData["SpDefModifier"]);

            // Same type attack bonus (STAB)
            if (mon.PokemonTypes.Length > 1)
            {
                if (mon.PokemonTypes[0] == MoveType || mon.PokemonTypes[1] == MoveType) stab = true;
            }
            else
            {
                if (mon.PokemonTypes[0] == MoveType) stab = true;
            }

            var p = Damage; // p = Power
            float d;

            // critical hit chance
            if (GetCritChance(attackercritRatioModifier) == 1)
            {
                critical = true;
                // ignore attackers negative attack/spatk stat stages
                if (attackerphysDefModifier < 0) attackerphysDefModifier = 0;
                if (attackerspDefModifier < 0) attackerspDefModifier = 0;
                // ignore defenders positive defense/spdef stat stages
                if (deffenderphysDefModifier > 0) deffenderphysDefModifier = 0;
                if (deffenderspDefModifier > 0) deffenderspDefModifier = 0;
                CombatText.NewText(target.projectile.Hitbox, Microsoft.Xna.Framework.Color.LightGray, "Critical hit!");
            }
            else
            if (_mrand.Next(1, GetCritChance(attackercritRatioModifier)) == 1)
            {
                critical = true;
                // ignore attackers negative attack/spatk stat stages
                if (attackerphysDefModifier < 0) attackerphysDefModifier = 0;
                if (attackerspDefModifier < 0) attackerspDefModifier = 0;
                // ignore defenders positive defense/spdef stat stages
                if (deffenderphysDefModifier > 0) deffenderphysDefModifier = 0;
                if (deffenderspDefModifier > 0) deffenderspDefModifier = 0;
                CombatText.NewText(target.projectile.Hitbox, Microsoft.Xna.Framework.Color.LightGray, "Critical hit!");
            }

            // Move resist
            float r1 = 1f, r2 = 1f;
            r1 = deffender.Types[0].GetResist(MoveType);
            if (deffender.Types.Length > 1) r2 = deffender.Types[1].GetResist(MoveType);

            float Modifier; // Modifier = Targets * Weather * Badge * Critical * random * STAB * Type * Burn * other

            float Targets = 1f; // Targets is 0.75 if the move has more than one target, and 1 otherwise.
            float Weather = 1f; // Weather is 1.5 if a Water-type move is being used during rain or a Fire-type move during harsh sunlight, and 0.5 if a Water-type move is used during harsh sunlight or a Fire-type move during rain, and 1 otherwise.
            float Critical = critical ? 1.5f : 1f; // Critical is 1.5 for a critical hit, and 1 otherwise.
            float random = _mrand.NextFloat(0.85f, 1f); // random is a random factor between 0.85 and 1.00 (inclusive)
            float STAB = stab ? 1.5f : 1f; // STAB is the same-type attack bonus. This is equal to 1.5 if the move's type matches any of the user's types, 2 if the user of the move additionally has Adaptability, and 1 if otherwise.
            float Type = r1 * r2; // Type is the type effectiveness. This can be 0 (ineffective); 0.25, 0.5 (not very effective); 1 (normally effective); 2, or 4 (super effective), depending on both the move's and target's types.
            float Burn = 1f; // Burn is 0.5 (from Generation III onward) if the attacker is burned, its Ability is not Guts, and the used move is a physical move (other than Facade from Generation VI onward), and 1 otherwise.
            float other = 1f; // Add later

            Modifier = Targets * Weather * Critical * random * STAB * Type * Burn * other;

            if (!Special)
            {
                float A = attacker.PhysDmg;
                float D = Sam(deffender.PhysDef, deffenderphysDefModifier, GetStat.Defense, critical);
                mod.Logger.Debug(A + " " + D + " " + p);
                d = (float)(Math.Floor(Math.Floor(Math.Floor(2 * (float)attacker.Level / 5 + 2) * A * p / D) / 50) + 2) * Modifier;
            }
            else
            {
                float A = attacker.SpDmg;
                float D = Sam(deffender.SpDef, deffenderspDefModifier, GetStat.SpDef, critical);
                mod.Logger.Debug(A + " " + D + " " + p);
                d = (float)(Math.Floor(Math.Floor(Math.Floor(2 * (float)attacker.Level / 5 + 2) * A * p / D) / 50) + 2) * Modifier;
            }

            if (r1 * r2 < 0.6f) Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/Damage0").WithVolume(.8f));
            else if (r1 * r2 > 3.9f) Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/Damage2").WithVolume(.8f));
            else Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/Damage1").WithVolume(.8f));

            float oldD = d;
            d = deffender.Damage((int)Math.Abs(d));
            target.damageReceived = true;
            PostTextLoc.Args = new object[] { attacker.PokemonName, deffender.PokemonName, MoveName, (int)d };
            mod.Logger.Debug($"{attacker.PokemonName} attacked {deffender.PokemonName} with {MoveName} for {oldD} damage internally. Actually dealt: {d}. Critical = {critical}. Has STAB = {stab}. [Modifier information] Targets = {Targets}. Weather = {Weather}. Critical Boost = {Critical}. Random Factor = {random}. STAB Boost = {STAB}. Type Effectiveness Multiplier = {Type}.");

            return d;
        }

        public float InflictDamage(BattleOpponent atacker, BattleOpponent defender)
        {
            Mod mod = TerramonMod.Instance;

            int attackerphysDefModifier = 0;
            int attackerspDefModifier = 0;
            int attackercritRatioModifier = 0;

            int deffenderphysDefModifier = 0;
            int deffenderspDefModifier = 0;

            bool critical = false;
            bool stab = false;

            if (atacker.PokeData.CustomData.ContainsKey("CritRatioModifier")) attackercritRatioModifier = int.Parse(atacker.PokeData.CustomData["CritRatioModifier"]);

            if (defender.PokeData.CustomData.ContainsKey("PhysDefModifier")) deffenderphysDefModifier = int.Parse(defender.PokeData.CustomData["PhysDefModifier"]);
            if (defender.PokeData.CustomData.ContainsKey("SpDefModifier")) deffenderspDefModifier = int.Parse(defender.PokeData.CustomData["SpDefModifier"]);

            // Same type attack bonus (STAB)
            if (atacker.PokeProj.PokemonTypes.Length > 1)
            {
                if (atacker.PokeProj.PokemonTypes[0] == MoveType || atacker.PokeProj.PokemonTypes[1] == MoveType) stab = true;
            }
            else
            {
                if (atacker.PokeProj.PokemonTypes[0] == MoveType) stab = true;
            }

            var p = Damage; // p = Power
            float d;

            // critical hit chance
            if (GetCritChance(attackercritRatioModifier) == 1)
            {
                critical = true;
                // ignore attackers negative attack/spatk stat stages
                if (attackerphysDefModifier < 0) attackerphysDefModifier = 0;
                if (attackerspDefModifier < 0) attackerspDefModifier = 0;
                // ignore defenders positive defense/spdef stat stages
                if (deffenderphysDefModifier > 0) deffenderphysDefModifier = 0;
                if (deffenderspDefModifier > 0) deffenderspDefModifier = 0;
                CombatText.NewText(defender.PokeProj.projectile.Hitbox, Microsoft.Xna.Framework.Color.LightGray, "Critical hit!");
            }
            else
            if (_mrand.Next(1, GetCritChance(attackercritRatioModifier)) == 1)
            {
                critical = true;
                // ignore attackers negative attack/spatk stat stages
                if (attackerphysDefModifier < 0) attackerphysDefModifier = 0;
                if (attackerspDefModifier < 0) attackerspDefModifier = 0;
                // ignore defenders positive defense/spdef stat stages
                if (deffenderphysDefModifier > 0) deffenderphysDefModifier = 0;
                if (deffenderspDefModifier > 0) deffenderspDefModifier = 0;
                CombatText.NewText(defender.PokeProj.projectile.Hitbox, Microsoft.Xna.Framework.Color.LightGray, "Critical hit!");
            }

            // Move resist
            float r1 = 1f, r2 = 1f;
            r1 = defender.PokeData.Types[0].GetResist(MoveType);
            if (defender.PokeData.Types.Length > 1) r2 = defender.PokeData.Types[1].GetResist(MoveType);

            float Modifier; // Modifier = Targets * Weather * Badge * Critical * random * STAB * Type * Burn * other

            float Targets = 1f; // Targets is 0.75 if the move has more than one target, and 1 otherwise.
            float Weather = 1f; // Weather is 1.5 if a Water-type move is being used during rain or a Fire-type move during harsh sunlight, and 0.5 if a Water-type move is used during harsh sunlight or a Fire-type move during rain, and 1 otherwise.
            float Critical = critical ? 1.5f : 1f; // Critical is 1.5 for a critical hit, and 1 otherwise.
            float random = _mrand.NextFloat(0.85f, 1f); // random is a random factor between 0.85 and 1.00 (inclusive)
            float STAB = stab ? 1.5f : 1f; // STAB is the same-type attack bonus. This is equal to 1.5 if the move's type matches any of the user's types, 2 if the user of the move additionally has Adaptability, and 1 if otherwise.
            float Type = r1 * r2; // Type is the type effectiveness. This can be 0 (ineffective); 0.25, 0.5 (not very effective); 1 (normally effective); 2, or 4 (super effective), depending on both the move's and target's types.
            float Burn = 1f; // Burn is 0.5 (from Generation III onward) if the attacker is burned, its Ability is not Guts, and the used move is a physical move (other than Facade from Generation VI onward), and 1 otherwise.
            float other = 1f; // Add later

            Modifier = Targets * Weather * Critical * random * STAB * Type * Burn * other;

            if (!Special)
            {
                float A = atacker.PokeData.PhysDmg;
                float D = Sam(defender.PokeData.PhysDef, deffenderphysDefModifier, GetStat.Defense, critical);
                mod.Logger.Debug(A + " " + D + " " + p);
                d = (float)(Math.Floor(Math.Floor(Math.Floor(2 * (float)atacker.PokeData.Level / 5 + 2) * A * p / D) / 50) + 2) * Modifier;
            }
            else
            {
                float A = atacker.PokeData.SpDmg;
                float D = Sam(defender.PokeData.SpDef, deffenderspDefModifier, GetStat.SpDef, critical);
                mod.Logger.Debug(A + " " + D + " " + p);
                d = (float)(Math.Floor(Math.Floor(Math.Floor(2 * (float)atacker.PokeData.Level / 5 + 2) * A * p / D) / 50) + 2) * Modifier;
            }

            if (r1 * r2 < 0.6f) Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/Damage0").WithVolume(.8f));
            else if (r1 * r2 > 3.9f) Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/Damage2").WithVolume(.8f));
            else Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/Damage1").WithVolume(.8f));

            float oldD = d;
            d = defender.PokeData.Damage((int)Math.Abs(d));
            defender.PokeProj.damageReceived = true;
            PostTextLoc.Args = new object[] { atacker.PokeData.PokemonName, defender.PokeData.PokemonName, MoveName, (int)d };
            mod.Logger.Debug($"{atacker.PokeData.PokemonName} attacked {defender.PokeData.PokemonName} with {MoveName} for {oldD} damage internally. Actually dealt: {d}. Critical = {critical}. Has STAB = {stab}. [Modifier information] Targets = {Targets}. Weather = {Weather}. Critical Boost = {Critical}. Random Factor = {random}. STAB Boost = {STAB}. Type Effectiveness Multiplier = {Type}.");

            return d;
        }

        /// <summary>
        /// Sam = Stat after multiplier.
        /// Use Aesam() for accuracy + evasion, as it follows a different system for multiplier calculations.
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public float Sam(int value, int modifier, GetStat stat, bool crit)
        {
            if (modifier == -6) return value * (2f / 8f);
            if (modifier == -5) return value * (2f / 7f);
            if (modifier == -4) return value * (2f / 6f);
            if (modifier == -3) return value * (2f / 5f);
            if (modifier == -2) return value * (2f / 4f);
            if (modifier == -1) return value * (2f / 3f);
            if (modifier == 0) return value * (2f / 2f);
            if (modifier == 1) return value * (3f / 2f);
            if (modifier == 2) return value * (4f / 2f);
            if (modifier == 3) return value * (5f / 2f);
            if (modifier == 4) return value * (6f / 2f);
            if (modifier == 5) return value * (7f / 2f);
            if (modifier == 6) return value * (8f / 2f);
            return value;
        }

        /// <summary>
        /// Restores HP to a given Pokemon
        /// </summary>
        /// <param name="pokemon">The Pokemon data of the target</param>
        /// <param name="target">The Pokemon projectile of the target</param>
        /// <param name="amount">The integer amount to heal</param>
        public int SelfHeal(PokemonData pokemon, ParentPokemon target, int amount)
        {
            Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/Heal0").WithVolume(.8f));
            if (pokemon.HP + amount > pokemon.MaxHP)
            {
                amount = pokemon.HP + amount - pokemon.MaxHP;
            }
            pokemon.HP += amount;
            target.healedHealth = true;
            return amount;
        }

        /// <summary>
        /// Adjusts modifier of the given stat for a target
        /// Returns string like "Bulbasaur's defense rose sharply!".
        /// </summary>
        /// <param name="pokemon">The Pokemon data of the target</param>
        /// <param name="target">The Pokemon projectile of the target</param>
        /// <param name="stat">String value of the stat to be adjusted</param>
        /// <param name="modifier">How many points to modify</param>
        /// <param name="state">The current BattleState</param>
        /// <param name="opponent">Whether or not this being called from wild Pokemon or trainer</param>
        public ILocalisedBindableString ModifyStat(PokemonData pokemon, ParentPokemon target, GetStat stat, int modifier, BattleState state, bool opponent)
        {
            ILocalisedBindableString text;

            string statname = "";
            string adjustment = "";

            if (opponent)
            {
                text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("moves.modifyStatText", "{0}'s {1} {2}")));
            }
            else
            {
                text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("moves.modifyStatText", "Wild {0}'s {1} {2}")));
            }

            if (modifier == -3) adjustment = "severely fell!";
            if (modifier == -2) adjustment = "harshly fell!";
            if (modifier == -1) adjustment = "fell!";
            if (modifier == 1) adjustment = "rose!";
            if (modifier == 2) adjustment = "sharply rose!";
            if (modifier == 3) adjustment = "drastically rose!";

            if (stat == GetStat.Defense)
            {
                statname = "Defense";
                if (pokemon.CustomData.ContainsKey("PhysDefModifier"))
                {
                    if (int.Parse(pokemon.CustomData["PhysDefModifier"]) == 6 && modifier > 0)
                    {
                        pokemon.CustomData["PhysDefModifier"] = "6";
                        adjustment = "won't go higher!";
                        text.Args = new object[]
                        {
                            pokemon.PokemonName,
                            statname,
                            adjustment
                        };
                        return text; // Cant go any higher!
                    }
                    else if (int.Parse(pokemon.CustomData["PhysDefModifier"]) == -6 && modifier < 0)
                    {
                        pokemon.CustomData["PhysDefModifier"] = "-6";
                        adjustment = "won't go lower!";
                        text.Args = new object[]
                        {
                            pokemon.PokemonName,
                            statname,
                            adjustment
                        };
                        return text; // Cant go any lower!
                    }
                }

                if (pokemon.CustomData.ContainsKey("PhysDefModifier"))
                {
                    int a = int.Parse(pokemon.CustomData["PhysDefModifier"]);
                    int b = modifier;
                    pokemon.CustomData["PhysDefModifier"] = (a + b).ToString();
                    if (modifier > 0)
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatRise").WithVolume(.8f));
                        target.statModifiedUp = true;
                    }
                    else
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatFall").WithVolume(.8f));
                        target.statModifiedDown = true;
                    }
                }
                else
                {
                    pokemon.CustomData.Add("PhysDefModifier", modifier.ToString());
                    if (modifier > 0)
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatRise").WithVolume(.8f));
                        target.statModifiedUp = true;
                    }
                    else
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatFall").WithVolume(.8f));
                        target.statModifiedDown = true;
                    }
                }

                if (int.Parse(pokemon.CustomData["PhysDefModifier"]) > 6)
                {
                    pokemon.CustomData["PhysDefModifier"] = "6";
                }

                if (int.Parse(pokemon.CustomData["PhysDefModifier"]) < -6)
                {
                    pokemon.CustomData["PhysDefModifier"] = "-6";
                }

                text.Args = new object[]
                {
                    pokemon.PokemonName,
                    statname,
                    adjustment
                };
                return text;
            }

            if (stat == GetStat.SpDef)
            {
                statname = "Special Defense";
                if (pokemon.CustomData.ContainsKey("SpDefModifier"))
                {
                    if (int.Parse(pokemon.CustomData["SpDefModifier"]) == 6 && modifier > 0)
                    {
                        pokemon.CustomData["SpDefModifier"] = "6";
                        adjustment = "won't go higher!";
                        text.Args = new object[]
                        {
                            pokemon.PokemonName,
                            statname,
                            adjustment
                        };
                        return text; // Cant go any higher!
                    }
                    else if (int.Parse(pokemon.CustomData["SpDefModifier"]) == -6 && modifier < 0)
                    {
                        pokemon.CustomData["SpDefModifier"] = "-6";
                        adjustment = "won't go lower!";
                        text.Args = new object[]
                        {
                            pokemon.PokemonName,
                            statname,
                            adjustment
                        };
                        return text; // Cant go any lower!
                    }
                }

                if (pokemon.CustomData.ContainsKey("SpDefModifier"))
                {
                    int a = int.Parse(pokemon.CustomData["SpDefModifier"]);
                    int b = modifier;
                    pokemon.CustomData["SpDefModifier"] = (a + b).ToString();
                    if (modifier > 0)
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatRise").WithVolume(.8f));
                        target.statModifiedUp = true;
                    }
                    else
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatFall").WithVolume(.8f));
                        target.statModifiedDown = true;
                    }
                }
                else
                {
                    pokemon.CustomData.Add("SpDefModifier", modifier.ToString());
                    if (modifier > 0)
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatRise").WithVolume(.8f));
                        target.statModifiedUp = true;
                    }
                    else
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatFall").WithVolume(.8f));
                        target.statModifiedDown = true;
                    }
                }

                if (int.Parse(pokemon.CustomData["SpDefModifier"]) > 6)
                {
                    pokemon.CustomData["SpDefModifier"] = "6";
                }

                if (int.Parse(pokemon.CustomData["SpDefModifier"]) < -6)
                {
                    pokemon.CustomData["SpDefModifier"] = "-6";
                }

                text.Args = new object[]
                {
                    pokemon.PokemonName,
                    statname,
                    adjustment
                };
                return text;
            }

            if (stat == GetStat.Speed)
            {
                statname = "Speed";
                if (pokemon.CustomData.ContainsKey("SpeedModifier"))
                {
                    if (int.Parse(pokemon.CustomData["SpeedModifier"]) == 6 && modifier > 0)
                    {
                        pokemon.CustomData["SpeedModifier"] = "6";
                        adjustment = "won't go higher!";
                        text.Args = new object[]
                        {
                            pokemon.PokemonName,
                            statname,
                            adjustment
                        };
                        return text; // Cant go any higher!
                    }
                    else if (int.Parse(pokemon.CustomData["SpeedModifier"]) == -6 && modifier < 0)
                    {
                        pokemon.CustomData["SpeedModifier"] = "-6";
                        adjustment = "won't go lower!";
                        text.Args = new object[]
                        {
                            pokemon.PokemonName,
                            statname,
                            adjustment
                        };
                        return text; // Cant go any lower!
                    }
                }

                if (pokemon.CustomData.ContainsKey("SpeedModifier"))
                {
                    int a = int.Parse(pokemon.CustomData["SpeedModifier"]);
                    int b = modifier;
                    pokemon.CustomData["SpeedModifier"] = (a + b).ToString();
                    if (modifier > 0)
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatRise").WithVolume(.8f));
                        target.statModifiedUp = true;
                    }
                    else
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatFall").WithVolume(.8f));
                        target.statModifiedDown = true;
                    }
                }
                else
                {
                    pokemon.CustomData.Add("SpeedModifier", modifier.ToString());
                    if (modifier > 0)
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatRise").WithVolume(.8f));
                        target.statModifiedUp = true;
                    }
                    else
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatFall").WithVolume(.8f));
                        target.statModifiedDown = true;
                    }
                }

                if (int.Parse(pokemon.CustomData["SpeedModifier"]) > 6)
                {
                    pokemon.CustomData["SpeedModifier"] = "6";
                }

                if (int.Parse(pokemon.CustomData["SpeedModifier"]) < -6)
                {
                    pokemon.CustomData["SpeedModifier"] = "-6";
                }

                text.Args = new object[]
                {
                    pokemon.PokemonName,
                    statname,
                    adjustment
                };
                return text;
            }

            // Pseudo-statistic
            if (stat == GetStat.CritRatio)
            {
                if (opponent) text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("moves.modifyStatText", "{0} is getting pumped!")));
                else text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("moves.modifyStatText", "The wild {0} is getting pumped!")));

                Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/StatRise").WithVolume(.8f));

                if (pokemon.CustomData.ContainsKey("CritRatioModifier"))
                {
                    if (int.Parse(pokemon.CustomData["CritRatioModifier"]) + modifier > 5) // Going past maximum
                    {
                        if (opponent) text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("moves.modifyStatText", "{0} is overflowing with energy!")));
                        else text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("moves.modifyStatText", "The wild {0} is overflowing with energy!")));
                        text.Args = new object[]
                        {
                            pokemon.PokemonName
                        };
                        target.gettingPumped = true;
                        return text;
                    }
                    pokemon.CustomData["CritRatioModifier"] = (int.Parse(pokemon.CustomData["CritRatioModifier"]) + modifier).ToString();
                    text.Args = new object[]
                    {
                        pokemon.PokemonName
                    };
                    target.gettingPumped = true;
                    return text;
                }
                else
                {
                    pokemon.CustomData.Add("CritRatioModifier", modifier.ToString());
                    text.Args = new object[]
                    {
                        pokemon.PokemonName
                    };
                    target.gettingPumped = true;
                    return text;
                }
            }

            return text;
        }

        // https://bulbapedia.bulbagarden.net/wiki/Critical_hit#Probability
        public int GetCritChance(int stage)
        {
            int m = 1;
            if (HighCritRatio) m++; // Increase stage by 1 if move has high critical strike ratio
            if (stage == 0) return 16 / m; // 6.25%
            if (stage == 1) return 8 / m; // 12.5%
            if (stage == 2) return 2 / m; // 50%
            if (stage >= 3) return 1 / m; // 100%
            return 16;
        }
        public enum GetStat
        {
            HP,
            Attack,
            Defense,
            SpAtk,
            SpDef,
            Speed,
            CritRatio // pseudo-statistic
        }
    }
}
