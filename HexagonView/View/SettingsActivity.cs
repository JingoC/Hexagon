using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonView.View
{
    using System.IO;
    using System.IO.IsolatedStorage;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using WinSystem;
    using WinSystem.Controls;
    using WinSystem.System;

    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    using HexagonLibrary.Model.GameMode;

    public class SettingsActivity : Activity
    {
        static string fileSettingsPath = "settings.json";

        Changer players;
        Label playersInfo;

        Toggle modeling;
        Label modelInfo;

        Changer modelTiming;
        Label modelTimingInfo;

        Changer rows;
        Label rowsInfo;

        Changer columns;
        Label columnsInfo;

        Toggle lifeEnable;
        Label lifeEnableInfo;

        Toggle lootEnable;
        Label lootEnableInfo;

        Toggle maxLifeEnable;
        Label maxLifeEnableInfo;

        Changer lootPointForCreate;
        Label lootPointForCreateInfo;

        Changer percentBonus;
        Label percentBonusInfo;

        Changer percentBlocked;
        Label percentBlockedInfo;

        Changer scaleHexagon;
        Label scaleHexagonInfo;

        Changer changeContainer;

        Container gamePlayContainer;
        Container viewContainer;
        Container modelingContainer;

        string[] headerContainer = { "View", "GamePlay", "Modeling" };

        void ChangeContainer()
        {
            var type = (int)this.changeContainer.Current.Value;
            this.changeContainer.Text = this.headerContainer[type];
            switch(type)
            {
                case 0: this.gamePlayContainer.Visible = this.modelingContainer.Visible = false; this.viewContainer.Visible = true; break;
                case 1: this.viewContainer.Visible = this.modelingContainer.Visible = false; this.gamePlayContainer.Visible = true; break;
                case 2: this.gamePlayContainer.Visible = this.viewContainer.Visible = false; this.modelingContainer.Visible = true; break;
            }

            float w = GraphicsSingleton.GetInstance().GetGraphics().PreferredBackBufferWidth;
            this.changeContainer.Position = new Vector2((w / 2) - (this.changeContainer.Width / 2), 5);
        }

        void Create()
        {
            var settings = this.LoadSettings();
            if (settings == null)
                settings = new GameSettings();

            this.gamePlayContainer = new Container() { Visible = false };
            this.gamePlayContainer.Position = new Vector2(0, 0);
            this.viewContainer = new Container() { Visible = true };
            this.viewContainer.Position = new Vector2(0, 0);
            this.modelingContainer = new Container() { Visible = false };
            this.modelingContainer.Position = new Vector2(0, 0);

            // Changer
            this.players = new Changer(new ValueRange(2, 10));
            this.players.Step = 1;
            this.players.Current.Value = settings.CountPlayers;


            // Label
            this.playersInfo = new Label() { Text = "Count Player ", ForeColor = Color.White };

            // Toggle
            this.modeling = new Toggle(settings.GameMode == TypeGameMode.Modeling);
            this.modelInfo = new Label() { Text = "Modeling", ForeColor = Color.White };

            // Changer
            this.modelTiming = new Changer(new ValueRange(0, 1000));
            this.modelTiming.Current.Value = settings.ModelStepTiming;
            this.modelTiming.Step = 50;

            // Label
            this.modelTimingInfo = new Label() { Text = $"StepTime {Environment.NewLine}(ms)", ForeColor = Color.White };

            // Changer
            this.rows = new Changer(new ValueRange(4, 100)) { Step = 1, Name = "RowChanger" };
            this.rows.Current.Value = settings.MapSize.Width;

            // Label
            this.rowsInfo = new Label() { Text = "Rows", ForeColor = Color.White };

            // Changer
            this.columns = new Changer(new ValueRange(4, 100)) { Step = 1, Name = "ColumnChanger" };
            this.columns.Current.Value = settings.MapSize.Height;

            // Label
            this.columnsInfo = new Label() { Text = "Columns", ForeColor = Color.White };

            // Toggle
            this.lifeEnable = new Toggle(settings.ViewLifeEnable);

            // Label
            this.lifeEnableInfo = new Label() { Text = "Life Enable", ForeColor = Color.White };

            // Toggle
            this.lootEnable = new Toggle(settings.ViewLootEnable);

            // Label
            this.lootEnableInfo = new Label() { Text = "Loot Enable", ForeColor = Color.White };

            // Toggle
            this.maxLifeEnable = new Toggle(settings.ViewMaxLife);

            // Label
            this.maxLifeEnableInfo = new Label() { Text = "Max Life Enable", ForeColor = Color.White };

            // Changer
            this.percentBonus = new Changer(new ValueRange(0, 100)) { Step = 10 };
            this.percentBonus.Current.Value = settings.PercentBonus;

            // Label
            this.percentBonusInfo = new Label() { Text = "Bonus (%)", ForeColor = Color.White };

            // Changer
            this.percentBlocked = new Changer(new ValueRange(0, 100)) { Step = 10 };
            this.percentBlocked.Current.Value = settings.PercentBlocked;

            // Label
            this.percentBlockedInfo = new Label() { Text = "Blocked (%)", ForeColor = Color.White };

            // Changer
            this.lootPointForCreate = new Changer(new ValueRange(1, 100)) { Step = 1 };
            this.lootPointForCreate.Current.Value = settings.LootPointForCreate;

            // Label
            this.lootPointForCreateInfo = new Label() { Text = "LP (Create)", ForeColor = Color.White };

            // Changer
            this.scaleHexagon = new Changer(new ValueRange(0.1, 5)) { Step = 0.1f };
            this.scaleHexagon.Current.Value = settings.ScaleHexagon;

            // Label
            this.scaleHexagonInfo = new Label() { Text = "Scale", ForeColor = Color.White };

            // Changer
            this.changeContainer = new Changer(new ValueRange(0, 2)) { Text = "View", ForeColor = Color.White, Step = 1 };
            this.changeContainer.Current.Value = 0;
            this.changeContainer.Text = this.headerContainer[0];
            this.changeContainer.ClickToUp += (s, e) => this.ChangeContainer();
            this.changeContainer.ClickToDown += (s, e) => this.ChangeContainer();
            
        }

        public SettingsActivity(Activity parent) : base(parent)
        {
            this.Create();
            
            // view
            this.viewContainer.Items.Add(this.lifeEnable);
            this.viewContainer.Items.Add(this.lifeEnableInfo);
            this.viewContainer.Items.Add(this.lootEnable);
            this.viewContainer.Items.Add(this.lootEnableInfo);
            this.viewContainer.Items.Add(this.maxLifeEnable);
            this.viewContainer.Items.Add(this.maxLifeEnableInfo);
            this.viewContainer.Items.Add(this.scaleHexagon);
            this.viewContainer.Items.Add(this.scaleHexagonInfo);
            
            // gamePlay
            this.gamePlayContainer.Items.Add(this.players);
            this.gamePlayContainer.Items.Add(this.playersInfo);
            this.gamePlayContainer.Items.Add(this.rows);
            this.gamePlayContainer.Items.Add(this.rowsInfo);
            this.gamePlayContainer.Items.Add(this.columns);
            this.gamePlayContainer.Items.Add(this.columnsInfo);
            this.gamePlayContainer.Items.Add(this.percentBonus);
            this.gamePlayContainer.Items.Add(this.percentBonusInfo);
            this.gamePlayContainer.Items.Add(this.percentBlocked);
            this.gamePlayContainer.Items.Add(this.percentBlockedInfo);
            this.gamePlayContainer.Items.Add(this.lootPointForCreate);
            this.gamePlayContainer.Items.Add(this.lootPointForCreateInfo);
            
            // modeling
            this.modelingContainer.Items.Add(this.modelInfo);
            this.modelingContainer.Items.Add(this.modeling);
            this.modelingContainer.Items.Add(this.modelTiming);
            this.modelingContainer.Items.Add(this.modelTimingInfo);

            this.Items.Add(this.changeContainer);
            this.Items.Add(this.viewContainer);
            this.Items.Add(this.gamePlayContainer);
            this.Items.Add(this.modelingContainer);

            this.LoadSettings();
        }

        void SaveSettings(GameSettings settings)
        {
            Storage.WriteAllText(fileSettingsPath, Storage.Serialize<GameSettings>(settings));
        }

        GameSettings LoadSettings()
        {
            return Storage.Deserialize<GameSettings>(Storage.ReadAllText(fileSettingsPath));
        }
        
        public override void Designer()
        {
            base.Designer();

            float w = GraphicsSingleton.GetInstance().GetGraphics().PreferredBackBufferWidth;
            float h = GraphicsSingleton.GetInstance().GetGraphics().PreferredBackBufferHeight;

            this.changeContainer.Text = this.headerContainer[0];
            this.changeContainer.Position = new Vector2((w / 2) - (this.changeContainer.Width / 2), 5);
            
            float stepX = w / 64;
            float stepY = h / 8;

            void SetPosition(IControl sc, float x, float y)
            {
                float _x = stepX * x;
                float _y = stepY * y + stepY / 2 - sc.Height / 2;
                sc.Position = new Vector2(_x, _y);
            };

            float c1_x = 1;
            float c2_x = 7;
            float c3_x = 20;
            float c4_x = 27;
            float c5_x = 39;
            float c6_x = 46;

            float r1_y = 0;
            float r2_y = 1;
            float r3_y = 2;
            float r4_y = 3;
            float r5_y = 4;

            void Designer_GamePlay()
            {
                // R1 - C1
                SetPosition(this.playersInfo, c1_x, r1_y);
                SetPosition(this.players, c2_x, r1_y);
                // R1 - C3
                SetPosition(this.percentBonusInfo, c3_x, r1_y);
                SetPosition(this.percentBonus, c4_x, r1_y);
                // R1 - C5
                // R1 - C7
                // R2 - C1
                SetPosition(this.rowsInfo, c1_x, r2_y);
                SetPosition(this.rows, c2_x, r2_y);
                // R2 - C3
                SetPosition(this.percentBlockedInfo, c3_x, r2_y);
                SetPosition(this.percentBlocked, c4_x, r2_y);
                // R2 - C5
                // R2 - C7
                // R3 - C1
                SetPosition(this.columnsInfo, c1_x, r3_y);
                SetPosition(this.columns, c2_x, r3_y);
                // R3 - C3
                SetPosition(this.lootPointForCreateInfo, c3_x, r3_y);
                SetPosition(this.lootPointForCreate, c4_x, r3_y);
                // R3 - C5
                // R3 - C7
                // R4 - C1
                // R4 - C3
                // R4 - C5
                // R4 - C7

            }

            void Designer_View()
            {
                // R1 - C1
                SetPosition(this.scaleHexagonInfo, c1_x, r1_y);
                SetPosition(this.scaleHexagon, c2_x, r1_y);
                // R1 - C3
                // R1 - C5
                SetPosition(this.lifeEnableInfo, c5_x, r1_y);
                SetPosition(this.lifeEnable, c6_x, r1_y);
                // R1 - C7
                // R2 - C1
                // R2 - C3
                // R2 - C5
                SetPosition(this.lootEnableInfo, c5_x, r2_y);
                SetPosition(this.lootEnable, c6_x, r2_y);
                // R2 - C7
                // R3 - C1
                // R3 - C3
                // R3 - C5
                SetPosition(this.maxLifeEnableInfo, c5_x, r3_y);
                SetPosition(this.maxLifeEnable, c6_x, r3_y);
                // R3 - C7
                // R4 - C1
                // R4 - C3
                // R4 - C5
                // R4 - C7

            }

            void Designer_Modeling()
            {
                // R1 - C1
                SetPosition(this.modelInfo, c1_x, r1_y);
                SetPosition(this.modeling, c2_x, r1_y);
                // R1 - C3
                // R1 - C5
                // R1 - C7
                // R2 - C1
                SetPosition(this.modelTimingInfo, c1_x, r2_y);
                SetPosition(this.modelTiming, c2_x, r2_y);
                // R2 - C3
                // R2 - C5
                // R2 - C7
                // R3 - C1
                // R3 - C3
                // R3 - C5
                // R3 - C7
                // R4 - C1
                // R4 - C3
                // R4 - C5
                // R4 - C7
            }

            Designer_View();
            Designer_GamePlay();
            Designer_Modeling();

            float yContainer = this.changeContainer.Height + this.changeContainer.Position.Y;
            this.gamePlayContainer.Position = new Vector2(0, yContainer);
            this.modelingContainer.Position = new Vector2(0, yContainer);
            this.viewContainer.Position = new Vector2(0, yContainer);
        }

        public GameSettings GetSettings()
        {
            var settings = new GameSettings()
            {
                CountPlayers = (int)this.players.Current.Value,
                ModelStepTiming = (int)this.modelTiming.Current.Value,
                MapSize = new Size() { Width = (int)this.rows.Current.Value, Height = (int)this.columns.Current.Value },
                GameMode = this.modeling.IsChecked ? TypeGameMode.Modeling : TypeGameMode.Normal,
                ViewLifeEnable = this.lifeEnable.IsChecked,
                ViewLootEnable = this.lootEnable.IsChecked,
                ViewMaxLife = this.maxLifeEnable.IsChecked,
                PercentBonus = (int)this.percentBonus.Current.Value,
                PercentBlocked = (int)this.percentBlocked.Current.Value,
                LootPointForCreate = (int)this.lootPointForCreate.Current.Value,
                ScaleHexagon = (float)this.scaleHexagon.Current.Value,
            };
            this.SaveSettings(settings);
            return settings;
        }
    }
}