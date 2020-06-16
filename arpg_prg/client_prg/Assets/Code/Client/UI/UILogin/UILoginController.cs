using System;

namespace Client.UI
{
	public class UILoginController : UIController<UILoginWindow, UILoginController>
	{
		//		protected override string _windowResource { get { return "prefabs/ui/scene/uilogingame.ab";} }
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uilogingame.ab";
			}
		}

		public UILoginController()
		{
			
		}

		protected override void _OnLoad ()
		{
			base._OnLoad ();
		}

		protected override void _OnShow ()
		{
			base._OnShow ();
		}

		protected override void _OnHide ()
		{
			base._OnHide ();
		}

		public override void Tick (float deltaTime)
		{
			base.Tick (deltaTime);
		}

		protected override void _Dispose ()
		{
			
			base._Dispose ();
		}

		public bool IsLogin
		{
			get
			{
				return _isLogin;
			}

			set
			{
				_isLogin = value;
			}
		}


		public bool IsRember
		{
			get
			{
				return _isRember;
			}

			set
			{
				_isRember = value;
			}
			
		}

		/// <summary>
		/// Sets the default phone.设置游戏默认的手机号
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetDefaultPhone(string value)
		{
			if (null != _window && getVisible ())
			{
				(_window as UILoginWindow).SetInputPhoneNum (value);
			}
		}

		public void SetServerName(string name,bool rightServer)
		{
			if (null != _window && getVisible()==true)
			{
				(_window as UILoginWindow).SetServerName (name,rightServer);
			}			
		}

		/// <summary>
		/// Wes the chat login.回调微信登录
		/// </summary>
		/// <param name="weChatId">We chat identifier.</param>
		public void WeChatLogin(string weChatId)
		{
			if (null != _window && getVisible()==true)
			{
				(_window as UILoginWindow).CallLoginWeiChat(weChatId);
			}		
		}

		private bool _isRember;
		private bool _isLogin=false;

	
	}
}

