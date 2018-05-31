using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem
{
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    using Controls;
    using System;

    public class Activity : Container
    {
        static int countActivity = 0;
        
        public Color Background { get; set; }
        public string Name { get; set; } = $"Activity{countActivity}";

        public Activity()
        {
            countActivity++;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsSingleton.GetInstance().GraphicsDevice.Clear(this.Background);

            GraphicsSingleton.GetInstance().GetSpriteBatch().Begin();
            base.Draw(gameTime);
            GraphicsSingleton.GetInstance().GetSpriteBatch().End();
        }
    }
}
