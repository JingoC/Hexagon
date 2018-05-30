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

        public virtual void Draw()
        {
            this.Items.ForEach((x) => x.Draw());
        }

        public virtual void CheckEntry(float x, float y)
        {
            this.Items.ForEach((v) => v.CheckEntry(x, y));
        }
    }
}
