using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public partial class UIRiskCardWindow
	{
		private void _InitCenter(GameObject go)
		{		

			lb_desc = go.GetComponentEx<Text> (Layout.lb_desc);

			lb_paymenyTxt = go.GetComponentEx<Text> (Layout.lb_paymentTxt);

			img_wordbg = go.GetComponentEx<Image> (Layout.img_wordimg);

			lb_desc2 = go.GetComponentEx<Text> (Layout.lb_desc2);

			//lb_paymentName2 = go.GetComponentEx<Text> (Layout.lb_paymentName2);
			lb_paymenyTxt2 = go.GetComponentEx<Text> (Layout.lb_paymentTxt2);

			lb_timeTxt = go.GetComponentEx<Text> (Layout.lb_timeTxt);
			lb_timeName = go.GetComponentEx<Text> (Layout.lb_timeName);

			_selectToggle = go.GetComponentEx<Toggle> (Layout.toggle_select);

			lb_cardname = go.GetComponentEx<Text>(Layout.lb_cardname);

			var rawImg = go.GetComponentEx<Image> (Layout.loadImg);
			_cardPic = new UIImageDisplay (rawImg);


			//zll 2016.10.21 add card action
			cardAction = go.GetComponentEx<Image>(Layout.cardAction);
			cardAction2 = go.GetComponentEx<Image>(Layout.cardAction1);
		}

		private void _OnShowCenter ()
		{
			if(null!=_controller.cardData)
			{
				_controller.isSlect = false;
				_showRiskCardData (_controller.cardData,_controller.cardData.cardPath);
			}

			_selectToggle.onValueChanged.AddListener (_OnSelectToggleHandler);

			//zll 2016.10.21 add card action
			cardAction.SetActiveEx(true);
			cardAction2.SetActiveEx(false);
		}


		private void _OnHideCenter()
		{
			_selectToggle.onValueChanged.RemoveListener (_OnSelectToggleHandler);
		}

		private void _OnSelectToggleHandler(bool value)
		{
			Console.WriteLine ("isSelect risk"+value.ToString());
			if (null != _controller)
			{
				_controller.isSlect = value;

				if (value == true)
				{
					if (_controller.isCanSelectFree ()==false)
					{
						_controller.isSlect = false;
						_selectToggle.isOn = false;

						MessageHint.Show ("金币不足，不能够选择自由选择");
					}	
				}						
			}
		}

		private void _showRiskCardData (Risk go, string imgPath)
		{
//			lb_cardTitle.text = go.title;
			if (go.desc == null || go.desc == "") 
			{
				lb_desc.SetActiveEx (false);
			}else
			{
//				lb_desc.text = go.desc;
				var str = go.desc;
				var str1 = str.Replace ("\\u3000", "\u3000");
				var str2 = str1.Replace ("\\n","\n");
				lb_desc.text =str2;
			}

			lb_cardname.text = go.title;

			lb_paymenyTxt.text = string.Concat (go.payment);

			if (null == go.desc2 || go.desc2 == "")
			{
				img_wordbg.SetActiveEx (false);

//				img_wordbgLeft.transform.localPosition = new Vector3 (17,img_wordbgLeft.transform.localPosition.y,0);

			} else
			{
				if (go.score == 0) 
				{
					lb_timeName.SetActiveEx (false);
					lb_timeTxt.SetActiveEx (false);
				} else
				{
					lb_timeTxt.text = string.Concat (go.score);
				}

				lb_desc2.text =go.desc2;

				lb_paymenyTxt2.text = string.Concat (go.payment2);
			}

			if ("" != imgPath)
			{
				if(null != _cardPic)
				{

					Debug.Log ("sssssss"+imgPath);
					_cardPic.Load (imgPath);
				}
			}


		}


		private void _OnDisposeCenter()
		{
			if(null != _cardPic)
			{
				_cardPic.Dispose ();
			}
		}
		private bool _isShowAction=false;
		//zll 2016.10.21 add card action
		private void actionTime(float deltaTime)
		{
			if (_isShowAction == true)
			{
				return;
			}


			addtime += deltaTime;

			if(addtime >= 0.40f)
			{
				cardAction.SetActiveEx(true);
				cardAction2.SetActiveEx(true);
				_isShowAction = true;
				addtime = 0;
			}
		}
	
		private Text lb_desc;

		//private Text lb_paymentName;
		private Text lb_paymenyTxt;

		private Text lb_desc2;
		//private Text lb_paymentName2;
		private Text lb_paymenyTxt2;

		private Image img_wordbg;


		private Text lb_cardname;


		private Text lb_timeName;
		private Text lb_timeTxt;

		private Toggle _selectToggle;

		private UIImageDisplay _cardPic;

		//zll 2016.10.21 add card action
		private Image cardAction;
		private Image cardAction2;
		private float addtime = 0;
	}
}

