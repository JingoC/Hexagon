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
    using HexagonLibrary.Entity.Players;
    using HexagonLibrary.Model;
    using HexagonLibrary.Model.GameMode;
    using HexagonLibrary.Entity.GameObjects;

    using HexagonView.Controls;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    class GameStatus
    {
        public int Step { get; set; } = 0;
        public int Created { get => this.Point / Player.LootPointForCreate; }
        public int Destroy { get => this.Point / Player.LootPointForCreate; }

        public int Point { get; set; } = 0;

        public string Log { get; set; }

        public override string ToString()
        {
            return $"Step: {this.Step}   Point: {this.Point}   Created: {this.Created}   Destroy: {this.Destroy}";
        }
    }

    public class GameActivity : Activity
    {
        Core core;
        GameSettings gameSettings;

        Label endMessageLabel = new Label() { Text = String.Empty, ForeColor = Color.Green, Visible = false, ZIndex = 1 };

        StatisticLine balanceLineSectors;
        StatisticLine balanceLineLife;

        GameStatus gameStatus = new GameStatus();
        Label statusLabel = new Label() { Text = String.Empty, ForeColor = Color.White };
        Container gameStatusContainer = new Container() { };

        GameButton gameControlButton = new GameButton() { Name = "endStepButton", Text = "", ForeColor = Color.Black };
        GameButton newGameButton = new GameButton() { Name = "newGameButton", Text = "N", ForeColor = Color.Black };
        GameButton autoAllocationButton = new GameButton() { Name = "autoAllocationButton", Text = "A", ForeColor = Color.Black };
        GameButton startModelButton = new GameButton() { Name = "startModeButton", Text = "M", ForeColor = Color.Black };

        Container menu = new Container();
        
        int stateStepBtn = 0;

        public GameActivity(Activity parent) : base(parent)
        {
            this.gameControlButton.OnClick += GameControlButton_OnClick;

            this.newGameButton.OnClick += (s, e) => { this.Start(); };
            this.startModelButton.OnClick += StartModelButton_OnClick;
            this.autoAllocationButton.OnClick += this.AutoAllocationButton_OnClick;

            this.gameStatusContainer.Items.Add(this.statusLabel);

            this.menu.Items.Add(this.gameControlButton);
            this.menu.Items.Add(this.newGameButton);
            this.menu.Items.Add(this.autoAllocationButton);
            this.menu.Items.Add(this.startModelButton);
            
            this.Items.Add(this.endMessageLabel);
            this.Items.Add(this.gameStatusContainer);
            this.Items.Add(this.menu);
        }

        private void AutoAllocationButton_OnClick(object sender, EventArgs e)
        {
            if (stateStepBtn == 1)
            {
                this.core.GameModeStrategy.LootPointAutoAllocate();
            }
        }

        private void GameControlButton_OnClick(object sender, EventArgs e)
        {
            if (this.core.GameModeStrategy.GameState != GameModeState.Play)
                return;

            if (stateStepBtn < 2)
            {
                void LootChangedHandler(Object obj, EventArgs ea)
                {
                    int lp = this.core.GameModeStrategy.User.LootPoints;
                    this.gameStatus.Point = lp;
                    this.statusLabel.Text = this.gameStatus.ToString();

                    if (lp == 0) EndLootAllocation(); else this.gameControlButton.Text = lp.ToString();
                };

                void FinishHandler(Object obj, EventArgs ea)
                {
                    this.gameControlButton.TextureManager.Textures.Change(0);
                    this.core.GameModeStrategy.FinishCpuStep -= FinishHandler;
                    stateStepBtn = 0;

                    var state = this.core.GameModeStrategy.GameState;
                    if (state != GameModeState.Play)
                    {
                        this.endMessageLabel.ForeColor = state == GameModeState.Win ? Color.Green : Color.Red;
                        this.endMessageLabel.Text = state == GameModeState.Win ? "You Win!" : "You Lose!";
                        this.endMessageLabel.Visible = true;
                    }
                };

                void EndLootAllocation()
                {
                    this.core.GameModeStrategy.User.LootPointsChanged -= LootChangedHandler;
                    this.gameControlButton.Text = String.Empty;
                    this.gameControlButton.TextureManager.Textures.Change(2);

                    this.core.GameModeStrategy.FinishCpuStep += FinishHandler;
                    this.core.GameModeStrategy.NextStep();

                    this.gameStatus.Step++;
                    this.statusLabel.Text = this.gameStatus.ToString();

                    this.autoAllocationButton.Visible = false;
                    stateStepBtn = 2;

                    this.UpdateBalance();
                }

                switch (stateStepBtn)
                {
                    // loot allocation
                    case 0:
                    {
                        this.core.GameModeStrategy.User.LootPointsChanged += LootChangedHandler;
                        this.core.GameModeStrategy.EndStep();
                        this.gameControlButton.TextureManager.Textures.Change(1);
                        this.autoAllocationButton.Visible = true;
                        stateStepBtn = 1;
                    }
                    break;
                    // end loot allocation
                    case 1: EndLootAllocation(); break;
                    default: break;
                }
            }
        }

        public override void ChangeActivity(bool active)
        {
            if (active)
            {
                this.core.GameModeStrategy.Resume();
            }
            else
            {
                this.cancelModeling = true;
                this.core.GameModeStrategy.Suspend();
            }

            base.ChangeActivity(active);
        }

        public override void Designer()
        {
            this.balanceLineSectors = new StatisticLine(this.Width, 10);
            this.balanceLineSectors.ZIndex = 1;

            this.balanceLineLife = new StatisticLine(this.Width, 10);
            this.balanceLineLife.ZIndex = 1;
            this.gameStatusContainer.Items.Add(this.balanceLineSectors);
            this.gameStatusContainer.Items.Add(this.balanceLineLife);

            base.Designer();

            this.newGameButton.TextureManager.Textures.Change(3);
            this.autoAllocationButton.TextureManager.Textures.Change(0);
            this.startModelButton.TextureManager.Textures.Change(4);
            
            this.statusLabel.Position = new Vector2(5, 2);
            this.balanceLineSectors.Position = new Vector2(2, this.statusLabel.Height + this.statusLabel.Position.Y + 10);
            this.balanceLineLife.Position = new Vector2(2, this.balanceLineSectors.Height + this.balanceLineSectors.Position.Y);
            this.gameStatusContainer.Position = new Vector2(0, 0);

            this.newGameButton.Position = new Vector2(10, 10);
            this.gameControlButton.Position = new Vector2(10, this.newGameButton.Position.Y + this.newGameButton.Height + 40);
            this.autoAllocationButton.Position = new Vector2(10, this.gameControlButton.Position.Y + this.gameControlButton.Height + 40);
            this.startModelButton.Position = new Vector2(10, this.autoAllocationButton.Position.Y + this.autoAllocationButton.Height + 40);

            int w = GraphicsSingleton.GetInstance().GetGraphics().PreferredBackBufferWidth;
            int h = GraphicsSingleton.GetInstance().GetGraphics().PreferredBackBufferHeight;

            this.menu.Position = new Vector2(w - this.menu.Width - 20, h / 2 - this.menu.Height / 2);

            this.endMessageLabel.Text = "You Win!";
            this.endMessageLabel.TextureManager.Fonts.Add(Resources.GetResource("endMessageFont") as SpriteFont);
            this.endMessageLabel.TextureManager.Fonts.Change(this.endMessageLabel.TextureManager.Fonts.Count() - 1);
            this.endMessageLabel.Position = new Vector2(this.Width / 2 - this.endMessageLabel.Width / 2,
                this.Height / 2 - this.endMessageLabel.Height / 2);
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

            base.Update(gameTime);
        }

        private bool cancelModeling;
        private void StartModelButton_OnClick(object sender, EventArgs e)
        {
            if (this.startModelButton.Text.Equals("M"))
            {
                this.cancelModeling = false;
                this.startModelButton.Text = "S";
                this.newGameButton.Visible = false;
                if (this.gameSettings.GameMode == TypeGameMode.Modeling)
                {
                    System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
                    {
                        bool isGameOver = false;
                        while (!isGameOver)
                        {
                            this.UpdateBalance();
                            int curStep = core.GameModeStrategy.Step;

                            this.gameStatus.Step = curStep;
                            this.statusLabel.Text = this.gameStatus.ToString();

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

                            isGameOver = (items.Count == 1) || this.cancelModeling;
                        }

                        this.newGameButton.Visible = true;
                        this.startModelButton.Text = "M";
                    }));
                    th.Start();
                }
            }
            else
            {
                this.cancelModeling = true;
            }
        }

        public void SetSettings(GameSettings settings)
        {
            this.gameSettings = settings;
            this.Items.Remove(this.core);
            this.core = new Core(settings);

            this.Start();

            switch (settings.GameMode)
            {
                case TypeGameMode.Normal:
                {
                    this.startModelButton.Visible = false;
                    this.gameControlButton.Visible = true;
                }
                break;
                case TypeGameMode.Modeling:
                {
                    this.startModelButton.Visible = true;
                    this.gameControlButton.Visible = false;
                }
                break;
            }

            this.Items.Add(this.core);
        }
        
        public void Start()
        {
            this.core.Reset();
            
            this.autoAllocationButton.Visible = false;

            this.gameStatus = new GameStatus();
            this.statusLabel.Text = this.gameStatus.ToString();
            this.core.Position = new Vector2(10, this.gameStatusContainer.Position.Y + this.gameStatusContainer.Height + 15);

            this.gameControlButton.TextureManager.Textures.Change(0);
            this.gameControlButton.Text = String.Empty;
            this.stateStepBtn = 0;

            this.endMessageLabel.Visible = false;

            this.balanceLineLife.Items.Clear();
            this.balanceLineSectors.Items.Clear();
            this.balanceLineSectors.Dispose();
            this.balanceLineLife.Dispose();
            for (int i = 0; i < this.gameSettings.CountPlayers; i++)
            {
                this.balanceLineSectors.Items.Add(new StatisticObject() { Name = $"{i}", Value = 1, Color = HexagonObject.Colors[i] });
                this.balanceLineLife.Items.Add(new StatisticObject() { Name = $"{i}", Value = 2, Color = HexagonObject.Colors[i] });
            }
            this.UpdateBalance();
        }

        private void UpdateBalance()
        {
            int count = 0;
            var hexs = this.core.GameModeStrategy.Map.Items.OfType<HexagonObject>();

            foreach (var p in this.core.GameModeStrategy.Players)
            {
                this.balanceLineSectors.Items[count].Value = hexs.Count(x => x.BelongUser == p.ID);
                this.balanceLineLife.Items[count].Value = hexs.Where(x => x.BelongUser == p.ID).Sum(x => x.Life) + p.LootPoints;
                count++;
            }
        }
    }
}
