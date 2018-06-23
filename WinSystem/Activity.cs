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
        
        public Activity Parent { get; set; }
        public override int Width { get => GraphicsSingleton.GetInstance().Window.ClientBounds.Width; }
        public override int Height { get => GraphicsSingleton.GetInstance().Window.ClientBounds.Height; }

        public Color Background { get; set; }
        
        public virtual void ChangeActivity(bool active)
        {

        }

        public Activity(Activity parent)
        {
            this.Parent = parent;
            this.Name = $"Activity{countActivity}";
            countActivity++;
        }

        public override void Designer()
        {
            base.Designer();
        }

        public virtual void Update(GameTime gameTime)
        {

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
