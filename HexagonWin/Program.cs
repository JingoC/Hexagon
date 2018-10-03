using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using MonoGuiFramework;
using MonoGuiFramework.Controls;
using HexagonLibrary;
using HexagonLibrary.Entity.GameObjects;
using HexagonLibrary.Model.GameMode;

namespace HexagonWin
{
    using HexagonView;
    using MonoGuiFramework.System;
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        static string json = "[" +
                "{\"Name\": \"gameBtn1\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"gameBtn2\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"gameBtn3\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"gameBtn4\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"gameBtn5\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"btn_newgame_idle\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"btn_newgame_click\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"btn_settings_idle\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"btn_settings_click\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"btn_idle_tmp\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"btn_click_tmp\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultToggleOn\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultToggleOff\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultChangerDown\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultChangerUp\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_green\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_red\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_yellow\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_brown\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_purple\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_orange\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_limegreen\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_blue\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_aqua\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_aqua_checked\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_gray\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_white\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_bonus_bomb\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"background_startpage\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultFont\", \"Type\": \"Font\"}," +
                "{\"Name\": \"gameBtnFont\", \"Type\": \"Font\"}," +
                "{\"Name\": \"settingsHeaderFont\", \"Type\": \"Font\"}," +
                "{\"Name\": \"endMessageFont\", \"Type\": \"Font\"}," +
                "]";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Form cursorPos = new System.Windows.Forms.Form();
            cursorPos.MinimizeBox = false;
            cursorPos.MaximizeBox = false;
            cursorPos.Show();
            cursorPos.SetBounds(1340, 0, 10, 50);

            System.Windows.Forms.Form dbg = new System.Windows.Forms.Form();
            System.Windows.Forms.TextBox tb = new System.Windows.Forms.TextBox();
            tb.Multiline = true;
            tb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            tb.Dock = System.Windows.Forms.DockStyle.Fill;
            tb.ReadOnly = true;
            dbg.Controls.Add(tb);
            dbg.MinimizeBox = false;
            dbg.MaximizeBox = false;
            dbg.Show();
            dbg.SetBounds(1340, 80, 580, 300);

            float x = 0;
            float y = 0;
            int scroll = 0;

            void PrintMouseInfo(DeviceEventArgs e)
            {
                x = e.X >= 0 ? e.X : x;
                y = e.Y >= 0 ? e.Y : y;
                scroll = e.ScrollValue >= 0 ? e.ScrollValue : scroll;
                cursorPos.Text = $"{x}:{y}:{scroll}";
            }

            Resources.AddJsonLoadResources(json);

            var logger = Logger.GetInstance();
            logger.Stream = new IOStream();
            logger.Stream.Write += (t) => tb.AppendText(t);

            using (var game = new GameView())
            {
                game.Graphics.GetGraphics().PreferredBackBufferWidth = 1300;
                game.Graphics.GetGraphics().PreferredBackBufferHeight = 800;
                game.Graphics.GetGraphics().ApplyChanges();

                game.Input.TouchEnable = false;
                game.Input.PositionChangedMouse += (s, e) => PrintMouseInfo(e);
                game.Input.ScrollingMouse += (s, e) => PrintMouseInfo(e);
                game.Exit += (s, e) => Environment.Exit(0);
                
                game.Run();
            }
        }
    }
#endif
}