using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Network.Catching;
using Terramon.Pokemon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Terramon.Items.Eggs
{
    public class BaseEggsClass : ModItem
    {
        public override string Texture => mod.Name + "/Items/Eggs/Tier1/NormalEgg";
        public virtual int WaitTime { get; } = 0;       //this should be in seconds
        public virtual int PokemonToDropType { get; private set; }
        public virtual string PokemonName { get; private set; }

        private int currentWaitTime;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("It's an egg!");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 32;
            item.rare = ItemRarityID.Green;
            item.value = Item.buyPrice(silver: 50);
        }

        private string GetSmallSpritePath(string pokemonName)
        {
            return "Terramon/Minisprites/Regular/mini" + pokemonName;
        }

        public override void UpdateInventory(Player player)
        {
            currentWaitTime++;
            if (currentWaitTime < WaitTime * 60) return;

            if (PokemonName == null || PokemonName == string.Empty)
            {
                item.TurnToAir();//This is invalid item
                return;
            }

            ParentPokemon hatchedPokemon = TerramonMod.GetPokemon(PokemonName);
            string pokemonName = hatchedPokemon?.GetType().Name;

            if (player.whoAmI != Main.myPlayer)
                return;

            switch (Main.netMode)
            {
                case NetmodeID.MultiplayerClient:
                    {
                        var packet = new BaseCatchPacket();
                        packet.Send((TerramonMod)mod, pokemonName, pokemonName, player.getRect(),
                            ItemType<PokeballCaught>());
                        break;
                    }

                case NetmodeID.Server:
                    {
                        BaseCaughtClass.writeDetour(pokemonName, pokemonName,
                            GetSmallSpritePath(pokemonName), 1, "", false); //This force server to write data when SetDefault() was called
                        Item.NewItem(player.position, ItemType<PokeballCaught>()); // Within this call SetDefault was called
                        break;
                    }

                default:
                    {
                        int pokeballIndex = Item.NewItem(player.position, ItemType<PokeballCaught>());
                        BaseCaughtClass pokeballItem = Main.item[pokeballIndex].modItem as BaseCaughtClass;
                        pokeballItem.PokemonName = pokeballItem.CapturedPokemon = pokemonName;
                        pokeballItem.SmallSpritePath = GetSmallSpritePath(pokemonName);
                        break;
                    }
            }

            Main.NewText(player.name + " the " + pokemonName + " Egg has hatched!", Color.Orange);
            player.ConsumeItem(item.type);
            currentWaitTime = 0;

        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int newWaitTime = (WaitTime * 60) - currentWaitTime;        //in frames
            TooltipLine tooltipAddition = new TooltipLine(mod, "Speed", "Remaining Incubation Time: "
                + newWaitTime / 3600        //conversion to minutes
                + " minutes & "
                + newWaitTime / 60 % 60      //conversion to seconds
                + " seconds");
            tooltips.Add(tooltipAddition);
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "currentWaitTime", currentWaitTime }
            };
        }

        public override void Load(TagCompound tag)
        {
            currentWaitTime = tag.GetInt("currentWaitTime");
        }

        public override ModItem Clone()
        {
            var it = ((BaseEggsClass)this.MemberwiseClone());
            it.PokemonName = PokemonName;
            it.PokemonToDropType = PokemonToDropType;
            it.currentWaitTime = currentWaitTime;
            return it;
        }

        public override ModItem Clone(Item item)
        {
            return Clone();
        }
    }
}