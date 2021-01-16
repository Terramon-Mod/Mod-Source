using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.Localisation;
using Terramon.Players;
using Terramon.Pokemon;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
// ReSharper disable InvalidXmlDocComment

namespace Terramon.Items.Pokeballs.Inventory
{
    public abstract class BaseCaughtClass : ModItem
    {
        ///// <summary>
        /////     I think this is not needed. I want  to store what mon are here
        /////     we better need to store a type string <see cref="nameof(Charmander)" />
        ///// </summary>
        //public int PokemonNPC;

        public ILocalisedBindableString goText = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("go", "Go {0}!")));
        public ILocalisedBindableString retire1Text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("retire1", "{0}, switch out!\nCome back!")));
        public ILocalisedBindableString retire2Text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("retire2", "{0}, return!")));
        public ILocalisedBindableString retire3Text = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("retire3", "That's enough for now, {0}!")));
        public ILocalisedBindableString pokeName = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("*")));
        public ILocalisedBindableString pokeballTooltip = TerramonMod.Localisation.GetLocalisedString(
            new LocalisedString(("pokeball.tooltip", "Contains {0} \nLeft click to send out this Pokémon (or return it to this ball).\nRight click to add to your party."), "*"));


        public string CapturedPokemon
        {
            get => PokeData.Pokemon;
            set => PokeData.Pokemon = value;
        }
        public string PokemonName
        {
            get => PokeData.Pokemon;
            set => PokeData.Pokemon = value;
        }
        public string SmallSpritePath;
        //public int PartySlotNumber;
        public int Level
        {
            get => PokeData.Level;
            set => PokeData.Level = value;
        }
        public int Exp
        {
            get => PokeData.Exp;
            set => PokeData.Exp = value;
        }
        public int ExpToNext
        {
            get => PokeData.ExpToNext;
            set => PokeData.ExpToNext = value;
        }

        public bool isShiny
        {
            get => PokeData.IsShiny;
            set => PokeData.IsShiny = value;
        }

        public BaseMove[] Moves
        {
            get => PokeData.Moves;
            set => PokeData.Moves = value;
        }

        public override bool CloneNewInstances => true;

        public override void SetDefaults()
        {
            if(PokeData == null)
                PokeData = new PokemonData();

            item.damage = 20;

            item.width = 24;
            item.height = 24;

            item.useTime = 20;
            item.useStyle = 1;
            item.useAnimation = 20;
            item.UseSound = SoundID.Item2;
            item.accessory = false;
            item.shoot = 10;
            item.scale = 1f;

            item.noMelee = true;

            item.rare = 0;

            //Detour handle
            if (Main.netMode != NetmodeID.Server || det_CapturedPokemon == null)
                return;

            //PokemonNPC = det_PokemonNPC;
            //det_PokemonNPC = 0;
            PokemonName = det_PokemonName;
            det_PokemonName = null;
            //SmallSpritePath = det_SmallSpritePath; 
            //det_SmallSpritePath = null;
            CapturedPokemon = det_CapturedPokemon;
            det_CapturedPokemon = null;
            isShiny = det_isShiny;
            det_isShiny = false;
            if (!string.IsNullOrEmpty(det_Moves))
            {
                var arr = det_Moves.Split('|');
                for (int i = 0; i < arr.Length || i < 4; i++)
                    if (!string.IsNullOrEmpty(arr[i]))
                        Moves[i] = TerramonMod.GetMove(arr[i]);

            }
            det_Moves = null;
            if (det_Data != null)
            {
                PokeData = det_Data;
                det_Data = null;
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame,
            Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (SmallSpritePath == null)
            {
                var mon = TerramonMod.GetPokemon(CapturedPokemon);
                if (mon == null)
                    return true;
                if (isShiny)
                {
                    SmallSpritePath = mon.IconName + "_Shiny";
                } else
                {
                    SmallSpritePath = mon.IconName;
                }
            }

            Texture2D pokemonTexture = ModContent.GetTexture(SmallSpritePath);
            Texture2D itemTexture = Main.itemTexture[item.type];
            spriteBatch.Draw(itemTexture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(pokemonTexture, position + itemTexture.Size() * Main.inventoryScale - new Vector2(4, 18),
                pokemonTexture.Frame(), drawColor, 0f, pokemonTexture.Size() / 2f, Main.inventoryScale,
                SpriteEffects.None, 0);
            return false;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY,
            ref int type, ref int damage, ref float knockBack)
        {
            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            var pokeBuff = ModContent.GetInstance<TerramonMod>().BuffType(nameof(PokemonBuff));
            if (pokeName.Value != PokemonName)
            {
                pokeName = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(PokemonName));
            }

            if (!player.HasBuff(pokeBuff))
            {
                player.AddBuff(pokeBuff, 2);
                if (isShiny)
                {
                    modPlayer.ActivePetShiny = true;
                } else
                {
                    modPlayer.ActivePetShiny = false;
                }
                modPlayer.ActivePetName = PokemonName;
                modPlayer.ActivatePet(PokemonName, false);
                goText.Args = new object[] { pokeName.Value };
                //CombatText.NewText(player.Hitbox, Color.White, goText.Value, true);
                Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/sendout").WithVolume(0.34f));
            }
            else
            {
                player.ClearBuff(pokeBuff);
                if (pokeName.Value != modPlayer.ActivePetName) { pokeName = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(modPlayer.ActivePetName)); };
                switch (Main.rand.Next(3))
                {
                    case 0:
                        retire1Text.Args = new object[] { pokeName.Value };
                        //CombatText.NewText(player.Hitbox, Color.White, retire1Text.Value, true);
                        break;
                    case 1:
                        retire2Text.Args = new object[] { pokeName.Value };
                        //CombatText.NewText(player.Hitbox, Color.White, retire2Text.Value, true);
                        break;
                    default:
                        retire3Text.Args = new object[] { pokeName.Value };
                        //CombatText.NewText(player.Hitbox, Color.White, retire3Text.Value, true);
                        break;
                }

                modPlayer.ActivePetName = string.Empty;
            }


            return true;
        }

        public override bool CanRightClick()
        {
            //Just make it little clear
            return ModContent.GetInstance<TerramonMod>().PartySlots.partyslot1.Item.IsAir ||
                   ModContent.GetInstance<TerramonMod>().PartySlots.partyslot2.Item.IsAir ||
                   ModContent.GetInstance<TerramonMod>().PartySlots.partyslot3.Item.IsAir ||
                   ModContent.GetInstance<TerramonMod>().PartySlots.partyslot4.Item.IsAir ||
                   ModContent.GetInstance<TerramonMod>().PartySlots.partyslot5.Item.IsAir ||
                   ModContent.GetInstance<TerramonMod>().PartySlots.partyslot6.Item.IsAir;
        }

        public override void RightClick(Player player)
        {
            if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot1.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot1.Item = item.Clone();
                item.TurnToAir();
            }
            else if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot2.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot2.Item = item.Clone();
                item.TurnToAir();
            }
            else if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot3.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot3.Item = item.Clone();
                item.TurnToAir();
            }
            else if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot4.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot4.Item = item.Clone();
                item.TurnToAir();
            }
            else if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot5.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot5.Item = item.Clone();
                item.TurnToAir();
            }
            else if (ModContent.GetInstance<TerramonMod>().PartySlots.partyslot6.Item.IsAir)
            {
                ModContent.GetInstance<TerramonMod>().PartySlots.partyslot6.Item = item.Clone();
                item.TurnToAir();
            }
            else
            {
                Main.NewText("All Party Slots are full", 255, 240, 20);
                return;
            }

            ((TerramonMod) mod).PartySlots.UpdateUI(null);
        }

        public const string POKEBAL_PROPERTY = "PokebalType";
        public const string MOVE1 = "Move1";
        public const string MOVE2 = "Move2";
        public const string MOVE3 = "Move3";
        public const string MOVE4 = "Move4";

        public PokemonData PokeData = new PokemonData();

        public override TagCompound Save()
        {
            if (PokeData != null)
            {
                PokeData.pokeballType = (byte)TerramonMod.PokeballFactory.GetEnum(this);
                return PokeData.GetCompound();

            }
            var tag = new TagCompound
            {
                //[nameof(PokemonNPC)] = PokemonNPC,
                [nameof(PokemonName)] = PokemonName,
                //[nameof(SmallSpritePath)] = SmallSpritePath,
                //
                //[nameof(SmallSpritePath)] = SmallSpritePath, // what do i do here
                //v2

                [nameof(isShiny)] = isShiny,
                [nameof(CapturedPokemon)] = CapturedPokemon,
                [nameof(Level)] = Level,
                [nameof(Exp)] = Exp,
                //Store move name
                [MOVE1] = Moves[0]?.GetType().Name ?? "",
                [MOVE2] = Moves[1]?.GetType().Name ?? "",
                [MOVE3] = Moves[2]?.GetType().Name ?? "",
                [MOVE4] = Moves[3]?.GetType().Name ?? "",
                //[nameof(Moves)] = from it in Moves select it.MoveName,
                //Used to restore items in sidebarUI
                [POKEBAL_PROPERTY] = (byte)TerramonMod.PokeballFactory.GetEnum(this)
            };


            return tag;
        }

        public override void Load(TagCompound tag)
        {
            PokeData = new PokemonData(tag);
            if (PokeData.pokeballType == 0)
            {
                PokeData.pokeballType = (byte)TerramonMod.PokeballFactory.GetEnum(this);
            }
            return;

            ////PokemonNPC = tag.GetInt(nameof(PokemonNPC));
            //PokemonName = tag.GetString(nameof(PokemonName));
            ////SmallSpritePath = tag.GetString(nameof(SmallSpritePath));
            //isShiny = tag.GetBool(nameof(isShiny));
            ////v2
            //CapturedPokemon = tag.ContainsKey(nameof(CapturedPokemon))
            //    ? tag.GetString(nameof(CapturedPokemon))
            //    : PokemonName;
            //Level = tag.ContainsKey(nameof(Level)) ? tag.GetInt(nameof(Level)) : 1;
            //Exp = tag.ContainsKey(nameof(Exp)) ? tag.GetInt(nameof(Exp)) : 0;

            //if (Moves == null)
            //    Moves = new string[4];
            //Moves[0] = tag.ContainsKey(MOVE1) ? tag.GetString(MOVE1) : null;
            //Moves[1] = tag.ContainsKey(MOVE2) ? tag.GetString(MOVE2) : null;
            //Moves[2] = tag.ContainsKey(MOVE3) ? tag.GetString(MOVE3) : null;
            //Moves[3] = tag.ContainsKey(MOVE4) ? tag.GetString(MOVE4) : null;

            ////Update all old pokebals
            //bool retrofit = true;
            //foreach (var it in Moves)
            //    if (!string.IsNullOrEmpty(it))
            //        retrofit = false;
            //if(retrofit)
            //    Moves = TerramonMod.GetPokemon(PokemonName).DefaultMove;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            for (int i = 0; i < tooltips.Count;)
                //if (tooltips[i].text.Contains("damage") || tooltips[i].text.Contains("knockback") ||
                //    tooltips[i].text.Contains("critical strike") || tooltips[i].text.Contains("speed"))
                if(tooltips[i].Name != "ItemName" && tooltips[i].Name != "Tooltip0")
                    tooltips.RemoveAt(i);
                else
                    i++;
        }

        public override void NetSend(BinaryWriter writer)
        {
            //writer.Write(PokemonNPC);
            if (PokemonName != null)
            {
                writer.Write(true);
                writer.Write(PokemonName);
            }
            else
            {
                writer.Write(false);
            }

            if (SmallSpritePath != null)
            {
                writer.Write(true);
                writer.Write(SmallSpritePath);
            }
            else
            {
                writer.Write(false);
            }

            //v2 
            if (CapturedPokemon != null)
            {
                writer.Write(true);
                writer.Write(CapturedPokemon);
            }
            else
            {
                writer.Write(false);
            }

            writer.Write(Level);
            writer.Write(Exp);
            writer.Write(isShiny);

            var mov = "";
            foreach (var it in Moves) mov += it + "|";
            writer.Write(mov);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            //PokemonNPC = reader.ReadInt32();
            if (reader.ReadBoolean())
                PokemonName = reader.ReadString();
            if (reader.ReadBoolean())
                SmallSpritePath = reader.ReadString();
            if (reader.ReadBoolean()) CapturedPokemon = reader.ReadString();

            Level = reader.ReadInt32();
            Exp = reader.ReadInt32();
            isShiny = reader.ReadBoolean();

            var movArr = reader.ReadString().Split('|');
            for (int i = 0; i < movArr.Length || i < 4; i++)
                if (!string.IsNullOrEmpty(movArr[i]))
                    Moves[i] = TerramonMod.GetMove(movArr[i]);
        }

        //TODO: Take rid with it

        #region Temp Server Detour

        internal static void writeDetour(string type, string name, string icon, int lvl = 1, string moves = "", bool shiny = false)
        {
            det_CapturedPokemon = type;
            det_PokemonName = name;
            det_SmallSpritePath = icon;
            det_Lvl = lvl;
            det_Moves = moves;
            det_isShiny = shiny;
        }

        internal static string det_CapturedPokemon;
        //internal static int det_PokemonNPC;
        internal static string det_PokemonName;
        internal static string det_SmallSpritePath;
        internal static int det_Lvl;
        internal static string det_Moves;
        internal static bool det_isShiny;
        internal static PokemonData det_Data;

        #endregion
    }
}