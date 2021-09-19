using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terramon.UI
{
    public class UITypewriterText : UIElement
    {
        public string _textTarget = "";
        private object _text = "";
        private int letterCount = 0;

        private float _textScale = 1f;

        private Vector2 _textSize = Vector2.Zero;

        private bool _isLarge;

        private Color _color = Color.White;

        public bool needReset = false;

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

        public UITypewriterText(string text, float textScale = 1f, bool large = false)
        {
            InternalSetText(text, textScale, large);
        }

        public UITypewriterText(LocalizedText text, float textScale = 1f, bool large = false)
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
            if (text == _textTarget) return;
            letterCount = text.Length;
            needReset = true;
            _text = "";
            _textTarget = text;
            InternalSetText(text, _textScale, _isLarge);
        }

        public void SetText(LocalizedText text)
        {
            if (text.ToString() == _textTarget) return;
            letterCount = text.ToString().Length;
            needReset = true;
            _text = "";
            _textTarget = text.ToString();
            InternalSetText(text, _textScale, _isLarge);
        }

        public void SetText(string text, float textScale, bool large)
        {
            if (text == _textTarget) return;
            letterCount = text.ToString().Length;
            needReset = true;
            _text = "";
            _textTarget = text.ToString();
            InternalSetText(text, textScale, large);
        }

        public void SetText(LocalizedText text, float textScale, bool large)
        {
            if (text.ToString() == _textTarget) return;
            letterCount = text.ToString().Length;
            needReset = true;
            _text = "";
            _textTarget = text.ToString();
            InternalSetText(text, textScale, large);
        }

        private void InternalSetText(object text, float textScale, bool large)
        {
            Vector2 textSize = new Vector2((large ? Main.fontDeathText : Main.fontMouseText).MeasureString(text.ToString()).X, large ? 32f : 16f) * textScale;
            _textScale = textScale;
            _textSize = textSize;
            _isLarge = large;
            MinWidth.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
            MinHeight.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
        }

        public float timer = 0;
        public int letterCounterVariable = 0;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (needReset)
            {
                timer = 0;
                letterCounterVariable = 0;
                needReset = false;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // We have _text, which is what is actually being displayed, and _textTarget which we use to animate.

            if (_textTarget.Length != 0)
            {
                if (letterCounterVariable < _textTarget.Length)
                {
                    if (timer > 0.01)
                    {
                        _text += _textTarget[letterCounterVariable].ToString();
                        letterCounterVariable += 1;
                        timer = 0;
                    }
                }
            }
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
                //Utils.DrawBorderString(spriteBatch, Text, pos, _color, _textScale);
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, Text, pos, _color, 0f, Vector2.Zero, new Vector2(_textScale));
            }
        }
    }
}
