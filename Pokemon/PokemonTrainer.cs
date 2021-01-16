using Terramon.Players;
using Terramon.UI.SidebarParty;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Terramon.Pokemon
{
    [AutoloadHead]
    public class PokemonTrainer : ModNPC
    {
        public override string Texture => "Terramon/Pokemon/PokemonTrainer";

        public override bool Autoload(ref string name)
        {
            name = "Pokémon Trainer";
            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pokémon Trainer");
            Main.npcFrameCount[npc.type] = 52;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            NPCID.Sets.HatOffsetY[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 26;
            npc.height = 44;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            animationType = NPCID.Guide;
        }

        public override string TownNPCName()
        {
            switch (WorldGen.genRand.Next(2))
            {
                case 0:
                    return "Red";
                default:
                    return "Red";
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return true;
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            TerramonPlayer player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            player.premierBallRewardCounter = 0;
            shop.item[nextSlot].SetDefaults(mod.ItemType("PokeballItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("GreatBallItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("UltraBallItem"));
            nextSlot++;
            if (!Main.dayTime)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("DuskBallItem"));
                nextSlot++;
            }

            var modExpanse = ModLoader.GetMod("tmonadds");
            if (modExpanse != null)
            {
                shop.item[nextSlot].SetDefaults(modExpanse.ItemType("GameBoyGray"));
                nextSlot++;
                if (NPC.downedBoss1)
                {
                    shop.item[nextSlot].SetDefaults(modExpanse.ItemType("GameBoyRed"));
                    nextSlot++;
                }
                int merchant = NPC.FindFirstNPC(NPCID.Merchant);
                if (merchant >= 0)
                {
                    shop.item[nextSlot].SetDefaults(modExpanse.ItemType("GameBoyBlue"));
                    nextSlot++;
                }
                if (NPC.downedGoblins)
                {
                    shop.item[nextSlot].SetDefaults(modExpanse.ItemType("GameBoyYellow"));
                    nextSlot++;
                }
                if (NPC.downedSlimeKing)
                {
                    shop.item[nextSlot].SetDefaults(modExpanse.ItemType("GameBoyGreen"));
                    nextSlot++;
                }
                int nurse = NPC.FindFirstNPC(NPCID.Nurse);
                if (nurse >= 0)
                {
                    shop.item[nextSlot].SetDefaults(modExpanse.ItemType("GameBoyPink"));
                    nextSlot++;
                }
                if (Main.bloodMoon || NPC.downedHalloweenKing)
                {
                    shop.item[nextSlot].SetDefaults(modExpanse.ItemType("GameBoyDark"));
                    nextSlot++;
                }
            }
        }

        public override string GetChat()
        {
            switch (Main.rand.Next(20))
            {
                case 0:
                    return "There's a lot of Pokémon out in the world, but you'll need Poké Balls to catch 'em!";
                case 1:
                    return "I just got back from my Alola vacation. See my tan lines?";
                case 2:
                    return "Pokémon are all sorted into cateories called 'Types'. These types all have unique strengths and weaknesses to each other.";
                case 3:
                    return "Evolution is amazing. I can help you discover your Pokémon's cool evolutions, but it'll cost some Rare Candies.";
                case 4:
                    return "In Johto there are these Pokémon called Furrets. They sure do love to walk";
                case 5:
                    return "Gotta Catch Em' All!";
                case 6:
                    return "As your journey progresses, I'll offer new things. Check back here every so often.";
                case 7:
                    return "Rare Candies can level up your Pokémon. They're quite challenging to find, so keep an eye out.";
                case 8:
                    return "Buy 10 Poké Balls at once, and you might get a surprise...";
                case 9:
                    return "Different Pokémon like living in different places. If you travel around, you may find new Pokemon!";
                case 10:
                    return "Recently I saw a trainer in a green robe. She looked to be in a hurry.";
                case 11:
                    return "Earlier I spotted a trainer wearing green whilst I was looking after Ivysaur. It looked like she was carrying some sort of egg.";
                case 12:
                    return "There are many different regions in the world. One day I hope to visit all of them!";
                case 13:
                    return "Have you played Mobile Creatures on the Game Boy? It has excellent music! Some can be played quite loud...";
                case 14:
                    return "I can give you some types of Poké Balls, but some require rare materials such as Nanites to craft.";
                case 15:
                    return "That Pokédex of yours doesn't seem to be working. I'll have to look into buying you an upgrade...";
                case 16:
                    return "I like to decorate my room with mini Pokéballs, crafted using the same ingredients as full-sized ones.";
                case 17:
                    return "I conveniently sell Pokéballs for you to buy, but if you're short of cash you can always make your own using apricorns and iron!";
                case 18:
                    return "Sometimes when I shake a tree, apricorns fall out! They taste especially good when juiced.";
                case 19:
                    return "Flying Pokémon can be hard to catch. If only I had a stronger Pokéball to catch them with...";
                default:
                    return "missingno.";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = "Evolution";
            //button2 = "Evolve";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
            else
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                EvolveUI.Visible = true;
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 8;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = mod.ProjectileType("RedBall");
            attackDelay = 2;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection,
            ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemType<Items.Pokeballs.Inventory.JokeBall>());
        }
    }
}