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

    public class Label : MonoObject
    {
        public override int Width { get => (int) this.Font.MeasureString(this.Text).X; }
        public override int Height => this.Font.Texture.Height;

        public override Vector2 Position
        {
            get => base.Position; set { base.Position = value; base.TextPosition = value; }
        }

        public Label()
        {
            
        }

        
    }
}
