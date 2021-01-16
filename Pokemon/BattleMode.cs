using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Razorwing.Framework.Graphics;
using Razorwing.Framework.Graphics.Transforms;
using Razorwing.Framework.Localisation;
using Razorwing.Framework.Utils;
using Terramon.Network.Sync;
using Terramon.Network.Sync.Battle;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terramon.UI;
using Terramon.UI.Battling;
using Terramon.UI.Moveset;
using Terramon.UI.SidebarParty;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using System.Reflection;

namespace Terramon.Pokemon
{
    /// <summary>
    /// Class what handles turn based battles between players and bots
    /// </summary>
    public class BattleMode
    {
        internal static BattleUI UI;// We will have a singleton battle UI. Move it to TerramonMod later...
        public BattleState State;
        public BattleStyle Style;
        public TerramonPlayer player1, player2;
        public PokemonData Wild;
        public ParentPokemon WildNPC;
        public int wildID;
        public bool awaitSync = false;
        protected BaseMove pMove, oMove;
        public int ForceDirection = 0; // Use to force player direction
        public bool MoveDone => pMove != null;
        //protected int atackTimeout;
        protected byte animInProggress; // 0 idle, 1 casting, 2 done

        public bool battleJustStarted = false;

        public static bool doneWildIntroAppear = false;
        public static bool doneWildIntro = false;

        public bool playMainLoop = false;

