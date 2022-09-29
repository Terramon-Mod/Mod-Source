using Microsoft.Xna.Framework;
using Razorwing.Framework.Localisation;
using Terramon.Players;
using Terraria;
using Terraria.Utilities;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Converters;
using Terraria.ModLoader;
using System.Text;
using Terraria.ID;
using static Terramon.Pokemon.Moves.DamageMove;

namespace Terramon.Pokemon.Moves
{
    public abstract class BaseMove
    {
        public abstract string MoveName { get; }
        public abstract string MoveDescription { get; }
        public virtual PokemonType MoveType { get; } = PokemonType.Normal;
        public abstract Target Target { get; }
        public virtual int Cooldown { get; } = 5 * 60; //5 seconds by default
        public virtual int Speed { get; } = 80;
        public virtual int MaxPP { get; } = 20;
        public virtual int MaxBoostPP { get; } = 20;
        public virtual int Priority => 0;
        public virtual bool HighCritRatio => false;

        public static UnifiedRandom _mrand;
        public static int _seed;

        public bool moveDone = false;
        public bool TurnAnimation { get; set; } = false;

        public static MoveDb[] moveDb; // Loaded once on Mod load

        /// <summary>
        /// Used for turn based battle animations
        /// </summary>
        public int AnimationFrame { get; set; }

        /// <summary>
        /// This text will be printed when turn animation ends
        /// </summary>
        public ILocalisedBindableString PostTextLoc { get; set; }
        /// <summary>
        /// Shortcut for text printing
        /// </summary>
        public string PostText => PostTextLoc?.Value;

        /// <summary>
        /// Weight of current move when <see cref="TerramonPlayer.AutoUse"/> enabled.
        /// The more weight the more chance to be used.
        /// </summary>
        public virtual int AutoUseWeight(ParentPokemon mon, Vector2 target, TerramonPlayer player) => 10;

        //If method bellow return false -> move action not cast if Perform failed, or ended if Update
        public virtual bool PerformInWorld(ParentPokemon mon, Vector2 target, TerramonPlayer player)
        {
            return false;
        }

        /// <summary>
        /// Called when casted in turn based battle
        /// </summary>
        /// <param name="mon">Attacker projectile</param>
        /// <param name="target">Defender projectile</param>
        /// <param name="player">Player class. CAN BE NULL for Wild pokemons, so or check for null or use ? operator like this: <code>player?.ActivePet</code></param>
        /// <param name="attacker">Attacker mon data like HP, Type, etc</param>
        /// <param name="deffender">Defender mon data</param>
        /// <returns>return true if move was performed</returns>
        public virtual bool PerformInBattle(ParentPokemon mon, ParentPokemon target,
            TerramonPlayer player, PokemonData attacker, PokemonData deffender, BaseMove move)
        {
            return false;
        }

        /// <summary>
        /// Overrides pokemon projectile AI. Can be called both from battle and from world.
        /// Use <see cref="TurnAnimation"/> to see if this called from turn battle
        /// </summary>
        /// <param name="mon">Projectile</param>
        /// <param name="player">Caster class. Always check is player active (<code>player.player.active</code>)
        /// bc for wild mons it will be invalid</param>
        /// <returns>Returning true disables default projectile AI</returns>
        public virtual bool OverrideAI(ParentPokemon mon, TerramonPlayer player)
        {
            return false;
        }

        /// <summary>
        /// Called when turn based animation is requested. Called 120 times synced with updates calls.
        /// So rn in perfect condition turn animation takes const 3 seconds.
        /// </summary>
        /// <param name="mon">Attacker projectile</param>
        /// <param name="target">Defender projectile</param>
        /// <param name="player">Player class. CAN BE NULL for Wild pokemons, so or check for null or use ? operator like this: <code>player?.ActivePet</code></param>
        /// <param name="attacker">Attacker mon data like HP, Type, etc</param>
        /// <param name="deffender">Defender mon data</param>
        /// <returns>Return true if animation continues</returns>
        public virtual bool AnimateTurn(ParentPokemon mon, ParentPokemon target,
            TerramonPlayer player, PokemonData attacker, PokemonData deffender, BattleState state, bool opponent)
        {
            return false;
        }

        #region BattleV2

