using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Model.StateMachines
{
    using Model;
    using Model.Navigation;
    using Entity.GameObjects;
    using Entity.Players;
    
    public delegate void StateMachineEventHandler(Object sender, StateMachineEventArgs e);

    public class StateMachineEventArgs : EventArgs
    {
        /// <summary>
        /// Объект по котором произведен первичный клик
        /// </summary>
        public HexagonObject SourceObject { get; set; }

        /// <summary>
        /// Объект по которому произведен вторичный клик после первичного
        /// </summary>
        public HexagonObject DestinationObject { get; set; }

        /// <summary>
        /// Объект по которому произведен повторный клик
        /// </summary>
        public HexagonObject DoubleObject { get; set; }

        public StateMachineEventArgs()
        {

        }
    }
}
