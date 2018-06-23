using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Model.GameMode
{
    public static class GameModeFactory
    {
        static public GameModeStrategy Create(GameSettings gameSettings)
        {
            switch(gameSettings.GameMode)
            {
                case TypeGameMode.Normal: return new GameModeNormal(gameSettings);
                case TypeGameMode.Modeling: return new GameModeModeling(gameSettings);
                default: throw new Exception("Type GameMode undefined");
            }
        }
    }
}
