using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terramon.UI.Battling
{
    // This UIHoverImageButton class inherits from UIImageButton. 
    // Inheriting is a great tool for UI design. 
    // By inheriting, we get the Image drawing, MouseOver sound, and fading for free from UIImageButton
    // We've added some code to allow the Button to show a text tooltip while hovered. 
    public class BattleEXPBar : Drawable
    {
		//private Texture2D _texture;
        private Bar Health;

		public string HoverText;

		private Texture2D _texture;
		private Texture2D _textureBack;

		private Texture2D _textureOutline;

		public float ImageScale = 1f;

		public float _visibilityActive = 1f;

		public bool local = true;

		public SoundEffectInstance lowHPSoundInstance;

		// These vars determine the color of the HP bar and also the fill percent.
		// If more than half of HP, bar is green.
		// If less than half and more than one fifth, bar is yellow.
		// If less than one fifth bar is red.
		public Color drawcolor = Color.White;
		public float fill = 1f;
		public bool setFill = true;

        public float ActuallScale => Health.Scale.X;

		public BattleEXPBar()
        {
            Append(Health = new Bar()
            {
                TextureName = "Terramon/UI/Battling/EXPBarFill",
                Parent = this,
            });
			_texture = ModContent.GetTexture("Terramon/UI/Battling/EXPBarFill");
			_textureBack = ModContent.GetTexture("Terramon/UI/Battling/EXPBarBack");
			_textureOutline = ModContent.GetTexture("Terramon/UI/Battling/EXPBar");
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(position: GetDimensions().Position() + _textureBack.Size() * (1f - ImageScale) / 2f, texture: _textureBack, sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: new Vector2(1f, 1f), effects: SpriteEffects.None, layerDepth: 0f);
			spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - ImageScale) / 2f, texture: _texture, sourceRectangle: null, color: drawcolor, rotation: 0f, origin: Vector2.Zero, scale: new Vector2(fill, 1f), effects: SpriteEffects.None, layerDepth: 0f);
			spriteBatch.Draw(position: GetDimensions().Position() + _textureOutline.Size() * (1f - ImageScale) / 2f, texture: _textureOutline, sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: ImageScale, effects: SpriteEffects.None, layerDepth: 0f);
			if (IsMouseHovering)
			{
				Main.hoverItemName = HoverText;
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			//Health.Update(gameTime);//Manual update

			if (ContainsPoint(Main.MouseScreen)) Main.LocalPlayer.mouseInterface = true;
		}

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            //base.DrawChildren(spriteBatch);
        }

        public class Bar : Drawable
        {
            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                var ImageScale = 1f;
                var drawcolor = Color.LightGreen;
                if (Parent is BattleHPBar bar)
                {
                    ImageScale = bar.ImageScale;
                    drawcolor = bar.drawcolor;
                }
				
			}
        }

	}
}