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
        string GetLog();
        void Calculate(Map map, CPU cpu);
    }
}
