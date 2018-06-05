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
        Core core;
        GameSettings gameSettings;

        Button endStepButton = new Button() { Name = "endStepButton", Text = "End Step" };
        Button nextStepButton = new Button() { Name = "nextStepButton", Text = "Next Step" };
        Button newGameButton = new Button() { Name = "newGameButton", Text = "New game" };
        Button startModelButton = new Button() { Name = "startModeButton", Text = "Modeling" };

        Label labelInfo = new Label() { Name = "labelInfo", ForeColor = Color.White };

        Button returnButton = new Button() { Text = "Return" };

        Container menu = new Container();

        public event EventHandler ExitActivity;

        public GameActivity()
        {
            this.endStepButton.OnClick += (s, e) => this.core.GameModeStrategy.EndStep();
            this.nextStepButton.OnClick += (s, e) => this.core.GameModeStrategy.NextStep();
            this.newGameButton.OnClick += (s, e) => this.core.Reset();
            this.startModelButton.OnClick += StartModelButton_OnClick;

            this.returnButton.OnClick += (s, e) => { if (this.ExitActivity != null) { this.ExitActivity(s, e); } };
            
            this.Items.Add(this.labelInfo);

            this.menu.Items.Add(this.endStepButton);
            this.menu.Items.Add(this.nextStepButton);
            this.menu.Items.Add(this.newGameButton);
            this.menu.Items.Add(this.startModelButton);
            this.menu.Items.Add(this.returnButton);

            this.Items.Add(this.menu);
        }

        public override void Designer()
        {
            base.Designer();

            this.labelInfo.Position = new Vector2(600, 700);

            this.returnButton.Position = new Vector2(10, 100);
            this.newGameButton.Position = new Vector2(10, this.returnButton.Position.Y + this.returnButton.Height + 20);
            this.endStepButton.Position = new Vector2(10, this.newGameButton.Position.Y + this.newGameButton.Height + 20);
            this.nextStepButton.Position = new Vector2(10, this.endStepButton.Position.Y + this.endStepButton.Height + 20);
            this.startModelButton.Position = new Vector2(10, this.nextStepButton.Position.Y + this.nextStepButton.Height + 20);

            int w = GraphicsSingleton.GetInstance().GetGraphics().PreferredBackBufferWidth;
            this.menu.Position = new Vector2(w - this.menu.Width - 10, 10);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            string nl = Environment.NewLine;

            if (this.core != null)
                core.Update();

#if false
            StringBuilder sb = new StringBuilder();
            foreach(var item in this.core.GameModeStrategy.Players)
            {
                int lootPoints = item.LootPoints;
                int id = item.ID;
                int sumLoot = core.GameModeStrategy.Map.Items.OfType<HexagonObject>().Where((x) => x.BelongUser == item.ID).Sum((x) => x.Loot);
                sb.AppendLine($"{id}, LootPoints: {lootPoints} [{sumLoot}]");
            }

            labelInfo.Text = $"Step: {core.GameModeStrategy.Step}{Environment.NewLine}{sb.ToString()}";
#else
            if (this.core.GameModeStrategy.GameSettings.PlayerMode == TypePlayerMode.Normal)
            {
                int Points = this.core.GameModeStrategy.User.LootPoints;
                int NewPoints = this.core.GameModeStrategy.Map.Items.OfType<HexagonObject>().Where((x) => x.BelongUser == 0).Sum((x) => x.Loot);
                labelInfo.Text = $"Step: {this.core.GameModeStrategy.Step}{Environment.NewLine}{Points} [{NewPoints}]";
            }
            else
            {
                labelInfo.Text = $"Step: {this.core.GameModeStrategy.Step}";
            }
#endif

            base.Update(gameTime);
        }

        private void StartModelButton_OnClick(object sender, EventArgs e)
        {
            if (this.gameSettings.PlayerMode == TypePlayerMode.Modeling)
            {
                System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
                {
                    bool isGameOver = false;
                    while (!isGameOver)
                    {
                        int curStep = core.GameModeStrategy.Step;
                        System.Threading.Thread.Sleep(this.core.GameModeStrategy.GameSettings.ModelStepTiming);
                        this.core.GameModeStrategy.NextStep();
                        while (curStep == this.core.GameModeStrategy.Step) { }

                        List<int> items = new List<int>();
                        var allItems = this.core.GameModeStrategy.Map.Items.OfType<HexagonObject>()
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
        }
        
        public void SetSettings(GameSettings settings)
        {
            this.gameSettings = settings;
            this.Items.Remove(this.core);
            this.core = new Core(settings);
            
            this.Items.Add(this.core);
        }

        public void Start()
        {
            this.core.Reset();
        }


    }
}
