using DiscordRPC;
using DiscordRPC.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.Configuration;
using Razorwing.Framework.IO.Stores;
using Razorwing.Framework.Localisation;
using Razorwing.Framework.Threading;
using Razorwing.Framework.Timing;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Network;
using Terramon.Network.Catching;
using Terramon.Network.Starter;
using Terramon.Pokemon;
using Terramon.Pokemon.Moves;
using Terramon.Razorwing.Framework.IO.Stores;
using Terramon.UI;
using Terramon.UI.Moveset;
using Terramon.UI.SidebarParty;
using Terramon.UI.Starter;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terramon.Players;
using Terramon.UI.Test;
using Terraria.Utilities;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;

namespace Terramon
{
    public class TerramonMod : Mod
    {
        // NEW DISCORD RICH PRESENCE INTEGRATION
        public DiscordRpcClient client;
        //

        internal ChooseStarter ChooseStarter;
        internal ChooseStarterBulbasaur ChooseStarterBulbasaur;
        internal ChooseStarterCharmander ChooseStarterCharmander;
        internal ChooseStarterSquirtle ChooseStarterSquirtle;

        internal AnimatorUI summaryUI;
        public UserInterface summaryUIInterface;

        public static bool PartyUITheme = true;
        public static bool PartyUIAutoMode = false;
        public static bool PartyUIReverseAutoMode = false;
        public static bool ShowHelpButton = false;
        public static bool HelpButtonInitialize = true;
        public int PartyUIThemeChanged = 0;

        // UI SIDEBAR //
        internal UISidebar UISidebar;
        internal Moves Moves;
        public PartySlots PartySlots { get; private set; }

        public static ModHotKey CompressSidebar;

        // UI SIDEBAR //

        internal PokegearUI PokegearUI;
        internal PokegearUIEvents PokegearUIEvents;
        internal EvolveUI evolveUI;
        public UserInterface _exampleUserInterface; // Choose Starter
        private UserInterface _exampleUserInterfaceNew; // Pokegear Main Menu
        private UserInterface PokegearUserInterfaceNew;
        private UserInterface evolveUserInterfaceNew; // Pokegear Events Menu
        private UserInterface _uiSidebar;
        private UserInterface _moves;
        private UserInterface _battle;
        public UserInterface _partySlots;

        public static ModHotKey PartyCycle;
        public static LocalisationManager Localisation;
        public static ResourceStore<byte[]> Store;
        public static Texture2DStore Textures;
        public Storage storage;
        public Scheduler Scheduler;
        private GameTimeClock schedulerClock;

        public GameTimeClock GameClock => schedulerClock;
        //evolution

        // some battling stuff
        public static Animator ZoomAnimator;
        public Vector2 battleCamera;

        public TerramonMod()
        {
            Instance = this;
            Localisation = new LocalisationManager(locale);
            Store = new ResourceStore<byte[]>(new EmbeddedStore());
            Localisation.AddLanguage(GameCulture.English.Name, new LocalisationStore(Store, GameCulture.English));
        }

        private static readonly string[] balls =
        {
            "Pokeball",
            "GreatBall",
            "UltraBall",
            "DuskBall",
            "PremierBall",
            "QuickBall",
            "TimerBall",
            "MasterBall",
            "ZeroBall",
            "PumpkinBall"
        };

      // catch chance of the ball refers to the same index as the ball
        private static readonly float[][] catchChances =
        {
            new[] {.1190f}, //Pokeball
            new[] {.1785f}, //Great Ball
            new[] {.2380f}, //Ultra Ball
            new[]
            {
                .2380f, //Dusk Ball
                .1190f
            },
            new[] {.1190f}, //Premier Ball
            new[] {.2380f}, //Quick Ball
            new[] {.2380f}, //Timer Ball
            new[] {1f}, //Master Ball
            new[] {.1190f}, //Zero Ball
            new[] {.3f} //Shadow Ball
        };

        public static string[] GetBallProjectiles()
        {
            string[] ballProjectiles = new string[balls.Length];
            for (int i = 0; i < balls.Length; i++) ballProjectiles[i] = balls[i] + "Projectile";

            return ballProjectiles;
        }

