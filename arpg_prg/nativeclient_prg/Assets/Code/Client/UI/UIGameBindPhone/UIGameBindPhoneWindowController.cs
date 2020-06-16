using System;

namespace Client.UI
{
    /// <summary>
    /// 绑定手机号界面
    /// </summary>
	public class UIGameBindPhoneWindowController:UIController<UIGameBindPhoneWindow,UIGameBindPhoneWindowController>
	{

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uibindphone.ab";
			}
		}

		protected override void _Dispose ()
		{

		}

		protected override void _OnLoad ()
		{

		}

		protected override void _OnShow ()
		{

		}

		protected override void _OnHide ()
		{

		}

        /// <summary>
        /// 检测当前手机是否可以用
        /// </summary>
        /// <param name="value"></param>
		public void CheckPhoneState(int value)
		{
			if (null != _window && getVisible ())
			{
				(_window as UIGameBindPhoneWindow).CheckPhoneState (value);
			}
		}


		public UIGameBindPhoneWindowController ()
		{
		}
	}
}

