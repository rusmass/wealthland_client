using System;
using UnityEngine;
using UnityEngine.UI;
using GameNet;
using LitJson;

namespace Client.UI
{
	public partial class UIModifyWindow
	{
		private void _initCenter(GameObject go)
		{

			btn_sure = go.GetComponentEx<Button> (Layout.btn_sure);
			btn_getRegist = go.GetComponentEx<Button> (Layout.btn_getIndentify);
			btn_sureModify = go.GetComponentEx<Button> (Layout.btn_sureModify);

			input_phone = go.GetComponentEx<InputField> (Layout.inputPhone);
			input_identify = go.GetComponentEx<InputField> (Layout.inputIdentify);
			input_scret = go.GetComponentEx<InputField> (Layout.inputScret);

			btn_close = go.GetComponentEx<Button> (Layout.btn_close);

			lb_time = go.GetComponentEx<Text> (Layout.lb_count);

			//			img_btnWord = go.GetComponentEx<Image> (Layout.btn_sure);
			//			Console.WriteLine (img_btnWord);
			//			img_uidisplay = new UIImageDisplay (img_btnWord);
		}


		private void _onShowCenter()
		{
			EventTriggerListener.Get (btn_sure.gameObject).onClick += _onBtnSureHandler;
			EventTriggerListener.Get (btn_sureModify.gameObject).onClick += _onBtnSureHandler;
			EventTriggerListener.Get (btn_getRegist.gameObject).onClick += _onBtnGetIdentifyHandler;
			EventTriggerListener.Get (btn_close.gameObject).onClick += _OnBtnCloseHandler;

			this._SetSureBtnWord ();

			if (_controller.isShowTime ())
			{
				ShowTimeCount ();				
			}
			else
			{
				HideTimeCount ();

			}
		}

		private void _onHideCenter()
		{
			EventTriggerListener.Get (btn_sure.gameObject).onClick -= _onBtnSureHandler;
			EventTriggerListener.Get (btn_sureModify.gameObject).onClick -= _onBtnSureHandler;
			EventTriggerListener.Get (btn_getRegist.gameObject).onClick -= _onBtnGetIdentifyHandler;
			EventTriggerListener.Get (btn_close.gameObject).onClick -= _OnBtnCloseHandler;
		}

		private void _onDisposeCenter()
		{
			//			if (null != img_uidisplay)
			//			{
			//				img_uidisplay.Dispose ();
			//			}
		}

		/// <summary>
		/// Shows the time count. 显示倒计时
		/// </summary>
		public void ShowTimeCount()
		{
			btn_getRegist.SetActiveEx (false);
			lb_time.SetActiveEx (true);
		}

		/// <summary>
		/// Updates the time number. 跟新倒计时时间
		/// </summary>
		/// <param name="vlaue">Vlaue.</param>
		public void UpdateTimeNum(string vlaue)
		{
			if (null != lb_time)
			{
				lb_time.text=vlaue;
			}
		}

		/// <summary>
		/// Hides the time count. 隐藏倒计时文本框
		/// </summary>
		public void HideTimeCount()
		{
			btn_getRegist.SetActiveEx (true);
			lb_time.SetActiveEx (false);
		}

		private void _onBtnSureHandler(GameObject go)
		{
			if (input_identify.text == "")
			{
				Console.WriteLine ("请入验证码");
				MessageHint.Show ("请入验证码");
				return;
			}

			//input_phone.text == "" || input_phone.text.Length < 11
			if (GameModel.IsTelephone(input_phone.text)==false)
			{
				Console.WriteLine ("请输入正确的手机号");
				MessageHint.Show ("手机号错误，请重新输入");
				input_phone.text = "";
				return;
			}

			if(input_scret.text=="")
			{
				Console.WriteLine ("请输入密码");
				MessageHint.Show ("请输入密码");
				return;
			}

			var scretModel = new PlayerRegistVo ();
			scretModel.code = input_identify.text;
			scretModel.password = input_scret.text;
			scretModel.phone = input_phone.text;

			//var tpm = new JsonData ();
			//tpm["code"]=scretModel.code ;
			//tpm["password"]=scretModel.password;
			//tpm ["phone"] = scretModel.phone;
			//var newData = new JsonData ();
			//newData ["jsonString"] = tpm;		


			var backdata = "";
			var tmpG = Coding<PlayerRegistVo>.encode (scretModel);

			Console.WriteLine ("jsonString:"+tmpG);

			Console.WriteLine ("修改密码成功sssssssss");
			HttpRequestManager.GetInstance ().GetModifyData (tmpG,_HandlerSuccess);

			Console.WriteLine (backdata);

			//			var backdatavo = Coding<PlayerRegistBackVo>.decode (backdata);
			//
			//			if(backdatavo.status==0)//chenggong
			//			{
			//				MessageHint.Show (backdatavo.msg);
			//			}
			//			else 
			//			{
			//				MessageHint.Show (backdatavo.msg);
			//			}

		}

		private void _HandlerSuccess()
		{
			var loginController = UIControllerManager.Instance.GetController<UILoginController> ();
			if (loginController.getVisible ())
			{
				loginController.SetDefaultPhone (input_phone.text);
			}

			_controller.setVisible (false);

		}


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
				MessageHint.Show ("手机号错误，请重新输入");
				input_phone.text = "";
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

		/// <summary>
		/// Sets the sure button word. 判断是修改秘吗还是注册账号
		/// </summary>
		/// <param name="value">Value.</param>
		private void _SetSureBtnWord()
		{
				
			btn_sure.SetActiveEx(false);
			btn_sureModify.SetActiveEx (true);


		}

		//private readonly string path_regist = "share/atlas/battle/login/phoneRiegitbtn.ab";
		//private readonly string path_modify = "share/atlas/battle/login/modifypassword.ab";

		private Button btn_sure;
		private Button btn_getRegist;
		private Button btn_sureModify;
		private Text lb_time;

		private Button btn_close;

		private Image img_btnWord;
		//		private UIImageDisplay img_uidisplay;

		private InputField input_phone;
		private InputField input_identify;
		private InputField input_scret;

		//private string identyCode="";
		//private string phoneNum="";

		//private InputField input_scretAgain;
	}
}

