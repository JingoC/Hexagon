using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Model.StateMachines
{
    using HexagonLibrary.Entity.GameObjects;

    public class GameNormalStateMachine : GameStateMachine
    {
        public event ClickObjectsStateMachineEventHandler Attack_His;
        public event ClickObjectsStateMachineEventHandler Attack_Free;
        public event ClickObjectsStateMachineEventHandler Attack_Blocked;
        public event ClickObjectsStateMachineEventHandler Attack_Enemy;
        public event ClickObjectsStateMachineEventHandler Attack_ChangeObject;

        public event ClickObjectsStateMachineEventHandler Allocate_His;
        public event ClickObjectsStateMachineEventHandler Allocate_Blocked;
        
        public GameNormalStateMachine() : base()
        {
            this.stateMachines[(int)TypeGameState.Attack].ClickFarObject += (s, e) => this.EventExexute(this.Attack_ChangeObject, s, e);
            this.stateMachines[(int)TypeGameState.Attack].ClickNearObject += (s, e) => this.EventExexute(this.Attack_ChangeObject, s, e);
            this.stateMachines[(int)TypeGameState.Attack].ClickOutOfRange += (s, e) => this.EventExexute(this.Attack_ChangeObject, s, e);
            this.stateMachines[(int)TypeGameState.Attack].DoubleClickFarObject += (s, e) => this.EventExexute(this.Attack_ChangeObject, s, e);
            this.stateMachines[(int)TypeGameState.Attack].ClickHisObject += (s, e) => this.EventExexute(this.Attack_His, s, e);
            this.stateMachines[(int)TypeGameState.Attack].DoubleClickHisObject += (s, e) => this.EventExexute(this.Attack_His, s, e);
            this.stateMachines[(int)TypeGameState.Attack].DoubleClickNearObject += delegate (Object s, ClickObjectsStateMachineEventArgs e)
            {
                switch(e.DestinationObject.Type)
                {
                    case TypeHexagon.Free: this.EventExexute(this.Attack_Free, s, e); break;
                    case TypeHexagon.Blocked: this.EventExexute(this.Attack_Blocked, s, e); break;
                    case TypeHexagon.Enemy: this.EventExexute(this.Attack_Enemy, s, e); break;
                }
            };

            this.stateMachines[(int)TypeGameState.Allocate].ClickHisObject += (s, e) => this.EventExexute(this.Allocate_His, s, e);
            this.stateMachines[(int)TypeGameState.Allocate].DoubleClickHisObject += (s, e) => this.EventExexute(this.Allocate_His, s, e);
            this.stateMachines[(int)TypeGameState.Allocate].ClickNearObject += (s, e) => this.EventExexute(this.Allocate_Blocked, s, e);
        }
        
        void EventExexute(ClickObjectsStateMachineEventHandler handler, Object sender, ClickObjectsStateMachineEventArgs e)
        {
            if (handler != null)
                handler(sender, e);
        }
    }
}
