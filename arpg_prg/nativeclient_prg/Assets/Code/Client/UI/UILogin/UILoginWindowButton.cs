using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using GameNet;
using LitJson;

namespace Client.UI
{
	public partial class UILoginWindow
	{
		private void _OnInitBottom(GameObject go)
		{
			_btnLogin = go.GetComponentEx<Button> (Layout.btn_longing);
			_btnWeChatLogin = go.GetComponentEx<Button> (Layout.btn_wechatLogin);
			_btnRegist = go.GetComponentEx<Button> (Layout.btn_regist);
			_btnfankui = go.GetComponentEx<Button> (Layout.btn_fankui);
			_btnModify = go.GetComponentEx<Button> (Layout.btn_modifypassword);
			_btnserver = go.GetComponentEx<Button> (Layout.btn_server);

			_phoneLogin = go.GetComponentEx<Button> (Layout.btn_PhoneLogin);

			//btn_login = go.GetComponentEx<Button> (Layout.btn_login);
			//img_tipword = go.GetComponentEx<Image> (Layout.img_tipword);

			img_phoneLoginbg = go.GetComponentEx<Image> (Layout.img_loginbg);
			img_bottombg = go.GetComponentEx<Image> (Layout.img_bottom);
			btn_showPhone = go.GetComponentEx<Button> (Layout.btn_PhoneLogin);

			btn_closePhoneBoard = go.GetComponentEx<Button> (Layout.btn_closePhoneBoard);
		}

		private void _OnShowBottom()
		{
			_btnserver.SetActiveEx (false);

			//Audio.AudioManager.Instance.StartMusic ();
            Audio.AudioManager.Instance.StartMusic();
            EventTriggerListener.Get (_btnLogin.gameObject).onClick+=_OnLoginHandler;
			EventTriggerListener.Get (_btnWeChatLogin.gameObject).onClick += _OnWeChatLoginHandler;
			EventTriggerListener.Get (_btnRegist.gameObject).onClick += _OnRegistHandler;

			EventTriggerListener.Get (_btnModify.gameObject).onClick += _OnModifyHandler;
			EventTriggerListener.Get (_btnserver.gameObject).onClick += _OnShowServerHandler;
			EventTriggerListener.Get (_btnfankui.gameObject).onClick += _OnfankuiHandler;

			EventTriggerListener.Get (btn_showPhone.gameObject).onClick += _OnShowPhoneBaordHandler;
			EventTriggerListener.Get (btn_closePhoneBoard.gameObject).onClick += _OnClosePhoneBoardHandler;

			_currentSever = System.Convert.ToString(_localConfig.LoadValue ("curServer", ""));

			_loginUsername.text=System.Convert.ToString(_localConfig.LoadValue (savedPhone, ""));

            _loginPassword.text =System.Convert.ToString( _localConfig.LoadValue(GameModel.GetInstance.UserPassword, ""));

            //EventTriggerListener.Get (btn_login.gameObject).onClick+=_OnLoginHandler;
            //var squence = DOTween.Sequence ();
            //squence.Append (img_tipword.transform.DOLocalMoveY (-240, 1f));
            //squence.Append (img_tipword.transform.DOLocalMoveY (-200,1f));
            //squence.SetLoops(int.MaxValue);

            NetWorkScript.getInstance ();

			if (_currentSever == "")
			{
				//SetServerName ("请选择服务器",false);
			}
			_HidePhoneBoard ();

            LitJson.JsonData data = new LitJson.JsonData();
            data["versionCode"] = GameModel.GetInstance.version;
            //data.ToJson()
            HttpRequestManager.GetInstance().GetCheckVersionNew(data.ToJson());
            //HttpRequestManager.GetInstance ().GetCheckVersionOld ();          
        }

		private void _OnHideBottom()
		{
			//EventTriggerListener.Get (btn_login.gameObject).onClick-=_OnLoginHandler;
			EventTriggerListener.Get (_btnLogin.gameObject).onClick-=_OnLoginHandler;
			EventTriggerListener.Get (_btnWeChatLogin.gameObject).onClick -= _OnWeChatLoginHandler;
			EventTriggerListener.Get (_btnRegist.gameObject).onClick -= _OnRegistHandler;

			EventTriggerListener.Get (_btnModify.gameObject).onClick -= _OnModifyHandler;
			EventTriggerListener.Get (_btnserver.gameObject).onClick -= _OnShowServerHandler;
			EventTriggerListener.Get (_btnfankui.gameObject).onClick -= _OnfankuiHandler;

			EventTriggerListener.Get (btn_showPhone.gameObject).onClick += _OnShowPhoneBaordHandler;
			EventTriggerListener.Get (btn_closePhoneBoard.gameObject).onClick += _OnClosePhoneBoardHandler;

			Audio.AudioManager.Instance.Stop ();
		}

