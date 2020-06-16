using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIOtherCardWindow
	{
		private void _OnInitCenter(GameObject go)
		{
			desc1 = go.GetComponentEx<Text> (Layout.lb_desc);
			desc2 = go.GetComponentEx<Text> (Layout.lb_desc2);
			desc3 = go.GetComponentEx<Text> (Layout.lb_desc3);

			var rawImg = go.GetComponentEx<Image> (Layout.loadImg);
			_cardPic = new UIImageDisplay (rawImg);

			lb_cardName = go.GetComponentEx<Text> (Layout.lbcardname);

			//_saleImg.SetActiveEx (false);
			//zll 2016.10.21 add card action
			cardAction = go.GetComponentEx<Image>(Layout.cardAction);
			cardAction2 = go.GetComponentEx<Image>(Layout.cardAction1);
		}

		private void _OnDisposeCenter()
		{
			if (null != _cardPic) 
			{
				_cardPic.Dispose ();
			}
		}


		private void _OnShowCenter()
		{
			if (null != _controller) 
			{
				setTitle (_controller.cardTitlePath);
				setOuterFateCardData (_controller.cardInfor,_controller.cardTitle);
				_cardPic.Load (_controller.cardPath);
			}
            
            
			//zll 2016.10.21 add card action
			cardAction.SetActiveEx(true);
			cardAction2.SetActiveEx(false);

            if (_controller.IsOnlyShow == true)
            {
                if (_controller.IsHasKnowledge())
                {
                    _StartShowKnowledge(true);
                }
            }
        }

		public void setOuterFateCardData(string strvalue,string cardtitle)
		{
			var str = strvalue;
			var str1 = str.Replace ("\\u3000", "\u3000");
			var str2 = str1.Replace ("\\n","\n");

			desc1.text = str2;

			lb_cardName.text = cardtitle;

			desc2.SetActiveEx (false);
			desc3.SetActiveEx (false);

            if(GameModel.GetInstance.isPlayNet==false)
            {
                if (_controller.player.isEnterInner)
                {
                    if (_controller.cardID == (int)SpecialCardType.InnerHealthType)
                    {
                        desc2.SetTextEx("玩家时间积分+10");
                        desc2.SetActiveEx(true);
                    }
                    else if(_controller.cardID ==(int)SpecialCardType.InnerStudyType)
                    {
                        desc2.SetTextEx("玩家品质积分+5");
                        desc2.SetActiveEx(true);
                    }

                 }
            }

            
		}

		private bool _isShowAction=false;
		//zll 2016.10.21 add card action
		private void actionTime(float deltaTime)
		{

			if(_isShowAction==true)
			{
				return;
			}

			addtime += deltaTime;

			if(addtime >= 0.40f)
			{
				cardAction.SetActiveEx(true);
				cardAction2.SetActiveEx(true);

				addtime = 0;

				_isShowAction = true;
			}
		}

		private Text lb_cardName;

		private Text desc1;
		private Text desc2;
		private Text desc3;
		private UIImageDisplay _cardPic;

		private Image _saleImg;

		//zll 2016.10.21 add card action
		private Image cardAction;
		private Image cardAction2;
		private float addtime = 0;
	}
}

