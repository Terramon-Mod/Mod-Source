using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terramon.UI
{
    // This UIHoverImageButton class inherits from UIImageButton. 
    // Inheriting is a great tool for UI design. 
    // By inheriting, we get the Image drawing, MouseOver sound, and fading for free from UIImageButton
    // We've added some code to allow the Button to show a text tooltip while hovered. 
    public class UIImagez : UIElement
    {
        private Texture2D _texture;

        public float ImageScale = 1f;

        public float _visibilityActive = 1f;

        public Color drawcolor = new Color(255, 255, 255, 255);

        public UIImagez(Texture2D texture)
        {
            _texture = texture;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
        }

        public void SetImage(Texture2D texture)
        {
            _texture = texture;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - ImageScale) / 2f, texture: _texture, sourceRectangle: null, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: ImageScale, effects: SpriteEffects.None, layerDepth: 0f);
        }
        public void SetVisibility(float visibility)
        {
            _visibilityActive = MathHelper.Clamp(visibility, 0f, 1f);
        }
    }
}