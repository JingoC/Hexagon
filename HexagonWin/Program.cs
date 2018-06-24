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
            System.Windows.Forms.Form cursorPos = new System.Windows.Forms.Form();
            cursorPos.MinimizeBox = false;
            cursorPos.MaximizeBox = false;
            cursorPos.Show();
            cursorPos.SetBounds(1340, 0, 10, 50);

            

            using (var winSystem = new GameView())
            {
                winSystem.Graphics.GetGraphics().PreferredBackBufferWidth = 1300;
                winSystem.Graphics.GetGraphics().PreferredBackBufferHeight = 800;
                winSystem.Graphics.GetGraphics().ApplyChanges();
                
                winSystem.Input.PositionChangedMouse += (s, e) => cursorPos.Text = $"{e.X}:{e.Y}";
                winSystem.Exit += (s, e) => Environment.Exit(0);

                winSystem.Run();
            }
        }
    }
#endif
}