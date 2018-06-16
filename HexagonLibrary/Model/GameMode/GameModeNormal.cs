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
            this.stateMachine.DoubleClickHisObject += StateMachine_DoubleClickHisObject;

            this.stateMachine.ClickAddLootPoint += StateMachine_ClickAddLootPoint;
            this.stateMachine.ClickModifyFreeOjbect += StateMachine_ClickModifyFreeOjbect;

        }
        
        private void StateMachine_ClickModifyFreeOjbect(object sender, StateMachineEventArgs e)
        {
            Player p = sender as Player;
#if false
            if (p.LootPoints >= e.DestinationObject.Life)
            {
                e.DestinationObject.BelongUser = -1;
                e.DestinationObject.Type = TypeHexagon.Blocked;
                e.DestinationObject.Visible = false;
                e.DestinationObject.SetDefaultTexture(TypeTexture.FieldMarked);

                p.LootPoints -= e.DestinationObject.Life;
                e.DestinationObject.Life = 0;
            }
#endif
            if (p.LootPoints == 0)
            {
                this.NextStep();
            }
        }

        private void StateMachine_ClickAddLootPoint(object sender, StateMachineEventArgs e)
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

        private void StateMachine_ClickNotAroundObject(object sender, StateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
        }

        private void StateMachine_ClickAroundObject(object sender, StateMachineEventArgs e)
        {
            var src = e.SourceObject as HexagonObject;
            var dst = e.DestinationObject as HexagonObject;

            if (this.Map.Attack(src, dst))
            {
                src.RestoreDefaultTexture();
                dst.TextureManager.Textures.Change((int)TypeTexture.UserActive0);
            }
        }

        private void StateMachine_ClickOutOfRange(object sender, StateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
        }

        private void StateMachine_ClickHisObject(object sender, StateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
            e.DestinationObject.TextureManager.Textures.Change((int)TypeTexture.UserActive0);
        }

        private void StateMachine_DoubleClickHisObject(object sender, StateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
            e.DestinationObject.TextureManager.Textures.Change((int)TypeTexture.UserActive0);
        }

        public override void EndStep()
        {
            if (this.stateMachine.GetGameState() == TypeGameState.Play)
            {
                this.Map.Items.OfType<HexagonObject>().Where(x => x.BelongUser == this.User.ID).ToList<HexagonObject>().ForEach(x => x.RestoreDefaultTexture());
                this.stateMachine.SetGameState(TypeGameState.EndStep);
                this.User.LootPoints += this.Map.Items.Where((x) => (x as HexagonObject).BelongUser == this.User.ID).Sum((x) => (x as HexagonObject).Loot);
            }
        }

        public override void NextStep()
        {

            if (this.stateMachine.GetGameState() == TypeGameState.EndStep)
            {
                this.IsReady = false;

                this.stateMachine.SetGameState(TypeGameState.Cpu);

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

                    base.NextStep();

                }));
                this.threadActionCpu.Start();
            }
        }
    }
}
