using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem.Controls
{
    public class Container : IControl
    {
        Vector2 position;

        public string Name { get; set; }
        public TextureContainer TextureManager { get; set; }
        public List<IControl> Items { get; set; } = new List<IControl>();

        public virtual int Width
        {
            get
            {
                float x = 0;
                float w = 0;
                foreach (var item in this.Items)
                {
                    float px = item.Position.X;
                    float pw = item.Width;

                    x = (px >= 0) && (px < x) ? px : x;
                    w = (pw > 0) && (pw > w) ? pw : w;
                }

                return (int) (x + w);
            }
            set { }
        }

        public virtual int Height
        {
            get
            {
                float y1 = 0;
                float y2 = 0;
                
                foreach(var item in this.Items)
                {
                    float py = item.Position.Y;
                    float ph = item.Height;

                    y1 = (py >= 0) && (py < y1) ? py : y1;
                    y2 = (ph > 0) && ((ph + py) > y2) ? ph + py : y2;
                }

                return (int) (y2 - y1);
            }
            set { }
        }

        public Vector2 Position
        {
            get => this.position;
            set
            {
                if (value != null)
                {
                    this.position = value;
                    foreach(var item in this.Items)
                    {
                        float x = this.position.X + item.Position.X;
                        float y = this.position.Y + item.Position.Y;
                        item.Position = new Vector2(x, y);
                    }
                }
            }
        }

        public int DrawOrder { get; set; }

        public bool Visible { get; set; } = true;

        public event EventHandler OnClick;
        public event EventHandler OnPressed;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public Container()
        {

        }

        public virtual void Designer()
        {
            this.Items.ForEach(x => x.Designer());
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (this.Visible)
                this.Items.ForEach((x) => x.Draw(gameTime));
        }

        public virtual void CheckEntry(float x, float y)
        {
            if (this.Visible)
                this.Items.ForEach((v) => v.CheckEntry(x, y));
        }

        public virtual void CheckEntryPressed(float x, float y)
        {
            if (this.Visible)
                this.Items.ForEach((v) => v.CheckEntryPressed(x, y));
        }
    }
}
