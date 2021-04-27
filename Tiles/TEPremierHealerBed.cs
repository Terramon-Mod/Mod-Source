using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terramon.Players;
using Terramon.UI;
using Razorwing.Framework.Graphics;

namespace Terramon.Tiles
{
    public class TEPremierHealerBed : ModTileEntity
    {
        // this tile entity is crucial in making healer bed animation and function work.

        public bool active = false;

        public bool generatedGameTime = false;

        public GameTime gameTime;

        public float anim = 0;

        public bool drawFirstBall = false;
        public bool playedFirstBwuip = false;

        public bool drawSecondBall = false;
        public bool playedSecondBwuip = false;

        public bool drawThirdBall = false;
        public bool playedThirdBwuip = false;

        public bool drawFourthBall = false;
        public bool playedFourthBwuip = false;

        public bool drawFifthBall = false;
        public bool playedFifthBwuip = false;

        public bool drawSixthBall = false;
        public bool playedSixthBwuip = false;

        public bool playedHealingSfx = false;
        public override void Update()
        {
            if (!generatedGameTime)
            {
                gameTime = new GameTime();
            }

            anim += (float)gameTime.ElapsedGameTime.TotalSeconds;

            var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();

            if (active)
            {
                anim++;

                if (anim <= 30)
                {
                    drawFirstBall = true;
                    if (!playedFirstBwuip)
                    {
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/bwuip").WithVolume(.4f));
                    }
                    playedFirstBwuip = true;
                }
                else if (anim > 30 && player.PartySlot2 != null)
                {
                    drawSecondBall = true;
                    if (!playedSecondBwuip) Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/bwuip").WithVolume(.4f));
                    playedSecondBwuip = true;
                }
                if (anim >= 60 && player.PartySlot3 != null)
                {
                    drawThirdBall = true;
                    if (!playedThirdBwuip) Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/bwuip").WithVolume(.4f));
                    playedThirdBwuip = true;
                }
                if (anim >= 90 && player.PartySlot4 != null)
                {
                    drawFourthBall = true;
                    if (!playedFourthBwuip) Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/bwuip").WithVolume(.4f));
                    playedFourthBwuip = true;
                }
                if (anim >= 120 && player.PartySlot5 != null)
                {
                    drawFifthBall = true;
                    if (!playedFifthBwuip) Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/bwuip").WithVolume(.4f));
                    playedFifthBwuip = true;
                }
                if (anim >= 150 && player.PartySlot6 != null)
                {
                    drawSixthBall = true;
                    if (!playedSixthBwuip) Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/bwuip").WithVolume(.4f));
                    playedSixthBwuip = true;
                }
                if (anim >= 190)
                {
                    player.healingAtHealerBed = true;

                    if (!playedHealingSfx)
                    {
                        var lp = Main.LocalPlayer;
                        CombatText.NewText(lp.Hitbox, new Color(75, 201, 96), "+", true);
                        Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/recovery").WithVolume(.4f));

                        // Heal mons
                        if (player.PartySlot1 != null) player.PartySlot1.HP = player.PartySlot1.MaxHP;
                        if (player.PartySlot2 != null) player.PartySlot2.HP = player.PartySlot2.MaxHP;
                        if (player.PartySlot3 != null) player.PartySlot3.HP = player.PartySlot3.MaxHP;
                        if (player.PartySlot4 != null) player.PartySlot4.HP = player.PartySlot4.MaxHP;
                        if (player.PartySlot5 != null) player.PartySlot5.HP = player.PartySlot5.MaxHP;
                        if (player.PartySlot6 != null) player.PartySlot6.HP = player.PartySlot6.MaxHP;

                        Main.NewText("All of your party Pokémon were healed to full health!", Color.LightGreen);
                    }
                    playedHealingSfx = true;
                }
                if (anim >= 400)
                {
                    active = false;
                }
            }
            else
            {
                anim = 0;

                drawFirstBall = false;
                playedFirstBwuip = false;
                drawSecondBall = false;
                playedSecondBwuip = false;
                drawThirdBall = false;
                playedThirdBwuip = false;
                drawFourthBall = false;
                playedFourthBwuip = false;
                drawFifthBall = false;
                playedFifthBwuip = false;
                drawSixthBall = false;
                playedSixthBwuip = false;
                playedHealingSfx = false;
                player.healingAtHealerBed = false;
            }
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == ModContent.TileType<PremierHealerBedTile>();
        }
        public virtual void OnKill()
        {
        }
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            // i - 1 and j - 2 come from the fact that the origin of the tile is "new Point16(1, 2);", so we need to pass the coordinates back to the top left tile. If using a vanilla TileObjectData.Style, make sure you know the origin value.
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i - 1, j - 1, 3); // this is -1, -1, however, because -1, -1 places the 3 diameter square over all the tiles, which are sent to other clients as an update.
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - 1, j - 2, Type, 0f, 0, 0, 0);
                return -1;
            }
            return Place(i - 1, j - 1);
        }
    }
}