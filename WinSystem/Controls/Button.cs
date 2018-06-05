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
        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                base.TextPosition = new Vector2(value.X + this.Width / 8, value.Y + this.Height / 2);
            }
        }

        public Button()
        {
            this.OnPressed += (s, e) => this.TextureManager.Textures.Change(1);
            this.OnClick += (s, e) => this.TextureManager.Textures.RestoreDefault();
        }

        public override void Designer()
        {
            if (this.Texture == null)
            {
                this.TextureManager.Textures.AddRange(Resources.GetResources(new List<string>()
                {
                    "defaultButton",
                    "defaultButtonPressed"
                }).OfType<Texture2D>());
            }

            base.Designer();
        }
    }
}
