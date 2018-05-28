using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Model.Navigation
{
    using Entity.GameObjects;

    public class GameObjectPositionInfo
    {
        public HexagonObject Current { get; set; }
        public List<HexagonObject> AroundObjects { get; set; } = new List<HexagonObject>();

        public GameObjectPositionInfo()
        {

        }
    }
}
