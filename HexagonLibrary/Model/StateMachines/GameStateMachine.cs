﻿using HexagonLibrary.Entity.Players;
using HexagonLibrary.Model.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Model.StateMachines
{
    public enum TypeGameState
    {
        Attack = 0,
        Allocate = 1,
        Wait = 2,
        Last = 3
    }

    public class GameStateMachine
    {
        private TypeGameState gameState = TypeGameState.Attack;
        private bool enable = false;

        protected List<ClickObjectsStateMachine> stateMachines = new List<ClickObjectsStateMachine>();
        
        public bool Enable
        {
            get => this.enable;
            set {
                this.enable = value;
                this.stateMachines.ForEach(x => x.Enable = false);
                if (value)
                {
                    this.stateMachines[(int)this.gameState].Enable = true;
                }
            }
        }

        public Map Map
        {
            get => this.stateMachines[0].Map;
            set { this.stateMachines.ForEach(x => x.Map = value); }
        }
        public TypeGameState GameState
        {
            get { return this.gameState; }
            set
            {
                this.gameState = value;
                if (this.enable)
                {
                    this.Enable = true;
                }
            }
        }
        
        public void SetActivePlayer(Player p)
        {
            this.stateMachines.ForEach(x => x.SetActivePlayer(p));
        }

        public GameStateMachine()
        {
            for(int i = 0; i < (int) TypeGameState.Last; i++)
            {
                this.stateMachines.Add(new ClickObjectsStateMachine());
            }

            this.GameState = TypeGameState.Attack;
        }
    }
}
