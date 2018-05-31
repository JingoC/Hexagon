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
    using WinSystem.System;
    using HexagonLibrary.Model.GameMode;

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
                Core core = new Core(new GameSettings());

                winSystem.LoadContentEvent += delegate (object sender, EventArgs e)
                {
                    Resources.AddResource("test_button", TypeResource.Texture2D);

                    var btn = new Button();
                    btn.Texture = Resources.GetResource("test_button") as Texture2D;
                    btn.OnClick += delegate (object s, EventArgs ev)
                    {
                        btn.Text = counter.ToString();
                        counter++;
                    };

                    var btn_next = new Button();
                    btn_next.Position = new Vector2(0, 80);
                    btn_next.Text = "Start game";
                    btn_next.Texture = Resources.GetResource("test_button") as Texture2D;
                    btn_next.OnClick += (s, ev) => winSystem.SelectActivity("gameActivity");
                    
                    var defBtn = new Button();
                    defBtn.Position = new Vector2(0, 160);
                    defBtn.Text = "Default";
                    defBtn.OnClick += (s, ev) => winSystem.SelectActivity("gameActivity");
                    
                    winSystem.ActivitySelected.Items.Add(btn);
                    winSystem.ActivitySelected.Items.Add(btn_next);
                    winSystem.ActivitySelected.Items.Add(defBtn);
                    
                    Activity coreActivity = new Activity();
                    coreActivity.Items.Add(core);
                    coreActivity.Name = "gameActivity";
                    coreActivity.Background = Color.Aqua;
                    winSystem.Activities.Add(coreActivity);

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
