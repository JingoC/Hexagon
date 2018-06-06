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
            get => this.Items.Count > 0 ? (int) this.Items.Max(x => x.Width + x.Position.X) : 0;
            set { }
        }

        public virtual int Height
        {
            get => this.Items.Count > 0 ? (int)this.Items.Max(x => x.Height + x.Position.Y) : 0;
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
