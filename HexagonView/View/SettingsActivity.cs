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

        Button returnButton = new Button() { Text = "Return" };

        public event EventHandler ExitActivity;

        public SettingsActivity(Activity parent) : base(parent)
        {
            this.returnButton.OnClick += (s, e) => { if (this.ExitActivity != null) { this.ExitActivity(s, e); } };

            this.modelTiming.Current.Value = 50;
            this.rows.Current.Value = 10;
            this.columns.Current.Value = 10;

            this.Items.Add(this.players);
            this.Items.Add(this.playersInfo);
            this.Items.Add(this.returnButton);
            this.Items.Add(this.modelInfo);
            this.Items.Add(this.modeling);
            this.Items.Add(this.modelTiming);
            this.Items.Add(this.modelTimingInfo);
            this.Items.Add(this.rows);
            this.Items.Add(this.rowsInfo);
            this.Items.Add(this.columns);
            this.Items.Add(this.columnsInfo);
        }

        public override void Designer()
        {
            base.Designer();

            int space = 400;

            this.players.Position = new Vector2(space - this.players.Width, 20);
            this.playersInfo.Position = new Vector2(30, this.players.Position.Y + this.players.Height / 2 - this.playersInfo.Height / 2);

            this.modeling.Position = new Vector2(space - this.modeling.Width, this.players.Position.Y + this.players.Height + 20);
            this.modelInfo.Position = new Vector2(30, this.modeling.Position.Y + this.modeling.Height / 2 - this.modelInfo.Height / 2);

            this.modelTiming.Position = new Vector2(space - this.modelTiming.Width, this.modeling.Position.Y + this.modeling.Height + 20);
            this.modelTimingInfo.Position = new Vector2(30, this.modelTiming.Position.Y + this.modelTiming.Height / 2 - this.modelTimingInfo.Height / 2);

            this.rows.Position = new Vector2(this.players.Position.X + this.players.Width + space - this.rows.Width + 20, 20);
            this.rowsInfo.Position = new Vector2(this.players.Position.X + this.players.Width + 20, this.rows.Position.Y + this.rows.Height / 2 - this.rowsInfo.Height / 2);

            this.columns.Position = new Vector2(this.modeling.Position.X + this.modeling.Width + space - this.columns.Width + 20, this.rows.Position.Y + this.rows.Height + 20);
            this.columnsInfo.Position = new Vector2(this.modeling.Position.X + this.modeling.Width + 20, this.columns.Position.Y + this.columns.Height / 2 - this.columnsInfo.Height / 2);
            
            this.returnButton.Position = new Vector2(30, this.modelTiming.Position.Y + this.modelTiming.Height);
        }

        public void SetSize(int row, int column)
        {

        }

        public GameSettings GetSettings()
        {
            return new GameSettings()
            {
                CountPlayers = (int)this.players.Current.Value,
                ModelStepTiming = (int)this.modelTiming.Current.Value,
                MapSize = new Size() { Width = (int)this.rows.Current.Value, Height = (int) this.columns.Current.Value },
                GameMode = this.modeling.IsChecked ? TypeGameMode.Modeling : TypeGameMode.Normal,
                PlayerMode = this.modeling.IsChecked ? TypePlayerMode.Modeling : TypePlayerMode.Normal
                
            };
        }
    }
}