        Timestamps timestamp;

        public virtual void EnterWorldRP()
        {
#if DEBUG
            return;
#endif
            timestamp = Timestamps.Now;
                client.SetPresence(new RichPresence()
                {
                    Details = "In-Game",
                    State = "Playing v0.4.2",
                    Assets = new Assets()
                    {
                        LargeImageKey = "largeimage2",
                        LargeImageText = "Terramon Mod",
                        SmallImageKey = "pokeball",
                        SmallImageText = "No Pokémon Selected"
                    },
                    Timestamps = timestamp
                });
        }

        public virtual void DisplayPokemonNameRP(string name, bool shinyness)
        {
#if DEBUG
            return;   
#endif
            if (shinyness)
            {
                name = name += " ✨";
            }
            client?.SetPresence(new RichPresence()
            {
                Details = "In-Game",
                State = "Playing v0.4.2",
                Assets = new Assets()
                {
                    LargeImageKey = "largeimage2",
                    LargeImageText = "Terramon Mod",
                    SmallImageKey = "pokeball",
                    SmallImageText = "Using " + name
                },
                Timestamps = timestamp
            });
        }

        public virtual void RemoveDisplayPokemonNameRP()
        {
#if DEBUG
            return;
#endif
            client?.SetPresence(new RichPresence()
                {
                    Details = "In-Game",
                    State = "Playing v0.4.2",
                    Assets = new Assets()
                    {
                        LargeImageKey = "largeimage2",
                        LargeImageText = "Terramon Mod",
                        SmallImageKey = "pokeball",
                        SmallImageText = "No Pokémon Selected"
                    },
                    Timestamps = timestamp
                });
        }

        public override void PreSaveAndQuit()
        {
            TerramonPlayer p = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            p.openingSfx?.Stop();
            client?.SetPresence(new RichPresence()
                {
                    Details = "In Menu",
                    State = "Playing v0.4.2",
                    Assets = new Assets()
                    {
                        LargeImageKey = "largeimage2",
                        LargeImageText = "Terramon Mod"
                    }
                });
        }
		
        protected DllResourceStore man;
        protected Bindable<string> locale = new Bindable<string>(Language.ActiveCulture.Name);

