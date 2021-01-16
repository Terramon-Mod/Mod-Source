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
    public class SummarySprite : UIElement
    {
		private Texture2D _texture;

		private Rectangle frame;

		public float ImageScale = 1f;

		public float _visibilityActive = 1f;

		public Color drawcolor = Color.White;

		public SummarySprite(Texture2D texture)
		{
			_texture = texture;
			frame = _texture.Frame(1, 2, 0, 0);
			Width.Set(_texture.Width, 0f);
			Height.Set(_texture.Height, 0f);
		}

		public void SetImage(Texture2D texture)
		{
			_texture = texture;
			Width.Set(_texture.Width, 0f);
			Height.Set(_texture.Height, 0f);
		}

		int frameTimer;
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			frameTimer++;
			if (frameTimer >= 45 && frameTimer < 90)
			{
				frame = _texture.Frame(1, 2, 0, 1);
			}
			if (frameTimer >= 90)
			{
				frame = _texture.Frame(1, 2, 0, 0);
				frameTimer = 0;
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - ImageScale) / 2f, texture: _texture, sourceRectangle: frame, color: Color.White * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: ImageScale, effects: SpriteEffects.FlipHorizontally, layerDepth: 0f);
		}

		public void SetVisibility(float visibility)
		{
			_visibilityActive = MathHelper.Clamp(visibility, 0f, 1f);
		}
	}
}