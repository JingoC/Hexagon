using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonLibrary.Entity.GameObjects
{
    public class HexagonObject : GameObject
    {
        public static HexagonObject Empty { get { return new HexagonObject(); } }

        public int Life { get; set; } = 0;
        public int SectorId { get; set; }
        public int BelongUser { get; set; } = -1;

        public override string Text => this.Life.ToString();

        public HexagonObject() : this(0)
        {

        }

        public HexagonObject(int sectorId)
        {
            this.SectorId = sectorId;
        }
    }
}
