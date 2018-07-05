using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

namespace HexagonLibrary
{
    using Entity.GameObjects;
    using Entity.Players;
    using Entity.Players.Strategy;
    using HexagonLibrary.Model.GameMode;
    using Microsoft.Xna.Framework;
    using Model.Navigation;
    using Model.StateMachines;
    using MonoGuiFramework.Controls;
    using MonoGuiFramework.System;
    using MonoGuiFramework.Base;

    public class Core : Container
    {
        private GameSettings gameSettings;

        public GameModeStrategy GameModeStrategy;
        
        public override int Width
        {
            get
            {
                return this.GameModeStrategy.Map.Rows[0].Sum(x => x.Width);
            }
        }

        public Core(GameSettings gameSettings)
        {
            this.Reset(gameSettings);
        }
        
        public void Reset(GameSettings gameSettings)
        {
            this.Name = "CoreHexagon";
            this.gameSettings = gameSettings;

            if (this.GameModeStrategy != null)
            {
                this.GameModeStrategy.Map.Items.Clear();
                this.GameModeStrategy.CPUs.Clear();
            }
                
            this.GameModeStrategy = GameModeFactory.Create(this.gameSettings);

            this.Items.Clear();
            this.Items.Add(this.GameModeStrategy.Map);
        }
        
        public override void Designer()
        {
            base.Designer();
        }

        public void Update()
        {

        }
    }
}
