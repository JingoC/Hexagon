﻿using System;
using System.Linq;
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
        UserActive0 = 10,
        FieldFree = 11,
        FieldMarked = 12,
        LastTexture = 12
    }
    
    public partial class GameObject : MonoObject
    {
        //public override Vector2 Position { get; set; }

        public GameObject()
        {
            
        }

        public override void Designer()
        {
            this.TextureManager.Textures.AddRange(Resources.GetResources(new List<string> {
                "hexagon_aqua",
                "hexagon_green",
                "hexagon_red",
                "hexagon_yellow",
                "hexagon_brown",
                "hexagon_purple",
                "hexagon_limegreen",
                "hexagon_orange",
                "hexagon_blue",
                "hexagon_white",
                "hexagon_aqua_checked",
                "hexagon_gray",
                "hexagon_white"
            }).OfType<Texture2D>());

            base.Designer();
        }

        public void RestoreDefaultTexture()
        {
            this.TextureManager.Textures.RestoreDefault();
        }

        public void SetDefaultTexture(TypeTexture typeTexture)
        {
            this.TextureManager.Textures.SetDefault((int)typeTexture);
            this.RestoreDefaultTexture();
        }
        
    }
}
