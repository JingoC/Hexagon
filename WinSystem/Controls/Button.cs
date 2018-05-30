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
        
        public Button()
        {

        }

        public override void Draw()
        {
            base.Draw();

            //GraphicsSingleton.GetInstance().GetSpriteBatch().DrawString()
        }
    }
}
