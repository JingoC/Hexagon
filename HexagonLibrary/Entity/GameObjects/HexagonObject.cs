﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace HexagonLibrary.Entity.GameObjects
{
    public enum TypeHexagon
    {
        Free = 0,
        Blocked = 1,
        Enemy = 2
    }

    public class HexagonObject : GameObject, IEquatable<HexagonObject>
    {
        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                this.TextPosition = new Vector2(value.X + this.Width / 4, value.Y + this.Height / 4);
            }
        }
        public static HexagonObject Empty { get { return new HexagonObject(); } }

        public int SectorId { get; set; }
        public int MaxLife { get; set; } = 100;
        public int Life { get; set; } = 0;
        public int BelongUser { get; set; } = -1;
        public TypeHexagon Type { get; set; } = TypeHexagon.Free;
        public int Loot { get; set; } = 0;
        public int Level { get; set; } = 0;

        public override string Text => this.Type != TypeHexagon.Blocked ? this.Life.ToString() : String.Empty;

        public HexagonObject() : this(0)
        {
            
        }
        
        public HexagonObject(int sectorId)
        {
            this.SectorId = sectorId;
            this.Designer();
        }

        public override void Designer()
        {
            base.Designer();
        }

        public bool Equals(HexagonObject other)
        {
            return (other != null) && (this.SectorId == other.SectorId);
        }
    }
}
