using Core;
using Core.Web;
using System;
using Core.FSM;
using System.Collections;
using System.Collections.Generic;

namespace Client.GameFSM
{
    /// <summary>
    ///  下载状态，从开始进入到下载新资源的状态
    /// </summary>
    public class DownloadState : FSMState
	{
		public DownloadState (Game content)
			:base(content, FSMStateType.DownloadState)
		{
		}
        /// <summary>
        ///  初始化卡牌数据，进入loginin状态
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        protected override FSM.State _DoTick (float deltaTime)
		{
			if (WebManager.IsInited ()) 
			{
                if (!_loaded)
                {
                    Metadata.MetadataManager.Instance.LoadMetadata();
                    _loaded = true;
                }
                else
                {
				    //return 	new LoadingState (_Content);	
					return new LoginState(_Content);
//					return new SelectRoleState(_Content);
                }
			}

			return this;
		}

        private bool _loaded;
	}
}

