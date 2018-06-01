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

        public User User { get; set; }
        public List<CPU> CPUs { get; set; }

        public GameModeStrategy() : this(new GameSettings())
        {

        }

        public GameModeStrategy(GameSettings gameSettings)
        {
            this.Step = 0;
            this.Mode = gameSettings.GameMode;

            this.CPUs = new List<CPU>();

#if MODEL
            this.CPUs.Add(new CPU() { ID = 0, Strategy = new FirstStrategy() });
#else
            this.User = new User() { ID = 0 };
#endif
            
            this.CPUs.Add(new CPU() { ID = 1, Strategy = new FirstStrategy() });
            this.CPUs.Add(new CPU() { ID = 2, Strategy = new FirstStrategy() });
            this.CPUs.Add(new CPU() { ID = 3, Strategy = new FirstStrategy() });

            this.Map = new Map(gameSettings.MapSize.Width, gameSettings.MapSize.Height);
            
            this.stateMachine = new StateMachine() { Map = this.Map };

#if MODEL
            this.stateMachine.SetActivePlayer(this.CPUs[0]);
#else
            this.stateMachine.SetActivePlayer(this.User);
#endif
        }

        public void LoadContent()
        {
            for (int row = 0; row < this.Map.Width; row++)
            {
                for (int col = 0; col < this.Map.Height; col++)
                {
                    this.Map.SetItem(this.GetMapItem(), row, col);
                }
            }

            this.Map.SetItem(new HexagonObject() { DefaultTexture = GameObject.GetTexture(TypeTexture.UserIdle0), MaxLife = 8, BelongUser = 0, Life = 2, Loot = 2 }, 1, 0);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = GameObject.GetTexture(TypeTexture.UserIdle1), MaxLife = 8, BelongUser = 1, Life = 2, Loot = 2 }, 2, 8);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = GameObject.GetTexture(TypeTexture.UserIdle2), MaxLife = 8, BelongUser = 2, Life = 2, Loot = 2 }, 8, 2);
            this.Map.SetItem(new HexagonObject() { DefaultTexture = GameObject.GetTexture(TypeTexture.UserIdle3), MaxLife = 8, BelongUser = 3, Life = 2, Loot = 2 }, 9, 7);
        }

        private HexagonObject GetMapItem()
        {
            int percentBlock = 5;

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
