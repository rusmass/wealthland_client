using System;
using UnityEngine.UI;
using UnityEngine;
using Client;
using GameNet;
using LitJson;

namespace Client.UI
{
	public partial class UIGameBindPhoneWindow
	{
		private void _initCenter(GameObject go)
		{		
			btn_getRegist = go.GetComponentEx<Button> (Layout.btn_getIndentify);
			btn_sureModify = go.GetComponentEx<Button> (Layout.btn_sureModify);

			input_phone = go.GetComponentEx<InputField> (Layout.inputPhone);
			input_identify = go.GetComponentEx<InputField> (Layout.inputIdentify);
			input_scret = go.GetComponentEx<InputField> (Layout.inputScret);

			btn_close = go.GetComponentEx<Button> (Layout.btn_close);

			bindTip = go.GetComponentEx<Text> (Layout.lb_bindtip);

			img_scretbg = go.GetComponentEx<Image> (Layout.img_scretbg);
		}


		private void _onShowCenter()
		{
			EventTriggerListener.Get (btn_sureModify.gameObject).onClick += _onBtnSureHandler;
			EventTriggerListener.Get (btn_getRegist.gameObject).onClick += _onBtnGetIdentifyHandler;
			EventTriggerListener.Get (btn_close.gameObject).onClick += _OnBtnCloseHandler;

			img_scretbg.SetActiveEx(false);

			input_phone.onEndEdit.AddListener(delegate {
				_checkPhoneData();
			});

			bindTip.SetActiveEx (false);
		}

		/// <summary>
		/// Checks the phone data.自测手机号
		/// </summary>
		private void _checkPhoneData()
		{
			if (GameModel.IsTelephone (input_phone.text) == true)
			{
				JsonData sendData = new JsonData ();
				sendData ["phone"] = input_phone.text;

				Console.WriteLine (sendData.ToJson());

				HttpRequestManager.GetInstance ().CheckPhoneNum (sendData.ToJson());
			}
		}

		/// <summary>
		/// Checks the state of the phone. 自测手机号的反馈
		/// </summary>
		/// <param name="value">Value.</param>
		public void CheckPhoneState(int value)
		{
			if (value == 0)
			{
				isCheckRightState = 0;
				bindTip.text = "当前号码已被绑定";
			} 
			else if (value == 1)
			{
				isCheckRightState = 1;
				bindTip.text = "当前号码可使用";
				img_scretbg.SetActiveEx (false);
			}
			else if (value == 2)
			{
				isCheckRightState = 2;
				bindTip.text = "当前号码可使用";
				img_scretbg.SetActiveEx (true);
			}

			bindTip.SetActiveEx (true);
		}

		private int isCheckRightState = 0;


		private void _onHideCenter()
		{
			EventTriggerListener.Get (btn_sureModify.gameObject).onClick -= _onBtnSureHandler;
			EventTriggerListener.Get (btn_getRegist.gameObject).onClick -= _onBtnGetIdentifyHandler;
			EventTriggerListener.Get (btn_close.gameObject).onClick -= _OnBtnCloseHandler;

			input_phone.onEndEdit.RemoveAllListeners ();
		}

