using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client.UI
{
	public partial  class UIGameHallWindow
	{
		private void _InitEnterRoom(GameObject go)
		{
			lb_roomNum =go.GetComponentEx<Text>(Layout.lb_numtxt);

			btn_numSure = go.GetComponentEx<Button> (Layout.btn_numsure);

			img_enteroom = go.GetComponentEx<Image> (Layout.img_enteroom);
			btn_reput = go.GetComponentEx<Button> (Layout.btn_reput);
			btn_delete = go.GetComponentEx<Button> (Layout.btn_delete);

			btn_closeenteromm = go.GetComponentEx<Button> (Layout.btn_closeenteroom);


			for (int i = 0; i < 10; i++)
			{
				var tmpbtn = go.GetComponentEx<Button> (Layout.btn_num+i);

				tmpbtn.name = "btnnum" + i;

				btn_numList.Add (tmpbtn);
			}

		}

		private void _ShowEnterRoom()
		{
			img_enteroom.SetActiveEx (false);	

			for (int i = 0; i < btn_numList.Count; i++)
			{
				EventTriggerListener.Get (btn_numList [i].gameObject).onClick += _OnSureClickNum;
			}

			EventTriggerListener.Get (btn_closeenteromm.gameObject).onClick += _onCloseEnteroomHandler;
			EventTriggerListener.Get (btn_reput.gameObject).onClick += _OnReputHandler;
			EventTriggerListener.Get (btn_delete.gameObject).onClick += _OnDeleteHandler;
			EventTriggerListener.Get (btn_numSure.gameObject).onClick += _OnSureEnteroomHandler;

		}

		private void _HideEnterRoom()
		{
			EventTriggerListener.Get (btn_closeenteromm.gameObject).onClick -= _onCloseEnteroomHandler;
			EventTriggerListener.Get (btn_reput.gameObject).onClick -= _OnReputHandler;
			EventTriggerListener.Get (btn_delete.gameObject).onClick -= _OnDeleteHandler;
			EventTriggerListener.Get (btn_numSure.gameObject).onClick -= _OnSureEnteroomHandler;

			for (int i = 0; i < btn_numList.Count; i++)
			{
				EventTriggerListener.Get (btn_numList [i].gameObject).onClick -= _OnSureClickNum;
			}

			btn_numList.Clear ();
		}
	
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="go"></param>
		private void _onCloseEnteroomHandler(GameObject go)
		{
			img_enteroom.SetActiveEx (false);
		}

        /// <summary>
        /// 点击加入房间
        /// </summary>
        /// <param name="go"></param>
		private void _OnSureEnteroomHandler(GameObject go)
		{
			//提交进入游戏房间
			var tmpstr = lb_roomNum.text;
			if (tmpstr.Length < 6)
			{
				MessageHint.Show ("请输入6位房间号");
				return;
			}

			GameModel.GetInstance.curRoomId = tmpstr;
			NetWorkScript.getInstance().RequestEnterRoom(GameModel.GetInstance.myHandInfor.uuid,tmpstr);
		}
        /// <summary>
        /// 重新数据房间号
        /// </summary>
        /// <param name="go"></param>
		private void _OnReputHandler(GameObject go)
		{
			lb_roomNum.text = "";
		}

        /// <summary>
        /// 删除房间号
        /// </summary>
        /// <param name="go"></param>
		private void _OnDeleteHandler(GameObject go)
		{			
			var tmpstr = lb_roomNum.text;
			if (tmpstr.Length < 1)
			{
				return;
			}

			var sub = tmpstr.Substring (0, tmpstr.Length-1);
			lb_roomNum.text = sub;
		}
        /// <summary>
        /// 点击确定
        /// </summary>
        /// <param name="go"></param>
		private void _OnSureClickNum(GameObject go)
		{
			if (lb_roomNum.text.Length == 6)
			{
				return;
			}

			//var num =int.Parse( go.name.Substring (6,1));
			var num = go.name.Substring(6,1);

			//roomNumStr += num;

			lb_roomNum.text += num;

			if (lb_roomNum.text.Length == 6)
			{
				var tmpstr = lb_roomNum.text;

				GameModel.GetInstance.curRoomId = tmpstr;
				NetWorkScript.getInstance().RequestEnterRoom(GameModel.GetInstance.myHandInfor.uuid,tmpstr);
			}
		}

        /// <summary>
        /// 显示输入房间号
        /// </summary>
		private void _OnShowEnteroomImg()
		{
			img_enteroom.SetActiveEx (true);

			lb_roomNum.text = "";
			roomNumStr = "";
		}

        /// <summary>
        /// 隐藏
        /// </summary>
		public void OnHideEnterroomImg()
		{
			img_enteroom.SetActiveEx (false);	
		}

		/// <summary>
		/// Raises the re init number text event.重置输入的房间号 
		/// </summary>
		public void OnReInitNumTxt()
		{
			if (null != lb_roomNum)
			{
				lb_roomNum.text = "";
			}
		}

		private List<Button> btn_numList=new List<Button>();

		private Button btn_numSure;
		private Button btn_reput;
		private Button btn_delete;

		private Image img_enteroom;

		private Button btn_closeenteromm;

		private Text lb_roomNum;

		private string roomNumStr;
	}
}

