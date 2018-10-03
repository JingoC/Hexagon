using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Model.GameMode
{
    public enum TypeGameMode
    {
        Normal = 1,
        Modeling = 2
    }

    public class Size
    {
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
    }

    public class Range
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public Range(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }
    }

    [Serializable]
    public class GameSettings
    {
        // Main game setting
        public Size MapSize { get; set; } = new Size() { Width = 10, Height = 10 };
        public int CountPlayers { get; set; } = 4;
        public TypeGameMode GameMode { get; set; } = TypeGameMode.Normal;
        public int ModelStepTiming { get; set; } = 50;

        // View settings
        public float ScaleHexagon { get; set; } = 1.5f;
        public bool ViewLootEnable { get; set; } = true;
        public bool ViewLifeEnable { get; set; } = true;
        public bool ViewMaxLife { get; set; } = true;
        public int DelayOnStep { get; set; } = 0;
        public int DelayOnAction { get; set; } = 10;
        
        // Hexagon settings
        public Range ScatterLoot { get; set; } = new Range(0, 5);
        public Range ScatterLife { get; set; } = new Range(0, 5);
        public Range ScatterMaxLife { get; set; } = new Range(4, 8);
        public bool IsAllSameMaxLife { get; set; } = true;

        // Map settings
        public int PercentBonus { get; set; } = 20;
        public int PercentBlocked { get; set; } = 15;
        public int LootPointForCreate { get; set; } = 8;

        public GameSettings()
        {

        }
    }
}
