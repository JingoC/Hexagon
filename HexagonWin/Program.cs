using System;

using Microsoft.Xna.Framework;

using WinSystem;
using WinSystem.Controls;
using HexagonLibrary;
using HexagonLibrary.Entity.GameObjects;

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

                Button endStepButton = new Button();
                endStepButton.Text = "End Step";
                endStepButton.Position = new Vector2(600, 20);
                endStepButton.Click += (s, e)=> core.EndStep();
                winSystem.ActivitySelected.Items.Add(endStepButton);

                Button nextStepButton = new Button();
                nextStepButton.Text = "Next Step";
                nextStepButton.Position = new Vector2(600, 60);
                nextStepButton.Click += (s, e) => core.NextStep();
                winSystem.ActivitySelected.Items.Add(nextStepButton);

                winSystem.LoadContentEvent += delegate (object sender, EventArgs e)
                {
                    core.LoadContent();

                    endStepButton.Font = GameObject.GetFont(TypeFonts.TextHexagon);
                    endStepButton.Texture = GameObject.GetTexture(TypeTexture.SysButton);
                    nextStepButton.Font = GameObject.GetFont(TypeFonts.TextHexagon);
                    nextStepButton.Texture = GameObject.GetTexture(TypeTexture.SysButton);
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