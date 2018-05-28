using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Entity.Players.Strategy
{
    using Model.Navigation;

    public interface ICPUStrategy
    {
        void Calculate(Map map, CPU cpu);
    }
}
