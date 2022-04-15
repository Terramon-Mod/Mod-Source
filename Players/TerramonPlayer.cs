using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Razorwing.Framework.Graphics;
using Razorwing.Framework.Localisation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Razorwing.Framework.Extensions;
using Terramon.Items.MiscItems;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Network.Sync;
using Terramon.Network.Sync.Battle;
using Terramon.Pokemon;
using Terramon.Pokemon.FirstGeneration.Fishing;
using Terramon.Pokemon.Moves;
using Terramon.UI;
using Terramon.UI.Moveset;
using Terramon.UI.SidebarParty;
using Terramon.UI.Starter;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using System.Management;
using System.Text;
using Razorwing.RPC;
using Razorwing.RPC.Attributes;
using Terramon.Network;

// ReSharper disable ParameterHidesMember
// ReSharper disable LocalVariableHidesMember


namespace Terramon.Players
{
    public sealed partial class TerramonPlayer : ModPlayer
    {
        public List<Item> list = new List<Item>();
        public List<Item> loadList = new List<Item>();

        public BattleMode Battle = null;
        public BattleModeV2 Battlev2 = null;

        //public int deletepokecase = 0;
        public int premierBallRewardCounter;

        private Dictionary<string, bool> ActivePets = new Dictionary<string, bool>();
        private List<PokemonData> pokemonStorage = new List<PokemonData>();
        public List<PokemonData> PokemonStore => pokemonStorage;
        public int ActivePetId = -1;
        public bool ActivePetShiny;
        public string ActivePetName = string.Empty;
        public bool CombatReady;
        public bool AutoUse;
        public bool sidebarSync = false;
        private bool loading = true;

        public bool healingAtHealerBed = false;

        // This bool save/load in TagCompound and determine whether this is the first battle the player has
        public bool firstBattle = true;

