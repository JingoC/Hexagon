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

        public int CountNotTheir()
        {
            return this.AroundObjects.Count(x => x.BelongUser != this.Current.BelongUser);
        }

        public int CountEnemy()
        {
            return this.AroundObjects.Count(x => (x.Type == TypeHexagon.Enemy) && (x.BelongUser != this.Current.BelongUser));
        }

        public int CountFree()
        {
            return this.AroundObjects.Count(x => x.Type == TypeHexagon.Free);
        }

        public int CountAvailableToAttack()
        {
            return this.AroundObjects.Count(x => ((x.Type == TypeHexagon.Free) || ((x.Type == TypeHexagon.Enemy) && (x.BelongUser != this.Current.BelongUser))));
        }


    }
}
