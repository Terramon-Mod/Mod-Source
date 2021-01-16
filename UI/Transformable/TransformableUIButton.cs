using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Razorwing.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace Terramon.UI.Transformable
{
    public class TransformableUIButton : Drawable
    {
        public Color ActiveColour { get; set; } = Color.White;
        public Color InactiveColour { get; set; } = Color.White * 0.4f;

        private bool lastState = false;

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
        }

        public override void OnActivate()
        {
            base.OnActivate();
            if (!lastState)
            {
                this.FadeColour(ActiveColour, 200d, Easing.Out);
                lastState = true;
            }
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            if (lastState)
            {
                this.FadeColour(InactiveColour, 200d, Easing.Out);
                lastState = false;
            }
        }
    }
}
