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

        StartPageActivity startPage = new StartPageActivity(null) { Name = "StartPage" };
        GameActivity gamePage = new GameActivity(null) { Name = "GamePage" };
        SettingsActivity settingsPage = new SettingsActivity(null) { Name = "SettingsPage" };
        

        public GameView() : base()
        {
            this.gamePage.Parent = this.startPage;
            this.settingsPage.Parent = this.startPage;

            this.Activities.Add(this.gamePage);
            this.Activities.Add(this.settingsPage);
            this.Activities.Add(this.startPage);
            
            //this.settingsPage.ExitActivity += (s, e) => this.ActivitySelected = startPage;
            this.gamePage.ExitActivity += (s, e) => this.ActivitySelected = startPage;

            var sttg_btn = this.startPage.Items.OfType<Button>().FirstOrDefault(x => x.Name.Equals("settingsGameButton"));
            sttg_btn.OnClick += (s, e) => this.ActivitySelected = settingsPage;

            var newgame_btn = this.startPage.Items.OfType<Button>().FirstOrDefault(x => x.Name.Equals("newGameButton"));
            newgame_btn.OnClick += delegate (Object sender, EventArgs e)
            {
                gamePage.SetSettings(settingsPage.GetSettings());
                this.ActivitySelected = gamePage;
            };

            this.ActivitySelected = startPage;
        }
    }
}
