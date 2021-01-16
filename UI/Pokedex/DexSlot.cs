using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Razorwing.Framework.Localisation;
using Terramon.Players;

namespace Terramon.UI.Pokedex
{
    // This UIHoverImageButton class inherits from UIImageButton. 
    // Inheriting is a great tool for UI design. 
    // By inheriting, we get the Image drawing, MouseOver sound, and fading for free from UIImageButton
    // We've added some code to allow the Button to show a text tooltip while hovered. 
    public class DexSlot : UIElement
    {
		private Texture2D _texture;

		private string HoverText;

		public float ImageScale = 2f;

		public float _visibilityActive = 0.7f;

		public Color drawcolor = Color.White;

		public int constantRange;

		public bool caught = false;

		public DexSlot(int id)
		{
			string slotName = BaseMove.PokemonNameFromId(id);
			_texture = ModContent.GetTexture($"Terramon/Minisprites/Regular/mini{slotName}");
			Width.Set(_texture.Width, 0f);
			Height.Set(_texture.Height, 0f);

			constantRange = id - 1;

			drawcolor = Color.White;
		}

		public void UpdateId(int id)
		{
			string slotName = BaseMove.PokemonNameFromId(id);
			if (slotName == "NOEXIST")
			{
				_texture = ModContent.GetTexture("Terramon/Pokemon/Empty");
			} else
			{
				_texture = ModContent.GetTexture($"Terramon/Minisprites/Regular/mini{slotName}");
			}

			if (Main.LocalPlayer.GetModPlayer<TerramonPlayer>().PokedexCompletion.Length > 0)
			{
				if (id > 151)
				{
					caught = false;
					return;
				}

				if (Main.LocalPlayer.GetModPlayer<TerramonPlayer>().PokedexCompletion[id - 1] == 1)
				{
					caught = true;
				}
				else
				{
					caught = false;
				}
			}
		}

		public void SetHoverText(string _text)
		{
			HoverText = _text;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (ModContent.GetInstance<TerramonMod>().summaryUI.hoveredHitboxes[constantRange] == 1)
			{
				_visibilityActive = 1f;
			} else
			{
				_visibilityActive = 0.7f;
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			if (caught)
			{
				spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - ImageScale) / 2f, texture: _texture, sourceRectangle: null, color: drawcolor * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: ImageScale, effects: SpriteEffects.None, layerDepth: 0f);

				if (ContainsPoint(Main.MouseScreen))
				{
					Main.hoverItemName = HoverText;
				}
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		}

		public void SetVisibility(float visibility)
		{
			_visibilityActive = MathHelper.Clamp(visibility, 0f, 1f);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			AnimatorUI dexInstance = ModContent.GetInstance<TerramonMod>().summaryUI;
			dexInstance.UpdateDexSlots();
		}
	}

	public class DexSlotHitbox : UIElement
	{
		private Texture2D _texture = ModContent.GetTexture("Terramon/UI/Pokedex/SlotHitbox");

		public UIText lockedNumbers;

		private string HoverText;

		public float ImageScale = 1f;

		public float _visibilityActive = 0f;

		public Color drawcolor = Color.White;

		public int constantRange;

		public string slotName;

		public bool caught = false;

		public override void OnInitialize()
		{
			base.OnInitialize();
		}

		public DexSlotHitbox(int id)
		{
			Width.Set(_texture.Width, 0f);
			Height.Set(_texture.Height, 0f);

			constantRange = id - 1;

			slotName = BaseMove.PokemonNameFromId(id);
			if (slotName == "NOEXIST" || !caught)
			{
				HoverText = "";
			}
			else
			{
				HoverText = $"{TerramonMod.Localisation.GetLocalisedString(new LocalisedString(slotName))} [c/919191:<No. {id.ToString().PadLeft(3, '0')}>]";
			}

			if (lockedNumbers == null)
			{
				lockedNumbers = new UIText("");
				lockedNumbers.HAlign = 0.5f;
				lockedNumbers.VAlign = 0.5f;
				Append(lockedNumbers);
			}

			if (!caught) lockedNumbers.SetText(id.ToString().PadLeft(3, '0'));
		}

		public void UpdateId(int id)
		{
			lockedNumbers.SetText("");
			slotName = BaseMove.PokemonNameFromId(id);

			if (Main.LocalPlayer.GetModPlayer<TerramonPlayer>().PokedexCompletion.Length > 0)
			{
				if (id > 151)
				{
					caught = false;
					return;
				}

				if (Main.LocalPlayer.GetModPlayer<TerramonPlayer>().PokedexCompletion[id - 1] == 1)
				{
					caught = true;
				}
				else
				{
					caught = false;
				}
			}

			if (slotName == "NOEXIST" || !caught)
			{
				HoverText = "";
			}
			else
			{
				HoverText = $"{TerramonMod.Localisation.GetLocalisedString(new LocalisedString(slotName))} [c/919191:<No. {id.ToString().PadLeft(3, '0')}>]";
			}

			if (!caught && id < 152) lockedNumbers.SetText(id.ToString().PadLeft(3, '0'));
			else
			{
				lockedNumbers.SetText("");
			}
		}

