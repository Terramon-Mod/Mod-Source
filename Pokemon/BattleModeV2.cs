using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Extensions.ValidationExtensions;
using Terraria;
using Terramon.Items.Pokeballs.Parts;
using Terramon.Network.BattlingV2;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terramon.UI.Battling.v2;
using Terraria.ID;

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

            //ui push
            if (Main.netMode != NetmodeID.Server)
            {

            }

        }



        public void Update()
        {
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
                            }
                        }
                    }
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
                                State = BattleState.Ended;
                            }
                        }
                    }
                }
            }

            if (State == BattleState.InProggress)
            {

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
                }

            }


            prevState = State;
        }


        private void Update_Animation()
        {
            if (ExecutingMove != null)
            {
                var target = MoveCaster == p1 ? p2 : p1;
                if (FrameCount == 0)// At initial frame
                {
                    if (!ExecutingMove.PerformInBattle(MoveCaster, target, this))
                    {
                        NextMove();
                    }
                    else
                    {
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
                    }
                }
            }
            else
            {
                State = BattleState.InProggress;//safety switch
            }
        }


        private bool NextMove()
        {
            if (MovesQuive.Count > 0)
            {
                var kvp = MovesQuive.Dequeue();//Pop one move from queue
                kvp_ExecutingMove = kvp;//And set it as next
                return true;
            }
            else
            {
                kvp_ExecutingMove = kvp_Null;
                State = BattleState.InProggress;
                return false;
            }
        }


        /// <summary>
        /// Automatically starts animation
        /// </summary>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        public void DealDamage(BattleOpponent target, int damage)
        {
            target.PokeData.Damage(damage);
            //TODO UI animation

        }

        public void Cleanup()
        {

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

        public enum BattleState
        {
            InProggress,
            Animating,
            Fainted,
            Ended,
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
