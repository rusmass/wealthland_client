using Core.FSM;
using Client.Cameras;

namespace Client.CamerasFSM
{
	public class FSMState : FSM.State
    {
        public FSMState(SmartCamera content, FSMStateType type)
            : base(content)
        {
            _stateType = type;
        }

        public FSMStateType StateType { get { return _stateType; } }

        private FSMStateType _stateType;
    }
}
