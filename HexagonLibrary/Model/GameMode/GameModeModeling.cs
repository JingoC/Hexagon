using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HexagonLibrary.Model.GameMode
{
    using HexagonLibrary.Entity.GameObjects;
    using HexagonLibrary.Entity.Players;

    using HexagonLibrary.Model.StateMachines;

    public class GameModeModeling : GameModeStrategy
    {
        GameNormalStateMachine stateMachine;

        public GameModeModeling() : this(new GameSettings())
        {

        }

        public GameModeModeling(GameSettings gameSettings) : base(gameSettings)
        {
            this.stateMachine = new GameNormalStateMachine() { Map = this.Map, GameState = TypeGameState.Attack };
        }

        public override void EndStep()
        {
            this.stateMachine.GameState = TypeGameState.Allocate;
        }

        public override void NextStep()
        {
            this.IsReady = false;

            this.stateMachine.GameState = TypeGameState.Wait;

            // multi threading
            this.threadActionCpu = new Thread(new ThreadStart(delegate ()
            {
                foreach (var item in this.CPUs)
                {
                    item.Strategy.Calculate(this.Map, item);
                }

                this.stateMachine.GameState = TypeGameState.Attack;

                this.IsReady = true;
                this.Step++;
            }));
            this.threadActionCpu.Start();
        }
    }
}