		public bool justHovered = false;

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - ImageScale) / 2f, texture: _texture, sourceRectangle: null, color: drawcolor * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: ImageScale, effects: SpriteEffects.None, layerDepth: 0f);

			if (ContainsPoint(Main.MouseScreen) && ModContent.GetInstance<TerramonMod>().summaryUI.IsPokedexOpen)
			{
				if (!justHovered)
				{
					justHovered = true;
					if (HoverText != string.Empty)
					{
						Main.PlaySound(SoundID.MenuTick, Main.LocalPlayer.position, 0);
					}
				}

				if (Main.mouseLeft && caught)
				{
					if (Main.mouseLeftRelease)
					{
						Main.PlaySound(ModContent.GetInstance<TerramonMod>()
							.GetLegacySoundSlot(SoundType.Custom, "Sounds/Cries/cry" + slotName).WithVolume(0.55f));
					}
				}

				ModContent.GetInstance<TerramonMod>().summaryUI.hoveredHitboxes[constantRange] = 1;
				Main.hoverItemName = HoverText;
				Main.LocalPlayer.mouseInterface = true;
			} else
			{
				ModContent.GetInstance<TerramonMod>().summaryUI.hoveredHitboxes[constantRange] = 0;
				justHovered = false;
			}
		}

		public void SetVisibility(float visibility)
		{
			_visibilityActive = MathHelper.Clamp(visibility, 0f, 1f);
		}
	}

	public class DexArrow : UIElement
	{
		private Texture2D _texture;

		private DexArrowDirection _dir;

		private string HoverText;

		public float ImageScale = 0.90f;

		public float _visibilityActive = 1f;

		public Color drawcolor = Color.White;

		public DexArrow(DexArrowDirection direction)
		{
			string texturePath;
			_dir = direction;
			if (_dir == DexArrowDirection.Up)
			{
				texturePath = "Terramon/UI/Pokedex/PageUp";
				HoverText = "Go up";
			}
			else
			{
				texturePath = "Terramon/UI/Pokedex/PageDown";
				HoverText = "Go down";
			}
			_texture = ModContent.GetTexture(texturePath);
			Width.Set(_texture.Width, 0f);
			Height.Set(_texture.Height, 0f);

			drawcolor = Color.White;
		}

		public void SetHoverText(string _text)
		{
			HoverText = _text;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - ImageScale) / 2f, texture: _texture, sourceRectangle: null, color: drawcolor * _visibilityActive, rotation: 0f, origin: Vector2.Zero, scale: ImageScale, effects: SpriteEffects.None, layerDepth: 0f);
			if (ContainsPoint(Main.MouseScreen)) Main.LocalPlayer.mouseInterface = true;
			if (ModContent.GetInstance<TerramonMod>().summaryUI.range[0] == 1 && _dir == DexArrowDirection.Up)
			{
				_texture = ModContent.GetTexture("Terramon/UI/Pokedex/PageUpDeactivated");
				return;
			}
			if (ModContent.GetInstance<TerramonMod>().summaryUI.range[0] == 145 && _dir == DexArrowDirection.Down)
			{
				_texture = ModContent.GetTexture("Terramon/UI/Pokedex/PageDownDeactivated");
				return;
			}

			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
				Main.hoverItemName = HoverText;
				if (_dir == DexArrowDirection.Up)
				{
					_texture = ModContent.GetTexture("Terramon/UI/Pokedex/PageUpHover");
				} else
				{
					_texture = ModContent.GetTexture("Terramon/UI/Pokedex/PageDownHover");
				}

				// Handle click
				if (Main.mouseLeft)
				{
					if (Main.mouseLeftRelease)
					{
						Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/dexarrow").WithVolume(.7f));
						AnimatorUI dexInstance = ModContent.GetInstance<TerramonMod>().summaryUI;
						if (_dir == DexArrowDirection.Down)
						{
							for (int i = 0; i < 18; i++)
							{
								dexInstance.range[i] += 18;
							}
						} else
						{
							for (int i = 0; i < 18; i++)
							{
								dexInstance.range[i] -= 18;
							}
						}
					}
				}
			} else
			{
				if (_dir == DexArrowDirection.Up)
				{
					_texture = ModContent.GetTexture("Terramon/UI/Pokedex/PageUp");
				}
				else
				{
					_texture = ModContent.GetTexture("Terramon/UI/Pokedex/PageDown");
				}
			}
		}

		public void SetVisibility(float visibility)
		{
			_visibilityActive = MathHelper.Clamp(visibility, 0f, 1f);
		}

		public enum DexArrowDirection
		{
			Up,
			Down
		}
	}
}