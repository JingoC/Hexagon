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
        bool visible = true;
        int drawOrder = 0;

        public TextureContainer TextureManager { get; set; } = new TextureContainer();
        
        public event EventHandler OnClick;
        public event EventHandler OnPressed;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public virtual string Text { get; set; } = String.Empty;
        public Color ForeColor { get; set; } = Color.Black;
        
        virtual public Vector2 Position { get; set; }
        public Color Color { get; set; } = Color.White;
        public string Name { get; set; }

        public Texture2D Texture { get => this.TextureManager.Textures.Current; }
        public SpriteFont Font { get => this.TextureManager.Fonts.Current; }
        public virtual int Width { get => this.Texture != null ? this.Texture.Width : 0; }
        public virtual int Height { get => this.Texture != null ? this.Texture.Height : 0; }

        public int DrawOrder
        {
            get => this.drawOrder;
            set
            {
                if (this.drawOrder != value)
                {
                    this.drawOrder = value;
                    if (this.DrawOrderChanged != null)
                        this.DrawOrderChanged(this, EventArgs.Empty);
                }
            }
        }
        public bool Visible
        {
            get => this.visible;
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    if (this.VisibleChanged != null)
                        this.VisibleChanged(this, EventArgs.Empty);
                }
            }
        }
        
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
            if (this.Visible)
            {
                if (this.IsEntry(x, y) && (this.OnClick != null))
                {
                    this.OnClick(this, EventArgs.Empty);
                }
            }
        }

        public virtual void Designer()
        {
            if (this.Font == null)
            {
                this.TextureManager.Fonts.Add(Resources.GetResource("defaultFont") as SpriteFont);
            }
        }

        public void CheckEntryPressed(float x, float y)
        {
            if (this.Visible)
            {
                if (this.IsEntry(x, y) && (this.OnPressed != null))
                    this.OnPressed(this, EventArgs.Empty);
            }
        }

        virtual public void Draw(GameTime gameTime)
        {
            if (this.Visible)
            {
                if (this.Texture != null)
                    GraphicsSingleton.GetInstance().GetSpriteBatch().Draw(this.Texture, this.Position, this.Color);

                if ((this.Font != null) && !this.Text.Equals(String.Empty))
                {
                    Vector2 position = new Vector2(this.Position.X + 10, this.Position.Y + 10);
                    GraphicsSingleton.GetInstance().GetSpriteBatch().DrawString(this.Font, this.Text, position, this.ForeColor);
                }
            }
        }
    }
}
