using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client.UI
{
	public partial class UIGameMailWindow
	{
		private void _InitCenter(GameObject go)
		{
			btn_close = go.GetComponentEx<Button> (Layout.btn_close);
			btn_gonggao = go.GetComponentEx<Button> (Layout.btn_title);

			lb_gongtitle = go.GetComponentEx<Text> (Layout.txt_title);
			lb_gongcontent = go.GetComponentEx<Text> (Layout.txt_content);

			btn_receive = go.GetComponentEx<Button> (Layout.btn_receive);
			btn_receiveAll = go.GetComponentEx<Button> (Layout.btn_receivedAll);
			btn_delete = go.GetComponentEx<Button> (Layout.btn_delete);
			btn_deleteAll = go.GetComponentEx<Button> (Layout.btn_deleteAll);
		}

		private void _ShowCenter()
		{
			EventTriggerListener.Get (this.btn_close.gameObject).onClick += onCloseHandler;

			EventTriggerListener.Get (this.btn_delete.gameObject).onClick += _OnClickDeleteHandler;
			EventTriggerListener.Get (this.btn_deleteAll.gameObject).onClick += _OnClickDeleteAllHandler;
			EventTriggerListener.Get (this.btn_receive.gameObject).onClick += _OnClickReceiveHandler;
			EventTriggerListener.Get (this.btn_receiveAll.gameObject).onClick += _OnClickRecieveAllHandler;

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
					tmpBtn.transform.SetParent (btn_gonggao.transform.parent);
					tmpBtn.transform.position = btn_gonggao.transform.position;
					tmpBtn.transform.localScale = Vector3.one;
					tmpBtn.transform.rotation =btn_gonggao.transform.rotation;
					tmpBtn.name = "btn"+i;
				}

				EventTriggerListener.Get (tmpBtn.gameObject).onClick += _OnSelectTitleHandler;
				var tmpStr = _controller.inforList [i].title;
				if(tmpStr.Length>5)
				{
					tmpStr = tmpStr.Substring (0, 5);
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
				tmpBtnIndex = 0;
				_ShowTipByIndex (0);
			}

//			GameModel.GetInstance.isFirstInGameHall = false;
		}

		private void _HideCenter()
		{
			EventTriggerListener.Get (this.btn_close.gameObject).onClick -= onCloseHandler;


			EventTriggerListener.Get (this.btn_delete.gameObject).onClick -= _OnClickDeleteHandler;
			EventTriggerListener.Get (this.btn_deleteAll.gameObject).onClick -= _OnClickDeleteAllHandler;
			EventTriggerListener.Get (this.btn_receive.gameObject).onClick -= _OnClickReceiveHandler;
			EventTriggerListener.Get (this.btn_receiveAll.gameObject).onClick -= _OnClickRecieveAllHandler;

			for (var i = 0; i < btnTitleList.Count; i++)
			{
				var tmpBtn = btnTitleList[i];

				EventTriggerListener.Get (tmpBtn.gameObject).onClick -= _OnSelectTitleHandler;
			}
		}

		private void _OnClickReceiveHandler(GameObject go)
		{
			if (tmpBtnIndex >= 0)
			{
				_controller.inforList [tmpBtnIndex].isRead = true;
			}
		}

		private void _OnClickRecieveAllHandler(GameObject go)
		{
			for (var i = 0; i < _controller.inforList.Count; i++)
			{
				_controller.inforList [i].isRead = true;
			}
		}

		private void _OnClickDeleteHandler(GameObject go)
		{
			_DeleteMailByIndex (tmpBtnIndex);
		}

		private void _OnClickDeleteAllHandler(GameObject go)
		{
			for (var i = _controller.inforList.Count-1; i >=0 ; i--)
			{
				var tmpVo=_controller.inforList[i];
				if (tmpVo.isRead == true)
				{
					_DeleteMailByIndex (i);
				}
			}
		}

		private void _DisposeCenter()
		{

		}

		private void _OnSelectTitleHandler(GameObject go)
		{
			var tmpIndex =int.Parse(go.name.Substring(3));
			tmpBtnIndex = tmpIndex;
			_ShowTipByIndex (tmpIndex);
		}

		private void _ShowTipByIndex(int value)
		{
			if (value >= 0)
			{
				var gonggaovalue =_controller.inforList[value]; 			
				_SetGonggaoTip (gonggaovalue.title,gonggaovalue.content);
				_SetSelectButtonByIndex (value);
			}
			else
			{
				lb_gongtitle.SetActiveEx (false);
				lb_gongcontent.SetActiveEx (false);
			}		
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

		/// <summary>
		/// Deletes the index of the main by.根据点击的索引值，删除按钮
		/// </summary>
		/// <param name="index">Index.</param>
		private void _DeleteMailByIndex(int index)
		{
			if (index >= 0)
			{
				if (_controller.inforList [index].isRead == true)
				{
					EventTriggerListener.Get (btnTitleList [index].gameObject).onClick -= _OnSelectTitleHandler;
					GameObject.Destroy (btnTitleList [index].gameObject);
					btnTitleList.RemoveAt (index);
					_controller.inforList.RemoveAt (index);

					tmpBtnIndex = -1;
					//10 , 8 
//					if (index < btnTitleList.Count - 1)
//					{
//						for (var i = index; i < btnTitleList.Count; i++)
//						{
//							btnTitleList [i].name = "btn" + i;
//						}
//					}

					for (var i = 0; i < btnTitleList.Count; i++)
					{
						btnTitleList [i].name = "btn" + i;
					}

				}
				if (btnTitleList.Count > 0)
				{
					tmpBtnIndex = 0;
					_ShowTipByIndex (0);
				}
				else
				{
					tmpBtnIndex = -1;
					_ShowTipByIndex (-2);
				}
			}
		}

		private void _SetGonggaoTip(string  title , string content)
		{
			lb_gongtitle.text = title;
			lb_gongcontent.text = content;
			lb_gongtitle.SetActiveEx (true);
			lb_gongcontent.SetActiveEx (true);
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

		private void onCloseHandler(GameObject go)
		{
			Console.WriteLine ("sssssssssssssssssssssssss");
			_controller.setVisible (false);
		}

		private Button btn_close;
		private Button btn_gonggao;

		private Button btn_receive;
		private Button btn_receiveAll;
		private Button btn_delete;
		private Button btn_deleteAll;

		private Color btnbgColor = new Color (116f/255,116f/255,116f/255,1f);
		private Color imgbgColor = new Color (180f/255,180f/255,180f/255,1f);
		private Color initColor = new Color (255f/255,255f/255,255f/255,1f);

		/// <summary>
		/// The index of the tmp button. 每次要删除的按钮的索引值，每次删除以后，要重置
		/// </summary>
		private int tmpBtnIndex=-1;


		private Text lb_gongtitle;
		private Text lb_gongcontent;

		private List<Button> btnTitleList = new List<Button> (); 
	}
}