        public ILocalisedBindableString pokeName = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("*")));

        public bool Attacking;

        public int ActivePartySlot
        {
            get => _activePartySlot;
            set
            {
                _activePartySlot = value;

                MoveSet = new BaseMove[] { null, null, null, null };

                PokemonData tag;
                switch (value)
                {
                    case 1:
                        tag = PartySlot1;
                        break;
                    case 2:
                        tag = PartySlot2;
                        break;
                    case 3:
                        tag = PartySlot3;
                        break;
                    case 4:
                        tag = PartySlot4;
                        break;
                    case 5:
                        tag = PartySlot5;
                        break;
                    case 6:
                        tag = PartySlot6;
                        break;
                    default:
                        return;
                }

                //var m1 = tag.ContainsKey(BaseCaughtClass.MOVE1) ? tag.GetString(BaseCaughtClass.MOVE1) : null;
                //var m2 = tag.ContainsKey(BaseCaughtClass.MOVE2) ? tag.GetString(BaseCaughtClass.MOVE2) : null;
                //var m3 = tag.ContainsKey(BaseCaughtClass.MOVE3) ? tag.GetString(BaseCaughtClass.MOVE3) : null;
                //var m4 = tag.ContainsKey(BaseCaughtClass.MOVE4) ? tag.GetString(BaseCaughtClass.MOVE4) : null;
                //MoveSet[0] = !string.IsNullOrEmpty(m1) ? TerramonMod.GetMove(m1) : null;
                //MoveSet[1] = !string.IsNullOrEmpty(m2) ? TerramonMod.GetMove(m2) : null;
                //MoveSet[2] = !string.IsNullOrEmpty(m3) ? TerramonMod.GetMove(m3) : null;
                //MoveSet[3] = !string.IsNullOrEmpty(m4) ? TerramonMod.GetMove(m4) : null;

                MoveSet = tag?.Moves;

                if (Main.netMode == NetmodeID.MultiplayerClient && Main.LocalPlayer == player && !loading)
                {
                    this.RPC(ActivePetSync,-2,ActivePartySlot,ExecutingSide.Both | ExecutingSide.DenySender);
                    //var p = new ActivePetSync();
                    //p.Send((TerramonMod)mod, this);
                }

                Battle?.HandleChange();
            }
        }

        [RPCCallable]
        public void ActivePetSync(int id, int slotId)
        {
            ActivePartySlot = slotId;
            if(id != -2)
                ActivePetId = id;
            if (Battle != null)
            {
                Battle.awaitSync = false;
                Battle.HandleChange();
            }
        }

        public PokemonData ActivePet
        {
            get
            {
                switch (ActivePartySlot)
                {
                    case 1:
                        return PartySlot1;
                    case 2:
                        return PartySlot2;
                    case 3:
                        return PartySlot3;
                    case 4:
                        return PartySlot4;
                    case 5:
                        return PartySlot5;
                    case 6:
                        return PartySlot6;
                    default:
                        return null;
                }
            }
        }


        private int _activePartySlot = -1;
        public BaseMove[] MoveSet;
        public int Cooldown;

        public int pkBallsThrown = 0;
        public int greatBallsThrown = 0;
        public int ultraBallsThrown = 0;
        public int pkmnCaught = 0;

        public int Language = 1;
        public int ItemNameColors = 1;

        private PokemonData _partySlot1;
        private PokemonData _partySlot2;
        private PokemonData _partySlot3;
        private PokemonData _partySlot4;
        private PokemonData _partySlot5;
        private PokemonData _partySlot6;

        public PokemonData PartySlot1
        {
            get => _partySlot1;
            set
            {
                _partySlot1 = value;
                if (player == Main.LocalPlayer)
                {
                    if (value == null)
                    {
                        ((TerramonMod)mod).PartySlots.partyslot1.Item.TurnToAir();
                        return;
                    }

                    if (!((TerramonMod)mod).PartySlots?.partyslot1?.Item?.IsAir ?? false)
                    {
                        //We need to update data inside item
                        var modItem = ((TerramonMod)mod).PartySlots.partyslot1.Item.modItem;
                        if (modItem?.item != null && modItem.item.active) modItem.Load(value);
                    }
                    else
                    {
                        LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot1.Item, value);
                    }

                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    sidebarSync = true;
                }
            }
        }

        public PokemonData PartySlot2
        {
            get => _partySlot2;
            set
            {
                _partySlot2 = value;
                if (player == Main.LocalPlayer)
                {
                    if (value == null)
                    {
                        ((TerramonMod)mod).PartySlots.partyslot2.Item.TurnToAir();
                        return;
                    }

                    if (!((TerramonMod)mod).PartySlots?.partyslot2?.Item?.IsAir ?? false)
                    {
                        //We need to update data inside item
                        var modItem = ((TerramonMod)mod).PartySlots.partyslot2.Item.modItem;
                        if (modItem?.item != null && modItem.item.active) modItem.Load(value);
                    }
                    else
                    {
                        LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot2.Item, value);
                    }
                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    sidebarSync = true;
                }
            }
        }

        public PokemonData PartySlot3
        {
            get => _partySlot3;
            set
            {
                _partySlot3 = value;
                if (player == Main.LocalPlayer)
                {
                    if (value == null)
                    {
                        ((TerramonMod)mod).PartySlots.partyslot3.Item.TurnToAir();
                        return;
                    }

                    if (!((TerramonMod)mod).PartySlots?.partyslot3?.Item?.IsAir ?? false)
                    {
                        //We need to update data inside item
                        var modItem = ((TerramonMod)mod).PartySlots.partyslot3.Item.modItem;
                        if (modItem?.item != null && modItem.item.active) modItem.Load(value);
                    }
                    else
                    {
                        LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot3.Item, value);
                    }
                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    sidebarSync = true;
                }
            }
        }

        public PokemonData PartySlot4
        {
            get => _partySlot4;
            set
            {
                _partySlot4 = value;
                if (player == Main.LocalPlayer)
                {
                    if (value == null)
                    {
                        mod.Logger.Info("null on set, returning");
                        ((TerramonMod)mod).PartySlots.partyslot4.Item.TurnToAir();
                        return;
                    }

                    if (!((TerramonMod)mod).PartySlots?.partyslot4?.Item?.IsAir ?? false)
                    {
                        //We need to update data inside item
                        var modItem = ((TerramonMod)mod).PartySlots.partyslot4.Item.modItem;
                        if (modItem?.item != null && modItem.item.active) modItem.Load(value);
                    }
                    else
                    {
                        LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot4.Item, value);
                    }
                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    sidebarSync = true;
                }
            }
        }

        public PokemonData PartySlot5
        {
            get => _partySlot5;
            set
            {
                _partySlot5 = value;
                if (player == Main.LocalPlayer)
                {
                    if (value == null)
                    {
                        ((TerramonMod)mod).PartySlots.partyslot5.Item.TurnToAir();
                        return;
                    }

                    if (!((TerramonMod)mod).PartySlots?.partyslot5?.Item?.IsAir ?? false)
                    {
                        //We need to update data inside item
                        var modItem = ((TerramonMod)mod).PartySlots.partyslot5.Item.modItem;
                        if (modItem?.item != null && modItem.item.active) modItem.Load(value);
                    }
                    else
                    {
                        LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot5.Item, value);
                    }
                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    sidebarSync = true;
                }
            }
        }

        public PokemonData PartySlot6
        {
            get => _partySlot6;
            set
            {
                _partySlot6 = value;
                if (player == Main.LocalPlayer)
                {
                    if (value == null)
                    {
                        ((TerramonMod)mod).PartySlots.partyslot6.Item.TurnToAir();
                        return;
                    }

                    if (!((TerramonMod)mod).PartySlots?.partyslot6?.Item?.IsAir ?? false)
                    {
                        //We need to update data inside item
                        var modItem = ((TerramonMod)mod).PartySlots.partyslot6.Item.modItem;
                        if (modItem?.item != null && modItem.item.active) modItem.Load(value);
                    }
                    else
                    {
                        LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot6.Item, value);
                    }
                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    sidebarSync = true;
                }
            }
        }


        //public int firstslottype = 1;
        public string firstslotname = "*";
        //public int secondslottype = 1;
        public string secondslotname = "*";
        //public int thirdslottype = 1;
        public string thirdslotname = "*";
        //public int fourthslottype = 1;
        public string fourthslotname = "*";
        //public int fifthslottype = 1;
        public string fifthslotname = "*";
        //public int sixthslottype = 1;
        public string sixthslotname = "*";

        public int CycleIndex;


        public static TerramonPlayer Get()
        {
            return Get(Main.LocalPlayer);
        }

        public static TerramonPlayer Get(Player player)
        {
            return player.GetModPlayer<TerramonPlayer>();
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            base.Kill(damage, hitDirection, pvp, damageSource);

            ActivePetName = string.Empty;
            ActivePartySlot = -1;
        }

        public int CatchIndex { get; internal set; }


        public override void Initialize()
        {
            healingAtHealerBed = false;
            InitializePokeballs();
            //Initialise active pets bools
            // ReSharper disable once LocalVariableHidesMember
            var list = TerramonMod.GetPokemonsNames();
            ActivePets = new Dictionary<string, bool>();
            foreach (var it in list) ActivePets.Add(it, false);
        }

        public override void ResetEffects()
        {
            //Set any active pet to false
            foreach (var it in ActivePets.Keys.ToArray()) ActivePets[it] = false;
        }


        public string lastactivename;

        /// <summary>
        ///     Enable only one pet for player at once
        /// </summary>
        /// <param name="name">Pokemon type name</param>
        /// <param name="combatReady">This pokemon summoned from party UI?</param>
        public void ActivatePet(string name, bool combatReady = true)
        {
            ResetEffects();

            if (ActivePet != null)
            {
                ActivePetShiny = ActivePet.IsShiny;
            }

            //var monName = ActivePets.FirstOrDefault(x => x.Value).Value;

            if (string.IsNullOrEmpty(name) || name == "*")
            {
                ActivePetId = -1;
                ActivePetName = "";
                CombatReady = false;
                return;
            }
            else
            {
                if (lastactivename != name)
                {
                    if (pokeName.Value != name)
                    {
                        pokeName = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(name));
                    }
                    GetInstance<TerramonMod>().DisplayPokemonNameRP(pokeName.Value, ActivePetShiny);
                }
            }

            lastactivename = name;

            if (!combatReady)
                ActivePartySlot = -1;
            CombatReady = combatReady;

            if (ActivePets.ContainsKey(name))
                ActivePets[name] = true;
            else
                throw new InvalidOperationException(
                    $"Pokemon {name} not registered! Please send log files to mod devs!");
        }

        public bool IsPetActive(string name)
        {
            if (ActivePets.ContainsKey(name))
                return ActivePets[name];

            throw new InvalidOperationException($"Pokemon {name} not registered! Please send log files to mod devs!");
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            AddStartItem(ref items, ItemType<PokeballItem>(), 8);
            AddStartItem(ref items, ItemType<Pokedex>());
        }

        public static bool MyUIStateActive(Player player)
        {
            return ChooseStarter.Visible;
        }

        private void LoadPartySlot(Item modItem, TagCompound value)
        {
            //var modItem = ((TerramonMod)mod).PartySlots.partyslot1.Item.modItem;
            TerramonMod.PokeballFactory.Pokebals en = TerramonMod.PokeballFactory.Pokebals.Nothing;
            if (value != null)
                en = (TerramonMod.PokeballFactory.Pokebals)value.GetByte(BaseCaughtClass.POKEBAL_PROPERTY);
            if (en == 0)
            {
                modItem.TurnToAir();
            }
            else
            {
                modItem.SetDefaults(TerramonMod.PokeballFactory.GetPokeballType(en));
                modItem.modItem.Load(value);
            }
        }

        // Store opening sfx
        public SoundEffectInstance openingSfx;

        public override void OnEnterWorld(Player player)
        {
            healingAtHealerBed = false;

            // Call to Mod class to enable in-world Rich Presence
            GetInstance<TerramonMod>().EnterWorldRP();
            //
            Battle?.Cleanup();
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            modPlayer.Attacking = false;
            Moves.Visible = false; // Ignore for v0.3

            Mod leveledMod = ModLoader.GetMod("Leveled");
            Mod overhaulMod = ModLoader.GetMod("TerrariaOverhaul");
            if (leveledMod != null)
                Main.NewText(
                    "Terramon is not compatible with the 'Leveled' mod, which is currently enabled. To prevent mod-breaking bugs, please disable one or the other.",
                    245, 46, 24);
            if (overhaulMod != null)
                Main.NewText(
                    "Terramon is not compatible with the 'Terraria Overhaul' mod, which is currently enabled. To prevent mod-breaking bugs, please disable one or the other.",
                    245, 46, 24);

            if (StarterChosen == false)
            {
                PartySlot1 = null;
                PartySlot2 = null;
                PartySlot3 = null;
                PartySlot4 = null;
                PartySlot5 = null;
                PartySlot6 = null;
            }

            //TODO: Override sidebarUI here
            if (PartySlot1 != null) LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot1.Item, PartySlot1);
            else PartySlot1 = null;
            if (PartySlot2 != null) LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot2.Item, PartySlot2);
            else PartySlot2 = null;
            if (PartySlot3 != null) LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot3.Item, PartySlot3);
            else PartySlot3 = null;
            if (PartySlot4 != null) LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot4.Item, PartySlot4);
            else PartySlot4 = null;
            if (PartySlot5 != null) LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot5.Item, PartySlot5);
            else PartySlot5 = null;
            if (PartySlot6 != null) LoadPartySlot(((TerramonMod)mod).PartySlots.partyslot6.Item, PartySlot6);
            else PartySlot6 = null;

            //Running one update to load sidebar without requiring to open inv
            ((TerramonMod)mod).PartySlots.UpdateUI(null);

            if (StarterChosen == false)
            {
                ChooseStarter.movieFinished = false;
                GetInstance<TerramonMod>()._exampleUserInterface.SetState(new ChooseStarter());
                ChooseStarter.Visible = true;
                ChooseStarter.movieFinished = false;
                PartySlots.Visible = false;
                UISidebar.Visible = false;
                player.frozen = true;
            }
            else
            {
                if (Battle == null) UISidebar.Visible = true;
            }



            loading = false;

            if (Main.netMode == NetmodeID.MultiplayerClient)
                new RequestSyncPacket().Send();

            // Check if update is available!
