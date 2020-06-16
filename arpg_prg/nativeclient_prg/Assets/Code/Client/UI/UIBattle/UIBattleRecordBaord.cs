using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
namespace Client.UI
{
    /// <summary>
    /// 系统提示和聊天的面板
    /// </summary>
	public class UIBattleRecordBaord
	{
		public UIBattleRecordBaord (GameObject go)
		{
//			btn_open = go.GetComponentEx<Button>(Layout.btn_open);
			btn_systemtip = go.GetComponentEx<Button> (Layout.btn_close);
			bgTransform = go.transform;
		
			recordTip = go.GetComponentEx<Text> (Layout.recordTip);

			_scrollRect = go.GetComponentEx<ScrollRect>("ScrollView");
			_scrollerContent = go.DeepFindEx("Content");

			btn_micchat = go.GetComponentEx<Button> (Layout.btn_micchat);
			btn_sendchat = go.GetComponentEx<Button> (Layout.btn_sendchat);
			btn_roomchat = go.GetComponentEx<Button> (Layout.btn_roomChat);		

			chatItemTxt = go.GetComponentEx<Text> (Layout.lb_txt);

			chatItemTxt.SetActiveEx (false);

			_scrollSize = _scrollRect.viewport.sizeDelta;

			_chatGridRect = go.GetComponentEx<ScrollRect> ("scrollchat");

			img_chat = go.GetComponentEx<Image> (Layout.img_chat);
			inputchat = go.GetComponentEx<InputField> (Layout.inputchat);

			var tmpimg = go.GetComponentEx<Image> (Layout.btn_close);
			img_systembtn = new UIImageDisplay (tmpimg);

			tmpimg = go.GetComponentEx<Image> (Layout.btn_roomChat);
			img_roomchatbtn = new UIImageDisplay (tmpimg);

//			_CreateChatGrid(chatItemTxt.gameObject);
//			_CreateWrapGrid (recordTip.gameObject);
			_OnShowSystemBoard ();

			if (GameModel.GetInstance.isPlayNet == false)
			{
				btn_roomchat.SetActiveEx (false);
			}

			btn_sendNormalChat = go.GetComponentEx<Button> (Layout.btn_normaChat);
			img_normalChatBg = go.GetComponentEx < Image > (Layout.img_normalchatbg);

			img_normalChatBg.SetActiveEx(false);

			var btn_tmpNormal = go.GetComponentEx<Button> (Layout.btn_normal0);
			for (var i = 0; i < normalChatList.Count; i++)
			{
				Button tmpbtn = null;
				if (i == 0)
				{
					tmpbtn = btn_tmpNormal;
					tmpbtn.name = "normal0";
				}
				else
				{
					tmpbtn = btn_tmpNormal.gameObject.CloneEx ().GetComponent<Button> ();
					tmpbtn.name = "normal" + i.ToString();
					tmpbtn.transform.SetParent (btn_tmpNormal.transform.parent);
					tmpbtn.transform.localPosition = btn_tmpNormal.transform.localPosition;
					tmpbtn.transform.localScale = btn_tmpNormal.transform.localScale;
				}
				if (null != tmpbtn)
				{
					tmpbtn.gameObject.GetComponentInChildren<Text> ().text = normalChatList[i];
					normalBtnList.Add (tmpbtn);
				}
			}

		}

        /// <summary>
        /// 创建聊天窗体和数据
        /// </summary>
        /// <param name="go"></param>
		private void _CreateChatGrid(GameObject go)
		{
			var items = _controller.GetChatList();
//			Console.Warning.WriteLine ("当前数组的长度是:"+items.Count.ToString());
			if (items.Count <= 0)
			{
				go.SetActive (false);
				return;
			}
			else
			{
				go.SetActive (true);
			}

			if (null == _chatGrid)
			{
				_chatGrid = new UIWrapGrid(go, items.Count);

				for (int i = 0; i < _chatGrid.Cells.Length; ++i)
				{
					var cell = _chatGrid.Cells[i];
					cell.DisplayObject = new UIBattleChatItem(cell.GetTransform().gameObject);
				}
				_chatGrid.OnRefreshCell += _OnRefreshCell;				
			}
			else
			{
				_chatGrid.GridSize = items.Count;
			}
			//			_chatGrid.Cells.GetUpperBound(items.Count-1);
			_chatGrid.Refresh();

			if (_chatGridRect.verticalNormalizedPosition > 0.3f)
			{
				_chatGridRect.verticalNormalizedPosition=0.05f;
			}
			else
			{
				_chatGridRect.verticalNormalizedPosition=0;
			}

//			_chatGridRect.verticalNormalizedPosition=0;

		}

