using System;
using UnityEngine;
using UnityEngine.UI;
using GameNet;

namespace Client.UI
{
	public partial class UIFeedBackWindow
	{

		private void _InitCenter(GameObject go)
		{
			btn_sure = go.GetComponentEx<Button> (Layout.btn_sure);
			btn_close = go.GetComponentEx<Button> (Layout.btn_close);
			txt_input = go.GetComponentEx<InputField> (Layout.text_input);
		}

		private void _ShowCenter()
		{
			EventTriggerListener.Get (btn_sure.gameObject).onClick += _OnSureHandler;
			EventTriggerListener.Get (btn_close.gameObject).onClick += _OnCloseHandler;

//			var str = "\\u3000\\u3000本基金公司新聘请一位明星基金经理，备受市场青睐。只有你能以此价格购买任意数量该基金。\\n每个人都可按此价格出售该基金。";
//			var str1 = str.Replace ("\\u3000", "\u3000");
//			var str2 = str1.Replace ("\\n","\n");
//			txt_input.text = str2;
		}

		private void _HideCenter()
		{
			EventTriggerListener.Get (btn_sure.gameObject).onClick -= _OnSureHandler;
			EventTriggerListener.Get (btn_close.gameObject).onClick -= _OnCloseHandler;		
		}

		private void _DisposeCenter()
		{
			
		}

		private void _OnCloseHandler(GameObject go)
		{
			_controller.setVisible (false);
		}

		private void _OnSureHandler(GameObject go)
		{
			if (txt_input.text == "")
			{
				Console.WriteLine ("请输入反馈建议");
				return;
			}

			var tmpData = new FankuiVo ();

			tmpData.input = txt_input.text;

			var str = Coding<FankuiVo>.encode (tmpData);

			//MessageHint.Show (str);

			//var backStr =  HttpRequestHelp.GetInstance ().GetFeedBackData (str);

			//var backStr = "";

			HttpRequestManager.GetInstance ().GetFeedBackData (str,_handlScuccess);

			//MessageHint.Show (backStr);


//			return;
//
//			var backVo = Coding<FankuiBackVo>.decode (backStr);
//
//			if (backVo.status == 0)//成功
//			{				
//				txt_input.text = "";
//			}
//			else
//			{
//				//失败
//			}
//
//			//Console.WriteLine (backStr);
//
//			MessageHint.Show (backVo.msg);
		
		}

		private void _handlScuccess()
		{
			txt_input.text = "";
		}

		private Button btn_sure;
		private Button btn_close;
		private InputField txt_input;
	}
}

