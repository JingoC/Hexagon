using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonView
{
    using View;

    using MonoGuiFramework;
    using MonoGuiFramework.Controls;
    using MonoGuiFramework.System;

    using HexagonLibrary;
    using HexagonLibrary.Model.Navigation;
    using HexagonLibrary.Model.GameMode;
    using HexagonLibrary.Entity.GameObjects;

    public class GameView : MonoGui
    {
        List<Activity> views = new List<Activity>();

        StartPageActivity startPage;
        GameActivity gamePage;
        SettingsActivity settingsPage;
        

        public GameView() : base()
        {
            this.Initialize += (s, e) =>
            {
                this.startPage = new StartPageActivity(null) { Name = "StartPage" };
                this.gamePage = new GameActivity(startPage) { Name = "GamePage" };
                this.settingsPage = new SettingsActivity(startPage) { Name = "SettingsPage" };

                this.ActivitySelected = startPage;

                this.Activities.Add(this.gamePage);
                this.Activities.Add(this.settingsPage);
                this.Activities.Add(this.startPage);

                this.Designer();
            };
        }

        protected void Designer()
        {
            var sttg_btn = this.startPage.GetChild("settingsGameButton") as Button;
            sttg_btn.OnClick += (s, ev) => this.ActivitySelected = settingsPage;

            var newgame_btn = this.startPage.GetChild("newGameButton") as Button;
            newgame_btn.OnClick += delegate (Object s, EventArgs ev)
            {
                gamePage.SetSettings(settingsPage.GetSettings());
                this.ActivitySelected = gamePage;
            };
        }
    }
}