        /// <summary>
        /// 刷新聊天数据的回调函数
        /// </summary>
        /// <param name="cell"></param>
		private void _OnRefreshCell(UIWrapGridCell cell)
		{
			//			Console.Warning.WriteLine ("当前的索引值:"+cell.Index.ToString());
			var index = cell.Index;
			var value = _controller.GetChatValueByIndex(index);
			var display = cell.DisplayObject as UIBattleChatItem;
			display.Refresh(value);
		}

		private void _CreateWrapGrid(GameObject go)
		{
			var items = _controller.GetDataList();

			recordTip.SetTextEx(items[0]);
			_tipList.Add (recordTip);

		}

//		private void _OnRefreshCell(UIWrapGridCell cell)
//		{
//			var index = cell.Index;
//			var value = _controller.GetValueByIndex(index);
//
//			var display = cell.DisplayObject as UIBattleRecodeItem;
//
//			display.Refresh(value);
//		}

        /// <summary>
        /// 添加baard的时，添加事件
        /// </summary>
		public void OnShowRecordBoard()
		{
//			EventTriggerListener.Get(btn_open.gameObject).onClick+=_OnOpenBoard;
			EventTriggerListener.Get(btn_systemtip.gameObject).onClick+=_OnCloseBoard;
			EventTriggerListener.Get (btn_roomchat.gameObject).onClick += _ShowChatBoard;
			EventTriggerListener.Get (btn_sendchat.gameObject).onClick += _OnSendChathandler;
			EventTriggerListener.Get (btn_sendNormalChat.gameObject).onClick += _OnClickNormalChatBtn;
			for (var i = 0; i < normalBtnList.Count; i++)
			{
				var tmpBtn = normalBtnList [i];
				EventTriggerListener.Get (tmpBtn.gameObject).onClick += _OnSendNormalChatHander;
			}            
			inputchat.onEndEdit.AddListener (_OnEndedTxt);
//			btn_open.SetActiveEx (false);
		}

        /// <summary>
        /// 设置battlecontroller，初始化数据
        /// </summary>
        /// <param name="value"></param>
		public void SetBattleController(UIBattleController value)
		{
			_controller = value;
			_CreateWrapGrid(recordTip.gameObject);
		}

        /// <summary>
        /// 隐藏玩家数据
        /// </summary>
		public void OnHideRecordBoard()
		{
//			EventTriggerListener.Get(btn_open.gameObject).onClick-=_OnOpenBoard;
			EventTriggerListener.Get(btn_systemtip.gameObject).onClick-=_OnCloseBoard;
			EventTriggerListener.Get (btn_sendchat.gameObject).onClick -= _OnSendChathandler;
			EventTriggerListener.Get (btn_roomchat.gameObject).onClick -= _ShowChatBoard;

			for (var i = 0; i < normalBtnList.Count; i++)
			{
				var tmpBtn = normalBtnList [i];
				EventTriggerListener.Get (tmpBtn.gameObject).onClick -= _OnSendNormalChatHander;
			}

			inputchat.onEndEdit.RemoveListener (_OnEndedTxt);
		}

        /// <summary>
        /// 输入聊天话术后，如果超过范围，截取文本
        /// </summary>
        /// <param name="value"></param>
		private void _OnEndedTxt(string value)
		{
			if (value.Length > 20)
			{
				inputchat.text = value.Substring(0,20);
			}			
		}

