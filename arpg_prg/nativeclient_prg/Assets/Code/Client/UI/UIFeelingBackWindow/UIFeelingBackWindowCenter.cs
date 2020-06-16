using System;
using UnityEngine;
using UnityEngine.UI;
using GameNet;
using LitJson;

namespace Client.UI
{
	public partial class UIFeelingBackWindow
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
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="go"></param>
		private void _OnCloseHandler(GameObject go)
		{
			_controller.setVisible (false);
		}

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="go"></param>
		private void _OnSureHandler(GameObject go)
		{
			if (txt_input.text == "")
			{
				Console.WriteLine ("请输入反馈建议");
				return;
			}

            var tmpData = new JsonData();
            tmpData["input"] = txt_input.text;
            tmpData["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
            HttpRequestManager.GetInstance().GetFeelingBackData(tmpData.ToJson(), _handlScuccess);

            //var tmpData = new JsonData();
            //tmpData["page"] = 1;
            //HttpRequestManager.GetInstance().GetFeelingShareData(tmpData.ToJson());

            //var tmpData = new JsonData();
            //tmpData["page"] = 1;
            //tmpData["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

            //HttpRequestManager.GetInstance().GetFeelSelfData(tmpData.ToJson());

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

