using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HexagonLibrary.Model.GameMode
{
    using Model.Navigation;

    using Entity.Players;
    using Entity.GameObjects;

    using Model.StateMachines;

    public class GameModeNormal : GameModeStrategy
    {
        GameNormalStateMachine stateMachine;

        public override void Suspend()
        {
            this.stateMachine.Enable = false;
            base.Suspend();
        }

        public override void Resume()
        {
            this.stateMachine.Enable = true;
            base.Resume();
        }

        public GameModeNormal() : this(new GameSettings())
        {

        }
        
        public GameModeNormal(GameSettings gameSettings) : base(gameSettings)
        {
            this.stateMachine = new GameNormalStateMachine() { Map = this.Map, GameState = TypeGameState.Attack };
            this.stateMachine.Enable = true;
            this.stateMachine.SetActivePlayer(this.User);

            this.stateMachine.Attack_His += this.StateMachine_Attack_His;
            this.stateMachine.Attack_ChangeObject += this.StateMachine_Attack_ChangeObject;
            this.stateMachine.Attack_Free += this.StateMachine_Attack_Free;
            this.stateMachine.Attack_Enemy += this.StateMachine_Attack_Free;

            this.stateMachine.Allocate_His += this.StateMachine_Allocate_His;
            this.stateMachine.Allocate_Blocked += StateMachine_Allocate_Blocked;

        }
        
        private void StateMachine_Attack_His(object sender, ClickObjectsStateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
            e.DestinationObject.TextureManager.Textures.Change((int)TypeTexture.UserActive0);
        }

        private void StateMachine_Attack_ChangeObject(object sender, ClickObjectsStateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
        }

        private void StateMachine_Attack_Free(object sender, ClickObjectsStateMachineEventArgs e)
        {
            HexagonObject src = e.SourceObject;
            HexagonObject dst = e.DestinationObject;
            
            if (this.Map.Attack(src, dst))
            {
                src.RestoreDefaultTexture();
                dst.TextureManager.Textures.Change((int)TypeTexture.UserActive0);
            }
        }

        private void StateMachine_Allocate_Blocked(object sender, ClickObjectsStateMachineEventArgs e)
        {
            HexagonObject src = e.SourceObject;
            HexagonObject dst = e.DestinationObject;

            if ((dst != null) && (this.User.IsAccessToCreate()))
            {
                this.Map.Create(this.User, dst);
            }
        }

        private void StateMachine_Allocate_His(object sender, ClickObjectsStateMachineEventArgs e)
        {
            Player p = sender as Player;

            if ((p.LootPoints > 0) && (e.DestinationObject.Life < e.DestinationObject.MaxLife))
            {
                e.DestinationObject.Life++;
                p.LootPoints--;
            }

            if (p.LootPoints == 0)
            {
                this.NextStep();
            }
        }
        
        public override void EndStep()
        {
            if (this.stateMachine.GameState == TypeGameState.Attack)
            {
                this.Map.Items.OfType<HexagonObject>().Where(x => x.BelongUser == this.User.ID).ToList<HexagonObject>().ForEach(x => x.RestoreDefaultTexture());
                this.stateMachine.GameState = TypeGameState.Allocate;
                this.User.LootPoints += this.Map.Items.Where((x) => (x as HexagonObject).BelongUser == this.User.ID).Sum((x) => (x as HexagonObject).Loot);
            }
        }
        
        public override void NextStep()
        {
            if (this.stateMachine.GameState == TypeGameState.Allocate)
            {
                this.IsReady = false;

                this.stateMachine.GameState = TypeGameState.Wait;

                // multi threading
                this.threadActionCpu = new Thread(new ThreadStart(delegate ()
                {
                    foreach (var item in this.CPUs)
                    {
                        item.Strategy.Calculate(this.Map, item);
                    }
                    
                    this.IsReady = true;
                    this.Step++;

                    this.stateMachine.GameState = TypeGameState.Attack;
                    base.NextStep();
                }));
                this.threadActionCpu.Start();
            }
        }

        public override void LootPointAutoAllocate()
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
                            if (this.User.LootPoints == 0)
                                break;

                            if (sumNeedLife == 0)
                                break;

                            if (item.Life != item.MaxLife)
                            {
                                item.Life++;
                                this.User.LootPoints--;
                                sumNeedLife--;
                            }
                        }
                    } while ((this.User.LootPoints > 0) && ((sumNeedLife > 0)));
                }

                return this.User.LootPoints;
            }

            // Получение списка своих объектов
            var hexYour = this.Map.Items.OfType<HexagonObject>().Where(x => x.BelongUser == this.User.ID).ToList();

            if (hexYour.Count() <= 0)
                return;

            // Получили список своих объектов у которых не полные жизни
            var hexNotFullLife = hexYour.Where((x) => x.Life < x.MaxLife).ToList();

            // получили список тех, у кого есть области для атаки
            var hexAttack = hexNotFullLife.Where((x) => this.Map.GetPositionInfo(x).CountAvailableToAttack() > 0).ToList();

            // Заполнили жизни тех у кого есть области для атаки, с приоритетом по количеству этих областей
            for (int countBusy = 6; countBusy > 0; countBusy--)
            {
                var tmpHex = hexAttack.Where(x => this.Map.GetPositionInfo(x).CountEnemy() == countBusy).ToList();

                if (UpdateLife(tmpHex) == 0)
                    return;
            }

            for (int countFree = 6; countFree > 0; countFree--)
            {
                var tmpHex = hexAttack.Where(x => this.Map.GetPositionInfo(x).CountFree() == countFree).ToList();

                if (UpdateLife(tmpHex) == 0)
                    return;
            }

            // получили список тех у кого нет областей для атаки
            // заполнили их жизни без приоритета
            var hexNotAttack = hexNotFullLife.Where((x) => this.Map.GetPositionInfo(x).CountAvailableToAttack() == 0).ToList();

            if (UpdateLife(hexNotAttack) == 0)
                return;

        }
    }
}
