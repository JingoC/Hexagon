using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Entity.Players
{
    public class Player
    {
        public static int LootPointForCreate { get; set; } = 4;
        public static int LootPointForDestroy { get; set; } = 4;

        private int lootPoints = 0;

        public int ID { get; set; } = 0;

        public int LootPoints
        {
            get => this.lootPoints;
            set
            {
                this.lootPoints = value;
                if (this.LootPointsChanged != null)
                    this.LootPointsChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler LootPointsChanged;

        public Player()
        {

        }

        public bool IsAccessToCreate()
        {
            return (this.lootPoints / Player.LootPointForCreate) > 0;
        }
    }
}
