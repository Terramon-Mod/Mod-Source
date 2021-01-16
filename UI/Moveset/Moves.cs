using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terramon.UI.Moveset
{
    // ExampleUIs visibility is toggled by typing "/coin" in chat. (See CoinCommand.cs)
    // ExampleUI is a simple UI example showing how to use UIPanel, UIImageButton, and even a custom UIElement.
    internal class Moves : UIState
    {
        public SidebarPanel mainPanel;
        public static bool Visible;
        public bool lightmode = true;

        public Texture2D firstmovetexture;
        public SidebarClass firstmove;
        private UIText firstmovename;

        public Texture2D secondmovetexture;
        public SidebarClass secondmove;
        private UIText secondmovename;

        public Texture2D thirdmovetexture;
        public SidebarClass thirdmove;
        private UIText thirdmovename;

        public Texture2D fourthmovetexture;
        public SidebarClass fourthmove;
        private UIText fourthmovename;


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

            mainPanel = new SidebarPanel();
            mainPanel.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            mainPanel.HAlign = 1.05f;
            mainPanel.VAlign = 1.05f;
            mainPanel.Width.Set(190, 0f);
            mainPanel.Height.Set(135f, 0f);
            mainPanel.BackgroundColor = new Color(44, 61, 158) * 0.65f;

            Texture2D firstmovetexture = ModContent.GetTexture("Terramon/UI/Moveset/NormalType");
            firstmove = new SidebarClass(firstmovetexture, "Normal Type | PP: 35/35");
            firstmove.HAlign = 0.05f; // 1
            firstmove.VAlign = 0.1f; // 1
            firstmove.Width.Set(16, 0);
            firstmove.Height.Set(16, 0);
            mainPanel.Append(firstmove);

            firstmovename = new UIText("0/0");
            firstmovename.HAlign = 0.25f;
            firstmovename.VAlign = 0.1f;
            firstmovename.SetText("Scratch");
            mainPanel.Append(firstmovename);

            Texture2D secondmovetexture = ModContent.GetTexture("Terramon/UI/Moveset/EmptyType");
            secondmove = new SidebarClass(secondmovetexture, "");
            secondmove.HAlign = 0.05f; // 1
            secondmove.VAlign = 0.3f; // 1
            secondmove.Width.Set(16, 0);
            secondmove.Height.Set(16, 0);
            mainPanel.Append(secondmove);

            Texture2D thirdmovetexture = ModContent.GetTexture("Terramon/UI/Moveset/EmptyType");
            thirdmove = new SidebarClass(thirdmovetexture, "");
            thirdmove.HAlign = 0.05f; // 1
            thirdmove.VAlign = 0.5f; // 1
            thirdmove.Width.Set(16, 0);
            thirdmove.Height.Set(16, 0);
            mainPanel.Append(thirdmove);

            Texture2D fourthmovetexture = ModContent.GetTexture("Terramon/UI/Moveset/EmptyType");
            fourthmove = new SidebarClass(fourthmovetexture, "");
            fourthmove.HAlign = 0.05f; // 1
            fourthmove.VAlign = 0.7f; // 1
            fourthmove.Width.Set(16, 0);
            fourthmove.Height.Set(16, 0);
            mainPanel.Append(fourthmove);

            Append(mainPanel);
            // As a recap, ExampleUI is a UIState, meaning it covers the whole screen. We attach mainPanel to ExampleUI some distance from the top left corner.
            // We then place playButton, closeButton, and moneyDiplay onto mainPanel so we can easily place these UIElements relative to mainPanel.
            // Since mainPanel will move, this proper organization will move playButton, closeButton, and moneyDiplay properly when mainPanel moves.
        }

        public override void Update(GameTime gameTime)
        {
            // Don't delete this or the UIElements attached to this UIState will cease to function.
            base.Update(gameTime);
        }
    }
}