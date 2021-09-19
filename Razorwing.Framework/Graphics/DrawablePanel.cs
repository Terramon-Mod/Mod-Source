using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.UI;

namespace Razorwing.Framework.Graphics
{
    public class DrawablePanel : Drawable
    {
		// Token: 0x06002120 RID: 8480 RVA: 0x0045C822 File Offset: 0x0045AA22
		public override void OnActivate()
		{
			if (DrawablePanel._borderTexture == null)
			{
				DrawablePanel._borderTexture = TextureManager.Load("Images/UI/PanelBorder");
			}
			if (DrawablePanel._backgroundTexture == null)
			{
				DrawablePanel._backgroundTexture = TextureManager.Load("Images/UI/PanelBackground");
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x0045C850 File Offset: 0x0045AA50
		public DrawablePanel()
		{
			base.SetPadding((float)DrawablePanel.CORNER_SIZE);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x0045C890 File Offset: 0x0045AA90
		private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			Point point2 = new Point(point.X + (int)dimensions.Width - DrawablePanel.CORNER_SIZE, point.Y + (int)dimensions.Height - DrawablePanel.CORNER_SIZE);
			int width = point2.X - point.X - DrawablePanel.CORNER_SIZE;
			int height = point2.Y - point.Y - DrawablePanel.CORNER_SIZE;
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE), new Rectangle?(new Rectangle(0, 0, DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE), new Rectangle?(new Rectangle(DrawablePanel.CORNER_SIZE + DrawablePanel.BAR_SIZE, 0, DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE), new Rectangle?(new Rectangle(0, DrawablePanel.CORNER_SIZE + DrawablePanel.BAR_SIZE, DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE), new Rectangle?(new Rectangle(DrawablePanel.CORNER_SIZE + DrawablePanel.BAR_SIZE, DrawablePanel.CORNER_SIZE + DrawablePanel.BAR_SIZE, DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + DrawablePanel.CORNER_SIZE, point.Y, width, DrawablePanel.CORNER_SIZE), new Rectangle?(new Rectangle(DrawablePanel.CORNER_SIZE, 0, DrawablePanel.BAR_SIZE, DrawablePanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + DrawablePanel.CORNER_SIZE, point2.Y, width, DrawablePanel.CORNER_SIZE), new Rectangle?(new Rectangle(DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE + DrawablePanel.BAR_SIZE, DrawablePanel.BAR_SIZE, DrawablePanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE, height), new Rectangle?(new Rectangle(0, DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE, DrawablePanel.BAR_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE, height), new Rectangle?(new Rectangle(DrawablePanel.CORNER_SIZE + DrawablePanel.BAR_SIZE, DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE, DrawablePanel.BAR_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + DrawablePanel.CORNER_SIZE, point.Y + DrawablePanel.CORNER_SIZE, width, height), new Rectangle?(new Rectangle(DrawablePanel.CORNER_SIZE, DrawablePanel.CORNER_SIZE, DrawablePanel.BAR_SIZE, DrawablePanel.BAR_SIZE)), color);
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x0045CB75 File Offset: 0x0045AD75
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			this.DrawPanel(spriteBatch, DrawablePanel._backgroundTexture, this.BackgroundColor);
			this.DrawPanel(spriteBatch, DrawablePanel._borderTexture, this.BorderColor);
		}

		// Token: 0x04003A99 RID: 15001
		private static int CORNER_SIZE = 12;

		// Token: 0x04003A9A RID: 15002
		private static int BAR_SIZE = 4;

		// Token: 0x04003A9B RID: 15003
		private static Texture2D _borderTexture;

		// Token: 0x04003A9C RID: 15004
		private static Texture2D _backgroundTexture;

		// Token: 0x04003A9D RID: 15005
		public Color BorderColor = Color.Black;

		// Token: 0x04003A9E RID: 15006
		public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;
	}
}
