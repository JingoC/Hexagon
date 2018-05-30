using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem
{
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    using Controls;
    using System;

    public class Activity
    {
        public List<MonoObject> Controls { get; private set; } = new List<MonoObject>();
        public Color Background { get; set; }

        public Activity()
        {

        }

        public void Draw()
        {
            GraphicsSingleton.GetInstance().GraphicsDevice.Clear(this.Background);

            this.Controls.ForEach((x) => x.Draw());
        }
    }
}