        /// <summary>
        /// 释放掉资源
        /// </summary>
		public void OnDisposeBoard()
		{
			if (null != _chatGrid)
			{
				_chatGrid.Dispose ();
			}
		}
        /// <summary>
        /// 发送聊天记录
        /// </summary>
        /// <param name="go"></param>
		private void _OnSendNormalChatHander(GameObject go)
		{
			var tmpIndex =int.Parse(go.name.Substring (6));
			var tmpStr = normalChatList [tmpIndex];
			if (null != tmpStr && tmpStr != "")
			{
				NetWorkScript.getInstance ().RoomChatBroad (tmpStr);
			}
			img_normalChatBg.SetActiveEx (false);			
		}

		private void _OnClickNormalChatBtn(GameObject go)
		{
			var boo = img_normalChatBg.IsActive ();
			img_normalChatBg.SetActiveEx (!boo);
		}

		private void _OnSendChathandler(GameObject go)
		{
			if (inputchat.text == "")
			{
				MessageHint.Show ("请输入对话文字");
				return;
			}

//			var chatvo = new NetChatVo ();
//			chatvo.chat = inputchat.text;
//			chatvo.playerId = GameModel.GetInstance.myHandInfor.uuid;
//			chatvo.playerName = GameModel.GetInstance.myHandInfor.nickName;
//			chatvo.playerName="ssssssss";

			NetWorkScript.getInstance ().RoomChatBroad (inputchat.text);
			inputchat.text = "";
//			Console.Warning.WriteLine ("ssssssssssssssssddddd");

//			_controller.UpdateChatData (chatvo);
		}

		private void _ShowChatBoard(GameObject go)
		{
			_OnShowChatBoard ();
		}		

		private void _OnCloseBoard(GameObject go)
		{
			_OnShowSystemBoard ();

//			UIBattleController.isOpenRecordWindow = false;
//			btn_close.SetActiveEx(false);
//			var quance = DOTween.Sequence ();
//			quance.Append (bgTransform.DOScale(Vector3.one*0.01f,0.5f));
//			quance.AppendCallback (()=>{
//				btn_open.SetActiveEx(true);
//				btn_open.transform.SetParent(bgTransform.parent);
//				btn_open.transform.localScale=Vector3.one;
//				btn_open.transform.localPosition=new Vector3(119,-178,0);
//			});
		}

		private void _OnShowSystemBoard()
		{
			_scrollRect.SetActiveEx (true);
//			Console.Error.WriteLine("面板消失啦啦啦啦啦啦啦啦啦");
			img_chat.SetActiveEx (false);
			_SelectLeftBtn (true);
		}

        /// <summary>
        /// 显示聊天窗口
        /// </summary>
		private void _OnShowChatBoard()
		{
			_scrollRect.SetActiveEx (false);
			img_chat.SetActiveEx (true);
			_SelectLeftBtn (false);
		}

        /// <summary>
        /// 刷新聊天记录组件
        /// </summary>
		public void UpdateChatItemRecord()
		{
			_CreateChatGrid(chatItemTxt.gameObject);
		}

