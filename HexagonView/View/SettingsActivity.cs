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

        void Create()
        {
            var settings = this.LoadSettings();
            if (settings == null)
                settings = new GameSettings();

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
            this.modelTimingInfo = new Label() { Text = "StepTime (ms)", ForeColor = Color.White };

            // Changer
            this.rows = new Changer(new ValueRange(4, 20)) { Step = 1, Name = "RowChanger" };
            this.rows.Current.Value = settings.MapSize.Width;

            // Label
            this.rowsInfo = new Label() { Text = "Rows", ForeColor = Color.White };

            // Changer
            this.columns = new Changer(new ValueRange(4, 20)) { Step = 1, Name = "ColumnChanger" };
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
        }

        public SettingsActivity(Activity parent) : base(parent)
        {
            this.Create();

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

            this.Items.Add(this.percentBonus);
            this.Items.Add(this.percentBonusInfo);
            this.Items.Add(this.percentBlocked);
            this.Items.Add(this.percentBlockedInfo);

            this.Items.Add(this.lootPointForCreate);
            this.Items.Add(this.lootPointForCreateInfo);

            this.LoadSettings();
        }

        void SaveSettings(GameSettings settings)
        {
            /*
            string content = JsonConvert.SerializeObject(settings);
#if ANDROID
            using (var istrg = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
            {
                using (var fstream = istrg.OpenFile(fileSettingsPath, FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = Encoding.Default.GetBytes(content.ToCharArray());
                    fstream.Write(bytes, 0, bytes.Count());
                }
            }
#else
            File.WriteAllText(fileSettingsPath, content);
#endif
*/
            Storage.WriteAllText(fileSettingsPath, Storage.Serialize<GameSettings>(settings));
        }

        GameSettings LoadSettings()
        {
            /*
            string content = String.Empty;
            bool isExists = false;

#if ANDROID
            try
            {
                using (var istrg = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
                {
                    using (var fstream = istrg.OpenFile(fileSettingsPath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[fstream.Length];
                        fstream.Read(bytes, 0, (int)fstream.Length);
                        content = Encoding.Default.GetString(bytes);
                        isExists = true;
                    }
                }
            }
            catch (FileNotFoundException e)
            {

            }
#else
            isExists = File.Exists(fileSettingsPath);
            if (isExists)
                content = File.ReadAllText(fileSettingsPath);
#endif

            return isExists ? JsonConvert.DeserializeObject<GameSettings>(content) : null;
            */

            return Storage.Deserialize<GameSettings>(Storage.ReadAllText(fileSettingsPath));
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
            float r5_y = 4;

            // C1 - R1
            SetPosition(this.playersInfo, c1_x, r1_y);
            SetPosition(this.players, c2_x, r1_y);
            // C3 - R1
            SetPosition(this.modelInfo, c3_x, r1_y);
            SetPosition(this.modeling, c4_x, r1_y);
            // C5 - R1
            SetPosition(this.lifeEnableInfo, c5_x, r1_y);
            SetPosition(this.lifeEnable, c6_x, r1_y);
            // C1 - R2
            SetPosition(this.rowsInfo, c1_x, r2_y);
            SetPosition(this.rows, c2_x, r2_y);
            // C3 - R2
            SetPosition(this.modelTimingInfo, c3_x, r2_y);
            SetPosition(this.modelTiming, c4_x, r2_y);
            // C5 - R2
            SetPosition(this.lootEnableInfo, c5_x, r2_y);
            SetPosition(this.lootEnable, c6_x, r2_y);
            // C1 - R3
            SetPosition(this.columnsInfo, c1_x, r3_y);
            SetPosition(this.columns, c2_x, r3_y);
            // C5 - R3
            SetPosition(this.maxLifeEnableInfo, c5_x, r3_y);
            SetPosition(this.maxLifeEnable, c6_x, r3_y);
            // C1 - R4
            SetPosition(this.lootPointForCreateInfo, c1_x, r4_y);
            SetPosition(this.lootPointForCreate, c2_x, r4_y);

            // C1 - R5
            SetPosition(this.percentBonusInfo, c1_x, r5_y);
            SetPosition(this.percentBonus, c2_x, r5_y);
            // C3 - R5
            SetPosition(this.percentBlockedInfo, c3_x, r5_y);
            SetPosition(this.percentBlocked, c4_x, r5_y);
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
            };
            this.SaveSettings(settings);
            return settings;
        }
    }
}