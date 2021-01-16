using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terramon.UI
{
    // ExampleUIs visibility is toggled by typing "/coin" in chat. (See CoinCommand.cs)
    // ExampleUI is a simple UI example showing how to use UIPanel, UIImageButton, and even a custom UIElement.
    internal class PokegearUI : UIState
    {
        public DragableUIPanel mainPanel;
        public static bool Visible;

        // In OnInitialize, we place various UIElements onto our UIState (this class).
        // UIState classes have width and height equal to the full screen, because of this, usually we first define a UIElement that will act as the container for our UI.
        // We then place various other UIElement onto that container UIElement positioned relative to the container UIElement.
        public override void OnInitialize()
        {
            // Here we define our container UIElement. In DragableUIPanel.cs, you can see that DragableUIPanel is a UIPanel with a couple added features.
            mainPanel = new DragableUIPanel();
            mainPanel.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            mainPanel.Left.Set(75f, 0f);
            mainPanel.Top.Set(100f, 0f);
            mainPanel.Width.Set(210, 0f);
            mainPanel.Height.Set(133f, 0f);

            //pokemon icons


            // Next, we create another UIElement that we will place. Since we will be calling `mainPanel.Append(playButton);`, Left and Top are relative to the top left of the mainPanel UIElement. 
            // By properly nesting UIElements, we can position things relatively to each other easily.

            Texture2D pokegearmenu = ModContent.GetTexture("Terramon/UI/PokegearMenu");
            UIImagez pokegear = new UIImagez(pokegearmenu);
            pokegear.Left.Set(41, 0f);
            pokegear.Top.Set(0, 0f);
            pokegear.Width.Set(1, 0f);
            pokegear.Height.Set(1, 0f);
            mainPanel.Append(pokegear);

            Texture2D buttonDeleteTexture = ModContent.GetTexture("Terraria/UI/ButtonDelete");
            UIHoverImageButton closeButton =
                new UIHoverImageButton(buttonDeleteTexture,
                    Language.GetTextValue("LegacyInterface.52")); // Localized text for "Close"
            closeButton.Left.Set(13, 0f);
            closeButton.Top.Set(10, 0f);
            closeButton.Width.Set(30, 0f);
            closeButton.Height.Set(30, 0f);
            closeButton.OnClick += CloseButtonClicked;
            mainPanel.Append(closeButton);

            Texture2D eventsButtonTexture = ModContent.GetTexture("Terramon/UI/EventsButton");
            UIHoverImageButton
                eventsButton =
                    new UIHoverImageButton(eventsButtonTexture, "Browse Ongoing Events"); // Localized text for "Close"
            eventsButton.Left.Set(63, 0f);
            eventsButton.Top.Set(70, 0f);
            eventsButton.Width.Set(132, 0f);
            eventsButton.Height.Set(44, 0f);
            eventsButton.OnClick += EventsButtonClicked;
            mainPanel.Append(eventsButton);

            Append(mainPanel);

            // As a recap, ExampleUI is a UIState, meaning it covers the whole screen. We attach mainPanel to ExampleUI some distance from the top left corner.
            // We then place playButton, closeButton, and moneyDiplay onto mainPanel so we can easily place these UIElements relative to mainPanel.
            // Since mainPanel will move, this proper organization will move playButton, closeButton, and moneyDiplay properly when mainPanel moves.
        }

        //private Mod achLib = ModLoader.GetMod("AchievementLib");
        private Player player = Main.LocalPlayer;

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuOpen);
            Visible = false;
            PokegearUIEvents.Visible = false;
        }

        private void EventsButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuOpen);
            PokegearUIEvents.Visible = true;
        }
    }
}