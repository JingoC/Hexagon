using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonView.View
{
    using WinSystem;
    using WinSystem.Controls;
    using WinSystem.System;

    using HexagonLibrary.Model.GameMode;

    public class SettingsActivity : Activity
    {
        public SettingsActivity()
        {
            
        }

        public GameSettings GetSettings()
        {
            return new GameSettings() { CountPlayers = 2 };
        }
    }
}
