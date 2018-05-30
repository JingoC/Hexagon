using System;
using System.Linq;

namespace WinSystemExample
{
#if WINDOWS || LINUX

    using WinSystem;
    using WinSystem.Controls;

    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    using HexagonLibrary;

    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        static int counter = 0;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var winSystem = new WSystem())
            {
                winSystem.SetResource("test_button", TypeResource.Texture);
                winSystem.SetResource("buttonFont", TypeResource.Font);
                
                var btn = new Button();
                btn.Click += delegate (object sender, EventArgs e)
                {
                    btn.Text = counter.ToString();
                    counter++;
                };
                
                var btn_next = new Button();
                btn_next.Position = new Vector2(0, 80);
                btn_next.Text = "Start game";
                btn_next.Click += (s, e) => winSystem.SelectActivity("gameActivity");

                winSystem.ActivitySelected.Items.Add(btn);
                winSystem.ActivitySelected.Items.Add(btn_next);

                Core core = new Core();
                Activity coreActivity = new Activity();
                coreActivity.Items.Add(core);
                coreActivity.Name = "gameActivity";
                coreActivity.Background = Color.Aqua;

                winSystem.Activities.Add(coreActivity);

                winSystem.LoadContentEvent += delegate (object sender, EventArgs e)
                {
                    btn.Texture = winSystem.GetResource("test_button") as Texture2D;
                    btn.Font = winSystem.GetResource("buttonFont") as SpriteFont;

                    btn_next.Texture = winSystem.GetResource("test_button") as Texture2D;
                    btn_next.Font = winSystem.GetResource("buttonFont") as SpriteFont;

                    core.LoadContent();
                };
                
                winSystem.UpdateEvent += delegate (object sender, EventArgs e)
                {
                    core.Update();
                };

                winSystem.Graphics.Run();
            }
                
        }
    }
#endif
}
