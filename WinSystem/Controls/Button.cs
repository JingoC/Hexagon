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
        public Button()
        {
            this.defaultTexture = "defaultButton";

            this.OnPressed += (s, e) => this.Texture = Resources.GetResource("defaultButtonPressed") as Texture2D;
            this.OnClick += (s, e) => this.Texture = Resources.GetResource("defaultButton") as Texture2D;
        }
    }
}
