using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



namespace Client.UI
{
	public partial class UIGongGaoWindow
	{
		private void _InitCenter(GameObject go)
		{
			btn_close = go.GetComponentEx<Button> (Layout.btn_close);
			btn_gonggao = go.GetComponentEx<Button> (Layout.btn_title);

			lb_gongtitle = go.GetComponentEx<Text> (Layout.txt_title);
			lb_gongcontent = go.GetComponentEx<Text> (Layout.txt_content);

			img_gonggao = go.GetComponentEx<Image> (Layout.img_gonggao);
		}

		private void _ShowCenter()
		{
			img_gonggao.SetActiveEx (false);
			//btn_close.SetActiveEx (false);
			EventTriggerListener.Get (this.btn_close.gameObject).onClick += onCloseHandler;

			lb_gongtitle.text = "";
			lb_gongcontent.text = "";

			for (var i = 0; i < _controller.inforList.Count; i++)
			{
				Button tmpBtn;
				if (i == 0)
				{
					tmpBtn = this.btn_gonggao;
					tmpBtn.name = "btn" + i;
				}
				else
				{
					tmpBtn = this.btn_gonggao;		
					tmpBtn =btn_gonggao.gameObject.CloneEx().GetComponent<Button>()  ;
					//tmpBtn =(Button) _btnServer.transform CloneEx(false)  ;
					tmpBtn.transform.SetParent (btn_gonggao.transform.parent);
					tmpBtn.transform.position = btn_gonggao.transform.position;
					tmpBtn.transform.localScale = Vector3.one;
					tmpBtn.transform.rotation =btn_gonggao.transform.rotation;
					tmpBtn.name = "btn"+i;
				}

				EventTriggerListener.Get (tmpBtn.gameObject).onClick += _OnSelectTitleHandler;
				var tmpStr = _controller.inforList [i].title;
				if(tmpStr.Length>6)
				{
					tmpStr = tmpStr.Substring (0, 6);
					tmpStr+="...";
				}
				tmpBtn.gameObject.GetComponentEx<Text> ("lb_txt").text =tmpStr;
				btnTitleList.Add (tmpBtn);
			}

			if (_controller.inforList.Count <= 0)
			{
				btn_gonggao.SetActiveEx (false);
				lb_gongtitle.SetActiveEx (false);
				lb_gongcontent.SetActiveEx (false);
			}
			else
			{
				_ShowTipByIndex (0);
			}

			GameModel.GetInstance.isFirstInGameHall = false;
		}

		private void _HideCenter()
		{
			EventTriggerListener.Get (this.btn_close.gameObject).onClick -= onCloseHandler;

			for (var i = 0; i < btnTitleList.Count; i++)
			{
				var tmpBtn = btnTitleList[i];

				EventTriggerListener.Get (tmpBtn.gameObject).onClick -= _OnSelectTitleHandler;
			}

			GameModel.GetInstance.isFirstInGameHall = false;

		}

		private void _DisposeCenter()
		{
			
		}

		private void _OnSelectTitleHandler(GameObject go)
		{
			var tmpIndex =int.Parse(go.name.Substring(3));
			_ShowTipByIndex (tmpIndex);
		}

		private void _SetSelectButtonByIndex(int index)
		{
			for (var i = 0; i < btnTitleList.Count; i++)
			{
				if (i == index)
				{
					_SetInitColor (btnTitleList[i].gameObject);
				}
				else
				{
					_SetGrayColor (btnTitleList[i].gameObject);
				}
			}
		}

		private void _ShowTipByIndex(int value)
		{
			_SetSelectButtonByIndex (value);

			
			var gonggaovalue =_controller.inforList[value]; 			
			bool isUrl = false;
			if (gonggaovalue.type == 1)
			{
				isUrl = true;
			}

			if (isUrl == false)
			{
				_SetGonggaoTip (gonggaovalue.title,gonggaovalue.content);
			}
			else
			{
				_SetGonggaoImg (gonggaovalue.content);
			}
		}

		private void _SetGonggaoTip(string  title , string content)
		{
			lb_gongtitle.text = title;
			lb_gongcontent.text = content;

			lb_gongtitle.SetActiveEx (true);
			lb_gongcontent.SetActiveEx (true);

			img_gonggao.SetActiveEx (false);
		}

		private void _SetGonggaoImg (string loadPath)
		{
			img_gonggao.SetActiveEx (true);
			AsyncImageDownload.Instance.SetAsyncImage (loadPath,img_gonggao);
			img_gonggao.SetNativeSize ();
			//img_gonggao.texture= LoadRawImg.CUTPicture2 (loadPath);

			lb_gongtitle.SetActiveEx (false);
			lb_gongcontent.SetActiveEx (false);
		}

		private void onCloseHandler(GameObject go)
		{
			Console.WriteLine ("sssssssssssssssssssssssss");
			_controller.setVisible (false);
		}

		/// <summary>
		/// Sets the color of the gray.设置灰色未选中状态
		/// </summary>
		/// <param name="go">Go.</param>
		private void _SetGrayColor(GameObject go)
		{
			go.GetComponent<Image> ().color = btnbgColor;
			go.GetComponentEx<Text> ("lb_txt").color = imgbgColor;
		}

		/// <summary>
		/// Sets the color of the init. 设置选中状态
		/// </summary>
		/// <param name="go">Go.</param>
		private void _SetInitColor(GameObject go)
		{
			go.GetComponent<Image> ().color = initColor;
			go.GetComponentEx<Text> ("lb_txt").color = initColor;
		}

		private Button btn_close;
		private Button btn_gonggao;
		private Text lb_gongtitle;

		private Text lb_gongcontent;
		private Image img_gonggao;

		private Color btnbgColor = new Color (116f/255,116f/255,116f/255,1f);
		private Color imgbgColor = new Color (180f/255,180f/255,180f/255,1f);
		private Color initColor = new Color (255f/255,255f/255,255f/255,1f);

		private List<Button> btnTitleList = new List<Button> (); 
	}
}