        /// <summary>
        /// Called at first time move executed in battle.
        /// Basically here you want to write defaults to <see cref="PokemonData.CustomData"/>
        /// </summary>
        /// <param name="atacker">Pokemon what use move and it's trainer (it not wild)</param>
        /// <param name="deffender">Opponent active pokemon and it's trainer (if not wild)</param>
        /// <param name="battle">Current Battle instance</param>
        /// <returns>true if move can be executed</returns>
        public virtual bool PerformInBattle(BattleOpponent atacker, BattleOpponent deffender, BattleModeV2 battle)
        {
            return false;
        }

        /// <summary>
        /// Called each battle update call and used to animate moves
        /// At moment of dealing damage to enemy call <see cref="BattleModeV2.DealDamage"/>
        /// It automatically will pause animation frame increase
        /// And starts UI animation (text)
        /// </summary>
        /// <param name="atacker">Pokemon what use move and it's trainer (it not wild)</param>
        /// <param name="deffender">Opponent active pokemon and it's trainer (if not wild)</param>
        /// <param name="battle">Current Battle instance</param>
        /// <param name="frame">Animation frame (starts from 0, now fully manipulated inside BattleMode)</param>
        /// <param name="skipBtnPressed">Is skip button pressed this frame.
        /// Allow skipping animation fragments</param>
        /// <returns>If true returned this method will be called next frame</returns>
        public virtual bool AnimateTurn(BattleOpponent atacker, BattleOpponent deffender, BattleModeV2 battle, int frame, bool skipBtnPressed = false)
        {
            return false;
        }

        #endregion

        public virtual void CheckIfAffects(ParentPokemon target, PokemonData deffender, BattleState state, bool opponent)
        {

        }

        /// <summary>
        /// First called when move was casted. If returned true this continue calls each update
        /// until false was returned.
        /// </summary>
        /// <param name="mon"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool Update(ParentPokemon mon, TerramonPlayer player)
        {
            return false;
        }

        public static int NewProjectile(Vector2 position, Vector2 velocity, int type)
        {
            var id = Projectile.NewProjectile(position, velocity, type, 0, 0);
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int id2 = Main.maxProjectiles; id2 > 0; id2++)
                {
                    if (!Main.projectile[id2].active)
                    {
                        Main.projectile[id2] = Main.projectile[id];
                        Main.projectile[id] = new Projectile
                        {
                            active = false,
                        };
                    }
                }
            }

