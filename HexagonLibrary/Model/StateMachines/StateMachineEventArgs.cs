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
        public HexagonObject LastObject { get; set; }
        public HexagonObject CurrentObject { get; set; }

        public StateMachineEventArgs()
        {

        }
    }
}
