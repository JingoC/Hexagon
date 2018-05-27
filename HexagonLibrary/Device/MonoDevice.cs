using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Device
{
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Input.Touch;
    
    public class MonoDevice : IGameDevice
    {
        private bool mouseIsPressed;

        public MonoDevice()
        {
            this.mouseIsPressed = false;
        }

        #region IGameDevice implementation

        public GameDeviceType Type { get; set; } = GameDeviceType.Touch;
        public bool Enable { get; set; } = true;
        public event DeviceEventHandler ScreenClick;

        public void Update()
        {
            if (this.Enable)
            {
                switch (this.Type)
                {
                    case GameDeviceType.Mouse: this.UpdateMouse(); break;
                    case GameDeviceType.Touch: this.UpdateTouch(); break;
                    default: break;
                }
            }
        }

        void UpdateMouse()
        {
            switch (Mouse.GetState().LeftButton)
            {
                case ButtonState.Pressed: { this.mouseIsPressed = true; } break;
                case ButtonState.Released:
                    {
                        if (this.mouseIsPressed)
                        {
                            this.mouseIsPressed = false;
                            DeviceEventArgs e = new DeviceEventArgs();
                            e.X = Mouse.GetState().X;
                            e.Y = Mouse.GetState().Y;

                            this.ScreenClickExecute(this, e);
                        }
                    }
                    break;
                default: break;
            }
        }

        void UpdateTouch()
        {
            var touch = TouchPanel.GetState().FirstOrDefault();
            if (touch != null)
            {

                switch (touch.State)
                {
                    case TouchLocationState.Pressed: { this.mouseIsPressed = true; } break;
                    case TouchLocationState.Released:
                        {
                            if (this.mouseIsPressed)
                            {
                                this.mouseIsPressed = false;
                                DeviceEventArgs e = new DeviceEventArgs();
                                e.X = touch.Position.X;
                                e.Y = touch.Position.Y;

                                this.ScreenClickExecute(this, e);
                            }
                        }
                        break;
                    default: break;
                }
            }
        }

        private void ScreenClickExecute(object sender, DeviceEventArgs e)
        {
            if (this.ScreenClick != null)
            {
                this.ScreenClick(sender, e);
            }
        }

#endregion

    }
}
