﻿using System;
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

    using HexagonLibrary.Model.GameMode;

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
            
            var sttg_btn = startPage.Items.OfType<Button>().FirstOrDefault(x => x.Name.Equals("settingsGameButton"));
            
            this.Activities.Add(startPage);

            settingsPage.ExitActivity += (s, e) => this.ActivitySelected = startPage;
            gamePage.ExitActivity += (s, e) => this.ActivitySelected = startPage;
            sttg_btn.OnClick += (s, e) => this.ActivitySelected = settingsPage;
            newgame_btn.OnClick += (s, e) => { gamePage.SetSettings(settingsPage.GetSettings()); this.ActivitySelected = gamePage; };


            gamePage.SetSettings(new GameSettings() { GameMode = TypeGameMode.Modeling, CountPlayers = 5, PlayerMode = TypePlayerMode.Modeling, MapSize = new Size() { Width = 10, Height = 12 } });
            this.ActivitySelected = gamePage;
        }
    }
}
