using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using Core.Web;
using System;

namespace Client.UI
{
	public partial class UIShowQualityWindow
	{
		private void _OnInitContent(GameObject go)
		{
			_txtLbcardname = go.GetComponentEx<Text>(Layout.txt_lbcardname);
			_txtPayment = go.GetComponentEx<Text>(Layout.txt_payment);
			_txtTimescore = go.GetComponentEx<Text>(Layout.txt_timescore);
			_txtQualitydesc = go.GetComponentEx<Text>(Layout.txt_qualitydesc);
			_txtDesc = go.GetComponentEx<Text>(Layout.txt_desc);

			_btnSure = go.GetComponentEx<Button>(Layout.btn_sure);

			_imgShowImage = go.GetComponentEx<Image>(Layout.img_showImage);
		}

		protected void _OnShowContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick += _OnBtnSureClick;
			SetContent(_controller.qualityLife);
		}

		protected void _OnHideContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick -= _OnBtnSureClick;
		}

		private void _OnBtnSureClick(GameObject go)
		{
			_controller.setVisible(false);
		}

		private void SetContent(QualityLife value)
		{
			_txtLbcardname.text = value.title;


			_txtPayment.text = "￥ "+ Math.Abs (value.payment);

			_txtTimescore.text = value.timeScore.ToString();

			_txtQualitydesc.text = string.Concat (value.qualityScore);;
	
			var str = value.desc;
			var str1 = str.Replace ("\\u3000", "\u3000");
			var str2 = str1.Replace ("\\n","\n");
			_txtDesc.text = str2;		

			WebManager.Instance.LoadWebItem(value.cardPath,item =>{
				using(item)
				{
					_imgShowImage.sprite = item.sprite;
				}
			});

		}

		private Text _txtLbcardname;

		private Text _txtPayment;

		private Text _txtTimescore;
		private Text _txtQualitydesc;

		private Text  _txtDesc;

		private Button _btnSure;

		private Image _imgShowImage;

	}
}

