using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System;
using Razorwing.Framework.Utils;
using Razorwing.Framework.Graphics;
using Terramon.UI.SidebarParty;
using Terramon.Network.Starter;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Players;
using Terramon.Pokemon;
using static Terramon.Pokemon.ExpGroups;
using Terramon.Pokemon.Utilities;
using Terramon.Pokemon.Moves;

namespace Terramon.UI.Starter
{
    // ExampleUIs visibility is toggled by typing "/coin" in chat. (See CoinCommand.cs)
    // ExampleUI is a simple UI example showing how to use UIPanel, UIImageButton, and even a custom UIElement.
    public class ChooseStarter : UIState
    {
        NonDragableUIPanel mainPanel;
        public static bool Visible;

        private UIText testmenu;
        private UIText testmenu2;

        // POKeMON BTN DEFINITIONS

         UIHoverImageButton bulbasaurTextureButton;
         UIHoverImageButtonDisabled chikoritaTextureButton;
         UIHoverImageButtonDisabled treeckoTextureButton;
         UIHoverImageButtonDisabled turtwigTextureButton;
         UIHoverImageButtonDisabled snivyTextureButton;
         UIHoverImageButtonDisabled chespinTextureButton;
         UIHoverImageButtonDisabled rowletTextureButton;

         UIHoverImageButton squirtleTextureButton;
         UIHoverImageButtonDisabled totodileTextureButton;
         UIHoverImageButtonDisabled mudkipTextureButton;
         UIHoverImageButtonDisabled piplupTextureButton;
         UIHoverImageButtonDisabled oshawottTextureButton;
         UIHoverImageButtonDisabled froakieTextureButton;
         UIHoverImageButtonDisabled popplioTextureButton;

         UIHoverImageButton charmanderTextureButton;
         UIHoverImageButtonDisabled cyndaquilTextureButton;
         UIHoverImageButtonDisabled torchicTextureButton;
         UIHoverImageButtonDisabled chimcharTextureButton;
         UIHoverImageButtonDisabled tepigTextureButton;
         UIHoverImageButtonDisabled fennekinTextureButton;
         UIHoverImageButtonDisabled littenTextureButton;

        private UIText fanMadeModText;

        private UIText pokemonNameText;
        private UIText pokemonDescText;

        private UIImagez starterselectmenu;

        private UIImagez bartop;
        private UIImagez barbottom;

        private UIImagez shaderBar1;
        private UIImagez shaderBar2;
        private UIImagez shaderBar3;

        float shaderBar1Speed;
        float shaderBar2Speed;
        float shaderBar3Speed;

        // MINI-MOVIE CUTSCENE VARS

        private UIImagez marginBarTop;
        private UIImagez marginBarBottom;

        private UIImagez whiteFlash;
        private UIImagez treesScrolling1;
        private UIImagez bottombushes;

        private UIImagez gengarFaceRight;
        private UIImagez nidorinoFaceLeft;

        private UIImagez gengarOrangeBg;
        private UIImagez nidorinoOrangeBg;

        private UIImagez smallBush;

        private UIImagez nidorinoBattle;
        private UIImagez gengarBattle;