            return id;
        }

        /// <summary>
        /// Return nearest enemy NPC around point (Length of circle is 400 pixels)
        /// </summary>
        public static NPC GetNearestNPC(int x, int y)
        {
            return GetNearestNPC(new Vector2(x, y));
        }

        /// <summary>
        /// Return nearest enemy NPC around point (Length of circle is 400 pixels)
        /// </summary>
        public static NPC GetNearestNPC(Vector2 pos)
        {
            int closest = -1;
            float lenght = float.MaxValue, buf;
            for (int i = 0; i < Main.maxNPCs; i++)
                if (Main.npc[i].active && !Main.npc[i].friendly && !(Main.npc[i].modNPC is ParentPokemonNPC))
                {
                    buf = (pos - Main.npc[i].position).LengthSquared();
                    if (buf < lenght)
                    {
                        closest = i;
                        lenght = buf;
                    }
                }

            if (closest == -1 || (pos - Main.npc[closest].position).LengthSquared() > 60000)//400^2
                return null;

            return Main.npc[closest];
        }

        /// <summary>
        /// Return nearest wild pokemon NPC around point
        /// </summary>
        public static NPC GetNearestPokemon(Vector2 pos)
        {
            int closest = -1;
            float lenght = float.MaxValue, buf;
            for (int i = 0; i < Main.maxNPCs; i++)
                if (Main.npc[i].active && Main.npc[i].modNPC is ParentPokemonNPC)
                {
                    buf = (pos - Main.npc[i].position).LengthSquared();
                    if (buf < lenght)
                    {
                        closest = i;
                        lenght = buf;
                    }
                }

            if (closest == -1 || (pos - Main.npc[closest].position).LengthSquared() > 60000)//400^2
                return null;

            return Main.npc[closest];
        }

        /// <summary>
        /// Return nearest player around point, excluding caller
        /// </summary>
        public static Player GetNearestPlayer(Vector2 pos, Player caller)
        {
            int closest = -1;
            float lenght = float.MaxValue, buf;
            for (int i = 0; i < Main.maxNPCs; i++)
                if (Main.player[i].active && Main.player[i] != caller)
                {
                    buf = (pos - Main.player[i].position).LengthSquared();
                    if (buf < lenght)
                    {
                        closest = i;
                        lenght = buf;
                    }
                }

            if (closest == -1 || (pos - Main.player[closest].position).LengthSquared() > 60000)//400^2
                return null;

            return Main.player[closest];
        }

        /// <summary>
        /// Call this from AnimateTurn after the animation is finished
        /// </summary>
        public void EndMove()
        {
            // obsolete
        }

        // Get Movedb class if needed
        public static void LoadMoveDb()
        {
            byte[] bytes = ModContent.GetInstance<TerramonMod>().GetFileBytes("Pokemon/Moves/MoveDB/mvdb.json");
            string json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            moveDb = JsonConvert.DeserializeObject<MoveDb[]>(json);
        }

        public static string PokemonNameFromId(int id)
        {
            id--;
            MoveDb[] database = moveDb;
            if (id >= 151)
            {
                return "NOEXIST";
            }
            return database[id].Name;
        }
        public static int PokemonIdFromName(string name)
        {
            string match = name;
            MoveDb[] database = moveDb;
            int iteration = 0;
            foreach (MoveDb pokemon in database) // Loop through MoveDb
            {
                iteration++;
                if (pokemon.Name == match)
                {
                    return iteration;
                }
            }
            return -1;
        }

        /// <summary>
        /// Get a full dictionary of all moves the specified Pokemon can learn via level up.
        /// </summary>
        /// <param name="t">The ParentPokemon type to get the learnset of</param>
        /// <returns>A dictionary of all learnable moves</returns>
        public virtual Dictionary<string, long>[] Learnset(ParentPokemon t)
        {
            string match = t.Name;
            MoveDb[] database = moveDb;
            foreach (MoveDb pokemon in database) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return pokemon.LearnAtLevel;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the BaseMove learned at the specified level. Don't use to get moves for a level 1, instead use BaseMove.DefaultMoves().
        /// </summary>
        /// <param name="t">The ParentPokemon type</param>
        /// <param name="targetlevel">The query level to search the database for</param>
        /// <returns></returns>

        public static BaseMove LearnAt(ParentPokemon t, int targetlevel)
        {
            string match = t.Name;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    foreach (Dictionary<string, long> move in pokemon.LearnAtLevel)
                    {
                        foreach (string key in move.Keys)
                        {
                            if (move[key] == targetlevel)
                            {
                                BaseMove type = TerramonMod.GetMove(key);
                                if (type == null) return null;
                                return type;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a string array of the move names that this Pokemon learns at level 1. Use TerramonMod.GetMove(string) to get BaseMove instances from the returned move names.
        /// </summary>
        /// <param name="t">The ParentPokemon type</param>
        /// <returns></returns>

        public static string[] DefaultMoves(ParentPokemon t)
        {
            string match = t.Name;
            int c = 0;
            string[] defaults = new string[] { "None", "None", "None", "None" };
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    foreach (Dictionary<string, long> move in pokemon.LearnAtLevel)
                    {
                        foreach (string key in move.Keys)
                        {
                            if (move[key] == 1)
                            {
                                defaults[c] = key;
                                c++;
                            }
                        }
                    }
                    c = 0;
                    return defaults;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a string array of the moves that the specified Pokemon would know at the specified level. Make sure to check for null, because this can return arrays with varying length.
        /// </summary>
        /// <param name="t">The ParentPokemon type</param>
        /// <param name="targetlevel">Level to index</param>
        /// <returns></returns>

        public static string[] AIMoveset(ParentPokemon t, int targetlevel)
        {
            string match = t.Name;
            string[] set = new string[] { "None", "None", "None", "None" };
            var lvls = new List<int>();
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    foreach (Dictionary<string, long> move in pokemon.LearnAtLevel)
                    {
                        foreach (string key in move.Keys)
                        {
                            lvls.Add((int)move[key]); // Add every target move level to list
                        }
                    }

                    lvls.Sort(); // eg. 1, 3, 5, 10, 18
                    lvls.Reverse(); // eg. 18, 10, 5, 3, 1

                    var goodlvls = new List<int>();
                    var goodlvlnames = new List<string>();

                    foreach (int l in lvls)
                    {
                        if (l <= targetlevel) goodlvls.Add(l);
                    }

                    foreach (int level in goodlvls)
                    {
                        foreach (Dictionary<string, long> move in pokemon.LearnAtLevel)
                        {
                            foreach (string key in move.Keys)
                            {
                                if (move[key] == level) goodlvlnames.Add(key);
                            }
                        }
                    }

                    return goodlvlnames.GetRange(0, goodlvls.Count).ToArray();
                }
            }
            return null;
        }

        public static int GetBaseExperienceYield(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.BaseExp;
                }
            }
            return 64; // Default value if for some reason none found
        }

        public static int GetBaseHP(PokemonData p)
        {
            if (moveDb == null) return 10;
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.BaseHP;
                }
            }
            return 45; // Default value if for some reason none found
        }
        public static int GetBaseAttack(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.BaseAttack;
                }
            }
            return 49; // Default value if for some reason none found
        }
        public static int GetBaseDefense(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.BaseDefense;
                }
            }
            return 49; // Default value if for some reason none found
        }
        public static int GetBaseSpAtk(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.BaseSpAtk;
                }
            }
            return 65; // Default value if for some reason none found
        }
        public static int GetBaseSpDef(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.BaseSpDef;
                }
            }
            return 65; // Default value if for some reason none found
        }
        public static int GetBaseSpeed(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.BaseSpeed;
                }
            }
            return 45; // Default value if for some reason none found
        }

        public static int EVYieldTotal(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)(pokemon.EVYieldHP + pokemon.EVYieldAttack + pokemon.EVYieldDefense + pokemon.EVYieldSpAtk + pokemon.EVYieldSpDef + pokemon.EVYieldSpeed);
                }
            }
            return 1; // Default value if for some reason none found
        }
        public static int EVYieldHP(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.EVYieldHP;
                }
            }
            return 1; // Default value if for some reason none found
        }
        public static int EVYieldAttack(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.EVYieldAttack;
                }
            }
            return 1; // Default value if for some reason none found
        }
        public static int EVYieldDefense(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.EVYieldDefense;
                }
            }
            return 1; // Default value if for some reason none found
        }
        public static int EVYieldSpAtk(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.EVYieldSpAtk;
                }
            }
            return 1; // Default value if for some reason none found
        }
        public static int EVYieldSpDef(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.EVYieldSpDef;
                }
            }
            return 1; // Default value if for some reason none found
        }
        public static int EVYieldSpeed(PokemonData p)
        {
            string match = p.PokemonName;
            foreach (MoveDb pokemon in moveDb) // Loop through MoveDb
            {
                if (pokemon.Name == match)
                {
                    return (int)pokemon.EVYieldSpeed;
                }
            }
            return 1; // Default value if for some reason none found
        }
    }

    public partial class MoveDb
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("baseExp")]
        public long BaseExp { get; set; }

        [JsonProperty("baseHp")]
        public long BaseHP { get; set; }

        [JsonProperty("baseAtk")]
        public long BaseAttack { get; set; }

        [JsonProperty("baseDef")]
        public long BaseDefense { get; set; }

        [JsonProperty("baseSpAtk")]
        public long BaseSpAtk { get; set; }

        [JsonProperty("baseSpDef")]
        public long BaseSpDef { get; set; }

        [JsonProperty("baseSpeed")]
        public long BaseSpeed { get; set; }

        [JsonProperty("evYieldHp")]
        public long EVYieldHP { get; set; }

        [JsonProperty("evYieldAtk")]
        public long EVYieldAttack { get; set; }

        [JsonProperty("evYieldDef")]
        public long EVYieldDefense { get; set; }

        [JsonProperty("evYieldSpAtk")]
        public long EVYieldSpAtk { get; set; }

        [JsonProperty("evYieldSpDef")]
        public long EVYieldSpDef { get; set; }

        [JsonProperty("evYieldSpeed")]
        public long EVYieldSpeed { get; set; }

        [JsonProperty("learnAtLevel")]
        public Dictionary<string, long>[] LearnAtLevel { get; set; }
    }

    public enum Target
    {
        Self,
        Opponent,
        Party,
        OpponentParty,
        Trainer
    }
}