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

            float stX = 30;
            int space = 450;

            float GetXOfs(IControl ctlr, float ofs) => ofs - ctlr.Width;
            float GetY(IControl ctlr, IControl depCtrl) => depCtrl.Height / 2 + depCtrl.Position.Y - ctlr.Height / 2;
            float GetYDepY(IControl depCtrl, float sH) => depCtrl.Height + depCtrl.Position.Y + sH;

            Vector2 GetPositionV(IControl ctrl, IControl dctrl, float sW, float sH) => new Vector2(GetXOfs(ctrl, sW), GetYDepY(dctrl, sH));
            Vector2 GetPositionH(IControl ctrl, IControl dctrl, float sx) => new Vector2(sx, GetY(ctrl, dctrl));

            this.players.Position = new Vector2(GetXOfs(this.players, space), 10);
            this.playersInfo.Position = GetPositionH(this.playersInfo, this.players, stX);

            this.modeling.Position = GetPositionV(this.modeling, this.players, space, 10);
            this.modelInfo.Position = GetPositionH(this.modelInfo, this.modeling, stX);

            this.modelTiming.Position = GetPositionV(this.modelTiming, this.modeling, space, 10);
            this.modelTimingInfo.Position = GetPositionH(this.modelTimingInfo, this.modelTiming, stX);

            this.rows.Position = new Vector2(GetXOfs(this.rows, space * 2), 60);
            this.rowsInfo.Position = GetPositionH(this.rowsInfo, this.rows, space + stX);

            this.columns.Position = GetPositionV(this.columns, this.rows, space * 2, 10);
            this.columnsInfo.Position = GetPositionH(this.columnsInfo, this.columns, space + stX);

            float h = GraphicsSingleton.GetInstance().Window.ClientBounds.Height;
            this.returnButton.Position = new Vector2(stX, h - this.returnButton.Height - 30);
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
