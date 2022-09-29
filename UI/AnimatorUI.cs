using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.Graphics;
using Razorwing.Framework.Graphics.Transforms;
using Razorwing.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Pokemon;
using Terramon.UI.Pokedex;
using Terramon.UI.SidebarParty;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using static Terramon.UI.Pokedex.DexArrow;
using Microsoft.Xna.Framework.Input;
using Terramon.Players;

namespace Terramon.UI
{
    public class AnimatorUI : UIState
    {
        public static bool Visible = true;

        public UIPanel headingPanel, mainPanel, spriteContainer;
        public SummarySprite overworldSprite;
        public SummaryImage gender, caughtBall, shiny, status;

        public UIText lv, name, trainerMemoHeader, memo1, memo2, memo3;

        private string target = "Charmeleon";

        public UIImage pokedexTexture;
        public UIText pokedexText, pokedexShowingNumbers, pokedexPercentComplete;

        public int[] range = new int[18];

        public DexSlot dexSlot1, dexSlot2, dexSlot3, dexSlot4, dexSlot5, dexSlot6, dexSlot7, dexSlot8, dexSlot9, dexSlot10, dexSlot11, dexSlot12, dexSlot13, dexSlot14, dexSlot15, dexSlot16, dexSlot17, dexSlot18;
        public DexSlotHitbox dexSlot1x, dexSlot2x, dexSlot3x, dexSlot4x, dexSlot5x, dexSlot6x, dexSlot7x, dexSlot8x, dexSlot9x, dexSlot10x, dexSlot11x, dexSlot12x, dexSlot13x, dexSlot14x, dexSlot15x, dexSlot16x, dexSlot17x, dexSlot18x;
        public DexArrow upArrow, downArrow;

        public int[] hoveredHitboxes = new int[18];