		private void testTip()
		{
			Console.WriteLine ("sssslililiilll9999999999999");
		}

		private void _OnLoginHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (GameModel.GetInstance.isNeedNewVersion == true)
			{
				return;
			}		
			Console.WriteLine ("user login ......");
            //var tiptroll = UIControllerManager.Instance.GetController<UITipSureOrNotController> ();
            //tiptroll.setVisible (true);
            //tiptroll.SetTip ("wanhhaahhahahah",testTip);
            if (_loginUsername.text == "")
            {
                MessageHint.Show("请输入手机号");
                return;
            }

            if (_loginPassword.text == "")
            {
                Console.WriteLine("请输入密码");
                MessageHint.Show("请输入密码");
                return;
            }

            if (GameModel.IsTelephone(_loginUsername.text)==false)
			{
				Console.WriteLine ("手机号错误，请重新输入");
				MessageHint.Show ("请输入正确的手机号");
				_loginUsername.text = "";
				return;
			}	
//			if (_currentSever == "")
//			{
//				Console.WriteLine ("请选择服务器");
//				MessageHint.Show ("请选择服务器");			
//				return;
//			}
			var loginData = new LoginVo ();
			loginData.password = _loginPassword.text;
			loginData.phone = _loginUsername.text;
			loginData.playerType = 0;
			loginData.serverName = ""; //_currentSever;
			loginData.weChatId = "0";

			_localConfig.SaveValue ("curServer", _currentSever);
			var tmp = Coding<LoginVo>.encode (loginData);
            GameModel.HttpAcount = _loginUsername.text;

            //			JsonData lll =new JsonData();
            //			lll ["password"] = loginData.password;
            //			lll ["phone"] = loginData.phone;
            //lll ["playerType"] = loginData.playerType;
            //			lll ["serverName"] = loginData.serverName+"电信1";
            //lll ["weChatId"] = "weChatId";
            //			LitJson.JsonData ddd = new LitJson.JsonData ();
            //			ddd ["jsonString"] = lll;
            //			Console.WriteLine (ddd.ToJson());

            GameModel.GetInstance.ShowNetLoading();


			HttpRequestManager.GetInstance ().GetLoginData (tmp,_loginSuccess);
			_localConfig.SaveValue (savedPhone,_loginUsername.text);
            _localConfig.SaveValue(GameModel.GetInstance.UserPassword, _loginPassword.text);


//			Console.WriteLine (loginbackData);
			//return;
		}
        /// <summary>
        /// 登录成功的回调函数
        /// </summary>
		private void _loginSuccess()
		{
			if (null != _controller) 
			{
				_controller.IsLogin = true;

				//2016-9-21 zll fix
				var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
				controller.setVisible (true);
				//controller.LoadSeletRoleUI();
				controller.LoadGameHallUI();
				_controller.setVisible (false);

				NetWorkScript.getInstance ().ConnetServer (GameModel.GetInstance.myHandInfor.uuid);
			}
		}

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="go"></param>
		private void _OnWeChatLoginHandler(GameObject go)
		{
            //CallLoginWeiChat("testddddddddlllalala20180614444");
            //return;

            //			var chooseRoleNet = UIControllerManager.Instance.GetController<UIChooseRoleNetWindowController> ();
            //			chooseRoleNet.setVisible (true);

   //         if (GameModel.GetInstance.isNeedNewVersion == true)
			//{
			//	return;
			//}
			//Audio.AudioManager.Instance.BtnMusic ();
   //         CallLoginWeiChat("20180423yuantengfei02");
   //         return;
            //			var fightroomController = UIControllerManager.Instance.GetController<UIFightroomController> ();
            //			fightroomController.setVisible (true);
            //			return;
            //			var tmmp = UIControllerManager.Instance.GetController<UIGongGaoController> ();
            //			tmmp.setVisible (true);
            //			return;
            //			_controller.IsLogin = true;
            //			//2016-9-21 zll fix
            //			var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
            //			controller.setVisible (true);
            //			//controller.LoadSeletRoleUI();
            //			controller.LoadGameHallUI();
            //			_controller.setVisible (false);
            //			Audio.AudioManager.Instance.BtnMusic ();

            //			if (_currentSever == "")
            //			{
            //				Console.WriteLine ("请选择服务器");
            //				MessageHint.Show ("请选择服务器");			
            //				return;
            //			}

            //sdfsdfsdfsdfsddddd   weitest003
            //			CallLoginWeiChat ("sdfsdfsdfsdfsddddd111");

            string ti = PlayerPrefs.GetString (GameModel.GetInstance.WeChatLastLoginTime, "-1");
			if (ti != "-1" && ti != System.DateTime.Now.Day.ToString())
				PlayerPrefs.SetString (GameModel.GetInstance.UserId, "");

			string userid= PlayerPrefs.GetString (GameModel.GetInstance.UserId, "");

           

			if (userid != "") 
			{
                GameModel.GetInstance.ShowNetLoading();
                var loginController = UIControllerManager.Instance.GetController<Client.UI.UILoginController> ();
				loginController.WeChatLogin (userid);
                CallLoginWeiChat(userid);
                MBGame.Instance.SaveTimeLocal ();
			}
			else
				MBGame.Instance.LoginWeChat ();
		}

