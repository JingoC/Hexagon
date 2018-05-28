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

    public enum StateMode
    {
        Play = 1,
        EndStep = 2,
        NextStep = 3,
        Cpu = 4
    }

    public class Core
    {
        Thread threadActionCpu;
        GameObjectPositionInfo lastPosInfo = new GameObjectPositionInfo();

        public Map Map { get; set; }
        public IGameDevice Device { get; private set; }
        public int Step { get; private set; }
        public bool IsReady { get; private set; }
        public StateMode State { get; private set; }

        public User User { get; set; }
        public List<CPU> CPUs { get; set; }

        public Core(GameDeviceType deviceType = GameDeviceType.Touch, int rows = 10, int columns = 10)
        {
            this.Step = 0;
            this.Map = new Map(rows, columns);

            this.Device = new MonoDevice() { Type = deviceType };
            this.Device.ScreenClick += Device_ScreenClick;

            this.State = StateMode.Play;

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

                this.State = StateMode.Play;
                this.Device.Enable = true;
                this.IsReady = true;
                this.Step++;
            }));
        }

        private void Device_ScreenClick(object sender, DeviceEventArgs e)
        {
            var hexagon = this.Map.Items.FirstOrDefault((x) => x.IsIntersection(e.X, e.Y));
            if (hexagon != null)
            {
                switch(this.State)
                {
                    case StateMode.Play:
                        {
                            if (hexagon.BelongUser == 0)
                            {
                                hexagon.Texture = MonoObject.UserActiveTextures[0];
                                this.lastPosInfo.AroundObjects.ForEach((x) => x.RestoreDefaultTexture());
                            }
                            else
                            {
                                foreach (var item in this.Map.Items)
                                {
                                    if (item.BelongUser == 0)
                                    {
                                        item.Texture = MonoObject.UserIdleTextures[0];
                                    }
                                }

                                this.lastPosInfo.AroundObjects.ForEach((x) => x.RestoreDefaultTexture());

                                var pi = this.Map.GetPositionInfo(hexagon);
                                pi.AroundObjects.ForEach((x) => x.Texture = MonoObject.GetTexture(TypeTexture.FieldMarked));

                                this.lastPosInfo = pi;
                            }
                        }
                        break;
                    case StateMode.EndStep:
                        {

                        }break;
                    default: break;
                }
            }
        }

        public void LoadContent(Object content)
        {
            MonoObject.LoadContent(content);

            for(int row = 0; row < this.Map.Width; row++)
            {
                for(int col = 0; col < this.Map.Height; col++)
                {
                    this.Map.SetItem(new HexagonObject() { DefaultTexture = MonoObject.FieldTextures[0] }, row, col);
                }
            }
            
            this.Map.SetItem(new HexagonObject() { DefaultTexture = MonoObject.UserIdleTextures[0], BelongUser = 0, Life = 8 }, 1, 0);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = MonoObject.UserIdleTextures[1], Life = 4 }, 2, 8);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = MonoObject.UserIdleTextures[2], Life = 2 }, 8, 2);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = MonoObject.UserIdleTextures[3], Life = 16 }, 9, 7);
        }

        public void Update()
        {
            this.Device.Update();
        }

        public void NextStep()
        {
            this.State = StateMode.NextStep;
            this.IsReady = false;
            this.Device.Enable = false;

            this.threadActionCpu.Start();
        }
    }
}
