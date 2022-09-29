using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.Graphics;
using Razorwing.Framework.Graphics.Transforms;
using Razorwing.Framework.Localisation;
using Terramon.Players;
using Terramon.Pokemon;
using Terramon.Pokemon.Moves;
using Terramon.UI.Moveset;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terramon.UI.Battling.v2
{
    public class BattleV2UI : UIState
    {
        public UIImagez whiteFlash;
        public UITypewriterText splashText;
        public UIText tipText;
        public ButtonMenuPanel ButtonMenuPanel;
        public MovesPanel MovesPanel;
        public HPPanel HP1, HP2;
        public bool Turn;
        public Action<BaseMove> MovePresed;
        public static bool Visible = false;

        public static bool doneWildIntro = false;

        public Color FLASH_COLOR = Color.White;
        public float FLASH_VISIBILITY = 0f;

        public BattleUIState State = BattleUIState.MainMenu;

        public BattleModeV2 Battle { get; set; }
        public BattlePlayerOpponent Player { get; set; }
        public BattleOpponent Enemy { get; set; }


        public override void OnInitialize()
        {
            // Splash text to replace Main.NewText() calls
            splashText = new UITypewriterText("", 1.1f);
            splashText.HAlign = 0.5f;
            splashText.Top.Set(-246, 1f);
            Append(splashText);

            whiteFlash = new UIImagez(ModContent.GetTexture("Terramon/UI/IntroMovie/WhiteFlash"));
            whiteFlash.HAlign = 0f;
            whiteFlash.VAlign = 0f;
            whiteFlash.ImageScale *= 5;
            whiteFlash._visibilityActive = 0f;

            tipText = new UIText("", 1.25f);
            tipText.TextColor = new Color(189, 189, 189);
            tipText.VAlign = 0.25f;
            tipText.HAlign = 0.5f;
            Append(tipText);

            HP1 = new HPPanel(true);
            HP1.Left.Set(20, 1f); //HP1.Left.Set(-340, 1f); 
            HP1.Top.Set(0, 0.6f);
            Append(HP1);

            HP2 = new HPPanel(false);
            HP2.Left.Set(30000, 0f); //HP2.Left.Set(160, 0f);
            HP2.Top.Set(0, 0.4f);
            Append(HP2);

            Append(whiteFlash);

            MovesPanel = new MovesPanel()
            {
                OnMoveClick = (x) =>
                {
                    if (Player.ForwardedMove == null)
                    {
                        Player.ForwardedMove = x;
                        State = BattleUIState.Animating;
                    }
                }
            };
            Append(MovesPanel);

            base.OnInitialize();
        }

        public void ResetData()
        {
            //HP1.Opponent = null;
            //HP2.Opponent = null;
            MovesPanel.PokeData = null;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            whiteFlash._visibilityActive = FLASH_VISIBILITY;

            if (!doneWildIntro)
            {
                whiteFlash.Remove();

                SetupPokeData();

                doneWildIntro = true;
            }

            base.Update(gameTime);
        }

        public void SetupPokeData()
        {
            var player = Player.Player;

            if (player.firstBattle)
            {
                //tipText.SetText("Use WASD to move the camera around.");
                player.firstBattle = false;
            }

            splashText.SetText($"What will {Player.PokeData.PokemonName} do?");

            // pan camera to local player pokemon
            Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/uislide").WithVolume(.6f));
            TerramonMod.ZoomAnimator.ScreenPosX(Player.PokeProj.projectile.position.X, 500, Easing.OutExpo);
            TerramonMod.ZoomAnimator.ScreenPosY(Player.PokeProj.projectile.position.Y, 500, Easing.OutExpo);

            if (ButtonMenuPanel == null)
            {
                ButtonMenuPanel = new ButtonMenuPanel();
                ButtonMenuPanel.OnInitialize();
            }

            ButtonMenuPanel.Data = Player;
            MovesPanel.PokeData = Player.PokeData;

            Append(ButtonMenuPanel);
        }
    }

    public enum BattleUIState
    {
        Intro,
        PostIntro,
        Animating,
        PreMainMenu,
        MainMenu,
        MovesMenu,
        BagMenu,
        PokemonChange,
    }

    public class ButtonMenuPanel : DrawablePanel
    {
        private BattlingMenuButton FightButton;
        private BattlingMenuButton BagButton;
        private BattlingMenuButton PokemonButton;
        private BattlingMenuButton RunButton;

        private UIText FightText;
        private UIText BagText;
        private UIText PokemonText;
        private UIText RunText;

        public BattlePlayerOpponent Data { get; set; }

        public new BattleV2UI Parent => (BattleV2UI) base.Parent;

        public override void OnInitialize()
        {
            this.Width.Set(450, 0f);
            this.Height.Set(200, 0f);
            this.Top.Set(500, 1f);
            this.Left.Set(-225, 0.5f);
            this.BackgroundColor = Color.White * 0f;
            this.BorderColor = Color.White * 0f;

            FightButton = new BattlingMenuButton(ModContent.GetTexture("Terramon/UI/Battling/FightButton"), ModContent.GetTexture("Terramon/UI/Battling/FightButton_Hover"));
            FightButton.OnClick += fight;
            Append(FightButton);

            FightText = new UIText("Fight", 0.6f, true);
            FightText.HAlign = 0.5f;
            FightText.VAlign = 0.5f;
            FightButton.Append(FightText);

            BagButton = new BattlingMenuButton(ModContent.GetTexture("Terramon/UI/Battling/BagButton"), ModContent.GetTexture("Terramon/UI/Battling/BagButton_Hover"));
            BagButton.Left.Set(218, 0f);
            Append(BagButton);

            BagText = new UIText("Bag", 0.6f, true);
            BagText.HAlign = 0.5f;
            BagText.VAlign = 0.5f;
            BagButton.Append(BagText);

            PokemonButton = new BattlingMenuButton(ModContent.GetTexture("Terramon/UI/Battling/PokemonButton"), ModContent.GetTexture("Terramon/UI/Battling/PokemonButton_Hover"));
            PokemonButton.Top.Set(76, 0f);
            Append(PokemonButton);

            PokemonText = new UIText("Pokémon", 0.6f, true);
            PokemonText.HAlign = 0.5f;
            PokemonText.VAlign = 0.5f;
            PokemonButton.Append(PokemonText);

            RunButton = new BattlingMenuButton(ModContent.GetTexture("Terramon/UI/Battling/RunButton"), ModContent.GetTexture("Terramon/UI/Battling/RunButton_Hover"));
            RunButton.Top.Set(76, 0f);
            RunButton.Left.Set(218, 0f);
            RunButton.OnClick += runAway;
            Append(RunButton);

            RunText = new UIText("Run", 0.6f, true);
            RunText.HAlign = 0.5f;
            RunText.VAlign = 0.5f;
            RunButton.Append(RunText);
        }

        public bool viewable = false;
        public override void Update(GameTime gameTime)
        {
            var player1 = Parent.Player;
            if (Parent.State == BattleUIState.PreMainMenu)
            {
                Parent.splashText.SetText($"What will {player1.PokeData.PokemonName} do?");
                //BattleMode.UI.splashText.SetText($"What will {player1.PokeData.PokemonName} do?");
            }
            if (Parent.State == BattleUIState.MainMenu)
            {
                if (!viewable)
                {
                    TerramonMod.ZoomAnimator.ScreenPosX((Parent.Player.PokeProj.projectile.position.X + 12), 500, Easing.OutExpo);
                    TerramonMod.ZoomAnimator.ScreenPosY((Parent.Player.PokeProj.projectile.position.Y), 500, Easing.OutExpo);
                    Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/uislide").WithVolume(.6f));
                    //TerramonMod.ZoomAnimator.ButtonMenuPanelX(-220, 500, Easing.OutExpo);
                    this.MoveToY(-220, 500, Easing.OutExpo);
                }
                viewable = true;
            }
            else
            {
                viewable = false;
                Top.Set(500, 1f);
            }

            Recalculate();


            base.Update(gameTime);
        }
        private void fight(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/uiselect").WithVolume(.55f));
            Parent.State = BattleUIState.MovesMenu;
        }

        private void runAway(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/fleeswitch").WithVolume(.55f));
            BattleMode.queueRunAway = true;
        }
    }

    public class HPPanel : DrawablePanel
    {
        private UIText LevelText, HPText, EXPText, HPLocal, PokeName;
        //private UIImage HP, HPBack, Party, PartyExh, Fart;

        public new BattleV2UI Parent => (BattleV2UI)base.Parent;


        public BattleHPBar HPBar;
        public BattleEXPBar EXPBar;

        private ILocalisedBindableString LocPokemon;

        public BattleOpponent Opponent => local ? Parent.Player : Parent.Enemy;

        public PokemonData PokeData => Opponent?.PokeData;
        private PokemonData pokeData;

        private int hpScaleTarget = 0;
        public int displayHpNumber = 0;
        public int displayHpNumberLerp = 0;
        public float lastDisplayHP = 0;
        public float displayHp = 0;

        public float fillval = 1f;

        public bool firstLoadHP = true;
        public bool adjusting = true;

        public bool local /*= true*/;//value set in constructor

        //private Texture HPTexture, HPBackTexture, PartyTexture, PartyExhausted;
        //public PokemonData PokeData
        //{
        //    get => pokeData;
        //    set
            
        //}
        //public int PartySize;

        public Vector2 Position
        {
            get => new Vector2(Left.Percent, Top.Percent);
            set
            {
                Left.Set(0, value.X);
                Top.Set(-55, value.Y);
            }
        }

        public HPPanel(bool l)
        {
            local = l;
        }

        public bool AdjustingHP()
        {
            return false; // later
        }

        public override void OnInitialize()
        {
            this.BackgroundColor = new Color(76, 78, 79);
            this.Width.Set(178, 0f);
            this.Height.Set(local ? 84 : 75, 0f);

            PokeName = new UIText(LocPokemon?.Value ?? string.Empty);
            PokeName.Left.Set(5, 0f);
            PokeName.Top.Set(5, 0f);
            Append(PokeName);

            HPText = new UIText("HP", 0.6f, false);
            HPText.Top.Set(32, 0f);
            HPText.Left.Set(5, 0f);
            Append(HPText);

            if (local)
            {
                EXPText = new UIText("EXP", 0.6f, false);
                EXPText.Top.Set(50, 0f);
                EXPText.Left.Set(5, 0f);
                Append(EXPText);
                
                HPLocal = new UIText("", 0.9f, false);
                HPLocal.Top.Set(52, 0f);
                HPLocal.Left.Set(5, 0f);
                //Append(HPLocal);

                EXPBar = new BattleEXPBar();
                EXPBar.Top.Set(50, 0f);
                EXPBar.Left.Set(-119, 1f);
                EXPBar.Height.Set(8, 0f);
                EXPBar.Width.Set(110, 0f);
                Append(EXPBar);
            }

            HPBar = local ? new BattleHPBar(Color.LightGreen, true) : new BattleHPBar(Color.LightGreen, false);
            HPBar.Top.Set(30, 0f);
            HPBar.Left.Set(25, 0f);
            HPBar.Height.Set(14, 0f);
            HPBar.Width.Set(124, 0f);
            Append(HPBar);

            LevelText = new UIText("Lv 1");
            LevelText.HAlign = 0.92f;
            LevelText.Top.Set(5, 0f);
            Append(LevelText);

            if (PokeData != null)
            {
                displayHp = PokeData.HP;
                HPBar.fill = (float)PokeData.HP / PokeData.MaxHP;
                hpScaleTarget = PokeData.HP;
                pokeData = PokeData;
                PokeDataChanged();
            }

            base.OnInitialize();
        }

        public double hpLerpTimer;
        public override void Update(GameTime gameTime)
        {
            hpLerpTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if(PokeData == null)
                return;

            if (local)
            {
                if (Parent.Player.PokeData != pokeData)
                {
                    pokeData = Parent.Player.PokeData;
                    //LocPokemon = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(pokeData?.Pokemon));
                    //PokeName.SetText(LocPokemon.Value);
                    PokeDataChanged();
                }
            }
            else
            {
                if (Parent.Enemy.PokeData != pokeData)
                {
                    pokeData = Parent.Enemy.PokeData;
                    //LocPokemon = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(pokeData?.Pokemon));
                    //PokeName.SetText(LocPokemon.Value);
                    PokeDataChanged();
                }
            }
            

            if (local)
            {
                HPLocal?.SetText($"{displayHpNumberLerp}/{PokeData?.MaxHP ?? 0}");
                EXPBar.HoverText = $"{PokeData?.Exp}/{PokeData?.ExpToNext}";
                HPBar.HoverText = $"{displayHpNumberLerp}/{PokeData?.MaxHP}";
            }
            else
            {
                HPBar.HoverText = $"{displayHpNumberLerp}/{PokeData?.MaxHP}";
            }

            // if display and actual hp are not equal...
            if (displayHpNumber != PokeData?.HP)
            {
                // ... we need to update the hp bar fill + display number.
                //if (local)
                //{
                //    TerramonMod.ZoomAnimator.HPBar1Fill((float)PokeData.HP / PokeData.MaxHP, 400, Easing.None);
                //    TerramonMod.ZoomAnimator.HPBar1DisplayNumber(PokeData.HP, 400, Easing.None);
                //}
                //else
                //{
                //    TerramonMod.ZoomAnimator.HPBar2Fill((float)PokeData.HP / PokeData.MaxHP, 400, Easing.None);
                //    TerramonMod.ZoomAnimator.HPBar2DisplayNumber(PokeData.HP, 400, Easing.None);
                //}
                this.HPBar.HPBarFill((float) PokeData.HP / PokeData.MaxHP, 400, Easing.None);
                this.HPBarValue(PokeData.HP, 400, Easing.None);
            }

            // it get set right after anyway to ensure the Above IF() does not get called more than once
            if (PokeData?.HP != null) displayHpNumber = PokeData.HP;

            base.Update(gameTime);
        }

        public void PokeDataChanged()
        {
            if (PokeData == null)
                return;

            displayHpNumber = PokeData.HP;
            displayHpNumberLerp = PokeData.HP;
            HPBar.fill = PokeData.HP;

            HPBar.fill = (float)PokeData?.HP / (float)PokeData?.MaxHP;
            if (local)
            {
                if (EXPBar.fill > 1f) EXPBar.fill = 1f;
                else EXPBar.fill = (float)PokeData?.Exp / (float)PokeData?.ExpToNext;
                EXPBar.HoverText = $"{PokeData?.Exp}/{PokeData?.ExpToNext}";
            }

            LocPokemon = TerramonMod.Localisation.GetLocalisedString(PokeData?.Pokemon ?? "MissingNO");
            PokeName?.SetText(LocPokemon.Value);
            LevelText.SetText($"Lv {PokeData?.Level ?? 1}");


        }
    }

    public static class HPBarExtensions
    {
        public static TransformSequence<T> HPBarFill<T>(this T drawable, float newValue, double duration = 0, Easing easing = Easing.None) 
            where T : BattleHPBar =>
            drawable.TransformTo(nameof(drawable.fill), newValue, duration, easing);
        public static TransformSequence<T> HPBarFill<T>(this TransformSequence<T> t, float newValue, double duration = 0, Easing easing = Easing.None)
            where T : BattleHPBar =>
            t.Append(o => o.HPBarFill(newValue, duration, easing));

        public static TransformSequence<T> HPBarValue<T>(this T drawable, int newValue, double duration = 0, Easing easing = Easing.None) 
            where T : HPPanel =>
            drawable.TransformTo(nameof(drawable.displayHpNumberLerp), newValue, duration, easing);
        public static TransformSequence<T> HPBarValue<T>(this TransformSequence<T> t, int newValue, double duration = 0, Easing easing = Easing.None)
            where T : HPPanel =>
            t.Append(o => o.HPBarValue(newValue, duration, easing));
    }

    public class MovesPanel : DrawablePanel
    {
        private BattleMoveButton Move1, Move2, Move3, Move4;

        private readonly BattleMoveButton[] MovesBtn = new BattleMoveButton[4];

        public new BattleV2UI Parent => (BattleV2UI)base.Parent;


        public PokemonData PokeData
        {
            get => pokeData;
            set
            {
                if (pokeData == value)
                    return;

                needUpdate = true;
                pokeData = value;
            }
        }
        private bool needUpdate;
        public static bool Visible;
        public Action<BaseMove> OnMoveClick;
        private PokemonData pokeData;

        public MovesPanel()
        {
        }

        public override void OnInitialize()
        {
            this.Width.Set(450, 0f);
            this.Height.Set(200, 0f);
            this.Top.Set(500, 1f);
            this.Left.Set(-225, 0.5f);
            this.BackgroundColor = Color.White * 0f;
            this.BorderColor = Color.White * 0f;
            var size = new Vector2(170, 40);

            MovesBtn[0] = Move1 = new BattleMoveButton(PokeData?.Moves[0], new Vector2(0, 0))
            {
                OnClick = (x) => OnMoveClick?.Invoke(x),
            };
            Append(Move1);
            MovesBtn[1] = Move2 = new BattleMoveButton(PokeData?.Moves[1], new Vector2(218, 0))
            {
                OnClick = (x) => OnMoveClick?.Invoke(x),
            };
            Append(Move2);
            MovesBtn[2] = Move3 = new BattleMoveButton(PokeData?.Moves[2], new Vector2(0, 76))
            {
                OnClick = (x) => OnMoveClick?.Invoke(x),
            };
            Append(Move3);
            MovesBtn[3] = Move4 = new BattleMoveButton(PokeData?.Moves[3], new Vector2(218, 76))
            {
                OnClick = (x) => OnMoveClick?.Invoke(x),
            };
            Append(Move4);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            //Be in touch if data is changed
            PokeData = Parent.Player.PokeData;

            if (needUpdate)
            {
                for (int i = 0; i < 4; i++)
                {
                    MovesBtn[i].Move = pokeData.Moves[i];
                }
#if DEBUG
                Move1.Move = TerramonMod.GetMove(nameof(ShootMove));
#endif
                //Move1.Move = pokeData.Moves[0];
                //Move2.Move = pokeData.Moves[1];
                //Move3.Move = pokeData.Moves[2];
                //Move4.Move = pokeData.Moves[3];
            }

            if (Parent.State == BattleUIState.MovesMenu)
            {
                if (Parent.Player.ForwardedMove == null)
                {
                    Top.Set(-220, 1f);
                }
                else
                {
                    Top.Set(500, 1f);
                }
            }
            else
            {
                Top.Set(500, 1f);
            }

            base.Update(gameTime);
        }
    }

    public class BattleMoveButton : Drawable
    {
        private Texture2D _texture = ModContent.GetTexture("Terramon/UI/Battling/InterfaceButton");

        private Texture2D _texture_hovered = ModContent.GetTexture("Terramon/UI/Battling/InterfaceButton_Hover");

        public float ImageScale = 1f;

        public float _visibilityActive = 1f;

        private UIText text;
        private UIImagez mask;
        public int PPLeft { get; set; }

        public BaseMove Move
        {
            get => move;
            set
            {
                if (value == move)
                    return;
                move = value;

                if (move != null)
                {
                    MoveName = TerramonMod.Localisation.GetLocalisedString(move.MoveName);
                    TypeName = TerramonMod.Localisation.GetLocalisedString(move.MoveType.ToString());
                    UpdateTypeMask();
                }
                else
                {
                    MoveName = TerramonMod.Localisation.GetLocalisedString("-");
                    TypeName = TerramonMod.Localisation.GetLocalisedString("???");
                }

                needUpdate = true;
            }
        }

        private ILocalisedBindableString MoveName, TypeName;
        private readonly bool leftSide;
        private bool needUpdate;

        internal string HoverText;

        public new Action<BaseMove> OnClick;

        public BattleMoveButton(BaseMove move, Vector2 pos)
        {
            this.move = move;
            if (move != null)
            {
                UpdateTypeMask();
                MoveName = TerramonMod.Localisation.GetLocalisedString(move.MoveName);
                TypeName = TerramonMod.Localisation.GetLocalisedString(move.MoveType.ToString());
            }
            else
            {
                MoveName = TerramonMod.Localisation.GetLocalisedString("-");
                TypeName = TerramonMod.Localisation.GetLocalisedString("???");
            }

            Left.Set(pos.X, 0);
            Top.Set(pos.Y, 0);
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
        }

        public override void OnInitialize()
        {
            text = new UIText(MoveName.Value, 0.4f, true);
            text.VAlign = 0.5f;
            text.HAlign = 0.5f;
            //text.Left.Set(ChatManager.GetStringSize(Main.fontMouseText, MoveName.Value, 1.2f));
            Append(text);

            if (move != null)
            {
                UpdateTypeMask();
            }

            base.OnInitialize();
        }

        protected void UpdateTypeMask()
        {
            if (ModContent.FileExists($"Terramon/UI/Battling/Masks/{move.MoveType}"))
            {
                var masktexture = ModContent.GetTexture($"Terramon/UI/Battling/Masks/{move.MoveType}");
                mask = new UIImagez(masktexture);
                Append(mask);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (IsMouseHovering)
            {
                spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - ImageScale) / 2f, texture: _texture_hovered, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: ImageScale, effects: SpriteEffects.None, layerDepth: 0f);
            }
            else
            {
                spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - ImageScale) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: ImageScale, effects: SpriteEffects.None, layerDepth: 0f);
            }
            if (IsMouseHovering) Main.hoverItemName = HoverText;
        }

        public bool lk;
        private BaseMove move;


        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            if (ContainsPoint(Main.MouseScreen))
            {
                //BattleMode.inMainMenu = true;
                Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/uiselect").WithVolume(.55f));
                OnClick?.Invoke(move);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ContainsPoint(Main.MouseScreen)) Main.LocalPlayer.mouseInterface = true;

            if (needUpdate)
            {
                needUpdate = false;

                if (move != null)
                {
                    if (HasChild(mask))
                    {
                        mask.SetImage(ModContent.GetTexture($"Terramon/UI/Battling/Masks/{move.MoveType}"));
                    }
                    else
                    {
                        var masktexture = ModContent.GetTexture($"Terramon/UI/Battling/Masks/{move.MoveType}");
                        mask = new UIImagez(masktexture);
                        Append(mask);
                    }
                }

                text.SetText(MoveName.Value);
                text.VAlign = 0.5f;
                text.HAlign = 0.5f;
            }

            HoverText = $"{TypeName.Value} PP: {move?.MaxPP}/{move?.MaxPP}";
        }
    }

}