        public override void Load()
        {
            // Load movedb

            BaseMove.LoadMoveDb();

            // Initalize Discord RP on Mod Load
            if (!Main.dedServ)
            {
                client = new DiscordRpcClient("790364236554829824");
                client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
                //

                Logger.Info("Attempting to establish Discord RP connection");

                // Subscribe to events
                client.OnReady += (sender, e) =>
                {
                    Logger.Info("Established connection");
                    Console.WriteLine("Received Ready from user {0}", e.User.Username);
                };

                client.OnPresenceUpdate += (sender, e) =>
                {
                    Console.WriteLine("Received Update! {0}", e.Presence);
                };

                client.OnError += (sender, e) =>
                {
                    Logger.Error("Could not start Discord RP. Reason: " + e.Message);
                };

                //Connect to the RPC
                client.Initialize();

                client.SetPresence(new RichPresence()
                {
                    Details = "In Menu",
                    State = "Playing v0.4.2",
                    Assets = new Assets()
                    {
                        LargeImageKey = "largeimage2",
                        LargeImageText = "Terramon Mod"
                    }
                });
            }

            BaseMove._mrand = new UnifiedRandom(BaseMove._seed = new Random().Next());
            //Load all mons to a store
            LoadPokemons();

            if (Main.netMode != NetmodeID.Server)
            {

                if (Localisation == null)
                {
                    Localisation = new LocalisationManager(locale);
                }
                locale = new Bindable<string>(Language.ActiveCulture.Name);

                storage = new ModStorage("Terramon");//Initialise local resource store
                Store = new ResourceStore<byte[]>(new EmbeddedStore());//Make new instance of ResourceStore with dependency what loads data from ModStore
                Store.AddStore(new StorageCachableStore(storage, new WebStore()));//Add second dependency what checks if data exist in local store.
                                                                                  //If not and provided URL load data from web and save it on drive
                Textures = new Texture2DStore(Store);//Initialise cachable texture store in order not creating new texture each call

                Localisation.AddLanguage(GameCulture.English.Name, new LocalisationStore(Store, GameCulture.English));//Adds en-US.lang file handling
                Localisation.AddLanguage(GameCulture.Russian.Name, new LocalisationStore(Store, GameCulture.Russian));//Adds ru-RU.lang file handling
#if DEBUG
                UseWebAssets = true;
                var ss = Localisation.GetLocalisedString(new LocalisedString(("title","Powered by broken code")));//It's terrible checking in ui from phone, so i can ensure everything works from version string
                //Main.versionNumber = ss.Value + "\n" + Main.versionNumber;
#endif
                Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/ShockwaveEffect")); // The path to the compiled shader file.
                Ref<Effect> whiteShaderRef = new Ref<Effect>(GetEffect("Effects/whiteshader")); // The path to the compiled shader file.
                Ref<Effect> battleIntroRef = new Ref<Effect>(GetEffect("Effects/BattleIntro")); // The path to the compiled shader file.
                Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave"].Load();
                Filters.Scene["BattleIntro"] = new Filter(new ScreenShaderData(battleIntroRef, "Shade"), EffectPriority.VeryHigh);
                Filters.Scene["BattleIntro"].Load();
                GameShaders.Misc["WhiteTint"] = new MiscShaderData(whiteShaderRef, "ArmorBasic");

                ChooseStarter = new ChooseStarter();
                ChooseStarter.Activate();
                ChooseStarterBulbasaur = new ChooseStarterBulbasaur();
                ChooseStarterBulbasaur.Activate();
                ChooseStarterCharmander = new ChooseStarterCharmander();
                ChooseStarterCharmander.Activate();
                ChooseStarterSquirtle = new ChooseStarterSquirtle();
                ChooseStarterSquirtle.Activate();
                PokegearUI = new PokegearUI();
                PokegearUI.Activate();
                PokegearUIEvents = new PokegearUIEvents();
                PokegearUIEvents.Activate();
                evolveUI = new EvolveUI();
                evolveUI.Activate();
                UISidebar = new UISidebar();
                UISidebar.Activate();
                Moves = new Moves();
                Moves.Activate();
                PartySlots = new PartySlots();
                PartySlots.Activate();
                _exampleUserInterface = new UserInterface();
                _exampleUserInterfaceNew = new UserInterface();
                PokegearUserInterfaceNew = new UserInterface();
                evolveUserInterfaceNew = new UserInterface();
                _uiSidebar = new UserInterface();
                _moves = new UserInterface();
                _partySlots = new UserInterface();
                _battle = new UserInterface();
                ParentPokemonNPC.HighlightTexture = new Dictionary<string, Texture2D>();
                ParentPokemon.HighlightTexture = new Dictionary<string, Texture2D>();

                //_exampleUserInterface.SetState(ChooseStarter); // Choose Starter
#if DEBUG
                _exampleUserInterface.SetState(new TestState());
#endif
                _exampleUserInterfaceNew.SetState(PokegearUI); // Pokegear Main Menu
                PokegearUserInterfaceNew.SetState(PokegearUIEvents); // Pokegear Events Menu
                evolveUserInterfaceNew.SetState(evolveUI);
                _uiSidebar.SetState(UISidebar);
                _moves.SetState(Moves);
                _partySlots.SetState(PartySlots);
                _battle.SetState(BattleMode.UI = new BattleUI());// Automatically assign shortcut

                summaryUI = new AnimatorUI();
                summaryUI.Activate();

                summaryUIInterface = new UserInterface();
                summaryUIInterface.SetState(summaryUI);
            }


            if (Main.dedServ)
                return;

            Scheduler = new Scheduler(Thread.CurrentThread, schedulerClock = new GameTimeClock());

            FirstPKMAbility = RegisterHotKey("First Pokémon Move", Keys.Z.ToString());
            SecondPKMAbility = RegisterHotKey("Second Pokémon Move", Keys.X.ToString());
            ThirdPKMAbility = RegisterHotKey("Third Pokémon Move", Keys.C.ToString());
            FourthPKMAbility = RegisterHotKey("Fourth Pokémon Move", Keys.V.ToString());

            CompressSidebar = RegisterHotKey("Compress Sidebar", Keys.RightShift.ToString());

            PartyCycle = RegisterHotKey("Quick Spawn First Party Pokémon", Keys.RightAlt.ToString());
        }

