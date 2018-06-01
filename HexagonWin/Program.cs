using System;

using Microsoft.Xna.Framework;

using WinSystem;
using WinSystem.Controls;
using HexagonLibrary;
using HexagonLibrary.Entity.GameObjects;
using HexagonLibrary.Model.GameMode;

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
                Core core = new Core(new GameSettings());
                Activity coreActivity = new Activity();
                                
                winSystem.ActivitySelected.Items.Add(core);

                Button endStepButton = new Button();
                endStepButton.Text = "End Step";
                endStepButton.Position = new Vector2(600, 20);
                endStepButton.OnClick += (s, e)=> core.GameModeStrategy.EndStep();
                winSystem.ActivitySelected.Items.Add(endStepButton);

                Button nextStepButton = new Button();
                nextStepButton.Text = "Next Step";
                nextStepButton.Position = new Vector2(600, 60);
                nextStepButton.OnClick += (s, e) => core.GameModeStrategy.NextStep();
                winSystem.ActivitySelected.Items.Add(nextStepButton);

                string nl = Environment.NewLine;
                var labelInfo = new Label();
                labelInfo.Name = "labelInfo";
                labelInfo.Position = new Vector2(600, 100);
                labelInfo.ForeColor = Color.White;
                winSystem.ActivitySelected.Items.Add(labelInfo);

                winSystem.LoadContentEvent += delegate (object sender, EventArgs e)
                {
                    core.LoadContent();
                };

                winSystem.UpdateEvent += delegate (object sender, EventArgs e)
                {
                    core.Update();
                    string text = string.Format("Step: {1}{0}LootPoint: {2}{0}",
                        Environment.NewLine,
                        core.GameModeStrategy.Step,
                        core.GameModeStrategy.User.LootPoints);
                    labelInfo.Text = text;
                };

                winSystem.Graphics.Run();
            }
        }
#endif
    }
}