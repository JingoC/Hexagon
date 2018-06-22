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
        BuildMap = 2,
        Modeling = 3
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
        public Size MapSize { get; set; } = new Size() { Width = 10, Height = 10 };
        public int CountPlayers { get; set; } = 4;
        public TypeGameMode GameMode { get; set; } = TypeGameMode.Normal;
        public int ModelStepTiming { get; set; } = 50;
        public Range ScatterLoot { get; set; } = new Range(0, 5);
        public Range ScatterLife { get; set; } = new Range(0, 5);
        public bool IsAllEqualLife { get; set; } = false;
        public Range ScatterMaxLife { get; set; } = new Range(4, 8);
        public bool ViewLootEnable { get; set; } = true;
        public bool ViewLifeEnable { get; set; } = true;
        public bool ViewMaxLife { get; set; } = true;
        public int PercentBonus { get; set; } = 20;
        public int PercentBlocked { get; set; } = 15;

        public GameSettings()
        {

        }
    }
}
