using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using On.Terraria.Achievements;
using Terramon.Players;
using Terramon.Pokemon.FirstGeneration.Normal.Bulbasaur;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Terraria.UI.Chat;
using Razorwing.Framework.Localisation;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable PossibleLossOfFraction

namespace Terramon.Pokemon
{
    public abstract class ParentPokemon : ModProjectile
    {
        public override string Texture => "Terramon/Pokemon/Empty";
        public static Dictionary<string, Texture2D> HighlightTexture;

        /// <summary>
        ///     Next stage pokemon to evolve.
        ///     If value == null => mon can't evolve
        /// </summary>
        public virtual Type EvolveTo { get; } = null;

        /// <summary>
        ///     How much candies need to evolve.
        /// </summary>
        public virtual int EvolveCost { get; } = 0;

        /// <summary>
        ///     Item what used for evolution
        /// </summary>
        public virtual EvolveItem EvolveItem => EvolveItem.RareCandy;

        /// <summary>
        ///     Just for checking if this mon can evolve or not
        /// </summary>
        public bool CanEvolve => EvolveTo != null && EvolveCost != 0;

        public virtual PokemonType[] PokemonTypes => new[] { PokemonType.Normal };

        public virtual ExpGroup ExpGroup => ExpGroup.MediumFast;

#if DEBUG
        public virtual string[] DefaultMove => new[] { nameof(ShootMove), nameof(HealMove), "", "" };
#else
        public virtual string[] DefaultMove => new[] {"", "", "", ""};
#endif

        public virtual int MaxHP => 45;
        public virtual int PhysicalDamage => 50;
        public virtual int PhysicalDefence => 50;
        public virtual int SpecialDamage => 50;
        public virtual int SpecialDefence => 50;
        public virtual int Speed => 45;

        public int TotalPoints => MaxHP + PhysicalDamage + PhysicalDefence + SpecialDamage + SpecialDefence + Speed;

        internal bool Wild;

        private string iconName;

        public int SpawnTime = 32;
        public virtual string IconName => iconName ?? (iconName = $"Terramon/Minisprites/Regular/mini{GetType().Name}");
	    private readonly string nameMatcher = "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))";

        public int AttackDuration;

        public bool shiny = false;

        public bool useAi = true;

        public int frame;
        public int frameCounter;

        private bool playedDropSfx = false;

        public bool drawMain = false; // Whether or not to draw the Pokemon texture at all, starts as false
        public float whiteFlashVal = 1f;
        public float dootscale = 0.1f;
        public bool DontTpOnCollide = false;

        public int throwBallAnimProjectile;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 28;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft *= 5;
            projectile.alpha = 255; // Start as transparent
            projectile.owner = Main.myPlayer;
            drawOffsetX = 0;
            if (Main.dedServ)
            {
                Wild = det_Wild;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var arr = GetType().Namespace.Split('.');
            string path = String.Empty;
            for (int i = 1; i < arr.Length && i < 4; i++) // We skip "Terramon" at 0 pos
            {
                path += path.Length > 0 ? $"/{arr[i]}" : arr[i];
            }

	        string n = Regex.Replace(projectile.Name, nameMatcher, "$1 ");

            if (n == "Nidoran ♂") n = "Nidoranm";
            if (n == "Nidoran ♀") n = "Nidoranf";
            if (n == "Mr. Mime") n = "Mrmime";

            path += $"/{n}/{n}";
            if (shiny)
            {
                path += "_Shiny";
            }

            float scale = dootscale;

            if (damageReceived)
            {
                if (flashFrame)
                {
                    scale = 0f;
                }
            }

            // Shaders

            if (Main.netMode != NetmodeID.Server)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                if (whiteFlashVal == 1f)
                {
                    GameShaders.Misc["WhiteTint"].UseOpacity(whiteFlashVal);
                    GameShaders.Misc["WhiteTint"].Apply();
                }
            }

            SpriteEffects effects =
                projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D pkmnTexture = mod.GetTexture(path);

            int frameHeight = pkmnTexture.Height / Main.projFrames[projectile.type];

            if (acidArmor) drawColor = Color.White.MultiplyRGB(new Color(181, 80, 136));

