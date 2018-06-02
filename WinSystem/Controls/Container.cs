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
        public List<IControl> Items { get; set; } = new List<IControl>();

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