		/// <summary>
		/// Calls the login wei chat.获取到微信id后调用
		/// </summary>
		/// <param name="weChatID">We chat I.</param>
		public void CallLoginWeiChat(string weChatID)
		{
			var loginData = new LoginVo ();
			loginData.password = "";
			loginData.phone = "";
			loginData.playerType = 1;
			loginData.serverName = "";
			loginData.weChatId = weChatID;

            GameModel.HttpAcount = weChatID;

            _localConfig.SaveValue ("curServer", _currentSever);
			var tmp = Coding<LoginVo>.encode (loginData);
			HttpRequestManager.GetInstance ().GetLoginData (tmp,_loginSuccess);
		}

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="go"></param>
		private void _OnRegistHandler(GameObject go)
		{
			Console.WriteLine ("regist game");
			Audio.AudioManager.Instance.BtnMusic ();
			if (GameModel.GetInstance.isNeedNewVersion == true)
			{
				return;
			}
			var registcontroller = UIControllerManager.Instance.GetController<UIRegistController> ();
			registcontroller.setVisible (true);

		}

		/// <summary>
		/// Ons the modify handler. 点击显示修改密码面板
		/// </summary>
		/// <param name="go">Go.</param>
		private void _OnModifyHandler(GameObject go)
		{
			Console.WriteLine ("modify password -----");

			Audio.AudioManager.Instance.BtnMusic ();

			if (GameModel.GetInstance.isNeedNewVersion == true)
			{
				return;
			}

			var modifycontroller = UIControllerManager.Instance.GetController<UIModifyWindowController> ();
			modifycontroller.setVisible (true);
		}

		/// <summary>
		/// Onfankuis the handler. 点击，显示反馈面板
		/// </summary>
		/// <param name="go">Go.</param>
		private void _OnfankuiHandler(GameObject go)
		{
			Console.WriteLine ("show fankui -----");

			if (GameModel.GetInstance.isNeedNewVersion == true)
			{
				return;
			}

//			NetWorkScript.getInstance ().testGame ();
//			return;		
			//测试对战房间的

			//return;

			//Console.WriteLine( );
			//测试匹配等待的
			//var daoController = UIControllerManager.Instance.GetController<UITipWaitingWindowController> ();
			//daoController.setVisible (true);

//			var inforController = UIControllerManager.Instance.GetController<UIPlayerInforController> ();
//			inforController.windowType = 1;
//			inforController.setVisible (true);
		
			//var gonggao = UIControllerManager.Instance.GetController<UIGongGaoController> ();
			//gonggao.setVisible (true);
			//return;
			var tmpController = UIControllerManager.Instance.GetController<UIFellingWindowControll> ();
			tmpController.setVisible (true);
			/*LoginBackVoData logback = new LoginBackVoData ();
			logback.gameId = 100;
			logback.name = "hahha";
			logback.playerImg = "liuliu";

			LoginBackVo backvo = new LoginBackVo ();
			backvo.msg = "lalalal";
			backvo.status = 1;
			backvo.data = logback;

			Console.WriteLine(Coding<LoginBackVo>.encode(backvo));
			//"{status:1,msg:lalalal,data:{playerImg:liuliu,name:hahha,gameId:100}}
			var tmpData = Coding<LoginBackVo>.decode(Coding<LoginBackVo>.encode(backvo));
			Console.WriteLine ("-------------------------"+tmpData.data.gameId);*/
		}
        
