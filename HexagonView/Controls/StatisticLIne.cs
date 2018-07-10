using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonView.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using MonoGuiFramework.Base;

    using MonoGuiFramework.System;

    public class StatisticObject
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public double Value { get; set; }
    }

    public class StatisticLine : Control, IDisposable
    {
        public List<StatisticObject> Items { get; set; } = new List<StatisticObject>();
        private List<Texture2D> textures { get; set; } = new List<Texture2D>();
        
        public override int Height => base.Height;
        public override int Width => base.Width;

        public StatisticLine() : this(10, 10)
        {

        }

        public StatisticLine(int width, int height) : base()
        {
            this.Width = width;
            this.Height = height;
        }

        public override void Draw(GameTime gameTime)
        {
            this.Dispose();

            double sum = this.Items.Sum(i => i.Value);

            var x = this.Position.Absolute.X;
            var y = this.Position.Absolute.Y;
            var h = this.Height;

            foreach (var item in this.Items)
            {
                int w = (int)(item.Value * this.Width / sum);

                if (w > 0)
                {
                    Texture2D rect = new Texture2D(Graphics.GraphicsDevice, w, h);
                    Color[] data = new Color[w * h];
                    for (int i = 0; i < data.Length; i++) data[i] = item.Color;
                    rect.SetData(data);

                    Vector2 pos = new Vector2(x, y);
                    SpriteBatch.Draw(rect, pos, item.Color);
                    this.textures.Add(rect);
                    x += (float)w;
                }
            }
        }

        public override void Dispose()
        {
            foreach(var texture in this.textures)
            {
                texture.Dispose();
            }

            this.textures.Clear();
            base.Dispose();
        }
    }
}
