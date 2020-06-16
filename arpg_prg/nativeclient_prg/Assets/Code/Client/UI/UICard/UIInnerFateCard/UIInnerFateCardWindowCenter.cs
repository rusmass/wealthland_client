using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public partial class UIInnerFateCardWindow
	{
		private void _OnInitCenter(GameObject go)
		{
			desc1 = go.GetComponentEx<Text> (Layout.lb_desc);
			desc2 = go.GetComponentEx<Text> (Layout.lb_desc2);
			desc3 = go.GetComponentEx<Text> (Layout.lb_desc3);

			lb_cardname = go.GetComponentEx<Text> (Layout.lb_cardName);

			var rawImg = go.GetComponentEx<Image> (Layout.loadImg);
			_cardPic = new UIImageDisplay (rawImg);

			img_bgBoard = go.GetComponentEx<Image> (Layout.img_bgBoard);

			//zll 2016.10.21 add card action
//			cardAction = go.GetComponentEx<Image>(Layout.cardAction);
//			cardAction2 = go.GetComponentEx<Image>(Layout.cardAction1);

			animator_crap = go.GetComponentEx<Animator> (Layout.action_craps);
			var tmpcarp = go.GetComponentEx<Image> (Layout.img_craps);
			img_carp =new UIImageDisplay(tmpcarp);
		}

		private void _OnDisposeCenter()
		{
			if (null != _cardPic) 
			{
				_cardPic.Dispose ();
			}


			if (null != img_carp)
			{
				img_carp.Dispose ();
			}
		}


		private void _OnShowCenter()
		{
			if (null != _controller.cardData) 
			{
				SetInnerFateCardData (_controller.cardData,_controller.cardData.cardPath);
			}

			img_carp.SetActive (false);
			animator_crap.enabled=false;
			animator_crap.SetActiveEx (false);


			//zll 2016.10.21 add card action
//			cardAction.SetActiveEx(true);
//			cardAction2.SetActiveEx(false);
		}

		private void SetInnerFateCardData(InnerFate go,string imgPath)
		{
			lb_cardname.text = go.title ;

			var str = go.desc;
			var str1 = str.Replace ("\\u3000", "\u3000");
			var str2 = str1.Replace ("\\n","\n");
			desc1.text =str2;
					
			desc2.SetActiveEx(false);
			desc3.SetActiveEx (false);

			if ("" != imgPath)
			{
				if(null != _cardPic)
				{
					_cardPic.Load (imgPath);
				}
			}

		}

		private void _ChangeSizeForBoarrow()
		{
			img_bgBoard.rectTransform.sizeDelta = _boardSize;
		}

		private bool _isShowAction=false;

		//zll 2016.10.21 add card action  效果 roll 筛子,两秒后停止,显示点数,筛子显示两秒后缩减
		private void actionTime(float deltaTime)
		{
			if (_isShowAction == false)
			{
				return;
			}

			addtime += deltaTime;
			if (addtime >= 1f && _isRolledCrap==false)
			{
				//cardAction.SetActiveEx(false);
//				cardAction2.SetActiveEx(true);
				_isRolledCrap=true;
				_HideCrapAndHandler();
			}
			else if(addtime>2f)
			{
				_isShowAction = false;
				addtime = 0;
				_controllerHandler ();
			}
		}

		private Text lb_cardname;

		private Text desc1;
		private Text desc2;
		private Text desc3;

		private Image img_bgBoard;
		private Vector2 _boardSize = new Vector2 (426, 485);


		private UIImageDisplay _cardPic;

		//zll 2016.10.21 add card action
//		private Image cardAction;
//		private Image cardAction2;
		private float addtime = 0;


		private Animator animator_crap;
		private UIImageDisplay img_carp;
		private bool _isRolledCrap=false;
		private int crapsNum=0;
	}
}

