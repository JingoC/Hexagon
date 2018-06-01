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
        public GameModeNormal() : this(new GameSettings())
        {

        }

        public GameModeNormal(GameSettings gameSettings) : base(gameSettings)
        {
            this.stateMachine.ClickHisObject += StateMachine_ClickHisObject;
            this.stateMachine.ClickOutOfRange += StateMachine_ClickOutOfRange;
            this.stateMachine.ClickAroundObject += StateMachine_ClickAroundObject;
            this.stateMachine.ClickNotAroundObject += StateMachine_ClickNotAroundObject;

            this.stateMachine.ClickAddLootPoint += StateMachine_ClickAddLootPoint;
            this.stateMachine.ClickModifyFreeOjbect += StateMachine_ClickModifyFreeOjbect;
        }

        private void StateMachine_ClickModifyFreeOjbect(object sender, StateMachineEventArgs e)
        {
            Player p = sender as Player;
            if (p.LootPoints >= e.CurrentObject.Life)
            {
                e.CurrentObject.BelongUser = -1;
                e.CurrentObject.Type = TypeHexagon.Blocked;
                e.CurrentObject.Visible = false;
                e.CurrentObject.DefaultTexture = GameObject.GetTexture(TypeTexture.FieldMarked);

                p.LootPoints -= e.CurrentObject.Life;
                e.CurrentObject.Life = 0;
            }
        }

        private void StateMachine_ClickAddLootPoint(object sender, StateMachineEventArgs e)
        {
            Player p = sender as Player;
            if ((p.LootPoints > 0) && (e.CurrentObject.Life < e.CurrentObject.MaxLife))
            {
                e.CurrentObject.Life++;
                p.LootPoints--;
            }
        }

        private void StateMachine_ClickNotAroundObject(object sender, StateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
        }

        private void StateMachine_ClickAroundObject(object sender, StateMachineEventArgs e)
        {
            this.Map.Attack(e.LastObject as HexagonObject, e.CurrentObject as HexagonObject);
        }

        private void StateMachine_ClickOutOfRange(object sender, StateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
        }

        private void StateMachine_ClickHisObject(object sender, StateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
            e.CurrentObject.Texture = GameObject.GetTexture(TypeTexture.UserActive0);
        }

        public override void EndStep()
        {
            this.stateMachine.SetGameState(TypeGameState.EndStep);
            this.User.LootPoints += this.Map.Items.Where((x) => (x as HexagonObject).BelongUser == this.User.ID).Sum((x) => (x as HexagonObject).Loot);
        }

        public override void NextStep()
        {
            this.IsReady = false;

            this.stateMachine.SetGameState(TypeGameState.Cpu);
#if true
            // multi threading
            this.threadActionCpu = new Thread(new ThreadStart(delegate ()
            {
                foreach (var item in this.CPUs)
                {
                    item.Strategy.Calculate(this.Map, item);
                }
                
                this.stateMachine.SetGameState(TypeGameState.Play);

                this.IsReady = true;
                this.Step++;
            }));
            this.threadActionCpu.Start();
#else
            foreach (var item in this.CPUs)
            {
                item.Strategy.Calculate(this.Map, item);
            }

            this.IsReady = true;
            this.Step++;
            this.stateMachine.SetGameState(TypeGameState.Play);
#endif
            }
    }
}
