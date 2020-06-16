using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using Core.Web;

namespace Client.UI
{
	public partial class UIShowFateWindow
	{
		private void _OnInitContent(GameObject go)
		{

			_txtLbcardname = go.GetComponentEx<Text>(Layout.txt_lbcardname);

			_txtDesc = go.GetComponentEx<Text>(Layout.txt_desc);

			_btnSure = go.GetComponentEx<Button>(Layout.btn_sure);
			 
			_imgShowImage = go.GetComponentEx<Image>(Layout.img_showImage);
		}

		protected void _OnShowContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick += _OnBtnSureClick;
			SetContent(_controller.fate);
		}

		protected void _OnHideContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick -= _OnBtnSureClick;
		}

		private void _OnBtnSureClick(GameObject go)
		{
			_controller.setVisible(false);
		}

		public void SetContent(Fate value)
		{
			_txtLbcardname.text = value.title;



			var str = value.cardIntroduce;
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

		private Text _txtDesc;

		private Button _btnSure;

		private Image _imgShowImage;

	}
}