#if !DEBUG
            var mod_version = Get("https://pokeparser.projectagon.repl.co/mod/ver");
            var current_version = $"v{mod.Version}";

            if (current_version != mod_version)
            {
                var msg = Get("https://pokeparser.projectagon.repl.co/update/message");
                if (msg != "Bye") Main.NewText($"[c/f3cc61:Terramon >] A new update is available to download ({mod_version}). " + msg);
                else Main.NewText($"[c/f3cc61:Terramon >] A new update is available to download ({mod_version})");
                Main.NewText($"Go the the Mod Browser to update!");
            }
#endif
        }



        public string Get(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }

        }

        //fishing for pokemon
        public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize,
            int worldLayer, int questFish, ref int caughtType, ref bool junk)
        {
            if (junk) return;
            if (liquidType == 0 && player.ZoneBeach && Main.rand.NextBool(6)) //16.7% chance from fishing
                caughtType = ItemType<MagikarpFish>();
            if (liquidType == 0 && player.ZoneBeach && Main.rand.NextBool(12)) //8.3% chance from fishing
                caughtType = Main.rand.Next(new[] { ItemType<GoldeenFish>(), ItemType<HorseaFish>(), ItemType<TentacoolFish>() });
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (TerramonMod.PartyCycle.JustPressed)
            {
                if (!player.HasBuff(mod.BuffType(firstslotname + "Buff")) && firstslotname != "*")
                {
                    player.AddBuff(mod.BuffType(firstslotname + "Buff"), 2);
                    Main.PlaySound(GetInstance<TerramonMod>()
                        .GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sendout"));
                    CombatText.NewText(player.Hitbox, Color.White, "Go! " + firstslotname + "!", true);
                }
            }
            if (TerramonMod.CompressSidebar.JustPressed)
            {
                if (GetInstance<TerramonMod>().UISidebar.compressing) return;
                GetInstance<TerramonMod>().UISidebar.isCompressed = !GetInstance<TerramonMod>().UISidebar.isCompressed;
            }
        }


        string lastmon = "";
        public override void PreUpdate()
        {
            if(Main.netMode == NetmodeID.MultiplayerClient && player == Main.LocalPlayer)
                ModContent.GetInstance<TerramonWorld>().PreUpdate();// Workaround bc World updates wont get called in clients...

            ShowItemIconForUsableItems(); // Appropriately sets item icon when holding an item that is usable by right-clicking a Pok�mon in the overworld

            var monName = ActivePets.FirstOrDefault(x => x.Value).Key;
            if (lastmon != monName)
            {
                if (string.IsNullOrEmpty(monName) || monName == "*")
                {
                    GetInstance<TerramonMod>().RemoveDisplayPokemonNameRP();
                }
            }
            lastmon = monName;
            if (StarterChosen)
            {
                if (Main.playerInventory || Battle != null)
                {
                    if (player.chest != -1 || Main.npcShop != 0 || EvolveUI.Visible)
                        PartySlots.Visible = false;
                    else
                        if (Battle == null) PartySlots.Visible = true;
                    UISidebar.Visible = false;
                }
                else
                {
                    EvolveUI.Visible = false;
                    if (Battle == null)
                    {
                        UISidebar.Visible = true;
                    }
                    PartySlots.Visible = false;
                }
            }

            if (ChooseStarter.Visible || ChooseStarterBulbasaur.Visible || ChooseStarterCharmander.Visible ||
                ChooseStarterSquirtle.Visible) ClearNPCs();

            if (!Main.dedServ)
            {
                var type = BuffType<PokemonBuff>();
                if (player.HasBuff(type) && !string.IsNullOrEmpty(ActivePetName))
                    Main.buffTexture[type] =
                        GetTexture(
                            $"Terramon/Buffs/{ActivePetName}Buff");
            }

            //Sync mon data between players
            if (Main.netMode == NetmodeID.MultiplayerClient && Main.LocalPlayer == player && sidebarSync)
            {
                var p = new PlayerSidebarSync();
                p.Send((TerramonMod)mod, this);
                sidebarSync = false;
            }

            //Note: If you compile code from VS -> moves don't have cooldowns
            if (Cooldown > 0 && ActiveMove == null)
#if DEBUG
                Cooldown = 0;
#else
                Cooldown--;
#endif

            if (Battle != null)
            {
                Battle.Update();
                if (Battle.State == BattleState.None)
                {
                    Battle.Cleanup();
                    Battle = null;
                }
            }
            //else if (Battlev2 != null)
            //{
            //    Battlev2.Update();
            //    if (Battlev2.State == BattleModeV2.BattleState.None)
            //    {
            //        Battlev2.Cleanup();
            //        Battlev2 = null;
            //    }
            //}

            //Moves logic
            if (Main.LocalPlayer == player && CombatReady && ActivePartySlot > 0 && ActivePartySlot <= 6 && ActivePetId != -1
                && Main.projectile[ActivePetId].modProjectile is ParentPokemon) //Integrity check
            { 
                if (ActiveMove != null)
                {
                    if (!ActiveMove.Update((ParentPokemon)Main.projectile[ActivePetId].modProjectile, this))
                        ActiveMove = null;
                }
                else if (Cooldown <= 0)
                {
                    var mod = (TerramonMod)this.mod;
                    if (AutoUse)
                    {
                        var f1 = MoveSet[0]?.AutoUseWeight((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this) ?? 0;
                        var f2 = MoveSet[1]?.AutoUseWeight((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this) ?? 0;
                        var f3 = MoveSet[2]?.AutoUseWeight((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this) ?? 0;
                        var f4 = MoveSet[3]?.AutoUseWeight((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this) ?? 0;
                        var sum = f1 + f2 + f3 + f4 + 1500; //1500 is idle
                        var w = Main.rand.Next(sum);
                        if (w < f1)
                        {
                            ActiveMove = MoveSet[0];
                            if (ActiveMove.PerformInWorld((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this))
                                Cooldown = ActiveMove.Cooldown;
                            else
                                ActiveMove = null;
                        }
                        else if (w < f1 + f2)
                        {
                            ActiveMove = MoveSet[1];
                            if (ActiveMove.PerformInWorld((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this))
                                Cooldown = ActiveMove.Cooldown;
                            else
                                ActiveMove = null;
                        }
                        else if (w < f1 + f2 + f3)
                        {
                            ActiveMove = MoveSet[2];
                            if (ActiveMove.PerformInWorld((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this))
                                Cooldown = ActiveMove.Cooldown;
                            else
                                ActiveMove = null;
                        }
                        else if (w < f1 + f2 + f3 + f4)
                        {
                            ActiveMove = MoveSet[3];
                            if (ActiveMove.PerformInWorld((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this))
                                Cooldown = ActiveMove.Cooldown;
                            else
                                ActiveMove = null;
                        }

                    }
                    if (mod.FirstPKMAbility.JustPressed && MoveSet[0] != null && ActiveMove == null)
                    {
                        ActiveMove = MoveSet[0];
                        if (ActiveMove.PerformInWorld((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this))
                            Cooldown = ActiveMove.Cooldown;
                        else
                            ActiveMove = null;
                    }
                    else if (mod.SecondPKMAbility.JustPressed && MoveSet[1] != null && ActiveMove == null)
                    {
                        ActiveMove = MoveSet[1];
                        if (ActiveMove.PerformInWorld((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this))
                            Cooldown = ActiveMove.Cooldown;
                        else
                            ActiveMove = null;
                    }
                    else if (mod.ThirdPKMAbility.JustPressed && MoveSet[2] != null && ActiveMove == null)
                    {
                        ActiveMove = MoveSet[2];
                        if (ActiveMove.PerformInWorld((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this))
                            Cooldown = ActiveMove.Cooldown;
                        else
                            ActiveMove = null;
                    }
                    else if (mod.FourthPKMAbility.JustPressed && MoveSet[3] != null && ActiveMove == null)
                    {
                        ActiveMove = MoveSet[3];
                        if (ActiveMove.PerformInWorld((ParentPokemon)Main.projectile[ActivePetId].modProjectile, Main.MouseWorld, this))
                            Cooldown = ActiveMove.Cooldown;
                        else
                            ActiveMove = null;
                    }

                }
            }
            else if (Main.LocalPlayer != player)
            {
                if (Battle != null)
                {
                    if (!Main.dedServ)
                        Battle.Update();
                    if (Battle.State == BattleState.None)
                    {
                        Battle.Cleanup();
                        Battle = null;
                    }
                }
            }
        }

        public void ShowItemIconForUsableItems()
        {
            if (Main.LocalPlayer.HeldItem.type == ItemType<Items.MiscItems.Medication.Potion>())
            {
                player.showItemIcon = true;
                player.showItemIcon2 = ItemType<Items.MiscItems.Medication.Potion>();
            }
            if (Main.LocalPlayer.HeldItem.type == ItemType<Items.MiscItems.Medication.SuperPotion>())
            {
                player.showItemIcon = true;
                player.showItemIcon2 = ItemType<Items.MiscItems.Medication.SuperPotion>();
            }
            if (Main.LocalPlayer.HeldItem.type == ItemType<Items.MiscItems.Medication.HyperPotion>())
            {
                player.showItemIcon = true;
                player.showItemIcon2 = ItemType<Items.MiscItems.Medication.HyperPotion>();
            }
            if (Main.LocalPlayer.HeldItem.type == ItemType<Items.MiscItems.Medication.MaxPotion>())
            {
                player.showItemIcon = true;
                player.showItemIcon2 = ItemType<Items.MiscItems.Medication.MaxPotion>();
            }
            if (Main.LocalPlayer.HeldItem.type == ItemType<Items.MiscItems.Medication.FullRestore>())
            {
                player.showItemIcon = true;
                player.showItemIcon2 = ItemType<Items.MiscItems.Medication.FullRestore>();
            }
        }

        public void RegisterToPokedex(string name)
        {
            Main.NewText(BaseMove.PokemonIdFromName(name));
            PokedexCompletion[BaseMove.PokemonIdFromName(name) - 1] = 1;
        }

        public BaseMove ActiveMove;

        public override void UpdateAutopause()
        {
            base.UpdateAutopause();
            if (StarterChosen)
            {
                if (Main.playerInventory)
                {
                    if (player.chest != -1 || Main.npcShop != 0 || EvolveUI.Visible)
                        PartySlots.Visible = false;
                    else
                        PartySlots.Visible = true;
                    UISidebar.Visible = false;
                }
                else
                {
                    EvolveUI.Visible = false;
                    UISidebar.Visible = true;
                    PartySlots.Visible = false;
                }
            }

            if (ChooseStarter.Visible || ChooseStarterBulbasaur.Visible || ChooseStarterCharmander.Visible ||
                ChooseStarterSquirtle.Visible) ClearNPCs();
        }

        public static void ClearNPCs()
        {
            for (int i = 0; i < Main.npc.Length; i++)
                if (Main.npc[i] != null && !Main.npc[i].townNPC)
                {
                    Main.npc[i].life = 0;
                    if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i);
                }
        }

        public override void ModifyScreenPosition()
        {
            if (GetInstance<TerramonMod>().battleCamera != Vector2.Zero && Battle != null)
            {
                Main.screenPosition = GetInstance<TerramonMod>().battleCamera - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            }
            base.ModifyScreenPosition();
        }

        private void AddStartItem(ref IList<Item> items, int itemType, int stack = 1)
        {
            Item item = new Item();

            item.SetDefaults(itemType);
            item.stack = stack;

            items.Add(item);
        }

        public override void PostBuyItem(NPC vendor, Item[] shop, Item item)
        {
            if (vendor.type == NPCType<PokemonTrainer>() && item.type == ItemType<PokeballItem>())
            {
                TerramonPlayer p = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                p.premierBallRewardCounter++;
                if (p.premierBallRewardCounter == 10)
                {
                    p.premierBallRewardCounter = 0;
                    player.QuickSpawnItem(ItemType<PremierBallItem>());
                }
            }
        }


        public override TagCompound Save()
        {
            PartySlots partySlots = GetInstance<TerramonMod>().PartySlots;
            if (partySlots.partyslot1.Item.IsAir)
            {
                firstslotname = "*";
                GetInstance<TerramonMod>().UISidebar.firstpkmn.TextureName = "Terramon/UI/SidebarParty/Empty";
                GetInstance<TerramonMod>().UISidebar.firstpkmn.HoverText = "";
                GetInstance<TerramonMod>().UISidebar.firstpkmn.Recalculate();
            }

            if (PartySlot2 == null)
            {
                secondslotname = "*";
                GetInstance<TerramonMod>().UISidebar.secondpkmn.TextureName = "Terramon/UI/SidebarParty/Empty";
                GetInstance<TerramonMod>().UISidebar.secondpkmn.HoverText = "";
                GetInstance<TerramonMod>().UISidebar.secondpkmn.Recalculate();
            }

            if (partySlots.partyslot3.Item.IsAir)
            {
                thirdslotname = "*";
                GetInstance<TerramonMod>().UISidebar.thirdpkmn.TextureName = "Terramon/UI/SidebarParty/Empty";
                GetInstance<TerramonMod>().UISidebar.thirdpkmn.HoverText = "";
                GetInstance<TerramonMod>().UISidebar.thirdpkmn.Recalculate();
            }

            if (partySlots.partyslot4.Item.IsAir)
            {
                fourthslotname = "*";
                GetInstance<TerramonMod>().UISidebar.fourthpkmn.TextureName = "Terramon/UI/SidebarParty/Empty";
                GetInstance<TerramonMod>().UISidebar.fourthpkmn.HoverText = "";
                GetInstance<TerramonMod>().UISidebar.fourthpkmn.Recalculate();
            }

            if (partySlots.partyslot5.Item.IsAir)
            {
                fifthslotname = "*";
                GetInstance<TerramonMod>().UISidebar.fifthpkmn.TextureName = "Terramon/UI/SidebarParty/Empty";
                GetInstance<TerramonMod>().UISidebar.fifthpkmn.HoverText = "";
                GetInstance<TerramonMod>().UISidebar.fifthpkmn.Recalculate();
            }

            if (partySlots.partyslot6.Item.IsAir)
            {
                sixthslotname = "*";
                GetInstance<TerramonMod>().UISidebar.sixthpkmn.TextureName = "Terramon/UI/SidebarParty/Empty";
                GetInstance<TerramonMod>().UISidebar.sixthpkmn.HoverText = "";
                GetInstance<TerramonMod>().UISidebar.sixthpkmn.Recalculate();
            }

            TagCompound tag = new TagCompound
            {
                [nameof(StarterChosen)] = StarterChosen,
                [nameof(firstBattle)] = firstBattle,
                [nameof(PokedexCompletion)] = PokedexCompletion
            };

            if (PartySlot1 != null && PartySlot1.pokeballType != 0)
                tag.Add(nameof(PartySlot1), PartySlot1.GetCompound());
            if (PartySlot2 != null && PartySlot2.pokeballType != 0)
                tag.Add(nameof(PartySlot2), PartySlot2.GetCompound());
            if (PartySlot3 != null && PartySlot3.pokeballType != 0)
                tag.Add(nameof(PartySlot3), PartySlot3.GetCompound());
            if (PartySlot4 != null && PartySlot4.pokeballType != 0)
                tag.Add(nameof(PartySlot4), PartySlot4.GetCompound());
            if (PartySlot5 != null && PartySlot5.pokeballType != 0)
                tag.Add(nameof(PartySlot5), PartySlot5.GetCompound());
            if (PartySlot6 != null && PartySlot6.pokeballType != 0)
                tag.Add(nameof(PartySlot6), PartySlot6.GetCompound());

            SavePokeballs(tag);

            tag.Add(nameof(PokemonStore), PokemonStore.SaveToTag());

            return tag;
        }

        public override void Load(TagCompound tag)
        {
            loading = true;
            Battle?.Cleanup();
            Battle = null;
            StarterChosen = tag.GetBool(nameof(StarterChosen));
            firstBattle = tag.GetBool(nameof(firstBattle));
            PokedexCompletion = tag.GetIntArray(nameof(PokedexCompletion));
            if (tag.ContainsKey(nameof(PartySlot1)))
                PartySlot1 = tag.ContainsKey(nameof(PartySlot1))
                    ? new PokemonData(tag.GetCompound(nameof(PartySlot1)))
                    : null;
            else
                PartySlot1 = null;
            if (tag.ContainsKey(nameof(PartySlot2)))
                PartySlot2 = tag.ContainsKey(nameof(PartySlot2)) ? new PokemonData(tag.GetCompound(nameof(PartySlot2))) : null;
            else
                PartySlot2 = null;
            if (tag.ContainsKey(nameof(PartySlot3)))
                PartySlot3 = tag.ContainsKey(nameof(PartySlot3)) ? new PokemonData(tag.GetCompound(nameof(PartySlot3))) : null;
            else
                PartySlot3 = null;
            if (tag.ContainsKey(nameof(PartySlot4)))
                PartySlot4 = tag.ContainsKey(nameof(PartySlot4)) ? new PokemonData(tag.GetCompound(nameof(PartySlot4))) : null;
            else
                PartySlot4 = null;
            if (tag.ContainsKey(nameof(PartySlot5)))
                PartySlot5 = tag.ContainsKey(nameof(PartySlot5)) ? new PokemonData(tag.GetCompound(nameof(PartySlot5))) : null;
            else
                PartySlot5 = null;
            if (tag.ContainsKey(nameof(PartySlot6)))
                PartySlot6 = tag.ContainsKey(nameof(PartySlot6)) ? new PokemonData(tag.GetCompound(nameof(PartySlot6))) : null;
            else
                PartySlot6 = null;

            // Fix spaces in slots
            FormatSlots();

            LoadPokeballs(tag);

            if (tag.ContainsKey(nameof(PokemonStore)))
                pokemonStorage = tag.GetCompound(nameof(PokemonStore)).LoadFromTag<PokemonData>().ToList();

            loading = false;
            sidebarSync = false;
        }


        /// <summary>
        /// Add PokeData to storage (PC)
        /// </summary>
        /// <param name="tag">PokeData to store in storage</param>
        /// <param name="maxPages">Maximum boxes count</param>
        /// <param name="pageSize">One box size</param>
        /// <returns>Return true if actually added. Returns false if out of space</returns>
        public bool AddToStorage(PokemonData tag, int maxPages = 8, int pageSize = 30)
        {
            if (pokemonStorage.Count < maxPages * pageSize)
            {
                pokemonStorage.Add(tag);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove PokeData from PC by its instance
        /// </summary>
        /// <param name="tag">Instance what should be deleted</param>
        /// <returns>Return true if actually remove. Return false if storage don't contain provided instance</returns>
        public bool RemoveFromStorage(PokemonData tag)
        {
            if (pokemonStorage.Contains(tag))
            {
                pokemonStorage.Remove(tag);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove PokeData from PC by its id
        /// </summary>
        /// <param name="id">List ID</param>
        /// <returns>Return true if actually remove. Return false if storage don't contain provided instance</returns>
        public bool RemoveFromStorage(int id)
        {
            if (id < pokemonStorage.Count)
            {
                pokemonStorage.RemoveAt(id);
                return true;
            }
            return false;
        }

        public PokemonData[] GetPage(int page, int pageSize = 30)
        {
            var arr = new PokemonData[pageSize];
            int offset = page * pageSize;
            for (int i = 0; i < pageSize; i++)
            {
                if (offset + i < pokemonStorage.Count)
                {
                    arr[i] = pokemonStorage[offset + i];
                }
                else
                {
                    arr[i] = null;
                }
            }

            return arr;
        }



        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (newPlayer && !Main.dedServ)
            {
                var p = new PlayerSidebarSync();
                p.Send((TerramonMod)mod, this, toWho);
            }
            base.SyncPlayer(toWho, fromWho, newPlayer);
        }

        // Prevent player movement in battle.
        public override void PreUpdateMovement()
        {
            if (Battle != null)
            {
                if (Battle.ForceDirection != 0)
                {
                    player.direction = Battle.ForceDirection;
                }
                else
                {
                    player.direction = Battle.WildNPC.projectile.position.X > player.position.X ? 1 : -1;
                }
                player.velocity.X = 0;
                if (player.mount.Active)
                {
                    player.velocity.Y = 0;
                }
                else
                {
                    player.velocity.Y = 5f;
                }
            }
        }

        public bool StarterChosen { get; set; }

        // For example PokedexCompletion[0] would be '1' if Bulbasaur has been caught by this player, elsewise 0.
        // Saved and loaded via player tag data.
        public int[] PokedexCompletion = new int[151];

        //public bool StarterPackageBought { get; } //Unused

        public int whoAmI => player.whoAmI;
    }
}