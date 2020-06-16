using System;

namespace Client
{
    public class GameEventRoleTurnChanged : GameEventArgs
    {
        public GameEventRoleTurnChanged(int roleID)
            : base(GameEvents.RoleTurnChanged)
        {
            RoleIndex = roleID;
        }

        public int RoleIndex;
    }
}

