using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

using System;
using System.Text;

using Microsoft.Xna.Framework;

using WinSystem;
using WinSystem.Controls;
using HexagonLibrary;
using HexagonLibrary.Model.GameMode;

namespace Hexagon
{
    [Activity(Label = "Hexagon"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        { 
            base.OnCreate(bundle);
            
            WSystem winSystem = new WSystem();
            
            var g = winSystem.Graphics;
            SetContentView((View)g.Services.GetService(typeof(View)));

            Core core = new Core(new GameSettings());
            WinSystem.Activity coreActivity = new WinSystem.Activity();

            winSystem.ActivitySelected.Items.Add(core);

            Button endStepButton = new Button();
            endStepButton.Text = "End Step";
            endStepButton.Position = new Vector2(600, 20);
            endStepButton.OnClick += (s, e) => core.GameModeStrategy.EndStep();
            winSystem.ActivitySelected.Items.Add(endStepButton);

            Button nextStepButton = new Button();
            nextStepButton.Text = "Next Step";
            nextStepButton.Position = new Vector2(600, 60);
            nextStepButton.OnClick += (s, e) => core.GameModeStrategy.NextStep();
            winSystem.ActivitySelected.Items.Add(nextStepButton);

            string nl = System.Environment.NewLine;
            var labelInfo = new Label();
            labelInfo.Name = "labelInfo";
            labelInfo.Position = new Vector2(600, 100);
            labelInfo.ForeColor = Color.White;
            winSystem.ActivitySelected.Items.Add(labelInfo);

            winSystem.LoadContentEvent += delegate (object sender, EventArgs e)
            {
                core.LoadContent();
            };

            winSystem.UpdateEvent += delegate (object sender, EventArgs e)
            {
                core.Update();
                string text = string.Format("Step: {1}{0}LootPoint: {2}{0}",
                    nl,
                    core.GameModeStrategy.Step,
                    core.GameModeStrategy.User.LootPoints);
                labelInfo.Text = text;
            };

            g.Run();
        }
    }
}

