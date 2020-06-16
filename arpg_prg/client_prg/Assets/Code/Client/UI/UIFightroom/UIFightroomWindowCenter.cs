using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client.UI
{
	public partial class UIFightroomWindow
	{
		private void _initCenter(GameObject go)
		{
			btn_close = go.GetComponentEx<Button> (Layout.btn_back);

			//img_player1 = go.GetComponentEx<Image> (Layout.img_player1);
			//img_player2 = go.GetComponentEx<Image> (Layout.img_player2);
			//img_player3 = go.GetComponentEx<Image> (Layout.img_player3);
			//img_player4 = go.GetComponentEx<Image> (Layout.img_player4);

			for (int i = 1; i < 5; i++)
			{
				Image tmpImg = go.GetComponentEx<Image> (Layout.img_player+i);

				headInforList.Add(tmpImg);

				UIImageDisplay tmpHead = new UIImageDisplay (tmpImg.gameObject.GetComponentEx<Image> ("headimg"));
				headList.Add (tmpHead);
				//headList[i]=tmpHead;
				Text tmpName = tmpImg.gameObject.GetComponentEx<Text> ("nametxt");
				nameList.Add (tmpName);

				var tmpReady = tmpImg.gameObject.GetComponentEx<Image> ("img_ready");
				readyList.Add (tmpReady);

			}

			btn_ready = go.GetComponentEx<Button> (Layout.btn_ready);

			btn_share = go.GetComponentEx<Button> (Layout.btn_share);

			lb_roomid = go.GetComponentEx<Text>(Layout.lb_roomid);

			/*head1 =new UIImageDisplay(img_player1.gameObject.GetComponentEx<Image> ("headimg"));
			head2 =new UIImageDisplay(img_player1.gameObject.GetComponentEx<Image> ("headimg"));
			head3 =new UIImageDisplay(img_player1.gameObject.GetComponentEx<Image> ("headimg"));
			head4 =new UIImageDisplay(img_player1.gameObject.GetComponentEx<Image> ("headimg"));
			headList.Add (head1);
			headList.Add (head2);
			headList.Add (head3);
			headList.Add (head4);*/

			//this ["img_player" + 1];

			if (null != _controller.headInforList)
			{
				if (_controller.headInforList.Count > 0)
				{
					SetPlayerHaed (_controller.headInforList);
				}
			}

		}

		public void SetPlayerHaed(List<PlayerHeadInfor> tmpList)
		{
			int tmpLen = tmpList.Count > 4 ? 4 : tmpList.Count;

			for (int i = 0; i < 4; i++)
			{
				if (i < tmpLen)
				{
					setInforByIndex (i,tmpList[i]);
				}
				else
				{
					setInforByIndex (i, null);
				}
			}
		}

		private void setInforByIndex(int index , PlayerHeadInfor infor)
		{
			if (null == infor)
			{
				headInforList [index].SetActiveEx (false);
			}
			else
			{				
				headInforList [index].SetActiveEx (true);

				headList [index].Load (infor.headImg);
				nameList [index].text = infor.nickName;
				if (infor.isReady == true)
				{
					readyList [index].SetActiveEx (true);
				}
				else 
				{
					readyList [index].SetActiveEx (false);
				}
			}



		}

		private void _showCenter()
		{
			GameModel.GetInstance.HideNetLoading ();

			EventTriggerListener.Get (btn_close.gameObject).onClick += _OnCloseHandler;
			EventTriggerListener.Get (btn_ready.gameObject).onClick += _OnClickReadyHandler;
			EventTriggerListener.Get (btn_share.gameObject).onClick += _OnClickShareHandler;
			SetPlayerHaed (GameModel.GetInstance.roomPlayerInforList);

			lb_roomid.text = GameModel.GetInstance.curRoomId;

			GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameRoomState;

			GameModel.GetInstance.isPlayNet = true;

			GameModel.GetInstance.HideNetLoading ();

		}

		/// <summary>
		/// Sets the sure button disabled. 设置当前按钮不可点击
		/// </summary>
		public void SetSureBtnDisabled()
		{
			if (null!=btn_ready)
			{
				btn_ready.enabled = false;
				btn_ready.gameObject.GetComponent<Image> ().color = _grayColor;
			}
		}

		private void _hideCenter()
		{
			EventTriggerListener.Get (btn_close.gameObject).onClick -= _OnCloseHandler;
			EventTriggerListener.Get (btn_ready.gameObject).onClick -= _OnClickReadyHandler;
			EventTriggerListener.Get (btn_share.gameObject).onClick -= _OnClickShareHandler;


			var readController = UIControllerManager.Instance.GetController<UINetGameReadyWindowController> ();

			var tmpHeadList = GameModel.GetInstance.roomPlayerInforList;

			for (var i = 0; i < tmpHeadList.Count; i++)
			{
				var tmpHeadInfor = tmpHeadList [i];
				if (null != tmpHeadInfor)
				{
					tmpHeadInfor.isReady = false;
				}
			}

			readController.SetPlayerHeadList (tmpHeadList);
		}


		private void _OnClickReadyHandler(GameObject go)
		{
			NetWorkScript.getInstance ().RequestReadyInRoom (GameModel.GetInstance.myHandInfor.uuid,GameModel.GetInstance.curRoomId);
		}


		private void _disposeCenter()
		{
			for (int i = 0; i < headList.Count; i++)
			{
				if (null != headList [i])
				{
					headList [i].Dispose ();
				}

			}
		}

		private void _OnCloseHandler(GameObject go)
		{
			//NetWorkScript.getInstance ().RequestReadyInRoom (GameModel.GetInstance.myHandInfor.uuid,GameModel.GetInstance.curRoomId);
			NetWorkScript.getInstance ().RequestExitRoom (GameModel.GetInstance.myHandInfor.uuid,GameModel.GetInstance.curRoomId);
		}

		private void _OnClickShareHandler(GameObject go)
		{
			if (null == ShareContentInfor.Instance.roomFightContent)
			{
				NetWorkScript.getInstance ().GetRoomShareData ();
			}
			else
			{				
				ShareContentInfor.Instance.setShareRoomTxt (GameModel.GetInstance.curRoomId);
				MBGame.Instance.ShareWeiChat (ShareContentInfor.Instance.roomFightContent);
			}
		}



		private Button btn_close;

		//private Image img_player1;
		//private Image img_player2;
		//private Image img_player3;
		//private Image img_player4;

		//private UIImageDisplay head1;
		//private UIImageDisplay head2;
		//private UIImageDisplay head3;
		//private UIImageDisplay head4;


		private List<UIImageDisplay> headList = new List<UIImageDisplay> ();
		private List<Text> nameList = new List<Text> ();
		private List<Image> readyList = new List<Image> ();
		private List<Image> headInforList = new List<Image> ();

		/// <summary>
		/// The color of the bright.设置按钮的颜色值
		/// </summary>
		private Color _brightColor = new Color (255f/255,255f/255,255f/255,1f);
		private Color _grayColor = new Color(85f/255,85f/255,85f/255,1f);


		private Button btn_ready;

		private Button btn_share;

		private Text lb_roomid;
		//List<Image> 
	}
}

