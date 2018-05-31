using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Model.GameMode
{
    public class GameModeBuildMap : GameModeStrategy
    {
        public GameModeBuildMap() : this(new GameSettings())
        {

        }

        public GameModeBuildMap(GameSettings gameSettings) : base(gameSettings)
        {

        }
    }
}
