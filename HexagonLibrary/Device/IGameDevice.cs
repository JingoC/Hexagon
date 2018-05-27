using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Device
{
    public enum GameDeviceType
    {
        Mouse = 1,
        Touch = 2
    }

    public interface IGameDevice
    {
        bool Enable { get; set; }
        GameDeviceType Type { get; set; }

        event DeviceEventHandler ScreenClick;
        void Update();
    }
}
