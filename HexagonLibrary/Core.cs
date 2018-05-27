using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HexagonLibrary
{
    using Entity.GameObjects;
    using Device;
    using Model;

    public class Core
    {
        public Map Map { get; set; }
        public IGameDevice Device { get; private set; }
        public int Step { get; private set; }
        public bool IsReady { get; private set; }

        public Core(GameDeviceType deviceType = GameDeviceType.Touch)
        {
            this.Step = 0;
            this.Map = new Map();

            this.Device = new MonoDevice() { Type = deviceType };
            this.Device.ScreenClick += Device_ScreenClick;
        }

        private void Device_ScreenClick(object sender, DeviceEventArgs e)
        {
            var hexagon = this.Map.Items.FirstOrDefault((x) => x.IsIntersection(e.X, e.Y));
            if (hexagon != null)
            {
                if (hexagon.BelongUser == 0)
                {
                    hexagon.Texture = MonoObject.UserActiveTextures[0];
                }
                else
                {
                    foreach(var item in this.Map.Items)
                    {
                        if (item.BelongUser == 0)
                        {
                            item.Texture = MonoObject.UserIdleTextures[0];
                        }
                    }
                }
            }
        }

        public void Update()
        {
            this.Device.Update();
        }

        public void NextStep()
        {
            this.IsReady = false;
            this.Device.Enable = false;
        }

        
    }
}
