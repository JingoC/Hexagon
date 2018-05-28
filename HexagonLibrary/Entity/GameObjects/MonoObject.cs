using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonLibrary.Entity.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    
    public enum TypeTexture
    {
        UserIdle0 = 0,
        UserIdle1 = 1,
        UserIdle2 = 2,
        UserIdle3 = 3,
        UserActive0 = 10,
        UserActive1 = 11,
        UserActive2 = 12,
        UserActive3 = 13,
        FieldFree = 20,
        FieldMarked = 21
    }

    public partial class MonoObject
    {
        public static List<Texture2D> UserIdleTextures = new List<Texture2D>();
        public static List<Texture2D> UserActiveTextures = new List<Texture2D>();
        public static List<Texture2D> FieldTextures = new List<Texture2D>();

        public static void LoadContent(Object Content)
        {
            var content = Content as ContentManager;
            MonoObject.UserIdleTextures.Add(content.Load<Texture2D>("hexagon_blue"));
            MonoObject.UserIdleTextures.Add(content.Load<Texture2D>("hexagon_green"));
            MonoObject.UserIdleTextures.Add(content.Load<Texture2D>("hexagon_red"));
            MonoObject.UserIdleTextures.Add(content.Load<Texture2D>("hexagon_yellow"));

            MonoObject.UserActiveTextures.Add(content.Load<Texture2D>("hexagon_blue_checked"));
            MonoObject.UserActiveTextures.Add(content.Load<Texture2D>("hexagon_blue_checked"));

            MonoObject.FieldTextures.Add(content.Load<Texture2D>("hexagon_gray"));
            MonoObject.FieldTextures.Add(content.Load<Texture2D>("hexagon_white"));
        }

        public static Texture2D GetTexture(TypeTexture typeTexture)
        {
            switch (typeTexture)
            {
                case TypeTexture.UserIdle0:
                case TypeTexture.UserIdle1:
                case TypeTexture.UserIdle2:
                case TypeTexture.UserIdle3: return MonoObject.UserIdleTextures[(int)typeTexture];
                case TypeTexture.UserActive0:
                case TypeTexture.UserActive1:
                case TypeTexture.UserActive2:
                case TypeTexture.UserActive3: return MonoObject.UserActiveTextures[(int)typeTexture - (int)TypeTexture.UserActive0];
                case TypeTexture.FieldFree:
                case TypeTexture.FieldMarked: return MonoObject.FieldTextures[(int)typeTexture - (int)TypeTexture.FieldFree];
                default: throw new Exception("Texture not found");
            }
        }
    }

    public partial class MonoObject
    {
        Vector2 position;
        Texture2D defaultTexture;
        
        public Texture2D DefaultTexture
        {
            get => this.defaultTexture;
            set { this.defaultTexture = value; this.Texture = this.defaultTexture; }
        }
        public Texture2D Texture { get; set; }
        public Color Color { get; set; } = Color.White;
        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                float x = this.position.X + this.Width / (this.Text.Length + 1) - 3;
                float y = this.position.Y + this.Height / 3;
                this.TextPositon = new Vector2(x, y);
            }
        }

        virtual public string Text { get { return String.Empty; } }
        public Vector2 TextPositon { get; private set; } = new Vector2(0, 0);

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
        
        public void RestoreDefaultTexture()
        {
            this.Texture = this.DefaultTexture;
        }
    }
}
