using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace Terramon.UI
{
    // This UIHoverImageButton class inherits from UIImageButton. 
    // Inheriting is a great tool for UI design. 
    // By inheriting, we get the Image drawing, MouseOver sound, and fading for free from UIImageButton
    // We've added some code to allow the Button to show a text tooltip while hovered. 
    internal class SidebarClass : Drawable
    {
        internal string HoverText;

        public SidebarClass(Texture2D texture, string hoverText)
        {
            HoverText = hoverText;
            Texture = texture;
        }

        public SidebarClass(string hoverText)
        {
            HoverText = hoverText;
        }

        private bool hovered = false;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (IsMouseHovering)
            {
                if (!hovered)
                {
                    this.ScaleTo(1.2f, 200);
                    //this.FlashColour(Color.LightCoral, 200);
                    hovered = true;
                }
                Main.hoverItemName = HoverText;
            }
            else
            {
                if (hovered)
                {
                    this.ScaleTo(1, 50);
                    hovered = false;
                }
            }
            base.DrawSelf(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.
            // Checking ContainsPoint and then setting mouseInterface to true is very common. This causes clicks on this UIElement to not cause the player to use current items. 
            if (ContainsPoint(Main.MouseScreen)) Main.LocalPlayer.mouseInterface = true;
        }
    }
}