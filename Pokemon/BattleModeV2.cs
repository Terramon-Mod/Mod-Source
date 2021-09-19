using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Razorwing.Framework.Graphics;
using Terramon.Extensions.ValidationExtensions;
using Terraria;
using Terramon.Items.Pokeballs.Parts;
using Terramon.Network.BattlingV2;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terramon.UI;
using Terramon.UI.Battling.v2;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Terramon.Pokemon
{
    public class BattleModeV2
    {
        /// <summary>
        /// Wild, trainers and players now merged in to one abstract class
        /// what contains all necessary information 
        /// </summary>
        private BattleOpponent p1, p2;
        private Queue<KeyValuePair<BattleOpponent, BaseMove>> MovesQuive = new Queue<KeyValuePair<BattleOpponent, BaseMove>>();
        private Dictionary<BattleOpponent, BaseMove> SelectedMoves = new Dictionary<BattleOpponent, BaseMove>();

        private static KeyValuePair<BattleOpponent, BaseMove> kvp_Null { get; }
            = new KeyValuePair<BattleOpponent, BaseMove>(null, null);

        private KeyValuePair<BattleOpponent, BaseMove> kvp_ExecutingMove = kvp_Null;

        public BaseMove ExecutingMove => kvp_ExecutingMove.Value;
        public BattleOpponent MoveCaster => kvp_ExecutingMove.Key;

        public BattleV2UI UI;

        public int FrameCount { get; set; }
        public int UIAnimDelay { get; set; }

        /// <summary>
        /// Allow pause moves animation frame increase.
        /// Autopause if UI make own animations
        /// </summary>
        public bool PauseFrame
        {
            get => pauseFrame ? pauseFrame : UIAnimDelay > 0;
            set => pauseFrame = value;
        }

        public static BattleV2UI ui = new BattleV2UI();
        public BattleState State = BattleState.InProggress;
        private BattleState prevState = BattleState.InProggress;
        private bool pauseFrame;

        public BattleModeV2(BattleOpponent first, BattleOpponent second, bool lazy = false,
            BattleStyle bs = BattleStyle.Default)
        {
            p1 = first;
            p2 = second;

            State = BattleState.Intro;

            //ui push
            if (Main.netMode != NetmodeID.Server &&
                first is BattlePlayerOpponent pl)
            {
                if (pl.Player.player == Main.LocalPlayer)
                {
                    UI = TerramonMod.Instance.V2Battle;
                    UI.Player = pl;
                    UI.Enemy = p2;
                    UI.Battle = this;
                    BattleV2UI.Visible = true;
                    UI.State = BattleUIState.Intro;
                    UI.SetupPokeData();
                }
            }


        }

        public void Update()
        {
            if(State == BattleState.Intro)
                Intro_Update();

            //Intro
            //UI.State = BattleUIState.PostIntro;
            if (UI?.State == BattleUIState.PostIntro)
            {
                UI.State = BattleUIState.MainMenu;
                State = BattleState.InProggress;
                if(UI!=null)
                    SplashText($"What {p1.PokeData.PokemonName} should do?");
            }

            if (UIAnimDelay > 0)
                UIAnimDelay--;


            if (State == BattleState.Animating)
            {
                Update_Animation();
            }

            if (State == BattleState.Fainted)
            {
                if (p1.Fainted)
                {
                    if (p1 is BattlePlayerOpponent p)
                    {
                        if (!p.NetPlayer)
                        {
                            if (State != prevState)
                            {
                                //UI warn and switch
                                State = BattleState.Ended;
                                return;
                            }
                        }
                    }
                    State = BattleState.Ended;
                    return;
                }
                if (p2.Fainted)
                {
                    if (p2 is BattlePlayerOpponent p)
                    {
                        if (!p.NetPlayer)
                        {
                            if (State != prevState)
                            {
                                //UI warn and switch
                                //State = BattleState.Ended;
                                State = BattleState.Ended;
                                return;
                            }
                        }
                    }else
                    if (p2 is BattleWildOpponent wild)
                    {
                        var exp = p1.PokeData.GiveEXP(p1.PokeData, p2.PokeData, Pokemon.BattleState.BattleWithWild, 1);
                        Text(
                            $"{wild.PokeData.PokemonName} was fainted. [PH] Your {p1.PokeData.PokemonName} received {exp} exp");
                        p1.PokeData.Exp += exp;
                        State = BattleState.Ended;

                    }
                    else
                        State = BattleState.Ended;
                    return;
                }
            }

            if (State == BattleState.InProggress)
            {
                if (UI != null)
                {
                    if (UI.State == BattleUIState.Animating)
                        UI.State = BattleUIState.MainMenu;
                    if (prevState == BattleState.Animating)
                    {
                        SplashText($"What {p1.PokeData.PokemonName} should do?");
                    }
                }

                if (!SelectedMoves.ContainsKey(p1))
                {
                    var move = p1.SelectMove(p2);
                    if (move != null)
                    {
                        SelectedMoves.Add(p1, move);
                    }
                }
                if (!SelectedMoves.ContainsKey(p2))
                {
                    var move = p2.SelectMove(p1);
                    if (move != null)
                    {
                        SelectedMoves.Add(p2, move);
                    }
                }

                if (SelectedMoves.Count > 2)
                {
                    throw new IndexOutOfRangeException($"{nameof(SelectedMoves)} contains move what 2 moves!");
                }
                if(SelectedMoves.Count == 2)
                {
                    if (p1.PokeData.Speed > p2.PokeData.Speed)
                    {
                        MovesQuive.Enqueue(new KeyValuePair<BattleOpponent, BaseMove>(p1, SelectedMoves[p1]));
                        MovesQuive.Enqueue(new KeyValuePair<BattleOpponent, BaseMove>(p2, SelectedMoves[p2]));
                    }else if(p1.PokeData.Speed == p2.PokeData.Speed)
                    {
                        if (BaseMove._mrand.Next(2) == 0)
                        {
                            MovesQuive.Enqueue(new KeyValuePair<BattleOpponent, BaseMove>(p1, SelectedMoves[p1]));
                            MovesQuive.Enqueue(new KeyValuePair<BattleOpponent, BaseMove>(p2, SelectedMoves[p2]));
                        }
                        else
                        {
                            MovesQuive.Enqueue(new KeyValuePair<BattleOpponent, BaseMove>(p2, SelectedMoves[p2]));
                            MovesQuive.Enqueue(new KeyValuePair<BattleOpponent, BaseMove>(p1, SelectedMoves[p1]));
                        }
                    }
                    else
                    {
                        MovesQuive.Enqueue(new KeyValuePair<BattleOpponent, BaseMove>(p2, SelectedMoves[p2]));
                        MovesQuive.Enqueue(new KeyValuePair<BattleOpponent, BaseMove>(p1, SelectedMoves[p1]));
                    }

                    State = BattleState.Animating;
                    if (UI != null)
                        UI.State = BattleUIState.Animating;
                    SelectedMoves.Clear();
                }

            }

            if (State == BattleState.Ended)
            {
                EndBattle();
            }


            prevState = State;
        }

        private bool Text(string text, bool localOnly = false)
        {
            if ((p1 is BattlePlayerOpponent pl && !pl.NetPlayer) || (p2 is BattlePlayerOpponent pl2 && !pl2.NetPlayer))
            {
                if (!localOnly)
                    Main.NewText(text);
                else
                {
                    if (p1 is BattlePlayerOpponent p && !p.NetPlayer)
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
            if ((p1 is BattlePlayerOpponent pl && !pl.NetPlayer) || (p2 is BattlePlayerOpponent pl2 && !pl2.NetPlayer))
            {
                if (!localOnly)
                    Main.NewText(text, color);
                else
                {
                    if (p1 is BattlePlayerOpponent p && !p.NetPlayer)
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

        private void Update_Animation()
        {
            if (ExecutingMove != null)
            {
                var target = MoveCaster == p1 ? p2 : p1;
                if (FrameCount == -1)// At initial frame
                {
                    if (!ExecutingMove.PerformInBattle(MoveCaster, target, this))
                    {
                        NextMove();
                    }
                    else
                    {
                        SplashText($"{MoveCaster.PokeData.PokemonName} uses {ExecutingMove?.MoveName}");
                        FrameCount = 0;//Reset frame counter
                    }
                }

                if (ExecutingMove?.AnimateTurn(MoveCaster, target, this, FrameCount) ?? false)
                {
                    if (!PauseFrame)
                        FrameCount++;
                }
                else
                {
                    if(!target.Fainted)
                        NextMove();
                    else
                    {
                        NextMove();
                        kvp_ExecutingMove = kvp_Null;
                        State = BattleState.Fainted;
                        if(UI != null)
                            UI.State = BattleUIState.MainMenu;
                    }
                }
            }
            else
            {
                if (MovesQuive.Count == 0)
                {
                    State = BattleState.InProggress;//safety switch
                    if (UI != null)
                        UI.State = BattleUIState.MainMenu;
                }
                else
                {
                    NextMove();
                    SplashText($"{MoveCaster.PokeData.PokemonName} uses {ExecutingMove?.MoveName}");
                }


            }
        }


        private bool NextMove()
        {
            if (MovesQuive.Count > 0)
            {
                var kvp = MovesQuive.Dequeue();//Pop one move from queue
                kvp_ExecutingMove = kvp;//And set it as next
                FrameCount = -1;
                return true;
            }
            else
            {
                kvp_ExecutingMove = kvp_Null;
                State = BattleState.InProggress;
                return false;
            }
        }

        protected void EndBattle()
        {
            // poof wild pokemon away in dust
            if (p2 is BattleWildOpponent)
            {
                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(p2.PokeProj.projectile.position, p2.PokeProj.projectile.width, p2.PokeProj.projectile.height, ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
                }

                p2.PokeProj.projectile.active = false;
            }
            if (p1 is BattleWildOpponent)
            {
                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(p1.PokeProj.projectile.position, p1.PokeProj.projectile.width, p1.PokeProj.projectile.height, ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
                }

                p1.PokeProj.projectile.active = false;
            }



            //This instance should be fully killed by GC, so no manipulation on this call needed
            // end battle, reset static variables
            //doneWildIntroAppear = false;
            //doneWildIntro = false;
            //wildTimer = 0;
            //BattleUI.doneWildIntro = false;
            if (UI != null)
            {
                // reset camera and game zoom
                TerramonMod.ZoomAnimator.GameZoom(1f, 500, Easing.Out);
                UI.ButtonMenuPanel.Remove();
                UI.splashText.SetText("");
                UI.MovesPanel.Top.Set(500, 1f);
                UI.HP1.firstLoadHP = true;
                UI.HP2.firstLoadHP = true;
                UI.HP1.Left.Set(20, 1f);
                UI.HP2.Left.Set(30000, 0f);
                UI.HP1.HPBar.lowHPSoundInstance?.Stop();
                UI.Append(UI.whiteFlash);
            }
            

            ModContent.GetInstance<TerramonMod>().battleCamera = Vector2.Zero;

            // reset modifiers
            if (p1.PokeData.CustomData.ContainsKey("PhysDefModifier")) p1.PokeData.CustomData.Remove("PhysDefModifier");
            if (p1.PokeData.CustomData.ContainsKey("SpDefModifier")) p1.PokeData.CustomData.Remove("SpDefModifier");
            if (p1.PokeData.CustomData.ContainsKey("SpeedModifier")) p1.PokeData.CustomData.Remove("SpeedModifier");
            if (p1.PokeData.CustomData.ContainsKey("CritRatioModifier")) p1.PokeData.CustomData.Remove("CritRatioModifier");

            State = BattleState.None;
        }


        /// <summary>
        /// Automatically starts animation
        /// </summary>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        public void DealDamage(BattleOpponent target, int damage)
        {
            //target.PokeData.Damage(damage);
            //TODO UI animation
            SplashText($"{ExecutingMove.MoveName} hit with {damage} damage");
        }

        public void SplashText(string text)
        {
            UI?.splashText.SetText(text);
            UIAnimDelay = text.Length + 10;
        }

        public void Cleanup()
        {
            ui = null;
        }

        #region Networking

        public static void RequestWildBattle(TerramonPlayer me, NPC wild)
        {
            switch (Main.netMode)
            {
                case NetmodeID.SinglePlayer:
                    throw new InvalidOperationException("Networking is not available in singleplayer!");
                case NetmodeID.Server:
                    throw new InvalidOperationException("Client function executed in serverside!");
            }

            if (!me.Validate().NotInBattle()
                .HasNotFaintedPokemons()
                .HasActivePetProjectile().Result())
            {

            }

            new StartBattleWithWildPacket()
                .Send(me.player.whoAmI, wild.whoAmI);

        }

        #endregion

        #region Intro

        protected void Intro_Update()
        {
            if (introMusicTimer < 190)
                intro_Music();
            else if (wildTimer < 515)
            {
                intro_Wild();
            }
            else
            {
                if(UI != null)
                    UI.State = BattleUIState.PostIntro;
                State = BattleState.InProggress;
            }
        }


        private int forceDirection;

        private void intro_Wild()
        {
            wildTimer++;

            if (p1 is BattlePlayerOpponent pl)//Execute only for local player  
            {
                if(pl.NetPlayer)
                    return;
            }else
                return;

            if (wildTimer == 370)
            {
                TerramonMod.ZoomAnimator.ScreenPosX(pl.Player.player.position.X + 10, 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(pl.Player.player.position.Y, 500, Easing.OutExpo);
                // Set splash text
                UI.splashText.SetText($"Go! {pl.PokeData.PokemonName}!");

                forceDirection = pl.PokeProj.projectile.position.X > pl.Player.player.position.X ? 1 : -1;

            }

            if (wildTimer == 430)
            {
                ParentPokemon playerpet = pl.PokeProj;
                playerpet.dootscale = 0.1f;
                playerpet.whiteFlashVal = 1f;
                playerpet.DontTpOnCollide = false;
            }

            if (wildTimer == 470)
            {
                UI.HP1.MoveToX(-340, 500, Easing.OutExpo);
                //TerramonMod.ZoomAnimator.HPBar1LeftPixels(-340, 500, Easing.OutExpo);
            }

            if (wildTimer >= 515)
            {
                forceDirection = 0;
            }

            // Make wild pokemon jump every so often
            //if (wildTimer >= 1300 && UI.Turn)
            //{
            //    wildTimer = 0;
            //    WildNPC.hopTimer = 0;
            //}
        }

        private int introMusicTimer, wildTimer = 369; //I love magic numbers, you know?

        private void intro_Music()
        {
            introMusicTimer++;

            if (p1 is BattlePlayerOpponent pl)//Execute only for local player  
            {
                if (pl.NetPlayer)
                    return;
            }
            else
                return;

            if (introMusicTimer > 13.832 * 60) // finished intro music
            {
                //playMainLoop = true;
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

            if (introMusicTimer == 42) TerramonMod.ZoomAnimator.GameZoom(5f, 1300, Easing.InExpo);

            if (introMusicTimer == 44)
            {
                TerramonMod.ZoomAnimator.WhiteFlashOpacity(1f, 1200, Easing.None);
            }

            if (introMusicTimer == 165)
            {
                ParentPokemon playerpet = p1.PokeProj;
                playerpet.SpawnTime = -252;
                playerpet.DontTpOnCollide = true;
                UI.HP2.Left.Set(160, 0f);
            }

            if (introMusicTimer == 170)
            {
                // Zoom in
                TerramonMod.ZoomAnimator.GameZoom(1f).GameZoom(1.7f, 0, Easing.None);
                // Pan camera to wild opponent, if this is BattleWithWild
                if (p2 is BattleWildOpponent)
                {
                    TerramonMod.ZoomAnimator.ScreenPosX(p2.PokeProj.projectile.position.X + 12, 0, Easing.None);
                    TerramonMod.ZoomAnimator.ScreenPosY(p2.PokeProj.projectile.position.Y, 0, Easing.None);
                }
                TerramonMod.ZoomAnimator.WhiteFlashOpacity(0f, 200, Easing.None);
            }

            if (introMusicTimer == 190)
            {
                Main.PlaySound(TerramonMod.Instance
                    .GetLegacySoundSlot(SoundType.Custom, "Sounds/Cries/cry" + p2.PokeData.Pokemon).WithVolume(0.55f));

                // Set splash text
                if(p2 is BattleWildOpponent)
                    UI.splashText.SetText($"A wild {p2.PokeData.PokemonName} appeared!");
            }
        }

        #endregion


        public enum BattleState
        {
            Intro,
            InProggress,
            Animating,
            Fainted,
            Ended,
            None,
        }
    }


    #region Interfaces

    public class BattlePlayerOpponent : BattleTrainerOpponent
    {
        public TerramonPlayer Player { get; }

        public bool NetPlayer { get; }

        public BaseMove ForwardedMove;
        
        public BattlePlayerOpponent(TerramonPlayer pl, bool netPlayer = false)
        {
            this.Player = pl;
            pokeData = pl.ActivePet;
            proj = (ParentPokemon)Main.projectile[pl.ActivePetId].modProjectile;
            NetPlayer = netPlayer;

            FillBall(pl.PartySlot1, 0);
            FillBall(pl.PartySlot2, 1);
            FillBall(pl.PartySlot3, 2);
            FillBall(pl.PartySlot4, 3);
            FillBall(pl.PartySlot5, 4);
            FillBall(pl.PartySlot6, 5);

        }

        public override BaseMove SelectMove(BattleOpponent enemy)
        {
            var fm = ForwardedMove;
            ForwardedMove = null;
            return fm;
        }

        private void FillBall(PokemonData data, int slot)
        {
            if (data.Validate().NotFainted().Result())
            {
                pokeballs[slot] = data.pokeballType;
            }
        }

        public override PokemonData SwitchPokemon()
        {
            return base.SwitchPokemon();
        }
    }

    public class BattleTrainerOpponent : BattleOpponent
    {

        public virtual PokemonData SwitchPokemon()
        {


            return null;
        }

        public override BaseMove SelectMove(BattleOpponent enemy)
        {
            return null;
        }
    }

    public class BattleWildOpponent : BattleOpponent
    {

        public BattleWildOpponent(ParentPokemon proj, PokemonData data)
        {
            this.proj = proj;
            this.pokeData = data;
        }

        public override BaseMove SelectMove(BattleOpponent enemy)
        {
#if DEBUG
            return TerramonMod.GetMove(nameof(ShootMove));
#endif
            if (PokeData.Validate().HasAvailableMoves().Result())
            {
                var list = PokeData.GetAvailableMoves();
                if (list.Count > 1)
                {
                    var s = BaseMove._mrand.Next(list.Count);
                    return list[s].Key;
                }
                else
                {
                    return list.First().Key;
                }
            }

            return TerramonMod.GetMove(nameof(Cut));
        }
    }

    public abstract class BattleOpponent : IDisposable
    {
        /// <summary>
        /// Represents pokeball types what opponent have (count and its type)
        /// Used for UI 
        /// </summary>
        public virtual byte[] Pokeballs => pokeballs;

        protected byte[] pokeballs = { (byte)TerramonMod.PokeballFactory.Pokebals.Pokeball, 0, 0, 0, 0, 0 };


        /// <summary>
        /// Represents pokemon projectile and used for animations
        /// </summary>
        public virtual ParentPokemon PokeProj => proj;
        protected ParentPokemon proj;

        /// <summary>
        /// Current active pokemon data. Used in logic
        /// </summary>
        public virtual PokemonData PokeData => pokeData;
        protected PokemonData pokeData;

        /// <summary>
        /// Pick a move for next battle turn.
        /// Will be called at next frame if returned null.
        /// Being called again on next turn
        /// </summary>
        /// <param name="enemy">Enemy data</param>
        /// <returns>Should return selected <see cref="BaseMove"/>
        /// or null if no decision is made</returns>
        public abstract BaseMove SelectMove(BattleOpponent enemy);

        public virtual bool Fainted => PokeData?.Fainted ?? true;
        public virtual bool CanSwitch => false;
        public virtual bool MoveSelected { get; set; }

        public virtual void Dispose()
        {
        }
    }


#endregion
}
