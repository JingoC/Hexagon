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

    public enum TypeResource
    {
        Texture = 1,
        Font = 2
    }

    public class WSystem : IDisposable
    {
        Dictionary<String, Object> resources = new Dictionary<string, Object>();
        Dictionary<String, TypeResource> resourcesInfo = new Dictionary<string, TypeResource>(); 

        public Graphics Graphics { get; private set; } = GraphicsSingleton.GetInstance();
        public Input Input { get; private set; } = InputSingleton.GetInstance();

        public List<Activity> Activities { get; private set; } = new List<Activity>();

        public Activity ActivitySelected;

        public event EventHandler LoadContentEvent;
        public event EventHandler UpdateEvent;

        public void SetResource(String name, TypeResource tr)
        {
            this.resourcesInfo.Add(name, tr);
        }

        public Object GetResource(String name)
        {
            return this.resources[name];
        }

        public WSystem()
        {
            this.Input.ClickTouch += (s, e) => this.ActivitySelected.Items.ForEach((x) => x.CheckEntry(e.X, e.Y));
            this.Input.ClickMouse += (s, e) => this.ActivitySelected.Items.ForEach((x) => x.CheckEntry(e.X, e.Y));

            this.Activities.Add(new Activity());
            this.Activities.Last().Background = Color.Black;
            this.ActivitySelected = this.Activities.Last();

            this.Graphics.DrawEvent += (s, e) => this.ActivitySelected.Draw();
            this.Graphics.UpdateEvent += delegate (object s, EventArgs e)
            {
                this.Input.Update();

                if (this.UpdateEvent != null)
                {
                    this.UpdateEvent(s, e);
                }
            };
            this.Graphics.LoadContentEvent += delegate (object s, EventArgs e)
            {

                foreach (var item in this.resourcesInfo)
                {
                    switch (item.Value)
                    {
                        case TypeResource.Texture: this.resources.Add(item.Key, this.Graphics.Content.Load<Texture2D>(item.Key)); break;
                        case TypeResource.Font: this.resources.Add(item.Key, this.Graphics.Content.Load<SpriteFont>(item.Key)); break;
                        default: break;
                    }
                }

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