        private UIImagez blackCover;
        public override void OnInitialize()
        {
            // Here we define our container UIElement. In DragableUIPanel.cs, you can see that DragableUIPanel is a UIPanel with a couple added features.
            mainPanel = new NonDragableUIPanel();
            mainPanel.SetPadding(0);
            mainPanel.Left.Set(0f, 0f);
            mainPanel.Top.Set(0f, 0f);
            mainPanel.Width.Set(0f, 1f);
            mainPanel.Height.Set(0f, 1f);
            mainPanel.BackgroundColor = new Color(0, 0, 0) * 1f;

            Texture2D starterselect = ModContent.GetTexture("Terramon/UI/Starter/CheckeredBackground");
            starterselectmenu = new UIImagez(starterselect);
            starterselectmenu.Left.Set(0, 0);
            starterselectmenu.Top.Set(0, 0);
            starterselectmenu.Width.Set(1, 0);
            starterselectmenu.Height.Set(1, 0);
            //mainPanel.Append(starterselectmenu);

            Texture2D shaderBarTexture = ModContent.GetTexture("Terramon/UI/Starter/ShaderBar");

                    shaderBar1 =
                        new UIImagez(shaderBarTexture);
                    shaderBar2 =
                        new UIImagez(shaderBarTexture);
                    shaderBar3 =
                        new UIImagez(shaderBarTexture);

            shaderBar1.HAlign = -0.5f;
            shaderBar1.VAlign = 0.1f;
            shaderBar1.Width.Set(1, 0f);
            shaderBar1.Height.Set(1, 0f);
            //mainPanel.Append(shaderBar1);

            shaderBar2.HAlign = -0.5f;
            shaderBar2.VAlign = 0.1f;
            shaderBar2.Width.Set(1, 0f);
            shaderBar2.Height.Set(1, 0f);
            //mainPanel.Append(shaderBar2);

            shaderBar3.HAlign = -0.5f;
            shaderBar3.VAlign = 0.1f;
            shaderBar3.Width.Set(1, 0f);
            shaderBar3.Height.Set(1, 0f);
            //mainPanel.Append(shaderBar3);

            Texture2D bartoptexture = ModContent.GetTexture("Terramon/UI/Starter/BarTop");
            bartop = new UIImagez(bartoptexture);
            bartop.HAlign = 0f;
            bartop.VAlign = 0f;
            bartop.Top.Set(0, 0);
            bartop.Width.Set(1, 0);
            bartop.Height.Set(1, 0);
            //mainPanel.Append(bartop);

            Texture2D barbottomtexture = ModContent.GetTexture("Terramon/UI/Starter/BarBottom");
            barbottom = new UIImagez(barbottomtexture);
            barbottom.HAlign = 0f;
            barbottom.VAlign = 1f;
            barbottom.Width.Set(1792, 0);
            barbottom.Height.Set(100, 0);
            //mainPanel.Append(barbottom);

            Texture2D treesScrolling1texture = ModContent.GetTexture("Terramon/UI/IntroMovie/TreesScrolling1");
            treesScrolling1 = new UIImagez(treesScrolling1texture);
            treesScrolling1.HAlign = -0.2f;
            treesScrolling1.Top.Set(100, 0);
            treesScrolling1.Width.Set(1, 0f);
            treesScrolling1.Height.Set(1, 0f);
            treesScrolling1._visibilityActive = 0f;
            mainPanel.Append(treesScrolling1);

            Texture2D gengarFaceRighttexture = ModContent.GetTexture("Terramon/UI/IntroMovie/GengarFaceRight");
            gengarFaceRight = new UIImagez(gengarFaceRighttexture);
            gengarFaceRight.HAlign = 0.5f;
            gengarFaceRight.VAlign = 0.6f;
            gengarFaceRight.Width.Set(732, 0f);
            gengarFaceRight.Height.Set(256, 0f);
            gengarFaceRight._visibilityActive = 0f;
            mainPanel.Append(gengarFaceRight);

            Texture2D nidorinoFaceLefttexture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoFaceLeft");
            nidorinoFaceLeft = new UIImagez(nidorinoFaceLefttexture);
            nidorinoFaceLeft.HAlign = 0.5f;
            nidorinoFaceLeft.VAlign = 0.6f;
            nidorinoFaceLeft.Width.Set(680, 0f);
            nidorinoFaceLeft.Height.Set(256, 0f);
            nidorinoFaceLeft._visibilityActive = 0f;
            mainPanel.Append(nidorinoFaceLeft);

            Texture2D bottombushestexture = ModContent.GetTexture("Terramon/UI/IntroMovie/BushesBottom");
            bottombushes = new UIImagez(bottombushestexture);
            bottombushes.HAlign = 0f;
            bottombushes.Top.Set(-216, 1f);
            bottombushes.Width.Set(1, 0f);
            bottombushes.Height.Set(1, 0f);
            bottombushes._visibilityActive = 0f;
            mainPanel.Append(bottombushes);

            Texture2D nidorinoOrangeBgtexture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoOrangeBg");
            nidorinoOrangeBg = new UIImagez(nidorinoOrangeBgtexture);
            nidorinoOrangeBg.Left.Set(-440, 1f);
            nidorinoOrangeBg.Top.Set(-500, 1f);
            nidorinoOrangeBg.Width.Set(440, 0f);
            nidorinoOrangeBg.Height.Set(404, 0f);
            nidorinoOrangeBg._visibilityActive = 0f;
            mainPanel.Append(nidorinoOrangeBg);

            Texture2D gengarOrangeBgtexture = ModContent.GetTexture("Terramon/UI/IntroMovie/GengarOrangeBg");
            gengarOrangeBg = new UIImagez(gengarOrangeBgtexture);
            gengarOrangeBg.HAlign = 0.015f;
            gengarOrangeBg.Top.Set(-290, 1f);
            gengarOrangeBg.Width.Set(1, 0f);
            gengarOrangeBg.Height.Set(1, 0f);
            gengarOrangeBg._visibilityActive = 0f;
            mainPanel.Append(gengarOrangeBg);

            Texture2D whiteFlashTexture = ModContent.GetTexture("Terramon/UI/IntroMovie/WhiteFlash");
            whiteFlash = new UIImagez(whiteFlashTexture);
            whiteFlash.HAlign = 0f;
            whiteFlash.VAlign = 0f;
            whiteFlash._visibilityActive = 0f;
            mainPanel.Append(whiteFlash);

            Texture2D nidorinoBattle1texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle1");
            nidorinoBattle = new UIImagez(nidorinoBattle1texture);
            nidorinoBattle.HAlign = -1.6f;
            nidorinoBattle.Top.Set(-356, 1f);
            nidorinoBattle.Width.Set(680, 0f);
            nidorinoBattle.Height.Set(256, 0f);
            nidorinoBattle._visibilityActive = 0f;
            mainPanel.Append(nidorinoBattle);

            Texture2D gengarBattle1texture = ModContent.GetTexture("Terramon/UI/IntroMovie/GengarBattle1");
            gengarBattle = new UIImagez(gengarBattle1texture);
            gengarBattle.HAlign = 2.6f;
            gengarBattle.Top.Set(-412, 1f);
            gengarBattle.Width.Set(732, 0f);
            gengarBattle.Height.Set(320, 0f);
            gengarBattle._visibilityActive = 0f;
            mainPanel.Append(gengarBattle);

Texture2D smallbushtexture = ModContent.GetTexture("Terramon/UI/IntroMovie/SmallBush");
            smallBush = new UIImagez(smallbushtexture);
            smallBush.HAlign = 1f;
            smallBush.Top.Set(-196, 1f);
            smallBush.Width.Set(1, 0f);
            smallBush.Height.Set(1, 0f);
            smallBush._visibilityActive = 0f;
            mainPanel.Append(smallBush);

            Texture2D marginbartexture = ModContent.GetTexture("Terramon/UI/IntroMovie/MarginBar");
            marginBarTop = new UIImagez(marginbartexture);
            marginBarTop.HAlign = 0f;
            marginBarTop.VAlign = 0f;
            marginBarTop.Top.Set(0, 0);
            marginBarTop.Width.Set(1, 0);
            marginBarTop.Height.Set(1, 0);
            mainPanel.Append(marginBarTop);

            marginBarBottom = new UIImagez(marginbartexture);
            marginBarBottom.HAlign = 0f;
            marginBarBottom.VAlign = 1f;
            marginBarBottom.Top.Set(0, 0);
            marginBarBottom.Width.Set(1792, 0);
            marginBarBottom.Height.Set(100, 0);
            mainPanel.Append(marginBarBottom);

            testmenu = new UIText("");
            testmenu.HAlign = 0.5f; // 1
            testmenu.VAlign = 0.2f; // 1
            testmenu.Width.Set(1, 0);
            testmenu.Height.Set(1, 0);
            mainPanel.Append(testmenu);

            testmenu2 = new UIText("");
            testmenu2.HAlign = 0.5f;
            testmenu2.Top.Set(30, 0);
            testmenu2.Width.Set(1, 0);
            testmenu2.Height.Set(1, 0);
            testmenu.Append(testmenu2);

            fanMadeModText = new UIText("");
            fanMadeModText.HAlign = 0.5f; // 1
            fanMadeModText.VAlign = 0.5f; // 1
            fanMadeModText.Width.Set(1, 0);
            fanMadeModText.Height.Set(1, 0);
            mainPanel.Append(fanMadeModText);

            pokemonNameText = new UIText("0/0");
            pokemonNameText.HAlign = 0.5f; // 1
            pokemonNameText.VAlign = 0.725f; // 1
            pokemonNameText.Width.Set(1, 0);
            pokemonNameText.Height.Set(1, 0);
            pokemonNameText.SetText("", 1.1f, false);
            mainPanel.Append(pokemonNameText);

            pokemonDescText = new UIText("0/0");
            pokemonDescText.HAlign = 0.5f; // 1
            pokemonDescText.VAlign = 0.8f; // 1
            pokemonDescText.Width.Set(1, 0);
            pokemonDescText.Height.Set(1, 0);
            pokemonDescText.SetText("", 1.1f, false);
            mainPanel.Append(pokemonDescText);

            // Grass types

            Texture2D bulbasaurTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Bulbasaur");
                bulbasaurTextureButton =
                    new UIHoverImageButton(bulbasaurTexture, "Bulbasaur");
            bulbasaurTextureButton.HAlign = 0.265f;
            bulbasaurTextureButton.VAlign = 0.35f;
            bulbasaurTextureButton.Width.Set(50, 0f);
            bulbasaurTextureButton.Height.Set(46, 0f);
            bulbasaurTextureButton.OnMouseOver += bulbasaurHovered;
            bulbasaurTextureButton.OnMouseOut += unHovered;
            bulbasaurTextureButton.OnClick += bulbasaurTextureButtonClicked;

            Texture2D chikoritaTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Chikorita");
                chikoritaTextureButton =
                    new UIHoverImageButtonDisabled(chikoritaTexture, "Coming Soon...");
            chikoritaTextureButton.HAlign = 0.345f;
            chikoritaTextureButton.VAlign = 0.35f;
            chikoritaTextureButton.Width.Set(50, 0f);
            chikoritaTextureButton.Height.Set(46, 0f);
            //chikoritaTextureButton.OnClick += bulbasaurTextureButtonClicked;

            Texture2D treeckoTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Treecko");
                treeckoTextureButton =
                    new UIHoverImageButtonDisabled(treeckoTexture, "Coming Soon...");
            treeckoTextureButton.HAlign = 0.425f;
            treeckoTextureButton.VAlign = 0.35f;
            treeckoTextureButton.Width.Set(50, 0f);
            treeckoTextureButton.Height.Set(46, 0f);
            //treeckoTextureButton.OnClick += bulbasaurTextureButtonClicked;

            Texture2D turtwigTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Turtwig");
                turtwigTextureButton =
                    new UIHoverImageButtonDisabled(turtwigTexture, "Coming Soon...");
            turtwigTextureButton.HAlign = 0.505f;
            turtwigTextureButton.VAlign = 0.35f;
            turtwigTextureButton.Width.Set(50, 0f);
            turtwigTextureButton.Height.Set(46, 0f);
            //turtwigTextureButton.OnClick += bulbasaurTextureButtonClicked;

            Texture2D snivyTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Snivy");
                snivyTextureButton =
                    new UIHoverImageButtonDisabled(snivyTexture, "Coming Soon...");
            snivyTextureButton.HAlign = 0.585f;
            snivyTextureButton.VAlign = 0.35f;
            snivyTextureButton.Width.Set(50, 0f);
            snivyTextureButton.Height.Set(46, 0f);
            //snivyTextureButton.OnClick += bulbasaurTextureButtonClicked;

            Texture2D chespinTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Chespin");
                chespinTextureButton =
                    new UIHoverImageButtonDisabled(chespinTexture, "Coming Soon...");
            chespinTextureButton.HAlign = 0.665f;
            chespinTextureButton.VAlign = 0.35f;
            chespinTextureButton.Width.Set(50, 0f);
            chespinTextureButton.Height.Set(46, 0f);
            //chespinTextureButton.OnClick += bulbasaurTextureButtonClicked;

            Texture2D rowletTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Rowlet");
                rowletTextureButton =
                    new UIHoverImageButtonDisabled(rowletTexture, "Coming Soon...");
            rowletTextureButton.HAlign = 0.745f;
            rowletTextureButton.VAlign = 0.35f;
            rowletTextureButton.Width.Set(50, 0f);
            rowletTextureButton.Height.Set(46, 0f);
            //rowletTextureButton.OnClick += bulbasaurTextureButtonClicked;

            // Water types

            Texture2D squirtleTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Squirtle");
                squirtleTextureButton =
                    new UIHoverImageButton(squirtleTexture, "Squirtle");
            squirtleTextureButton.HAlign = 0.265f;
            squirtleTextureButton.VAlign = 0.45f;
            squirtleTextureButton.Width.Set(50, 0f);
            squirtleTextureButton.Height.Set(46, 0f);
            squirtleTextureButton.OnMouseOver += squirtleHovered;
            squirtleTextureButton.OnMouseOut += unHovered;
            squirtleTextureButton.OnClick += squirtleTextureButtonClicked;

            Texture2D totodileTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Totodile");
                totodileTextureButton =
                    new UIHoverImageButtonDisabled(totodileTexture, "Coming Soon...");
            totodileTextureButton.HAlign = 0.345f;
            totodileTextureButton.VAlign = 0.45f;
            totodileTextureButton.Width.Set(50, 0f);
            totodileTextureButton.Height.Set(46, 0f);
            //totodileTextureButton.OnClick += squirtleTextureButtonClicked;

            Texture2D mudkipTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Mudkip");
                mudkipTextureButton =
                    new UIHoverImageButtonDisabled(mudkipTexture, "Coming Soon...");
            mudkipTextureButton.HAlign = 0.425f;
            mudkipTextureButton.VAlign = 0.45f;
            mudkipTextureButton.Width.Set(50, 0f);
            mudkipTextureButton.Height.Set(46, 0f);
            //mudkipTextureButton.OnClick += squirtleTextureButtonClicked;

            Texture2D piplupTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Piplup");
                piplupTextureButton =
                    new UIHoverImageButtonDisabled(piplupTexture, "Coming Soon...");
            piplupTextureButton.HAlign = 0.505f;
            piplupTextureButton.VAlign = 0.45f;
            piplupTextureButton.Width.Set(50, 0f);
            piplupTextureButton.Height.Set(46, 0f);
            //piplupTextureButton.OnClick += squirtleTextureButtonClicked;

            Texture2D oshawottTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Oshawott");
                oshawottTextureButton =
                    new UIHoverImageButtonDisabled(oshawottTexture, "Coming Soon...");
            oshawottTextureButton.HAlign = 0.585f;
            oshawottTextureButton.VAlign = 0.45f;
            oshawottTextureButton.Width.Set(50, 0f);
            oshawottTextureButton.Height.Set(46, 0f);
            //oshawottTextureButton.OnClick += squirtleTextureButtonClicked;

            Texture2D froakieTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Froakie");
                froakieTextureButton =
                    new UIHoverImageButtonDisabled(froakieTexture, "Coming Soon...");
            froakieTextureButton.HAlign = 0.665f;
            froakieTextureButton.VAlign = 0.45f;
            froakieTextureButton.Width.Set(50, 0f);
            froakieTextureButton.Height.Set(46, 0f);
            //froakieTextureButton.OnClick += squirtleTextureButtonClicked;

            Texture2D popplioTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Popplio");
                popplioTextureButton =
                    new UIHoverImageButtonDisabled(popplioTexture, "Coming Soon...");
            popplioTextureButton.HAlign = 0.745f;
            popplioTextureButton.VAlign = 0.45f;
            popplioTextureButton.Width.Set(50, 0f);
            popplioTextureButton.Height.Set(46, 0f);
            //popplioTextureButton.OnClick += squirtleTextureButtonClicked;

            // Fire types

            Texture2D charmanderTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Charmander");
                charmanderTextureButton =
                    new UIHoverImageButton(charmanderTexture, "Charmander");
            charmanderTextureButton.HAlign = 0.265f;
            charmanderTextureButton.VAlign = 0.55f;
            charmanderTextureButton.Width.Set(50, 0f);
            charmanderTextureButton.Height.Set(46, 0f);
            charmanderTextureButton.OnMouseOver += charmanderHovered;
            charmanderTextureButton.OnMouseOut += unHovered;
            charmanderTextureButton.OnClick += charmanderTextureButtonClicked;

            Texture2D cyndaquilTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Cyndaquil");
                cyndaquilTextureButton =
                    new UIHoverImageButtonDisabled(cyndaquilTexture, "Coming Soon...");
            cyndaquilTextureButton.HAlign = 0.345f;
            cyndaquilTextureButton.VAlign = 0.55f;
            cyndaquilTextureButton.Width.Set(50, 0f);
            cyndaquilTextureButton.Height.Set(46, 0f);
            //cyndaquilTextureButton.OnClick += charmanderTextureButtonClicked;

            Texture2D torchicTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Torchic");
                torchicTextureButton =
                    new UIHoverImageButtonDisabled(torchicTexture, "Coming Soon...");
            torchicTextureButton.HAlign = 0.425f;
            torchicTextureButton.VAlign = 0.55f;
            torchicTextureButton.Width.Set(50, 0f);
            torchicTextureButton.Height.Set(46, 0f);
            //torchicTextureButton.OnClick += charmanderTextureButtonClicked;

            Texture2D chimcharTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Chimchar");
                chimcharTextureButton =
                    new UIHoverImageButtonDisabled(chimcharTexture, "Coming Soon...");
            chimcharTextureButton.HAlign = 0.505f;
            chimcharTextureButton.VAlign = 0.55f;
            chimcharTextureButton.Width.Set(50, 0f);
            chimcharTextureButton.Height.Set(46, 0f);
            //chimcharTextureButton.OnClick += charmanderTextureButtonClicked;

            Texture2D tepigTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Tepig");
                tepigTextureButton =
                    new UIHoverImageButtonDisabled(tepigTexture, "Coming Soon...");
            tepigTextureButton.HAlign = 0.585f;
            tepigTextureButton.VAlign = 0.55f;
            tepigTextureButton.Width.Set(50, 0f);
            tepigTextureButton.Height.Set(46, 0f);
            //tepigTextureButton.OnClick += charmanderTextureButtonClicked;

            Texture2D fennekinTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Fennekin");
                fennekinTextureButton =
                    new UIHoverImageButtonDisabled(fennekinTexture, "Coming Soon...");
            fennekinTextureButton.HAlign = 0.665f;
            fennekinTextureButton.VAlign = 0.55f;
            fennekinTextureButton.Width.Set(50, 0f);
            fennekinTextureButton.Height.Set(46, 0f);
            //fennekinTextureButton.OnClick += charmanderTextureButtonClicked;

            Texture2D littenTexture = ModContent.GetTexture("Terramon/UI/Starter/PKMN/Litten");
                littenTextureButton =
                    new UIHoverImageButtonDisabled(littenTexture, "Coming Soon...");
            littenTextureButton.HAlign = 0.745f;
            littenTextureButton.VAlign = 0.55f;
            littenTextureButton.Width.Set(50, 0f);
            littenTextureButton.Height.Set(46, 0f);
            //littenTextureButton.OnClick += charmanderTextureButtonClicked;

            Append(mainPanel);

            double n = Main.rand.NextDouble() * (0.85 - -0.1) + -0.1;
            shaderBar1.VAlign = (float)n;
            n = Main.rand.NextDouble() * (0.85 - -0.1) + -0.1;
            shaderBar2.VAlign = (float)n;
            n = Main.rand.NextDouble() * (0.85 - -0.1) + -0.1;
            shaderBar3.VAlign = (float)n;

            double s = Main.rand.NextDouble() * (0.009 - 0.002) + 0.002;
            shaderBar1Speed = (float)s;
            s = Main.rand.NextDouble() * (0.009 - 0.002) + 0.002;
            shaderBar2Speed = (float)s;
            s = Main.rand.NextDouble() * (0.009 - 0.002) + 0.002;
            shaderBar3Speed = (float)s;
        }