        public override void Unload()
        {
            client?.Dispose();
            client = null;
            Instance = null;
            _exampleUserInterface?.SetState(null); // Choose Starter
            _exampleUserInterfaceNew?.SetState(null); // Pokegear Main Menu
            PokegearUserInterfaceNew?.SetState(null); // Pokegear Events Menu
            evolveUserInterfaceNew?.SetState(null);
            summaryUIInterface?.SetState(null);
            _uiSidebar?.SetState(null);
            _partySlots?.SetState(null);
            _moves?.SetState(null);
            _battle?.SetState(null);
            BattleMode.UI = null;
            PartySlots = null;
            pokemonStore = null;
            wildPokemonStore = null;
            movesStore = null;
            _exampleUserInterface = null;
            _exampleUserInterfaceNew = null;
            PokegearUserInterfaceNew = null;
            _uiSidebar = null;
            _partySlots = null;
            _moves = null;
            _battle = null;
            BaseMove._mrand = null;


            ChooseStarter.Deactivate();
            ChooseStarter = null;
            ChooseStarterBulbasaur.Deactivate();
            ChooseStarterBulbasaur = null;
            ChooseStarterCharmander.Deactivate();
            ChooseStarterCharmander = null;
            ChooseStarterSquirtle.Deactivate();
            ChooseStarterSquirtle = null;
            PokegearUI.Deactivate();
            PokegearUI = null;
            PokegearUIEvents.Deactivate();
            PokegearUIEvents = null;
            evolveUI.Deactivate();
            evolveUI = null;
            UISidebar.Deactivate();
            UISidebar = null;
            Moves.Deactivate();
            Moves = null;

            summaryUI.Deactivate();
            summaryUI = null;

            PartyCycle = null;
            FirstPKMAbility = null;
            SecondPKMAbility = null;
            ThirdPKMAbility = null;
            FourthPKMAbility = null;
            CompressSidebar = null;

            Localisation = null;
            Textures = null;
            storage = null;
            Store.Dispose();
            Store = null;
            Scheduler?.CancelDelayedTasks();
            Scheduler = null;
            schedulerClock = null;
            ParentPokemonNPC.HighlightTexture = null;
            ParentPokemon.HighlightTexture = null;
        }

        //ModContent.GetInstance<TerramonMod>(). (grab instance)


        public static float[][] GetCatchChances()
        {
            return catchChances;
        }

        /* public override void Load()
		{
			Main.music[MusicID.OverworldDay] = GetMusic("Sounds/Music/overworldnew");
			Main.music[MusicID.Night] = GetMusic("Sounds/Music/nighttime");
			Main.music[MusicID.AltOverworldDay] = GetMusic("Sounds/Music/overworldnew");
		} */

        // UI STUFF DOWN HERE

