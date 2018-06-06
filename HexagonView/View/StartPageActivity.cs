using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonView.View
{
    using WinSystem;
    using WinSystem.Controls;
    using WinSystem.System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class StartPageActivity : Activity
    {
        Button newGame = new Button() { Name = "newGameButton", Text = "New game" };
        Button settingsGame = new Button() { Name = "settingsGameButton", Text = "Settings" };

        public StartPageActivity(Activity parent) : base(parent)
        {
            var graphics = GraphicsSingleton.GetInstance();
            
            this.Items.Add(newGame);
            this.Items.Add(settingsGame);
        }

        public override void Designer()
        {
            base.Designer();

            int w = (Resources.GetResource("defaultButton") as Texture2D).Width;
            int h = (Resources.GetResource("defaultButton") as Texture2D).Height;

            newGame.Position = new Vector2(this.Width / 2 - w / 2, 100);
            settingsGame.Position = new Vector2(this.Width / 2 - w / 2, 200);
        }
        
    }
}