        public override void OnInitialize()
        {
            mainPanel = new UIPanel();
            mainPanel.BackgroundColor = new Color(28, 36, 66) * 0.95f;
            mainPanel.Width.Set(440, 0f);
            mainPanel.Height.Set(310, 0f);
            mainPanel.HAlign = 0.5f;
            mainPanel.VAlign = 0.5f;

            //Append(mainPanel);

            spriteContainer = new UIPanel();
            spriteContainer.BackgroundColor = new Color(20, 25, 46) * 1f;
            spriteContainer.Width.Set(148, 0f);
            spriteContainer.Height.Set(110, 0f);
            spriteContainer.HAlign = 0.1f;
            spriteContainer.VAlign = 0.46f;

            gender = new SummaryImage(ModContent.GetTexture("Terramon/UI/Summary/" + "Male"), "Male");
            gender.Top.Set(-4, 0f);
            gender.Left.Set(-14, 1f);

            //spriteContainer.Append(gender);

            caughtBall = new SummaryImage(ModContent.GetTexture("Terramon/Minisprites/Ball1"), "Caught in a Poké Ball");
            caughtBall.Top.Set(-12, 1f);
            caughtBall.Left.Set(-14, 1f);

            //spriteContainer.Append(caughtBall);

            Texture2D overworldTexture = ModContent.GetTexture($"Terramon/Pokemon/FirstGeneration/Normal/{target}/{target}");
            overworldSprite = new SummarySprite(overworldTexture);
            overworldSprite.HAlign = 0.5f;
            overworldSprite.VAlign = 0.5f;

            //spriteContainer.Append(overworldSprite);

            name = new UIText("Charmeleon", 0.55f, true);
            name.Top.Set(-41, 0f);
            name.Left.Set(-2, 0f);

            lv = new UIText("Lv 16", 0.75f, false);

            //spriteContainer.Append(name);
            //spriteContainer.Append(lv);

            trainerMemoHeader = new UIText("Trainer Memo", 0.35f, true);
            trainerMemoHeader.HAlign = 0.5f;
            trainerMemoHeader.Top.Set(34, 1f);
            //spriteContainer.Append(trainerMemoHeader);

            memo1 = new UIText("Adamant nature.", 0.3f, true);
            memo1.HAlign = 0.5f;
            memo1.Top.Set(54, 1f);
            //spriteContainer.Append(memo1);

            memo2 = new UIText("Caught in the cavern layer", 0.3f, true);
            memo2.HAlign = 0.5f;
            memo2.Top.Set(68, 1f);
            //spriteContainer.Append(memo2);

            memo3 = new UIText("at Lv 5.", 0.3f, true);
            memo3.HAlign = 0.5f;
            memo3.Top.Set(82, 1f);
            //spriteContainer.Append(memo3);

            //mainPanel.Append(spriteContainer);

            headingPanel = new UIPanel();
            headingPanel.BackgroundColor = new Color(63, 82, 151) * 1f;
            headingPanel.Width.Set(340, 0f);
            headingPanel.Height.Set(64, 0f);
            headingPanel.HAlign = 0.5f;
            headingPanel.Top.Set(-38, 0f);

            UIText text = new UIText("Pokémon Summary", 0.8f, true);
            text.HAlign = 0.5f;
            text.VAlign = 0.5f;
            //headingPanel.Append(text);

            //mainPanel.Append(headingPanel);

            // == Pokédex UI ==

            pokedexTexture = new UIImage(ModContent.GetTexture("Terramon/UI/Pokedex/Pokedex"));
            pokedexTexture.HAlign = 0.5f;
            pokedexTexture.VAlign = 3.8f;
            Append(pokedexTexture);

            pokedexText = new UIText("Pokédex", 0.65f, true);
            pokedexText.HAlign = 0.5f;
            pokedexText.Top.Set(50, 0f);
            pokedexTexture.Append(pokedexText);

            pokedexShowingNumbers = new UIText("Showing No. 001-018", 1f, false);
            pokedexShowingNumbers.HAlign = 0.185f;
            pokedexShowingNumbers.Top.Set(30, 0f);
            pokedexTexture.Append(pokedexShowingNumbers);

            pokedexPercentComplete = new UIText("100% Completed", 1f, false);
            pokedexPercentComplete.HAlign = 0.795f;
            pokedexPercentComplete.Top.Set(30, 0f);
            pokedexTexture.Append(pokedexPercentComplete);

            upArrow = new DexArrow(DexArrowDirection.Up);
            upArrow.Left.Set(588, 0f);
            upArrow.Top.Set(135, 0f);
            pokedexTexture.Append(upArrow);

            downArrow = new DexArrow(DexArrowDirection.Down);
            downArrow.Left.Set(588, 0f);
            downArrow.Top.Set(250, 0f);
            pokedexTexture.Append(downArrow);

            dexSlot1 = new DexSlot(1);
            dexSlot1.Top.Set(84, 0f);
            dexSlot1.Left.Set(129, 0f);
            pokedexTexture.Append(dexSlot1);

            dexSlot1x = new DexSlotHitbox(1);
            dexSlot1x.Left.Set(131, 0f);
            dexSlot1x.Top.Set(103, 0f);
            pokedexTexture.Append(dexSlot1x);

            dexSlot2 = new DexSlot(2);
            dexSlot2.Top.Set(84, 0f);
            dexSlot2.Left.Set(204, 0f);
            pokedexTexture.Append(dexSlot2);

            dexSlot2x = new DexSlotHitbox(2);
            dexSlot2x.Left.Set(206, 0f);
            dexSlot2x.Top.Set(103, 0f);
            pokedexTexture.Append(dexSlot2x);

            dexSlot3 = new DexSlot(3);
            dexSlot3.Top.Set(84, 0f);
            dexSlot3.Left.Set(279, 0f);
            pokedexTexture.Append(dexSlot3);

            dexSlot3x = new DexSlotHitbox(3);
            dexSlot3x.Left.Set(281, 0f);
            dexSlot3x.Top.Set(103, 0f);
            pokedexTexture.Append(dexSlot3x);

            dexSlot4 = new DexSlot(4);
            dexSlot4.Top.Set(84, 0f);
            dexSlot4.Left.Set(354, 0f);
            pokedexTexture.Append(dexSlot4);

            dexSlot4x = new DexSlotHitbox(4);
            dexSlot4x.Left.Set(356, 0f);
            dexSlot4x.Top.Set(103, 0f);
            pokedexTexture.Append(dexSlot4x);

            dexSlot5 = new DexSlot(5);
            dexSlot5.Top.Set(84, 0f);
            dexSlot5.Left.Set(429, 0f);
            pokedexTexture.Append(dexSlot5);

            dexSlot5x = new DexSlotHitbox(5);
            dexSlot5x.Left.Set(431, 0f);
            dexSlot5x.Top.Set(103, 0f);
            pokedexTexture.Append(dexSlot5x);

            dexSlot6 = new DexSlot(6);
            dexSlot6.Top.Set(84, 0f);
            dexSlot6.Left.Set(504, 0f);
            pokedexTexture.Append(dexSlot6);

            dexSlot6x = new DexSlotHitbox(6);
            dexSlot6x.Left.Set(506, 0f);
            dexSlot6x.Top.Set(103, 0f);
            pokedexTexture.Append(dexSlot6x);

            dexSlot7 = new DexSlot(7);
            dexSlot7.Top.Set(158, 0f); // + 74
            dexSlot7.Left.Set(129, 0f);
            pokedexTexture.Append(dexSlot7);

            dexSlot7x = new DexSlotHitbox(7);
            dexSlot7x.Left.Set(131, 0f);
            dexSlot7x.Top.Set(177, 0f);
            pokedexTexture.Append(dexSlot7x);

            dexSlot8 = new DexSlot(8);
            dexSlot8.Top.Set(158, 0f);
            dexSlot8.Left.Set(204, 0f);
            pokedexTexture.Append(dexSlot8);

            dexSlot8x = new DexSlotHitbox(8);
            dexSlot8x.Left.Set(206, 0f);
            dexSlot8x.Top.Set(177, 0f);
            pokedexTexture.Append(dexSlot8x);

            dexSlot9 = new DexSlot(9);
            dexSlot9.Top.Set(158, 0f);
            dexSlot9.Left.Set(279, 0f);
            pokedexTexture.Append(dexSlot9);

            dexSlot9x = new DexSlotHitbox(9);
            dexSlot9x.Left.Set(281, 0f);
            dexSlot9x.Top.Set(177, 0f);
            pokedexTexture.Append(dexSlot9x);

            dexSlot10 = new DexSlot(10);
            dexSlot10.Top.Set(158, 0f);
            dexSlot10.Left.Set(354, 0f);
            pokedexTexture.Append(dexSlot10);

            dexSlot10x = new DexSlotHitbox(10);
            dexSlot10x.Left.Set(356, 0f);
            dexSlot10x.Top.Set(177, 0f);
            pokedexTexture.Append(dexSlot10x);

            dexSlot11 = new DexSlot(11);
            dexSlot11.Top.Set(158, 0f);
            dexSlot11.Left.Set(429, 0f);
            pokedexTexture.Append(dexSlot11);

            dexSlot11x = new DexSlotHitbox(11);
            dexSlot11x.Left.Set(431, 0f);
            dexSlot11x.Top.Set(177, 0f);
            pokedexTexture.Append(dexSlot11x);

            dexSlot12 = new DexSlot(12);
            dexSlot12.Top.Set(158, 0f);
            dexSlot12.Left.Set(504, 0f);
            pokedexTexture.Append(dexSlot12);

            dexSlot12x = new DexSlotHitbox(12);
            dexSlot12x.Left.Set(506, 0f);
            dexSlot12x.Top.Set(177, 0f);
            pokedexTexture.Append(dexSlot12x);

            dexSlot13 = new DexSlot(13);
            dexSlot13.Top.Set(232, 0f); // + 74
            dexSlot13.Left.Set(129, 0f);
            pokedexTexture.Append(dexSlot13);

            dexSlot13x = new DexSlotHitbox(13);
            dexSlot13x.Left.Set(131, 0f);
            dexSlot13x.Top.Set(250, 0f);
            pokedexTexture.Append(dexSlot13x);

            dexSlot14 = new DexSlot(14);
            dexSlot14.Top.Set(232, 0f);
            dexSlot14.Left.Set(204, 0f);
            pokedexTexture.Append(dexSlot14);

            dexSlot14x = new DexSlotHitbox(14);
            dexSlot14x.Left.Set(206, 0f);
            dexSlot14x.Top.Set(250, 0f);
            pokedexTexture.Append(dexSlot14x);

            dexSlot15 = new DexSlot(15);
            dexSlot15.Top.Set(232, 0f);
            dexSlot15.Left.Set(279, 0f);
            pokedexTexture.Append(dexSlot15);

            dexSlot15x = new DexSlotHitbox(15);
            dexSlot15x.Left.Set(281, 0f);
            dexSlot15x.Top.Set(250, 0f);
            pokedexTexture.Append(dexSlot15x);

            dexSlot16 = new DexSlot(16);
            dexSlot16.Top.Set(232, 0f);
            dexSlot16.Left.Set(354, 0f);
            pokedexTexture.Append(dexSlot16);

            dexSlot16x = new DexSlotHitbox(16);
            dexSlot16x.Left.Set(356, 0f);
            dexSlot16x.Top.Set(250, 0f);
            pokedexTexture.Append(dexSlot16x);

            dexSlot17 = new DexSlot(17);
            dexSlot17.Top.Set(232, 0f);
            dexSlot17.Left.Set(429, 0f);
            pokedexTexture.Append(dexSlot17);

            dexSlot17x = new DexSlotHitbox(17);
            dexSlot17x.Left.Set(431, 0f);
            dexSlot17x.Top.Set(250, 0f);
            pokedexTexture.Append(dexSlot17x);

            dexSlot18 = new DexSlot(18);
            dexSlot18.Top.Set(232, 0f);
            dexSlot18.Left.Set(504, 0f);
            pokedexTexture.Append(dexSlot18);

            dexSlot18x = new DexSlotHitbox(18);
            dexSlot18x.Left.Set(506, 0f);
            dexSlot18x.Top.Set(250, 0f);
            pokedexTexture.Append(dexSlot18x);

            DexInit();

            // == End Pokédex UI ==

            Append(TerramonMod.ZoomAnimator = new Animator());

            base.OnInitialize();
        }

