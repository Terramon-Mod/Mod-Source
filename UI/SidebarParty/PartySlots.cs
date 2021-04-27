using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Players;
using Terramon.Pokemon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using System;
using Terramon.Tiles.ShelfBlocks;
using Razorwing.Framework.Utils;
using Razorwing.Framework.Graphics;
using Terraria.GameContent.UI.Elements;

namespace Terramon.UI.SidebarParty
{
    // ExampleUIs visibility is toggled by typing "/coin" in chat. (See CoinCommand.cs)
    // ExampleUI is a simple UI example showing how to use UIPanel, UIImageButton, and even a custom UIElement.
    public class PartySlots : UIState
    {
        private DragableUIPanel mainPanel;
        public static bool Visible;
        public bool lightmode = true;
        public static bool hoveringAnySlot = false;
        public bool isCompressed = false;

        public static PokemonData toSwap;
        public static int toSwapSlotNumber;
        public static PokemonData swapWith;
        public static int swapWithSlotNumber;

        public Texture2D test;

        private UIHoverImageButton toggleslots;
        private UIText partytip;
        public CustomPartyItemSlot customslot1;
        public CustomPartyItemSlot customslot2;
        public CustomPartyItemSlot customslot3;
        public CustomPartyItemSlot customslot4;
        public CustomPartyItemSlot customslot5;
        public CustomPartyItemSlot customslot6;

        public VanillaItemSlotWrapper partyslot1;
        public VanillaItemSlotWrapper partyslot2;
        public VanillaItemSlotWrapper partyslot3;
        public VanillaItemSlotWrapper partyslot4;
        public VanillaItemSlotWrapper partyslot5;
        public VanillaItemSlotWrapper partyslot6;

        private UIImagez customCursor;

        // To detect when it needs to update slots
        public PokemonData lastSlot1, lastSlot2, lastSlot3, lastSlot4, lastSlot5, lastSlot6;

        // In OnInitialize, we place various UIElements onto our UIState (this class).
        // UIState classes have width and height equal to the full screen, because of this, usually we first define a UIElement that will act as the container for our UI.
        // We then place various other UIElement onto that container UIElement positioned relative to the container UIElement.
        public override void OnInitialize()
        {
            // Here we define our container UIElement. In DragableUIPanel.cs, you can see that DragableUIPanel is a UIPanel with a couple added features.
            // Here we define our container UIElement. In DragableUIPanel.cs, you can see that DragableUIPanel is a UIPanel with a couple added features.


            //pokemon icons


            // Next, we create another UIElement that we will place. Since we will be calling `mainPanel.Append(playButton);`, Left and Top are relative to the top left of the mainPanel UIElement. 
            // By properly nesting UIElements, we can position things relatively to each other easily.
            mainPanel = new DragableUIPanel();
            mainPanel.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            mainPanel.HAlign = 0.4f;
            mainPanel.VAlign = 0.65f;
            mainPanel.Width.Set(210, 0f);
            mainPanel.Height.Set(150f, 0f);

            toggleslots = new UIHoverImageButton(ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBall"), "Hide Party");
            toggleslots.Top.Set(265, 0f);
            toggleslots.Left.Set(121, 0f);
            toggleslots.OnClick += toggleSlotsFunc;
            Append(toggleslots);

            partytip = new UIText("Press Q to show held items");
            partytip.Top.Set(310, 0f);
            partytip.Left.Set(186, 0f);
            //Append(partytip);

            customslot1 = new CustomPartyItemSlot(1);
            customslot1.Top.Set(254, 0f);
            customslot1.Left.Set(158, 0f);
            Append(customslot1);

            customslot2 = new CustomPartyItemSlot(2);
            customslot2.Top.Set(254, 0f);
            customslot2.Left.Set(206, 0f);
            Append(customslot2);

            customslot3 = new CustomPartyItemSlot(3);
            customslot3.Top.Set(254, 0f);
            customslot3.Left.Set(254, 0f);
            Append(customslot3);

            customslot4 = new CustomPartyItemSlot(4);
            customslot4.Top.Set(254, 0f);
            customslot4.Left.Set(301, 0f);
            Append(customslot4);

            customslot5 = new CustomPartyItemSlot(5);
            customslot5.Top.Set(254, 0f);
            customslot5.Left.Set(349, 0f);
            Append(customslot5);

            customslot6 = new CustomPartyItemSlot(6);
            customslot6.Top.Set(254, 0f);
            customslot6.Left.Set(396, 0f);
            Append(customslot6);

            customCursor = new UIImagez(ModContent.GetTexture("Terramon/Pokemon/Empty"));
            Append(customCursor);

            partyslot1 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem);
            partyslot1.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            partyslot1.HAlign = 0.10f;
            partyslot1.VAlign = 0.15f;
            partyslot1.ValidItemFunc = item => item.IsAir || TerramonMod.PokeballFactory.GetEnum(item.modItem) !=
                                               TerramonMod.PokeballFactory.Pokebals.Nothing;
            mainPanel.Append(partyslot1);

            partyslot2 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem);
            partyslot2.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            partyslot2.HAlign = 0.5f;
            partyslot2.VAlign = 0.15f;
            partyslot2.ValidItemFunc = item => item.IsAir || TerramonMod.PokeballFactory.GetEnum(item.modItem) !=
                                               TerramonMod.PokeballFactory.Pokebals.Nothing;
            mainPanel.Append(partyslot2);

