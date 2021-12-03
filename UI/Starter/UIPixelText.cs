using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terramon.UI.Starter
{
    public class UIPixelText : UIElement
    {
        private object _text = "";

        private float _textScale = 1f;

        private Vector2 _textSize = Vector2.Zero;

        private bool _isLarge;

        private Color _color = Color.White;

        public string Text => _text.ToString();

        public Color TextColor
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public UIPixelText(string text, float textScale = 1f, bool large = false)
        {
            InternalSetText(text, textScale, large);
        }

        public UIPixelText(LocalizedText text, float textScale = 1f, bool large = false)
        {
            InternalSetText(text, textScale, large);
        }

        public override void Recalculate()
        {
            InternalSetText(_text, _textScale, _isLarge);
            base.Recalculate();
        }

        public void SetText(string text)
        {
            InternalSetText(text, _textScale, _isLarge);
        }

        public void SetText(LocalizedText text)
        {
            InternalSetText(text, _textScale, _isLarge);
        }

        public void SetText(string text, float textScale, bool large)
        {
            InternalSetText(text, textScale, large);
        }

        public void SetText(LocalizedText text, float textScale, bool large)
        {
            InternalSetText(text, textScale, large);
        }

        private void InternalSetText(object text, float textScale, bool large)
        {
            Vector2 textSize = new Vector2((large ? Main.fontDeathText : Main.fontMouseText).MeasureString(text.ToString()).X, large ? 32f : 16f) * textScale;
            _text = text;
            _textScale = textScale;
            _textSize = textSize;
            _isLarge = large;
            MinWidth.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
            MinHeight.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle innerDimensions = GetInnerDimensions();
            Vector2 pos = innerDimensions.Position();
            if (_isLarge)
            {
                pos.Y -= 10f * _textScale;
            }
            else
            {
                pos.Y -= 2f * _textScale;
            }
            pos.X += (innerDimensions.Width - _textSize.X) * 0.5f;
            if (_isLarge)
            {
                Utils.DrawBorderStringBig(spriteBatch, Text, pos, _color, _textScale);
            }
            else
            {
                DrawBorderString(spriteBatch, Text, pos, _color, _textScale);
            }
        }

        public static Vector2 DrawBorderString(SpriteBatch sb, string text, Vector2 pos, Color color, float scale = 1f, float anchorx = 0f, float anchory = 0f, int maxCharactersDisplayed = -1)
        {
            if (maxCharactersDisplayed != -1 && text.Length > maxCharactersDisplayed)
            {
                text.Substring(0, maxCharactersDisplayed);
            }
            DynamicSpriteFont fontMouseText = Main.fontMouseText;
            Vector2 vector = fontMouseText.MeasureString(text);
            TextSnippet[] snippets = ChatManager.ParseMessage(text, Color.White).ToArray();
            ChatManager.ConvertNormalSnippets(snippets);
            int cum;
            ChatManager.DrawColorCodedString(sb, fontMouseText, snippets, pos, color, 0f, new Vector2(anchorx, anchory) * vector, new Vector2(scale), out cum, 1f);
            return vector * scale;
        }
    }
}
