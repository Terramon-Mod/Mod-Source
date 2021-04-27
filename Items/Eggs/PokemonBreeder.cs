using Terramon.Players;
using Terramon.UI.SidebarParty;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Terramon.Items.Eggs
{
    [AutoloadHead]
    public class PokemonBreeder : ModNPC
    {
        public override string Texture => "Terramon/Items/Eggs/PokemonBreeder";

        public override bool Autoload(ref string name)
        {
            name = "Pokémon Breeder";
            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pokémon Breeder");
            Main.npcFrameCount[npc.type] = 26;
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
            shop.item[nextSlot].SetDefaults(mod.ItemType("UltraBallItem"));
            nextSlot++;
        }

        public override string GetChat()
        {
            switch (Main.rand.Next(8))
            {
                case 0:
                    return "I'd love to visit Kalos some day. The Pokémon there are so cute!";
                case 1:
                    return "Two Pokémon are able to breed and produce an egg, but only if they're compatible with each other!";
                case 2:
                    return "These are not shorts! These are half-pants!";
                case 3:
                    return "Aren't baby Pokémon so cute?";
                case 4:
                    return "I love to watch my Pokémon grow, and learn the power that's inside of them!";
                case 5:
                    return "Did you know that Pokémon have preferred biomes? Rock-type Pokémon like to live in deserts.";
		case 6:
                    return "Pokémon that hatch from eggs are able to inherit moves, stats and more from their parents!";
                default:
                    return "missingno.";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            shop = true;
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
            projType = mod.ProjectileType("GreenBall");
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