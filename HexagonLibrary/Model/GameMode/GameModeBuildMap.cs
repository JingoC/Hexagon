using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HexagonLibrary.Model.GameMode
{
    using HexagonLibrary.Entity.Players;
    using HexagonLibrary.Entity.GameObjects;
    using HexagonLibrary.Model.StateMachines;

    public class GameModeBuildMap : GameModeStrategy
    {
        GameBuildMapStateMachine stateMachine;

        public GameModeBuildMap() : this(new GameSettings())
        {

        }

        public GameModeBuildMap(GameSettings gameSettings) : base(gameSettings)
        {
            this.stateMachine = new GameBuildMapStateMachine() { Map = this.Map };

            this.stateMachine.SetActivePlayer(this.User);

            this.stateMachine.Attack_His += this.StateMachine_Attack_His;
            this.stateMachine.Attack_ChangeObject += (s, e) => this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
            this.stateMachine.Attack_Blocked += this.StateMachine_Attack_Hexagon;
            this.stateMachine.Attack_Enemy += this.StateMachine_Attack_Hexagon;
            this.stateMachine.Allocate_His += this.StateMachine_Allocate_His;
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

        private void StateMachine_Attack_Hexagon(object sender, ClickObjectsStateMachineEventArgs e)
        {
            HexagonObject src = e.SourceObject;
            HexagonObject dst = e.DestinationObject;

            if (this.Map.Attack(src, dst))
            {
                src.RestoreDefaultTexture();
                dst.TextureManager.Textures.Change((int)TypeTexture.UserActive0);
            }
        }

        private void StateMachine_Attack_His(object sender, ClickObjectsStateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
            e.DestinationObject.TextureManager.Textures.Change((int)TypeTexture.UserActive0);
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
        protected override HexagonObject GetMapItem()
        {
            HexagonObject hex = new HexagonObject()
            {
                Visible = false,
                Type = TypeHexagon.Blocked,
                Loot = r.Next(this.GameSettings.ScatterLoot.Min, this.GameSettings.ScatterLoot.Max),
                Life = r.Next(this.GameSettings.ScatterLife.Min, this.GameSettings.ScatterLife.Max),
                MaxLife = this.GameSettings.IsAllEqualLife ? r.Next(8, 8) : r.Next(this.GameSettings.ScatterMaxLife.Min, this.GameSettings.ScatterMaxLife.Max)
            };
            hex.SetDefaultTexture(TypeTexture.FieldFree);

            return hex;
        }
    }
}
