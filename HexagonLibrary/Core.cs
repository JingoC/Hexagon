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
        public GameModeStrategy GameModeStrategy;

        public Core(GameSettings gameSettings)
        {
            this.GameModeStrategy = GameModeFactory.Create(gameSettings);

            this.Items.Add(this.GameModeStrategy.Map);
        }

        public void LoadContent()
        {
            this.GameModeStrategy.LoadContent();
        }

        public void Update()
        {
            // my logic
        }
    }
}
