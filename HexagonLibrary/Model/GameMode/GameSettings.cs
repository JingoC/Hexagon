﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Model.GameMode
{
    public enum TypeGameMode
    {
        Normal = 1,
        BuildMap = 2
    }

    public class Size
    {
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
    }

    public class GameSettings
    {
        public Size MapSize { get; set; } = new Size() { Width = 10, Height = 10 };
        public int NumberEnemies { get; set; } = 3;
        public TypeGameMode GameMode { get; set; } = TypeGameMode.Normal;

        public GameSettings()
        {

        }
    }
}