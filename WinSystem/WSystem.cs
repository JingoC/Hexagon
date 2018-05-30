using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem
{
    using System;
    using Microsoft.Xna.Framework;

    public class WSystem
    {
        public Graphics Graphics { get; private set; } = GraphicsSingleton.GetInstance();
        public Input Input { get; private set; } = InputSingleton.GetInstance();

        public List<Activity> Activities { get; private set; } = new List<Activity>();

        public Activity ActivitySelected;

        public event EventHandler LoadContentEvent;
        public event EventHandler UpdateEvent;

        public WSystem()
        {
            this.Input.ClickTouch += Input_ClickTouch;

            this.Activities.Add(new Activity());
            this.Activities.Last().Background = Color.Black;

            this.Graphics.DrawEvent += (s, e) => this.ActivitySelected.Draw();
            this.Graphics.UpdateEvent += delegate (object s, EventArgs e)
            {
                if (this.UpdateEvent != null)
                {
                    this.UpdateEvent(s, e);
                }
            };
        }

        private void Input_ClickTouch(object sender, EventArgs e)
        {
            this.ActivitySelected.Controls.ForEach((x) => x.CheckEntry(0, 0));
        }

        
    }
}