        protected ILocalisedBindableString playerChallenge =
            TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("playerChallenge", "{0} is challenging you!")));
        protected ILocalisedBindableString wildChallenge =
            TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("wildChallenge", "Wild {0} was appeared!")));

        protected ILocalisedBindableString pokeName1, pokeName2;

        // This is for checking if in main menu which has the Fight, Bag, Pokemon and Run buttons
        public static bool inMainMenu = false;
        public static bool queueRunAway = false;
        public static bool queueEndMove = false;

        public BattleMode(TerramonPlayer fpl, BattleState state, PokemonData second = null, ParentPokemonNPC npc = null, TerramonPlayer spl = null, bool lazy = false, BattleStyle bs = BattleStyle.Default)
        {

            if (fpl.player == Main.LocalPlayer) //If this is client player
            {
                //Initialize battle UI
                if (UI == null)
                    UI = new BattleUI();
                else
                {
                    UI.ResetData();
                }
                //And make it visible
                BattleUI.Visible = true;
                battleJustStarted = true;
                inMainMenu = true;
            }

            State = state;
            Style = bs;
            player1 = fpl;
            player2 = spl;
            Wild = second;
            animInProggress = 0;

            //Guard some code from repeating
            if(!lazy)
            //If we received npc argument
                if (npc != null)
                {
                    Wild = new PokemonData()
                    {
                        Pokemon = npc.HomeClass().Name
                    };
                    Wild.HP = Wild.MaxHP;
                    //Replace NPC with projectile
                    if (Main.netMode == NetmodeID.SinglePlayer || player1.player == Main.LocalPlayer)
                    {

                        wildID = Projectile.NewProjectile(npc.npc.position, Vector2.Zero,
                            TerramonMod.Instance.ProjectileType(npc.HomeClass().Name), 0, 0, fpl.whoAmI);
                        npc.npc.active = false;
                        WildNPC = (ParentPokemon)Main.projectile[wildID].modProjectile;
                        WildNPC.Wild = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            var packet = new WildNPCTransformPacket();
                            int i;
                            for (i = 0; i < Main.maxNPCs; i++)
                            {
                                if (Main.npc[i] == npc.npc)
                                    break;
                            }
                            packet.Send(TerramonMod.Instance, i, wildID);

                        }
                    }
                    //else
                    //{
                    //    //Make proj packet
                    //    var packet = new WildNPCTransformPacket();
                    //    int i;
                    //    for (i = 0; i < Main.maxNPCs; i++)
                    //    {
                    //        if(Main.npc[i] == npc.npc)
                    //            break;
                    //    }
                    //    packet.Send(TerramonMod.Instance, i);
                    //    awaitSync = true;
                    //}
                   
                }

            pokeName1 = TerramonMod.Localisation.GetLocalisedString(fpl.ActivePetName);
            if (!Main.dedServ && player1.player == Main.LocalPlayer)
            {
                switch (State)
                {
                    case BattleState.BattleWithPlayer:
                        pokeName2 = TerramonMod.Localisation.GetLocalisedString(spl?.ActivePetName);
                        UI.HP1.PokeData = fpl.ActivePet;
                        UI.HP2.PokeData = spl?.ActivePet;
                        UI.MovesPanel.PokeData = fpl.ActivePet;
                        playerChallenge.Args = new object[] { spl?.player.name };
                        Text(playerChallenge.Value, true);
                        break;
                    case BattleState.BattleWithWild:
                        pokeName2 = TerramonMod.Localisation.GetLocalisedString(second?.Pokemon);
                        UI.HP1.PokeData = fpl.ActivePet;
                        UI.HP2.PokeData = Wild;
                        //string[] m = BaseMove.DefaultMoves((ParentPokemon)Main.projectile[player1.ActivePetId].modProjectile);
                        UI.MovesPanel.PokeData = new PokemonData()
                        {
                            Pokemon = npc.HomeClass().Name,
                            Moves = new BaseMove[] { new Pound(), new KarateChop(), new DoubleSlap(), new Earthquake() }
                        };
                        wildChallenge.Args = new object[] { second?.Pokemon };
                        //Text(wildChallenge.Value);
                        break;
                }

                UI.MovePresed = (move) =>
                {
                    pMove = move;
                    if (State == BattleState.BattleWithPlayer)
                    {
                        player2?.Battle?.SyncMove(move.GetType().Name, false);
                    }
                    //Move chose packet
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        var p = new MoveChangePacket();
                        p.Send(TerramonMod.Instance, move);
                    }
                };
            }

        }

        private int escapeCountdown = 0;
        public static int animWindow = 0;
        public static bool moveEnd = false;
        private byte animMode = 0;//0 Idle, 1 playerMonAnim, 2 enemyMonAnim, 3 enemyMonContinue, 4 playerMonContinue
        public int introMusicTimer = 0;
        private int wildTimer = 0;
        public static int endMoveTimer;

        // camera control
        private Vector2 OffsetModifier;
        public void Update()
        {
            // prevent player from opening inventory during battle
            Player player = Main.LocalPlayer;
            player.releaseInventory = false;

            // increment wild timer
            wildTimer++;
            if (!playMainLoop) introMusicTimer++;

            if (introMusicTimer > 13.832 * 60) // finished intro music
            {
                playMainLoop = true;
                introMusicTimer = 0;
            }

            if (introMusicTimer == 1)
            {
                TerramonMod.ZoomAnimator.WhiteFlashOpacity(1f, 130, Easing.None);
            }

            if (introMusicTimer == 10)
            {
                TerramonMod.ZoomAnimator.WhiteFlashOpacity(0f, 110, Easing.None);
            }

            if (introMusicTimer == 18)
            {
                TerramonMod.ZoomAnimator.WhiteFlashOpacity(1f, 130, Easing.None);
            }

            if (introMusicTimer == 28)
            {
                TerramonMod.ZoomAnimator.WhiteFlashOpacity(0f, 110, Easing.None);
            }

            if (introMusicTimer == 38)
            {
                Filters.Scene.Activate("BattleIntro", Vector2.Zero);
            }

            if (introMusicTimer > 37 && introMusicTimer < 165)
            {
                SetParameters(Interpolation.ValueAt(introMusicTimer, 0f, 0.5f, 37, 125, Easing.OutExpo), Interpolation.ValueAt(introMusicTimer, 0f, 0.2f, 37, 125, Easing.OutExpo), CreateZoomMatrix(1f, player1.player.Center - Main.screenPosition, Main.screenWidth, Main.screenHeight));
            }

            if (introMusicTimer == 42) TerramonMod.ZoomAnimator.GameZoom(5f, 1500, Easing.InExpo);

            if (introMusicTimer == 44)
            {
                TerramonMod.ZoomAnimator.WhiteFlashOpacity(1f, 1200, Easing.None);
            }

            if (introMusicTimer == 105) Filters.Scene.Deactivate("BattleIntro");

            if (introMusicTimer == 165)
            {
                ParentPokemon playerpet = (ParentPokemon)Main.projectile[player1.ActivePetId].modProjectile;
                playerpet.SpawnTime = -252;
                playerpet.DontTpOnCollide = true;
                UI.HP2.Left.Set(160, 0f);
            }

            if (introMusicTimer == 170)
            {
                // Zoom in
                TerramonMod.ZoomAnimator.GameZoom(1f).GameZoom(1.7f, 0, Easing.None);
                // Pan camera to wild opponent, if this is BattleWithWild
                if (State == BattleState.BattleWithWild)
                {
                    TerramonMod.ZoomAnimator.ScreenPosX(WildNPC.projectile.position.X + 12, 0, Easing.None);
                    TerramonMod.ZoomAnimator.ScreenPosY(WildNPC.projectile.position.Y, 0, Easing.None);
                }
                TerramonMod.ZoomAnimator.WhiteFlashOpacity(0f, 200, Easing.None);
            }

            // force music fade to null
            Main.musicFade[ModContent.GetInstance<TerramonMod>().GetSoundSlot(SoundType.Music, "Sounds/Music/Battling/wildbattle")] = 1f;

            // make player1 pokemon face wildnpc projectile
            if (State == BattleState.BattleWithWild)
            {
                Main.projectile[player1.ActivePetId].modProjectile.projectile.spriteDirection = Main.projectile[player1.ActivePetId].modProjectile.projectile.position.X > WildNPC.projectile.position.X ? 1 : -1;
            }

            // return to main menu from moves select menu via escape key
            if (Main.keyState.IsKeyDown(Keys.Escape) && !inMainMenu)
            {
                inMainMenu = true;
            }

            if (pMove?.AnimationFrame == 140 && pMove.Target != Target.Self)
            {
                pMove?.CheckIfAffects(WildNPC, Wild, State, false);
            }

            if (oMove?.AnimationFrame == 140 && oMove.Target != Target.Self)
            {
                oMove?.CheckIfAffects((ParentPokemon)(Main.projectile[player1.ActivePetId].modProjectile), player1.ActivePet, State, false);
            }

            // CAMERA & ZOOM CONTROL //
            if (Main.keyState.IsKeyDown(Keys.D) && doneWildIntro && UI.Turn)
            {
                //if (UI.tipText.Text == "Use WASD to move the camera around.") UI.tipText.SetText("Press SPACE to reset the camera.");
                OffsetModifier.X += 4;
                TerramonMod.ZoomAnimator.ScreenPosX((Main.projectile[player1.ActivePetId].modProjectile.projectile.position.X + 12) + OffsetModifier.X, 1, Easing.None);
            }
            if (Main.keyState.IsKeyDown(Keys.A) && doneWildIntro && UI.Turn)
            {
                //if (UI.tipText.Text == "Use WASD to move the camera around.") UI.tipText.SetText("Press SPACE to reset the camera.");
                OffsetModifier.X -= 4;
                TerramonMod.ZoomAnimator.ScreenPosX((Main.projectile[player1.ActivePetId].modProjectile.projectile.position.X + 12) + OffsetModifier.X, 1, Easing.None);
            }
            if (Main.keyState.IsKeyDown(Keys.W) && doneWildIntro && UI.Turn)
            {
                //if (UI.tipText.Text == "Use WASD to move the camera around.") UI.tipText.SetText("Press SPACE to reset the camera.");
                OffsetModifier.Y -= 4;
                TerramonMod.ZoomAnimator.ScreenPosY((Main.projectile[player1.ActivePetId].modProjectile.projectile.position.Y) + OffsetModifier.Y, 1, Easing.None);
            }
            if (Main.keyState.IsKeyDown(Keys.S) && doneWildIntro && UI.Turn)
            {
                //if (UI.tipText.Text == "Use WASD to move the camera around.") UI.tipText.SetText("Press SPACE to reset the camera.");
                OffsetModifier.Y += 4;
                TerramonMod.ZoomAnimator.ScreenPosY((Main.projectile[player1.ActivePetId].modProjectile.projectile.position.Y) + OffsetModifier.Y, 1, Easing.None);
            }
            if (Main.keyState.IsKeyDown(Keys.Space) && doneWildIntro && UI.Turn && OffsetModifier != Vector2.Zero)
            {
                //if (UI.tipText.Text == "Press SPACE to reset the camera.") UI.tipText.SetText("Use UP and DOWN arrows to zoom in/out.");
                OffsetModifier = Vector2.Zero;
                TerramonMod.ZoomAnimator.ScreenPosX((Main.projectile[player1.ActivePetId].modProjectile.projectile.position.X + 12), 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY((Main.projectile[player1.ActivePetId].modProjectile.projectile.position.Y), 500, Easing.OutExpo);
            }
            if (Main.keyState.IsKeyDown(Keys.Up) && doneWildIntro && UI.Turn && Main.GameZoomTarget < 2f)
            {
                //if (UI.tipText.Text == "Use UP and DOWN arrows to zoom in/out.") UI.tipText.SetText("");
                TerramonMod.ZoomAnimator.GameZoom(Main.GameZoomTarget + 0.03f, 1, Easing.None);
            }
            if (Main.keyState.IsKeyDown(Keys.Down) && doneWildIntro && UI.Turn && Main.GameZoomTarget > 1f)
            {
                //if (UI.tipText.Text == "Use UP and DOWN arrows to zoom in/out.") UI.tipText.SetText("");
                TerramonMod.ZoomAnimator.GameZoom(Main.GameZoomTarget - 0.03f, 1, Easing.None);
            }
            // END CAMERA & ZOOM CONTROL //



            if (battleJustStarted && introMusicTimer > 190)
            {
                Main.PlaySound(ModContent.GetInstance<TerramonMod>()
                        .GetLegacySoundSlot(SoundType.Custom, "Sounds/Cries/cry" + Wild.Pokemon).WithVolume(0.55f));
                
                // Set splash text
                UI.splashText.SetText($"A wild {Wild.PokemonName} appeared!");

                battleJustStarted = false;
            }

            if (wildTimer == 370 && !doneWildIntroAppear)
            {
                TerramonMod.ZoomAnimator.ScreenPosX(player1.player.position.X + 10, 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(player1.player.position.Y, 500, Easing.OutExpo);

                // Set splash text
                UI.splashText.SetText($"Go! {player1.ActivePet.PokemonName}!");

                ForceDirection = Main.projectile[player1.ActivePetId].modProjectile.projectile.position.X > player1.player.position.X ? 1 : -1;

                doneWildIntroAppear = true;
            }

            if (wildTimer == 470 && !doneWildIntro)
            {
                TerramonMod.ZoomAnimator.HPBar1LeftPixels(-340, 500, Easing.OutExpo);
            }

            if (wildTimer == 430 && !doneWildIntro)
            {
                ParentPokemon playerpet = (ParentPokemon)Main.projectile[player1.ActivePetId].modProjectile;
                playerpet.dootscale = 0.1f;
                playerpet.whiteFlashVal = 1f;
                playerpet.DontTpOnCollide = false;
            }

            if (wildTimer >= 515 && !doneWildIntro)
            {
                doneWildIntro = true;
                ForceDirection = 0;
            }

            // Make wild pokemon jump every so often
            if (wildTimer >= 1300 && UI.Turn)
            {
                wildTimer = 0;
                WildNPC.hopTimer = 0;
            }

            if (queueRunAway)
            {
                queueRunAway = false;

                if (State == BattleState.BattleWithPlayer)
                {
                    if (Text("You escaped, but lost some money...", true));
                    //Escape packet
                }

                if (Text("Got away safely!", new Color(200, 50, 70), true) && Main.netMode == NetmodeID.MultiplayerClient)
                    new BattleEndPacket().Send(TerramonMod.Instance);

                EndBattle();
            }

            if (player1.player == Main.LocalPlayer && Main.keyState.IsKeyDown(Keys.Escape))
            {
                if (escapeCountdown == 0)
                    escapeCountdown = 5*60;
                if (escapeCountdown == 3*60)
                    Text("Trying to escape...",new Color(200,50,70), true);
                escapeCountdown -= 1;

                if (escapeCountdown == 0)
                {
                    if (State == BattleState.BattleWithPlayer)
                    {
                        if (Text("You escaped, but lost some money...", true)) ;
                        //Escape packet
                    }
                    
                    if(Text("Escaped!", new Color(200, 50, 70), true) && Main.netMode == NetmodeID.MultiplayerClient)
                        new BattleEndPacket().Send(TerramonMod.Instance);
                    TerramonMod.ZoomAnimator.GameZoom(1f, 500, Easing.Out);
                    State = BattleState.None;
                }
            }

            switch (State)
            {
                case BattleState.None:
                    BattleUI.Visible = false;
                    return;
                case BattleState.BattleWithWild when !WildNPC?.projectile.active ?? !awaitSync:
                    Text($"Pokemon disappear!");
                    //End packet
                    State = BattleState.None;
                    break;
            }

            if (awaitSync)
            {
                return;
            }
            else
            {
                faintedPrinted = false;
            }


            //atackTimeout = atackTimeout > 0 ? atackTimeout - 1 : 0;
            animWindow = animMode > 0 ? animWindow + 1 : animWindow;

            if (pMove == null && animInProggress == 0 && Main.LocalPlayer == player1.player)
            {
                UI.Turn = true;
            }

            if (State != BattleState.BattleWithPlayer && oMove == null) // If this is single player
            {
                //Need an advanced AI for trainers
                int d = Main.rand.Next(1, 3);
                if (d == 0) oMove = new ShootMove();
                if (d == 1) oMove = new Absorb();
                if (d == 2) oMove = new Acid();
            }

            if (animInProggress == 2)
            {
                if (animMode > 2)
                {
                    oMove = null;
                    pMove = null;
                    animMode = 0;
                }

                animInProggress = 0;
            }

            if (queueEndMove)
            {
                endMoveTimer++;
                if (endMoveTimer > 100)
                {
                    moveEnd = true;
                    queueEndMove = false;
                    endMoveTimer = 0;
                }
            }

            if (animInProggress == 1)
            {
                if (animMode == 1 || animMode == 4)
                {
                    if (pMove != null)
                    {
                        pMove.AnimationFrame = animWindow;
                        switch (State)
                        {
                            case BattleState.BattleWithWild:
                                animInProggress = (byte)(pMove.AnimateTurn((ParentPokemon)Main.projectile[player1.ActivePetId].modProjectile, WildNPC, player1, player1.ActivePet, Wild, State, false) ? 1 : 2);
                                break;
                            case BattleState.BattleWithPlayer:
                                animInProggress = (byte)(pMove.AnimateTurn((ParentPokemon)Main.projectile[player1.ActivePetId].modProjectile, (ParentPokemon)Main.projectile[player2.ActivePetId].modProjectile, player1, player1.ActivePet, player2.ActivePet, State, false) ? 1 : 2);
                                break;
                        }
                    }
                }
                else if(animMode == 2 || animMode == 3)
                {
                    if (State != BattleState.BattleWithPlayer)
                        if (oMove != null)
                        {
                            oMove.AnimationFrame = animWindow;
                            switch (State)
                            {
                                case BattleState.BattleWithWild:
                                    animInProggress = (byte)(oMove.AnimateTurn(WildNPC, (ParentPokemon)(Main.projectile[player1.ActivePetId].modProjectile), null, Wild,
                                        player1.ActivePet, State, true) ? 1 : 2);
                                    break;
                                case BattleState.BattleWithPlayer:
                                    //oMove.AnimateTurn((ParentPokemon)(Main.projectile[player2.ActivePetId].modProjectile), (ParentPokemon)(Main.projectile[player1.ActivePetId].modProjectile), player2, player2.ActivePet,
                                    //    player1.ActivePet);
                                    break;
                            }
                            
                        }
                        
                }

            }else if (animMode > 0 && animMode < 3)
            {
                animMode += 2;//Shift to second casts
                animWindow = 0;
                animInProggress = 1;
                switch (animMode)
                {
                    case 3:
                        switch (State)
                        {
                            case BattleState.BattleWithWild:
                            case BattleState.BattleWithTrainer:
                                if (!Wild.Fainted)
                                {
                                    if (oMove != null)
                                    {
                                        oMove.AnimationFrame = 0;
                                        oMove.PerformInBattle(WildNPC, (ParentPokemon)(Main.projectile[player1.ActivePetId].modProjectile), null, Wild,
                                            player1.ActivePet, oMove);
                                    }                                    
                                }
                                break;
                            case BattleState.BattleWithPlayer:
                                if (!player2.ActivePet.Fainted)
                                {
                                    if (oMove != null)
                                    {
                                        oMove.AnimationFrame = 0;
                                        //oMove?.PerformInBattle((ParentPokemon)Main.projectile[player2.ActivePetId].modProjectile
                                        //    , (ParentPokemon)Main.projectile[player2.ActivePetId].modProjectile, player2,
                                        //    player2.ActivePet, player1.ActivePet);
                                    }
                                }
                                break;
                        }
                        
                        break;
                    case 4:
                        if (!player1.ActivePet.Fainted)
                        {
                            if (pMove != null)
                            {
                                pMove.AnimationFrame = 0;
                                switch (State)
                                {
                                    case BattleState.BattleWithWild:
                                        pMove?.PerformInBattle((ParentPokemon)Main.projectile[player1.ActivePetId].modProjectile, WildNPC,
                                            player1, player1.ActivePet, Wild, pMove);
                                        break;
                                    case BattleState.BattleWithPlayer:
                                        pMove?.PerformInBattle((ParentPokemon)Main.projectile[player1.ActivePetId].modProjectile, (ParentPokemon)Main.projectile[player2.ActivePetId].modProjectile,
                                            player1, player1.ActivePet, player2.ActivePet, pMove);
                                        break;
                                }
                            }
                        }
                        break;
                }
            }
            else
            if (pMove != null && oMove != null)
            {
                //Calculate speed here
                bool useCheck = true;
                switch (State)
                {
                    case BattleState.BattleWithWild:
                        if (pMove.Priority != oMove.Priority) // Priority checks
                        {
                            if (pMove.Priority > oMove.Priority) useCheck = false;
                            else useCheck = true;
                            break;
                        }
                        useCheck = (player1.ActivePet.Speed) < (Wild.Speed);
                        break;
                    case BattleState.BattleWithPlayer:
                        if (pMove.Priority != oMove.Priority) // Priority checks
                        {
                            if (pMove.Priority > oMove.Priority) useCheck = false;
                            else useCheck = true;
                            break;
                        }
                        useCheck = (player1.ActivePet.Speed) < (player2.ActivePet.Speed);
                        break;
                }

                if (!useCheck)
                {
                    pMove.AnimationFrame = 0;
                    switch (State)
                    {
                        case BattleState.BattleWithWild:
                            pMove?.PerformInBattle((ParentPokemon)Main.projectile[player1.ActivePetId].modProjectile, WildNPC,
                                player1, player1.ActivePet, Wild, pMove);
                            break;
                        case BattleState.BattleWithPlayer:
                            pMove?.PerformInBattle((ParentPokemon)Main.projectile[player1.ActivePetId].modProjectile, (ParentPokemon)Main.projectile[player2.ActivePetId].modProjectile,
                                player1, player1.ActivePet, player2.ActivePet, pMove);
                            break;
                    }

                    animMode = 1;
                }
                else
                {
                    oMove.AnimationFrame = 0;
                    switch (State)
                    {
                        case BattleState.BattleWithWild:
                            oMove?.PerformInBattle(WildNPC, (ParentPokemon)(Main.projectile[player1.ActivePetId].modProjectile),
                                null, Wild, player1.ActivePet, oMove);
                            break;
                        case BattleState.BattleWithPlayer:
                            oMove?.PerformInBattle((ParentPokemon)(Main.projectile[player2.ActivePetId].modProjectile), (ParentPokemon)(Main.projectile[player1.ActivePetId].modProjectile),
                                player2, player2.ActivePet, player1.ActivePet, oMove);
                            break;
                    }

                    animMode = 2;
                }
                animWindow = 0;
                animInProggress = 1;
                //atackTimeout = 260;
            }

            if (animInProggress != 1)
            {
                if (player1.ActivePet.Fainted ||
                    ((State == BattleState.BattleWithTrainer || State == BattleState.BattleWithWild) && Wild.Fainted) ||
                    (player2?.ActivePet?.Fainted ?? false))
                {
                    animMode = 5;
                    animWindow = 0;
                    pMove = null;
                    oMove = null;
                }
            }

            if (player1.ActivePet?.Fainted ?? false)
            {
                if (!faintedPrinted && (player1.PartySlot1 != null && !player1.PartySlot1.Fainted) ||
                    (player1.PartySlot2 != null && !player1.PartySlot2.Fainted) ||
                    (player1.PartySlot3 != null && !player1.PartySlot3.Fainted) ||
                    (player1.PartySlot4 != null && !player1.PartySlot4.Fainted) ||
                    (player1.PartySlot5 != null && !player1.PartySlot5.Fainted) ||
                    (player1.PartySlot6 != null && !player1.PartySlot6.Fainted))
                {
                    Text($"Your {player1.ActivePet.PokemonName} was fainted! Please, send out another pokemon!"); // TODO: Change this placeholders to LocalisedString
                    awaitSync = true;
                }
                else
                {
                    if(Text($"Your {player1.ActivePet.PokemonName} was fainted! You loose this battle!") && Main.netMode == NetmodeID.MultiplayerClient)
                        new BattleEndPacket().Send(TerramonMod.Instance);
                    EndBattle();
                    //State = BattleState.None;
                }
            }

            switch (State)
            {
                case BattleState.BattleWithWild:
                    if (Wild.Fainted && animInProggress != 1)
                    {
                        int received = player1.ActivePet.GiveEXP(player1.ActivePet, Wild, BattleState.BattleWithWild, 1);
                        if(Text($"The wild {Wild.PokemonName} fainted! {player1.ActivePet?.PokemonName} gained {received.ToString()} EXP!", true)) EndBattle(); ;
                    }
                    break;
                case BattleState.BattleWithPlayer:
                    if (!player2.player.active)
                    {
                        Text("Your opponent just disappear!", true);
                        //End packet
                        player2.Battle?.Cleanup();
                        player2.Battle = null;
                        EndBattle();
                        new BattleEndPacket().Send(TerramonMod.Instance);
                        //State = BattleState.None;
                    }
                    else if (!faintedPrinted && (player2.ActivePet.Fainted &&
                              ((player2.PartySlot1 != null && !player2.PartySlot1.Fainted) ||
                               (player2.PartySlot2 != null && !player2.PartySlot2.Fainted) ||
                               (player2.PartySlot3 != null && !player2.PartySlot3.Fainted) ||
                               (player2.PartySlot4 != null && !player2.PartySlot4.Fainted) ||
                               (player2.PartySlot5 != null && !player2.PartySlot5.Fainted) ||
                               (player2.PartySlot6 != null && !player2.PartySlot6.Fainted))))
                    {
                        if (Text(
                            $"Opponent pokemon was fainted. [PH] Your {player1.ActivePet?.PokemonName} received 50xp",
                            true))
                        {
                            faintedPrinted = true;
                            player1.ActivePet.Exp += 50;
                            awaitSync = true;
                            var pac = new PlayerSidebarSync();
                            pac.Send(TerramonMod.Instance, player1);
                            //send sync packet
                        }
                    }
                    else if (!faintedPrinted && (player2.ActivePet.Fainted &&
                                                 ((player2.PartySlot1 != null && player2.PartySlot1.Fainted) ||
                                                  (player2.PartySlot2 != null && player2.PartySlot2.Fainted) ||
                                                  (player2.PartySlot3 != null && player2.PartySlot3.Fainted) ||
                                                  (player2.PartySlot4 != null && player2.PartySlot4.Fainted) ||
                                                  (player2.PartySlot5 != null && player2.PartySlot5.Fainted) ||
                                                  (player2.PartySlot6 != null && player2.PartySlot6.Fainted))))
                    {
                        if (Text($"All oponent pokemons was fainted. [PH] Your {player1.ActivePet?.PokemonName} received 50xp", true))
                        {
                            player1.ActivePet.Exp += 50;
                            var pac = new PlayerSidebarSync();
                            pac.Send(TerramonMod.Instance, player1);
                            //send sync packet
                        }
                        if(Text($"You received 10s for winning", true))
                        {
                            //do reward stuff;
                            new BattleEndPacket().Send(TerramonMod.Instance);

                        }
                    }

                    //Text("PvP not supported rn");
                    //State = BattleState.None;
                    break;
            }

        }
        
        public virtual void EndBattle()
        {
            // poof wild pokemon away in dust
            if (State == BattleState.BattleWithWild)
            {
                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(WildNPC.projectile.position, WildNPC.projectile.width, WildNPC.projectile.height, ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
                }
            }

            // reset camera and game zoom
            TerramonMod.ZoomAnimator.GameZoom(1f, 500, Easing.Out);

            // end battle, reset static variables
            doneWildIntroAppear = false;
            doneWildIntro = false;
            wildTimer = 0;
            BattleUI.doneWildIntro = false;
            UI.ButtonMenuPanel.Remove();
            UI.splashText.SetText("");
            UI.MovesPanel.Top.Set(500, 1f);
            UI.HP1.firstLoadHP = true;
            UI.HP2.firstLoadHP = true;
            UI.HP1.Left.Set(20, 1f);
            UI.HP2.Left.Set(30000, 0f);
            UI.HP1.HPBar.lowHPSoundInstance?.Stop();
            UI.Append(UI.whiteFlash);

            ModContent.GetInstance<TerramonMod>().battleCamera = Vector2.Zero;

            // reset modifiers
            if (player1.ActivePet.CustomData.ContainsKey("PhysDefModifier")) player1.ActivePet.CustomData.Remove("PhysDefModifier");
            if (player1.ActivePet.CustomData.ContainsKey("SpDefModifier")) player1.ActivePet.CustomData.Remove("SpDefModifier");
            if (player1.ActivePet.CustomData.ContainsKey("SpeedModifier")) player1.ActivePet.CustomData.Remove("SpeedModifier");
            if (player1.ActivePet.CustomData.ContainsKey("CritRatioModifier")) player1.ActivePet.CustomData.Remove("CritRatioModifier");

            State = BattleState.None;
        }

        private bool faintedPrinted;

        //This print's text only for players what actually in battle.
        //If text get printed return true
        private bool Text(string text, bool localOnly =false)
        {
            if (Main.LocalPlayer == player1?.player || Main.LocalPlayer == player2?.player)
            {
                if(!localOnly)
                    Main.NewText(text);
                else
                {
                    if (player1?.player == Main.LocalPlayer)
                    {
                        Main.NewText(text);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private bool Text(string text, Color color, bool localOnly = false)
        {
            if (Main.LocalPlayer == player1?.player || Main.LocalPlayer == player2?.player)
            {
                if (!localOnly)
                    Main.NewText(text);
                else
                {
                    if (player1?.player == Main.LocalPlayer)
                    {
                        Main.NewText(text, color);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        internal void SyncMove(string MoveName, bool p1 = true)
        {
            if (p1)
            {
                foreach (BaseMove it in player1.ActivePet.Moves)
                {
                    if (it.GetType().Name == MoveName)
                    {
                        pMove = it;
                        break;
                    }
                }
            }
            else
            {
                foreach (BaseMove it in player2.ActivePet.Moves)
                {
                    if (it.GetType().Name == MoveName)
                    {
                        oMove = it;
                        break;
                    }
                }

            }
        }



        public void HandleChange()
        {
            switch (State)
            {
                case BattleState.BattleWithPlayer:
                    pokeName2 = TerramonMod.Localisation.GetLocalisedString(player2?.ActivePetName);
                    pokeName1 = TerramonMod.Localisation.GetLocalisedString(player1?.ActivePetName);
                    UI.HP1.PokeData = player1.ActivePet;
                    UI.HP2.PokeData = player2?.ActivePet;
                    UI.MovesPanel.PokeData = player1.ActivePet;
                    playerChallenge.Args = new object[] { player2?.player.name };
                    Text(playerChallenge.Value, true);
                    break;
                case BattleState.BattleWithWild:
                    pokeName1 = TerramonMod.Localisation.GetLocalisedString(player1?.ActivePetName);
                    UI.HP1.PokeData = player1.ActivePet;
                    UI.MovesPanel.PokeData = player1.ActivePet;
                    break;
            }
        }

        public BaseMove AIOverride(ParentPokemon mon)
        {
            switch (animMode)
            {
                case 1 when pMove != null && animWindow > 0 && (ParentPokemon)(Main.projectile[player1.ActivePetId].modProjectile) == mon:
                    return pMove;
                case 2 when oMove != null && animWindow > 0:
                    switch (State)
                    {
                        case BattleState.BattleWithTrainer:
                        case BattleState.BattleWithWild:
                            if (WildNPC == mon)
                                return oMove;
                            break;
                        case BattleState.BattleWithPlayer:
                            return player2?.ActiveMove;
                        case BattleState.None:
                        default:
                            return null;
                    
                    }
                    break;
            }
            return null;
        }

        public void Cleanup()
        {
            if (Main.netMode != NetmodeID.Server)
                BattleUI.Visible = false;

            if (WildNPC != null)
            {
                WildNPC.Wild = false;
                WildNPC.projectile.timeLeft = 0;
                WildNPC.projectile.Kill();
            }

        }

        public BattleSyncData GetSyncData()
        {
            switch (State)
            {
                case BattleState.BattleWithPlayer:
                    return new BattleSyncData()
                    {
                        state = State,
                        pl1 = player1.player.whoAmI,
                        pl2 = player2.player.whoAmI,
                        wild = null,
                        wildID = -1
                    };
                case BattleState.BattleWithWild:
                    return new BattleSyncData()
                    {
                        state = State,
                        pl1 = player1.player.whoAmI,
                        pl2 = -1,
                        wild = Wild,
                        wildID = wildID
                    };
            }

            return new BattleSyncData()
            {
                state = BattleState.None
            }; ;
        }

        public struct BattleSyncData
        {
            public BattleState state;
            public int pl1;
            public PokemonData wild;
            public int pl2;
            public int wildID;
        }

        /// <summary>
        /// This method will output the zoom matrix needed to be passed into the battle intro shader.
        /// </summary>
        /// <param name="zoom">The extent of the zoom. 1 = no zoom, 2 = 2x, etc.</param>
        /// <param name="focus">The screen-space coordinates representing the focus of the zoom (the zoom will zoom in on this point).</param>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        /// <returns>The matrix.</returns>
        private Matrix CreateZoomMatrix(float zoom, Vector2 focus, int screenWidth, int screenHeight)
        {
            Vector2 zoomFocus = new Vector2(focus.X / screenWidth, focus.Y / screenHeight) * zoom;

            // Linear algebra. In order to create the zoom we need to first translate to the focus point, scale around that, and then translate back.
            // Matrix multiplication is not commutative so order matters. In order for the transformation to be correct the matrices must be multiplied in this order.
            // At the end, we invert the matrix because we want the action that leads to the zoom, not the zoomed state.
            return Matrix.Invert(Matrix.CreateTranslation(-zoomFocus.X, -zoomFocus.Y, 0) * Matrix.CreateScale(zoom) * Matrix.CreateTranslation(zoomFocus.X, zoomFocus.Y, 0));
        }

        private void SetParameters(float maskExtent, float blurIntensity, Matrix zoomMatrix)
        {
            Effect shader = Filters.Scene["BattleIntro"].GetShader().Shader;

            shader.Parameters["maskExtent"].SetValue(maskExtent);
            shader.Parameters["blurStrength"].SetValue(blurIntensity);
            shader.Parameters["zoomMatrix"].SetValue(zoomMatrix);
        }

    }

    public class BattleUI : UIState
    {
        public UIImagez whiteFlash;
        public UITypewriterText splashText;
        public UIText tipText;
        public ButtonMenuPanel ButtonMenuPanel;
        public MovesPanel MovesPanel;
        public HPPanel HP1, HP2;
        public bool Turn;
        public Action<BaseMove> MovePresed;
        public static bool Visible = false;

        public static bool doneWildIntro = false;

        public Color FLASH_COLOR = Color.White;
        public float FLASH_VISIBILITY = 0f;

        public override void OnInitialize()
        {
            // Splash text to replace Main.NewText() calls
            splashText = new UITypewriterText("", 1.1f);
            splashText.HAlign = 0.5f;
            splashText.Top.Set(-246, 1f);
            Append(splashText);

            whiteFlash = new UIImagez(ModContent.GetTexture("Terramon/UI/IntroMovie/WhiteFlash"));
            whiteFlash.HAlign = 0f;
            whiteFlash.VAlign = 0f;
            whiteFlash.ImageScale *= 5;
            whiteFlash._visibilityActive = 0f;

            tipText = new UIText("", 1.25f);
            tipText.TextColor = new Color(189, 189, 189);
            tipText.VAlign = 0.25f;
            tipText.HAlign = 0.5f;
            Append(tipText);

            HP1 = new HPPanel(true);
            HP1.Left.Set(20, 1f); //HP1.Left.Set(-340, 1f); 
            HP1.Top.Set(0, 0.6f);
            Append(HP1);

            HP2 = new HPPanel(false);
            HP2.Left.Set(30000, 0f); //HP2.Left.Set(160, 0f);
            HP2.Top.Set(0, 0.4f);
            Append(HP2);

            Append(whiteFlash);

            MovesPanel = new MovesPanel()
            {
                OnMoveClick = (x) =>
                {
                    if (Turn)
                    {
                        Turn = false;
                        MovePresed?.Invoke(x);
                    }
                }
            };
            Append(MovesPanel);

            base.OnInitialize();
        }

        public void ResetData()
        {
            HP1.PokeData = null;
            HP2.PokeData = null;
            MovesPanel.PokeData = null;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!Visible)
                return;
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            whiteFlash._visibilityActive = FLASH_VISIBILITY;

            if (BattleMode.doneWildIntro && !doneWildIntro)
            {
                var player1 = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();

                if (player1.firstBattle)
                {
                    //tipText.SetText("Use WASD to move the camera around.");
                    player1.firstBattle = false;
                }

                whiteFlash.Remove();

                splashText.SetText($"What will {player1.ActivePet.PokemonName} do?");

                // pan camera to local player pokemon
                Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/uislide").WithVolume(.6f));
                TerramonMod.ZoomAnimator.ScreenPosX(Main.projectile[player1.ActivePetId].modProjectile.projectile.position.X, 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(Main.projectile[player1.ActivePetId].modProjectile.projectile.position.Y, 500, Easing.OutExpo);

                ButtonMenuPanel = new ButtonMenuPanel();
                ButtonMenuPanel.OnInitialize();
                Append(ButtonMenuPanel);

                doneWildIntro = true;
            }

            base.Update(gameTime);
        }
    }

    public class ButtonMenuPanel : UIPanel
    {
        private BattlingMenuButton FightButton;
        private BattlingMenuButton BagButton;
        private BattlingMenuButton PokemonButton;
        private BattlingMenuButton RunButton;

        private UIText FightText;
        private UIText BagText;
        private UIText PokemonText;
        private UIText RunText;
        public override void OnInitialize()
        {
            this.Width.Set(450, 0f);
            this.Height.Set(200, 0f);
            this.Top.Set(500, 1f);
            this.Left.Set(-225, 0.5f);
            this.BackgroundColor = Color.White * 0f;
            this.BorderColor = Color.White * 0f;

            FightButton = new BattlingMenuButton(ModContent.GetTexture("Terramon/UI/Battling/FightButton"), ModContent.GetTexture("Terramon/UI/Battling/FightButton_Hover"));
            FightButton.OnClick += fight;
            Append(FightButton);

            FightText = new UIText("Fight", 0.6f, true);
            FightText.HAlign = 0.5f;
            FightText.VAlign = 0.5f;
            FightButton.Append(FightText);

            BagButton = new BattlingMenuButton(ModContent.GetTexture("Terramon/UI/Battling/BagButton"), ModContent.GetTexture("Terramon/UI/Battling/BagButton_Hover"));
            BagButton.Left.Set(218, 0f);
            Append(BagButton);

            BagText = new UIText("Bag", 0.6f, true);
            BagText.HAlign = 0.5f;
            BagText.VAlign = 0.5f;
            BagButton.Append(BagText);

            PokemonButton = new BattlingMenuButton(ModContent.GetTexture("Terramon/UI/Battling/PokemonButton"), ModContent.GetTexture("Terramon/UI/Battling/PokemonButton_Hover"));
            PokemonButton.Top.Set(76, 0f);
            Append(PokemonButton);

            PokemonText = new UIText("Pokémon", 0.6f, true);
            PokemonText.HAlign = 0.5f;
            PokemonText.VAlign = 0.5f;
            PokemonButton.Append(PokemonText);

            RunButton = new BattlingMenuButton(ModContent.GetTexture("Terramon/UI/Battling/RunButton"), ModContent.GetTexture("Terramon/UI/Battling/RunButton_Hover"));
            RunButton.Top.Set(76, 0f);
            RunButton.Left.Set(218, 0f);
            RunButton.OnClick += runAway;
            Append(RunButton);

            RunText = new UIText("Run", 0.6f, true);
            RunText.HAlign = 0.5f;
            RunText.VAlign = 0.5f;
            RunButton.Append(RunText);
        }

        public bool viewable = false;
        public override void Update(GameTime gameTime)
        {
            var player1 = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            if (BattleMode.inMainMenu && BattleMode.UI.Turn && BattleMode.doneWildIntro && BattleMode.UI.splashText._textTarget != $"What will {player1.ActivePet.PokemonName} do?")
            {
                BattleMode.UI.splashText.SetText($"What will {player1.ActivePet.PokemonName} do?");
            }
            if (BattleMode.inMainMenu && BattleMode.UI.Turn && BattleMode.doneWildIntro)
            {
                if (!viewable)
                {
                    TerramonMod.ZoomAnimator.ScreenPosX((Main.projectile[player1.ActivePetId].modProjectile.projectile.position.X + 12), 500, Easing.OutExpo);
                    TerramonMod.ZoomAnimator.ScreenPosY((Main.projectile[player1.ActivePetId].modProjectile.projectile.position.Y), 500, Easing.OutExpo);
                    Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/uislide").WithVolume(.6f));
                    TerramonMod.ZoomAnimator.ButtonMenuPanelX(-220, 500, Easing.OutExpo);
                }
                viewable = true;
            }
            else
            {
                viewable = false;
                Top.Set(500, 1f);
            }

            Recalculate();
            base.Update(gameTime);
        }
        private void fight(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/uiselect").WithVolume(.55f));
            BattleMode.inMainMenu = false;
        }

        private void runAway(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/fleeswitch").WithVolume(.55f));
            BattleMode.queueRunAway = true;
        }
    }

    public class HPPanel : UIPanel
    {
        private UIText LevelText, HPText, EXPText, HPLocal, PokeName;
        private UIImage HP, HPBack, Party, PartyExh, Fart;
        
        public BattleHPBar HPBar;
        public BattleEXPBar EXPBar;

        private ILocalisedBindableString LocPokemon;

        private PokemonData pokeData;

        private int hpScaleTarget = 0;
        public int displayHpNumber = 0;
        public int displayHpNumberLerp = 0;
        public float lastDisplayHP = 0;
        public float displayHp = 0;

        public float fillval = 1f;

        public bool firstLoadHP = true;
        public bool adjusting = true;

        public bool local = true;

        //private Texture HPTexture, HPBackTexture, PartyTexture, PartyExhausted;
        public PokemonData PokeData
        {
            get => pokeData;
            set
            {
                if(value == pokeData)
                    return;

                pokeData = value;

                if (firstLoadHP)
                {
                    if (pokeData != null)
                    {
                        displayHpNumber = pokeData.HP;
                        displayHpNumberLerp = pokeData.HP;
                        HPBar.fill = pokeData.HP;
                        firstLoadHP = false;
                    }
                }

                if (pokeData != null)
                {
                    HPBar.fill = (float)pokeData?.HP / (float)pokeData?.MaxHP;
                    if (local)
                    {
                        if (EXPBar.fill > 1f) EXPBar.fill = 1f;
                        else EXPBar.fill = (float)pokeData?.Exp / (float)pokeData?.ExpToNext;
                        EXPBar.HoverText = $"{pokeData?.Exp}/{pokeData?.ExpToNext}";
                    }
                }

                LocPokemon = TerramonMod.Localisation.GetLocalisedString(pokeData?.Pokemon ?? "MissingNO");
                PokeName?.SetText(LocPokemon.Value);
            }
        }
        //public int PartySize;

        public Vector2 Position
        {
            get => new Vector2(Left.Percent, Top.Percent);
            set
            {
                Left.Set(0, value.X);
                Top.Set(-55, value.Y);
            }
        }

        public HPPanel(bool l)
        {
            local = l;
        }

        public bool AdjustingHP()
        {
            return false; // later
        }

        public override void OnInitialize()
        {
            this.BackgroundColor = new Color(76, 78, 79);
            this.Width.Set(178, 0f);
            if (local)
            {
                this.Height.Set(84, 0f);
            } else
            {
                this.Height.Set(75, 0f);
            }

            PokeName = new UIText(LocPokemon?.Value ?? string.Empty);
            PokeName.Left.Set(5, 0f);
            PokeName.Top.Set(5, 0f);
            Append(PokeName);

            HPText = new UIText("HP", 0.6f, false);
            HPText.Top.Set(32, 0f);
            HPText.Left.Set(5, 0f);
            Append(HPText);

	        EXPText = new UIText("EXP", 0.6f, false);
            EXPText.Top.Set(50, 0f);
            EXPText.Left.Set(5, 0f);
            if (local) Append(EXPText);

            if (local)
            {
                HPLocal = new UIText("", 0.9f, false);
                HPLocal.Top.Set(52, 0f);
                HPLocal.Left.Set(5, 0f);
                //Append(HPLocal);

                EXPBar = new BattleEXPBar();
                EXPBar.Top.Set(50, 0f);
                EXPBar.Left.Set(-119, 1f);
		        EXPBar.Height.Set(8, 0f);
		        EXPBar.Width.Set(110, 0f);
                Append(EXPBar);
            }

            if (local) HPBar = new BattleHPBar(Color.LightGreen, true);
            else HPBar = new BattleHPBar(Color.LightGreen, false);
            HPBar.Top.Set(30, 0f);
            HPBar.Left.Set(25, 0f);
            HPBar.Height.Set(14, 0f);
            HPBar.Width.Set(124, 0f);
            Append(HPBar);

            LevelText = new UIText("Lv 1");
            LevelText.HAlign = 0.92f;
            LevelText.Top.Set(5, 0f);
            Append(LevelText);

            if (pokeData != null)
            {
                displayHp = pokeData.HP;
                HPBar.fill = (float)pokeData.HP/pokeData.MaxHP;
                hpScaleTarget = pokeData.HP;
            }

            base.OnInitialize();
        }

        public double hpLerpTimer;
        public override void Update(GameTime gameTime)
        {
            hpLerpTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            LocPokemon = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(pokeData?.Pokemon));
            PokeName.SetText(LocPokemon.Value);

            if (local)
            {
                HPLocal?.SetText($"{displayHpNumberLerp}/{pokeData?.MaxHP ?? 0}");
                EXPBar.HoverText = $"{pokeData?.Exp}/{pokeData?.ExpToNext}";
                HPBar.HoverText = $"{displayHpNumberLerp}/{pokeData?.MaxHP}";
            } else
            {
                HPBar.HoverText = $"{displayHpNumberLerp}/{pokeData?.MaxHP}";
            }

            // if display and actual hp are not equal...
            if (displayHpNumber != pokeData?.HP)
            {
                // ... we need to update the hp bar fill + display number.
                if (local)
                {
                    TerramonMod.ZoomAnimator.HPBar1Fill((float)pokeData.HP / pokeData.MaxHP, 400, Easing.None);
                    TerramonMod.ZoomAnimator.HPBar1DisplayNumber(pokeData.HP, 400, Easing.None);
                } else
                {
                    TerramonMod.ZoomAnimator.HPBar2Fill((float)pokeData.HP / pokeData.MaxHP, 400, Easing.None);
                    TerramonMod.ZoomAnimator.HPBar2DisplayNumber(pokeData.HP, 400, Easing.None);
                }
            }

            // it get set right after anyway to ensure the Above IF() does not get called more than once
            if (pokeData?.HP != null) displayHpNumber = pokeData.HP;

            LevelText.SetText($"Lv {pokeData?.Level ?? 1}");

            base.Update(gameTime);
        }
    }

    public class MovesPanel : UIPanel
    {
        private BattleMoveButton Move1, Move2, Move3, Move4;

        public PokemonData PokeData
        {
            get => pokeData;
            set
            {
                if(pokeData == value)
                    return;

                needUpdate = true;
                pokeData = value;
            }
        }
        private bool needUpdate;
        public static bool Visible;
        public Action<BaseMove> OnMoveClick;
        private PokemonData pokeData;

        public MovesPanel()
        {
        }

        public override void OnInitialize()
        {
            this.Width.Set(450, 0f);
            this.Height.Set(200, 0f);
            this.Top.Set(500, 1f);
            this.Left.Set(-225, 0.5f);
            this.BackgroundColor = Color.White * 0f;
            this.BorderColor = Color.White * 0f;
            var size = new Vector2(170, 40);

            Move1 = new BattleMoveButton(PokeData?.Moves[0], new Vector2(0, 0))
            {
                OnClick = (x) => OnMoveClick?.Invoke(x),
            };
            Append(Move1);
            Move2 = new BattleMoveButton(PokeData?.Moves[1], new Vector2(218, 0))
            {
                OnClick = (x) => OnMoveClick?.Invoke(x),
            };
            Append(Move2);
            Move3 = new BattleMoveButton(PokeData?.Moves[2], new Vector2(0, 76))
            {
                OnClick = (x) => OnMoveClick?.Invoke(x),
            };
            Append(Move3);
            Move4 = new BattleMoveButton(PokeData?.Moves[3], new Vector2(218, 76))
            {
                OnClick = (x) => OnMoveClick?.Invoke(x),
            };
            Append(Move4);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (needUpdate)
            {
                Move1.Move = pokeData.Moves[0];
                Move2.Move = pokeData.Moves[1];
                Move3.Move = pokeData.Moves[2];
                Move4.Move = pokeData.Moves[3];
            }

            if (!BattleMode.inMainMenu) {
                if (BattleMode.UI.Turn)
                {
                    Top.Set(-220, 1f);
                } else
                {
                    Top.Set(500, 1f);
                }
            } else
            {
                Top.Set(500, 1f);
            }

            base.Update(gameTime);
        }
    }

    public enum BattleState
    {
        None = 0,
        BattleWithWild,
        BattleWithTrainer,
        BattleWithPlayer,//Should use networking
    }
    public enum BattleStyle
    {
        Default = 0,
        VsWildLegendaryBirds,
        VsWildMewtwo,
        VsWildMew
    }
}
