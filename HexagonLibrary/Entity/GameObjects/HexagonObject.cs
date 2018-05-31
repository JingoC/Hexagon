using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonLibrary.Entity.GameObjects
{
    public enum TypeHexagon
    {
        User = 1,
        Free = 2,
        Blocked = 3,
        Enemy = 4
    }

    public class HexagonObject : GameObject, IEquatable<HexagonObject>
    {
        public static HexagonObject Empty { get { return new HexagonObject(); } }

        public int SectorId { get; set; }
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
        }

        public bool Equals(HexagonObject other)
        {
            return (other != null) && (this.SectorId == other.SectorId);
        }
    }
}
