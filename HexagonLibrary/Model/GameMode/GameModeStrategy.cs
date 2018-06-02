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

    public class GameModeStrategy
    {
        Random r = new Random((int)DateTime.Now.Ticks);

        protected Thread threadActionCpu;
        protected GameObjectPositionInfo lastPosInfo = new GameObjectPositionInfo();
        protected StateMachine stateMachine;

        public bool IsReady { get; protected set; }
        public int Step { get; protected set; }
        public Map Map { get; set; }
        public TypeGameMode Mode { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();
        public User User { get; set; }
        public List<CPU> CPUs { get; set; }

        public GameModeStrategy() : this(new GameSettings())
        {

        }

        public GameModeStrategy(GameSettings gameSettings)
        {
            this.Step = 0;
            this.Mode = gameSettings.GameMode;

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
        }

        public void LoadContent()
        {
            for (int row = 0; row < this.Map.Height; row++)
            {
                for (int col = 0; col < this.Map.Width; col++)
                {
                    this.Map.SetItem(this.GetMapItem(), row, col);
                }
            }

            foreach(var cpu in this.CPUs)
            {
                int row = 0;
                int column = 0;

                do
                {
                    row = r.Next(this.Map.Width);
                    column = r.Next(this.Map.Height);
                } while (this.Map.Rows[row][column].Type != TypeHexagon.Free);

                this.Map.SetItem(new HexagonObject()
                {
                    DefaultTexture = GameObject.GetTexture((TypeTexture)(TypeTexture.UserIdle0 + cpu.ID)),
                    MaxLife = 8,
                    BelongUser = cpu.ID,
                    Life = 2,
                    Loot = 2
                }, row, column);
            }
        }

        private HexagonObject GetMapItem()
        {
            int percentBlock = 20;

            if (r.Next(101) < percentBlock)
            {
                return new HexagonObject()
                {
                    DefaultTexture = GameObject.GetTexture(TypeTexture.FieldFree),
                    Visible = false,
                    Type = TypeHexagon.Blocked
                };
            }
            else
            {
                return new HexagonObject()
                {
                    DefaultTexture = GameObject.GetTexture(TypeTexture.FieldFree),
                    Loot = r.Next(0, 2),
                    Life = r.Next(0, 2),
                    MaxLife = r.Next(8, 8),
                    Font = GameObject.GetFont(TypeFonts.TextHexagon)
                };
            }
        }

        public virtual void EndStep() { }
        public virtual void NextStep() { }
    }
}
