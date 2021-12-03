using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Items.Pokeballs.Thrown;
using Terramon.Network.Catching;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Razorwing.Framework.Localisation;
using Terramon.Tiles.ShelfBlocks;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terramon.Network.Sync.Battle;
using Terraria.UI.Chat;

namespace Terramon.Pokemon
{
    public abstract class ParentPokemonNPC : ModNPC
    {
        public override string Texture => "Terramon/Pokemon/Empty";

        public ILocalisedBindableString pokeName = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("*")));

        private readonly string[] ballProjectiles = TerramonMod.GetBallProjectiles();
        private readonly float[][] catchChances = TerramonMod.GetCatchChances();
        private readonly string nameMatcher = "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))";
        public static Dictionary<string, Texture2D> HighlightTexture;


        public int frame;
        public int frameCounter;
        public abstract Type HomeClass();

        private int ballUsage;

        protected bool shiny = false;
        protected int timer;

        // distance from player
        private float d;

        public static bool PlayerIsInForest(Player player)
        {
            return !player.ZoneJungle
                   && !player.ZoneDungeon
                   && !player.ZoneCorrupt
                   && !player.ZoneCrimson
                   && !player.ZoneHoly
                   && !player.ZoneSnow
                   && !player.ZoneUndergroundDesert
                   && !player.ZoneGlowshroom
                   && !player.ZoneMeteor
                   && !player.ZoneBeach
                   && !player.ZoneDesert
                   && player.ZoneOverworldHeight;
        }


        public static bool PlayerIsInEvils(Player player)
        {
            return player.ZoneCrimson
                   || player.ZoneCorrupt;
        }

        public string PokeName()
        {
            return Regex.Replace(HomeClass().Name, nameMatcher, "$1 ");
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            string n = Regex.Replace(HomeClass().Name, nameMatcher, "$1 ");
            var arr = GetType().Namespace.Split('.');
            string path = String.Empty;
            for (int i = 1; i < arr.Length && i < 4; i++)// We skip "Terramon" at 0 pos
            {
                path += path.Length > 0 ? $"/{arr[i]}" : arr[i];
            }
            path += $"/{n}/{n}";
            if (shiny)
            {
                path += "_Shiny";
            }

            SpriteEffects effects = npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D pkmnTexture = mod.GetTexture(path);
            int frameHeight = pkmnTexture.Height / Main.npcFrameCount[npc.type];

            if (highlighted && HighlightTexture != null)
            {
                if (!HighlightTexture.ContainsKey(n))//If we don't have one color texture
                {
                    var data = new Color[pkmnTexture.Width * pkmnTexture.Height];//New array with size equal texture total pixels
                    pkmnTexture.GetData(data);//Get all pixel color values
                    for (int i = 0; i < data.Length; i++)//For each pixel
                    {
                        if (data[i].A > 0)//What actually visible
                        {
                            byte a = data[i].A;
                            data[i] = Color.Gold;
                            data[i].A = drawColor.A;
                        }
                    }
                    HighlightTexture.Add(n, new Texture2D(Main.graphics.GraphicsDevice, pkmnTexture.Width, pkmnTexture.Height));// Store modified texture
                    HighlightTexture[n].SetData(data);//And load pixel data
                }

                foreach (Vector2 of in ChatManager.ShadowDirections)//For each shadow direction
                {
                    var offset = of;
                    offset *= 2;
                    if (Main.LocalPlayer.GetModPlayer<TerramonPlayer>().Battle == null && d < 60000)
                    {
                        if (!justHighlighted)
                        {
                            justHighlighted = true;
                            Main.PlaySound(SoundID.MenuTick, Main.LocalPlayer.position, 0);
                        }
                        spriteBatch.Draw(HighlightTexture[n], npc.position - Main.screenPosition + new Vector2(0, -6) + offset,
                        new Rectangle(0, frameHeight * frame, pkmnTexture.Width, frameHeight), Color.White, npc.rotation,
                        new Vector2(pkmnTexture.Width / 2f, frameHeight / 2), npc.scale * 2, effects, -0f);
                    }
                }
            }
            if (highlighted && Main.LocalPlayer.GetModPlayer<TerramonPlayer>().Battle == null && d < 60000) drawColor = Color.White;
            spriteBatch.Draw(pkmnTexture, npc.position - Main.screenPosition + new Vector2(0, -6),
                new Rectangle(0, frameHeight * frame, pkmnTexture.Width, frameHeight), drawColor, npc.rotation,
                new Vector2(pkmnTexture.Width / 2f, frameHeight / 2), npc.scale * 2, effects, 0f);
            if (highlighted)
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, "Lv. 5", npc.position - Main.screenPosition + new Vector2(-32, -28), Color.White, 0f, Vector2.Zero, new Vector2(0.8f));
            }
            return false;
        }

        private bool highlighted = false;
        private bool justHighlighted = false;
        public override void AI()
        {
            npc.scale = 1f;
            //Fix shifted hitbox
            var rect = new Rectangle(npc.Hitbox.X, npc.Hitbox.Y, npc.Hitbox.Width, npc.Hitbox.Height);
            rect.X -= rect.Width;
            rect.Y -= rect.Height;
            rect.Width *= 2;
            rect.Height *= 2;
            //Set flag if hovered
            highlighted = rect.Contains(Main.MouseWorld.ToPoint());

            if (!highlighted) justHighlighted = false;

            // distance
            d = (Main.LocalPlayer.position.X - npc.position.X) * (Main.LocalPlayer.position.X - npc.position.X) + (Main.LocalPlayer.position.Y - npc.position.Y) * (Main.LocalPlayer.position.Y - npc.position.Y);

            //Detect right click on pokemon
            if (highlighted && Main.mouseRight)
            {
                if (Main.mouseRightRelease)
                {
                    var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                    if (d < 60000 && player.ActivePet == null)
                    {
                        Main.NewText("Please send out a Pokémon before starting a battle!", new Color(200, 50, 70));
                        return;
                    };
                    if (d > 60000)
                    {
                        //Main.NewText("You are not close enough to this Pokémon.", Color.LightGray);
                        return;
                    }
                    if (player.Battle != null) return;
                    if (player.ActivePet.Fainted || player.ActivePet.HP == 0)
                    {
                        Main.NewText($"Your {player.ActivePet.Pokemon} is fainted and can't fight!");
                        return;
                    }

                    var data = new PokemonData() //this line (tmonadds)
                    {
                        Pokemon = HomeClass().Name,
                    };
                    player.Battle = new BattleMode(player, BattleState.BattleWithWild, npc: this, second: data);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        var p = new StartBattlePacket();
                        p.Send(TerramonMod.Instance, BattleState.BattleWithWild, data, player.Battle.wildID);
                    }
                }
            }

            //Animations

            npc.spriteDirection = npc.velocity.X > 0 ? -1 : (npc.velocity.X < 0 ? 1 : npc.spriteDirection);

            if (npc.velocity.X > 0f || npc.velocity.Y > 0f)
            {
                frameCounter++;
                if (frameCounter > 30)
                {
                    frame += 1;
                    frameCounter = 0;
                    if (frame >= Main.npcFrameCount[npc.type])
                    {
                        frame = 0;
                    }
                }
            }
            else
            {
                frame = 1;
                frameCounter = 0;
            }
        }


        public override void SetStaticDefaults()
        {
            string n = Regex.Replace(HomeClass().Name, nameMatcher, "$1 ");
            if (pokeName.Value != n)
            {
                pokeName = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(n));
                n = pokeName.Value;
            }
            DisplayName.SetDefault(n);
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            var shinynum = Main.rand.Next(4096); // Generates number between 0 and 4095, essentially a 0.02% chance
            if (shinynum == 0)
            {
                shiny = true;
            }

            npc.defense = 0;
            npc.lifeMax = 15;
            npc.lifeRegen = 15;
            npc.knockBackResist = 0.5f;
            npc.friendly = true;
            npc.damage = 0;
            npc.value = 0f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = mod.GetLegacySoundSlot(SoundType.NPCHit, "Sounds/NPCHit/capturepokemon");

            npc.aiStyle = 7;
            aiType = NPCID.Bunny;
        }

        private string GetSmallSpritePath(NPC npc)
        {
            if (shiny)
            {
                return "Terramon/Minisprites/Regular/mini" + npc.TypeName + "_Shiny";
            }
            else
            {
                return "Terramon/Minisprites/Regular/mini" + npc.TypeName;
            }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        private bool? CanBeHitByPlayer(Player player)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            foreach (string ballProjectile in ballProjectiles)
                if (projectile.type == mod.ProjectileType(ballProjectile) && projectile.ai[1] == 1)
                    return true;

            return false;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.ZoneOverworldHeight)
                return 0.1f;
            return 0f;
        }

        public override void UpdateLifeRegen(ref int damage)
        {
            npc.life = npc.lifeMax;
        }

        // this method will be improved later
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback,
            ref bool crit, ref int hitDirection)
        {
            knockback = 0;
            crit = false;

            for (int i = 0; i < ballProjectiles.Length; i++)
                if (projectile.type == mod.ProjectileType(ballProjectiles[i]) && projectile.ai[1] == 1)
                {
                    if (ballProjectiles[i] == "MasterBallProjectile") // Master Ball never fails
                    {
                        Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<MasterBallCaught>());
                        return;
                    }
                    else if (ballProjectiles[i] == "ZeroBallProjectile") // Master Ball never fails
                    {
                        if (Main.rand.NextFloat() < .1190f)
                        {
                            Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<ZeroBallCaught>());
                            return;
                        }
                    }
                    else if (ballProjectiles[i] == "DuskBallProjectile") // Special Condition day/night
                    {
                        ballUsage++;
                        if (!Main.dayTime && Main.rand.NextFloat() < catchChances[i][0] ||
                            Main.dayTime && Main.rand.NextFloat() < catchChances[i][1])
                        {
                            Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<DuskBallCaught>());
                            return;
                        }
                    }
                    else if (ballProjectiles[i] == "QuickBallProjectile") // Special Condition
                    {
                        ballUsage++;
                        if (ballUsage == 1) // 2x catch chance
                            if (Main.rand.NextFloat() < .2380f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<QuickBallCaught>());
                                return;
                            }

                        if (ballUsage == 2) // 1.8x catch chance
                            if (Main.rand.NextFloat() < .2142f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<QuickBallCaught>());
                                return;
                            }

                        if (ballUsage == 3) // 1.6x catch chance
                            if (Main.rand.NextFloat() < .1904f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<QuickBallCaught>());
                                return;
                            }

                        if (ballUsage == 4) // 1.4x catch chance
                            if (Main.rand.NextFloat() < .1666f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<QuickBallCaught>());
                                return;
                            }

                        if (ballUsage == 5) // 1.2x catch chance
                            if (Main.rand.NextFloat() < .1428f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<QuickBallCaught>());
                                return;
                            }

                        if (ballUsage == 6) // 1x catch chance
                            if (Main.rand.NextFloat() < .1190f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<QuickBallCaught>());
                                return;
                            }

                        if (ballUsage == 7) // 0.75x catch chance
                            if (Main.rand.NextFloat() < .08925f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<QuickBallCaught>());
                                return;
                            }

                        if (ballUsage >= 8) // anything more.. 0.5x
                            if (Main.rand.NextFloat() < .0595f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<QuickBallCaught>());
                                return;
                            }
                    }
                    else if (ballProjectiles[i] == "TimerBallProjectile") // Special Condition
                    {
                        ballUsage++;
                        if (ballUsage == 1) // 0.5x catch chance
                            if (Main.rand.NextFloat() < .0595f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 2) // 0.75x catch chance
                            if (Main.rand.NextFloat() < .08925f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 3) // 1x catch chance
                            if (Main.rand.NextFloat() < .1190f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 4) // 1.2x catch chance
                            if (Main.rand.NextFloat() < .1428f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 5) // 1.4x catch chance
                            if (Main.rand.NextFloat() < .1666f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 6) // 1.6x catch chance
                            if (Main.rand.NextFloat() < .1904f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 7) // 1.8x catch chance
                            if (Main.rand.NextFloat() < .2142f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 8) // 2x catch chance
                            if (Main.rand.NextFloat() < .2380f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 9) // 2.2x catch chance
                            if (Main.rand.NextFloat() < .2618f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 10) // 2.4x catch chance
                            if (Main.rand.NextFloat() < .2856f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 11) // 2.6x catch chance
                            if (Main.rand.NextFloat() < .3094f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage == 12) // 2.8x catch chance
                            if (Main.rand.NextFloat() < .3332f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }

                        if (ballUsage >= 13) // anything more.. 3x
                            if (Main.rand.NextFloat() < .3570f)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<TimerBallCaught>());
                                return;
                            }
                    }
                    else if (ballProjectiles[i] == "PumpkinBallProjectile")
                    {
                        string name = GetType().ToString().Substring(0, GetType().ToString().Length - 3);
                        ModProjectile pokemonProj = mod.GetProjectile(name);
                        ParentPokemon pkmn = pokemonProj as ParentPokemon;
                        if (!Main.dayTime && Main.halloween)
                        {
                            if (Main.rand.Next(0, 100) < 45)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<PumpkinBallCaught>());
                                return;
                            }
                        }
                        else if (Main.halloween)
                        {
                            if (Main.rand.Next(0, 100) < 30)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<PumpkinBallCaught>());
                                return;
                            }
                        }
                        else
                        {
                            if (Main.rand.Next(0, 100) < 15)
                            {
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<PumpkinBallCaught>());
                                return;
                            }
                        }
                    }

                    ballUsage++;
                    for (int j = 0; j < catchChances[i].Length; j++) // Retain loop for improvement later
                        if (Main.rand.NextFloat() < catchChances[i][j])
                        {
                            if (projectile.type == ModContent.ProjectileType<PokeballProjectile>()
                            ) // Special Condition
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<PokeballCaught>());
                            if (projectile.type == ModContent.ProjectileType<GreatBallProjectile>()
                            ) // Special Condition
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<GreatBallCaught>());
                            if (projectile.type == ModContent.ProjectileType<UltraBallProjectile>()
                            ) // Special Condition
                                Catch(ref projectile, ref crit, ref damage, ModContent.ItemType<UltraBallCaught>());
                            if (projectile.type == ModContent.ProjectileType<PremierBallProjectile>()
                            ) // Special Condition
                                Catch(ref projectile, ref crit, ref damage,
                                    ModContent.ItemType<PremierBallCaught>());
                            return;
                        }
                    break;
                }

            CombatText.NewText(npc.Hitbox, Color.White, "Miss...", true);

            if (projectile.type == ModContent.ProjectileType<PokeballProjectile>()) // Special Condition
                if (Main.rand.Next(3) == 0)
                    Item.NewItem(npc.getRect(), ModContent.ItemType<PokeballItem>());
            if (projectile.type == ModContent.ProjectileType<GreatBallProjectile>()) // Special Condition
                if (Main.rand.Next(3) == 0)
                    Item.NewItem(npc.getRect(), ModContent.ItemType<GreatBallItem>());
            if (projectile.type == ModContent.ProjectileType<UltraBallProjectile>()) // Special Condition
                if (Main.rand.Next(3) == 0)
                    Item.NewItem(npc.getRect(), ModContent.ItemType<UltraBallItem>());
            if (projectile.type == ModContent.ProjectileType<DuskBallProjectile>()) // Special Condition
                if (Main.rand.Next(3) == 0)
                    Item.NewItem(npc.getRect(), ModContent.ItemType<DuskBallItem>());
            if (projectile.type == ModContent.ProjectileType<PremierBallProjectile>()) // Special Condition
                if (Main.rand.Next(3) == 0)
                    Item.NewItem(npc.getRect(), ModContent.ItemType<PremierBallItem>());
            if (projectile.type == ModContent.ProjectileType<QuickBallProjectile>()) // Special Condition
                if (Main.rand.Next(3) == 0)
                    Item.NewItem(npc.getRect(), ModContent.ItemType<QuickBallItem>());
            if (projectile.type == ModContent.ProjectileType<TimerBallProjectile>()) // Special Condition
                if (Main.rand.Next(3) == 0)
                    Item.NewItem(npc.getRect(), ModContent.ItemType<TimerBallItem>());
            if (projectile.type == ModContent.ProjectileType<ZeroBallProjectile>()) // Special Condition
                if (Main.rand.Next(3) == 0)
                    Item.NewItem(npc.getRect(), ModContent.ItemType<ZeroBallItem>());
            if (projectile.type == ModContent.ProjectileType<PumpkinBallProjectile>())
                if (Main.rand.Next(3) == 0)
                    Item.NewItem(npc.getRect(), ModContent.ItemType<PumpkinBallItem>());

            damage = 0;
            npc.life = npc.lifeMax + 1;
            projectile.ai[1] = 0;
        }

        private void Catch(ref Projectile proj, ref bool crit, ref int dmg, int type)
        {
            var PokeNam = Regex.Replace(HomeClass().Name, nameMatcher, "$1 ");
            if (pokeName.Value != PokeNam)
            {
                pokeName = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(PokeNam));
            }
            proj.ai[1] = 2;
            crit = false;
            dmg = npc.lifeMax;
            CreateDust(4);
            CombatText.NewText(npc.Hitbox, Color.Orange, $"{pokeName.Value} was caught!", true);

            TerramonPlayer modPlayer = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            modPlayer.PartySlot2 = new PokemonData()
            {
                Pokemon = HomeClass().Name,
                pokeballType = 1
            };
            modPlayer.secondslotname = modPlayer.PartySlot2.PokemonName;

            modPlayer.RegisterToPokedex(HomeClass().Name);

            return;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                var packet = new BaseCatchPacket();
                packet.Send((TerramonMod)mod, HomeClass().Name, npc.TypeName, npc.getRect(), type, shiny);
            }
            else
            {
                if (Main.netMode == NetmodeID.Server)
                    BaseCaughtClass.writeDetour(HomeClass().Name, npc.TypeName, GetSmallSpritePath(npc), 1, "", shiny);

                int index = Item.NewItem(npc.getRect(), type);
                if (index >= 400)
                    return;

                if (Main.netMode == NetmodeID.Server)
                    return;

                if (!(Main.item[index].modItem is BaseCaughtClass item)) return;
                item.PokeData = new PokemonData()
                {
                    Pokemon = HomeClass().Name,
                };
                if (shiny)
                {
                    item.isShiny = true;
                }
                else
                {
                    item.isShiny = false;
                }

                item.CapturedPokemon = HomeClass().Name;
            }
        }

        private void CreateDust(int counter)
        {
            for (int j = 0; j < counter; j++)
                Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 220, npc.velocity.X * -0.5f,
                    npc.velocity.Y * -0.5f);
        }
    }
}