        private Player player = Main.LocalPlayer;

        public static float shaderBar1Timer = 0;
        float shaderBar1MoveTimer = 0;
        float fanMadeModTimer = 0;

        // mini movie cutscene timers
        float flashWhite1Timer = 0;
        float orangeBgTimer = 0;
        float flashWhite2Timer = 0;
        float scrollFastTimer = 0;

        // mini movie cutscene vars
        byte flashWhite1 = 0;
        double startflashwhite1;
        double endflashwhite1;
        bool didOrangeBg = false;
        byte flashWhite2 = 0;
        double startflashwhite2;
        double endflashwhite2;
        bool playNidoranRoar = false;
        byte playedNidoranRoar = 1;
        bool playGengarRoar = false;

        double startScrollInBattleNidorino;
        double endScrollInBattleNidorino;

        double startSlideBackGengar = 0;
        double endSlideBackGengar = 0;

        double startSlashGengar = 0;
        double endSlashGengar = 0;

        double startNidorinoDodge = 0;
        double endNidorinoDodge = 0;

        double startNidorinoJumpForward = 0;
        double endNidorinoJumpForward = 0;

        double startSmallHopBack1 = 0;
        double endSmallHopBack1 = 0;

        double startSmallHopBack2 = 0;
        double endSmallHopBack2 = 0;

