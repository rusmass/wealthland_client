using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIConditionWindow
	{
		private void _OnInitCenter(GameObject go)
		{
			lb_title = go.GetComponentEx<Text> (Layout.lb_title);
			lb_infor = go.GetComponentEx<Text> (Layout.lb_infor);

			var tmpimg = go.GetComponentEx<Image> (Layout.img_title);
			img_title = new UIImageDisplay (tmpimg);

			btn_sure=go.GetComponentEx<Button>(Layout.btn_sure);

		}

		private void _OnShowCenter()
		{
			EventTriggerListener.Get (btn_sure.gameObject).onClick += _OnSureHandler;
			if (null != _controller)
			{
				_conditionType = _controller.showConditionType;

				if (_conditionType == 1)
				{
					lb_title.text = txt_successtitle;
					lb_infor.text = txt_successInfor;

					if(null !=img_title)
					{
						img_title.Load (lb_path);
					}
				}
			}
		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get (btn_sure.gameObject).onClick -= _OnSureHandler;
		}

		private void _OnDisposeCenter()
		{
			if (null != img_title)
			{
				img_title.Dispose ();
			}
		}

		private void _OnSureHandler(GameObject go)
		{
			if (_conditionType == 0)
			{
				var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
				if (null != controller)
				{
					controller.ConditionStartGame ();
				}
			}
			else if(_conditionType==1)
			{
				var tmpController = UIControllerManager.Instance.GetController<UIEnterInnerWindowController> ();
				if (null != tmpController)
				{
					tmpController.ConditionSuccess ();
				}
			}

			_controller.setVisible (false);
		}

		private string txt_successtitle="如何从“内圈”进入“核心圈”？";
		private string txt_successInfor="玩家可以选择随时与银行核对其财务报表以确认是否符合胜利的标准， 核对的内容如下：\nA、流动现金增加量 ＞ 20万\nB、时间积分＞1000分\nC、生活品质积分＞100分\n";

		private string lb_path = "share/atlas/battle/waiquanjiaoyi/qa_ruheyingdeyouxi.ab";

		private Text lb_title;
		private Text lb_infor;
		private UIImageDisplay img_title;

		private int _conditionType=0;


		private Button btn_sure;

	}
}

