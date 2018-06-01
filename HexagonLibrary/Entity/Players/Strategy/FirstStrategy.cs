using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexagonLibrary.Model.Navigation;

namespace HexagonLibrary.Entity.Players.Strategy
{
    using Entity.GameObjects;

    public class FirstStrategy : ICPUStrategy
    {
        private Map map;
        private CPU cpu;

        public FirstStrategy()
        {

        }

        public void Calculate(Map map, CPU cpu)
        {
            this.map = map;
            this.cpu = cpu;


            var hex = this.map.Items.OfType<HexagonObject>()
                .Where((x) => x.BelongUser == cpu.ID)
                .Where((x) => x.Life > 0)
                .Where((x) => this.map.GetPositionInfo(x).AroundObjects.Count((y) => (y.BelongUser != cpu.ID) && (y.Type != TypeHexagon.Blocked)) > 0)
                .ToList();

            if (hex != null)
            {
                foreach (var item in hex)
                {
                    if (item.Life > 0)
                        this.Attack(item);

                    //System.Threading.Thread.Sleep(200);
                }
            }

            this.EnpStep();
        }

        void Attack(HexagonObject h)
        {
            var around = this.map.GetPositionInfo(h);

            HexagonObject hitem = around.AroundObjects
                .Where((x) => (x.BelongUser != this.cpu.ID) && (x.Type != TypeHexagon.Blocked))
                .OrderBy((x) => x.Life)
                .FirstOrDefault();

            this.map.Attack(h, hitem);
        }

        void EnpStep()
        {
            var hex = this.map.Items.OfType<HexagonObject>()
                .Where((x) => x.BelongUser == cpu.ID)
                .Where((x) => x.Life < x.MaxLife)
                .ToList();

            if (hex.Count() <= 0)
                return;

            this.cpu.LootPoints += hex.Sum((x) => x.Loot);
            
            do
            {
                bool allFull = true;

                foreach (var item in hex)
                {
                    if (item.Life == item.MaxLife)
                        continue;

                    allFull = false;

                    if (this.cpu.LootPoints == 0)
                        return;

                    item.Life++;
                    this.cpu.LootPoints--;
                }

                if (allFull)
                    break;
            } while (true);
        }
    }
}
