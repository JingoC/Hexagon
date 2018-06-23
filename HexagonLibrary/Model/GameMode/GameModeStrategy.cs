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
        protected Random r = new Random((int)DateTime.Now.Ticks);

        protected Thread threadActionCpu;
        protected GameObjectPositionInfo lastPosInfo = new GameObjectPositionInfo();

        public bool IsReady { get; protected set; }
        public int Step { get; protected set; }
        public Map Map { get; set; }
        public GameSettings GameSettings { get; private set; }

        public List<Player> Players { get; set; } = new List<Player>();
        public User User { get; set; }
        public List<CPU> CPUs { get; set; }

        public event EventHandler FinishCpuStep;
        
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

            this.CPUs = new List<CPU>();

            if (gameSettings.GameMode == TypeGameMode.Normal)
            {
                this.User = new User() { ID = 0 };
                
                for (int i = 1; i < gameSettings.CountPlayers; i++)
                {
                    this.CPUs.Add(new CPU() { ID = i, Strategy = new FirstStrategy() });
                }
                
                this.Players.Add(this.User);
                this.Players.AddRange(this.CPUs);
            }
            else if (gameSettings.GameMode == TypeGameMode.Modeling)
            {
                for (int i = 0; i < gameSettings.CountPlayers; i++)
                {
                    this.CPUs.Add(new CPU() { ID = i, Strategy = new FirstStrategy() });
                }
                
                this.Players.AddRange(this.CPUs);
            }else if (gameSettings.GameMode == TypeGameMode.BuildMap)
            {
                this.User = new User() { ID = 0 };

                for (int i = 1; i < gameSettings.CountPlayers; i++)
                {
                    this.CPUs.Add(new CPU() { ID = i, Strategy = new BuildMapStrategy() });
                }

                this.Players.Add(this.User);
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

            foreach (var player in this.Players)
            {
                int row = 0;
                int column = 0;

                do
                {
                    row = r.Next(this.Map.Row);
                    column = r.Next(this.Map.Column);
                } while (this.Map.Rows[row][column].Type == TypeHexagon.Enemy);

                var hex = new HexagonObject() { MaxLife = 8, BelongUser = player.ID, Life = 2, Loot = 2, Type = TypeHexagon.Enemy };
                hex.SetDefaultTexture((TypeTexture)(TypeTexture.UserIdle0 + player.ID));

                this.Map.SetItem(hex, row, column);
            }
        }

        protected virtual HexagonObject GetMapItem()
        {
            int percentBlock = this.GameSettings.PercentBlocked;
            int percentBonus = this.GameSettings.PercentBonus;

            int allPercent = percentBlock + percentBonus;
            allPercent = allPercent < 100 ? 100 : allPercent;

            var v = r.Next(allPercent + 1);

            var loot = r.Next(this.GameSettings.ScatterLoot.Min, this.GameSettings.ScatterLoot.Max);
            var life = r.Next(this.GameSettings.ScatterLife.Min, this.GameSettings.ScatterLife.Max);
            var maxLife = this.GameSettings.IsAllEqualLife ? r.Next(8, 8) : r.Next(this.GameSettings.ScatterMaxLife.Min, this.GameSettings.ScatterMaxLife.Max);

            HexagonObject hex = new HexagonObject()
            {
                Loot = loot,
                Life = life,
                MaxLife = maxLife
            };
            hex.SetDefaultTexture(TypeTexture.FieldFree);

            if (v < percentBlock)
            {
                hex.Visible = false;
                hex.Type = TypeHexagon.Blocked;
            }
            else if (v < percentBonus)
            {
                hex.Bonus = (TypeHexagonBonus)r.Next(1, (int)TypeHexagonBonus.Last + 1);
                hex.SetDefaultTexture(TypeTexture.BonusBomb);
                hex.Life = 0;
            }
            else
            {
                
            }

            return hex;
        }

        public virtual void Suspend()
        {

        }

        public virtual void Resume()
        {

        }

        public virtual void EndStep()
        {
            
        }

        public virtual void NextStep()
        {
            if (this.FinishCpuStep != null)
                this.FinishCpuStep(this, EventArgs.Empty);
        }
    }
}
