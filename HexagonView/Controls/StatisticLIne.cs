using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonView.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using MonoGuiFramework.Controls;

    using MonoGuiFramework.System;

    public class StatisticObject
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public double Value { get; set; }
    }

    public class StatisticLine : MonoObject, IDisposable
    {
        public List<StatisticObject> Items { get; set; } = new List<StatisticObject>();
        private List<Texture2D> textures { get; set; } = new List<Texture2D>();

        int width;
        int height;

        public override int Height => this.height;
        public override int Width => this.width;

        public StatisticLine() : this(10, 10)
        {

        }

        public StatisticLine(int width, int height) : base()
        {
            this.width = width;
            this.height = height;
        }

        public override void Draw(GameTime gameTime)
        {
            this.Dispose();

            var g = GraphicsSingleton.GetInstance();
            var sb = g.GetSpriteBatch();

            double sum = this.Items.Sum(i => i.Value);

            var x = this.Position.Absolute.X;
            var y = this.Position.Absolute.Y;
            var h = this.height;

            foreach (var item in this.Items)
            {
                int w = (int)(item.Value * this.width / sum);

                if (w > 0)
                {
                    Texture2D rect = new Texture2D(g.GraphicsDevice, w, h);
                    Color[] data = new Color[w * h];
                    for (int i = 0; i < data.Length; i++) data[i] = item.Color;
                    rect.SetData(data);

                    Vector2 pos = new Vector2(x, y);
                    sb.Draw(rect, pos, item.Color);
                    this.textures.Add(rect);
                    x += (float)w;
                }
            }
        }

        public void Dispose()
        {
            foreach(var texture in this.textures)
            {
                texture.Dispose();
            }

            this.textures.Clear();
        }
    }
}
