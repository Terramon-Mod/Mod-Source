using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModKit;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Razorwing.Framework.Graphics.DebugDrawing
{
    public class DebugDrawings : Drawable
    {
        public UIElement Target = null;
        public string DebugMouseText = string.Empty;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Target != null)
            {
                DebugMouseText = string.Empty;
                DrawTree(Target, spriteBatch);
                if (DebugMouseText != string.Empty)
                {
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText,
                        DebugMouseText, new Vector2(Main.mouseX, Main.mouseY+15),
                        Color.White, 0, Vector2.Zero, Vector2.One,
                        400);
                    DebugMouseText = string.Empty;
                }
            }
        }

        private void DrawTree(UIElement el, SpriteBatch spriteBatch)
        {
            var list = Reflect.GetF<List<UIElement>>(el, "Elements");
            if (el is Drawable dr) 
            {
                //if(dr.Active)
                    Draw(el, spriteBatch);
            }
            else
            {
                Draw(el, spriteBatch);
            }
            foreach (UIElement t in list)
            {
                var r = t.GetDimensions().ToRectangle();
                if (r.Contains(Main.mouseX, Main.mouseY))
                {
                    DebugMouseText = $"{t} Pos: {r.X},{r.Y} Size: {r.Width},{r.Height}";
                }
                //Draw(t, spriteBatch);
                DrawTree(t, spriteBatch);
            }
        }

        private void Draw(UIElement self, SpriteBatch spriteBatch)
        {
            var rect = self.GetDimensions().ToRectangle();
            var pixel = Main.magicPixel;
            var color = Color.Red;
            Drawable d = self as Drawable;
            if (d != null)
                color = d.Colour;
            //Bound Rectangle
            spriteBatch.Draw(pixel, new Rectangle(rect.X-3, rect.Y,2, rect.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y-3,rect.Width, 2), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width + 3, rect.Y,2, rect.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height + 3,rect.Width, 2), color);

            if (d != null)
            {
                //Origin Point
                //var op = d.OriginPoint + (d.RelativeOriginPoit * d.Size);
                spriteBatch.Draw(pixel, new Rectangle((int)(rect.X /*+ op.X*/ - 3), (int)(rect.Y /*+ op.Y*/ - 3), 6, 6), Color.Gold);
            }
            
        }
    }
}
