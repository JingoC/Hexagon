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
        UserIdle4 = 4,
        UserIdle5 = 5,
        UserIdle6 = 6,
        UserIdle7 = 7,
        UserIdle8 = 8,
        UserIdle9 = 9,
        UserActive0 = 20,
        FieldFree = 30,
        FieldMarked = 31
    }

    public enum TypeFonts
    {
        TextHexagon = 1
    }

    public partial class GameObject
    {
        public static Texture2D GetTexture(TypeTexture typeTexture)
        {
            switch (typeTexture)
            {
                case TypeTexture.UserIdle0: return Resources.GetResource("hexagon_aqua") as Texture2D;
                case TypeTexture.UserIdle1: return Resources.GetResource("hexagon_green") as Texture2D;
                case TypeTexture.UserIdle2: return Resources.GetResource("hexagon_red") as Texture2D;
                case TypeTexture.UserIdle3: return Resources.GetResource("hexagon_yellow") as Texture2D;
                case TypeTexture.UserIdle4: return Resources.GetResource("hexagon_brown") as Texture2D;
                case TypeTexture.UserIdle5: return Resources.GetResource("hexagon_purple") as Texture2D;
                case TypeTexture.UserIdle6: return Resources.GetResource("hexagon_limegreen") as Texture2D;
                case TypeTexture.UserIdle7: return Resources.GetResource("hexagon_purple") as Texture2D;
                case TypeTexture.UserIdle8: return Resources.GetResource("hexagon_blue") as Texture2D;
                case TypeTexture.UserIdle9: return Resources.GetResource("hexagon_white") as Texture2D;
                case TypeTexture.UserActive0: return Resources.GetResource("hexagon_aqua_checked") as Texture2D;
                case TypeTexture.FieldFree: return Resources.GetResource("hexagon_gray") as Texture2D;
                case TypeTexture.FieldMarked: return Resources.GetResource("hexagon_white") as Texture2D;
                default: throw new Exception("Texture not found");
            }
        }

        public static SpriteFont GetFont(TypeFonts typeFont)
        {
            switch(typeFont)
            {
                case TypeFonts.TextHexagon: return Resources.GetResource("defaultFont") as SpriteFont;
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

        public GameObject()
        {
            
        }
        
        public void RestoreDefaultTexture()
        {
            this.Texture = this.DefaultTexture;
        }

        
    }
}
