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

    using MonoGuiFramework;
    using MonoGuiFramework.Containers;
    using MonoGuiFramework.Controls;
    using MonoGuiFramework.System;
    using MonoGuiFramework.Base;

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
        
        Activity viewActivity;
        Activity modelingActivity;
        
        void Create()
        {
            var settings = this.LoadSettings();
            if (settings == null)
                settings = new GameSettings();

            this.Name = "GamePlay";
            this.viewActivity = new Activity(this.Parent);
            this.viewActivity.Name = "View";
            this.modelingActivity = new Activity(this.Parent);
            this.modelingActivity.Name = "Modeling";

            this.Navigation[(int)TypeNavigationActivity.Right] = this.viewActivity;
            this.viewActivity.Navigation[(int)TypeNavigationActivity.Left] = this;
            this.Navigation[(int)TypeNavigationActivity.Left] = this.modelingActivity;
            this.modelingActivity.Navigation[(int)TypeNavigationActivity.Right] = this;
            
            this.players = new Changer(new ValueRange(2, 10));
            this.players.Step = 1;
            this.players.Current.Value = settings.CountPlayers;
            this.playersInfo = new Label() { Text = "Count Player ", ForeColor = Color.White };

            this.modeling = new Toggle(settings.GameMode == TypeGameMode.Modeling);
            this.modelInfo = new Label() { Text = "Modeling", ForeColor = Color.White };

            this.modelTiming = new Changer(new ValueRange(0, 1000));
            this.modelTiming.Current.Value = settings.ModelStepTiming;
            this.modelTiming.Step = 50;

            this.modelTimingInfo = new Label() { Text = $"StepTime {Environment.NewLine}(ms)", ForeColor = Color.White };

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
        }

        public SettingsActivity(Activity parent) : base(parent)
        {
            this.Parent = parent;
            this.Create();
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
            float w = this.graphics.Width;
            float h = this.graphics.Height;

            void Designer_GamePlay()
            {
                VerticalContainer rows = new VerticalContainer(this) { TextureScale = ScaleMode.None };
                rows.BorderColor = Color.Yellow;
                rows.SetBounds(0, 0, (int)w, (int)h);

                HorizontalContainer row1 = new HorizontalContainer(rows) { TextureScale = ScaleMode.None };
                row1.BorderColor = Color.Blue;
                row1.SetBounds(0, 0, (int)w, 100);

                HorizontalContainer r1c1 = new HorizontalContainer(row1) { TextureScale = ScaleMode.None };
                r1c1.SetBounds(0, 0, 200, 100);

                this.playersInfo.BorderColor = Color.Green;
                this.playersInfo.SetBounds(0, 0, 1, 1);
                this.players.BorderColor = Color.Green;
                this.players.SetBounds(10, 0, 100, 100);
                
                r1c1.Items.Add(this.playersInfo);
                r1c1.Items.Add(this.players);
                
                HorizontalContainer r1c2 = new HorizontalContainer(row1) { TextureScale = ScaleMode.None };
                r1c2.BorderColor = Color.Brown;
                r1c2.SetBounds(0, 0, 200, 100);

                this.percentBlockedInfo.SetBounds(0, 0, 10, 10);
                this.percentBlocked.SetBounds(10, 0, 100, 100);

                r1c2.Items.Add(this.percentBlockedInfo);
                r1c2.Items.Add(this.percentBlocked);
                
                HorizontalContainer row2 = new HorizontalContainer(rows) { TextureScale = ScaleMode.None };
                row2.BorderColor = Color.Red;
                row2.SetBounds(0, 0, (int)w, 100);

                HorizontalContainer r2c1 = new HorizontalContainer(row2) { TextureScale = ScaleMode.None };
                r2c1.SetBounds(0, 0, 200, 100);

                this.rowsInfo.SetBounds(0, 0, 1, 1);
                this.rows.SetBounds(10, 0, 100, 100);

                r2c1.Items.Add(this.rowsInfo);
                r2c1.Items.Add(this.rows);

                HorizontalContainer r2c2 = new HorizontalContainer(row2) { TextureScale = ScaleMode.None };
                r2c2.SetBounds(0, 0, 200, 100);

                this.percentBonusInfo.SetBounds(0, 0, 1, 1);
                this.percentBonus.SetBounds(10, 0, 100, 100);

                r2c2.Items.Add(this.percentBonusInfo);
                r2c2.Items.Add(this.percentBonus);

                HorizontalContainer row3 = new HorizontalContainer(rows) { TextureScale = ScaleMode.None };
                row3.SetBounds(0, 0, (int)w, 100);

                HorizontalContainer r3c1 = new HorizontalContainer(row3) { TextureScale = ScaleMode.None };
                r3c1.SetBounds(0, 0, 200, 100);

                this.columnsInfo.SetBounds(0, 0, 1, 1);
                this.columns.SetBounds(10, 0, 100, 100);

                r3c1.Items.Add(this.columnsInfo);
                r3c1.Items.Add(this.columns);

                HorizontalContainer r3c2 = new HorizontalContainer(row3) { TextureScale = ScaleMode.None };
                r3c2.SetBounds(0, 0, 200, 100);

                this.lootPointForCreateInfo.SetBounds(0, 0, 1, 1);
                this.lootPointForCreate.SetBounds(10, 0, 100, 100);

                r3c2.Items.Add(this.lootPointForCreateInfo);
                r3c2.Items.Add(this.lootPointForCreate);
                
                // gamePlay
                row1.Items.Add(r1c1);
                row1.Items.Add(r1c2);
                row2.Items.Add(r2c1);
                row2.Items.Add(r2c2);
                row3.Items.Add(r3c1);
                row3.Items.Add(r3c2);
                
                rows.Items.Add(row1);
                rows.Items.Add(row2);
                rows.Items.Add(row3);

                this.Items.Add(rows);
                
                this.SetBounds(0, 0, this.Width, this.Height);
            }

            void Designer_View()
            {
                
                this.scaleHexagonInfo.SetBounds(10, 10, 100, 100);
                this.scaleHexagonInfo.BorderColor = Color.Red;
                this.scaleHexagon.SetBounds(200, 200, 100, 100);
                this.scaleHexagon.BorderColor = Color.Yellow;

                Changer c = new Changer(new ValueRange(10, 200));
                c.BorderColor = Color.White;
                c.SetBounds(400, 400, 1, 1);

                this.viewActivity.Items.Add(this.scaleHexagon);
                this.viewActivity.Items.Add(this.scaleHexagonInfo);
                this.viewActivity.Items.Add(c);
                /*
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

                // view
                this.viewContainer.Items.Add(this.lifeEnable);
                this.viewContainer.Items.Add(this.lifeEnableInfo);
                this.viewContainer.Items.Add(this.lootEnable);
                this.viewContainer.Items.Add(this.lootEnableInfo);
                this.viewContainer.Items.Add(this.maxLifeEnable);
                this.viewContainer.Items.Add(this.maxLifeEnableInfo);
                this.viewContainer.Items.Add(this.scaleHexagon);
                this.viewContainer.Items.Add(this.scaleHexagonInfo);
                */

                this.viewActivity.SetBounds(0, 0, this.Width, this.Height);
                this.viewActivity.Designer();
            }

            void Designer_Modeling()
            {
                /*
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
                // modeling
                this.modelingContainer.Items.Add(this.modelInfo);
                this.modelingContainer.Items.Add(this.modeling);
                this.modelingContainer.Items.Add(this.modelTiming);
                this.modelingContainer.Items.Add(this.modelTimingInfo);

                this.Items.Add(this.changeContainer);
                this.Items.Add(this.viewContainer);
                this.Items.Add(this.gamePlayContainer);
                this.Items.Add(this.modelingContainer);
                */
            }

            Designer_View();
            Designer_GamePlay();
            Designer_Modeling();

            base.Designer();
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