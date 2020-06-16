using System;
using Core;
using Client;
using Core.FSM;

namespace Client.GameFSM
{
    /// <summary>
    ///  状态机开始的状态
    /// </summary>
	public class StartState : FSMState
	{		
		public StartState (Game content)
			:base(content, FSMStateType.StartState)
		{
			
		}

        /// <summary>
        ///  如果在编辑器下，显示选择网络还是单机。手机上直接进入登录引导界面
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
		public override void Enter (Core.FSM.Event e, FSM.State lastState)
		{
#if UNITY_EDITOR
			UIUpdateSelectWindow.Toggle ();
#endif
		}

		protected override void _OnExit (Core.FSM.Event e, FSM.State nextState)
		{
#if UNITY_EDITOR
			UIUpdateSelectWindow.Toggle ();
#endif
		}

		protected override FSM.State _DoTick (float deltaTime)
		{
#if UNITY_EDITOR
			if (UIUpdateSelectWindow.UseLocalResource)
			{
				return new DownloadState(_Content);
			}

			if (UIUpdateSelectWindow.DownloadFromInternet)
			{
				UnityEditor.EditorUtility.DisplayDialog("Hint", "Temporarily not supported!", "ok");
				UIUpdateSelectWindow.DownloadFromInternet = false;
			}
#else
			return new UnpackState(_Content);
#endif
			return this;
		}
	}
}

