using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using WinSystem;
using WinSystem.Controls;
using HexagonLibrary;
using HexagonLibrary.Entity.GameObjects;
using HexagonLibrary.Model.GameMode;

namespace HexagonWin
{
    using HexagonView;
    using WinSystem.System;
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Resources.LoadResource();
            using (var winSystem = new GameView())
            {
                winSystem.Graphics.Run();
            }
        }
    }
#endif
}