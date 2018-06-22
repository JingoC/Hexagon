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

    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    using HexagonLibrary.Model.GameMode;

    public class SettingsActivity : Activity
    {
        Changer players = new Changer(new ValueRange(2, 10)) { Step = 1 };
        Label playersInfo = new Label() { Text = "Count Player ", ForeColor = Color.White };

        Toggle modeling = new Toggle(false);
        Label modelInfo = new Label() { Text = "Modeling", ForeColor = Color.White };

        Changer modelTiming = new Changer(new ValueRange(0, 1000)) { Step = 50 };
        Label modelTimingInfo = new Label() { Text = "StepTime (ms)", ForeColor = Color.White };

        Changer rows = new Changer(new ValueRange(4, 20)) { Step = 1, Name = "RowChanger" };
        Label rowsInfo = new Label() { Text = "Rows", ForeColor = Color.White };

        Changer columns = new Changer(new ValueRange(4, 20)) { Step = 1, Name = "ColumnChanger" };
        Label columnsInfo = new Label() { Text = "Columns", ForeColor = Color.White };

        Toggle lifeEnable = new Toggle(true);
        Label lifeEnableInfo = new Label() { Text = "Life Enable", ForeColor = Color.White };

        Toggle lootEnable = new Toggle(true);
        Label lootEnableInfo = new Label() { Text = "Loot Enable", ForeColor = Color.White };

        Toggle maxLifeEnable = new Toggle(true);
        Label maxLifeEnableInfo = new Label() { Text = "Max Life Enable", ForeColor = Color.White };

        Changer gameMode = new Changer(new ValueRange(0, 1)) { Step = 1 };
        Label gameModeInfo = new Label() { Text = "Game mode", ForeColor = Color.White };
        
        string[] gameModes = { "Normal", "BuildMap" };

        void SetGameModeText() => this.gameMode.Text = this.gameModes[(int)this.gameMode.Current.Value];

        public SettingsActivity(Activity parent) : base(parent)
        {
            this.modelTiming.Current.Value = 50;
            this.rows.Current.Value = 10;
            this.columns.Current.Value = 10;

            this.gameMode.ClickToUp += (s, e) => SetGameModeText();
            this.gameMode.ClickToDown += (s, e) => SetGameModeText();
            
            this.Items.Add(this.players);
            this.Items.Add(this.playersInfo);
            this.Items.Add(this.modelInfo);
            this.Items.Add(this.modeling);
            this.Items.Add(this.modelTiming);
            this.Items.Add(this.modelTimingInfo);
            this.Items.Add(this.rows);
            this.Items.Add(this.rowsInfo);
            this.Items.Add(this.columns);
            this.Items.Add(this.columnsInfo);

            this.Items.Add(this.lifeEnable);
            this.Items.Add(this.lifeEnableInfo);
            this.Items.Add(this.lootEnable);
            this.Items.Add(this.lootEnableInfo);
            this.Items.Add(this.maxLifeEnable);
            this.Items.Add(this.maxLifeEnableInfo);

            this.Items.Add(this.gameMode);
            this.Items.Add(this.gameModeInfo);
        }

        public override void Designer()
        {
            base.Designer();

            float w = GraphicsSingleton.GetInstance().Window.ClientBounds.Width;
            float h = GraphicsSingleton.GetInstance().Window.ClientBounds.Height;
            
            /*
            |-----|-----|-----|-----|
            |  0  |  1  |  2  |  3  |
            |-----|-----|-----|-----|
            |  1  |     |     |     |
            |-----|-----|-----|-----|
            |  2  |     |     |     |
            |-----|-----|-----|-----|
            |  3  |     |     |     |
            |-----|-----|-----|-----|
            */

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

            SetPosition(this.playersInfo, c1_x, r1_y);
            SetPosition(this.players, c2_x, r1_y);
            
            SetPosition(this.modelInfo, c3_x, r1_y);
            SetPosition(this.modeling, c4_x, r1_y);

            SetPosition(this.lifeEnableInfo, c5_x, r1_y);
            SetPosition(this.lifeEnable, c6_x, r1_y);

            SetPosition(this.rowsInfo, c1_x, r2_y);
            SetPosition(this.rows, c2_x, r2_y);

            SetPosition(this.modelTimingInfo, c3_x, r2_y);
            SetPosition(this.modelTiming, c4_x, r2_y);

            SetPosition(this.lootEnableInfo, c5_x, r2_y);
            SetPosition(this.lootEnable, c6_x, r2_y);

            SetPosition(this.columnsInfo, c1_x, r3_y);
            SetPosition(this.columns, c2_x, r3_y);
            
            SetPosition(this.maxLifeEnableInfo, c5_x, r3_y);
            SetPosition(this.maxLifeEnable, c6_x, r3_y);

            SetPosition(this.gameModeInfo, c1_x, r4_y);
            SetPosition(this.gameMode, c2_x, r4_y);

            SetGameModeText();
        }

        public GameSettings GetSettings()
        {
            TypeGameMode GetTypeModeGame(int index)
            {
                switch(index)
                {
                    case 0: return TypeGameMode.Normal;
                    case 1: return TypeGameMode.BuildMap;
                    default: return TypeGameMode.Normal;
                }
            }

            return new GameSettings()
            {
                CountPlayers = (int)this.players.Current.Value,
                ModelStepTiming = (int)this.modelTiming.Current.Value,
                MapSize = new Size() { Width = (int)this.rows.Current.Value, Height = (int) this.columns.Current.Value },
                GameMode = this.modeling.IsChecked ? TypeGameMode.Modeling : GetTypeModeGame((int)this.gameMode.Current.Value),
                ViewLifeEnable = this.lifeEnable.IsChecked,
                ViewLootEnable = this.lootEnable.IsChecked,
                ViewMaxLife = this.maxLifeEnable.IsChecked,
                
            };
        }
    }
}
