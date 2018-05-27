using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonLibrary.Entity.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    public class MonoObject
    {
        Vector2 position;

        public static List<Texture2D> UserIdleTextures = new List<Texture2D>();
        public static List<Texture2D> UserActiveTextures = new List<Texture2D>();
        public static List<Texture2D> FieldTextures = new List<Texture2D>();

        public Texture2D Texture { get; set; }
        public Color Color { get; set; } = Color.White;
        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                float x = this.position.X + this.Width / (this.Text.Length + 1);
                float y = this.position.Y + this.Height / 3;
                this.TextPositon = new Vector2(x, y);
            }
        }

        virtual public string Text { get { return String.Empty; } }
        public Vector2 TextPositon { get; set; } = new Vector2(0, 0);

        public int Width { get { return this.Texture.Width; } }
        public int Height { get { return this.Texture.Height; } }

        public MonoObject()
        {
        }

        public bool IsIntersection(float x, float y)
        {
            float x1 = this.Position.X;
            float x2 = this.Position.X + this.Width;
            float y1 = this.Position.Y;
            float y2 = this.Position.Y + this.Height;

            return ((x > x1) && (x < x2) && (y > y1) && (y < y2));
        }

    }
}
