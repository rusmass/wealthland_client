using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIEnterInnerWindow
	{
		private void _OnInitCenter(GameObject go)
		{
			lb_income= go.GetComponentEx<Text>(Layout.lb_income);
			lb_incomequal= go.GetComponentEx<Text>(Layout.lb_incomequal);
			lb_incometarget= go.GetComponentEx<Text>(Layout.lb_incometarget);

			lb_timetxt= go.GetComponentEx<Text>(Layout.lb_timetxt);
			lb_timenqual= go.GetComponentEx<Text>(Layout.lb_timenqual);
			lb_timetarget= go.GetComponentEx<Text>(Layout.lb_timetarget);

			lb_qualitytxt= go.GetComponentEx<Text>(Layout.lb_qualitytxt);
			lb_qualtyenqual= go.GetComponentEx<Text>(Layout.lb_qualtyenqual);
			lb_qualitytarget= go.GetComponentEx<Text>(Layout.lb_qualitytarget);

			lb_incomeDesc = go.GetComponentEx<Text> (Layout.lb_incomeDesc);
			lb_timeDesc = go.GetComponentEx<Text> (Layout.lb_timeDesc);
			lb_qualityDesc = go.GetComponentEx<Text> (Layout.lb_qualityDesc);

            lb_countTime = go.GetComponentEx<Text>(Layout.lb_countTime);

//			lb_tiptxt = go.GetComponentEx<Text> (Layout.lb_tiptxt);
			lb_income.text ="";
			lb_incometarget.text = "";

			lb_timetxt.text = "";
			lb_timetarget.text ="";

			lb_qualitytxt.text ="";
			lb_qualitytarget.text ="";

		}

		private void _OnDisposeCenter()
		{
			
		}

		private void _OnShowCenter()
		{
			setTitle ("结算日");
			if (null != _controller.playerInfor) 
			{
				SetCardData (_controller.playerInfor);
			}

//			lb_tiptxt.text = GameTipManager.Instance.GetEnterTip ();

		}

		public void SetCardData(PlayerInfo player)
		{

			lb_incomeDesc.text =string.Format("当前\"{0}\"","<color=#e94444>非劳务收入</color>");
			lb_timeDesc.text = string.Format("当前\"{0}\"","<color=#e94444>时间积分</color>");
			lb_qualityDesc.text = string.Format("当前\"{0}\"","<color=#e94444>品质积分</color>");

			lb_incomequal.text = string.Format ("x {0} =","<color=#e94444>100</color>");
			lb_timenqual.text = string.Format ("x {0} =","<color=#e94444>100</color>");
			lb_qualtyenqual.text = string.Format ("x {0} =","<color=#e94444>10</color>");


			lb_income.text = player.totalIncome.ToString ();
			lb_incometarget.text = HandleStringTool.HandleMoneyTostring(player.totalIncome * 100);

			lb_timetxt.text = player.timeScore.ToString();
			lb_timetarget.text =(player.timeScore * 100).ToString();

			lb_qualitytxt.text = player.qualityScore.ToString ();
			lb_qualitytarget.text = (player.qualityScore * 10).ToString ();
		}


        private Text lb_countTime;	

		private Text lb_income;
		private Text lb_incomequal;
		private Text lb_incometarget;

		private Text lb_timetxt;
		private Text lb_timenqual;
		private Text lb_timetarget;

		private Text lb_qualitytxt;
		private Text lb_qualtyenqual;
		private Text lb_qualitytarget;

		private Text lb_incomeDesc;
		private Text lb_timeDesc;
		private Text lb_qualityDesc;

		// ytf 0926 提示文本框
//		public Text lb_tiptxt;

	}
}

