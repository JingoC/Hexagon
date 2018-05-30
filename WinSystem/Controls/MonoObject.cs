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

    public enum TextAlignHorizontal
    {
        Left = 1,
        Center = 2,
        Right = 3
    }

    public enum TextAlignVertical
    {
        Top = 1,
        Center = 2,
        Botton = 3
    }

    public class MonoObject : IControl
    {
        public Texture2D Texture { get; set; }
        virtual public Vector2 Position { get; set; }
        public Color Color { get; set; } = Color.White;
        public string Name { get; set; }

        public int Width { get => this.Texture.Width; }
        public int Height { get => this.Texture.Height; }

        public event EventHandler Click;

        public MonoObject()
        {

        }

        public bool IsEntry(float x, float y)
        {
            float x1 = this.Position.X;
            float y1 = this.Position.Y;
            float x2 = x1 + this.Width;
            float y2 = y1 + this.Height;

            return ((x > x1) && (x < x2) && (y > y1) && (y < y2));
        }

        public void CheckEntry(float x, float y)
        {
            if (this.IsEntry(x, y) && (this.Click != null))
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
