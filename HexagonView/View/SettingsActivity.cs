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
        int minPlayer = 2;
        int maxPlayer = 10;
        int valuePlayer = 4;

        Changer players = new Changer();
        Label playersInfo = new Label() { Text = "Count Player ", ForeColor = Color.White };

        Toggle modeling = new Toggle(false);
        Label modelInfo = new Label() { Text = "Modeling", ForeColor = Color.White };

        Button returnButton = new Button() { Text = "Return" };

        public event EventHandler ExitActivity;

        public SettingsActivity()
        {
            this.players.ClickToDown += (s, e) => { if (this.valuePlayer > minPlayer) { this.valuePlayer--; } this.players.Text = this.valuePlayer.ToString(); };
            this.players.ClickToUp += (s, e) => { if (this.valuePlayer < maxPlayer) { this.valuePlayer++; } this.players.Text = this.valuePlayer.ToString(); };
            this.players.Text = this.valuePlayer.ToString();

            this.returnButton.OnClick += (s, e) => { if (this.ExitActivity != null) { this.ExitActivity(s, e); } };

            this.Items.Add(this.players);
            this.Items.Add(this.playersInfo);
            this.Items.Add(this.returnButton);
            this.Items.Add(this.modelInfo);
            this.Items.Add(this.modeling);

        }

        public override void Designer()
        {
            this.playersInfo.Position = new Vector2(20, 30);
            this.players.Position = new Vector2(140, 20);

            this.modeling.Position = new Vector2(140, 80);
            this.modelInfo.Position = new Vector2(20, 80);

            this.returnButton.Position = new Vector2(20, 300);

            base.Designer();
        }

        public GameSettings GetSettings()
        {
            return new GameSettings()
            {
                CountPlayers = this.valuePlayer,
                PlayerMode = this.modeling.IsChecked ? TypePlayerMode.Modeling : TypePlayerMode.Normal
            };
        }
    }
}
