using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIBattleWindow
	{
		private void _OnInitCountdown(GameObject go)
		{
			_imgbuttonbg = go.GetComponentEx<Image> (Layout.img_btnbg);		

			img_roundTip = go.GetComponentEx<Image> (Layout.img_roundTip);
			lb_roundTip = go.GetComponentEx<Text> (Layout.lb_roundtip);
			lb_roundPlayer = go.GetComponentEx<Text> (Layout.lb_roundPlayer);

            img_controller = go.GetComponentEx<Image>(Layout.img_controllerBoard);
            lb_controllerTime = go.GetComponentEx<Text>(Layout.lb_controllerTime);
            lb_controllerTip = go.GetComponentEx<Text>(Layout.lb_controllerTip);

            controllerPosition = img_controller.transform.localPosition;

        }

		private void _OnShowCountdown()
		{
            //_imgbuttonbg.SetActiveEx (false);

            if (null != _imgbuttonbg)
			{
				if (_imgbuttonbg.IsActive () == false) 
				{
					_imgbuttonbg.SetActiveEx (true);
				}
			}
			_tmpCountImg.SetActiveEx(false);

            img_controller.SetActiveEx(false);
		
		}

		/// <summary>
		/// Shows the stay round tip.显示哪个玩家在玩
		/// </summary>
		public void ShowStayRoundTip()
		{
			img_roundTip.SetActiveEx (true);

			var tmpStr = "";

			if (PlayerManager.Instance.IsHostPlayerTurn())
			{
				tmpStr = "轮到您了，请掷骰子";
                //				lb_roundTip.text =string.Format("第{0}回合",_currentTurnCount+1);
                Audio.AudioManager.Instance.Tip_ReturnedMe(PlayerManager.Instance.HostPlayerInfo.careerID);
			}
			else
			{
				tmpStr = string.Format ("当前轮到<color=#00f1a4>{0}</color>进行操作",_currentPlayer.playerName);
//				lb_roundTip.text =string.Format("第{0}回合",_currentTurnCount);
			}

			lb_roundPlayer.text = tmpStr;

			var sequence = DOTween.Sequence ();
			sequence.Append (img_roundTip.transform.DOLocalMoveX (img_roundTip.rectTransform.localPosition.x, 2.5f));
			sequence.AppendCallback (HideStayRoundTip);

            HideControllerBoard();

		}

		/// <summary>
		/// Hides the stay round tip.隐藏玩家提示面板
		/// </summary>
		private void HideStayRoundTip()
		{
			img_roundTip.SetActiveEx (false);
		}

       /// <summary>
       ///  右下角提示框倒计时更新
       /// </summary>
       /// <param name="deltime"></param>
        public void updateControllerBoardTime(float deltime)
        {
            if (isCountController == true)
            {
                controllerlefttime -= deltime;

                var tmptime = GetTime(controllerlefttime);

                if (controllerlefttime <= 0)
                {
                    isCountController = false;

                    tmptime = "0";
                }
                lb_controllerTime.text =tmptime;

            }
        }

        /// <summary>
        ///  刷新时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private string GetTime(float time)
        {
            return HandleNumToTimeTool.ChangeNumberToTime(time);
        }

        /// <summary>
        ///  显示当前谁在操作. 进入到选择卡牌的时候回提示谁在操作卡牌，切换玩家的时候，隐藏
        /// </summary>
        /// <param name="heroName"></param>
        /// <param name="cardTitle"></param>
        public void ShowControlerBoard(string heroName,string cardTitle)
        {
            img_controller.SetActiveEx(true);

            controllerlefttime = controllerTotalTime;
            lb_controllerTime.text = controllerlefttime.ToString();
            lb_controllerTip.text = string.Format("当前玩家：{0}\n正在操作：{1}卡牌。", heroName, cardTitle);

            img_controller.transform.localPosition = controllerPosition;
            var sequence = DOTween.Sequence();
            sequence.Append(img_controller.transform.DOLocalMoveX(_boardendX, 0.5f));
            isCountController = true;
            //sequence.AppendCallback(HideStayRoundTip);
        }

        /// <summary>
        /// 隐藏玩家操作提示面板
        /// </summary>
        public void HideControllerBoard()
        {
            img_controller.transform.localPosition = controllerPosition;
            img_controller.SetActiveEx(false);
            isCountController = false;
        }


		public void _OnHideCountdown()
		{
			if (null != _imgbuttonbg)
			{
				_imgbuttonbg.DestroyEx ();
			}		

			if (null != _tmpCountImg)
			{
				_tmpCountImg.gameObject.DestroyEx ();
			}
		}


		private string _countdownPath = "share/atlas/battle/daoshu/{0}.ab";
//		private int _currentCount = 3;



		private Image _imgbuttonbg;
		private Image _tmpCountImg;

		private Image img_roundTip;
		private Text  lb_roundTip;
		private Text  lb_roundPlayer;

        private Image img_controller;
        private Text lb_controllerTip;
        private Text lb_controllerTime;
        private float controllerlefttime=60;
        private float controllerTotalTime=60;
        private bool isCountController = false;
        private Vector3 controllerPosition;
        private float _boardendX=97; 


	}
}

