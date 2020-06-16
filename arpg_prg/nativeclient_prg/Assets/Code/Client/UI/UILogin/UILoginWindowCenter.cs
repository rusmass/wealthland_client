using System;
using UnityEngine;
using UnityEngine.UI;

using System.Text.RegularExpressions;

namespace Client.UI
{
	public partial class UILoginWindow
	{
		private void _OnInitCenter(GameObject go)
		{
			_loginUsername = go.GetComponentEx<InputField> (Layout.lb_loginusename);
			_loginPassword = go.GetComponentEx<InputField> (Layout.lb_loginpassword);
//			_btnForget = go.GetComponentEx<Button> (Layout.btn_forget);
//			_toggleRember = go.GetComponentEx<Toggle> (Layout.toggle_rember);
			lb_version = go.GetComponentEx<Text> (Layout.lb_version);
//			if (null != _controller)
//			{
//				_controller.IsRember = _toggleRember.isOn;
//			}
			showAppVersion (go,"1.2");
            //Convert.ToBoolean()

           
		}

		private void _OnShowCenter()
		{
//			EventTriggerListener.Get(_btnForget.gameObject).onClick+=_OnForgetHandler;
//			_toggleRember.onValueChanged.AddListener (_OnToggleRemberHandler);				var tmps = "ssssssss_ssssdfsdfs$_sdfsdfsfdsfsf$_";
//			string[] tmplist = Regex.Split (tmps,"\\$_");
//			//string[] tmplist =tmps.Split(new char[2]{'$','_'});
//			Debug.LogError ("--------------"+tmplist.Length.ToString());
//			for (int i = 0; i < tmplist.Length; i++)
//			{
//				Debug.LogError ("ddddddd====------"+i+"---------"+(tmplist[i]=="").ToString());
//			}
			lb_version.text = string.Format("版本号:{0}",GameModel.GetInstance.version);
		}

        /// <summary>
        /// 显示是游戏版本
        /// </summary>
        /// <param name="tmpGO"></param>
        /// <param name="vesions"></param>
		private void showAppVersion(GameObject tmpGO , string vesions)
		{
			var tmptxt = tmpGO.GetComponentEx<Text> ("v3ersion");
			if (null != tmptxt)
			{
				tmptxt.text = vesions;
			}

//			txtObjet.transform.parent = bgimg.transform;
//			txtObjet.transform.localScale = Vector3.one;
//			txtObjet.transform.position = Vector3.one;
//			txtObjet.transform.position = new Vector3 (100,100,100);
//			tmptxt.text = vesions;		
		}


		private void _OnHideCenter()
		{
//			EventTriggerListener.Get(_btnForget.gameObject).onClick-=_OnForgetHandler;
//			_toggleRember.onValueChanged.RemoveListener (_OnToggleRemberHandler);

		}

//		private void _OnForgetHandler(GameObject go)
//		{
//			Console.WriteLine ("forget password");
//			Audio.AudioManager.Instance.BtnMusic ();
//
//		}

		private void _OnToggleRemberHandler(bool value)
		{
			Console.WriteLine (string.Format("is rembessr the infor , {0}",value));
			if(null != _controller)
			{
				_controller.IsRember=value;
				Audio.AudioManager.Instance.BtnMusic ();
			}

		}

		/// <summary>
		/// Sets the input phone number. 设置手机号
		/// </summary>
		public void SetInputPhoneNum(string value)
		{
			if (null != _loginUsername)
			{
				_loginUsername.text = value;
			}
		}


//		private Button _btnForget;
//		private Toggle _toggleRember;
		private InputField _loginPassword;
		private InputField _loginUsername;

		private string _currentSever="";

		private Text lb_version;
	}
}

