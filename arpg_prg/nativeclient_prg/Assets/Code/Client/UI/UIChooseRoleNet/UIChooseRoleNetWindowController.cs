using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using System.Collections.Generic;
using LitJson;


namespace Client.UI
{
    /// <summary>
    /// 网络版选择角色界面
    /// </summary>
	public class UIChooseRoleNetWindowController:UIController<UIChooseRoleNetWindow,UIChooseRoleNetWindowController>
	{
		protected override string _windowResource {
			get {
				//netgamechoose   netgamechoosescene
				return "prefabs/ui/scene/netselectrolegame.ab";
			}
		}

		public UIChooseRoleNetWindowController ()
		{
		}

		public PlayerInitData SelectRole(int index)
		{
//			var playerInitList = new List<PlayerInitData> ();
//			var template = MetadataManager.Instance.GetTemplateTable<PlayerInitData> ();
//			var it = template.GetEnumerator ();
//			while (it.MoveNext ()) 
//			{				
//				var value = it.Current.Value as PlayerInitData;
//
//				if (value.id != 100003 && value.id != 100004)
//				{
//					playerInitList.Add(value);			
//				}
//			}

			var tmpvalue=index;
			return _playerInitList[tmpvalue];
		}

		/// <summary>
		/// Gets the init data.获取人物基础信息
		/// </summary>
		/// <returns>The init data.</returns>
		public List<PlayerInitData> GetInitData()
		{			
			return _playerInitList;
		}


		public void SetInitData(List<PlayerInitData> value)
		{
			_playerInitList = value;
		}

		private  List<PlayerInitData> _playerInitList=null;

        /// <summary>
        /// 更新选择玩家的信息
        /// </summary>
        /// <param name="roleStatus"></param>
        /// <param name="playerState"></param>
        /// <param name="readyStatus"></param>
        public void UpdateSelectInfor(JsonData roleStatus, JsonData playerState, JsonData readyStatus)
        {
            if (null != _window && getVisible())
            {
                (_window as UIChooseRoleNetWindow).UpdateSelectInfor(roleStatus, playerState, readyStatus);
            }
        }


        /// <summary>
        /// Updates the sure infor.刷新确定选择的信息
        /// </summary>
        public void UpdateSureInfor(JsonData value)
		{
			if (null != _window && getVisible ())
			{
				(_window as UIChooseRoleNetWindow).UpdateSureInfor(value);
			}
		}


		/// <summary>
		/// Sets the select infor.选择某个角色
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetSelectInfor(NetChooseRoleInfor value)
		{
			if (null != _window && getVisible() == true)
			{
				(_window as UIChooseRoleNetWindow).NetSelectInfor(value);
			}
		}

		/// <summary>
		/// Sets the cancle infor. 放弃选择角色
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetCancleInfor(NetChooseRoleInfor value)
		{
			if (null != _window && getVisible() == true)
			{
				(_window as UIChooseRoleNetWindow).NetCancleInfor(value);
			}
		}

		public override void Tick (float deltaTime)
		{
			if (null != _window && this.getVisible ())
			{
				(_window as UIChooseRoleNetWindow).UpdateTimeHandler (deltaTime);
			}
		}

		/// <summary>
		/// Sets the ready image.显示准备的小icon
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetReadyImg(string value)
		{
			if (null != _window && getVisible () == true)
			{
				(_window as UIChooseRoleNetWindow).SetReadyImg (value);
			}
		}

        /// <summary>
        /// 获取选择角色信息的列表
        /// </summary>
        /// <returns></returns>
		public  List<NetChooseRoleInfor> GetRigthPlayerInfors()
		{
			return rightplayerinfors;
		}

        /// <summary>
        /// 设置选择角色信息的列表
        /// </summary>
        /// <param name="value"></param>
		public  void SetRightPlayerInfors(List<NetChooseRoleInfor> value)
		{
			rightplayerinfors = value;
		}

		private List<NetChooseRoleInfor> rightplayerinfors;
	}
}

