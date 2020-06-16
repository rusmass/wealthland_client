using System;
using UnityEngine;
using UnityEngine.UI;


namespace Client.UI
{
	public partial class UICheckOutWindow
	{
		private void _InitCenter(GameObject go)
		{
			var rawImg = go.GetComponentEx<Image> (Layout.rawImg);

			_loadImg = new UIImageDisplay (rawImg);
		}

		private void _OnShowCenter()
		{
			if(null != _controller.playerInfor)
			{
				ShowCenterImg (_controller.playerInfor);
			}
			maxLen = _controller.pathList.Count;
			_time = new Counter (1.3f);
		}

		private void ShowCenterImg(PlayerInfo playerInfor)
		{			
			var checkoutMoney=(playerInfor.cashFlow + playerInfor.totalIncome + playerInfor.innerFlowMoney-playerInfor.initCardLoan-playerInfor.initCarLoan-playerInfor.initEducationLoan-playerInfor.initHouseMortgages-playerInfor.initOtherSpend-playerInfor.initAdditionalDebt-playerInfor.initTax-(playerInfor.childNum*playerInfor.oneChildPrise)) ;

			if (checkoutMoney<0) 
			{
				
				_loadImg.Load ("share/atlas/battle/jiezhanri/checkout_balance_2.ab");
			}
		}

		private void _OnHideCenter()
		{
			
		}	

		private void _OnDisposeCenter()
		{
			if (null != _loadImg) 
			{
				_loadImg.Dispose ();
			}
		}


		public void TickLoad(float value)
		{
			if (null != _time && _time.Increase (value) == true)
			{
				if (currentIndex < maxLen)
				{
					var str = _controller.pathList [currentIndex];
					var title = _controller.cardNameList [currentIndex];
					var id=_controller.cardIdList[currentIndex];
					Console.WriteLine (string.Format("当前的卡牌的名称是：{0},,id是：{1},,路径是：{2}",title,id,str));				
					_loadImg.Load (str);
					currentIndex++;
					_time.Reset ();
				}
				else 
				{
					Console.WriteLine ("卡牌加载晚啦");
					_time = null;
						
				}

			}

		}

		private Counter _time;
		private int currentIndex=0;
		private int maxLen=0;



		private UIImageDisplay _loadImg;
	}
}