        public override void UpdateUI(GameTime gameTime)
        {
            //Update scheduler clock time for transform sequences
            schedulerClock.UpdateTime(gameTime);
            ZoomAnimator.Update(gameTime);

            if (ChooseStarter.Visible) _exampleUserInterface?.Update(gameTime);
            if (PokegearUI.Visible) _exampleUserInterfaceNew?.Update(gameTime);
            if (PokegearUIEvents.Visible) PokegearUserInterfaceNew?.Update(gameTime);
            if (EvolveUI.Visible) evolveUserInterfaceNew?.Update(gameTime);
            //starters
            if (ChooseStarterBulbasaur.Visible) _exampleUserInterface?.Update(gameTime);
            if (ChooseStarterCharmander.Visible) _exampleUserInterface?.Update(gameTime);
            if (ChooseStarterSquirtle.Visible) _exampleUserInterface?.Update(gameTime);
            if (UISidebar.Visible) _uiSidebar?.Update(gameTime);
            if (Moves.Visible) _moves?.Update(gameTime);
            if (PartySlots.Visible && !BattleUI.Visible) _partySlots?.Update(gameTime);
            if (BattleUI.Visible) _battle.Update(gameTime);
            if (AnimatorUI.Visible) summaryUI.Update(gameTime);
#if DEBUG
            if (TestState.Visible) _exampleUserInterface?.Update(gameTime);
#endif
            Scheduler.Update();//Update all transform sequences after updates
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            //int StarterSelectionLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 1")); //Unused var
            if (mouseTextIndex != -1)
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Terramon: Pokemon Interfaces",
                    delegate
                    {
                        if (ChooseStarter.Visible) _exampleUserInterface.Draw(Main.spriteBatch, GameClock?.GameTime);
                        if (PokegearUI.Visible) _exampleUserInterfaceNew.Draw(Main.spriteBatch, GameClock?.GameTime);
                        if (PokegearUIEvents.Visible) PokegearUserInterfaceNew.Draw(Main.spriteBatch, GameClock?.GameTime);
                        if (EvolveUI.Visible) evolveUserInterfaceNew.Draw(Main.spriteBatch, GameClock?.GameTime);
                        if (ChooseStarterBulbasaur.Visible)
                            _exampleUserInterface.Draw(Main.spriteBatch, GameClock?.GameTime);
                        if (ChooseStarterCharmander.Visible)
                            _exampleUserInterface.Draw(Main.spriteBatch, GameClock?.GameTime);
                        if (ChooseStarterSquirtle.Visible) _exampleUserInterface.Draw(Main.spriteBatch, GameClock?.GameTime);
                        if (UISidebar.Visible) _uiSidebar.Draw(Main.spriteBatch, GameClock?.GameTime);
                        if (Moves.Visible) _moves.Draw(Main.spriteBatch, new GameTime());
                        if (PartySlots.Visible && !BattleUI.Visible) _partySlots.Draw(Main.spriteBatch, GameClock?.GameTime);
                        if (BattleUI.Visible) _battle.Draw(Main.spriteBatch, GameClock?.GameTime);
                        if (AnimatorUI.Visible) summaryUIInterface.Draw(Main.spriteBatch, GameClock?.GameTime);

#if DEBUG
                        if (TestState.Visible) _exampleUserInterface?.Draw(Main.spriteBatch, GameClock?.GameTime);
#endif
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
        }

        public static bool MyUIStateActive(Player player)
        {
            return ChooseStarter.Visible;
        }

        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            Transform.Zoom = new Vector2(Main.GameZoomTarget);
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active) return;

            var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            var modExpanse = ModLoader.GetMod("tmonadds");

            if (player.Battle != null && modExpanse == null)
            {
                priority = MusicPriority.BossHigh;
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Battling/wildbattle");
                return;
            }

            if (MyUIStateActive(Main.LocalPlayer) && !ChooseStarter.movieFinished)
            {
                priority = MusicPriority.BossHigh;
                music = GetSoundSlot(SoundType.Music, null);
            }

            if (player.healingAtHealerBed)
            {
                priority = MusicPriority.BossHigh;
                music = GetSoundSlot(SoundType.Music, null);
            }
            if (MyUIStateActive(Main.LocalPlayer) && ChooseStarter.movieFinished)
            {
                priority = MusicPriority.BossHigh;
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/wifi");
            }
        }

        // END UI STUFF


#region HotKeys

        public ModHotKey FirstPKMAbility { get; private set; }
        public ModHotKey SecondPKMAbility { get; private set; }
        public ModHotKey ThirdPKMAbility { get; private set; }
        public ModHotKey FourthPKMAbility { get; private set; }

#endregion


        /// <summary>
        ///     Class used to save pokeball rarity when manipulating
        ///     items data;
        /// </summary>
        public static class PokeballFactory
        {
            public enum Pokebals : byte
            {
                Nothing = 0,
                Pokeball,
                GreatBall,
                UltraBall,
                MasterBall,
                DuskBall,
                PremierBall,
                QuickBall,
                TimerBall,
                ZeroBall,
                PumpkinBall
            }

