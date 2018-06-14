using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HexagonLibrary.Model.GameMode
{
    using Model.Navigation;
    using Entity.Players;
    using Entity.Players.Strategy;
    using Entity.GameObjects;
    using Model.StateMachines;
    using WinSystem.Controls;

    public class GameModeStrategy
    {
        Random r = new Random((int)DateTime.Now.Ticks);

        protected Thread threadActionCpu;
        protected GameObjectPositionInfo lastPosInfo = new GameObjectPositionInfo();
        protected StateMachine stateMachine;

        public bool IsReady { get; protected set; }
        public int Step { get; protected set; }
        public Map Map { get; set; }
        public GameSettings GameSettings { get; private set; }

        public List<Player> Players { get; set; } = new List<Player>();
        public User User { get; set; }
        public List<CPU> CPUs { get; set; }

        public GameModeStrategy() : this(new GameSettings())
        {

        }

        public GameModeStrategy(GameSettings gameSettings)
        {
            this.Step = 0;
            this.GameSettings = gameSettings;

            HexagonObject.LifeEnable = gameSettings.ViewLifeEnable;
            HexagonObject.LootEnable = gameSettings.ViewLootEnable;
            HexagonObject.MaxLifeEnable = gameSettings.ViewMaxLife;

            this.Map = new Map(gameSettings.MapSize.Width, gameSettings.MapSize.Height);
            this.stateMachine = new StateMachine() { Map = this.Map };

            this.CPUs = new List<CPU>();

            if (gameSettings.PlayerMode == TypePlayerMode.Normal)
            {
                this.User = new User() { ID = 0 };
                
                for (int i = 1; i < gameSettings.CountPlayers; i++)
                {
                    this.CPUs.Add(new CPU() { ID = i, Strategy = new FirstStrategy() });
                }

                this.stateMachine.SetActivePlayer(this.User);
                this.Players.Add(this.User);
                this.Players.AddRange(this.CPUs);
            }
            else if (gameSettings.PlayerMode == TypePlayerMode.Modeling)
            {
                for (int i = 0; i < gameSettings.CountPlayers; i++)
                {
                    this.CPUs.Add(new CPU() { ID = i, Strategy = new FirstStrategy() });
                }

                this.stateMachine.SetActivePlayer(this.CPUs[0]);
                
                this.Players.AddRange(this.CPUs);
            }

            this.Generate();
        }

        void Generate()
        {
            for (int row = 0; row < this.Map.Row; row++)
            {
                for (int col = 0; col < this.Map.Column; col++)
                {
                    this.Map.SetItem(this.GetMapItem(), row, col);
                }
            }

            foreach(var player in this.Players)
            {
                int row = 0;
                int column = 0;
                
                do
                {
                    row = r.Next(this.Map.Row);
                    column = r.Next(this.Map.Column);
                } while (this.Map.Rows[row][column].Type != TypeHexagon.Free);

                var hex = new HexagonObject() { MaxLife = 8, BelongUser = player.ID, Life = 2, Loot = 2 };
                hex.SetDefaultTexture((TypeTexture)(TypeTexture.UserIdle0 + player.ID));
                
                this.Map.SetItem(hex, row, column);
            }
        }

        private HexagonObject GetMapItem()
        {
            int percentBlock = 20;

            if (r.Next(101) < percentBlock)
            {
                HexagonObject hex = new HexagonObject()
                {
                    Visible = false,
                    Type = TypeHexagon.Blocked
                };
                hex.SetDefaultTexture(TypeTexture.FieldFree);

                return hex;
            }
            else
            {
                var hex = new HexagonObject()
                {
                    Loot = r.Next(this.GameSettings.ScatterLoot.Min, this.GameSettings.ScatterLoot.Max),
                    Life = r.Next(this.GameSettings.ScatterLife.Min, this.GameSettings.ScatterLife.Max),
                    MaxLife = this.GameSettings.IsAllEqualLife ? r.Next(8, 8) : r.Next(this.GameSettings.ScatterMaxLife.Min, this.GameSettings.ScatterMaxLife.Max)
                };
                hex.SetDefaultTexture(TypeTexture.FieldFree);

                return hex;
            }
        }

        public virtual void EndStep() { }
        public virtual void NextStep() { }
    }
}
