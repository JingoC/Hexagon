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

        int minModelTiming = 0;
        int maxModelTiming = 1000;
        int valueModelTiming = 50;
        Changer modelTiming = new Changer();
        Label modelTimingInfo = new Label() { Text = "Model step (ms)", ForeColor = Color.White };

        Button returnButton = new Button() { Text = "Return" };

        public event EventHandler ExitActivity;

        public SettingsActivity()
        {
            this.players.ClickToDown += (s, e) => { if (this.valuePlayer > this.minPlayer) { this.valuePlayer--; } this.players.Text = this.valuePlayer.ToString(); };
            this.players.ClickToUp += (s, e) => { if (this.valuePlayer < this.maxPlayer) { this.valuePlayer++; } this.players.Text = this.valuePlayer.ToString(); };
            this.players.Text = this.valuePlayer.ToString();

            this.modelTiming.ClickToDown += (s, e) => { if (this.valueModelTiming > this.minModelTiming) { this.valueModelTiming-=50; } this.modelTiming.Text = this.valueModelTiming.ToString(); };
            this.modelTiming.ClickToUp += (s, e) => { if (this.valueModelTiming < this.maxModelTiming) { this.valueModelTiming+=50; } this.modelTiming.Text = this.valueModelTiming.ToString(); };
            this.modelTiming.Text = this.valueModelTiming.ToString();
            
            this.returnButton.OnClick += (s, e) => { if (this.ExitActivity != null) { this.ExitActivity(s, e); } };

            this.Items.Add(this.players);
            this.Items.Add(this.playersInfo);
            this.Items.Add(this.returnButton);
            this.Items.Add(this.modelInfo);
            this.Items.Add(this.modeling);
            this.Items.Add(this.modelTiming);
            this.Items.Add(this.modelTimingInfo);

        }

        public override void Designer()
        {
            this.playersInfo.Position = new Vector2(30, 30);
            this.players.Position = new Vector2(150, 20);

            this.modelInfo.Position = new Vector2(30, 80);
            this.modeling.Position = new Vector2(150, 80);

            this.modelTimingInfo.Position = new Vector2(30, 140);
            this.modelTiming.Position = new Vector2(150, 140);
            
            this.returnButton.Position = new Vector2(30, 300);

            base.Designer();
        }

        public GameSettings GetSettings()
        {
            return new GameSettings()
            {
                CountPlayers = this.valuePlayer,
                ModelStepTiming = this.valueModelTiming,
                GameMode = this.modeling.IsChecked ? TypeGameMode.Modeling : TypeGameMode.Normal,
                PlayerMode = this.modeling.IsChecked ? TypePlayerMode.Modeling : TypePlayerMode.Normal
                
            };
        }
    }
}
