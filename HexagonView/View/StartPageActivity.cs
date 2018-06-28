using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonView.View
{
    using MonoGuiFramework;
    using MonoGuiFramework.Controls;
    using MonoGuiFramework.System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class StartPageActivity : Activity
    {
        Button newGame = new Button() { Name = "newGameButton", Scale = 2f };
        Button settingsGame = new Button() { Name = "settingsGameButton", ForeColor = Color.White, Scale = 2f };

        public StartPageActivity(Activity parent) : base(parent)
        {
            var graphics = GraphicsSingleton.GetInstance();
            
           
            this.Items.Add(newGame);
            this.Items.Add(settingsGame);
        }

        public override void Designer()
        {
            this.BackgroundImage = Resources.GetResource("background_startpage") as Texture2D;

            this.newGame.TextureManager.Textures.AddRange(Resources.GetResources(new List<string>()
            {
                "btn_newgame_idle",
                "btn_newgame_click"
            }).OfType<Texture2D>());

            this.settingsGame.TextureManager.Textures.AddRange(Resources.GetResources(new List<string>()
            {
                "btn_settings_idle",
                "btn_settings_click"
            }).OfType<Texture2D>());

            base.Designer();
            
            newGame.Position = new Vector2(this.Width / 2 - this.newGame.Width / 2, 100);
            settingsGame.Position = new Vector2(this.Width / 2 - this.settingsGame.Width / 2, this.newGame.Position.Y + this.newGame.Height + 60);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
