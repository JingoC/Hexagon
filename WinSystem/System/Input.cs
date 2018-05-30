﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem.System
{
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Input.Touch;

    public class InputSingleton
    {
        static Input input;

        static public Input GetInstance()
        {
            return input != null ? InputSingleton.input : (input = new Input());
        }
    }

    public delegate void DeviceEventHandler(object sender, DeviceEventArgs e);

    public class DeviceEventArgs : EventArgs
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float X2 { get; set; }
        public float Y2 { get; set; }

        public DeviceEventArgs()
        {

        }
    }

    public class Input
    {
        public event DeviceEventHandler ClickTouch;
        public event DeviceEventHandler PressedTouch;
        public event DeviceEventHandler UnPressedTouch;
        public event DeviceEventHandler MoveTouch;

        public event DeviceEventHandler ClickMouse;
        public event DeviceEventHandler PressedMouse;
        public event DeviceEventHandler UnPressedMouse;

        private bool mouseIsPressed;

        public bool Enable { get; set; } = true;

        public Input()
        {
            this.mouseIsPressed = false;
        }
        
        public void Update()
        {
            if (this.Enable)
            {
                this.UpdateMouse();
                //this.UpdateTouch();
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
                            
                            if (this.ClickMouse != null)
                                this.ClickMouse(this, e);
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

                                //this.ScreenClickExecute(this, e);
                            }
                        }
                        break;
                    default: break;
                }
            }
        }
    }
}