﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonView.Controls
{
    using Microsoft.Xna.Framework.Graphics;

    using WinSystem.System;
    using WinSystem.Controls;
    using Microsoft.Xna.Framework;

    public class GameButton : Button
    {
        protected override Vector2 TextPosition
        {
            get
            {
                if (this.Texture != null)
                {
                    float tx = this.Position.X;
                    float ty = this.Position.Y;
                    float tw = this.Width;
                    float th = this.Height;

                    float w = (int)this.Font.MeasureString(this.Text).X;
                    float h = this.Font.GetGlyphs().First(v => v.Value.Character == '0').Value.BoundsInTexture.Height;

                    float x = tx + (tw / 2) - (w / 2);
                    float y = ty + (th / (1.5f * 2)) - (h / 2);

                    return new Vector2(x, y);
                }

                return new Vector2(-100, -100);
            }
        }

        public GameButton() : base()
        {
            base.Scale = 1f;
        }

        public override void Designer()
        {
            if (this.Texture == null)
            {
                this.TextureManager.Textures.AddRange(Resources.GetResources(new List<string>()
                {
                    "gameBtn1",
                    "gameBtn2",
                    "gameBtn3",
                    "gameBtn4",
                    "gameBtn5"
                }).OfType<Texture2D>());
            }

            if (this.Font == null)
            {
                this.TextureManager.Fonts.Add(Resources.GetResource("gameBtnFont") as SpriteFont);
            }

            base.Designer();
        }
        
    }
}