		private void _onDisposeCenter()
		{
			//			if (null != img_uidisplay)
			//			{
			//				img_uidisplay.Dispose ();
			//			}
		}
        /// <summary>
        /// 点击绑定说手机号按钮
        /// </summary>
        /// <param name="go"></param>
		private void _onBtnSureHandler(GameObject go)
		{
			
			if (input_identify.text == "")
			{
				Console.WriteLine ("请入验证码");
				MessageHint.Show ("请入验证码");
				return;
			}

			if (input_identify.text != HttpRequestManager.GetInstance ().identyCode)
			{
				MessageHint.Show ("请输入正确的验证码");
				return;
			}

			//input_phone.text == "" || input_phone.text.Length < 11
			if (GameModel.IsTelephone(input_phone.text)==false)
			{
				Console.WriteLine ("请输入正确的手机号");
				MessageHint.Show ("请输入正确的手机号");
				return;
			}

			if (isCheckRightState == 0)
			{
				Console.WriteLine ("手机认证失败");
				MessageHint.Show ("当前手机不可绑定，请输入正确的手机号");
				return;
			}

			if (isCheckRightState == 2)
			{
				if(input_scret.text=="")
				{
					Console.WriteLine ("请输入密码");
					MessageHint.Show ("请输入密码");
					return;
				}
			}

			var jsonData = new JsonData ();

			jsonData["code"]=input_identify.text;
			jsonData["password"]=input_scret.text;
			jsonData ["phone"] = input_phone.text;
			jsonData ["account"] = GameModel.HttpAcount;

//			var scretModel = new PlayerRegistVo ();
//			scretModel.code = input_identify.text;
//			scretModel.password = input_scret.text;
//			scretModel.phone = input_phone.text;
//			var backdata = "";

			var tmpG =jsonData.ToJson() ;//Coding<PlayerRegistVo>.encode (scretModel);

			HttpRequestManager.GetInstance ().BindPhone (tmpG,_BindPhoneDirect);

			Console.WriteLine ("jsonString:"+tmpG);
//			Console.WriteLine (backdata);
//			_controller.setVisible (false);
		}

        /// <summary>
        /// 绑定手机后的回调函数
        /// </summary>
		private void _BindPhoneDirect()
		{
			var logcontroller = Client.UIControllerManager.Instance.GetController<UILoginController> ();
			if (logcontroller.getVisible () == true)
			{
				logcontroller.setVisible (false);
			}

			var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
			controller.setVisible (true);
			//controller.LoadSeletRoleUI();
			controller.LoadGameHallUI();
			_controller.setVisible (false);
			NetWorkScript.getInstance ().ConnetServer (GameModel.GetInstance.myHandInfor.uuid);
			_controller.setVisible (false);
		}


        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="go"></param>
		private void _OnBtnCloseHandler(GameObject go)
		{
			this._controller.setVisible (false);
		}

		/// <summary>
		/// Ons the button get identify handler. 获取验证码
		/// </summary>
		/// <param name="go">Go.</param>
		private void _onBtnGetIdentifyHandler(GameObject go)
		{
			var phoneNum = input_phone.text;

			//			if ( phoneNum== "" ||phoneNum.Length<11)
			//			{
			//				MessageHint.Show ("请输入正确的手机号");
			//				return;
			//			}

			if (GameModel.IsTelephone (phoneNum) == false)
			{
				MessageHint.Show ("请输入正确的手机号"+phoneNum);
				return;
			}

			var getcodemodel = new PlayerGetCode ();
			getcodemodel.phone = phoneNum;


			LitJson.JsonData data = new LitJson.JsonData ();
			data ["phone"] = getcodemodel.phone;

			HttpRequestManager.GetInstance ().GetCheckCodeData(data.ToJson());
			//HttpRequestManager.GetInstance ().GetCheckCodeData(data.ToJson());
			//			Console.WriteLine ("jsonString="+data.ToJson());
			//Coding<PlayerGetCode>.encode (getcodemodel);
			//			var getCodeStr = HttpRequestHelp.GetInstance ().GetCheckCodeData(data.ToJson());
			//			var getcodeback = Coding<PlayerGetCodeBackVo>.decode (getCodeStr);
			//
			//			Console.WriteLine (getCodeStr);
			//
			//			if(getcodeback.status==0)//成功
			//			{
			//				identyCode = getcodeback.data.code;
			//				MessageHint.Show ("已经发验证码，注意接收");
			//			}
			//			else
			//			{
			//				Console.WriteLine (getcodeback.msg);
			//			}

		}



		//private readonly string path_regist = "share/atlas/battle/login/phoneRiegitbtn.ab";
		//private readonly string path_modify = "share/atlas/battle/login/modifypassword.ab";


		private Button btn_getRegist;
		private Button btn_sureModify;

		private Button btn_close;

		private Image img_btnWord;
		//		private UIImageDisplay img_uidisplay;

		private InputField input_phone;
		private InputField input_identify;
		private InputField input_scret;

		//private string identyCode="";
		//private string phoneNum="";

		private Text bindTip;

		private Image img_scretbg;

		//private InputField input_scretAgain;
	}
}

