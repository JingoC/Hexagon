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

    using HexagonLibrary;
    using HexagonLibrary.Model;
    using HexagonLibrary.Model.GameMode;
    using HexagonLibrary.Entity.GameObjects;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    public class GameActivity : Activity
    {
        Core core = new Core(new GameSettings());

        Button endStepButton = new Button() { Name = "endStepButton", Text = "End Step" };
        Button nextStepButton = new Button() { Name = "nextStepButton", Text = "Next Step" };
        Button newGameButton = new Button() { Name = "newGameButton", Text = "New game" };
        Button startModelButton = new Button() { Name = "startModeButton", Text = "Modeling" };

        Label labelInfo = new Label() { Name = "labelInfo", ForeColor = Color.White };

        public GameActivity()
        {
            Core core = new Core(new GameSettings() { MapSize = new Size() { Width = 10, Height = 10 } });
            
            endStepButton.OnClick += (s, e) => core.GameModeStrategy.EndStep();
            nextStepButton.OnClick += (s, e) => core.GameModeStrategy.NextStep();
            newGameButton.OnClick += (s, e) => this.core.Reset();
            startModelButton.OnClick += StartModelButton_OnClick;

            this.Items.Add(endStepButton);
            this.Items.Add(nextStepButton);
            this.Items.Add(newGameButton);        
            this.Items.Add(startModelButton);
            this.Items.Add(labelInfo);
            
            this.Items.Add(this.core);
        }

        public override void Update(GameTime gameTime)
        {
            string nl = Environment.NewLine;

            if (this.core != null)
                core.Update();
            
            StringBuilder sb = new StringBuilder();
            foreach(var item in this.core.GameModeStrategy.Players)
            {
                int lootPoints = item.LootPoints;
                int id = item.ID;
                int sumLoot = core.GameModeStrategy.Map.Items.OfType<HexagonObject>().Where((x) => x.BelongUser == item.ID).Sum((x) => x.Loot);
                sb.AppendLine($"{id}, LootPoints: {lootPoints} [{sumLoot}]");
            }

            labelInfo.Text = $"Step: {core.GameModeStrategy.Step}{Environment.NewLine}{sb.ToString()}";


            base.Update(gameTime);
        }

        private void StartModelButton_OnClick(object sender, EventArgs e)
        {
            System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
            {
                bool isGameOver = false;
                while (!isGameOver)
                {
                    int curStep = core.GameModeStrategy.Step;
                    System.Threading.Thread.Sleep(50);
                    core.GameModeStrategy.NextStep();
                    while (curStep == core.GameModeStrategy.Step) { }

                    List<int> items = new List<int>();
                    var allItems = core.GameModeStrategy.Map.Items.OfType<HexagonObject>()
                                .Where((x) => x.BelongUser >= 0);

                    foreach (var item in allItems)
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
        }

        public override void Designer()
        {
            this.endStepButton.Position = new Vector2(600, 20);
            this.nextStepButton.Position = new Vector2(600, 60);
            this.startModelButton.Position = new Vector2(600, 240);
            this.newGameButton.Position = new Vector2(600, 300);
            this.labelInfo.Position = new Vector2(600, 100);

            base.Designer();
        }

        public void SetSettings(GameSettings settings)
        {
            this.Items.Remove(this.core);
            this.core = new Core(settings);
            this.core.LoadContent();
            this.Items.Add(this.core);
        }

        public void Start()
        {
            this.core.Reset();
        }
    }
}