            /// <summary>
            ///     Return type id for provided pokeball.
            ///     Mostly used for loading from saves
            /// </summary>
            /// <param name="item">Byte enum of save pokeball</param>
            /// <returns>Return item id or 0 if this is not a pokeball</returns>
            public static int GetPokeballType(Pokebals item)
            {
                switch (item)
                {
                    case Pokebals.Pokeball:
                        return ModContent.ItemType<PokeballCaught>();
                    case Pokebals.GreatBall:
                        return ModContent.ItemType<GreatBallCaught>();
                    case Pokebals.UltraBall:
                        return ModContent.ItemType<UltraBallCaught>();
                    case Pokebals.MasterBall:
                        return ModContent.ItemType<MasterBallCaught>();
                    case Pokebals.DuskBall:
                        return ModContent.ItemType<DuskBallCaught>();
                    case Pokebals.PremierBall:
                        return ModContent.ItemType<PremierBallCaught>();
                    case Pokebals.QuickBall:
                        return ModContent.ItemType<QuickBallCaught>();
                    case Pokebals.TimerBall:
                        return ModContent.ItemType<TimerBallCaught>();
                    case Pokebals.ZeroBall:
                        return ModContent.ItemType<ZeroBallCaught>();
                    case Pokebals.PumpkinBall:
                        return ModContent.ItemType<PumpkinBallCaught>();
                    default:
                        return 0;
                }
            }

            /// <summary>
            ///     Return enum byte for provided item.
            ///     Mostly used for saving
            /// </summary>
            /// <param name="item">ModItem of item</param>
            /// <returns>
            ///     Return byte enum or <see cref="Pokebals.Nothing" />
            ///     if provided item is not a pokeball
            /// </returns>
            public static Pokebals GetEnum(ModItem item)
            {
                if (item is PokeballCaught) return Pokebals.Pokeball;
                if (item is GreatBallCaught) return Pokebals.GreatBall;
                if (item is UltraBallCaught) return Pokebals.UltraBall;
                if (item is MasterBallCaught) return Pokebals.MasterBall;
                if (item is DuskBallCaught) return Pokebals.DuskBall;
                if (item is PremierBallCaught) return Pokebals.PremierBall;
                if (item is QuickBallCaught) return Pokebals.QuickBall;
                if (item is TimerBallCaught) return Pokebals.TimerBall;
                if (item is ZeroBallCaught) return Pokebals.ZeroBall;
                if (item is PumpkinBallCaught) return Pokebals.PumpkinBall;
                return Pokebals.Nothing;
            }

            /// <summary>
            ///     Return item type id from provided pokeball
            /// </summary>
            /// <param name="item">ModItem of item</param>
            /// <returns>Return item id or 0 if this is not a pokeball</returns>
            public static int GetPokeballType(ModItem item)
            {
                if (item is PokeballCaught) return ModContent.ItemType<PokeballCaught>();
                if (item is GreatBallCaught) return ModContent.ItemType<GreatBallCaught>();
                if (item is UltraBallCaught) return ModContent.ItemType<UltraBallCaught>();
                if (item is DuskBallCaught) return ModContent.ItemType<DuskBallCaught>();
                if (item is PremierBallCaught) return ModContent.ItemType<PremierBallCaught>();
                if (item is QuickBallCaught) return ModContent.ItemType<QuickBallCaught>();
                if (item is TimerBallCaught) return ModContent.ItemType<TimerBallCaught>();
                if (item is MasterBallCaught) return ModContent.ItemType<MasterBallCaught>();
                if (item is ZeroBallCaught) return ModContent.ItemType<ZeroBallCaught>();
                if (item is PumpkinBallCaught) return ModContent.ItemType<PumpkinBallCaught>();
                return 0;
            }
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {

            //In case i f*ck the code
            try
            {
                string type = reader.ReadString();
#if DEBUG
                Main.NewText($"Received packet: [c/ff3333:{type}]");
#endif
                switch (type)
                {
                    case SpawnStarterPacket.NAME:
                    {
                        //Server can't have any UI
                        if (whoAmI == 256)
                            return;
                        SpawnStarterPacket packet = new SpawnStarterPacket();
                        packet.HandleFromClient(reader, whoAmI);
                    }
                        break;
                    case BaseCatchPacket.NAME:
                    {
                        //Server should handle it from client
                        if (whoAmI == 256)
                            return;
                        BaseCatchPacket packet = new BaseCatchPacket();
                        packet.HandleFromClient(reader, whoAmI);
                    }
                        break;
                    default:
                        if (packetStore.ContainsKey(type))
                        {
                            if (whoAmI == 256)
                                packetStore[type].HandleFromServer(reader);
                            else
                                packetStore[type].HandleFromClient(reader, whoAmI);
                        }
                        break;
                }
                
            }
            catch (Exception e)
            {
                Logger.ErrorFormat(
                    "Exception appear in HandlePacket. Please, contact mod devs with folowing stacktrace:\n\n{0}\n\n{1}",
                    e.Message, e.StackTrace);
#if DEBUG
                Main.NewText($"[c/ff3322:{e.Message}]");
#endif
            }
        }

