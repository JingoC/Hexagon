using System;
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
            return input != null ? InputSingleton.input : new Input();
        }
    }

    public class Input
    {
        public event EventHandler ClickTouch;
        public event EventHandler PressedTouch;
        public event EventHandler UnPressedTouch;
        public event EventHandler MoveTouch;

        public event EventHandler ClickMouse;
        public event EventHandler PressedMouse;
        public event EventHandler UnPressedMouse;

        public Input()
        {
            
        }
    }
}