        double startFinalJump = 0;
        double endFinalJump = 0;

        double startBlackCover = 0;
        double endBlackCover = 0;

        public static bool movieFinished = false;

        bool fanMadeModTextShown = false;

        private Random rand = new Random();

        byte didSelectStarter = 0;
        bool render = true;

        public static bool playedIntroMusic = false;

        double start;
        double end;

        double abc = 0;

        public override void Update(GameTime gameTime)
        {
            // Increment timers
            shaderBar1Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            shaderBar1MoveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (movieFinished)
            {
                fanMadeModTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            flashWhite1Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            orangeBgTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            flashWhite2Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (flashWhite2 == 1)
            {
                scrollFastTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //var modExpanse = ModLoader.GetMod("tmonadds");
            if (shaderBar1Timer > 0 && !playedIntroMusic/* && modExpanse == null*/)
            {
                playedIntroMusic = true;
                TerramonPlayer p = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                p.openingSfx = Main.PlaySound(SoundLoader.customSoundType, Style: ModContent.GetInstance<TerramonMod>().GetSoundSlot(SoundType.Custom, "Sounds/Custom/opening"));
            }

            if (shaderBar1Timer >= 1)
            {
                shaderBar1Timer = 0;
            }

            if (flashWhite1Timer >= 0.9 && flashWhite1 == 0)
            {
                startflashwhite1 = gameTime.TotalGameTime.TotalSeconds;
                endflashwhite1 = startflashwhite1 + 0.15;
                flashWhite1 = 1;
            }

            if (flashWhite2Timer >= 3.8 && flashWhite2 == 0)
            {
                startflashwhite2 = gameTime.TotalGameTime.TotalSeconds;
                endflashwhite2 = startflashwhite2 + 0.15;
                flashWhite2 = 1;
                startScrollInBattleNidorino = gameTime.TotalGameTime.TotalSeconds;
                endScrollInBattleNidorino = startScrollInBattleNidorino + 1.1;
            }

            if (orangeBgTimer >= 2.4 && !didOrangeBg)
            {
                didOrangeBg = true;

                // hide old

                treesScrolling1._visibilityActive = 0f;
                bottombushes._visibilityActive = 0f;
                gengarFaceRight._visibilityActive = 0f;
                nidorinoFaceLeft._visibilityActive = 0f;
                mainPanel.BackgroundColor = new Color(251, 145, 0) * 1f;

                // show new

                gengarOrangeBg._visibilityActive = 1f;
                nidorinoOrangeBg._visibilityActive = 1f;
            }

            if (didOrangeBg && shaderBar1MoveTimer > 0.01)
            {
                gengarOrangeBg.Top.Pixels -= 1;
                nidorinoOrangeBg.Top.Pixels += 1;
            }

            if (flashWhite1 == 1 && gameTime.TotalGameTime.TotalSeconds < endflashwhite1 && flashWhite2 == 0)
            {
                treesScrolling1._visibilityActive = 1f;
                bottombushes._visibilityActive = 1f;
                gengarFaceRight._visibilityActive = 1f;
                nidorinoFaceLeft._visibilityActive = 1f;
                whiteFlash._visibilityActive = (float)Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.99, 0, startflashwhite1, endflashwhite1, Easing.None);
            }

            if (flashWhite2 == 1 && gameTime.TotalGameTime.TotalSeconds < endflashwhite2)
            {
                treesScrolling1.HAlign = -0.9f;
                treesScrolling1._visibilityActive = 1f;
                mainPanel.BackgroundColor = new Color(0, 0, 0) * 1f;
                gengarOrangeBg._visibilityActive = 0f;
                nidorinoOrangeBg._visibilityActive = 0f;
                smallBush._visibilityActive = 1f;
                whiteFlash._visibilityActive = (float)Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.99, 0, startflashwhite2, endflashwhite2, Easing.None);
            }

            if (flashWhite1 == 1 && shaderBar1MoveTimer > 0.01 && flashWhite2 == 0)
            {
                treesScrolling1.HAlign += 0.002f;
                bottombushes.HAlign -= 0.002f;
            }

            if (flashWhite2 == 1 && shaderBar1MoveTimer > 0.01)
            {
                if (scrollFastTimer < 0.8)
                {
                    treesScrolling1.HAlign += 0.01f;
                    smallBush.HAlign -= 0.019f;
                } else
                {
                    treesScrolling1.HAlign += 0.001f;
                    smallBush.HAlign -= 0.002f;
                }

                if (nidorinoBattle.HAlign < 0.5f && gameTime.TotalGameTime.TotalSeconds < endScrollInBattleNidorino)
                {
                    nidorinoBattle.HAlign = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, -1.6f, 0.5f, startScrollInBattleNidorino, endScrollInBattleNidorino, Easing.OutExpo);
                    nidorinoBattle._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0f, 1f, startScrollInBattleNidorino, endScrollInBattleNidorino - 0.5, Easing.None);
                }
                if (gengarBattle.HAlign > 0.5f)
                {
                    gengarBattle.HAlign = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 2.6f, 0.5f, startScrollInBattleNidorino, endScrollInBattleNidorino, Easing.OutExpo);
                    gengarBattle._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0f, 1f, startScrollInBattleNidorino, endScrollInBattleNidorino - 0.5, Easing.None);
                }

                if (scrollFastTimer > 1.35)
                {
                    nidorinoBattle.Top.Set(-346, 1f);
                    Texture2D nidorinoBattle2texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle2");
                    nidorinoBattle.SetImage(nidorinoBattle2texture);
                    Texture2D gengarBattle2texture = ModContent.GetTexture("Terramon/UI/IntroMovie/GengarBattle2");
                    gengarBattle.SetImage(gengarBattle2texture);
                }

                if (scrollFastTimer > 1.45)
                {
                    Texture2D nidorinoBattle3texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle3");
                    nidorinoBattle.SetImage(nidorinoBattle3texture);
                    playNidoranRoar = true;
                }

                if (playNidoranRoar && playedNidoranRoar == 1)
                {
                    Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Cries/cryNidorino").WithVolume(.7f));
                    playNidoranRoar = false;
                    playedNidoranRoar = 2;
                }

                // shake nidoran when roaring

                if (scrollFastTimer > 1.55)
                {
                    nidorinoBattle.Top.Set(-346, 1f);
                }

                if (scrollFastTimer > 1.62)
                {
                    nidorinoBattle.Top.Set(-356, 1f);
                }

                if (scrollFastTimer > 1.69)
                {
                    nidorinoBattle.Top.Set(-346, 1f);
                }

                if (scrollFastTimer > 1.76)
                {
                    nidorinoBattle.Top.Set(-356, 1f);
                }

                if (scrollFastTimer > 1.83)
                {
                    nidorinoBattle.Top.Set(-346, 1f);
                }

                if (scrollFastTimer > 1.9)
                {
                    nidorinoBattle.Top.Set(-356, 1f);
                }

                if (scrollFastTimer > 1.97)
                {
                    nidorinoBattle.Top.Set(-346, 1f);
                    Texture2D gengarBattle1texture = ModContent.GetTexture("Terramon/UI/IntroMovie/GengarBattle1");
                    gengarBattle.SetImage(gengarBattle1texture);
                }

                if (scrollFastTimer > 2.04)
                {
                    nidorinoBattle.Top.Set(-356, 1f);
                }

                if (scrollFastTimer > 2.11)
                {
                    Texture2D nidorinoBattle1texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle1");
                    nidorinoBattle.SetImage(nidorinoBattle1texture);
                }

                if (scrollFastTimer > 2.8 && scrollFastTimer < 3.2)
                {
                    if (startSlideBackGengar == 0)
                    {
                        startSlideBackGengar = gameTime.TotalGameTime.TotalSeconds;
                        endSlideBackGengar = startSlideBackGengar + 0.4;
                    }
                    Texture2D gengarBattle3texture = ModContent.GetTexture("Terramon/UI/IntroMovie/GengarBattle3");
                    gengarBattle.SetImage(gengarBattle3texture);
                    gengarBattle.HAlign = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.5f, 0.25f, startSlideBackGengar, endSlideBackGengar, Easing.OutExpo);
                }

                if (scrollFastTimer > 3.2 && scrollFastTimer < 3.45)
                {
                    if (startSlashGengar == 0)
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Cries/cryGengar").WithVolume(.7f));
                        startSlashGengar = gameTime.TotalGameTime.TotalSeconds;
                        endSlashGengar = startSlashGengar + 0.25;
                    }
                    Texture2D gengarBattle4texture = ModContent.GetTexture("Terramon/UI/IntroMovie/GengarBattle4");
                    gengarBattle.SetImage(gengarBattle4texture);
                    gengarBattle.HAlign = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.25f, 0.5f, startSlashGengar, endSlashGengar, Easing.Out);
                }

                if (scrollFastTimer > 3.2 && scrollFastTimer < 3.45)
                {
                    Texture2D gengarBattle5texture = ModContent.GetTexture("Terramon/UI/IntroMovie/GengarBattle5");
                    gengarBattle.SetImage(gengarBattle5texture);
                }

                if (scrollFastTimer > 3.1 && scrollFastTimer < 3.2)
                {
                    Texture2D nidorinoBattle2texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle2");
                    nidorinoBattle.SetImage(nidorinoBattle2texture);
                }

                if (scrollFastTimer > 3.2 && scrollFastTimer < 3.6)
                {
                    if (startNidorinoDodge == 0)
                    {
                        startNidorinoDodge = gameTime.TotalGameTime.TotalSeconds;
                        endNidorinoDodge = startNidorinoDodge + 0.4;
                    }
                    Texture2D nidorinoBattle4texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle4");
                    nidorinoBattle.SetImage(nidorinoBattle4texture);
                    nidorinoBattle.HAlign = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.5f, 0.9f, startNidorinoDodge, endNidorinoDodge, Easing.None);
                }

                if (scrollFastTimer > 3.6 && scrollFastTimer < 3.7)
                {
                    Texture2D nidorinoBattle1texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle1");
                    nidorinoBattle.SetImage(nidorinoBattle1texture);
                }

                if (scrollFastTimer > 3.7 && scrollFastTimer < 3.8)
                {
                    Texture2D nidorinoBattle2texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle2");
                    nidorinoBattle.SetImage(nidorinoBattle2texture);
                }

                if (scrollFastTimer > 3.8 && scrollFastTimer < 4.3)
                {
                    if (startNidorinoJumpForward == 0)
                    {
                        startNidorinoJumpForward = gameTime.TotalGameTime.TotalSeconds;
                        endNidorinoJumpForward = startNidorinoJumpForward + 0.5;
                    }
                    Texture2D nidorinoBattle4texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle4");
                    nidorinoBattle.SetImage(nidorinoBattle4texture);
                    nidorinoBattle.HAlign = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.9f, 0.5f, startNidorinoJumpForward, endNidorinoJumpForward, Easing.None);
                }

                if (scrollFastTimer > 4.5 && scrollFastTimer < 4.8)
                {
                    if (startSmallHopBack1 == 0)
                    {
                        startSmallHopBack1 = gameTime.TotalGameTime.TotalSeconds;
                        endSmallHopBack1 = startSmallHopBack1 + 0.3;
                    }
                    Texture2D nidorinoBattle4texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle4");
                    nidorinoBattle.SetImage(nidorinoBattle4texture);
                    nidorinoBattle.HAlign = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.5f, 0.7f, startSmallHopBack1, endSmallHopBack1, Easing.None);
                }

                if (scrollFastTimer > 5.1 && scrollFastTimer < 5.4)
                {
                    if (startSmallHopBack2 == 0)
                    {
                        startSmallHopBack2 = gameTime.TotalGameTime.TotalSeconds;
                        endSmallHopBack2 = startSmallHopBack2 + 0.3;
                    }
                    Texture2D nidorinoBattle4texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle4");
                    nidorinoBattle.SetImage(nidorinoBattle4texture);
                    nidorinoBattle.HAlign = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.7f, 0.9f, startSmallHopBack2, endSmallHopBack2, Easing.None);
                }

                if (scrollFastTimer > 4.5 && scrollFastTimer < 5.9)
                {
                    Texture2D gengarBattle2texture = ModContent.GetTexture("Terramon/UI/IntroMovie/GengarBattle2");
                    gengarBattle.SetImage(gengarBattle2texture);
                }

                if (scrollFastTimer > 6)
                {
                    Texture2D nidorinoBattle2texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle2");
                    nidorinoBattle.SetImage(nidorinoBattle2texture);
                }

                if (scrollFastTimer > 6.8 && scrollFastTimer < 8.1)
                {
                    if (startFinalJump == 0)
                    {
                        startFinalJump = gameTime.TotalGameTime.TotalSeconds;
                        endFinalJump = startFinalJump + 1.6;
                    }
                    Texture2D nidorinoBattle5texture = ModContent.GetTexture("Terramon/UI/IntroMovie/NidorinoBattle5");
                    nidorinoBattle.SetImage(nidorinoBattle5texture);
                    nidorinoBattle.HAlign = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.9f, 0.35f, startFinalJump, endFinalJump, Easing.None);
                    nidorinoBattle.Top.Pixels = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, -356, -436, startFinalJump, endFinalJump, Easing.None);
                    whiteFlash._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0f, 1f, startFinalJump, endFinalJump - 0.8, Easing.None);
                }

                if (scrollFastTimer > 8.1 && !movieFinished)
                {
                    nidorinoBattle._visibilityActive = 0f;
                    gengarBattle._visibilityActive = 0f;
                    whiteFlash._visibilityActive = 0f;
		            treesScrolling1._visibilityActive = 0f;
                }

                if (scrollFastTimer > 10.5 && !movieFinished)
                {
                    // remove literally every single cutscene movie element
                    marginBarTop.Remove();
                    marginBarBottom.Remove();
                    treesScrolling1.Remove();
                    bottombushes.Remove();
                    gengarFaceRight.Remove();
                    gengarOrangeBg.Remove();
                    gengarBattle.Remove();
                    nidorinoFaceLeft.Remove();
                    nidorinoOrangeBg.Remove();
                    nidorinoBattle.Remove();
                    smallBush.Remove();
                    whiteFlash.Remove();

                    // show things

                    mainPanel.Append(starterselectmenu);
                    mainPanel.Append(shaderBar1);
                    mainPanel.Append(shaderBar2);
                    mainPanel.Append(shaderBar3);
                    mainPanel.Append(bartop);
                    mainPanel.Append(barbottom);

                    fanMadeModText.Remove();
                    mainPanel.Append(fanMadeModText);

                    testmenu.Remove();
                    mainPanel.Append(testmenu);
                    testmenu2.Remove();
                    testmenu.Append(testmenu2);
                    pokemonNameText.Remove();
                    mainPanel.Append(pokemonNameText);
                    pokemonDescText.Remove();
                    mainPanel.Append(pokemonDescText);

                    // add black cover

                    Texture2D blackTexture = ModContent.GetTexture("Terramon/UI/IntroMovie/BlackCover");
                    blackCover = new UIImagez(blackTexture);
                    blackCover._visibilityActive = 1f;
                    mainPanel.Append(blackCover);

                    fanMadeModText.SetText("Terramon is a fan-made mod. All rights belong to Nintendo & Game Freak.", 1.1f, false);

                    movieFinished = true;
                }

                if (scrollFastTimer > 10.5 && scrollFastTimer < 11.5)
                {
                    if (startBlackCover == 0)
                    {
                        startBlackCover = gameTime.TotalGameTime.TotalSeconds;
                        endBlackCover = startBlackCover + 1;
                    }
                    blackCover._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f, startBlackCover, endBlackCover, Easing.None);
                }

                if (scrollFastTimer > 11.5)
                {
                    blackCover.Remove();
                }
            }

            if (flashWhite1 == 1 && gameTime.TotalGameTime.TotalSeconds > endflashwhite1 && flashWhite2 == 0)
            {
                whiteFlash._visibilityActive = 0f;
            }

            // Remove fan-made mod alert after some time
            if (fanMadeModTimer > 6 && !fanMadeModTextShown) // TEMPORARY!
            {
                fanMadeModText.SetText("");
                fanMadeModTextShown = true;
                
                //append everything else!

                testmenu.SetText("Welcome to the world of Pokémon! Thank you for downloading this mod!", 1.1f, false);
                testmenu2.SetText("Now, please choose your desired starter Pokémon!", 1.1f, false);

                mainPanel.Append(bulbasaurTextureButton);
                mainPanel.Append(chikoritaTextureButton);
                mainPanel.Append(treeckoTextureButton);
                mainPanel.Append(turtwigTextureButton);
                mainPanel.Append(snivyTextureButton);
                mainPanel.Append(chespinTextureButton);
                mainPanel.Append(rowletTextureButton);

                mainPanel.Append(squirtleTextureButton);
                mainPanel.Append(totodileTextureButton);
                mainPanel.Append(mudkipTextureButton);
                mainPanel.Append(piplupTextureButton);
                mainPanel.Append(oshawottTextureButton);
                mainPanel.Append(froakieTextureButton);
                mainPanel.Append(popplioTextureButton);

                mainPanel.Append(charmanderTextureButton);
                mainPanel.Append(cyndaquilTextureButton);
                mainPanel.Append(torchicTextureButton);
                mainPanel.Append(chimcharTextureButton);
                mainPanel.Append(tepigTextureButton);
                mainPanel.Append(fennekinTextureButton);
                mainPanel.Append(littenTextureButton);
            }

            // Check if pokemon was just selected
            if (didSelectStarter == 1)
            {
                start = gameTime.TotalGameTime.TotalSeconds;
                charmanderTextureButton.FadeOut(1000);
                squirtleTextureButton.FadeOut(1000);
                bulbasaurTextureButton.FadeOut(1000);

                end = start + 2;
                didSelectStarter = 2;
            }

            if (didSelectStarter == 2 && gameTime.TotalGameTime.TotalSeconds < end)
            {
                render = false;

                testmenu.SetText("", 1.1f, false);
                testmenu2.SetText("", 1.1f, false);
                pokemonNameText.SetText("", 1.1f, false);
                pokemonDescText.SetText("", 1.1f, false);

                bartop.VAlign = (float)Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0, -0.2, start, end, Easing.Out);
                barbottom.VAlign = (float)Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1, 1.2, start, end, Easing.Out);
                mainPanel.BackgroundColor = new Color(97, 97, 97) * (float)Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.7, 0, start, end, Easing.None);
                
                starterselectmenu._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f, start, end, Easing.None);
                shaderBar1._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f, start, end, Easing.None);
                shaderBar2._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f, start, end, Easing.None);
                shaderBar3._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f, start, end, Easing.None);

                // fade out pkmn

                //bulbasaurTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f, start, end - 1, Easing.None);
                chikoritaTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                treeckoTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                turtwigTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                snivyTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                chespinTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                rowletTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);

                //squirtleTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f, start, end - 1, Easing.None);
                totodileTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                mudkipTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                piplupTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                oshawottTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                froakieTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                popplioTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);

                //charmanderTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 1f, 0f, start, end - 1, Easing.None);
                cyndaquilTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                torchicTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                chimcharTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                tepigTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                fennekinTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
                littenTextureButton._visibilityActive = Interpolation.ValueAt(gameTime.TotalGameTime.TotalSeconds, 0.4f, 0f, start, end - 1, Easing.None);
            }

            // Move bars across screen

            if (shaderBar1MoveTimer > 0.01)
            {
                shaderBar1.HAlign += shaderBar1Speed;
                shaderBar2.HAlign += shaderBar2Speed;
                shaderBar3.HAlign += shaderBar3Speed;
                shaderBar1MoveTimer = 0;
            }

            if (shaderBar1.HAlign > 1.3f)
            {
                shaderBar1.HAlign = -0.9f;
                var n = Main.rand.NextDouble() * (0.85 - -0.1) + -0.1;
                shaderBar1.VAlign = (float)n;

                // recalculate the speed

                double s = Main.rand.NextDouble() * (0.008 - 0.003) + 0.003;
                shaderBar1Speed = (float)s;
            }

            if (shaderBar2.HAlign > 1.3f)
            {
                shaderBar2.HAlign = -0.9f;
                var n = Main.rand.NextDouble() * (0.85 - -0.1) + -0.1;
                shaderBar2.VAlign = (float)n;

                // recalculate the speed

                double s = Main.rand.NextDouble() * (0.008 - 0) + 0.003;
                shaderBar2Speed = (float)s;
            }

            if (shaderBar3.HAlign > 1.3f)
            {
                shaderBar3.HAlign = -0.9f;
                var n = Main.rand.NextDouble() * (0.85 - -0.1) + -0.1;
                shaderBar3.VAlign = (float)n;

                // recalculate the speed

                double s = Main.rand.NextDouble() * (0.008 - 0.003) + 0.003;
                shaderBar3Speed = (float)s;
            }

            // Don't delete this or the UIElements attached to this UIState will cease to function.
            base.Update(gameTime);
        }

        private void bulbasaurHovered(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!render) return;
            pokemonNameText.SetText("Bulbasaur", 1.1f, false);
            pokemonDescText.SetText("A strange seed was planted on its back at birth. The plant sprouts and grows with this Pokémon.", 1.1f, false);
        }
        private void squirtleHovered(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!render) return;
            pokemonNameText.SetText("Squirtle", 1.1f, false);
            pokemonDescText.SetText("It shelters itself in its shell, then strikes back with spouts of water at every opportunity.", 1.1f, false);
        }
        private void charmanderHovered(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!render) return;
            pokemonNameText.SetText("Charmander", 1.1f, false);
            pokemonDescText.SetText("The flame on its tail indicates Charmander’s life force. If it is healthy, the flame burns brightly.", 1.1f, false);
        }
        private void unHovered(UIMouseEvent evt, UIElement listeningElement)
        {
            pokemonNameText.SetText("", 1.1f, false);
            pokemonDescText.SetText("", 1.1f, false);
        }
        private void bulbasaurTextureButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            didSelectStarter = 1;

            TerramonPlayer p = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            Mod mod = ModContent.GetInstance<TerramonMod>();
            Player player = Main.LocalPlayer;
            Main.PlaySound(SoundID.MenuOpen);
            Main.PlaySound(SoundID.Coins);
            p.StarterChosen = true;

            UISidebar.Visible = true;
            p.firstslotname = "Bulbasaur";
            p.PartySlot1 = new PokemonData
            {
                pokemon = "Bulbasaur",
                pokeballType = 1,
                Level = 5,
                ExpToNext = ExpLookupTable.ToNextLevel(5, ExpGroup.MediumSlow)
            };
            p.PartySlot1.HP = p.PartySlot1.MaxHP;

            // Register in pokedex
            p.PokedexCompletion[BaseMove.PokemonIdFromName(p.firstslotname) - 1] = 1;

            Visible = false;
            ModContent.GetInstance<TerramonMod>()._exampleUserInterface.SetState(null);
        }

        private void charmanderTextureButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            didSelectStarter = 1;

            TerramonPlayer p = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            Mod mod = ModContent.GetInstance<TerramonMod>();
            Player player = Main.LocalPlayer;
            Main.PlaySound(SoundID.MenuOpen);
            Main.PlaySound(SoundID.Coins);
            p.StarterChosen = true;

            UISidebar.Visible = true;
            p.firstslotname = "Charmander";
            p.PartySlot1 = new PokemonData
            {
                pokemon = "Charmander",
                pokeballType = 1,
                Level = 5,
                ExpToNext = ExpLookupTable.ToNextLevel(5, ExpGroup.MediumSlow)
            };
            p.PartySlot1.HP = p.PartySlot1.MaxHP;

            // Register in pokedex
            p.PokedexCompletion[BaseMove.PokemonIdFromName(p.firstslotname) - 1] = 1;

            Visible = false;
            ModContent.GetInstance<TerramonMod>()._exampleUserInterface.SetState(null);
        }

        private void squirtleTextureButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            didSelectStarter = 1;

            TerramonPlayer p = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            Mod mod = ModContent.GetInstance<TerramonMod>();
            Player player = Main.LocalPlayer;
            Main.PlaySound(SoundID.MenuOpen);
            Main.PlaySound(SoundID.Coins);
            p.StarterChosen = true;

            UISidebar.Visible = true;
            p.firstslotname = "Squirtle";
            p.PartySlot1 = new PokemonData
            {
                pokemon = "Squirtle",
                pokeballType = 1,
                Level = 5,
                ExpToNext = ExpLookupTable.ToNextLevel(5, ExpGroup.MediumSlow)
            };
            p.PartySlot1.HP = p.PartySlot1.MaxHP;

            // Register in pokedex
            p.PokedexCompletion[BaseMove.PokemonIdFromName(p.firstslotname) - 1] = 1;

            Visible = false;
            ModContent.GetInstance<TerramonMod>()._exampleUserInterface.SetState(null);
        }
    }
}