        public void DexInit()
        {
            // Get range, starts at 1-18.

            range[0] = 1;
            range[1] = 2;
            range[2] = 3;
            range[3] = 4;
            range[4] = 5;
            range[5] = 6;
            range[6] = 7;
            range[7] = 8;
            range[8] = 9;
            range[9] = 10;
            range[10] = 11;
            range[11] = 12;
            range[12] = 13;
            range[13] = 14;
            range[14] = 15;
            range[15] = 16;
            range[16] = 17;
            range[17] = 18;
        }

        public void UpdateDexSlots()
        {
            string paddedStart = range[0].ToString().PadLeft(3, '0');
            string paddedEnd = range[17].ToString().PadLeft(3, '0');

            if (range[17] >= 151)
            {
                pokedexShowingNumbers.SetText($"Showing No. {paddedStart}-151");
            }
            else
            {
                pokedexShowingNumbers.SetText($"Showing No. {paddedStart}-{paddedEnd}");
            }
            pokedexShowingNumbers.HAlign = 0.185f;

            dexSlot1.UpdateId(range[0]);
            dexSlot2.UpdateId(range[1]);
            dexSlot3.UpdateId(range[2]);
            dexSlot4.UpdateId(range[3]);
            dexSlot5.UpdateId(range[4]);
            dexSlot6.UpdateId(range[5]);
            dexSlot7.UpdateId(range[6]);
            dexSlot8.UpdateId(range[7]);
            dexSlot9.UpdateId(range[8]);
            dexSlot10.UpdateId(range[9]);
            dexSlot11.UpdateId(range[10]);
            dexSlot12.UpdateId(range[11]);
            dexSlot13.UpdateId(range[12]);
            dexSlot14.UpdateId(range[13]);
            dexSlot15.UpdateId(range[14]);
            dexSlot16.UpdateId(range[15]);
            dexSlot17.UpdateId(range[16]);
            dexSlot18.UpdateId(range[17]);

            dexSlot1x.UpdateId(range[0]);
            dexSlot2x.UpdateId(range[1]);
            dexSlot3x.UpdateId(range[2]);
            dexSlot4x.UpdateId(range[3]);
            dexSlot5x.UpdateId(range[4]);
            dexSlot6x.UpdateId(range[5]);
            dexSlot7x.UpdateId(range[6]);
            dexSlot8x.UpdateId(range[7]);
            dexSlot9x.UpdateId(range[8]);
            dexSlot10x.UpdateId(range[9]);
            dexSlot11x.UpdateId(range[10]);
            dexSlot12x.UpdateId(range[11]);
            dexSlot13x.UpdateId(range[12]);
            dexSlot14x.UpdateId(range[13]);
            dexSlot15x.UpdateId(range[14]);
            dexSlot16x.UpdateId(range[15]);
            dexSlot17x.UpdateId(range[16]);
            dexSlot18x.UpdateId(range[17]);
        }

