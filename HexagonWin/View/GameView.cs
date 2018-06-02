using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonWin.View
{
    using WinSystem;
    using WinSystem.Controls;
    using WinSystem.System;

    public class GameView : WSystem
    {
        List<Activity> views = new List<Activity>();

        public GameView()
        {
            GameActivity gamePage = new GameActivity();
            gamePage.Name = "GamePage";
            this.Activities.Add(gamePage);

            SettingsActivity settingsPage = new SettingsActivity();
            settingsPage.Name = "SettingsPage";
            this.Activities.Add(settingsPage);
            
            StartPageActivity startPage = new StartPageActivity();
            startPage.Name = "StartPage";
            var newgame_btn = startPage.Items.OfType<Button>().FirstOrDefault(x => x.Name.Equals("newGameButton"));
            newgame_btn.OnClick += (s, e) => { gamePage.SetSettings(settingsPage.GetSettings()); this.ActivitySelected = gamePage; };
            var sttg_btn = startPage.Items.OfType<Button>().FirstOrDefault(x => x.Name.Equals("settingsGameButton"));
            sttg_btn.OnClick += (s, e) => this.ActivitySelected = settingsPage;
            this.Activities.Add(startPage);

            this.ActivitySelected = startPage;
        }
    }
}
