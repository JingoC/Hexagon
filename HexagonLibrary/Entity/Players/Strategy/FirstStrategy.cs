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

            
            var hex = this.map.Items.OfType<HexagonObject>().ToList().Where<HexagonObject>((x) => x.BelongUser == cpu.ID);

            if (hex != null)
            {
                foreach (var item in hex)
                {
                    if (item.Life > 0)
                        this.Attack(item);
                }
            }

            this.EnpStep();

        }

        void Attack(HexagonObject h)
        {
            var around = this.map.GetPositionInfo(h);

            
            HexagonObject hitem = around.AroundObjects.Where((x)=>x.BelongUser != this.cpu.ID).OrderBy((x) => x.Life).FirstOrDefault();

            if (hitem != null)
            {
                if (hitem.Life > h.Life)
                {
                    hitem.Life -= h.Life;
                    h.Life = 0;
                }
                else
                {
                    hitem.Life = h.Life - (hitem.Life == 0 ? 1 : hitem.Life);
                    h.Life = 0;
                    hitem.BelongUser = this.cpu.ID;
                    hitem.DefaultTexture = GameObject.GetTexture((TypeTexture)(TypeTexture.UserIdle0 + this.cpu.ID));
                    hitem.Type = TypeHexagon.Enemy;
                }
            }
        }

        void EnpStep()
        {
            var hex = this.map.Items.OfType<HexagonObject>().ToList().Where<HexagonObject>((x) => x.BelongUser == cpu.ID);
            this.cpu.LootPoints += hex.Sum((x) => x.Loot);

            int points = 0;
            do
            {
                foreach (var item in hex)
                {
                    if (points == this.cpu.LootPoints)
                        return;

                    item.Life++;
                    points++;
                }
            } while (true);
        }
    }
}
