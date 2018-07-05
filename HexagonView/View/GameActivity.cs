using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonView.View
{
    using MonoGuiFramework;
    using MonoGuiFramework.Controls;
    using MonoGuiFramework.System;

    using HexagonLibrary;
    using HexagonLibrary.Entity.Players;
    using HexagonLibrary.Model;
    using HexagonLibrary.Model.GameMode;
    using HexagonLibrary.Entity.GameObjects;

    using HexagonView.Controls;
    using MonoGuiFramework.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using MonoGuiFramework.Containers;

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

        Label endMessageLabel;

        StatisticLine balanceLineSectors;
        StatisticLine balanceLineLife;

        GameStatus gameStatus = new GameStatus();
        Label statusLabel;

        GameButton gameControlButton;
        GameButton newGameButton;
        GameButton autoAllocationButton;
        GameButton startModelButton;

        Container menu = new Container();
        
        int stateStepBtn = 0;

        public GameActivity(Activity parent) : base(parent)
        {
            this.Scrollable = true;
            this.gameStatus = new GameStatus();

            this.Designer();
        }

        public override void Designer()
        {
            VerticalContainer rows = new VerticalContainer(this) { BorderColor = Color.Red };
            this.Items.Add(rows);

            VerticalContainer top = new VerticalContainer(rows) { Position = new Position(4, 5), BorderColor = Color.Green };
            rows.Items.Add(top);

            HorizontalContainer content = new HorizontalContainer(rows) { BorderColor = Color.Blue, Position = new Position(0, 0) };
            rows.Items.Add(content);

            HorizontalContainer content_game = new HorizontalContainer(content) { BorderColor = Color.Yellow };
            content_game.SetBounds(0, 10, (int)(this.Width * 0.8), (int)(this.Height * 0.7));
            content.Items.Add(content_game);
            
            VerticalContainer content_menu = new VerticalContainer(content) { BorderColor = Color.Aqua };
            content.Items.Add(content_menu);
            
            this.statusLabel = new Label()
            {
                ForeColor = Color.White,
                Text = "Status",
                Position = new Position(10, 4)
            };
            top.Items.Add(this.statusLabel);

            this.balanceLineSectors = new StatisticLine(this.Width, 10) { DrawOrder = 1, Position = new Position(10, 4) };
            top.Items.Add(this.balanceLineSectors);

            this.balanceLineLife = new StatisticLine(this.Width, 10) { DrawOrder = 1, Position = new Position(10, 0) };
            top.Items.Add(this.balanceLineLife);

            this.core = new Core(new GameSettings());
            content_game.Items.Add(this.core);

            this.newGameButton = new GameButton()
            {
                Name = "newGameButton",
                Text = "N", ForeColor = Color.Black,
                Position = new Position(0, 10),
            };
            this.newGameButton.TextureManager.Textures.Change(3);
            this.newGameButton.OnClick += (s, e) => this.Start();
            content_menu.Items.Add(this.newGameButton);
            
            this.gameControlButton = new GameButton()
            {
                Name = "endStepButton",
                Text = String.Empty,
                ForeColor = Color.Black,
                Position = new Position(0, 10)
            };
            this.gameControlButton.OnClick += GameControlButton_OnClick;
            content_menu.Items.Add(this.gameControlButton);
            
            this.autoAllocationButton = new GameButton()
            {
                Name = "autoAllocationButton",
                Text = "A",
                ForeColor = Color.Black,
                Position = new Position(0, 10)
            };
            this.autoAllocationButton.TextureManager.Textures.Change(0);
            this.autoAllocationButton.OnClick += this.AutoAllocationButton_OnClick;
            content_menu.Items.Add(this.autoAllocationButton);

            this.startModelButton = new GameButton()
            {
                Name = "startModeButton",
                Text = "M",
                ForeColor = Color.Black,
                Position = new Position(0, 10)
            };
            this.startModelButton.TextureManager.Textures.Change(4);
            this.startModelButton.OnClick += StartModelButton_OnClick;
            content_menu.Items.Add(this.startModelButton);

            this.endMessageLabel = new Label()
            {
                Text = "You Win!",
                ForeColor = Color.Green,
                Visible = false,
                DrawOrder = 1
            };
            this.endMessageLabel.Position = new Position(this.Width / 2 - this.endMessageLabel.Width / 2, this.Height / 2 - this.endMessageLabel.Height / 2);
            this.endMessageLabel.TextureManager.Fonts.Add(Resources.GetResource("endMessageFont") as SpriteFont);
            this.endMessageLabel.TextureManager.Fonts.Change(this.endMessageLabel.TextureManager.Fonts.Count() - 1);
            this.Items.Add(endMessageLabel);

            base.Designer();
            
            content_menu.Position = new Position(content_game.Width - (this.Width - content_menu.Width - 10), content.Height / 2 - content_menu.Height / 2);
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
            this.core.Reset(settings);

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
        }
        
        public void Start()
        {
            this.core.Reset(this.gameSettings);
            
            this.autoAllocationButton.Visible = false;

            this.gameStatus = new GameStatus();
            this.statusLabel.Text = this.gameStatus.ToString();

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