            partyslot3 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem);
            partyslot3.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            partyslot3.HAlign = 0.90f;
            partyslot3.VAlign = 0.15f;
            partyslot3.ValidItemFunc = item => item.IsAir || TerramonMod.PokeballFactory.GetEnum(item.modItem) !=
                                               TerramonMod.PokeballFactory.Pokebals.Nothing;
            mainPanel.Append(partyslot3);

            partyslot4 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem);
            partyslot4.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            partyslot4.HAlign = 0.10f;
            partyslot4.VAlign = 0.85f;
            partyslot4.ValidItemFunc = item => item.IsAir || TerramonMod.PokeballFactory.GetEnum(item.modItem) !=
                                               TerramonMod.PokeballFactory.Pokebals.Nothing;
            mainPanel.Append(partyslot4);

            partyslot5 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem);
            partyslot5.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            partyslot5.HAlign = 0.5f;
            partyslot5.VAlign = 0.85f;
            partyslot5.ValidItemFunc = item => item.IsAir || TerramonMod.PokeballFactory.GetEnum(item.modItem) !=
                                               TerramonMod.PokeballFactory.Pokebals.Nothing;
            mainPanel.Append(partyslot5);

            partyslot6 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem);
            partyslot6.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            partyslot6.HAlign = 0.90f;
            partyslot6.VAlign = 0.85f;
            partyslot6.ValidItemFunc = item => item.IsAir || TerramonMod.PokeballFactory.GetEnum(item.modItem) !=
                                               TerramonMod.PokeballFactory.Pokebals.Nothing;
            mainPanel.Append(partyslot6);

            //Append(mainPanel);

            partyslot1.OnItemPlaced += UpdateUI;
            partyslot2.OnItemPlaced += UpdateUI;
            partyslot3.OnItemPlaced += UpdateUI;
            partyslot4.OnItemPlaced += UpdateUI;
            partyslot5.OnItemPlaced += UpdateUI;
            partyslot6.OnItemPlaced += UpdateUI;
        }

        private byte compressAnimation;

        public bool compressing;
        public bool isReallyCompressed;

        private double startCompressAnimation;
        private double endCompressAnimation;

        private bool finishedIn;

        private void toggleSlotsFunc(UIMouseEvent evt, UIElement listeningElement)
        {
            if (compressing) return;
            isCompressed = !isCompressed;

            toggleslots.RotateTo(0)
                .RotateTo((float)Math.PI * 2f, 350f);

            toggleslots.SetHoverText("");

            customslot1.wasJustClicked = false;
            customslot2.wasJustClicked = false;
            customslot3.wasJustClicked = false;
            customslot4.wasJustClicked = false;
            customslot5.wasJustClicked = false;
            customslot6.wasJustClicked = false;
            toSwap = null;
            toSwapSlotNumber = 0;
            swapWithSlotNumber = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();

            if (toggleslots.IsMouseHovering && finishedIn) toggleslots.Texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBall");
            else if (isCompressed) toggleslots.Texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBallGreyed");

            toggleslots.OriginPoint = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBall").Size() / 2;

            UpdateSwap();

            if (isCompressed && !finishedIn)
            {
                if (compressAnimation == 0)
                {
                    Main.PlaySound(SoundID.MenuClose, Main.LocalPlayer.position);
                    toggleslots.Texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBallGreyed");
                    startCompressAnimation = gameTime.TotalGameTime.TotalSeconds;
                    endCompressAnimation = startCompressAnimation + 0.35;
                    compressing = true;
                    compressAnimation = 1;
                }

                if (compressAnimation == 1 && gameTime.TotalGameTime.TotalSeconds < endCompressAnimation)
                {
                    customslot1._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                    customslot2._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                    customslot3._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                    customslot4._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                    customslot5._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                    customslot6._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                }

                if (compressAnimation == 1 && gameTime.TotalGameTime.TotalSeconds < endCompressAnimation + 0.25)
                {
                    toggleslots.Left.Pixels = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 121, 406,
                        startCompressAnimation, endCompressAnimation + 0.25, Easing.OutExpo);
                    //toggleslots.Rotation = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0, (float)(Math.PI / 180) * 360,
                    //    startCompressAnimation, endCompressAnimation + 0.25, Easing.OutExpo);
                }

                if (compressAnimation == 1 && gameTime.TotalGameTime.TotalSeconds > endCompressAnimation + 0.25)
                {
                    customslot1._visibilityActive = 0;
                    customslot2._visibilityActive = 0;
                    customslot3._visibilityActive = 0;
                    customslot4._visibilityActive = 0;
                    customslot5._visibilityActive = 0;
                    customslot6._visibilityActive = 0;

                    compressing = false;
                    compressAnimation = 0;
                    startCompressAnimation = 0;
                    endCompressAnimation = 0;
                    finishedIn = true;
                    toggleslots.SetHoverText("Show Party");
                }
            }

            if (!isCompressed && finishedIn)
            {
                if (compressAnimation == 0)
                {
                    Main.PlaySound(SoundID.MenuOpen, Main.LocalPlayer.position);
                    toggleslots.Texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBall");
                    startCompressAnimation = gameTime.TotalGameTime.TotalSeconds;
                    endCompressAnimation = startCompressAnimation + 0.35;
                    compressing = true;
                    compressAnimation = 1;
                }

                if (compressAnimation == 1 && gameTime.TotalGameTime.TotalSeconds < endCompressAnimation)
                {
                    customslot1._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0f, 1f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                    customslot2._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0f, 1f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                    customslot3._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0f, 1f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                    customslot4._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0f, 1f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                    customslot5._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0f, 1f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                    customslot6._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0f, 1f,
                        startCompressAnimation, endCompressAnimation, Easing.None);
                }

                if (compressAnimation == 1 && gameTime.TotalGameTime.TotalSeconds < endCompressAnimation + 0.25)
                {
                    toggleslots.Left.Pixels = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 406, 121,
                        startCompressAnimation, endCompressAnimation + 0.25, Easing.OutExpo);
                    toggleslots.Rotation = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, (float)(Math.PI / 180) * 360, 0,
                        startCompressAnimation, endCompressAnimation + 0.25, Easing.OutExpo);
                }

                if (compressAnimation == 1 && gameTime.TotalGameTime.TotalSeconds > endCompressAnimation + 0.25)
                {
                    compressing = false;
                    compressAnimation = 0;
                    startCompressAnimation = 0;
                    endCompressAnimation = 0;
                    finishedIn = false;
                    toggleslots.SetHoverText("Hide Party");
                }
            }
        }

        internal void UpdateUI(Item item)
        {
            SaveButtonClicked();
            customslot1.UpdatePokemonContent();
            customslot2.UpdatePokemonContent();
            customslot3.UpdatePokemonContent();
            customslot4.UpdatePokemonContent();
            customslot5.UpdatePokemonContent();
            customslot6.UpdatePokemonContent();
        }

        public void UpdateSwap()
        {
            customslot1.UpdatePokemonContent(true);
            customslot2.UpdatePokemonContent(true);
            customslot3.UpdatePokemonContent(true);
            customslot4.UpdatePokemonContent(true);
            customslot5.UpdatePokemonContent(true);
            customslot6.UpdatePokemonContent(true);
        }

        public CustomPartyItemSlot GetSlott(int i)
        {
            if (i == 1) return customslot1;
            if (i == 2) return customslot2;
            if (i == 3) return customslot3;
            if (i == 4) return customslot4;
            if (i == 5) return customslot5;
            if (i == 6) return customslot6;
            return customslot1;
        }
        public PokemonData GetMpPartyDataInstance(int i)
        {
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (i == 1) return modPlayer.PartySlot1;
            if (i == 2) return modPlayer.PartySlot2;
            if (i == 3) return modPlayer.PartySlot3;
            if (i == 4) return modPlayer.PartySlot4;
            if (i == 5) return modPlayer.PartySlot5;
            if (i == 6) return modPlayer.PartySlot6;
            return modPlayer.PartySlot1;
        }

        //private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        //{
        //    Main.PlaySound(SoundID.MenuOpen);
        //    ModContent.GetInstance<TerramonMod>().UISidebar.CycleIndex = 0;
        //    TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
        //    Mod mod = ModContent.GetInstance<TerramonMod>();
        //    Player player = Main.LocalPlayer;
        //    // clear buffs
        //    player.ClearBuff(mod.BuffType(modPlayer.firstslotname + "Buff"));
        //    player.ClearBuff(mod.BuffType(modPlayer.secondslotname + "Buff"));
        //    player.ClearBuff(mod.BuffType(modPlayer.thirdslotname + "Buff"));
        //    player.ClearBuff(mod.BuffType(modPlayer.fourthslotname + "Buff"));
        //    player.ClearBuff(mod.BuffType(modPlayer.fifthslotname + "Buff"));
        //    player.ClearBuff(mod.BuffType(modPlayer.sixthslotname + "Buff"));
        //    if (partyslot1.Item.IsAir)
        //    {
        //        modPlayer.firstslotname = "*";
        //        modPlayer.PartySlot1 = null;
        //        ModContent.GetInstance<TerramonMod>().UISidebar.firstpkmn
        //            .TextureName = "Terramon/UI/SidebarParty/Empty";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.firstpkmn.HoverText = "";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.firstpkmn.Recalculate();
        //    }

        //    if (partyslot2.Item.IsAir)
        //    {
        //        modPlayer.secondslotname = "*";
        //        modPlayer.PartySlot2 = null;
        //        ModContent.GetInstance<TerramonMod>().UISidebar.secondpkmn
        //            .TextureName = "Terramon/UI/SidebarParty/Empty";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.secondpkmn.HoverText = "";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.secondpkmn.Recalculate();
        //    }

        //    if (partyslot3.Item.IsAir)
        //    {
        //        modPlayer.thirdslotname = "*";
        //        modPlayer.PartySlot3 = null;
        //        ModContent.GetInstance<TerramonMod>().UISidebar.thirdpkmn
        //            .TextureName = "Terramon/UI/SidebarParty/Empty";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.thirdpkmn.HoverText = "";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.thirdpkmn.Recalculate();
        //    }

        //    if (partyslot4.Item.IsAir)
        //    {
        //        modPlayer.fourthslotname = "*";
        //        modPlayer.PartySlot4 = null;
        //        ModContent.GetInstance<TerramonMod>().UISidebar.fourthpkmn
        //            .TextureName = "Terramon/UI/SidebarParty/Empty";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.fourthpkmn.HoverText = "";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.fourthpkmn.Recalculate();
        //    }

        //    if (partyslot5.Item.IsAir)
        //    {
        //        modPlayer.fifthslotname = "*";
        //        modPlayer.PartySlot5 = null;
        //        ModContent.GetInstance<TerramonMod>().UISidebar.fifthpkmn
        //            .TextureName = "Terramon/UI/SidebarParty/Empty";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.fifthpkmn.HoverText = "";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.fifthpkmn.Recalculate();
        //    }

        //    if (partyslot6.Item.IsAir)
        //    {
        //        modPlayer.sixthslotname = "*";
        //        modPlayer.PartySlot6 = null;
        //        ModContent.GetInstance<TerramonMod>().UISidebar.sixthpkmn
        //            .TextureName = "Terramon/UI/SidebarParty/Empty";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.sixthpkmn.HoverText = "";
        //        ModContent.GetInstance<TerramonMod>().UISidebar.sixthpkmn.Recalculate();
        //    }

        //    Main.NewText("Party Cleared.");
        //}

        private void SaveButtonClicked()
        {
            Main.PlaySound(SoundID.MenuOpen);
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            ModContent.GetInstance<TerramonMod>().UISidebar.CycleIndex = 0;
            Mod mod = ModContent.GetInstance<TerramonMod>();
            Player player = Main.LocalPlayer;
            modPlayer.CatchIndex = 0;
            // clear buffs
            player.ClearBuff(mod.BuffType(modPlayer.firstslotname + "Buff"));
            player.ClearBuff(mod.BuffType(modPlayer.secondslotname + "Buff"));
            player.ClearBuff(mod.BuffType(modPlayer.thirdslotname + "Buff"));
            player.ClearBuff(mod.BuffType(modPlayer.fourthslotname + "Buff"));
            player.ClearBuff(mod.BuffType(modPlayer.fifthslotname + "Buff"));
            player.ClearBuff(mod.BuffType(modPlayer.sixthslotname + "Buff"));
            if (partyslot1.Item.IsAir)
            {
                modPlayer.firstslotname = "*";
                //old_1 = "*";
                modPlayer.PartySlot1 = null;
                ModContent.GetInstance<TerramonMod>().UISidebar.firstpkmn
                    .TextureName = "Terramon/UI/SidebarParty/Empty";
                ModContent.GetInstance<TerramonMod>().UISidebar.firstpkmn.HoverText = "";
                ModContent.GetInstance<TerramonMod>().UISidebar.firstpkmn.Recalculate();
                ModContent.GetInstance<TerramonMod>().UISidebar.firstpkmnring.SetImage(ModContent.GetTexture("Terraria/Item_0"));
                ModContent.GetInstance<TerramonMod>().UISidebar.firstpkmnring.Recalculate();
            }

            if (partyslot2.Item.IsAir)
            {
                modPlayer.secondslotname = "*";
                //old_2 = "*";
                modPlayer.PartySlot2 = null;
                ModContent.GetInstance<TerramonMod>().UISidebar.secondpkmn
                    .TextureName = "Terramon/UI/SidebarParty/Empty";
                ModContent.GetInstance<TerramonMod>().UISidebar.secondpkmn.HoverText = "";
                ModContent.GetInstance<TerramonMod>().UISidebar.secondpkmn.Recalculate();
                ModContent.GetInstance<TerramonMod>().UISidebar.secondpkmnring.SetImage(ModContent.GetTexture("Terraria/Item_0"));
                ModContent.GetInstance<TerramonMod>().UISidebar.secondpkmnring.Recalculate();
            }

            if (partyslot3.Item.IsAir)
            {
                //old_3 = "*";
                modPlayer.thirdslotname = "*";
                modPlayer.PartySlot3 = null;
                ModContent.GetInstance<TerramonMod>().UISidebar.thirdpkmn
                    .TextureName = "Terramon/UI/SidebarParty/Empty";
                ModContent.GetInstance<TerramonMod>().UISidebar.thirdpkmn.HoverText = "";
                ModContent.GetInstance<TerramonMod>().UISidebar.thirdpkmn.Recalculate();
                ModContent.GetInstance<TerramonMod>().UISidebar.thirdpkmnring.SetImage(ModContent.GetTexture("Terraria/Item_0"));
                ModContent.GetInstance<TerramonMod>().UISidebar.thirdpkmnring.Recalculate();
            }

            if (partyslot4.Item.IsAir)
            {
                //old_4 = "*";
                modPlayer.fourthslotname = "*";
                modPlayer.PartySlot4 = null;
                ModContent.GetInstance<TerramonMod>().UISidebar.fourthpkmn
                    .TextureName = "Terramon/UI/SidebarParty/Empty";
                ModContent.GetInstance<TerramonMod>().UISidebar.fourthpkmn.HoverText = "";
                ModContent.GetInstance<TerramonMod>().UISidebar.fourthpkmn.Recalculate();
                ModContent.GetInstance<TerramonMod>().UISidebar.fourthpkmnring.SetImage(ModContent.GetTexture("Terraria/Item_0"));
                ModContent.GetInstance<TerramonMod>().UISidebar.fourthpkmnring.Recalculate();
            }

            if (partyslot5.Item.IsAir)
            {
                //old_5 = "*";
                modPlayer.fifthslotname = "*";
                modPlayer.PartySlot5 = null;
                ModContent.GetInstance<TerramonMod>().UISidebar.fifthpkmn
                    .TextureName = "Terramon/UI/SidebarParty/Empty";
                ModContent.GetInstance<TerramonMod>().UISidebar.fifthpkmn.HoverText = "";
                ModContent.GetInstance<TerramonMod>().UISidebar.fifthpkmn.Recalculate();
                ModContent.GetInstance<TerramonMod>().UISidebar.fifthpkmnring.SetImage(ModContent.GetTexture("Terraria/Item_0"));
                ModContent.GetInstance<TerramonMod>().UISidebar.fifthpkmnring.Recalculate();
            }

            if (partyslot6.Item.IsAir)
            {
                //old_6 = "*";
                modPlayer.sixthslotname = "*";
                modPlayer.PartySlot6 = null;
                ModContent.GetInstance<TerramonMod>().UISidebar.sixthpkmn
                    .TextureName = "Terramon/UI/SidebarParty/Empty";
                ModContent.GetInstance<TerramonMod>().UISidebar.sixthpkmn.HoverText = "";
                ModContent.GetInstance<TerramonMod>().UISidebar.sixthpkmn.Recalculate();
                ModContent.GetInstance<TerramonMod>().UISidebar.sixthpkmnring.SetImage(ModContent.GetTexture("Terraria/Item_0"));
                ModContent.GetInstance<TerramonMod>().UISidebar.sixthpkmnring.Recalculate();
            }

            if (!ModContent.GetInstance<TerramonMod>().PartySlots.partyslot1.Item.IsAir)
            {
                //var type = TerramonMod.GetPokeballType(partyslot1.Item.modItem);
                var pokeballCaught = (BaseCaughtClass)partyslot1.Item.modItem;
                //modPlayer.firstslottype = pokeballCaught.PokemonNPC;
                //old_1 = pokeballCaught.PokemonName ;
                modPlayer.firstslotname = pokeballCaught.PokemonName;
                modPlayer.PartySlot1 = new PokemonData(pokeballCaught.Save());
            }

            if (!ModContent.GetInstance<TerramonMod>().PartySlots.partyslot2.Item.IsAir)
            {
                //var type = TerramonMod.GetPokeballType(partyslot1.Item.modItem);
                var pokeballCaught = (BaseCaughtClass)partyslot2.Item.modItem;
                //modPlayer.secondslottype = pokeballCaught.PokemonNPC;
                //old_2 = pokeballCaught.PokemonName;
                modPlayer.secondslotname = pokeballCaught.PokemonName;
                modPlayer.PartySlot2 = new PokemonData(pokeballCaught.Save());
            }

            if (!ModContent.GetInstance<TerramonMod>().PartySlots.partyslot3.Item.IsAir)
            {
                //var type = TerramonMod.GetPokeballType(partyslot1.Item.modItem);
                var pokeballCaught = (BaseCaughtClass)partyslot3.Item.modItem;
                //modPlayer.thirdslottype = pokeballCaught.PokemonNPC;
                //old_3 = pokeballCaught.PokemonName;
                modPlayer.thirdslotname = pokeballCaught.PokemonName;
                modPlayer.PartySlot3 = new PokemonData(pokeballCaught.Save());
            }

            if (!ModContent.GetInstance<TerramonMod>().PartySlots.partyslot4.Item.IsAir)
            {
                //var type = TerramonMod.GetPokeballType(partyslot1.Item.modItem);
                var pokeballCaught = (BaseCaughtClass)partyslot4.Item.modItem;
                //modPlayer.fourthslottype = pokeballCaught.PokemonNPC;
                //old_4 = pokeballCaught.PokemonName;
                modPlayer.fourthslotname = pokeballCaught.PokemonName;
                modPlayer.PartySlot4 = new PokemonData(pokeballCaught.Save());
            }

            if (!ModContent.GetInstance<TerramonMod>().PartySlots.partyslot5.Item.IsAir)
            {
                //var type = TerramonMod.GetPokeballType(partyslot1.Item.modItem);
                var pokeballCaught = (BaseCaughtClass)partyslot5.Item.modItem;
                //modPlayer.fifthslottype = pokeballCaught.PokemonNPC;
                //old_5 = pokeballCaught.PokemonName;
                modPlayer.fifthslotname = pokeballCaught.PokemonName;
                modPlayer.PartySlot5 = new PokemonData(pokeballCaught.Save());
            }

            if (!ModContent.GetInstance<TerramonMod>().PartySlots.partyslot6.Item.IsAir)
            {
                //var type = TerramonMod.GetPokeballType(partyslot1.Item.modItem);
                var pokeballCaught = (BaseCaughtClass)partyslot6.Item.modItem;
                //modPlayer.sixthslottype = pokeballCaught.PokemonNPC;
                //old_6 = pokeballCaught.PokemonName;
                modPlayer.sixthslotname = pokeballCaught.PokemonName;
                modPlayer.PartySlot6 = new PokemonData(pokeballCaught.Save());
            }

            //Main.NewText("Party Saved!");
        }
    }
    public class CustomPartyItemSlot : UIElement
    {
        private Texture2D _texture;

        private int _slot;

        private PokemonData stored;

        public float _visibilityActive = 1f;

        public Color drawcolor = Color.White;

        private UIImagez minisprite;

        public bool wasJustClicked = false;

        private string HoverText;

        public CustomPartyItemSlot(int slot)
        {
            _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBg");
            _slot = slot;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
        }

        public override void OnInitialize()
        {
            Texture2D minispriteTexture = ModContent.GetTexture("Terramon/Pokemon/Empty");
            minisprite = new UIImagez(minispriteTexture);
            minisprite.Left.Set(-8, 0f);
            minisprite.Top.Set(-14, 0f);
            Append(minisprite);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (ContainsPoint(Main.MouseScreen)) Main.LocalPlayer.mouseInterface = true;

            // Fainted Pokemon appear semi transparent
            if (stored != null)
            {
                if (stored.HP != 0) minisprite._visibilityActive = _visibilityActive;
                else minisprite._visibilityActive = _visibilityActive * 0.4f;
            }

            if (stored == null) return;

            if (HoldingUsableItem() && wasJustClicked)
            {
                wasJustClicked = false;
                PartySlots.toSwap = null;
                PartySlots.toSwapSlotNumber = 0;
                PartySlots.swapWith = null;
                PartySlots.swapWithSlotNumber = 0;
            }

            if (HoldingPotion() && CanUsePotion(stored)) { _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBgClicked"); return; }
            if (HoldingSuperPotion() && CanUseSuperPotion(stored)) { _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBgClicked"); return; }
            if (HoldingHyperPotion() && CanUseHyperPotion(stored)) { _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBgClicked"); return; }
            if (HoldingMaxPotion() && CanUseMaxPotion(stored)) { _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBgClicked"); return; }
            if (HoldingFullRestore() && CanUseFullRestore(stored)) { _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBgClicked"); return; }
            if (HoldingRevive() && CanUseRevive(stored)) { _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBgClicked"); return; }
            if (HoldingMaxRevive() && CanUseMaxRevive(stored)) { _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBgClicked"); return; }

            if (wasJustClicked)
            {
                _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBgClicked");
            }
            else
            {
                _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBg");
            }
        }

        public bool CanUsePotion(PokemonData mon) { if (HoldingPotion() && mon.HP != mon.MaxHP && mon.HP != 0) return true; else return false; }
        public bool CanUseSuperPotion(PokemonData mon) { if (HoldingSuperPotion() && mon.HP != mon.MaxHP && mon.HP != 0) return true; else return false; }
        public bool CanUseHyperPotion(PokemonData mon) { if (HoldingHyperPotion() && mon.HP != mon.MaxHP && mon.HP != 0) return true; else return false; }
        public bool CanUseMaxPotion(PokemonData mon) { if (HoldingMaxPotion() && mon.HP != mon.MaxHP && mon.HP != 0) return true; else return false; }
        public bool CanUseFullRestore(PokemonData mon) { if (HoldingFullRestore() && mon.HP != mon.MaxHP && mon.HP != 0) return true; else return false; }
        public bool CanUseRevive(PokemonData mon) { if (HoldingRevive() && mon.HP == 0) return true; else return false; }
        public bool CanUseMaxRevive(PokemonData mon) { if (HoldingMaxRevive() && mon.HP == 0) return true; else return false; }

        public bool HoldingUsableItem()
        {
            if (HoldingPotion() ||
                HoldingSuperPotion() ||
                HoldingHyperPotion() ||
                HoldingMaxPotion() ||
                HoldingFullRestore() ||
                HoldingRevive() ||
                HoldingMaxRevive()) return true;
            return false;
        }

        public bool HoldingPotion()
        {
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.Battle != null) return false;
            if (Main.mouseItem.type == ModContent.ItemType<Items.MiscItems.Medication.Potion>()) return true;
            return false;
        }

        public bool HoldingSuperPotion()
        {
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.Battle != null) return false;
            if (Main.mouseItem.type == ModContent.ItemType<Items.MiscItems.Medication.SuperPotion>()) return true;
            return false;
        }

        public bool HoldingHyperPotion()
        {
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.Battle != null) return false;
            if (Main.mouseItem.type == ModContent.ItemType<Items.MiscItems.Medication.HyperPotion>()) return true;
            return false;
        }

        public bool HoldingMaxPotion()
        {
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.Battle != null) return false;
            if (Main.mouseItem.type == ModContent.ItemType<Items.MiscItems.Medication.MaxPotion>()) return true;
            return false;
        }

        public bool HoldingFullRestore()
        {
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.Battle != null) return false;
            if (Main.mouseItem.type == ModContent.ItemType<Items.MiscItems.Medication.FullRestore>()) return true;
            return false;
        }
        public bool HoldingRevive()
        {
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.Battle != null) return false;
            if (Main.mouseItem.type == ModContent.ItemType<Items.MiscItems.Medication.Revive>()) return true;
            return false;
        }
        public bool HoldingMaxRevive()
        {
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (modPlayer.Battle != null) return false;
            if (Main.mouseItem.type == ModContent.ItemType<Items.MiscItems.Medication.MaxRevive>()) return true;
            return false;
        }

        public void UpdatePokemonContent(bool afterSwap = false)
        {
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (_slot == 1) stored = modPlayer.PartySlot1;
            if (_slot == 2) stored = modPlayer.PartySlot2;
            if (_slot == 3) stored = modPlayer.PartySlot3;
            if (_slot == 4) stored = modPlayer.PartySlot4;
            if (_slot == 5) stored = modPlayer.PartySlot5;
            if (_slot == 6) stored = modPlayer.PartySlot6;

            if (stored == null)
            {
                HoverText = "";
                minisprite.SetImage(ModContent.GetTexture("Terramon/Pokemon/Empty"));
                _texture = ModContent.GetTexture("Terramon/UI/SidebarParty/PartySlotBgInactive");
                return;
            }

            if (stored.HP != 0) HoverText =
                $"[c/a1a1a1:{stored.PokemonName} (Lv. {stored.Level})]" +
                $" \nHP: {stored.HP}/{stored.MaxHP}" +
                $" \nEXP: {stored.Exp}/{stored.ExpToNext}";
            else
                HoverText =
                $"[c/a1a1a1:{stored.PokemonName} (Lv. {stored.Level})] [c/ff4f4f:(Fainted)]" +
                $" \nHP: {stored.HP}/{stored.MaxHP}" +
                $" \nEXP: {stored.Exp}/{stored.ExpToNext}";

            string minispritePath;
            if (!stored.IsShiny) minispritePath = "Terramon/Minisprites/Regular/mini" + stored.Pokemon;
            else minispritePath = "Terramon/Minisprites/Regular/mini" + stored.Pokemon + "_Shiny";

            minisprite.SetImage(ModContent.GetTexture(minispritePath));
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (IsMouseHovering && !ModContent.GetInstance<TerramonMod>().PartySlots.isCompressed) Main.hoverItemName = HoverText;
            if (IsMouseHovering && Main.mouseLeft && !ModContent.GetInstance<TerramonMod>().PartySlots.isCompressed && !HoldingUsableItem())
            {
                if (Main.mouseLeftRelease)
                {
                    // Swap pokemon
                    if (PartySlots.toSwap != null)
                    {
                        PartySlots.swapWith = stored;
                        PartySlots.swapWithSlotNumber = _slot;

                        if (PartySlots.swapWith == null)
                        {
                            ModContent.GetInstance<TerramonMod>().PartySlots.customslot1.wasJustClicked = false;
                            ModContent.GetInstance<TerramonMod>().PartySlots.customslot2.wasJustClicked = false;
                            ModContent.GetInstance<TerramonMod>().PartySlots.customslot3.wasJustClicked = false;
                            ModContent.GetInstance<TerramonMod>().PartySlots.customslot4.wasJustClicked = false;
                            ModContent.GetInstance<TerramonMod>().PartySlots.customslot5.wasJustClicked = false;
                            ModContent.GetInstance<TerramonMod>().PartySlots.customslot6.wasJustClicked = false;
                            PartySlots.toSwap = null;
                            PartySlots.toSwapSlotNumber = 0;
                            PartySlots.swapWithSlotNumber = 0;
                            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
                            return;
                        }

                        if (PartySlots.swapWithSlotNumber == PartySlots.toSwapSlotNumber)
                        {
                            wasJustClicked = false;
                            PartySlots.toSwap = null;
                            PartySlots.toSwapSlotNumber = 0;
                            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
                            return;
                        }

                        ModContent.GetInstance<TerramonMod>().PartySlots.customslot1.wasJustClicked = false;
                        ModContent.GetInstance<TerramonMod>().PartySlots.customslot2.wasJustClicked = false;
                        ModContent.GetInstance<TerramonMod>().PartySlots.customslot3.wasJustClicked = false;
                        ModContent.GetInstance<TerramonMod>().PartySlots.customslot4.wasJustClicked = false;
                        ModContent.GetInstance<TerramonMod>().PartySlots.customslot5.wasJustClicked = false;
                        ModContent.GetInstance<TerramonMod>().PartySlots.customslot6.wasJustClicked = false;

                        PokemonData a;
                        PokemonData b;

                        a = PartySlots.toSwap;
                        b = PartySlots.swapWith;

                        PartySlots.toSwap = b;
                        PartySlots.swapWith = a;

                        TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                        if (PartySlots.toSwapSlotNumber == 1)
                        {
                            modPlayer.PartySlot1 = b;
                            modPlayer.firstslotname = b.Pokemon;
                        }
                        if (PartySlots.toSwapSlotNumber == 2)
                        {
                            modPlayer.PartySlot2 = b;
                            modPlayer.secondslotname = b.Pokemon;
                        }
                        if (PartySlots.toSwapSlotNumber == 3)
                        {
                            modPlayer.PartySlot3 = b;
                            modPlayer.thirdslotname = b.Pokemon;
                        }
                        if (PartySlots.toSwapSlotNumber == 4)
                        {
                            modPlayer.PartySlot4 = b;
                            modPlayer.fourthslotname = b.Pokemon;
                        }
                        if (PartySlots.toSwapSlotNumber == 5)
                        {
                            modPlayer.PartySlot5 = b;
                            modPlayer.fifthslotname = b.Pokemon;
                        }
                        if (PartySlots.toSwapSlotNumber == 6)
                        {
                            modPlayer.PartySlot6 = b;
                            modPlayer.sixthslotname = b.Pokemon;
                        }
                        if (PartySlots.swapWithSlotNumber == 1)
                        {
                            modPlayer.PartySlot1 = a;
                            modPlayer.firstslotname = a.Pokemon;
                        }
                        if (PartySlots.swapWithSlotNumber == 2)
                        {
                            modPlayer.PartySlot2 = a;
                            modPlayer.secondslotname = a.Pokemon;
                        }
                        if (PartySlots.swapWithSlotNumber == 3)
                        {
                            modPlayer.PartySlot3 = a;
                            modPlayer.thirdslotname = a.Pokemon;
                        }
                        if (PartySlots.swapWithSlotNumber == 4)
                        {
                            modPlayer.PartySlot4 = a;
                            modPlayer.fourthslotname = a.Pokemon;
                        }
                        if (PartySlots.swapWithSlotNumber == 5)
                        {
                            modPlayer.PartySlot5 = a;
                            modPlayer.fifthslotname = a.Pokemon;
                        }
                        if (PartySlots.swapWithSlotNumber == 6)
                        {
                            modPlayer.PartySlot6 = a;
                            modPlayer.sixthslotname = a.Pokemon;
                        }

                        Main.PlaySound(SoundID.Tink, Main.LocalPlayer.position);

                        // Clear pokemon
                        var pokeBuff = ModContent.GetInstance<TerramonMod>().BuffType(nameof(PokemonBuff));
                        if (Main.LocalPlayer.HasBuff(pokeBuff))
                        {
                            for (int i = 0; i < 18; i++)
                            {
                                Dust.NewDust(Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.position, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.width, Main.projectile[modPlayer.ActivePetId].modProjectile.projectile.height,
                                    ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
                            }
                            Main.LocalPlayer.ClearBuff(pokeBuff);
                            modPlayer.ActivePetName = string.Empty;
                            modPlayer.ActivePartySlot = -1;
                        }

                        ModContent.GetInstance<TerramonMod>().PartySlots.UpdateSwap();
                        if (a.HP == 0) minisprite._visibilityActive = _visibilityActive * 0.4f;
                        if (b.HP == 0) ModContent.GetInstance<TerramonMod>().PartySlots.GetSlott(PartySlots.toSwapSlotNumber).minisprite._visibilityActive = _visibilityActive * 0.4f;

                        PartySlots.toSwap = null;
                        PartySlots.toSwapSlotNumber = 0;
                        PartySlots.swapWith = null;
                        PartySlots.swapWithSlotNumber = 0;
                    }
                    else
                    {
                        if (stored == null)
                        {
                            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
                            return;
                        }
                        if (!wasJustClicked) wasJustClicked = true;
                        PartySlots.toSwap = stored;
                        PartySlots.toSwapSlotNumber = _slot;
                        Main.PlaySound(SoundID.MenuTick, Main.LocalPlayer.position);
                    }
                }
            }

            // Handle item usage on pokemon in slots
            if (stored != null && HoldingUsableItem() && IsMouseHovering && Main.mouseLeft && !ModContent.GetInstance<TerramonMod>().PartySlots.isCompressed)
            {
                if (Main.mouseLeftRelease)
                {
                    if (HoldingPotion())
                    {
                        if (!CanUsePotion(stored))
                        {
                            Main.NewText($"It'll have no effect on {stored.PokemonName}.", Color.LightGray);
                            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
                            return;
                        }
                        Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 13);
                        Main.mouseItem.stack--;
                        Main.NewText($"Restored {stored.Heal(20)} HP to {stored.PokemonName}!", Color.LightGreen);
                    }
                    if (HoldingSuperPotion())
                    {
                        if (!CanUseSuperPotion(stored))
                        {
                            Main.NewText($"It'll have no effect on {stored.PokemonName}.", Color.LightGray);
                            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
                            return;
                        }
                        Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 13);
                        Main.mouseItem.stack--;
                        Main.NewText($"Restored {stored.Heal(50)} HP to {stored.PokemonName}!", Color.LightGreen);
                    }
                    if (HoldingHyperPotion())
                    {
                        if (!CanUseHyperPotion(stored))
                        {
                            Main.NewText($"It'll have no effect on {stored.PokemonName}.", Color.LightGray);
                            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
                            return;
                        }
                        Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 13);
                        Main.mouseItem.stack--;
                        Main.NewText($"Restored {stored.Heal(120)} HP to {stored.PokemonName}!", Color.LightGreen);
                    }
                    if (HoldingMaxPotion())
                    {
                        if (!CanUseMaxPotion(stored))
                        {
                            Main.NewText($"It'll have no effect on {stored.PokemonName}.", Color.LightGray);
                            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
                            return;
                        }
                        Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 13);
                        Main.mouseItem.stack--;
                        stored.Heal(ModContent.GetInstance<TerramonMod>().PartySlots.GetMpPartyDataInstance(_slot).MaxHP);
                        Main.NewText($"Fully restored {stored.PokemonName}'s HP!", Color.LightGreen);
                    }
                    if (HoldingFullRestore())
                    {
                        if (!CanUseFullRestore(stored))
                        {
                            Main.NewText($"It'll have no effect on {stored.PokemonName}.", Color.LightGray);
                            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
                            return;
                        }
                        Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 13);
                        Main.mouseItem.stack--;
                        stored.Heal(ModContent.GetInstance<TerramonMod>().PartySlots.GetMpPartyDataInstance(_slot).MaxHP);
                        Main.NewText($"Fully restored {stored.PokemonName}'s HP and cured all status conditions!", Color.LightGreen);
                    }
                    if (HoldingRevive())
                    {
                        if (!CanUseRevive(stored))
                        {
                            Main.NewText($"It'll have no effect on {stored.PokemonName}.", Color.LightGray);
                            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
                            return;
                        }
                        Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 4);
                        Main.mouseItem.stack--;
                        stored.Heal(ModContent.GetInstance<TerramonMod>().PartySlots.GetMpPartyDataInstance(_slot).MaxHP / 2);
                        stored.Fainted = false;
                        Main.NewText($"{stored.PokemonName} was revived and healed to half of its Max HP!", new Color(250, 210, 110));
                    }
                    if (HoldingMaxRevive())
                    {
                        if (!CanUseMaxRevive(stored))
                        {
                            Main.NewText($"It'll have no effect on {stored.PokemonName}.", Color.LightGray);
                            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
                            return;
                        }
                        Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 4);
                        Main.mouseItem.stack--;
                        stored.Heal(ModContent.GetInstance<TerramonMod>().PartySlots.GetMpPartyDataInstance(_slot).MaxHP);
                        stored.Fainted = false;
                        Main.NewText($"{stored.PokemonName} was revived and healed to its Max HP!", new Color(250, 210, 110));
                    }
                }
            }

            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - 0.85f) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: 0.85f, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
        }
    }
}