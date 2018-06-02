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
    using WinSystem.Controls;
    using WinSystem.System;

    public class Core : Container
    {
        private GameSettings gameSettings;

        public GameModeStrategy GameModeStrategy;

        public Core(GameSettings gameSettings)
        {
            this.gameSettings = gameSettings;

            this.Reset();
        }

        public void Reset()
        {
            this.GameModeStrategy = GameModeFactory.Create(this.gameSettings);
            this.GameModeStrategy.LoadContent();

            this.Items.Clear();
            this.Items.Add(this.GameModeStrategy.Map);
        }

        public void LoadContent()
        {
            this.Reset();
        }

        public override void Designer()
        {
            base.Designer();
            this.LoadContent();
        }
        public void Update()
        {
            // my logic
        }
    }
}
