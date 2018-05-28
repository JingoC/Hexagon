using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Entity.Players
{
    using Strategy;

    public class CPU : Player
    {
        public ICPUStrategy Strategy { get; set; }

        public CPU()
        {

        }
    }
}