        public static TerramonMod Instance { get; private set; }

        public static IEnumerable<string> GetPokemonsNames()
        {
            return Instance.pokemonStore.Keys.ToList();
        }

        public static ParentPokemon GetPokemon(string monName)
        {
            if (monName == null) return null;
            if (Instance.pokemonStore != null && Instance.pokemonStore.ContainsKey(monName))
                return Instance.pokemonStore[monName];
            return null;
        }

        // ReSharper disable once UnusedMember.Global
        public static ParentPokemonNPC GetWildPokemon(string monName)
        {
            if (monName == null) return null;
            if (Instance.pokemonStore != null && Instance.pokemonStore.ContainsKey(monName))
                return Instance.wildPokemonStore[monName];
            return null;
        }

        public static BaseMove GetMove(string moveName)
        {
            if (string.IsNullOrEmpty(moveName)) return null;
            if (Instance.movesStore != null && Instance.movesStore.ContainsKey(moveName))
                return Instance.movesStore[moveName];
            return null;
        }


        private Dictionary<string, ParentPokemon> pokemonStore;
        private Dictionary<string, ParentPokemonNPC> wildPokemonStore;
        private Dictionary<string, BaseMove> movesStore;
        private Dictionary<string, Packet> packetStore;

        private void LoadPokemons()
        {
            pokemonStore = new Dictionary<string, ParentPokemon>();
            wildPokemonStore = new Dictionary<string, ParentPokemonNPC>();
            movesStore = new Dictionary<string, BaseMove>();
            packetStore = new Dictionary<string, Packet>();
            foreach (TypeInfo it in GetType().Assembly.DefinedTypes)
            {
                var baseType = it.BaseType;
                if (it.IsAbstract)
                    continue;
                bool valid = false;
                if (baseType == typeof(ParentPokemon) || baseType == typeof(ParentPokemonNPC) ||
                    baseType == typeof(BaseMove) || baseType == typeof(Packet))
                    valid = true;
                else
                    //Recurrent seek for our class
                    while (baseType != null && baseType != typeof(object))
                    {
                        if (baseType == typeof(ParentPokemon) || baseType == typeof(ParentPokemonNPC) ||
                            baseType == typeof(BaseMove) || baseType == typeof(Packet))
                        {
                            valid = true;
                            break;
                        }

                        baseType = baseType.BaseType;
                    }

                if (valid)
                    try
                    {
                        if (baseType == typeof(ParentPokemon))
                            pokemonStore.Add(it.Name, (ParentPokemon) Activator.CreateInstance(it));
                        else if (baseType == typeof(ParentPokemonNPC))
                            wildPokemonStore.Add(it.Name, (ParentPokemonNPC) Activator.CreateInstance(it));
                        else if (baseType == typeof(BaseMove))
                            movesStore.Add(it.Name, (BaseMove) Activator.CreateInstance(it));
                        else if (baseType == typeof(Packet))
                        {
                            var p = (Packet) Activator.CreateInstance(it);
                            packetStore.Add(p.PacketName, p);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error(
                            "Exception caught in Events register loop. Report mod author with related stacktrace: \n" +
                            $"{e.Message}\n" +
                            $"{e.StackTrace}\n");
                    }
            }
        }
        public static bool UseWebAssets = false;
        public static bool WebResourceAvailable(string name) => Store.Get(name) != null;
    }
}