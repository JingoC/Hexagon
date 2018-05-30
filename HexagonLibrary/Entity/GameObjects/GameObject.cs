using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonLibrary.Entity.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using WinSystem.System;
    using WinSystem.Controls;

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

    public enum TypeFonts
    {
        TextHexagon = 1
    }

    public partial class GameObject
    {
        public static List<Texture2D> UserIdleTextures = new List<Texture2D>();
        public static List<Texture2D> UserActiveTextures = new List<Texture2D>();
        public static List<Texture2D> FieldTextures = new List<Texture2D>();
        public static List<SpriteFont> Fonts = new List<SpriteFont>();

        public static void LoadContent()
        {
            var content = GraphicsSingleton.GetInstance().Content as ContentManager;
            GameObject.UserIdleTextures.Add(content.Load<Texture2D>("hexagon_blue"));
            GameObject.UserIdleTextures.Add(content.Load<Texture2D>("hexagon_green"));
            GameObject.UserIdleTextures.Add(content.Load<Texture2D>("hexagon_red"));
            GameObject.UserIdleTextures.Add(content.Load<Texture2D>("hexagon_yellow"));

            GameObject.UserActiveTextures.Add(content.Load<Texture2D>("hexagon_blue_checked"));
            GameObject.UserActiveTextures.Add(content.Load<Texture2D>("hexagon_blue_checked"));

            GameObject.FieldTextures.Add(content.Load<Texture2D>("hexagon_gray"));
            GameObject.FieldTextures.Add(content.Load<Texture2D>("hexagon_white"));

            GameObject.Fonts.Add(content.Load<SpriteFont>("LifeFont"));
        }

        public static Texture2D GetTexture(TypeTexture typeTexture)
        {
            switch (typeTexture)
            {
                case TypeTexture.UserIdle0:
                case TypeTexture.UserIdle1:
                case TypeTexture.UserIdle2:
                case TypeTexture.UserIdle3: return GameObject.UserIdleTextures[(int)typeTexture];
                case TypeTexture.UserActive0:
                case TypeTexture.UserActive1:
                case TypeTexture.UserActive2:
                case TypeTexture.UserActive3: return GameObject.UserActiveTextures[(int)typeTexture - (int)TypeTexture.UserActive0];
                case TypeTexture.FieldFree:
                case TypeTexture.FieldMarked: return GameObject.FieldTextures[(int)typeTexture - (int)TypeTexture.FieldFree];
                default: throw new Exception("Texture not found");
            }
        }

        public static SpriteFont GetFont(TypeFonts typeFont)
        {
            switch(typeFont)
            {
                case TypeFonts.TextHexagon: return GameObject.Fonts[0];
                default: throw new Exception("Font not found");
            }
        }
    }

    public partial class GameObject : MonoObject
    {
        Texture2D defaultTexture;
        
        public Texture2D DefaultTexture
        {
            get => this.defaultTexture;
            set { this.defaultTexture = value; this.Texture = this.defaultTexture; }
        }

        virtual public string Text { get { return String.Empty; } }
        public SpriteFont Font { get; set; }
        public Color ForeColor { get; set; } = Color.Black;

        public GameObject()
        {
            
        }
        
        public void RestoreDefaultTexture()
        {
            this.Texture = this.DefaultTexture;
        }

        public override void Draw()
        {
            base.Draw();

            Vector2 position = new Vector2(this.Position.X + 10, this.Position.Y + 10);
            GraphicsSingleton.GetInstance().GetSpriteBatch().DrawString(this.Font, this.Text, position, this.ForeColor);
        }
    }
}
