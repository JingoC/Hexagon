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

        public List<HexagonObject> Get(TypeHexagon type)
        {
            return this.AroundObjects.Where(x => x.Type == type).ToList();
        }

        public int CountNotTheir()
        {
            return this.AroundObjects.Count(x => 
                        (x.BelongUser != this.Current.BelongUser)
                        && (x.SectorId >= 0));
        }

        public int CountEnemy()
        {
            return this.AroundObjects.Count(x => 
                (x.Type == TypeHexagon.Enemy) 
                && (x.BelongUser != this.Current.BelongUser)
                && (x.SectorId >= 0)
                );
        }

        public int CountFree()
        {
            return this.AroundObjects.Count(x => (x.Type == TypeHexagon.Free)
                                && (x.SectorId >= 0));
        }

        public int CountAvailableToAttack()
        {
            return this.AroundObjects.Count(x => 
                    ((x.Type == TypeHexagon.Free) 
                    || ((x.Type == TypeHexagon.Enemy) && (x.BelongUser != this.Current.BelongUser))) 
                    && (x.SectorId >= 0)
                    );
        }


    }
}
