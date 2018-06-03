﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem.Controls
{
    using WinSystem.System;

    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    public class Changer : Container
    {
        Button btnDown = new Button();
        Button btnUp = new Button();
        Label labelValue = new Label();

        public event EventHandler ClickToDown;
        public event EventHandler ClickToUp;

        public string Text { get => this.labelValue.Text; set => this.labelValue.Text = value; }
        public Color ForeColor { get => this.labelValue.ForeColor; set => this.labelValue.ForeColor = value; }

        public Changer()
        {
            this.Items.Add(this.btnDown);
            this.Items.Add(this.labelValue);
            this.Items.Add(this.btnUp);
        }
        
        public override void Designer()
        {
            this.btnDown.Name = "Down";
            this.btnDown.Position = new Vector2(0, 0);
            this.btnDown.TextureManager.Textures.Add(Resources.GetResource("defaultChangerDown") as Texture2D);
            this.btnDown.OnClick += (s, e) => this.ClickExecute(this.ClickToDown);

            this.labelValue.Name = "Value";
            this.labelValue.ForeColor = Color.White;
            this.labelValue.Position = new Vector2(this.btnDown.Width + 2, 0);

            this.btnUp.Name = "Up";
            this.btnUp.Position = new Vector2(this.labelValue.Position.X + 26, 0);
            this.btnUp.TextureManager.Textures.Add(Resources.GetResource("defaultChangerUp") as Texture2D);
            this.btnUp.OnClick += (s, e) => this.ClickExecute(this.ClickToUp);
            
            base.Designer();
        }

        private void ClickExecute(EventHandler click)
        {
            if (click != null)
                click(this, EventArgs.Empty);
        }

        public override void Draw(GameTime gameTime)
        {
            float ofsX = this.Position.X;
            float ofsY = this.Position.Y;
            this.btnDown.Position = new Vector2(ofsX, ofsY);
            this.labelValue.Position = new Vector2(this.btnDown.Position.X + this.btnDown.Width, ofsY + (this.btnDown.Height / 4) - 6);
            this.btnUp.Position = new Vector2(this.labelValue.Position.X + this.labelValue.Width + 20, ofsY);
            base.Draw(gameTime);
        }
    }
}