        /// <summary>
        /// 刷新提示面板
        /// </summary>
		public void UpdateTipRecord()
		{
//			if (null != _uiwrapGridContent)
//			{
//				_uiwrapGridContent.GridSize = _controller.GetDataList ().Count;
//			}
			var tiplist=_controller.GetDataList();
			if (tiplist.Count > 15)
			{			
				for (var i = 1; i < 11; i++)
				{
                    if(_tipList.Count>=2)
                    {
                        var tmpTxt = _tipList[1];

                        if (null != tmpTxt)
                        {
                            _tipList.RemoveAt(1);
                            GameObject.Destroy(tmpTxt.gameObject);
                        }
                    }					
				}
                //tiplist.TrimExcess();
                tiplist.RemoveRange (1, 11);
               // Console.Error.WriteLine("当前话术数组的长度："+tiplist.Count.ToString());
			}

			var tmpStr = _controller.GetValueByIndex (tiplist.Count-1);

			var txt = recordTip.gameObject.CloneEx (true).GetComponent<Text>();
			txt.SetTextEx (tmpStr);
			txt.transform.SetParent (recordTip.transform.parent);
			txt.transform.localScale = Vector3.one;
//			var txtHight= txt.rectTransform.sizeDelta.y;
			var txtPosition = txt.transform.localPosition;
			txt.transform.localPosition = new Vector3 (txtPosition.x,txtPosition.y,0);
			_tipList.Add (txt);

			if (_tipList.Count > 4)
			{
//				var tmpSize=_scrollerContent.GetComponent<RectTransform>().sizeDelta;
//				if (tmpSize.y > _scrollSize.y)
//				{
//					var tmpPosition =_scrollerContent.transform.localPosition;
//					_scrollerContent.localPosition = new Vector3 (tmpPosition.x,tmpSize.y-60,tmpPosition.z);
//				}
//				if(_scrollRect.verticalScrollbar.IsActive())
//				{
//					_scrollRect.verticalScrollbar.value = 0;
//				}
				if (_countTime == null)
				{
					_countTime = new Counter (_delayTime);
				}
				else
				{
					_countTime.Reset();
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
		public void RecordTick(float deltaTime)
		{
			if (null != _countTime && _countTime.Increase (deltaTime))
			{
				if(null !=_scrollRect)
				{
					if(_scrollRect.verticalScrollbar.IsActive())
					{
						_scrollRect.verticalScrollbar.value = 0;
					}
				}
				_countTime = null;
			}
		}

        /// <summary>
        /// 切换系统聊天和房间聊天
        /// </summary>
        /// <param name="value"></param>
		private void _SelectLeftBtn(bool value)
		{
			if (value == true)
			{
//				btn_systemtip.gameObject.GetComponentInChildren<Text> ().color = _grayColor;
				btn_systemtip.gameObject.GetComponentInChildren<Text> ().text = "系统";
				btn_roomchat.gameObject.GetComponentInChildren<Text> ().text = "<color=#b4b4b4ff>房间</color>";

				img_systembtn.Load (_brightbgPath);
				img_roomchatbtn.Load (_graybgPath);
			}
			else
			{
				btn_systemtip.gameObject.GetComponentInChildren<Text> ().text = "<color=#b4b4b4ff>系统</color>"; ;
				btn_roomchat.gameObject.GetComponentInChildren<Text> ().text = "房间";

				img_systembtn.Load (_graybgPath);
				img_roomchatbtn.Load (_brightbgPath);
			}
		}


		private Counter _countTime;
		private float _delayTime= 0.2f;


		private Button btn_systemtip;
//		private Button btn_open;

		private Text recordTip;

		private Text chatItemTxt;

		private UIWrapGrid _chatGrid;
		private ScrollRect _chatGridRect;


		private ScrollRect _scrollRect;
		private Image img_chat;
		private Button btn_sendchat;
		private Button btn_micchat;
		private Button btn_roomchat;
		private InputField inputchat;

		private Transform _scrollerContent;
		private Vector2 _scrollSize;

		private List<Text> _tipList=new List<Text>();

		private Transform bgTransform;
		private UIBattleController _controller;

		private string _brightbgPath="share/atlas/battle/battlemain/chatbglight.ab";
		private string _graybgPath=  "share/atlas/battle/battlemain/chatbggray.ab";

		//private Color _grayColor = new Color (50f, 50f, 50f, 255f);
		//private Color _brightColor = new Color (255f,255f,255f,255f);
		private UIImageDisplay img_systembtn;
		private UIImageDisplay img_roomchatbtn;


		private List<string> normalChatList =new List<string>() { "很高兴见到你们","抓紧时间唷！","打得漂亮！","加油！加油！","不错！不错！"};
		private Button btn_sendNormalChat;
		private Image  img_normalChatBg;
		private List<Button> normalBtnList = new List<Button> ();
		//private float normalbtnHeight=24.3f;



		class Layout
		{
			public static string img_tipboard="tipRecordBoard";
			public static string btn_close="closeboard";
			public static string btn_open="openboard";
			public static string recordTip="recordTip";

			public static string img_chat="chatboardbgone";
			public static string btn_sendchat="btn_sendchat";
			public static string btn_micchat="btn_micchat";
			public static string btn_roomChat="btn_roomchat";

			public static string lb_txt="txtname";

			public static string inputchat="chatinput";


			public static string btn_normaChat="btn_normalchat";
			public static string img_normalchatbg="img_normalchat";
			public static string btn_normal0="btn_normal0";

		}
	}
}