        public void SummaryData(PokemonData data)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;
            base.Draw(spriteBatch);
        }

        public bool IsPokedexOpen = false;
        public bool animating = false;
        public float lastStoredVAlign;
        public int storedMapStyle;
        public override void Update(GameTime gameTime)
        {
            if (lastStoredVAlign != pokedexTexture.VAlign) animating = true;
            else
            {
                animating = false;
            }

            lastStoredVAlign = pokedexTexture.VAlign;

            if (!animating)
            {
                if (Main.keyState.IsKeyDown(Keys.L) && !IsPokedexOpen && !Main.playerInventory && !Main.drawingPlayerChat)
                {
                    storedMapStyle = Main.mapStyle;
                    Main.mapStyle = 0;
                    IsPokedexOpen = true;
                    Main.PlaySound(TerramonMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/opendex").WithVolume(.7f));
                    TerramonMod.ZoomAnimator.DexVAlign(0.5f, 600, Easing.OutExpo);

                    // Update the displayed completion percentage here, when dex is opened.

                    float caughtOf = 0;
                    float total = 151;
                    foreach (int i in Main.LocalPlayer.GetModPlayer<TerramonPlayer>().PokedexCompletion)
                    {
                        if (i == 1) caughtOf++;
                    }

                    float perc = caughtOf / total * 100;

                    pokedexPercentComplete.SetText($"{perc.ToString("0.00")}% Completion");
                }
                else if (Main.keyState.IsKeyDown(Keys.L) && IsPokedexOpen && !Main.playerInventory && !Main.drawingPlayerChat)
                {
                    Main.mapStyle = storedMapStyle;
                    IsPokedexOpen = false;
                    Main.PlaySound(TerramonMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/closedex").WithVolume(.7f));
                    TerramonMod.ZoomAnimator.DexVAlign(3.8f, 600, Easing.In);
                }
            }

            if (Main.keyState.IsKeyDown(Keys.Escape) && IsPokedexOpen)
            {
                Main.mapStyle = storedMapStyle;
                IsPokedexOpen = false;
                TerramonMod.ZoomAnimator.DexVAlign(3.8f, 0, Easing.In);
            }

            base.Update(gameTime);
        }
    }

    public class Animator : Drawable
    {
        public float ZoomTarget
        {
            get => Main.GameZoomTarget;
            set => Main.GameZoomTarget = value;
        }
        public float ScreenPosXTarget
        {
            get => Main.screenPosition.X + (Main.screenWidth / 2);
            set => TerramonMod.Instance.battleCamera.X = value;
        }
        public float ScreenPosYTarget
        {
            get => Main.screenPosition.Y + (Main.screenHeight / 2);
            set => TerramonMod.Instance.battleCamera.Y = value;
        }

        public Vector2 ScreenPos
        {
            get => Main.screenPosition;
            set => TerramonMod.Instance.battleCamera = value;
        }

        public float ButtonMenuPanelX
        {
            get => BattleMode.UI.ButtonMenuPanel.Top.Pixels;
            set => BattleMode.UI.ButtonMenuPanel.Top.Pixels = value;
        }

        public float HPBar1Fill
        {
            get => BattleMode.UI.HP1.HPBar.fill;
            set => BattleMode.UI.HP1.HPBar.fill = value;
        }

        public float HPBar2Fill
        {
            get => BattleMode.UI.HP2.HPBar.fill;
            set => BattleMode.UI.HP2.HPBar.fill = value;
        }

        public int HPBar1DisplayNumber
        {
            get => BattleMode.UI.HP1.displayHpNumberLerp;
            set => BattleMode.UI.HP1.displayHpNumberLerp = value;
        }

        public int HPBar2DisplayNumber
        {
            get => BattleMode.UI.HP2.displayHpNumberLerp;
            set => BattleMode.UI.HP2.displayHpNumberLerp = value;
        }

        public float HPBar1LeftPixels
        {
            get => BattleMode.UI.HP1.Left.Pixels;
            set => BattleMode.UI.HP1.Left.Pixels = value;
        }

        public float WhiteFlashOpacityVal
        {
            get => BattleMode.UI.FLASH_VISIBILITY;
            set => BattleMode.UI.FLASH_VISIBILITY = value;
        }

        public float DexVerticalAlign
        {
            get => TerramonMod.Instance.summaryUI.pokedexTexture.VAlign;
            set => TerramonMod.Instance.summaryUI.pokedexTexture.VAlign = value;
        }
    }
    public static class AnimatorExtensions
    {
        public static TransformSequence<T> GameZoom<T>(this T drawable, float newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.ZoomTarget), newValue, duration, easing);
        public static TransformSequence<T> GameZoom<T>(this TransformSequence<T> t, float newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.GameZoom(newValue, duration, easing));
        public static TransformSequence<T> ScreenPosX<T>(this T drawable, float newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.ScreenPosXTarget), newValue, duration, easing);
        public static TransformSequence<T> ScreenPosX<T>(this TransformSequence<T> t, float newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.ScreenPosX(newValue, duration, easing));
        public static TransformSequence<T> ScreenPosY<T>(this T drawable, float newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.ScreenPosYTarget), newValue, duration, easing);
        public static TransformSequence<T> ScreenPosY<T>(this TransformSequence<T> t, float newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.ScreenPosY(newValue, duration, easing));

        public static TransformSequence<T> ScreenPos<T>(this T drawable, Vector2 newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.ScreenPos), newValue, duration, easing);
        public static TransformSequence<T> ScreenPos<T>(this TransformSequence<T> t, Vector2 newValue, double duration = 0, Easing easing = Easing.None)
            where T : Animator =>
            t.Append(o => o.ScreenPos(newValue, duration, easing));

        public static TransformSequence<T> ButtonMenuPanelX<T>(this T drawable, float newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.ButtonMenuPanelX), newValue, duration, easing);
        public static TransformSequence<T> ButtonMenuPanelX<T>(this TransformSequence<T> t, float newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.ButtonMenuPanelX(newValue, duration, easing));

        public static TransformSequence<T> HPBar1Fill<T>(this T drawable, float newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.HPBar1Fill), newValue, duration, easing);
        public static TransformSequence<T> HPBar1Fill<T>(this TransformSequence<T> t, float newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.HPBar1Fill(newValue, duration, easing));
        public static TransformSequence<T> HPBar2Fill<T>(this T drawable, float newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.HPBar2Fill), newValue, duration, easing);
        public static TransformSequence<T> HPBar2Fill<T>(this TransformSequence<T> t, float newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.HPBar2Fill(newValue, duration, easing));

        public static TransformSequence<T> HPBar1DisplayNumber<T>(this T drawable, int newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.HPBar1DisplayNumber), newValue, duration, easing);
        public static TransformSequence<T> HPBar1DisplayNumber<T>(this TransformSequence<T> t, int newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.HPBar1DisplayNumber(newValue, duration, easing));
        public static TransformSequence<T> HPBar2DisplayNumber<T>(this T drawable, int newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.HPBar2DisplayNumber), newValue, duration, easing);
        public static TransformSequence<T> HPBar2DisplayNumber<T>(this TransformSequence<T> t, int newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.HPBar2DisplayNumber(newValue, duration, easing));

        public static TransformSequence<T> HPBar1LeftPixels<T>(this T drawable, float newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.HPBar1LeftPixels), newValue, duration, easing);
        public static TransformSequence<T> HPBar1LeftPixels<T>(this TransformSequence<T> t, float newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.HPBar1LeftPixels(newValue, duration, easing));

        public static TransformSequence<T> WhiteFlashOpacity<T>(this T drawable, float newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.WhiteFlashOpacityVal), newValue, duration, easing);
        public static TransformSequence<T> WhiteFlashOpacity<T>(this TransformSequence<T> t, float newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.WhiteFlashOpacity(newValue, duration, easing));

        public static TransformSequence<T> DexVAlign<T>(this T drawable, float newValue, double duration = 0, Easing easing = Easing.None) where T : Animator =>
            drawable.TransformTo(nameof(drawable.DexVerticalAlign), newValue, duration, easing);
        public static TransformSequence<T> DexVAlign<T>(this TransformSequence<T> t, float newValue, double duration = 0, Easing easing = Easing.None)
                  where T : Animator =>
                  t.Append(o => o.DexVAlign(newValue, duration, easing));
    }
}
