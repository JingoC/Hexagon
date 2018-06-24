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
    using HexagonView;

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

            var winSystem = new GameView();

            SetContentView((View)winSystem.Graphics.Services.GetService(typeof(View)));
            winSystem.Exit += (s, e) => Java.Lang.JavaSystem.Exit(0);
            winSystem.Run();
        }
    }
}