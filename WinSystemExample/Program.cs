using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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
                List<Color> colors = new List<Color>() { Color.White, Color.Red, Color.Green, Color.Blue, Color.Yellow };
                
                var label_example = new Label();
                label_example.Text = "Toggle Off";
                label_example.Position = new Vector2(20, 130);
                label_example.ForeColor = Color.White;
                winSystem.ActivitySelected.Items.Add(label_example);

                var btn_example = new Button();
                btn_example.Position = new Vector2(20, 80);
                btn_example.Text = "Example0";
                btn_example.OnClick += (s, ev) => { counter++; btn_example.Text = $"Example{counter}"; label_example.ForeColor = colors[counter % colors.Count]; };
                winSystem.ActivitySelected.Items.Add(btn_example);
                
                var toggle_example = new Toggle();
                toggle_example.Position = new Vector2(20, 170);
                toggle_example.IsChanged += (s, e) => label_example.Text = "Toggle " + (toggle_example.IsChecked ? "On" : "Off"); 
                winSystem.ActivitySelected.Items.Add(toggle_example);
                
                winSystem.LoadContentEvent += delegate (object sender, EventArgs e)
                {
                    
                };
                
                winSystem.UpdateEvent += delegate (object sender, EventArgs e)
                {
                    
                };

                winSystem.Graphics.Run();
            }
                
        }
    }
#endif
}
