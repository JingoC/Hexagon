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

    using MonoGuiFramework.System;

    public enum TypeState
    {
        Idle = 0,
        Click_HisObject,
        Click_NearObject,
        Click_FarObject,
        Click2_HisObject,
        Click2_NearObject,
        Click2_FarObject,
        Click_NoGameObject
    }
    
    public class ClickObjectsStateMachine
    {
        private Player player;
        private TypeState state;
        private HexagonObject lastObject;
        private HexagonObject currentObject;
        
        public Map Map { get; set; }
        public bool Enable { get; set; }
        public void SetActivePlayer(Player player) {this.player = player;}

        /// <summary>
        /// Событие, когда произведен первичный клик по собственному объекту
        /// </summary>
        public event ClickObjectsStateMachineEventHandler ClickHisObject;

        /// <summary>
        /// Событие, когда произведен повторный клик по собственному объекту
        /// </summary>
        public event ClickObjectsStateMachineEventHandler DoubleClickHisObject;

        /// <summary>
        /// Событие, когда произведен первичный клик по соседнему объекту
        /// </summary>
        public event ClickObjectsStateMachineEventHandler ClickNearObject;

        public event ClickObjectsStateMachineEventHandler DoubleClickNearObject;

        /// <summary>
        /// Событие, когда произведен первичный клик по отдаленному объекту
        /// </summary>
        public event ClickObjectsStateMachineEventHandler ClickFarObject;

        public event ClickObjectsStateMachineEventHandler DoubleClickFarObject;

        /// <summary>
        /// Событие, когда произведен клик по участку за пределами карты
        /// </summary>
        public event ClickObjectsStateMachineEventHandler ClickOutOfRange;
        
        public ClickObjectsStateMachine()
        {
            this.state = TypeState.Idle;
            
            InputSingleton.GetInstance().ClickMouse += Device_ScreenClick;
            InputSingleton.GetInstance().ClickTouch += Device_ScreenClick;
        }

        private void Device_ScreenClick(object sender, DeviceEventArgs e)
        {
            if (this.Enable)
            {
                var control = this.Map.Items.FirstOrDefault((x) => (x as HexagonObject).IsEntry(e.X, e.Y));
                this.currentObject = control != null ? control as HexagonObject : null;

                this.GameState_PlayHandler();
            }
        }

        void GameState_PlayHandler()
        {
            if (this.currentObject != null)
            {
                bool isHisLO = (this.lastObject != null) && (this.lastObject.BelongUser == this.player.ID);
                bool isHisCO = this.currentObject.BelongUser == this.player.ID;
                bool isEqualLoCo = this.currentObject.Equals(this.lastObject);

                if (isHisCO)
                {
                    switch (this.state)
                    {
                        case TypeState.Click_HisObject:{this.SetState(isEqualLoCo ? TypeState.Click2_HisObject : TypeState.Click_HisObject);}break;
                        case TypeState.Click2_HisObject: this.SetState(TypeState.Click2_HisObject); break;
                        default: this.SetState(TypeState.Click_HisObject); break;
                    }
                }
                else
                {
                    bool isNearObject = this.Map.GetPositionInfo(this.currentObject).AroundObjects.Any(x => x.BelongUser == this.player.ID);
                    bool isLinkedObject = (this.lastObject != null) && (this.Map.IsLinkedObjects(this.currentObject, this.lastObject));

                    switch (this.state)
                    {
                        case TypeState.Click_HisObject:
                        case TypeState.Click2_NearObject:
                        case TypeState.Click2_HisObject:
                        {
                            this.SetState(isLinkedObject ? TypeState.Click2_NearObject : isNearObject ? TypeState.Click_NearObject : TypeState.Click_FarObject);
                        } break;
                        default:
                        {
                            this.SetState(isNearObject ? TypeState.Click_NearObject : TypeState.Click_FarObject);
                        } break;
                    }
                }
                
                this.lastObject = this.currentObject;
                this.currentObject = null;
            }
            else
            {
                this.lastObject = null;
                this.SetState(TypeState.Click_NoGameObject);
            }
        }
        
        void SetState(TypeState state)
        {
            TypeState lastState = this.state;

            switch (state)
            {
                case TypeState.Idle: break;
                case TypeState.Click_HisObject: this.EventExecute(this.ClickHisObject); break;
                case TypeState.Click_NearObject: this.EventExecute(this.ClickNearObject); break;
                case TypeState.Click_FarObject: this.EventExecute(this.ClickFarObject); break;
                case TypeState.Click2_HisObject: this.EventExecute(this.DoubleClickHisObject); break;
                case TypeState.Click2_NearObject: this.EventExecute(this.DoubleClickNearObject); break;
                case TypeState.Click2_FarObject: this.EventExecute(this.DoubleClickFarObject); break;
                case TypeState.Click_NoGameObject: this.EventExecute(this.ClickOutOfRange); break;
                default: break;
            }

            this.state = state;
        }

        void EventExecute(ClickObjectsStateMachineEventHandler eventObject)
        {
            if (eventObject != null)
            {
                ClickObjectsStateMachineEventArgs sm = new ClickObjectsStateMachineEventArgs();
                sm.SourceObject = this.lastObject;
                sm.DestinationObject = this.currentObject;

                eventObject(this.player, sm);
            }
        }
    }
}
