using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Razorwing.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace Terramon.UI.Test
{
    public class TestState : UIState
    {
        public static Drawable shader;
        public static bool Visible;

        public override void OnInitialize()
        {
            base.OnInitialize();

            shader = new Drawable()
            {
                TextureName = "Terramon/Minisprites/Regular/miniEevee",
            };
            Append(shader);
        }

        private bool pres = false;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!pres && Main.keyState.IsKeyDown(Keys.C))
            {
                pres = true;
                //shader.ClearTransforms();
                shader.MoveTo(new Vector2(500f, 500f), 2000, Easing.InBack).Then()
                    .MoveTo(new Vector2(0), 2000).Then()
                    .MoveToRelative(new Vector2(0.5f, 0.5f), 2000).Then()
                    .MoveToRelative(new Vector2(0), 2000);
                shader.ScaleTo(new Vector2(10, 10), 2000, Easing.InOutElastic).Then()
                    .ScaleTo(new Vector2(5, 5), 2000).Then()
                    .ScaleTo(new Vector2(20, 20), 2000).Then()
                    .ScaleTo(1f, 2000);
                shader.FadeInFromZero(8000);
            }
            else if (Main.keyState.IsKeyUp(Keys.C))
            {
                pres = false;
            }
        }
    }
}
