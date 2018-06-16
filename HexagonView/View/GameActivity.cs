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

    using HexagonView.Controls;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    public class GameActivity : Activity
    {
        Core core;
        GameSettings gameSettings;

        GameButton gameControlButton = new GameButton() { Name = "endStepButton", Text = "", ForeColor = Color.Black };
        GameButton newGameButton = new GameButton() { Name = "newGameButton", Text = "N", ForeColor = Color.Black };
        GameButton startModelButton = new GameButton() { Name = "startModeButton", Text = "M", ForeColor = Color.Black };
        
        Container menu = new Container();

        public event EventHandler ExitActivity;

        int stateStepBtn = 0;

        public GameActivity(Activity parent) : base(parent)
        {
            this.gameControlButton.OnClick += GameControlButton_OnClick;

            this.newGameButton.OnClick += (s, e) => { this.Start(); };
            this.startModelButton.OnClick += StartModelButton_OnClick;
            
            this.menu.Items.Add(this.gameControlButton);
            this.menu.Items.Add(this.newGameButton);
            this.menu.Items.Add(this.startModelButton);

            this.Items.Add(this.menu);
        }

        private void GameControlButton_OnClick(object sender, EventArgs e)
        {
            if (stateStepBtn < 2)
            {
                void LootChangedHandler(Object obj, EventArgs ea)
                {
                    int lp = this.core.GameModeStrategy.User.LootPoints;

                    if (lp == 0) EndLootAllocation(); else this.gameControlButton.Text = lp.ToString();
                };

                void FinishHandler(Object obj, EventArgs ea)
                {
                    this.gameControlButton.TextureManager.Textures.Change(0);
                    this.core.GameModeStrategy.FinishCpuStep -= FinishHandler;
                    stateStepBtn = 0;
                };

                void EndLootAllocation()
                {
                    this.core.GameModeStrategy.User.LootPointsChanged -= LootChangedHandler;
                    this.gameControlButton.Text = String.Empty;
                    this.gameControlButton.TextureManager.Textures.Change(2);

                    this.core.GameModeStrategy.FinishCpuStep += FinishHandler;
                    this.core.GameModeStrategy.NextStep();

                    stateStepBtn = 2;
                }

                switch (stateStepBtn)
                {
                    // loot allocation
                    case 0:
                        {
                            this.core.GameModeStrategy.User.LootPointsChanged += LootChangedHandler;
                            this.core.GameModeStrategy.EndStep();
                            this.gameControlButton.TextureManager.Textures.Change(1);
                            stateStepBtn = 1;
                        }
                        break;
                    // end loot allocation
                    case 1: EndLootAllocation(); break;
                    default: break;
                }
            }
        }

        public override void Designer()
        {
            base.Designer();

            this.newGameButton.TextureManager.Textures.Change(3);
            this.startModelButton.TextureManager.Textures.Change(4);

            this.newGameButton.Position = new Vector2(10, 10);
            this.gameControlButton.Position = new Vector2(10, this.newGameButton.Position.Y + this.newGameButton.Height + 40);
            this.startModelButton.Position = new Vector2(10, this.gameControlButton.Position.Y + this.gameControlButton.Height + 40);

            int w = GraphicsSingleton.GetInstance().GetGraphics().PreferredBackBufferWidth;
            int h = GraphicsSingleton.GetInstance().GetGraphics().PreferredBackBufferHeight;

            this.menu.Position = new Vector2(w - this.menu.Width - 20, h / 2 - this.menu.Height / 2);
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
            switch(settings.GameMode)
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
                    }break;
            }

            this.Items.Add(this.core);
        }

        public void Start()
        {
            this.core.Reset();
            this.gameControlButton.TextureManager.Textures.Change(0);
            this.gameControlButton.Text = String.Empty;
            this.stateStepBtn = 0;
        }


    }
}