            if (highlighted && HighlightTexture != null)
            {
                if (!HighlightTexture.ContainsKey(n))
                {
                    var data = new Color[pkmnTexture.Width * pkmnTexture.Height];
                    pkmnTexture.GetData(data);
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i].A > 0)
                        {
                            byte a = data[i].A;
                            data[i] = Color.Gold;
                            data[i].A = a;
                        }
                    }
                    HighlightTexture.Add(n, new Texture2D(Main.graphics.GraphicsDevice, pkmnTexture.Width, pkmnTexture.Height));
                    HighlightTexture[n].SetData(data);
                }

                foreach (Vector2 of in ChatManager.ShadowDirections)
                {
                    var offset = of;
                    offset *= 2;
                    if (HoldingUsableItem())
                    {
                        if (!justHighlighted)
                        {
                            justHighlighted = true;
                            Main.PlaySound(SoundID.MenuTick, Main.LocalPlayer.position, 0);
                        }
                        spriteBatch.Draw(HighlightTexture[n], projectile.position - Main.screenPosition + new Vector2(14, 0) + offset,
                        new Rectangle(0, frameHeight * frame, pkmnTexture.Width, frameHeight), Color.White, projectile.rotation,
                        new Vector2(pkmnTexture.Width / 2f, frameHeight / 2), projectile.scale, effects, -0f);
                    }
                }
            }
            if (highlighted && Main.LocalPlayer.GetModPlayer<TerramonPlayer>().Battle == null && HoldingUsableItem()) drawColor = Color.White;
            if (drawMain) spriteBatch.Draw(pkmnTexture, projectile.position - Main.screenPosition + new Vector2(14, 0),
                new Rectangle(0, frameHeight * frame, pkmnTexture.Width, frameHeight), drawColor, projectile.rotation,
                new Vector2(pkmnTexture.Width / 2f, frameHeight / 2), scale * 2, effects, 0f);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.zephyrfish = false; // Relic from aiType
            if (aiType != 0)
                mainAi = aiType;
            return true;
        }

        private int mainAi = ProjectileID.Puppy;

        // for wild, walking pokemon
        public int hopTimer = -150;
        private bool jumping = false;
        private bool flashFrame;

        private float lockedPosX;

        // Battling properties
        // There are a lot of these. These affect the Pokemon's appearance / behavior in various ways

        /// <summary>
        /// Activates when this Pokemon takes damage from any source. Used to play a flickering animation.
        /// </summary>
        public bool damageReceived = false;
        private int damageReceivedTimer;

        /// <summary>
        /// Activates when this Pokemon has their HP healed by any source. Used to play a restoration animation.
        /// </summary>
        public bool healedHealth = false;
        private int healedHealthTimer;

        /// <summary>
        /// Activates when this Pokemon has one of their stats increased by any source. Plays a power-up animation.
        /// </summary>
        public bool statModifiedUp = false;
        private int statModifiedUpTimer;

        /// <summary>
        /// Activates when this Pokemon has one of their stats decreased by any source. Plays a power-down animation.
        /// </summary>
        public bool statModifiedDown = false;
        private int statModifiedDownTimer;

        /// <summary>
        /// Activates when this Pokemon has their critical ratio stage increased by a move like Focus Energy
        /// </summary>
        public bool gettingPumped = false;
        private int gettingPumpedTimer;

        /// <summary>
        /// Activates when this Pokemon uses the move Acid Armor
        /// </summary>
        public bool acidArmor = false;

        /// <summary>
        /// Activates when this Pokemon uses the move Focus Energy
        /// </summary>
        public bool focusEnergy = true;

        // End battling properties

        private bool highlighted = false;
        private bool justHighlighted = false;

        public bool flying = false;
        public override void AI()
        {
            //Fix shifted hitbox
            var rect = projectile.Hitbox;
            rect.X -= rect.Width;
            rect.Y -= rect.Height;
            rect.Width *= 2;
            rect.Height *= 2;
            //Set flag if hovered
            highlighted = rect.Contains(Main.MouseWorld.ToPoint());

            if (!highlighted) justHighlighted = false;

            if (!Wild && !flying && SpawnTime >= 0 && useAi) PuppyAI();

            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();

            if (modPlayer.ActivePetShiny)
            {
                shiny = true;
            }
            else
            {
                shiny = false;
            }

            //Animations

            if (!Wild)
            {
                if (projectile.velocity.X != 0) projectile.spriteDirection = projectile.velocity.X > 0 ? -1 : (projectile.velocity.X < 0 ? 1 : projectile.spriteDirection);

                if (highlighted && Main.mouseRight)
                {
                    if (Main.mouseRightRelease)
                    {
                        if (HoldingPotion()) UsePotion();
                        if (HoldingSuperPotion()) UseSuperPotion();
                        if (HoldingHyperPotion()) UseHyperPotion();
                        if (HoldingMaxPotion()) UseMaxPotion();
                        if (HoldingFullRestore()) UseFullRestore();
                    }
                }
            }

            if (projectile.velocity.X != 0 || projectile.velocity.Y > 1 && !Wild)
            {
                frameCounter++;
                if (frameCounter > 15)
                {
                    frame += 1;
                    frameCounter = 0;
                    if (frame >= Main.projFrames[projectile.type])
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

            if (Wild)
            {
                dootscale = 1f;
                whiteFlashVal = 0f;
                frameCounter++;
                if (frameCounter > 15)
                {
                    frame += 1;
                    frameCounter = 0;
                    if (frame >= Main.projFrames[projectile.type])
                    {
                        frame = 0;
                    }
                }
            }

            SpawnTime++;
            if (SpawnTime < 33 && !Wild)
            {
                drawMain = false;
            }
            else drawMain = true;

            if (SpawnTime > 33 && !Wild)
            {
                if (dootscale < 1f) dootscale += 0.045f;
                else
                {
                    dootscale = 1f;
                    whiteFlashVal = 0f;
                }
            }

            if (SpawnTime == 33 && player.active)
            {

                string n = Regex.Replace(projectile.Name, nameMatcher, "$1 ");

                if (n == "Nidoran ♂") n = "Nidoranm";
                if (n == "Nidoran ♀") n = "Nidoranf";
                if (n == "Mr. Mime") n = "Mrmime";

                if (!Main.dedServ && !Wild)
                {
                    Main.PlaySound(ModContent.GetInstance<TerramonMod>()
                        .GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/exitball").WithVolume(0.4f));
                    Main.PlaySound(ModContent.GetInstance<TerramonMod>()
                        .GetLegacySoundSlot(SoundType.Custom, "Sounds/Cries/cry" + n).WithVolume(0.55f));
                }

                if (!Wild)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        Dust e = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,
                            87, 0, 0, 0, default(Color), 1.2f);
                        e.noGravity = true;
                        e = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,
                            87, 0, 0, 0, default(Color), 1.2f);
                        e.noGravity = true;
                        e = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,
                            87, 0, 0, 0, default(Color), 1.2f);
                        e.noGravity = true;
                    }
                }
            }

            /// <summary>
            /// Battling animations
            /// </summary>
       
            // Damage received

            if (damageReceived)
            {
                damageReceivedTimer++;
                if (damageReceivedTimer <= 10)
                {
                    flashFrame = true;
                }
                if (damageReceivedTimer >= 11 && damageReceivedTimer <= 20)
                {
                    flashFrame = false;
                }
                if (damageReceivedTimer >= 21 && damageReceivedTimer <= 30)
                {
                    flashFrame = true;
                }
                if (damageReceivedTimer >= 31 && damageReceivedTimer <= 40)
                {
                    flashFrame = false;
                }
                if (damageReceivedTimer >= 41 && damageReceivedTimer <= 50)
                {
                    flashFrame = true;
                }
                if (damageReceivedTimer >= 51 && damageReceivedTimer <= 60)
                {
                    flashFrame = false;
                    damageReceived = false;
                    damageReceivedTimer = 0;
                }
            }

            // Health restoration

            if (healedHealth)
            {
                healedHealthTimer++;
                if (healedHealthTimer < 90)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        Dust dust1 = Dust.NewDustDirect(projectile.position + new Vector2(Main.rand.Next(-7, 7), Main.rand.Next(-7, 7)), projectile.width, projectile.height, 74, 0f, 0f, 0, Color.White, 0.75f);
                        dust1.alpha = 100;
                        dust1.velocity.Y = -3f;
                        dust1.noGravity = true;
                    }
                } else
                {
                    healedHealthTimer = 0;
                    healedHealth = false;
                }
            }

            // Stat modified up

            if (statModifiedUp)
            {
                statModifiedUpTimer++;
                if (statModifiedUpTimer < 90)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        Dust dust1 = Dust.NewDustDirect(projectile.position + new Vector2(Main.rand.Next(-7, 7), Main.rand.Next(-7, 7)), projectile.width, projectile.height, 182, 0f, 0f, 0, Color.White, 0.75f);
                        dust1.alpha = 100;
                        dust1.velocity.Y = -1.7f;
                        dust1.noGravity = true;
                    }
                }
                else
                {
                    statModifiedUpTimer = 0;
                    statModifiedUp = false;
                }
            }

            // Stat modified down

            if (statModifiedDown)
            {
                statModifiedDownTimer++;
                if (statModifiedDownTimer < 90)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        Dust dust1 = Dust.NewDustDirect(projectile.position + new Vector2(Main.rand.Next(-7, 7), Main.rand.Next(-7, 7)), projectile.width, projectile.height, 56, 0f, 0f, 0, Color.White, 0.75f);
                        dust1.alpha = 100;
                        dust1.velocity.Y = 1.25f;
                        dust1.noGravity = true;
                    }
                }
                else
                {
                    statModifiedDownTimer = 0;
                    statModifiedDown = false;
                }
            }

            // Getting pumped (Currently obsolete)

            if (gettingPumped)
            {
                gettingPumped = false;
            }

            /// <summary>
            /// End battling animations
            /// </summary>

            if (hopTimer <= 1)
            {
                lockedPosX = projectile.position.X;
            }

            if (Wild)
            {
                hopTimer++;
                projectile.timeLeft = 5;
                //projectile.tileCollide = false;
                if (hopTimer >= 40 && hopTimer <= 48)
                {
                    projectile.velocity.Y += -0.4f;
                    jumping = true;
                }
                if (hopTimer >= 48)
                {
                    jumping = false;
                }
                if (hopTimer >= 62 && hopTimer <= 72)
                {
                    projectile.velocity.Y += -0.4f;
                    jumping = true;
                }
                if (hopTimer >= 72)
                {
                    jumping = false;
                }
                if (!jumping) projectile.velocity.Y = 1f;
                projectile.position.X = lockedPosX;
                projectile.spriteDirection = projectile.position.X > player.position.X ? 1 : -1;
                return;
            }

            if (!player.active)
            {
                projectile.timeLeft = 0;
            }


            if (player.dead)
            {
                modPlayer.ResetEffects();
                modPlayer.ActivePetId = -1;
            }

            if (modPlayer.IsPetActive(GetType().Name))
            {
                projectile.timeLeft = 2;
            }
            else if ((modPlayer.Battle?.awaitSync ?? false) || modPlayer.Battle?.WildNPC == this)
            {
                projectile.timeLeft = 2;
                Wild = true;
            }

            if (modPlayer.ActiveMove != null)
            {
                if (modPlayer.ActiveMove.OverrideAI(this, modPlayer))
                    aiType = 0;
            }
            else if (modPlayer.Battle?.AIOverride(this) != null) //If used inside battle
            {
                var t = modPlayer.Battle?.AIOverride(this);
                t.TurnAnimation = true;
                if (t.OverrideAI(this, modPlayer))
                {
                    aiType = 0;
                }

                t.TurnAnimation = false;
            }
            else if (aiType == 0)
            {
                aiType = mainAi;
            }

            if (modPlayer.Attacking)
            {
                AttackDuration++;
                if (AttackDuration < 60)
                {
                    if (projectile.type == ModContent.ProjectileType<Bulbasaur>())
                    {
                        Projectile.NewProjectile(projectile.position.X + 23, projectile.position.Y + 8, 0f, 0f,
                            ModContent.ProjectileType<AngerOverlay>(), 0, 0, Main.myPlayer);
                    }

                    AttackDuration = 0;
                }
                else
                {
                    // dont make any more.
                }
            }
            else
            {

            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[projectile.owner];
            if (SpawnTime <= 6 && !DontTpOnCollide)
            {
                projectile.position = player.position;
            }

            if (!playedDropSfx && !Wild && Main.LocalPlayer.GetModPlayer<TerramonPlayer>().Battle != null)
            {
                playedDropSfx = true;
            }

            return true;
        }

        /// <summary>
        /// Unused, was just created to test item usage on pokemon
        /// </summary>
        /// <returns>Always returns false</returns>
        public bool HoldingApricorn()
        {
            return false;
        }

        public bool HoldingUsableItem() {
            if (HoldingPotion() ||
                HoldingSuperPotion() ||
                HoldingHyperPotion() ||
                HoldingMaxPotion() ||
                HoldingFullRestore()) return true;
            return false;
        }

        public bool HoldingPotion()
        {
            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (!highlighted) return false;
            if (modPlayer.Battle != null) return false;
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Items.MiscItems.Medication.Potion>()) return true;
            return false;
        }

        public bool HoldingSuperPotion()
        {
            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (!highlighted) return false;
            if (modPlayer.Battle != null) return false;
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Items.MiscItems.Medication.SuperPotion>()) return true;
            return false;
        }

        public bool HoldingHyperPotion()
        {
            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (!highlighted) return false;
            if (modPlayer.Battle != null) return false;
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Items.MiscItems.Medication.HyperPotion>()) return true;
            return false;
        }

        public bool HoldingMaxPotion()
        {
            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (!highlighted) return false;
            if (modPlayer.Battle != null) return false;
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Items.MiscItems.Medication.MaxPotion>()) return true;
            return false;
        }

        public bool HoldingFullRestore()
        {
            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (!highlighted) return false;
            if (modPlayer.Battle != null) return false;
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Items.MiscItems.Medication.FullRestore>()) return true;
            return false;
        }

        public static bool det_Wild;

        private void PuppyAI() //334
        {
            if (!Main.player[projectile.owner].active)
            {
                projectile.active = false;
                return;
            }

            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            int num;
            if (projectile.lavaWet)
            {
                projectile.ai[0] = 1f;
                projectile.ai[1] = 0f;
            }

            num = 60 + 30 * projectile.minionPos;
            if (Main.player[projectile.owner].dead)
            {
                Main.player[projectile.owner].puppy = false;
            }
            else if (Main.player[projectile.owner].puppy)
            {
                projectile.timeLeft = 2;
            }

            if (Main.player[projectile.owner].position.X + (float) (Main.player[projectile.owner].width / 2) <
                projectile.position.X + (float) (projectile.width / 2) - (float) num)
            {
                flag = true;
            }
            else if (Main.player[projectile.owner].position.X + (float) (Main.player[projectile.owner].width / 2) >
                     projectile.position.X + (float) (projectile.width / 2) + (float) num)
            {
                flag2 = true;
            }


            if (projectile.ai[1] == 0f && !Wild)
            {
                int num36 = 500;
                if (Main.player[projectile.owner].rocketDelay2 > 0)
                {
                    projectile.ai[0] = 1f;
                }

                Vector2 vector6 = new Vector2(projectile.position.X + (float) projectile.width * 0.5f,
                    projectile.position.Y + (float) projectile.height * 0.5f);
                float num37 = Main.player[projectile.owner].position.X +
                              (float) (Main.player[projectile.owner].width / 2) - vector6.X;
                float num38 = Main.player[projectile.owner].position.Y +
                              (float) (Main.player[projectile.owner].height / 2) - vector6.Y;
                float num39 = (float) Math.Sqrt((double) (num37 * num37 + num38 * num38));
                if (num39 > 2000f)
                {
                    projectile.position.X = Main.player[projectile.owner].position.X +
                                            (float) (Main.player[projectile.owner].width / 2) -
                                            (float) (projectile.width / 2);
                    projectile.position.Y = Main.player[projectile.owner].position.Y +
                                            (float) (Main.player[projectile.owner].height / 2) -
                                            (float) (projectile.height / 2);
                }
                else if (num39 > (float) num36 || (Math.Abs(num38) > 300f))
                {
                    if (num38 > 0f && projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }

                    if (num38 < 0f && projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }

                    projectile.ai[0] = 1f;
                }
            }

            if (projectile.ai[0] != 0f)
            {
                float num40 = 0.2f;
                int num41 = 200;
                projectile.tileCollide = false;
                Vector2 vector7 = new Vector2(projectile.position.X + (float) projectile.width * 0.5f,
                    projectile.position.Y + (float) projectile.height * 0.5f);
                float num42 = Main.player[projectile.owner].position.X +
                              (float) (Main.player[projectile.owner].width / 4) - vector7.X;
                float num48 = Main.player[projectile.owner].position.Y +
                              (float) (Main.player[projectile.owner].height / 4) - vector7.Y;
                float num49 = (float) Math.Sqrt((double) (num42 * num42 + num48 * num48));
                float num50 = 10f;
                float num51 = num49;
                if (num49 < (float) num41 && Main.player[projectile.owner].velocity.Y == 0f &&
                    projectile.position.Y + (float) projectile.height <= Main.player[projectile.owner].position.Y +
                    (float) Main.player[projectile.owner].height &&
                    !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.ai[0] = 0f;
                    if (projectile.velocity.Y < -6f)
                    {
                        projectile.velocity.Y = -6f;
                    }
                }

                if (num49 < 60f)
                {
                    num42 = projectile.velocity.X;
                    num48 = projectile.velocity.Y;
                }
                else
                {
                    num49 = num50 / num49;
                    num42 *= num49;
                    num48 *= num49;
                }

                if (projectile.velocity.X < num42)
                {
                    projectile.velocity.X = projectile.velocity.X + num40;
                    if (projectile.velocity.X < 0f)
                    {
                        projectile.velocity.X = projectile.velocity.X + num40 * 1.5f;
                    }
                }

                if (projectile.velocity.X > num42)
                {
                    projectile.velocity.X = projectile.velocity.X - num40;
                    if (projectile.velocity.X > 0f)
                    {
                        projectile.velocity.X = projectile.velocity.X - num40 * 1.5f;
                    }
                }

                if (projectile.velocity.Y < num48)
                {
                    projectile.velocity.Y = projectile.velocity.Y + num40;
                    if (projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y = projectile.velocity.Y + num40 * 1.5f;
                    }
                }

                if (projectile.velocity.Y > num48)
                {
                    projectile.velocity.Y = projectile.velocity.Y - num40;
                    if (projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y = projectile.velocity.Y - num40 * 1.5f;
                    }
                }

                projectile.frameCounter++;
                if (projectile.frameCounter > 1)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }

                if (projectile.frame < 7 || projectile.frame > 10)
                {
                    projectile.frame = 7;
                }

                projectile.rotation = projectile.velocity.X * 0.1f;
            }
            else
            {
                bool flag7 = false;
                Vector2 vector9 = Vector2.Zero;
                bool flag8 = false;
                if (projectile.ai[1] != 0f)
                {
                    flag = false;
                    flag2 = false;
                }

                projectile.rotation = 0f;
                projectile.tileCollide = true;

                float num103 = 0.08f;
                float num104 = 8f;
                if (flag)
                {
                    if ((double) projectile.velocity.X > -3.5)
                    {
                        projectile.velocity.X = projectile.velocity.X - num103;
                    }
                    else
                    {
                        projectile.velocity.X = projectile.velocity.X - num103 * 0.25f;
                    }
                }
                else if (flag2)
                {
                    if ((double) projectile.velocity.X < 3.5)
                    {
                        projectile.velocity.X = projectile.velocity.X + num103;
                    }
                    else
                    {
                        projectile.velocity.X = projectile.velocity.X + num103 * 0.25f;
                    }
                }
                else
                {
                    projectile.velocity.X = projectile.velocity.X * 0.9f;
                    if (projectile.velocity.X >= -num103 && projectile.velocity.X <= num103)
                    {
                        projectile.velocity.X = 0f;
                    }
                }

                if (flag || flag2)
                {
                    int num105 = (int) (projectile.position.X + (float) (projectile.width / 2)) / 16;
                    int j2 = (int) (projectile.position.Y + (float) (projectile.height / 2)) / 16;
                    if (flag)
                    {
                        num105--;
                    }

                    if (flag2)
                    {
                        num105++;
                    }

                    num105 += (int) projectile.velocity.X;
                    if (WorldGen.SolidTile(num105, j2))
                    {
                        flag4 = true;
                    }
                }

                if (Main.player[projectile.owner].position.Y + (float) Main.player[projectile.owner].height - 8f >
                    projectile.position.Y + (float) projectile.height)
                {
                    flag3 = true;
                }

                Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width,
                    projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY, 1, false, 0);
                if (projectile.velocity.Y == 0f)
                {
                    if (!flag3 && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
                    {
                        int num106 = (int) (projectile.position.X + (float) (projectile.width / 2)) / 16;
                        int j3 = (int) (projectile.position.Y + (float) (projectile.height / 2)) / 16 + 1;
                        if (flag)
                        {
                            num106--;
                        }

                        if (flag2)
                        {
                            num106++;
                        }

                        WorldGen.SolidTile(num106, j3);
                    }

                    if (flag4)
                    {
                        int num107 = (int) (projectile.position.X + (float) (projectile.width / 2)) / 16;
                        int num108 = (int) (projectile.position.Y + (float) projectile.height) / 16 + 1;
                        if (WorldGen.SolidTile(num107, num108) || Main.tile[num107, num108].halfBrick() ||
                            Main.tile[num107, num108].slope() > 0 || ProjectileID.Puppy == 200)
                        {
                            {
                                try
                                {
                                    num107 = (int) (projectile.position.X + (float) (projectile.width / 2)) / 16;
                                    num108 = (int) (projectile.position.Y + (float) (projectile.height / 2)) / 16;
                                    if (flag)
                                    {
                                        num107--;
                                    }

                                    if (flag2)
                                    {
                                        num107++;
                                    }

                                    num107 += (int) projectile.velocity.X;
                                    if (!WorldGen.SolidTile(num107, num108 - 1) &&
                                        !WorldGen.SolidTile(num107, num108 - 2))
                                    {
                                        projectile.velocity.Y = -5.1f;
                                    }
                                    else if (!WorldGen.SolidTile(num107, num108 - 2))
                                    {
                                        projectile.velocity.Y = -7.1f;
                                    }
                                    else if (WorldGen.SolidTile(num107, num108 - 5))
                                    {
                                        projectile.velocity.Y = -11.1f;
                                    }
                                    else if (WorldGen.SolidTile(num107, num108 - 4))
                                    {
                                        projectile.velocity.Y = -10.1f;
                                    }
                                    else
                                    {
                                        projectile.velocity.Y = -9.1f;
                                    }
                                }
                                catch
                                {
                                    projectile.velocity.Y = -9.1f;
                                }
                            }
                        }
                    }
                }

                if (projectile.velocity.X > num104)
                {
                    projectile.velocity.X = num104;
                }

                if (projectile.velocity.X < -num104)
                {
                    projectile.velocity.X = -num104;
                }

                if (projectile.velocity.X < 0f)
                {
                    projectile.direction = -1;
                }

                if (projectile.velocity.X > 0f)
                {
                    projectile.direction = 1;
                }

                if (projectile.velocity.X > num103 && flag2)
                {
                    projectile.direction = 1;
                }

                if (projectile.velocity.X < -num103 && flag)
                {
                    projectile.direction = -1;
                }

                if (flag5)
                {
                    if (projectile.ai[1] > 0f)
                    {
                        if (projectile.localAI[1] == 0f)
                        {
                            projectile.localAI[1] = 1f;
                            projectile.frame = 1;
                        }

                        if (projectile.frame != 0)
                        {
                            projectile.frameCounter++;
                            if (projectile.frameCounter > 4)
                            {
                                projectile.frame++;
                                projectile.frameCounter = 0;
                            }

                            if (projectile.frame == 4)
                            {
                                projectile.frame = 0;
                            }
                        }
                    }
                    else if (projectile.velocity.Y == 0f)
                    {
                        projectile.localAI[1] = 0f;
                        if (projectile.velocity.X == 0f)
                        {
                            projectile.frame = 0;
                            projectile.frameCounter = 0;
                        }
                        else if ((double) projectile.velocity.X < -0.8 || (double) projectile.velocity.X > 0.8)
                        {
                            projectile.frameCounter += (int) Math.Abs(projectile.velocity.X);
                            projectile.frameCounter++;
                            if (projectile.frameCounter > 6)
                            {
                                projectile.frame++;
                                projectile.frameCounter = 0;
                            }

                            if (projectile.frame < 5)
                            {
                                projectile.frame = 5;
                            }

                            if (projectile.frame >= 11)
                            {
                                projectile.frame = 5;
                            }
                        }
                        else
                        {
                            projectile.frame = 0;
                            projectile.frameCounter = 0;
                        }
                    }
                    else if (projectile.velocity.Y < 0f)
                    {
                        projectile.frameCounter = 0;
                        projectile.frame = 4;
                    }
                    else if (projectile.velocity.Y > 0f)
                    {
                        projectile.frameCounter = 0;
                        projectile.frame = 4;
                    }

                    projectile.velocity.Y = projectile.velocity.Y + 0.4f;
                    if (projectile.velocity.Y > 10f)
                    {
                        projectile.velocity.Y = 10f;
                    }

                    float arg_5B67_0 = projectile.velocity.Y;
                    return;
                }

                if (projectile.velocity.Y == 0f)
                {
                    if (projectile.velocity.X == 0f)
                    {
                        if (projectile.frame > 0)
                        {
                            projectile.frameCounter += 2;
                            if (projectile.frameCounter > 6)
                            {
                                projectile.frame++;
                                projectile.frameCounter = 0;
                            }

                            if (projectile.frame >= 7)
                            {
                                projectile.frame = 0;
                            }
                        }
                        else
                        {
                            projectile.frame = 0;
                            projectile.frameCounter = 0;
                        }
                    }
                    else if ((double) projectile.velocity.X < -0.8 || (double) projectile.velocity.X > 0.8)
                    {
                        projectile.frameCounter += (int) Math.Abs((double) projectile.velocity.X * 0.75);
                        projectile.frameCounter++;
                        if (projectile.frameCounter > 6)
                        {
                            projectile.frame++;
                            projectile.frameCounter = 0;
                        }

                        if (projectile.frame >= 7 || projectile.frame < 1)
                        {
                            projectile.frame = 1;
                        }
                    }
                    else if (projectile.frame > 0)
                    {
                        projectile.frameCounter += 2;
                        if (projectile.frameCounter > 6)
                        {
                            projectile.frame++;
                            projectile.frameCounter = 0;
                        }

                        if (projectile.frame >= 7)
                        {
                            projectile.frame = 0;
                        }
                    }
                    else
                    {
                        projectile.frame = 0;
                        projectile.frameCounter = 0;
                    }
                }
                else if (projectile.velocity.Y < 0f)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 2;
                }
                else if (projectile.velocity.Y > 0f)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 4;
                }

                projectile.velocity.Y = projectile.velocity.Y + 0.4f;
                if (projectile.velocity.Y > 10f)
                {
                    projectile.velocity.Y = 10f;
                    return;
                }
            }


        }

        public void UsePotion()
        {
            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (modPlayer.ActivePet.HP == modPlayer.ActivePet.MaxHP)
            {
                Main.NewText($"It'll have no effect on {TerramonMod.Localisation.GetLocalisedString(new LocalisedString(modPlayer.ActivePetName))}.", Color.LightGray);
                return;
            }
            Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 13);
            Main.LocalPlayer.HeldItem.stack--;
            CombatText.NewText(projectile.Hitbox, CombatText.HealLife, modPlayer.ActivePet.Heal(20));
            for (int i = 0; i < 24; i++)
            {
                var d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,
                    156, 0, 0, 0, new Color(165, 132, 206));
                d.noGravity = true;
            }
        }

        public void UseSuperPotion()
        {
            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (modPlayer.ActivePet.HP == modPlayer.ActivePet.MaxHP)
            {
                Main.NewText($"It'll have no effect on {TerramonMod.Localisation.GetLocalisedString(new LocalisedString(modPlayer.ActivePetName))}.", Color.LightGray);
                return;
            }
            Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 13);
            Main.LocalPlayer.HeldItem.stack--;
            CombatText.NewText(projectile.Hitbox, CombatText.HealLife, modPlayer.ActivePet.Heal(50));
            for (int i = 0; i < 24; i++)
            {
                var d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,
                    156, 0, 0, 0, new Color(165, 132, 206));
                d.noGravity = true;
            }
        }

        public void UseHyperPotion()
        {
            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (modPlayer.ActivePet.HP == modPlayer.ActivePet.MaxHP)
            {
                Main.NewText($"It'll have no effect on {TerramonMod.Localisation.GetLocalisedString(new LocalisedString(modPlayer.ActivePetName))}.", Color.LightGray);
                return;
            }
            Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 13);
            Main.LocalPlayer.HeldItem.stack--;
            CombatText.NewText(projectile.Hitbox, CombatText.HealLife, modPlayer.ActivePet.Heal(120));
            for (int i = 0; i < 24; i++)
            {
                var d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,
                    156, 0, 0, 0, new Color(165, 132, 206));
                d.noGravity = true;
            }
        }
        public void UseMaxPotion()
        {
            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (modPlayer.ActivePet.HP == modPlayer.ActivePet.MaxHP)
            {
                Main.NewText($"It'll have no effect on {TerramonMod.Localisation.GetLocalisedString(new LocalisedString(modPlayer.ActivePetName))}.", Color.LightGray);
                return;
            }
            Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 13);
            Main.LocalPlayer.HeldItem.stack--;
            CombatText.NewText(projectile.Hitbox, CombatText.HealLife, modPlayer.ActivePet.Heal(modPlayer.ActivePet.MaxHP));
            for (int i = 0; i < 24; i++)
            {
                var d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,
                    156, 0, 0, 0, new Color(165, 132, 206));
                d.noGravity = true;
            }
        }

        // TO-DO: Make full restore cure status conditions once they are implemented
        public void UseFullRestore()
        {
            Player player = Main.player[projectile.owner];
            TerramonPlayer modPlayer = player.GetModPlayer<TerramonPlayer>();
            if (modPlayer.ActivePet.HP == modPlayer.ActivePet.MaxHP)
            {
                Main.NewText($"It'll have no effect on {TerramonMod.Localisation.GetLocalisedString(new LocalisedString(modPlayer.ActivePetName))}.", Color.LightGray);
                return;
            }
            Main.PlaySound(SoundID.Item, Main.LocalPlayer.position, 13);
            Main.LocalPlayer.HeldItem.stack--;
            CombatText.NewText(projectile.Hitbox, CombatText.HealLife, modPlayer.ActivePet.Heal(modPlayer.ActivePet.MaxHP));
            for (int i = 0; i < 24; i++)
            {
                var d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,
                    156, 0, 0, 0, new Color(165, 132, 206));
                d.noGravity = true;
            }
        }
    }


    public enum EvolveItem
    {
        RareCandy,
        LinkCable,
        FireStone,
        ThunderStone,
        WaterStone,
        LeafStone,
        MoonStone,
        Eeveelution
    }
}