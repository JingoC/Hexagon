using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Device
{
    public delegate void DeviceEventHandler(object sender, DeviceEventArgs e);

    public class DeviceEventArgs : EventArgs
    {
        public float X { get; set; }
        public float Y { get; set; }

        public DeviceEventArgs()
        {

        }
    }
}
