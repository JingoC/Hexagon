using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

namespace HexagonLibrary
{
    using Entity.GameObjects;
    using Device;
    using Model.Navigation;
    using Entity.Players;
    using Entity.Players.Strategy;
    using Model.StateMachines;
    
    public class Core
    {
        Thread threadActionCpu;
        GameObjectPositionInfo lastPosInfo = new GameObjectPositionInfo();
        StateMachine stateMachine;

        public Map Map { get; set; }
        public IGameDevice Device { get; private set; }
        public int Step { get; private set; }
        public bool IsReady { get; private set; }

        public User User { get; set; }
        public List<CPU> CPUs { get; set; }

        public Core(GameDeviceType deviceType = GameDeviceType.Touch, int rows = 10, int columns = 10)
        {
            this.Step = 0;
            this.Map = new Map(rows, columns);

            this.Device = new MonoDevice() { Type = deviceType };
            
            this.User = new User() { ID = 0 };
            this.CPUs = new List<CPU>();
            this.CPUs.Add(new CPU() { ID = 1, Strategy = new FirstStrategy() });
            this.CPUs.Add(new CPU() { ID = 2, Strategy = new FirstStrategy() });
            this.CPUs.Add(new CPU() { ID = 3, Strategy = new FirstStrategy() });

            this.threadActionCpu = new Thread(new ThreadStart(delegate()
            {
                foreach(var item in this.CPUs)
                {
                    item.Strategy.Calculate(this.Map, item);
                }

                this.Device.Enable = true;
                this.IsReady = true;
                this.Step++;
            }));

            this.stateMachine = new StateMachine(this.Device) { Map = this.Map };

            this.stateMachine.ClickHisObject += StateMachine_ClickHisObject;
            this.stateMachine.ClickOutOfRange += StateMachine_ClickOutOfRange;
            this.stateMachine.ClickAroundObject += StateMachine_ClickAroundObject;
            this.stateMachine.ClickNotAroundObject += StateMachine_ClickNotAroundObject;

            this.stateMachine.ClickAddLootPoint += StateMachine_ClickAddLootPoint;
            this.stateMachine.ClickModifyFreeOjbect += StateMachine_ClickModifyFreeOjbect;
            this.stateMachine.SetActivePlayer(this.User);
        }

        private void StateMachine_ClickModifyFreeOjbect(object sender, StateMachineEventArgs e)
        {
            Player p = sender as Player;
            if (p.LootPoints >= e.CurrentObject.Life)
            {
                e.CurrentObject.BelongUser = -1;
                e.CurrentObject.Type = TypeHexagon.Blocked;
                e.CurrentObject.DefaultTexture = MonoObject.GetTexture(TypeTexture.FieldMarked);

                p.LootPoints -= e.CurrentObject.Life;
                e.CurrentObject.Life = 0;
            }
        }

        private void StateMachine_ClickAddLootPoint(object sender, StateMachineEventArgs e)
        {
            Player p = sender as Player;
            if (p.LootPoints > 0)
            {
                e.CurrentObject.Life++;
                p.LootPoints--;
            }
        }

        private void StateMachine_ClickNotAroundObject(object sender, StateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => x.RestoreDefaultTexture());
        }

        private void StateMachine_ClickAroundObject(object sender, StateMachineEventArgs e)
        {
            Player player = sender as Player;
            HexagonObject lo = e.LastObject;
            HexagonObject co = e.CurrentObject;
            
            if (lo.Life > 0)
            {
                if (co.Life > lo.Life)
                {
                    co.Life -= lo.Life;
                    lo.Life = 0;
                }
                else
                {
                    co.Life = lo.Life - (co.Life == 0 ? 1 : co.Life);
                    lo.Life = 0;
                    co.BelongUser = player.ID;
                    co.DefaultTexture = MonoObject.GetTexture((TypeTexture)player.ID);
                    lo.RestoreDefaultTexture();
                    co.Texture = MonoObject.GetTexture((TypeTexture)(TypeTexture.UserActive0 + player.ID));
                    co.Type = TypeHexagon.User;
                }
            }
        }

        private void StateMachine_ClickOutOfRange(object sender, StateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => x.RestoreDefaultTexture());
        }

        private void StateMachine_ClickHisObject(object sender, StateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => x.RestoreDefaultTexture());
            e.CurrentObject.Texture = MonoObject.UserActiveTextures[0];
        }
        
        public void LoadContent(Object content)
        {
            MonoObject.LoadContent(content);

            Random r = new Random((int)DateTime.Now.Ticks);
            for(int row = 0; row < this.Map.Width; row++)
            {
                for(int col = 0; col < this.Map.Height; col++)
                {
                    this.Map.SetItem(new HexagonObject() { DefaultTexture = MonoObject.FieldTextures[0], Loot = r.Next(1, 10), Life = r.Next(0, 2) }, row, col);
                }
            }
            
            this.Map.SetItem(new HexagonObject() { DefaultTexture = MonoObject.UserIdleTextures[0], BelongUser = 0, Life = 2, Loot = 4 }, 1, 0);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = MonoObject.UserIdleTextures[1], Life = 4, Loot = 2 }, 2, 8);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = MonoObject.UserIdleTextures[2], Life = 2, Loot = 2 }, 8, 2);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = MonoObject.UserIdleTextures[3], Life = 16, Loot = 2 }, 9, 7);

            
        }

        public void Update()
        {
            this.Device.Update();
        }

        public void EndStep()
        {
            this.stateMachine.SetGameState(TypeGameState.EndStep);
            this.User.LootPoints += this.Map.Items.Where((x) => x.BelongUser == this.User.ID).Sum((x) => x.Loot);
        }

        public void NextStep()
        {
            this.IsReady = false;
            this.Device.Enable = false;

            this.stateMachine.SetGameState(TypeGameState.Cpu);
            this.threadActionCpu.Start();
        }
    }
}
