using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Model.StateMachines
{
    using Model.Navigation;
    using Entity.Players;
    using Entity.GameObjects;

    using WinSystem.System;

    public enum TypeState
    {
        Idle = 0,
        Click_Background = 1,
        Click_HisObject = 2,
        Click_FreeObject = 3,
        Click_EnemyObject = 4,
        Click2_HisObject = 5,
        Click2_FreeObject = 6,
        Click2_EnemyObject = 7,
        Click2_System = 8
    }

    public enum TypeGameState
    {
        Play = 1,
        EndStep = 2,
        NextStep = 3,
        Cpu = 4
    }

    public class StateMachine
    {
        private Player player;
        private TypeState state;
        private HexagonObject lastObject;
        private HexagonObject currentObject;
        private TypeGameState gameState;
        
        public Map Map { get; set; }

        public void SetActivePlayer(Player player)
        {
            this.player = player;
        }

        public void SetGameState(TypeGameState state)
        {
            this.gameState = state;
        }

        public TypeGameState GetGameState()
        {
            return this.gameState;
        }

        #region События во время state == TypeState.Play
        /// <summary>
        /// Событие, когда произведен первичный клик по собственному объекту
        /// </summary>
        public event StateMachineEventHandler ClickHisObject;

        /// <summary>
        /// Событие, когда произведен первичный клик по соседнему объекту
        /// </summary>
        public event StateMachineEventHandler ClickAroundObject;

        /// <summary>
        /// Событие, когда произведен первичный клик по отдаленному объекту
        /// </summary>
        public event StateMachineEventHandler ClickNotAroundObject;

        /// <summary>
        /// Событие, когда произведен клик по участку за пределами карты
        /// </summary>
        public event StateMachineEventHandler ClickOutOfRange;

        #endregion

        #region События во время state == TypeState.EndStep

        public event StateMachineEventHandler ClickAddLootPoint;
        public event StateMachineEventHandler ClickModifyFreeOjbect;

        #endregion

        public StateMachine()
        {
            this.state = TypeState.Idle;
            this.gameState = TypeGameState.Play;
            
            InputSingleton.GetInstance().ClickMouse += Device_ScreenClick;
            InputSingleton.GetInstance().ClickTouch += Device_ScreenClick;
        }

        private void Device_ScreenClick(object sender, DeviceEventArgs e)
        {
            var control = this.Map.Items.FirstOrDefault((x) => (x as HexagonObject).IsEntry(e.X, e.Y));
            HexagonObject hexagon = control != null ? control as HexagonObject : null;

            switch (this.gameState)
            {
                case TypeGameState.Play: this.GameState_PlayHandler(hexagon); break;
                case TypeGameState.EndStep: this.GameState_EndStepHandler(hexagon); break;
                case TypeGameState.NextStep: break;
                case TypeGameState.Cpu: break;
                default: break;
            }
        }

        void GameState_EndStepHandler(HexagonObject hexagon)
        {
            if (hexagon != null)
            {
                this.currentObject = hexagon;

                if (this.currentObject.BelongUser == this.player.ID)
                {
                    this.SetState(TypeState.Click_HisObject);
                }
                else
                {
                    if (this.Map.GetPositionInfo(this.currentObject).AroundObjects.Any((x) => x.BelongUser == this.player.ID))
                    {
                        this.SetState(TypeState.Click_FreeObject);
                    }
                }
            }
            else
            {

            }
        }

        void GameState_PlayHandler(HexagonObject hexagon)
        {
            if (hexagon != null)
            {
                this.currentObject = hexagon;

                if (this.currentObject.BelongUser == this.player.ID)
                {
                    switch (this.state)
                    {
                        case TypeState.Click_HisObject:
                            {
                                this.SetState(this.currentObject.Equals(this.lastObject) ? TypeState.Click2_HisObject : TypeState.Click_HisObject);
                            }
                            break;
                        case TypeState.Click2_HisObject: this.SetState(TypeState.Click2_HisObject); break;
                        default: this.SetState(TypeState.Click_HisObject); break;
                    }
                }
                else
                {
                    switch (this.currentObject.Type)
                    {
                        case TypeHexagon.Free:
                            {
                                switch (this.state)
                                {
                                    case TypeState.Click2_FreeObject:
                                        {
                                            if (this.lastObject.BelongUser == this.player.ID)
                                            {
                                                this.SetState(this.Map.IsLinkedObjects(this.lastObject, this.currentObject) ?
                                                    TypeState.Click2_FreeObject : TypeState.Click_FreeObject);
                                            }
                                            else
                                            {
                                                this.SetState(TypeState.Click_FreeObject);
                                            }
                                        }
                                        break;
                                    case TypeState.Click_HisObject:
                                        {
                                            this.SetState(this.Map.IsLinkedObjects(this.lastObject, this.currentObject) ?
                                                TypeState.Click2_FreeObject : TypeState.Click_FreeObject);
                                        }
                                        break;
                                    default: this.SetState(TypeState.Click_FreeObject); break;
                                }
                            }
                            break;
                        case TypeHexagon.Blocked:
                            {
                                this.SetState(TypeState.Click_Background);
                            }
                            break;
                        case TypeHexagon.Enemy:
                            {
                                this.SetState(TypeState.Click2_EnemyObject);
                            }break;
                        default: this.SetState(TypeState.Click_Background); break;
                    }
                }

                this.lastObject = this.currentObject;
                this.currentObject = null;
            }
            else
            {
                this.lastObject = null;
                this.currentObject = null;
                this.SetState(TypeState.Click_Background);
            }
        }
        
        void SetState(TypeState state)
        {
            switch(this.gameState)
            {
                case TypeGameState.Play: this.UpdateState_Play(state); break;
                case TypeGameState.EndStep: this.UpdateState_EndStop(state); break;
                default: break;
            }
        }

        void UpdateState_EndStop(TypeState state)
        {
            TypeState lastState = this.state;

            switch(state)
            {
                case TypeState.Click_HisObject: this.EventExecute(this.ClickAddLootPoint); break;
                case TypeState.Click_FreeObject: this.EventExecute(this.ClickModifyFreeOjbect); break;
                default: this.EventExecute(this.ClickOutOfRange); break;
            }

            this.state = state;
        }

        void UpdateState_Play(TypeState state)
        {
            TypeState lastState = this.state;

            switch (state)
            {
                case TypeState.Idle: break;
                case TypeState.Click_Background: this.EventExecute(this.ClickOutOfRange); break;
                case TypeState.Click_HisObject: this.EventExecute(this.ClickHisObject); break;
                case TypeState.Click2_FreeObject: this.EventExecute(this.ClickAroundObject); break;
                case TypeState.Click2_EnemyObject: this.EventExecute(this.ClickAroundObject); break;
                default: this.EventExecute(this.ClickNotAroundObject); break;
            }

            this.state = state;
        }

        void EventExecute(StateMachineEventHandler eventObject)
        {
            if (eventObject != null)
            {
                StateMachineEventArgs sm = new StateMachineEventArgs();
                sm.SourceObject = this.lastObject;
                sm.DestinationObject = this.currentObject;

                eventObject(this.player, sm);
            }
        }
    }
}
