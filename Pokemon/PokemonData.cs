using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Razorwing.Framework.Extensions;
using Razorwing.Framework.Localisation;
using Steamworks;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Pokemon.Moves;
using Terramon.Pokemon.Utilities;
using Terraria;
using Terraria.ModLoader.IO;
using static Terramon.Pokemon.ExpGroups;
using static Terramon.Pokemon.Moves.DamageMove;

namespace Terramon.Pokemon
{
    //TODO: Make BaseCaughtClass use this class instead fields
    //TODO: bc we migrate to this class to work with data

    /// <summary>
    /// Class what simplify access to pokemon <see cref="PokeballCaught"/>'s <see cref="TagCompound"/> data
    /// </summary>
    public class PokemonData : ITagLoadable
    {

        internal bool needUpdate = false;
        internal byte pokeballType;
        private int level;
        private int exp;
        private int expToNext;
        private int maxHp;
        private int hp;
        public string pokemon;

        private ILocalisedBindableString localised;
        public string PokemonName
        {
            get
            {
                var l = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(Pokemon));
                return l?.Value ?? Pokemon;
            }
        }

        public string Pokemon
        {
            get => pokemon;
            set
            {
                if (pokemon == value)
                    return;

                pokemon = value;
                localised = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(pokemon));
                var mon = TerramonMod.GetPokemon(Pokemon);
                Types = mon.PokemonTypes;
                ExperienceGroup = mon.ExpGroup;
                ExpToNext = EXPToNextYield(Level + 1, ExperienceGroup);
            }
        }

        public bool IsShiny { get; set; }
        public BaseMove[] Moves { get; set; }
        public int[] MovesPP { get; set; }
        public PokemonType[] Types { get; private set; }// Auto filled
        public ExpGroup ExperienceGroup { get; private set; }// Auto filled
        public bool Fainted { get; set; }//TODO: Add saving HP, MaxHP and Fainted to pokeball TagCompound.

        /// <summary>
        /// Used to store some local data what don't be saved
        /// </summary>
        public Dictionary<string, string> CustomData { get; set; } = new Dictionary<string, string>();

        public int Level
        {
            get => level;
            set
            {
                if (value == level)
                    return;
                level = value;
                needUpdate = true;
            }
        }

        public int Exp
        {
            get => exp;
            set
            {
                if (exp == value)
                    return;
                exp = value;
                while (exp >= expToNext)
                {
                    Level += 1;
                    int leftover = exp - expToNext;
                    expToNext = ExpLookupTable.ToNextLevel(Level, ExperienceGroup);
                    exp = leftover;
                }
                needUpdate = true;
            }
        }
        public int ExpToNext
        {
            get => expToNext;
            set
            {
                if (expToNext == value)
                    return;
                expToNext = value;
                needUpdate = true;
            }
        }

        public int MaxHP
        {
            get => TotalStatValue(GetStat.HP);
            set
            {
                if (maxHp == value)
                    return;

                maxHp = value;
                needUpdate = true;
            }
        }

        public int HP
        {
            get => hp;
            set
            {
                if (hp == value)
                    return;

                hp = value > MaxHP ? MaxHP : value;
                if (hp <= 0)
                    Fainted = true;
                needUpdate = true;

            }
        }

        /// <summary>
        /// Health (HP)
        /// </summary>
        public int MaxHPIV { get; set; } = 0;
        public int MaxHPEV { get; set; } = 0;

        /// <summary>
        /// Physical damage 
        /// </summary>
        public int PhysDmg
        {
            get => TotalStatValue(GetStat.Attack);
            set
            {
                return;
            }
        }
        public int PhysDmgIV { get; set; } = 0;
        public int PhysDmgEV { get; set; } = 0;
        /// <summary>
        /// Physical defense
        /// </summary>
        public int PhysDef
        {
            get => TotalStatValue(GetStat.Defense);
            set
            {
                return;
            }
        }
        public int PhysDefIV { get; set; } = 0;
        public int PhysDefEV { get; set; } = 0;
        /// <summary>
        /// Special damage
        /// </summary>
        public int SpDmg { get => TotalStatValue(GetStat.SpAtk); set { return; } }
        public int SpDmgIV { get; set; } = 0;
        public int SpDmgEV { get; set; } = 0;
        /// <summary>
        /// Special defense
        /// </summary>
        public int SpDef { get => TotalStatValue(GetStat.SpDef); set { return; } }
        public int SpDefIV { get; set; } = 0;
        public int SpDefEV { get; set; } = 0;
        /// <summary>
        /// Speed
        /// </summary>
        public int Speed { get => TotalStatValue(GetStat.Speed); set { return; } }
        public int SpeedIV { get; set; } = 0;
        public int SpeedEV { get; set; } = 0;

        public int TotalStatValue(GetStat stat)
        {
            if (stat == GetStat.HP)
            {
                float hpStat = 0;
                float a = (float)Math.Floor((float)MaxHPEV / 4f); // Add EVs division here later
                float b = (float)Math.Floor(2 * (float)BaseMove.GetBaseHP(this) + MaxHPIV + a);
                float c = (float)Math.Floor(b * Level / 100);
                hpStat = c + Level + 10;
                return (int)hpStat;
            }

            float otherStat = 0;
            if (stat == GetStat.Attack)
            {
                float a = (float)Math.Floor((float)PhysDmgEV / 4f); // Add EVs division here later
                float b = (float)Math.Floor(2 * (float)BaseMove.GetBaseAttack(this) + PhysDmgIV + a);
                float c = (float)Math.Floor(b * Level / 100);
                float d = c + 5;
                float e = (float)Math.Floor(d * 1f); // Add nature here later
                otherStat = e;
            }
            if (stat == GetStat.Defense)
            {
                float a = (float)Math.Floor((float)PhysDefEV / 4f); // Add EVs division here later
                float b = (float)Math.Floor(2 * (float)BaseMove.GetBaseDefense(this) + PhysDefIV + a);
                float c = (float)Math.Floor(b * Level / 100);
                float d = c + 5;
                float e = (float)Math.Floor(d * 1f); // Add nature here later
                otherStat = e;
            }
            if (stat == GetStat.SpAtk)
            {
                float a = (float)Math.Floor((float)SpDmgEV / 4f); // Add EVs division here later
                float b = (float)Math.Floor(2 * (float)BaseMove.GetBaseSpAtk(this) + SpDmgIV + a);
                float c = (float)Math.Floor(b * Level / 100);
                float d = c + 5;
                float e = (float)Math.Floor(d * 1f); // Add nature here later
                otherStat = e;
            }
            if (stat == GetStat.SpDef)
            {
                float a = (float)Math.Floor((float)SpDefEV / 4f); // Add EVs division here later
                float b = (float)Math.Floor(2 * (float)BaseMove.GetBaseSpDef(this) + SpDefIV + a);
                float c = (float)Math.Floor(b * Level / 100);
                float d = c + 5;
                float e = (float)Math.Floor(d * 1f); // Add nature here later
                otherStat = e;
            }
            if (stat == GetStat.Speed)
            {
                float a = (float)Math.Floor((float)SpeedEV / 4f); // Add EVs division here later
                float b = (float)Math.Floor(2 * (float)BaseMove.GetBaseSpeed(this) + SpeedIV + a);
                float c = (float)Math.Floor(b * Level / 100);
                float d = c + 5;
                float e = (float)Math.Floor(d * 1f); // Add nature here later
                otherStat = e;
            }
            return (int)otherStat;
        }

        /// <summary>
        /// Increase mon HP by <see cref="amout"/> and return actually healed value;
        /// </summary>
        /// <param name="delta">How many HP need to refill</param>
        /// <returns>Applied healing (same as <see cref="delta"/> if <see cref="MaxHP"/> not reached)</returns>
        public int Heal(int delta)
        {
            if (hp + delta > MaxHP)
            {
                var d = MaxHP - HP;
                HP = MaxHP;
                return d;
            }

            HP += delta;
            return delta;
        }

        /// <summary>
        /// Decrease mon HP by <see cref="amout"/> and return actually damage value;
        /// </summary>
        /// <param name="delta">How many HP need to decrease</param>
        /// <returns>Applied damage</returns>
        [Obsolete]
        public int Damage(int delta, PokemonType damageType)
        {
            foreach (PokemonType it in Types)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator // Disable this warning bc we have precise values
                if (it.GetResist(damageType) == 1f)
                    continue;
                delta = (int)(delta * it.GetResist(damageType));
            }
            if (hp - delta < 0)
            {
                var d = HP;
                HP = 0;//This makes mon automatically fainted
                return d;
            }

            HP -= delta;
            return delta;
        }

        public int Damage(int delta)
        {
            if (hp - delta < 0)
            {
                var d = HP;
                HP = 0; //This makes mon automatically fainted
                return d;
            }

            // Impossible to deal 0 damage
            if (delta == 0) delta = 1;

            HP -= delta;
            return delta;
        }

        public int GiveEXP(PokemonData pokemon, PokemonData opponent, BattleState state, int participated)
        {
            float a; // *a* is equal to 1 if the fainted Pokémon is wild, or 1.5 if the fainted Pokémon is owned by a Trainer
            int b = BaseMove.GetBaseExperienceYield(opponent); // *b* is the base experience yield of the fainted Pokémon's species
            int e = 1; // *e* is equal to 1.5 if the winning Pokémon is holding a Lucky Egg, or 1 otherwise (UNIMPLEMENTED)
            int f = 1; // *f* is equal to 1.2 if the Pokémon has an Affection of two hearts or more (UNIMPLEMENTED)
            int l = opponent.Level; // *l* is the level of the fainted/caught Pokémon
            int lp = pokemon.Level; // *lp* is the level of the victorious Pokémon
            int p = 1; // *p* is equal to 1 if no Exp. Point Power (Pass Power, O-Power, Rotom Power) is active (UNIMPLEMENTED)
            int s = participated; // *s* is equal to the number of Pokémon that participated in the battle and have not fainted
            int t = 1; // *t* is equal to 1 if the winning Pokémon's current owner is its Original Trainer, always 1 since no trading is implemented yet
            int v = 1; // *v* is equal to 1.2 if the winning Pokémon is at or past the level where it would be able to evolve, but it has not, 1 otherwise

            int gain;

            if (state == BattleState.BattleWithWild) a = 1;
            else a = 1.5f;

            gain = (int)(a * t * b * e * l * p * f * v) / (7 * s);

            pokemon.Exp += gain;

            // Give out effort value points.
            if (!ReachedEVMax())
            {
                GainEVs(opponent);
            }

            return gain;
        }

        public void GainEVs(PokemonData opponent)
        {
            if (EVTotal() + BaseMove.EVYieldHP(opponent) > 510) MaxHPEV += EVTotal() + BaseMove.EVYieldHP(opponent) - 510;
            else MaxHPEV += BaseMove.EVYieldHP(opponent);
            if (MaxHPEV > 252) MaxHPEV = 252;
            if (ReachedEVMax()) return;
            if (EVTotal() + BaseMove.EVYieldAttack(opponent) > 510) PhysDmgEV += EVTotal() + BaseMove.EVYieldAttack(opponent) - 510;
            else PhysDmgEV += BaseMove.EVYieldAttack(opponent);
            if (PhysDmgEV > 252) PhysDmgEV = 252;
            if (ReachedEVMax()) return;
            if (EVTotal() + BaseMove.EVYieldDefense(opponent) > 510) PhysDefEV += EVTotal() + BaseMove.EVYieldDefense(opponent) - 510;
            else PhysDefEV += BaseMove.EVYieldDefense(opponent);
            if (PhysDefEV > 252) PhysDefEV = 252;
            if (ReachedEVMax()) return;
            if (EVTotal() + BaseMove.EVYieldSpAtk(opponent) > 510) SpDmgEV += EVTotal() + BaseMove.EVYieldSpAtk(opponent) - 510;
            else SpDmgEV += BaseMove.EVYieldSpAtk(opponent);
            if (SpDmgEV > 252) SpDmgEV = 252;
            if (ReachedEVMax()) return;
            if (EVTotal() + BaseMove.EVYieldSpDef(opponent) > 510) SpDefEV += EVTotal() + BaseMove.EVYieldSpDef(opponent) - 510;
            else SpDefEV += BaseMove.EVYieldSpDef(opponent);
            if (SpDefEV > 252) SpDefEV = 252;
            if (ReachedEVMax()) return;
            if (EVTotal() + BaseMove.EVYieldSpeed(opponent) > 510) SpeedEV += EVTotal() + BaseMove.EVYieldSpeed(opponent) - 510;
            else SpeedEV += BaseMove.EVYieldSpeed(opponent);
            if (SpeedEV > 252) SpeedEV = 252;
        }

        public int EVTotal() { return MaxHPEV + PhysDmgEV + PhysDefEV + SpDmgEV + SpDefEV + SpeedEV; }
        public bool ReachedEVMax()
        {
            if (EVTotal() >= 510) return true;
            return false;
        }

        public int EXPToNextYield(int level, ExpGroup obs, string name = "")
        {
            ParentPokemon mon;
            if (name == "") mon = TerramonMod.GetPokemon(Pokemon);
            else mon = TerramonMod.GetPokemon(name);

            level -= 1;

            return ExpLookupTable.ToNextLevel(level, mon.ExpGroup);
        }

        public int GenerateIVs()
        {
            return Main.rand?.Next(1, 32) ?? 0; // IVs range from 0-31
        }

        public PokemonData()
        {
            Moves = new BaseMove[] { null, null, null, null };
            Types = new[] { PokemonType.Normal };
            ExperienceGroup = ExpGroup.MediumFast;
            MaxHP = 45 + Main.rand?.Next(20) ?? 0;
            HP = 45 + Main.rand?.Next(20) ?? 0;

            MaxHPIV = GenerateIVs();
            PhysDmgIV = GenerateIVs();
            PhysDefIV = GenerateIVs();
            SpDmgIV = GenerateIVs();
            SpDefIV = GenerateIVs();
            SpeedIV = GenerateIVs();

            Level = 1 + Main.rand?.Next(8) ?? 0;
            ExpToNext = 0;
            Fainted = false;
        }

        public PokemonData(TagCompound tag)
        {
            Load(tag);
        }

        public PokemonData(BaseCaughtClass tag)
        {
            IsShiny = tag.isShiny;
            //v2
            pokemon = tag.CapturedPokemon;
            var mon = TerramonMod.GetPokemon(Pokemon);
            Types = mon.PokemonTypes;
            ExperienceGroup = mon.ExpGroup;

            level = tag.Level;//Assign to field here to avoid leveling up
            ExpToNext = EXPToNextYield(level + 1, ExperienceGroup);
            exp = tag.Exp;
            //expToNext = tag.ExpToNext;

            if (Moves == null)
                Moves = new BaseMove[4];
            Moves[0] = tag.Moves[0];
            Moves[1] = tag.Moves[1];
            Moves[2] = tag.Moves[2];
            Moves[3] = tag.Moves[3];


            Fainted = tag.PokeData.Fainted;
            MaxHP = tag.PokeData.MaxHP;
            HP = tag.PokeData.HP;
            PhysDmg = tag.PokeData.PhysDmg;
            PhysDef = tag.PokeData.PhysDef;
            SpDmg = tag.PokeData.SpDmg;
            SpDef = tag.PokeData.SpDef;

            MaxHPIV = GenerateIVs();
            PhysDmgIV = GenerateIVs();
            PhysDefIV = GenerateIVs();
            SpDmgIV = GenerateIVs();
            SpDefIV = GenerateIVs();
            SpeedIV = GenerateIVs();

            //Update all old pokebals
            bool retrofit = true;
            foreach (var it in Moves)
                if (it != null)
                    retrofit = false;
            if (retrofit)
            {
                var def = TerramonMod.GetPokemon(Pokemon).DefaultMove;
                try //In case someone forgot leave nulls at empty moves 
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Moves[i] = TerramonMod.GetMove(def[i]);
                    }
                }
                catch (Exception e)
                {
                    TerramonMod.Instance.Logger.Error($"Looks like someone from dev team don't fill moves field properly for {Pokemon} pokemon.\n" +
                                                      $"Please, report this to Terramon Team!");
                    TerramonMod.Instance.Logger.Error(e);
                }
            }

            pokeballType = (byte)TerramonMod.PokeballFactory.GetEnum(tag);
        }


        public static implicit operator TagCompound(PokemonData tag)
        {
            if (tag == null)
                return null;
            return new TagCompound()
            {
                [nameof(BaseCaughtClass.PokemonName)] = tag.Pokemon,
                //[nameof(SmallSpritePath)] = SmallSpritePath,
                //
                //[nameof(SmallSpritePath)] = SmallSpritePath, // what do i do here
                //v2

                [nameof(BaseCaughtClass.isShiny)] = tag.IsShiny,
                [nameof(BaseCaughtClass.CapturedPokemon)] = tag.Pokemon,
                [nameof(BaseCaughtClass.Level)] = tag.Level,
                [nameof(BaseCaughtClass.Exp)] = tag.Exp,

                [nameof(MaxHPIV)] = tag.MaxHPIV,
                [nameof(HP)] = tag.HP,
                [nameof(PhysDmgIV)] = tag.PhysDmgIV,
                [nameof(PhysDefIV)] = tag.PhysDefIV,
                [nameof(SpDmgIV)] = tag.SpDmgIV,
                [nameof(SpDefIV)] = tag.SpDefIV,
                [nameof(SpeedIV)] = tag.SpeedIV,
                [nameof(PhysDmgEV)] = tag.PhysDmgEV,
                [nameof(PhysDefEV)] = tag.PhysDefEV,
                [nameof(SpDmgEV)] = tag.SpDmgEV,
                [nameof(SpDefEV)] = tag.SpDefEV,
                [nameof(SpeedEV)] = tag.SpeedEV,

                //Store move name
                [BaseCaughtClass.MOVE1] = tag.Moves?[0]?.GetType().Name ?? "",
                [BaseCaughtClass.MOVE2] = tag.Moves?[1]?.GetType().Name ?? "",
                [BaseCaughtClass.MOVE3] = tag.Moves?[2]?.GetType().Name ?? "",
                [BaseCaughtClass.MOVE4] = tag.Moves?[3]?.GetType().Name ?? "",
                //[nameof(Moves)] = from it in Moves select it.MoveName,
                //Used to restore items in sidebarUI
                [BaseCaughtClass.POKEBAL_PROPERTY] = tag.pokeballType
            };
        }

        public static explicit operator PokemonData(TagCompound tag)
        {
            return new PokemonData(tag);
        }

        public void Load(TagCompound tag)
        {
            IsShiny = tag.GetBool(nameof(BaseCaughtClass.isShiny));
            //v2
            pokemon = tag.ContainsKey(nameof(BaseCaughtClass.CapturedPokemon))
                ? tag.GetString(nameof(BaseCaughtClass.CapturedPokemon))
                : tag.GetString(nameof(BaseCaughtClass.PokemonName));

            if (!string.IsNullOrEmpty(pokemon))
            {
                var mon = TerramonMod.GetPokemon(Pokemon);
                Types = mon.PokemonTypes;
                ExperienceGroup = mon.ExpGroup;

                level = tag.ContainsKey(nameof(Level)) ? tag.GetInt(nameof(Level)) : 1;
                ExpToNext = EXPToNextYield(Level + 1, ExperienceGroup);
                exp = tag.ContainsKey(nameof(Exp)) ? tag.GetInt(nameof(Exp)) : 0;
                ExpToNext = EXPToNextYield(level + 1, ExperienceGroup);

                //expToNext = tag.ContainsKey(nameof(ExpToNext)) ? tag.GetInt(nameof(ExpToNext)) : 0;

                //Average values from bulbasaur
                MaxHPIV = tag.ContainsKey(nameof(MaxHPIV)) ? tag.GetInt(nameof(MaxHPIV)) : GenerateIVs();
                HP = tag.ContainsKey(nameof(HP)) ? tag.GetInt(nameof(HP)) : 5;
                Fainted = tag.ContainsKey(nameof(Fainted)) && tag.GetBool(nameof(Fainted));//ReSharper changes
                PhysDefIV = tag.ContainsKey(nameof(PhysDefIV)) ? tag.GetInt(nameof(PhysDefIV)) : GenerateIVs();
                PhysDmgIV = tag.ContainsKey(nameof(PhysDmgIV)) ? tag.GetInt(nameof(PhysDmgIV)) : GenerateIVs();
                SpDefIV = tag.ContainsKey(nameof(SpDefIV)) ? tag.GetInt(nameof(SpDefIV)) : GenerateIVs();
                SpDmgIV = tag.ContainsKey(nameof(SpDmgIV)) ? tag.GetInt(nameof(SpDmgIV)) : GenerateIVs();
                SpeedIV = tag.ContainsKey(nameof(SpeedIV)) ? tag.GetInt(nameof(SpeedIV)) : GenerateIVs();
                MaxHPEV = tag.ContainsKey(nameof(MaxHPEV)) ? tag.GetInt(nameof(MaxHPEV)) : 0;
                PhysDefEV = tag.ContainsKey(nameof(PhysDefEV)) ? tag.GetInt(nameof(PhysDefEV)) : 0;
                PhysDmgEV = tag.ContainsKey(nameof(PhysDmgEV)) ? tag.GetInt(nameof(PhysDmgEV)) : 0;
                SpDefEV = tag.ContainsKey(nameof(SpDefEV)) ? tag.GetInt(nameof(SpDefEV)) : 0;
                SpDmgEV = tag.ContainsKey(nameof(SpDmgEV)) ? tag.GetInt(nameof(SpDmgEV)) : 0;
                SpeedEV = tag.ContainsKey(nameof(SpeedEV)) ? tag.GetInt(nameof(SpeedEV)) : 0;

                if (Moves == null)
                    Moves = new BaseMove[4];
                Moves[0] = tag.ContainsKey(BaseCaughtClass.MOVE1) ? TerramonMod.GetMove(tag.GetString(BaseCaughtClass.MOVE1)) : null;
                Moves[1] = tag.ContainsKey(BaseCaughtClass.MOVE2) ? TerramonMod.GetMove(tag.GetString(BaseCaughtClass.MOVE2)) : null;
                Moves[2] = tag.ContainsKey(BaseCaughtClass.MOVE3) ? TerramonMod.GetMove(tag.GetString(BaseCaughtClass.MOVE3)) : null;
                Moves[3] = tag.ContainsKey(BaseCaughtClass.MOVE4) ? TerramonMod.GetMove(tag.GetString(BaseCaughtClass.MOVE4)) : null;

                //Update all old pokebals
                bool retrofit = true;
                foreach (var it in Moves)
                    if (it != null)
                        retrofit = false;
                if (retrofit)
                {
                    var def = TerramonMod.GetPokemon(Pokemon).DefaultMove;
                    try //In case someone forgot leave nulls at empty moves 
                    {
                        for (int i = 0; i < 4; i++)
                        {

                            Moves[i] = TerramonMod.GetMove(def[i]);

                        }
                    }
                    catch (Exception e)
                    {
                        TerramonMod.Instance.Logger.Error($"Looks like someone from dev team don't fill moves field properly for {Pokemon} pokemon.\n" +
                                                          $"Please, report this to Terramon Team!");
                        TerramonMod.Instance.Logger.Error(e);
                    }
                }

                pokeballType = tag.GetByte(BaseCaughtClass.POKEBAL_PROPERTY);
            }
        }

        public TagCompound GetCompound() => new TagCompound()
        {
            [nameof(BaseCaughtClass.PokemonName)] = this.Pokemon,
            //[nameof(SmallSpritePath)] = SmallSpritePath,
            //
            //[nameof(SmallSpritePath)] = SmallSpritePath, // what do i do here
            //v2

            [nameof(BaseCaughtClass.isShiny)] = this.IsShiny,
            [nameof(BaseCaughtClass.CapturedPokemon)] = this.Pokemon,
            [nameof(BaseCaughtClass.Level)] = this.Level,
            [nameof(BaseCaughtClass.Exp)] = this.Exp,

            [nameof(MaxHPIV)] = this.MaxHPIV,
            [nameof(HP)] = this.HP,
            [nameof(PhysDmgIV)] = this.PhysDmgIV,
            [nameof(PhysDefIV)] = this.PhysDefIV,
            [nameof(SpDmgIV)] = this.SpDmgIV,
            [nameof(SpDefIV)] = this.SpDefIV,
            [nameof(SpeedIV)] = this.SpeedIV,
            [nameof(MaxHPEV)] = this.MaxHPEV,
            [nameof(PhysDmgEV)] = this.PhysDmgEV,
            [nameof(PhysDefEV)] = this.PhysDefEV,
            [nameof(SpDmgEV)] = this.SpDmgEV,
            [nameof(SpDefEV)] = this.SpDefEV,
            [nameof(SpeedEV)] = this.SpeedEV,

            //Store move name
            [BaseCaughtClass.MOVE1] = this.Moves?[0]?.GetType().Name ?? "",
            [BaseCaughtClass.MOVE2] = this.Moves?[1]?.GetType().Name ?? "",
            [BaseCaughtClass.MOVE3] = this.Moves?[2]?.GetType().Name ?? "",
            [BaseCaughtClass.MOVE4] = this.Moves?[3]?.GetType().Name ?? "",
            //[nameof(Moves)] = from it in Moves select it.MoveName,
            //Used to restore items in sidebarUI
            [BaseCaughtClass.POKEBAL_PROPERTY] = this.pokeballType
        };

        public List<KeyValuePair<BaseMove, int>> GetAvailableMoves()
        {
            var dict = new List<KeyValuePair<BaseMove, int>>();
            
            for(int i = 0; i < 4; i++)
            {
                if (MovesPP[i] > 0)
                {
                    dict.Add(new KeyValuePair<BaseMove, int>(Moves[i], MovesPP[i]));
                }
            }

            return dict;
        }

        public enum NonVolatileStatus
        {
            Burn,
            Freeze,
            Paralysis,
            Poison,
            Sleep
        }

        public enum VolatileStatus
        {
            Bound,
            NoEscape,
            Confusion,
            Curse,
            Embargo,
            Encore,
            Flinch,
            HealBlock,
            Identified,
            Infatuation,
            LeechSeed,
            Nightmare,
            PerishSong,
            Taunt,
            Telekinesis,
            Torment
        }

        public enum VolatileBattleStatus
        {
            AquaRing,
            Bracing,
            ChargingTurn,
            CenterOfAttention,
            DefenseCurl,
            Rooting,
            MagicCoat,
            Minimize,
            Protection,
            Recharging,
            SemiInvulnerableTurn,
            Substitute,
            TakingAim,
            Withdrawing
        }
    }
}
