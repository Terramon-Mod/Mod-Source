using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.Graphics.Transforms;
using Razorwing.Framework.Threading;
using Razorwing.Framework.Timing;
using Terramon;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Razorwing.Framework.Graphics
{
    public class Drawable : Transformable
    {
        private Texture2D texture = null;
        public Texture2D Texture
        {
            get => texture;
            set
            {
                if(texture == value)
                    return;
                texture = value;
                Size = texture?.Bounds.Size() ?? Vector2.Zero;
            }
        }

        public string TextureName
        {
            get => Texture?.Name ?? "";
            set => Texture = ModContent.GetTexture(value);
        }

        public override GameTimeClock Clock
        {
            get => TerramonMod.Instance?.GameClock;
            set
            {

            }
        }

        public Vector2 RelativePosition
        {
            get => new Vector2(Left.Percent, Top.Percent);
            set
            {
                Left.Percent = value.X;
                Top.Percent = value.Y;
            }
        }

        public Vector2 Position
        {
            get => new Vector2(Left.Pixels, Top.Pixels);
            set
            {
                Left.Pixels = value.X;
                Top.Pixels = value.Y;
            }
        }

        public float X
        {
            get => Left.Pixels;
            set => Left.Pixels = value;
        }

        public float Y
        {
            get => Top.Pixels;
            set => Top.Pixels = value;
        }

        private Color colour = Color.White;
        public Color Colour
        {
            get => colour;
            set => colour = value;
        }

        public float Alpha
        {
            get => (float)colour.A / 255f;
            set => colour.A = (byte)MathHelper.Clamp(value * 255, 0, 255);
        }

        private float rotation;
        public float Rotation
        {
            get => rotation;
            set => rotation = value;
        }

        private Vector2 scale = Vector2.One;
        public Vector2 Scale
        {
            get => scale;
            set => scale = value;
        }

        public Vector2 Size
        {
            get => new Vector2(Width.Pixels, Height.Pixels);
            set
            {
                Width.Pixels = value.X;
                Height.Pixels = value.Y;
            }
        }

        public Vector2 RelativeSize
        {
            get => new Vector2(Width.Percent, Height.Percent);
            set
            {
                Width.Percent = value.X;
                Height.Percent = value.Y;
            }
        }


        public Vector2 OriginPoint { get; set; } = Vector2.Zero;

        private Rectangle? textureRect = null;
        /// <summary>
        /// Part of the texture to render
        /// </summary>
        public Rectangle? TextureRect
        {
            get => textureRect;
            set
            {
                textureRect = value;
                if (value.HasValue)
                    Size = value.Value.Size();
            }
        }

        protected internal ScheduledDelegate Schedule(Action action) => TerramonMod.Instance.Scheduler.AddDelayed(action, TransformDelay);

        /// <summary>
        /// Hide sprite.
        /// </summary>
        public virtual void Hide(double duration = 0) => this.FadeOut(duration);

        /// <summary>
        /// Show sprite.
        /// </summary>
        public virtual void Show(double duration = 0) => this.FadeIn(duration);

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (Texture != null && Alpha > 0.01f)// Don't render if we have nothing to render or this can't be seen
            {
                Vector2 position = GetDimensions().Position();
                position += texture.Size() * (new Vector2(1) - Scale) / 2f;
                position += OriginPoint;
                spriteBatch.Draw(texture, position, textureRect, Colour, rotation, OriginPoint, Scale, SpriteEffects.None, 0f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdateTransforms();//run before base.Update so transformations get 
            //applied before style recalculations
            base.Update(gameTime);
        }
    }

    public enum RotationDirection
    {
        Clockwise,
        CounterClockwise,
    }
    public enum Direction
    {
        Horizontal,
        Vertical,
    }
}
