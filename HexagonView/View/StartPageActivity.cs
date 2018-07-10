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
    using MonoGuiFramework.Base;

    using MonoGuiFramework.Containers;

    public class StartPageActivity : Activity
    {
        Button newGame;
        Button settingsGame;

        VerticalContainer menu;

        public StartPageActivity(Activity parent) : base(parent)
        {
            var graphics = GraphicsSingleton.GetInstance();
            this.Designer();
        }

        public override void Designer()
        {
            this.BackgroundImage = Resources.GetResource("background_startpage") as Texture2D;

            this.menu = new VerticalContainer();

            this.newGame = new Button(this.menu) { Name = "newGameButton", Scale = 2f };
            this.newGame.TextureManager.Textures.AddRange(Resources.GetResources(new List<string>()
            {
                "btn_newgame_idle",
                "btn_newgame_click"
            }).OfType<Texture2D>());
            this.newGame.SetBounds(0, 0, this.MaxWidth, this.MaxHeight);
            this.menu.Items.Add(this.newGame);

            this.settingsGame = new Button(this.menu) { Name = "settingsGameButton", Scale = 2f };
            this.settingsGame.TextureManager.Textures.AddRange(Resources.GetResources(new List<string>()
            {
                "btn_settings_idle",
                "btn_settings_click"
            }).OfType<Texture2D>());
            this.settingsGame.SetBounds(0, 80, this.MaxWidth, this.MaxHeight);
            this.menu.Items.Add(this.settingsGame);

            this.menu.SetBounds(Graphics.Width / 2 - this.menu.Width / 2, Graphics.Height / 2 - this.menu.Height / 2, this.menu.Width, this.menu.Height);

            this.Items.Add(this.menu);

            base.Designer();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
