using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Model.GameMode
{
    using HexagonLibrary.Entity.GameObjects;

    public class GameModeBuildMap : GameModeStrategy
    {
        public GameModeBuildMap() : this(new GameSettings())
        {

        }

        public GameModeBuildMap(GameSettings gameSettings) : base(gameSettings)
        {
            //this.stateMachine.ClickHisObject += StateMachine_ClickHisObject;
            //this.stateMachine.ClickNearObject += StateMachine_ClickAroundObject;
            
        }

        private void StateMachine_ClickAroundObject(object sender, StateMachines.ClickObjectsStateMachineEventArgs e)
        {
            if (sender is HexagonObject)
            {
                var hex = sender as HexagonObject;
                hex.BelongUser = this.User.ID;
                hex.TextureManager.Textures.Change(0);
            }
        }

        private void StateMachine_ClickHisObject(object sender, StateMachines.ClickObjectsStateMachineEventArgs e)
        {
            this.Map.Items.ForEach((x) => (x as HexagonObject).RestoreDefaultTexture());
            e.DestinationObject.TextureManager.Textures.Change((int)TypeTexture.UserActive0);
        }
    }
}
