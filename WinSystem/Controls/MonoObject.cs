using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;

    using WinSystem.System;

    public class MonoObject
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }

        public int Width { get => this.Texture.Width; }
        public int Height { get => this.Texture.Height; }

        public event EventHandler Click;

        public MonoObject()
        {

        }

        public void CheckEntry(int x, int y)
        {
            float x1 = this.Position.X;
            float y1 = this.Position.Y;
            float x2 = x1 + this.Width;
            float y2 = y1 + this.Height;
            if ((x > x1) && (x < x2) && (y > y1) && (y < y2) && (this.Click != null))
            {
                this.Click(this, EventArgs.Empty);
            }
        }

        virtual public void Draw()
        {
            GraphicsSingleton.GetInstance().GetSpriteBatch().Draw(this.Texture, this.Position, this.Color);
        }
    }
}
