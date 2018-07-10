using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGuiFramework.Base;
using MonoGuiFramework.System;

namespace HexagonLibrary.Entity.GameObjects
{
    public enum TypeHexagon
    {
        Free = 0,
        Blocked = 1,
        Enemy = 2
    }

    public enum TypeHexagonBonus
    {
        None = 0,
        Bomb = 1,
        Last = 1
    }

    public class HexagonObject : GameObject, IEquatable<HexagonObject>
    {
        static public bool LifeEnable { get; set; } = true;
        static public bool LootEnable { get; set; } = true;
        static public bool MaxLifeEnable { get; set; } = true;
        static public float ScaleHexagon { get; set; } = 0.15f;

        public override Position Position
        {
            get => base.Position;
            protected set
            {
                base.Position = value;
                this.TextPosition = new Vector2(value.Absolute.X + this.Width / 4 + 1, value.Absolute.Y + this.Height / 4 - 1);
            }
        }
        public static HexagonObject Empty { get { return new HexagonObject(); } }

        public int SectorId { get; set; }
        public int MaxLife { get; set; } = 8;
        public int Life { get; set; } = 0;
        public int BelongUser { get; set; } = -1;
        public TypeHexagon Type { get; set; } = TypeHexagon.Free;
        public int Loot { get; set; } = 0;
        public int Level { get; set; } = 0;
        public TypeHexagonBonus Bonus { get; set; } = TypeHexagonBonus.None;
        public override string Text
        {
            get
            {
                if (this.Bonus == TypeHexagonBonus.None)
                {
                    string c00 = HexagonObject.LifeEnable ? $"{this.Life}" : " ";
                    string c01 = HexagonObject.LootEnable ? $"   {this.Loot}" : String.Empty;
                    string c10 = HexagonObject.MaxLifeEnable ? $"{this.MaxLife}" : " ";
                    string c11 = true ? String.Empty : $"   {this.BelongUser}";
                    return $"{c00}{c01}{Environment.NewLine}{c10}{c11}";
                }

                return String.Empty;
            }
        }

        public HexagonObject() : this(0)
        {
            
        }
        
        public HexagonObject(int sectorId)
        {
            this.SectorId = sectorId;
            this.Scale = HexagonObject.ScaleHexagon;
            this.Designer();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var font = this.TextureManager.Fonts.Current;
            if (font != null)
            {
                var measure = font.MeasureString(this.Text);
                int x = (int)(this.Width / 2 - measure.X / 2 + this.Position.Absolute.X);
                int y = (int)(this.Height / 2 - measure.Y / 2 + this.Position.Absolute.Y);

                Vector2 position = new Vector2(x, y);
                SpriteBatch.DrawString(this.TextureManager.Fonts.Current, this.Text, position, Color.Black);
            }
        }

        public override void Designer()
        {
            if (this.TextureManager.Fonts.Current == null)
                this.TextureManager.Fonts.Add(Resources.GetResource("defaultFont") as SpriteFont);
            base.Designer();
        }

        public bool Equals(HexagonObject other)
        {
            return (other != null) && (this.SectorId == other.SectorId);
        }
    }
}
