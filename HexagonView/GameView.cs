using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonView
{
    using View;

    using WinSystem;
    using WinSystem.Controls;
    using WinSystem.System;

    using HexagonLibrary;
    using HexagonLibrary.Model.Navigation;
    using HexagonLibrary.Model.GameMode;
    using HexagonLibrary.Entity.GameObjects;

    public class GameView : WSystem
    {
        List<Activity> views = new List<Activity>();

        GameActivity gamePage = new GameActivity() { Name = "GamePage" };
        SettingsActivity settingsPage = new SettingsActivity() { Name = "SettingsPage" };
        StartPageActivity startPage = new StartPageActivity() { Name = "StartPage" };

        public GameView() : base()
        {   
            this.Activities.Add(this.gamePage);
            this.Activities.Add(this.settingsPage);
            this.Activities.Add(this.startPage);
            
            this.settingsPage.ExitActivity += (s, e) => this.ActivitySelected = startPage;
            this.gamePage.ExitActivity += (s, e) => this.ActivitySelected = startPage;

            var sttg_btn = this.startPage.Items.OfType<Button>().FirstOrDefault(x => x.Name.Equals("settingsGameButton"));
            sttg_btn.OnClick += (s, e) => this.ActivitySelected = settingsPage;

            var newgame_btn = this.startPage.Items.OfType<Button>().FirstOrDefault(x => x.Name.Equals("newGameButton"));
            newgame_btn.OnClick += delegate (Object sender, EventArgs e)
            {
                gamePage.SetSettings(settingsPage.GetSettings());
                this.ActivitySelected = gamePage;
            };

            //gamePage.SetSettings(new GameSettings() { GameMode = TypeGameMode.Modeling, CountPlayers = 5, PlayerMode = TypePlayerMode.Modeling, MapSize = new Size() { Width = row, Height = column } });
            this.ActivitySelected = startPage;
        }
    }
}
