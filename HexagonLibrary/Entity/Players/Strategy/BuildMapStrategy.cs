using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexagonLibrary.Model.Navigation;

namespace HexagonLibrary.Entity.Players.Strategy
{
    using HexagonLibrary.Entity.GameObjects;
    using HexagonLibrary.Model.GameMode;

    public class BuildMapStrategy : ICPUStrategy
    {
        private Map map;
        private CPU cpu;
        private GameSettings settings;

        string log = String.Empty;

        public BuildMapStrategy(GameSettings settings)
        {
            this.settings = settings;
        }
        
        public string GetLog()
        {
            return this.log;
        }

        public void Calculate(Map map, CPU cpu)
        {
            this.map = map;
            this.cpu = cpu;
            
            bool IsAttacks = false;
            do
            {
                IsAttacks = false;

                var hex = this.map.Items.OfType<HexagonObject>()
                    .Where((x) => (x.BelongUser == cpu.ID) && (x.Life > 0))
                    .Where((x) => this.map.GetPositionInfo(x).CountAvailableToAttack() > 0)
                    .ToList();

                if (hex == null)
                    break;
                
                foreach (var item in hex)
                {
                    if (item.Life > 0)
                        if (this.Attack(item))
                        {
                            IsAttacks = true;
                            System.Threading.Thread.Sleep(this.settings.DelayOnAction);
                        }
                            
                }
            } while (IsAttacks);
            
            this.EndStep();
        }

        bool Attack(HexagonObject h)
        {
            var around = this.map.GetPositionInfo(h);

            HexagonObject hitem = around.AroundObjects
                .Where((x) => (x.BelongUser != this.cpu.ID))
                .Where(x => x.Type != TypeHexagon.Blocked)
                .OrderBy((x) => x.Life)
                .FirstOrDefault();

            return this.map.Attack(h, hitem);
        }

        void EndStep()
        {
            int UpdateLife(List<HexagonObject> items)
            {
                if (items.Count > 0)
                {
                    int sumNeedLife = items.Sum(x => (x.MaxLife - x.Life));

                    do
                    {
                        foreach (var item in items)
                        {
                            if (this.cpu.LootPoints == 0)
                                break;

                            if (sumNeedLife == 0)
                                break;

                            if (item.Life != item.MaxLife)
                            {
                                item.Life++;
                                this.cpu.LootPoints--;
                                sumNeedLife--;
                            }
                        }
                    } while ((this.cpu.LootPoints > 0) && ((sumNeedLife > 0)));

                }

                return this.cpu.LootPoints;
            }

            // Получение списка своих объектов
            var hexYour = this.map.Items.OfType<HexagonObject>().Where(x => x.BelongUser == this.cpu.ID).ToList();

            if (hexYour.Count() <= 0)
                return;

            this.cpu.LootPoints += hexYour.Sum((x) => x.Loot);

            // Получили список своих объектов у которых не полные жизни
            var hexNotFullLife = hexYour.Where((x) => x.Life < x.MaxLife).ToList();

            // получили список тех, у кого есть области для атаки
            var hexAttack = hexNotFullLife.Where((x) => this.map.GetPositionInfo(x).CountAvailableToAttack() > 0).ToList();

            // Заполнили жизни тех у кого есть области для атаки, с приоритетом по количеству этих областей
            for (int countBusy = 6; countBusy > 0; countBusy--)
            {
                var tmpHex = hexAttack.Where(x => this.map.GetPositionInfo(x).CountEnemy() == countBusy).ToList();

                if (UpdateLife(tmpHex) == 0)
                    return;
            }

            for (int countFree = 6; countFree > 0; countFree--)
            {
                var tmpHex = hexAttack.Where(x => this.map.GetPositionInfo(x).CountFree() == countFree).ToList();

                if (UpdateLife(tmpHex) == 0)
                    return;
            }

            // Потратили оставшиеся LootPoint на восстановление клеток
            foreach (var item in hexYour)
            {
                if (!this.cpu.IsAccessToCreate())
                    break;

                var blocked = this.map.GetPositionInfo(item).AroundObjects.Where(x => (x.Type == TypeHexagon.Blocked) && (x.SectorId >= 0)).ToList();
                if (blocked != null)
                {
                    foreach (var b in blocked)
                    {
                        if (!this.cpu.IsAccessToCreate())
                            break;

                        this.map.Create(this.cpu, b);
                    }
                }
            }

            // получили список тех у кого нет областей для атаки
            // заполнили их жизни без приоритета
            var hexNotAttack = hexNotFullLife.Where((x) => this.map.GetPositionInfo(x).CountAvailableToAttack() == 0).ToList();

            if (UpdateLife(hexNotAttack) == 0)
                return;
        }
    }
}
