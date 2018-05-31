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
        protected Thread threadActionCpu;
        protected GameObjectPositionInfo lastPosInfo = new GameObjectPositionInfo();
        protected StateMachine stateMachine;

        public bool IsReady { get; protected set; }
        public int Step { get; protected set; }
        public Map Map { get; set; }
        public TypeGameMode Mode { get; set; }

        public User User { get; set; }
        public List<CPU> CPUs { get; set; }

        public GameModeStrategy() : this(new GameSettings())
        {

        }

        public GameModeStrategy(GameSettings gameSettings)
        {
            this.Step = 0;
            this.Mode = gameSettings.GameMode;

            this.User = new User() { ID = 0 };

            this.CPUs = new List<CPU>();
            this.CPUs.Add(new CPU() { ID = 1, Strategy = new FirstStrategy() });
            this.CPUs.Add(new CPU() { ID = 2, Strategy = new FirstStrategy() });
            this.CPUs.Add(new CPU() { ID = 3, Strategy = new FirstStrategy() });

            this.Map = new Map(gameSettings.MapSize.Width, gameSettings.MapSize.Height);
            
            this.stateMachine = new StateMachine() { Map = this.Map };
            this.stateMachine.SetActivePlayer(this.User);
        }

        public void LoadContent()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            for (int row = 0; row < this.Map.Width; row++)
            {
                for (int col = 0; col < this.Map.Height; col++)
                {
                    this.Map.SetItem(new HexagonObject()
                    {
                        DefaultTexture = GameObject.GetTexture(TypeTexture.FieldFree),
                        Loot = r.Next(1, 10),
                        Life = r.Next(0, 2),
                        Font = GameObject.GetFont(TypeFonts.TextHexagon)
                    }, row, col);
                }
            }

            this.Map.SetItem(new HexagonObject() { DefaultTexture = GameObject.GetTexture(TypeTexture.UserIdle0), BelongUser = 0, Life = 2, Loot = 4 }, 1, 0);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = GameObject.GetTexture(TypeTexture.UserIdle1), Life = 4, Loot = 2 }, 2, 8);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = GameObject.GetTexture(TypeTexture.UserIdle2), Life = 2, Loot = 2 }, 8, 2);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = GameObject.GetTexture(TypeTexture.UserIdle3), Life = 16, Loot = 2 }, 9, 7);
        }

        public virtual void EndStep() { }
        public virtual void NextStep() { }
    }
}
