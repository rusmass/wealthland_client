using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using System;
using Core.Web;

namespace Client.UI
{
	public partial class UIShowRiskWindow
	{
		private void _OnInitContent(GameObject go)
		{
			lb_desc = go.GetComponentEx<Text> (Layout.lb_desc);

			lb_paymenyTxt = go.GetComponentEx<Text> (Layout.lb_paymentTxt);

			img_wordbg = go.GetComponentEx<Image> (Layout.img_wordimg);

			lb_desc2 = go.GetComponentEx<Text> (Layout.lb_desc2);

			lb_paymenyTxt2 = go.GetComponentEx<Text> (Layout.lb_paymentTxt2);

			lb_timeTxt = go.GetComponentEx<Text> (Layout.lb_timeTxt);
			lb_timeName = go.GetComponentEx<Text> (Layout.lb_timeName);

			lb_cardname = go.GetComponentEx<Text>(Layout.lb_cardname);

			_btnSure = go.GetComponentEx<Button>(Layout.btn_sure);
			_imgShowImage = go.GetComponentEx<Image>(Layout.img_showImage);
		}

		protected void _OnShowContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick += _OnBtnSureClick;
			SetContent(_controller.risk);
		}

		protected void _OnHideContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick -= _OnBtnSureClick;
		}

		private void _OnBtnSureClick(GameObject go)
		{
			_controller.setVisible(false);
		}

		private void SetContent(Risk go)
		{
	
			//			lb_cardTitle.text = go.title;
			if (go.desc == null || go.desc == "") 
			{
				lb_desc.SetActiveEx (false);
			}else
			{
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

				var str = go.desc2;
				var str1 = str.Replace ("\\u3000", "\u3000");
				var str2 = str1.Replace ("\\n","\n");
				lb_desc2.text =str2;

				lb_paymenyTxt2.text = string.Concat (go.payment2);
			}

			WebManager.Instance.LoadWebItem(go.cardPath,item =>{
				using(item)
				{
					_imgShowImage.sprite = item.sprite;
				}
			});
		}

		private Text lb_desc;

		//private Text lb_paymentName;
		private Text lb_paymenyTxt;

		private Text lb_desc2;
		//private Text lb_paymentName2;
		private Text lb_paymenyTxt2;

		private Text lb_cardname;

		private Image img_wordbg;

		private Text lb_timeName;
		private Text lb_timeTxt;




		private Button _btnSure;
		private Image _imgShowImage;
	}
}