	    /// <summary>
	    /// Ons the show server handler. 点击显示服务器
	    /// </summary>
	    /// <param name="go">Go.</param>
		private void _OnShowServerHandler(GameObject go)
		{
			Console.WriteLine ("show fuwuqi -----");
			if (GameModel.GetInstance.isNeedNewVersion == true)
			{
				return;
			}
			HttpRequestManager.GetInstance ().GetServerListData("",_handlerServersuccess);

			//var tmpList =serverList.data.list;
			/*var tmpList = new List<string> ();
			tmpList.Add ("广东1");
			tmpList.Add ("黑北一");
			tmpList.Add ("北京一");
			*/
//			for (int i = 0; i < tmpList.Count; i++)
//			{
//				uiServerControll.serverList .Add(tmpList [i].serverName);
//			}
			//uiServerControll.serverList=tmpList;
		}

        /// <summary>
        /// 选择服务器成功后
        /// </summary>
		private void _handlerServersuccess()
		{
			var uiServerControll = UIControllerManager.Instance.GetController<UISelectServerController> ();
			if (_currentSever == "")
			{
				uiServerControll.curServer = "选择服务器";
			}
			else 
			{
				uiServerControll.curServer = _currentSever;
			}		

			uiServerControll.serverList=HttpRequestManager.GetInstance().serviceList;
			uiServerControll.setVisible (true);
		}
	
		/// <summary>
		/// Sets the name of the server. 设计服务器的名字
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetServerName(string value,bool hasServer=true)
		{
			var str = "";
			if (hasServer == true)
			{
				str = "当前服务器:"+value;
				_currentSever = value;
			}
			else
			{
				str = value;
				Debug.Log ("-----------"+_currentSever);
			}
			//Console.WriteLine ("ssssssssssssssssssssss----------------------"+str); //Console.WriteLine (_btnserver.gameObject);
			_btnserver.gameObject.GetComponentInChildren<Text>().text=str;
		}

//		private void checkVersion()
//		{
//			var tmpStr = HttpRequestHelp.GetInstance ().GetCheckVersionData ();
//			var versionbackdata = Coding<CheckVersionBack>.decode(tmpStr);
//		}

		private void _OnClosePhoneBoardHandler(GameObject go)
		{
			_HidePhoneBoard ();
		}

		private void _OnShowPhoneBaordHandler(GameObject go)
		{
//			if (_currentSever == "")
//			{
//				Console.WriteLine ("请选择服务器");
//				MessageHint.Show ("请选择服务器");			
//				return;
//			}
			_ShowPhoneBoard ();
		}

		/// <summary>
		/// Hides the phone board.关闭手机登录面板
		/// </summary>
		private void _HidePhoneBoard()
		{
			img_bottombg.SetActiveEx (true);
			img_phoneLoginbg.SetActiveEx (false);
		}

		/// <summary>
		/// Shows the phone board.显示手机登录面板
		/// </summary>
		private void _ShowPhoneBoard()
		{
			img_bottombg.SetActiveEx (false);
			img_phoneLoginbg.SetActiveEx (true);
		}

		private Button _btnLogin;

		private Button _btnWeChatLogin;
		private Button _phoneLogin;

		private Button _btnRegist;

		private Button _btnfankui;
		private Button _btnModify;
		private Button _btnserver;

		private LocalConfigManager _localConfig = new LocalConfigManager ();

		/// <summary>
		/// The saved phone.保存登录的手机号
		/// </summary>
		private readonly string savedPhone = "savaloginphone";       

		//private Button btn_login;
		//private Image img_tipword;
		private Image img_phoneLoginbg;
		private Image img_bottombg;
		private Button btn_closePhoneBoard;
		private Button btn_showPhone;



			
	}
}

