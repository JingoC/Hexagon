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

#if false
            using (var winSystem = new WSystem())
            {
                Core core = new Core(new GameSettings() { MapSize = new Size() { Width = 10, Height = 10 } });
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

                Button newGameButton = new Button();
                newGameButton.Text = "New game";
                newGameButton.Position = new Vector2(600, 300);
                newGameButton.OnClick += (s, e) => core.Reset();
                winSystem.ActivitySelected.Items.Add(newGameButton);

                Button startModelButton = new Button();
                startModelButton.Text = "Modeling";
                startModelButton.Position = new Vector2(600, 240);
                startModelButton.OnClick += delegate (object sender, EventArgs e)
                {
                    System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
                    {
                        bool isGameOver = false;
                        while(!isGameOver)
                        {
                            int curStep = core.GameModeStrategy.Step;
                            System.Threading.Thread.Sleep(50);
                            core.GameModeStrategy.NextStep();
                            while (curStep == core.GameModeStrategy.Step) { }

                            List<int> items = new List<int>();
                            var allItems = core.GameModeStrategy.Map.Items.OfType<HexagonObject>()
                                        .Where((x) => x.BelongUser >= 0);
                            
                            foreach(var item in allItems)
                            {
                                if (items.All(x => x != item.BelongUser))
                                {
                                    items.Add(item.BelongUser);
                                    
                                    if (items.Count > 1)
                                        break;
                                }
                            }

                            isGameOver = items.Count == 1;
                        }
                    }));
                    th.Start();
                };
                winSystem.ActivitySelected.Items.Add(startModelButton);

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
                    string text = string.Format("Step: {1}{0}LootPoint0: {2} [{3}]{0}LootPoint1: {4} [{5}]{0}LootPoint2: {6} [{7}]{0}LootPoint3: {8} [{9}]{0}",
                        Environment.NewLine,
                        core.GameModeStrategy.Step,
#if MODEL
                        core.GameModeStrategy.CPUs[3].LootPoints,
                        core.GameModeStrategy.Map.Items.OfType<HexagonObject>()
                                    .Where<HexagonObject>((x) => x.BelongUser == core.GameModeStrategy.CPUs[3].ID)
                                    .Sum((x) => x.Loot),
#else
                        core.GameModeStrategy.User.LootPoints,
                        core.GameModeStrategy.Map.Items.OfType<HexagonObject>()
                                    .Where<HexagonObject>((x)=>x.BelongUser == core.GameModeStrategy.User.ID)
                                    .Sum((x) => x.Loot),
#endif
                        core.GameModeStrategy.CPUs[0].LootPoints,
                        core.GameModeStrategy.Map.Items.OfType<HexagonObject>()
                                    .Where<HexagonObject>((x) => x.BelongUser == core.GameModeStrategy.CPUs[0].ID)
                                    .Sum((x) => x.Loot),
                        core.GameModeStrategy.CPUs[1].LootPoints,
                        core.GameModeStrategy.Map.Items.OfType<HexagonObject>()
                                    .Where<HexagonObject>((x) => x.BelongUser == core.GameModeStrategy.CPUs[1].ID)
                                    .Sum((x) => x.Loot),
                        core.GameModeStrategy.CPUs[2].LootPoints,
                        core.GameModeStrategy.Map.Items.OfType<HexagonObject>()
                                    .Where<HexagonObject>((x) => x.BelongUser == core.GameModeStrategy.CPUs[2].ID)
                                    .Sum((x) => x.Loot)
                        );
                    labelInfo.Text = text;
                };

                winSystem.Graphics.Run();
            }
#else
            using (var winSystem = new View.GameView())
            {
                winSystem.Graphics.Run();
            }
#endif
        }
#endif
        }
}