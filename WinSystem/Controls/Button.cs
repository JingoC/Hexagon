using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using System;

    public class Button : MonoObject
    {
        public string Text { get; set; } = String.Empty;
        public Color ForeColor { get; set; } = Color.Black;
        public SpriteFont Font { get; set; }

        public Button()
        {

        }

        public override void Draw()
        {
            base.Draw();

            Vector2 position = new Vector2(this.Position.X + 10, this.Position.Y + 10);
            GraphicsSingleton.GetInstance().GetSpriteBatch().DrawString(this.Font, this.Text, position, this.ForeColor);
        }
    }
}
