using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.Graphics;
using Razorwing.Framework.Graphics.Transforms;
using Razorwing.Framework.Localisation;
using Razorwing.Framework.Utils;
using System;
using Terramon.Items.Pokeballs;
using Terramon.Network.Sync.Battle;
using Terramon.Players;
using Terramon.Pokemon;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terramon.UI.SidebarParty
{
    internal class UISidebar : UIState
    {
        public SidebarPanel mainPanel;
        public static bool Visible;
        public bool lightmode = true;

        public ILocalisedBindableString sendOutText =
            TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("sidebar.sendOut",
                "Left click to send out!")));

        public ILocalisedBindableString goText =
            TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("go", "Go {0}!")));

        public ILocalisedBindableString retire1Text =
            TerramonMod.Localisation.GetLocalisedString(
                new LocalisedString(("retire1", "{0}, switch out!\nCome back!")));

        public ILocalisedBindableString retire2Text =
            TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("retire2", "{0}, return!")));

        public ILocalisedBindableString retire3Text =
            TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("retire3",
                "That's enough for now, {0}!")));

        public ILocalisedBindableString pokemonName1 = TerramonMod.Localisation.GetLocalisedString("*");
        public ILocalisedBindableString pokemonName2 = TerramonMod.Localisation.GetLocalisedString("*");
        public ILocalisedBindableString pokemonName3 = TerramonMod.Localisation.GetLocalisedString("*");
        public ILocalisedBindableString pokemonName4 = TerramonMod.Localisation.GetLocalisedString("*");
        public ILocalisedBindableString pokemonName5 = TerramonMod.Localisation.GetLocalisedString("*");
        public ILocalisedBindableString pokemonName6 = TerramonMod.Localisation.GetLocalisedString("*");

        public ILocalisedBindableString helpText =
            TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("sidebar.help", "Terramon Help")));

        public ILocalisedBindableString help1Text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString((
            "sidebar.help1",
            "(1/3) Welcome to Terramon {0}, where you can discover and catch Pokémon in Terraria! Keep pressing this button for more tips and tricks.")));

        public ILocalisedBindableString help2Text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString((
            "sidebar.help2",
            "(2/3) For support, join the official Discord server using the [c/f7e34d:/discord] command. Or, access our wiki with the [c/f7e34d:/wiki] command.")));

        public ILocalisedBindableString help3Text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString((
            "sidebar.help3",
            "(3/3) Also, feel free to customize your experience with the Mod Config in [c/ff8f33:Settings > Mod Configuration] or from the Mods menu.")));


        public string p1 = "*", p2 = "*", p3 = "*", p4 = "*", p5 = "*", p6 = "*";


        public UIOpaqueButton choose;
        public UIOpaqueButton battle;

        //sidebar pkmn textures
        public SidebarClass firstpkmn;
        public Texture2D firstpkmnringtexture;
        public UIImagez firstpkmnring;

        public SidebarClass secondpkmn;
        public Texture2D secondpkmnringtexture;
        public UIImagez secondpkmnring;

        public SidebarClass thirdpkmn;
        public Texture2D thirdpkmnringtexture;
        public UIImagez thirdpkmnring;

        public SidebarClass fourthpkmn;
        public Texture2D fourthpkmnringtexture;
        public UIImagez fourthpkmnring;

        public SidebarClass fifthpkmn;
        public Texture2D fifthpkmnringtexture;
        public UIImagez fifthpkmnring;

        public SidebarClass sixthpkmn;
        public Texture2D sixthpkmnringtexture;
        public UIImagez sixthpkmnring;

        public int CycleIndex;
        public int HelpListCycler;

        public bool isCompressed = false;


        public override void OnInitialize()
        {
            //Add version string as argument so it can be passed in other locales
            help1Text.Args = new object[] { TerramonMod.Instance.Version };

            Append(TerramonMod.ZoomAnimator = new Animator());

            //pokemon icons
            mainPanel = new SidebarPanel();
            mainPanel.SetPadding(0);
            mainPanel.Left.Set(-8, 0f);
            mainPanel.VAlign = 0.65f;
            mainPanel.Width.Set(94, 0f);
            mainPanel.Height.Set(400, 0f);
            mainPanel.BackgroundColor = new Color(15, 15, 15) * 0.65f;

            Texture2D chooseTexture = ModContent.GetTexture("Terramon/UI/SidebarParty/Help");
            choose = new UIOpaqueButton(chooseTexture, helpText.Value);
            choose.HAlign = 0.007f; // 1
            choose.VAlign = 0.98f; // 1
            choose.Width.Set(20, 0);
            choose.Height.Set(32, 0);
            choose.OnClick += HelpClicked;
            Append(choose);

            //#if DEBUG
            chooseTexture = ModContent.GetTexture("Terramon/UI/SidebarParty/LightMode");
            battle = new UIOpaqueButton(chooseTexture, "[PH] Start Battle")
            {
                HAlign = 0.007f, // 1
                VAlign = 0.9f // 1
            };
            battle.Width.Set(28, 0);
            battle.Height.Set(38, 0);
            battle.OnClick += (e, x) =>
            {
                var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                if (player.ActivePet?.Fainted ?? false)
                {
                    Main.NewText($"Your {player.ActivePet.Pokemon} is fainted and can't fight!");
                    return;
                }

                if (Main.keyState.PressingShift())
                {
                    var pl = BaseMove.GetNearestPlayer(Main.LocalPlayer.position, Main.LocalPlayer);
                    if (pl != null)
                    {
                        player.Battle = new BattleMode(player, BattleState.BattleWithPlayer,
                            spl: pl.GetModPlayer<TerramonPlayer>());
                        pl.GetModPlayer<TerramonPlayer>().Battle = new BattleMode(pl.GetModPlayer<TerramonPlayer>(),
                            BattleState.BattleWithPlayer, spl: player);

                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            var p = new StartBattlePacket();
                            p.Send(TerramonMod.Instance, BattleState.BattleWithPlayer, pl);
                        }
                    }
                }
                else
                {
                    var id = BaseMove.GetNearestPokemon(Main.LocalPlayer.position);
                    if (id != null)
                    {
                        var data = new PokemonData()
                        {
                            Pokemon = ((ParentPokemonNPC)id.modNPC).HomeClass().Name,
                        };
                        player.Battle = new BattleMode(player, BattleState.BattleWithWild,
                            npc: (ParentPokemonNPC)id.modNPC, second: data);
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            var p = new StartBattlePacket();
                            p.Send(TerramonMod.Instance, BattleState.BattleWithWild, data, player.Battle.wildID);
                        }
                    }
                }

            };
            //Append(battle);

            //#endif

            //firstpkmntexture = ModContent.GetTexture("Terramon/UI/SidebarParty/Empty");
            firstpkmn = new SidebarClass("")
            {
                HAlign = 0.6f,
                VAlign = 0.08888f,
                Size = new Vector2(50, 40),
                TextureName = "Terramon/UI/SidebarParty/Empty"
            };
            firstpkmn.OnClick += SpawnPKMN1;
            mainPanel.Append(firstpkmn);

            firstpkmnringtexture = ModContent.GetTexture("Terraria/Item_0");
            firstpkmnring = new UIImagez(firstpkmnringtexture);
            firstpkmnring.HAlign = 0.6f; // 1
            firstpkmnring.VAlign = 0.06666f; // 1
            firstpkmnring.Width.Set(40, 0);
            firstpkmnring.Height.Set(40, 0);
            mainPanel.Append(firstpkmnring);

            //secondpkmntexture = ModContent.GetTexture("Terramon/UI/SidebarParty/Empty");
            secondpkmn = new SidebarClass("")
            {
                HAlign = 0.6f,
                VAlign = 0.25555f,
                Size = new Vector2(50, 40),
                TextureName = "Terramon/UI/SidebarParty/Empty"
            };
            secondpkmn.OnClick += SpawnPKMN2;
            mainPanel.Append(secondpkmn);

            secondpkmnringtexture = ModContent.GetTexture("Terraria/Item_0");
            secondpkmnring = new UIImagez(secondpkmnringtexture);
            secondpkmnring.HAlign = 0.6f; // 1
            secondpkmnring.VAlign = 0.25555f; // 1
            secondpkmnring.Width.Set(40, 0);
            secondpkmnring.Height.Set(40, 0);
            mainPanel.Append(secondpkmnring);

            //thirdpkmntexture = ModContent.GetTexture("Terramon/UI/SidebarParty/Empty");
            thirdpkmn = new SidebarClass("")
            {
                HAlign = 0.6f,
                VAlign = 0.41111f,
                Size = new Vector2(50, 40),
                TextureName = "Terramon/UI/SidebarParty/Empty"
            };
            thirdpkmn.OnClick += SpawnPKMN3;
            mainPanel.Append(thirdpkmn);

            thirdpkmnringtexture = ModContent.GetTexture("Terraria/Item_0");
            thirdpkmnring = new UIImagez(thirdpkmnringtexture);
            thirdpkmnring.HAlign = 0.6f; // 1
            thirdpkmnring.VAlign = 0.41111f; // 1
            thirdpkmnring.Width.Set(40, 0);
            thirdpkmnring.Height.Set(40, 0);
            mainPanel.Append(thirdpkmnring);

            //fourthpkmntexture = ModContent.GetTexture("Terramon/UI/SidebarParty/Empty");
            fourthpkmn = new SidebarClass("")
            {
                HAlign = 0.6f,
                VAlign = 0.58888f,
                Size = new Vector2(50, 40),
                TextureName = "Terramon/UI/SidebarParty/Empty"
            };
            fourthpkmn.OnClick += SpawnPKMN4;
            mainPanel.Append(fourthpkmn);

            fourthpkmnringtexture = ModContent.GetTexture("Terraria/Item_0");
            fourthpkmnring = new UIImagez(fourthpkmnringtexture);
            fourthpkmnring.HAlign = 0.6f; // 1
            fourthpkmnring.VAlign = 0.58888f; // 1
            fourthpkmnring.Width.Set(40, 0);
            fourthpkmnring.Height.Set(40, 0);
            mainPanel.Append(fourthpkmnring);

            //fifthpkmntexture = ModContent.GetTexture("Terramon/UI/SidebarParty/Empty");
            fifthpkmn = new SidebarClass("")
            {
                HAlign = 0.6f,
                VAlign = 0.75555f,
                Size = new Vector2(50, 40),
                TextureName = "Terramon/UI/SidebarParty/Empty"
            };
            fifthpkmn.OnClick += SpawnPKMN5;
            mainPanel.Append(fifthpkmn);

            fifthpkmnringtexture = ModContent.GetTexture("Terraria/Item_0");
            fifthpkmnring = new UIImagez(fifthpkmnringtexture);
            fifthpkmnring.HAlign = 0.6f; // 1
            fifthpkmnring.VAlign = 0.75555f; // 1
            fifthpkmnring.Width.Set(40, 0);
            fifthpkmnring.Height.Set(40, 0);
            mainPanel.Append(fifthpkmnring);

            //sixthpkmntexture = ModContent.GetTexture("Terramon/UI/SidebarParty/Empty");
            sixthpkmn = new SidebarClass("")
            {
                HAlign = 0.6f,
                VAlign = 0.91111f,
                Size = new Vector2(50, 40),
                TextureName = "Terramon/UI/SidebarParty/Empty"
            };
            sixthpkmn.OnClick += SpawnPKMN6;
            mainPanel.Append(sixthpkmn);

            sixthpkmnringtexture = ModContent.GetTexture("Terraria/Item_0");
            sixthpkmnring = new UIImagez(sixthpkmnringtexture);
            sixthpkmnring.HAlign = 0.6f; // 1
            sixthpkmnring.VAlign = 0.93333f; // 1
            sixthpkmnring.Width.Set(40, 0);
            sixthpkmnring.Height.Set(40, 0);
            mainPanel.Append(sixthpkmnring);

            Append(mainPanel);
        }

        private byte compressAnimation;

        public bool compressing;
        public bool isReallyCompressed;

        private double startCompressAnimation;
        private double endCompressAnimation;

        private bool finishedIn;

        public override void Update(GameTime gameTime)
        {
            // Don't delete this or the UIElements attached to this UIState will cease to function.
            base.Update(gameTime);

            if (isCompressed && !finishedIn)
            {
                if (compressAnimation == 0)
                {
                    startCompressAnimation = gameTime.TotalGameTime.TotalSeconds;
                    endCompressAnimation = startCompressAnimation + 1;
                    compressing = true;
                    compressAnimation = 1;

                    firstpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .Schedule(() => //Apply changes from mid time
                        {
                            isReallyCompressed = true;
                        }).Then()
                        .ScaleTo(1f, 500f,
                            Easing.Out); //This will be called at same frame as Schedule bc no delay specified in Then method
                    secondpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .ScaleTo(1f, 500f, Easing.Out);
                    thirdpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .ScaleTo(1f, 500f, Easing.Out);
                    fourthpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .ScaleTo(1f, 500f, Easing.Out);
                    fifthpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .ScaleTo(1f, 500f, Easing.Out);
                    sixthpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .ScaleTo(1f, 500f, Easing.Out);
                }

                if (compressAnimation == 1 && gameTime.TotalGameTime.TotalSeconds < endCompressAnimation)
                {
                    mainPanel.Width.Pixels = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 94, 52,
                        startCompressAnimation, endCompressAnimation, Easing.OutExpo);
                }

                //if (gameTime.TotalGameTime.TotalSeconds > endCompressAnimation - 0.5)
                //{
                //    isReallyCompressed = true;
                //    firstpkmn.ScaleTo(1f, 500, Easing.Out);
                //    secondpkmn.ScaleTo(1f, 500, Easing.Out);
                //    thirdpkmn.ScaleTo(1f, 500, Easing.Out);
                //    fourthpkmn.ScaleTo(1f, 500, Easing.Out);
                //    fifthpkmn.ScaleTo(1f, 500, Easing.Out);
                //    sixthpkmn.ScaleTo(1f, 500, Easing.Out);
                //}
                if (gameTime.TotalGameTime.TotalSeconds > endCompressAnimation)
                {
                    compressing = false;
                    compressAnimation = 0;
                    startCompressAnimation = 0;
                    endCompressAnimation = 0;
                    finishedIn = true;
                }
            }

            if (!isCompressed && finishedIn)
            {
                if (compressAnimation == 0)
                {
                    startCompressAnimation = gameTime.TotalGameTime.TotalSeconds;
                    endCompressAnimation = startCompressAnimation + 1;
                    compressing = true;
                    compressAnimation = 1;

                    firstpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .Schedule(() => //Apply changes from mid time
                        {
                            isReallyCompressed = false;
                        }).Then()
                        .ScaleTo(1f, 500f,
                            Easing.Out); //This will be called at same frame as Schedule bc no delay specified in Then method
                    secondpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .ScaleTo(1f, 500f, Easing.Out);
                    thirdpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .ScaleTo(1f, 500f, Easing.Out);
                    fourthpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .ScaleTo(1f, 500f, Easing.Out);
                    fifthpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .ScaleTo(1f, 500f, Easing.Out);
                    sixthpkmn.ScaleTo(0.001f, 500, Easing.Out).Then()
                        .ScaleTo(1f, 500f, Easing.Out);
                }

                if (compressAnimation == 1 && gameTime.TotalGameTime.TotalSeconds < endCompressAnimation)
                {
                    mainPanel.Width.Pixels = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 52, 94,
                        startCompressAnimation, endCompressAnimation, Easing.OutExpo);
                }

                //if (gameTime.TotalGameTime.TotalSeconds > endCompressAnimation - 0.5)
                //{
                //    isReallyCompressed = false;
                //    firstpkmn.ScaleTo(1f, 500, Easing.Out);
                //    secondpkmn.ScaleTo(1f, 500, Easing.Out);
                //    thirdpkmn.ScaleTo(1f, 500, Easing.Out);
                //    fourthpkmn.ScaleTo(1f, 500, Easing.Out);
                //    fifthpkmn.ScaleTo(1f, 500, Easing.Out);
                //    sixthpkmn.ScaleTo(1f, 500, Easing.Out);
                //}
                if (gameTime.TotalGameTime.TotalSeconds > endCompressAnimation)
                {
                    compressing = false;
                    compressAnimation = 0;
                    startCompressAnimation = 0;
                    endCompressAnimation = 0;
                    finishedIn = false;
                }
            }

            mainPanel.Height.Set(0, 0.7f);

            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            var slotName = modPlayer.firstslotname;
            var slotTag = modPlayer.PartySlot1;

            if (slotName != "*")
            {
                if (p1 != slotName)
                {
                    p1 = slotName;
                    pokemonName1 =
                        TerramonMod.Localisation.GetLocalisedString(new LocalisedString(slotName));
                }

                updateSlot(slotTag, firstpkmn, firstpkmnring,
                    pokemonName1.Value); //move copypaste to method, so we can modify display data from one place
            }

            slotName = modPlayer.secondslotname;
            slotTag = modPlayer.PartySlot2;

            if (slotName != "*")
            {
                if (p2 != slotName)
                {
                    p2 = slotName;
                    pokemonName2 =
                        TerramonMod.Localisation.GetLocalisedString(new LocalisedString(slotName));
                }

                updateSlot(slotTag, secondpkmn, secondpkmnring, pokemonName2.Value);
            }

            slotName = modPlayer.thirdslotname;
            slotTag = modPlayer.PartySlot3;

            if (slotName != "*")
            {
                if (p3 != slotName)
                {
                    p3 = slotName;
                    pokemonName3 =
                        TerramonMod.Localisation.GetLocalisedString(new LocalisedString(slotName));
                }

                updateSlot(slotTag, thirdpkmn, thirdpkmnring, pokemonName3.Value);
            }

            slotName = modPlayer.fourthslotname;
            slotTag = modPlayer.PartySlot4;

            if (slotName != "*")
            {
                if (p4 != slotName)
                {
                    p4 = slotName;
                    pokemonName4 =
                        TerramonMod.Localisation.GetLocalisedString(new LocalisedString(slotName));
                }

                updateSlot(slotTag, fourthpkmn, fourthpkmnring, pokemonName4.Value);
            }

            slotName = modPlayer.fifthslotname;
            slotTag = modPlayer.PartySlot5;

            if (slotName != "*")
            {
                if (p5 != slotName)
                {
                    p5 = slotName;
                    pokemonName5 =
                        TerramonMod.Localisation.GetLocalisedString(new LocalisedString(slotName));
                }

                updateSlot(slotTag, fifthpkmn, fifthpkmnring, pokemonName5.Value);

            }

            slotName = modPlayer.sixthslotname;
            slotTag = modPlayer.PartySlot6;

            if (slotName != "*")
            {
                if (p6 != slotName)
                {
                    p6 = slotName;
                    pokemonName6 =
                        TerramonMod.Localisation.GetLocalisedString(new LocalisedString(slotName));
                }

                updateSlot(slotTag, sixthpkmn, sixthpkmnring, pokemonName6.Value);
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private void updateSlot(PokemonData slot, SidebarClass side, UIImagez ring, string name)
        {
            if (!isReallyCompressed)
            {
                side.TextureName = "Terramon/Minisprites/Regular/SidebarSprites/" + slot.Pokemon;
            }
            else
            {
                side.TextureName = "Terramon/UI/SidebarParty/CaughtIn/" + slot.pokeballType;
            }

            side.HoverText = name + $"[i:{ModContent.ItemType<SidebarPKBALL>()}]" +
                             $"\nLVL: {slot.Level}" +
                             $"\nEXP: {slot.Exp}" +
                             $"\nEXP Lv Up: {slot.ExpToNext}" +
                             $"\nHP: {slot.HP}/{slot.MaxHP}" +
                             $"\n{sendOutText.Value}";
            side.Recalculate();
        }

        private void HelpClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            HelpListCycler++;
            switch (HelpListCycler)
            {
                case 1:
                    Main.NewText(help1Text.Value);
                    break;
                case 2:
                    Main.NewText(help2Text.Value);
                    break;
                case 3:
                    Main.NewText(help3Text.Value);
                    HelpListCycler = 0;
                    break;
            }
        }

        private bool UpdateBattle(TerramonPlayer pl)
        {
            if (pl.Battle != null) //If player in battle
            {
                if ((pl.Battle.awaitSync && (pl.ActivePet.Fainted)) || (!pl.Battle.MoveDone))
                {
                    //Change mon packet
                    pl.Battle.awaitSync = false;
                    if (pl.Battle.State == BattleState.BattleWithPlayer)
                    {
                        pl.Battle.player2.Battle.awaitSync = false;
                    }

                    return true;
                }
                else
                {
                    return false; //We don't allow change mon if move was selected
                }
            }

            return true;
        }

        private void SpawnPKMN1(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.firstslotname == "*")
                return;

            if (!UpdateBattle(modPlayer))
                return;

            var pokeBuff = ModContent.GetInstance<TerramonMod>().BuffType(nameof(PokemonBuff));
            var pet = modPlayer.PartySlot1.Pokemon;
            if (modPlayer.ActivePetName != pet)
            {
                if (!string.IsNullOrEmpty(modPlayer.ActivePetName) && modPlayer.ActivePetName != "*")
                    PrintSwitch(player, modPlayer);
                if (!player.HasBuff(pokeBuff)) player.AddBuff(pokeBuff, 2);
                modPlayer.ActivePetName = pet;
                goText.Args = new object[] { pokemonName1.Value };
                //CombatText.NewText(player.Hitbox, Color.White, goText.Value, true);
                Main.PlaySound(ModContent.GetInstance<TerramonMod>()
                    .GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sendout"));
                modPlayer.ActivePartySlot = 1; //This automatically sync active pet
            }
            else
            {
                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.position, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.width, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.height,
                        ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
                }
                player.ClearBuff(pokeBuff);
                PrintSwitch(player, modPlayer);
                modPlayer.ActivePetName = string.Empty;
                modPlayer.ActivePartySlot = -1;
            }
        }

        private void SpawnPKMN2(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.secondslotname == "*")
                return;

            if (!UpdateBattle(modPlayer))
                return;

            var pokeBuff = ModContent.GetInstance<TerramonMod>().BuffType(nameof(PokemonBuff));
            var pet = modPlayer.PartySlot2.Pokemon;
            if (modPlayer.ActivePetName != pet)
            {
                if (!string.IsNullOrEmpty(modPlayer.ActivePetName) && modPlayer.ActivePetName != "*")
                    PrintSwitch(player, modPlayer);
                if (!player.HasBuff(pokeBuff)) player.AddBuff(pokeBuff, 2);
                modPlayer.ActivePetName = pet;
                goText.Args = new object[] { pokemonName2.Value };
                //CombatText.NewText(player.Hitbox, Color.White, goText.Value, true);
                Main.PlaySound(ModContent.GetInstance<TerramonMod>()
                    .GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sendout"));
                modPlayer.ActivePartySlot = 2;
            }
            else
            {
                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.position, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.width, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.height,
                        ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
                }
                player.ClearBuff(pokeBuff);
                PrintSwitch(player, modPlayer);
                modPlayer.ActivePetName = string.Empty;
                modPlayer.ActivePartySlot = -1;
            }
        }

        private void SpawnPKMN3(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.thirdslotname == "*")
                return;

            if (!UpdateBattle(modPlayer))
                return;

            var pokeBuff = ModContent.GetInstance<TerramonMod>().BuffType(nameof(PokemonBuff));
            var pet = modPlayer.PartySlot3.Pokemon;
            if (modPlayer.ActivePetName != pet)
            {
                if (!string.IsNullOrEmpty(modPlayer.ActivePetName) && modPlayer.ActivePetName != "*")
                    PrintSwitch(player, modPlayer);
                if (!player.HasBuff(pokeBuff)) player.AddBuff(pokeBuff, 2);
                modPlayer.ActivePetName = pet;
                goText.Args = new object[] { pokemonName3.Value };
                //CombatText.NewText(player.Hitbox, Color.White, goText.Value, true);
                Main.PlaySound(ModContent.GetInstance<TerramonMod>()
                    .GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sendout"));
                modPlayer.ActivePartySlot = 3;
            }
            else
            {
                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.position, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.width, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.height,
                        ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
                }
                player.ClearBuff(pokeBuff);
                PrintSwitch(player, modPlayer);
                modPlayer.ActivePetName = string.Empty;
                modPlayer.ActivePartySlot = -1;
            }
        }

        private void SpawnPKMN4(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.fourthslotname == "*")
                return;

            if (!UpdateBattle(modPlayer))
                return;

            var pokeBuff = ModContent.GetInstance<TerramonMod>().BuffType(nameof(PokemonBuff));
            var pet = modPlayer.PartySlot4.Pokemon;
            if (modPlayer.ActivePetName != pet)
            {
                if (!string.IsNullOrEmpty(modPlayer.ActivePetName) && modPlayer.ActivePetName != "*")
                    PrintSwitch(player, modPlayer);
                if (!player.HasBuff(pokeBuff)) player.AddBuff(pokeBuff, 2);
                modPlayer.ActivePetName = pet;
                goText.Args = new object[] { pokemonName4.Value };
                //CombatText.NewText(player.Hitbox, Color.White, goText.Value, true);
                Main.PlaySound(ModContent.GetInstance<TerramonMod>()
                    .GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sendout"));
                modPlayer.ActivePartySlot = 4;
            }
            else
            {
                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.position, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.width, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.height,
                        ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
                }
                player.ClearBuff(pokeBuff);
                PrintSwitch(player, modPlayer);
                modPlayer.ActivePetName = string.Empty;
                modPlayer.ActivePartySlot = -1;
            }
        }

        private void SpawnPKMN5(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.fifthslotname == "*")
                return;

            if (!UpdateBattle(modPlayer))
                return;

            var pokeBuff = ModContent.GetInstance<TerramonMod>().BuffType(nameof(PokemonBuff));
            var pet = modPlayer.PartySlot5.Pokemon;
            if (modPlayer.ActivePetName != pet)
            {
                if (!string.IsNullOrEmpty(modPlayer.ActivePetName) && modPlayer.ActivePetName != "*")
                    PrintSwitch(player, modPlayer);
                if (!player.HasBuff(pokeBuff)) player.AddBuff(pokeBuff, 2);
                modPlayer.ActivePetName = pet;
                goText.Args = new object[] { pokemonName5.Value };
                //CombatText.NewText(player.Hitbox, Color.White, goText.Value, true);
                Main.PlaySound(ModContent.GetInstance<TerramonMod>()
                    .GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sendout"));
                modPlayer.ActivePartySlot = 5;
            }
            else
            {
                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.position, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.width, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.height,
                        ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
                }
                player.ClearBuff(pokeBuff);
                PrintSwitch(player, modPlayer);
                modPlayer.ActivePetName = string.Empty;
                modPlayer.ActivePartySlot = -1;
            }
        }

        private void SpawnPKMN6(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.sixthslotname == "*")
                return;

            if (!UpdateBattle(modPlayer))
                return;

            var pokeBuff = ModContent.GetInstance<TerramonMod>().BuffType(nameof(PokemonBuff));
            var pet = modPlayer.PartySlot6.Pokemon;
            if (modPlayer.ActivePetName != pet)
            {
                if (!string.IsNullOrEmpty(modPlayer.ActivePetName) && modPlayer.ActivePetName != "*")
                    PrintSwitch(player, modPlayer);
                if (!player.HasBuff(pokeBuff)) player.AddBuff(pokeBuff, 2);
                modPlayer.ActivePetName = pet;
                goText.Args = new object[] { pokemonName6.Value };
                //CombatText.NewText(player.Hitbox, Color.White, goText.Value, true);
                Main.PlaySound(ModContent.GetInstance<TerramonMod>()
                    .GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sendout"));
                modPlayer.ActivePartySlot = 6;
            }
            else
            {
                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.position, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.width, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.height,
                        ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
                }
                player.ClearBuff(pokeBuff);
                PrintSwitch(player, modPlayer);
                modPlayer.ActivePetName = string.Empty;
                modPlayer.ActivePartySlot = -1;
            }
        }

        protected void PrintSwitch(Player player, TerramonPlayer modPlayer)
        {
            var rect = new Rectangle(player.Hitbox.X, player.Hitbox.Y, player.Hitbox.Width, player.Hitbox.Height);
            rect.Y -= 35;
            var pet = "*";
            switch (modPlayer.ActivePartySlot)
            {
                case 1:
                    pet = pokemonName1.Value;
                    break;
                case 2:
                    pet = pokemonName2.Value;
                    break;
                case 3:
                    pet = pokemonName3.Value;
                    break;
                case 4:
                    pet = pokemonName4.Value;
                    break;
                case 5:
                    pet = pokemonName5.Value;
                    break;
                case 6:
                    pet = pokemonName6.Value;
                    break;
            }

            switch (Main.rand.Next(3))
            {
                case 0:
                    retire1Text.Args = new object[] { pet };
                    //CombatText.NewText(rect, Color.White, retire1Text.Value, true);
                    break;
                case 1:
                    retire2Text.Args = new object[] { pet };
                    //CombatText.NewText(rect, Color.White, retire2Text.Value, true);
                    break;
                default:
                    retire3Text.Args = new object[] { pet };
                    //CombatText.NewText(rect, Color.White, retire3Text.Value, true);
                    break;
            }
        }
    }
}