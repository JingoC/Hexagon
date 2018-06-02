using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using WinSystem.Controls;
    
    public class WSystem : IDisposable
    {
        public Graphics Graphics { get; private set; } = GraphicsSingleton.GetInstance();
        public Input Input { get; private set; } = InputSingleton.GetInstance();

        public List<Activity> Activities { get; private set; } = new List<Activity>();

        public Activity ActivitySelected;

        public event EventHandler LoadContentEvent;
        public event EventHandler UpdateEvent;
        
        public WSystem()
        {
            this.Input.ClickTouch += (s, e) => this.ActivitySelected.Items.ForEach((x) => x.CheckEntry(e.X, e.Y));
            this.Input.ClickMouse += (s, e) => this.ActivitySelected.Items.ForEach((x) => x.CheckEntry(e.X, e.Y));
            this.Input.PressedMouse += (s, e) => this.ActivitySelected.Items.ForEach((x) => x.CheckEntryPressed(e.X, e.Y));

            this.Activities.Add(new Activity());
            this.Activities.Last().Background = Color.Black;
            this.ActivitySelected = this.Activities.Last();

            this.Graphics.DrawEvent += (s, e) => this.ActivitySelected.Draw(s as GameTime);
            this.Graphics.UpdateEvent += delegate (object s, EventArgs e)
            {
                this.Input.Update();

                if (this.ActivitySelected != null)
                    this.ActivitySelected.Update(s as GameTime);

                if (this.UpdateEvent != null)
                    this.UpdateEvent(s, e);
            };
            this.Graphics.LoadContentEvent += delegate (object s, EventArgs e)
            {
                Resources.LoadResource();

                this.Activities.ForEach(x => x.Designer());
                if (this.LoadContentEvent != null)
                {
                    this.LoadContentEvent(s, e);
                }
            };
        }

        public void SelectActivity(String name)
        {
            var activity = this.Activities.FirstOrDefault((x) => x.Name.Equals(name));
            if (activity != null)
                this.ActivitySelected = activity;
        }

        public void Dispose()
        {
            
        }
    }
}
