using System;

using Microsoft.Xna.Framework;

using WinSystem;
using WinSystem.Controls;
using HexagonLibrary;

namespace HexagonWin
{
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
            using (var winSystem = new WSystem())
            {
                Core core = new Core();
                Activity coreActivity = new Activity();
                
                winSystem.ActivitySelected.Items.Add(core);

                winSystem.LoadContentEvent += delegate (object sender, EventArgs e)
                {
                    core.LoadContent();
                };

                winSystem.UpdateEvent += delegate (object sender, EventArgs e)
                {
                    core.Update();
                };

                winSystem.Graphics.Run();
            }
        }
#endif
    }
}