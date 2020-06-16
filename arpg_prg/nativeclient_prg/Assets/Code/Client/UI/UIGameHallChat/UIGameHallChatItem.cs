using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UIGameHallChatItem
	{
		public UIGameHallChatItem (GameObject go)
		{

			itemSize=go.GetComponent<RectTransform> ().sizeDelta;

			lb_name = go.GetComponentEx<Text> (Layout.lb_name);
			lb_txt = go.GetComponentEx<Text> (Layout.lb_txt);
			lb_time = go.GetComponentEx<Text> (Layout.lb_time);

			btn_head = go.GetComponentEx<Button> (Layout.btn_head);

			var img_head = go.GetComponentEx<Image> (Layout.btn_head);

            img_displayHead = img_head;// new UIImageDisplay (img_head);



			var tmpTransform=lb_txt.rectTransform;
			txtChatPosition=tmpTransform.localPosition;
			txtChatSize = tmpTransform.sizeDelta;

			tmpTransform = lb_name.rectTransform;
			txtNamePosition = tmpTransform.localPosition;
			txtNameSize = tmpTransform.sizeDelta;

			tmpTransform = btn_head.gameObject.GetComponent<RectTransform> ();
			btnHeadPosition = tmpTransform.localPosition;
			btnHeadSize = tmpTransform.sizeDelta;

			tmpTransform = lb_time.rectTransform;
			txtTimePosition = tmpTransform.localPosition;
			txtTimeSize = tmpTransform.sizeDelta;

			var imgsex = go.GetComponentEx<Image> (Layout.img_sex);
			img_sex = new UIImageDisplay (imgsex);
		}


		public void Refresh(NetChatVo value)
		{			
//			_lbTitleTxt.text = value.title;
//			if(UIDebtAndPaybackController.isDebt==true)
//			{
//				_lbBorrow.text = value.borrow.ToString ();
//			}
//			else
//			{
//				_lbBorrow.text =string.Format("-{0}", Mathf.Abs(value.debt).ToString ()) ;
//			}
			lb_name.text = value.playerName;
			lb_txt.text = value.chat;
			lb_time.text = value.sendTime;


			if (tmpHeadPath != value.playerHead)
			{
				//img_displayHead.Load (value.playerHead);
                AsyncImageDownload.Instance.SetAsyncImage(value.playerHead, img_displayHead);
                tmpHeadPath = value.playerHead;
			}



			if (value.playerId == GameModel.GetInstance.myHandInfor.uuid)
			{
				//				lb_txt;
				btn_head.transform.localPosition=new Vector3(itemSize.x-btnHeadSize.x-10, btnHeadPosition.y,btnHeadPosition.z);
				lb_name.transform.localPosition = new Vector3 (itemSize.x -btnHeadSize.x-10-txtNameSize.x - 10, txtNamePosition.y, txtNamePosition.z);
				lb_txt.transform.localPosition = new Vector3 (itemSize.x-btnHeadSize.x-10-txtChatSize.x-10,txtChatPosition.y,txtChatPosition.z);
				lb_time.transform.localPosition = new Vector3(itemSize.x -btnHeadSize.x-10-lb_name.preferredWidth - 20-txtTimeSize.x,txtTimePosition.y,txtTimePosition.z);
				lb_name.alignment =TextAnchor.MiddleRight; //TextAlignment.Right;TextAnchor
				lb_txt.alignment = TextAnchor.MiddleRight;
				lb_time.alignment = TextAnchor.MiddleRight;

				if (null != img_sex) 
				{
					img_sex.SetActive (false);
				}
			}
			else
			{
				btn_head.transform.localPosition=btnHeadPosition;
				lb_name.transform.localPosition = txtNamePosition;
				lb_txt.transform.localPosition = txtChatPosition;
				lb_time.transform.localPosition=new Vector3(txtNamePosition.x+lb_name.preferredWidth+20,txtTimePosition.y,txtTimePosition.z);
				lb_name.alignment = TextAnchor.MiddleLeft;
				lb_txt.alignment = TextAnchor.MiddleLeft;
				lb_time.alignment = TextAnchor.MiddleLeft;

				if (null != img_sex) 
				{
					img_sex.SetActive (true);
					if (value.sex == 0)
					{
						img_sex.Load (sexWomanPath);
					}
					else if(value.sex==1)
					{
						img_sex.Load (sexManPath);
					}
				}
			}


			_chatvo = value;

		}
			
		/// <summary>
		/// Disposes the self.释放资源
		/// </summary>
		public void DisposeSelf()
		{
			if (null != img_displayHead)
			{				
				//img_displayHead.Dispose ();

			}
		}

		private string tmpHeadPath = "";

		private NetChatVo _chatvo;
		private Text lb_name;
		private Text lb_txt;
		private Text lb_time;
		private Button btn_head;
		private Image img_displayHead;

		private UIImageDisplay img_sex;



		private Vector2 itemSize;	

		private Vector3 txtNamePosition;
		private Vector3 txtChatPosition;
		private Vector3 btnHeadPosition;
		private Vector3 txtTimePosition;

		private Vector2 txtNameSize;
		private Vector2 txtChatSize;
		private Vector2 btnHeadSize;
		private Vector2 txtTimeSize;

		private string sexManPath="share/atlas/battle/gamehall/sexman.ab";
		private string sexWomanPath="share/atlas/battle/gamehall/sexwomen.ab";

		class Layout
		{			
			public static string lb_name="nametxt";

			public static string lb_txt="numtxt";

			public static string btn_head="btn_head";

			public static string lb_time="timetxt";

			public static string img_sex="img_sex";

		}
	}
}

