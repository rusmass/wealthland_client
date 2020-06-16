using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using System.Collections.Generic;


namespace Client.UI
{
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

		public  List<NetChooseRoleInfor> GetRigthPlayerInfors()
		{
			return rightplayerinfors;
		}

		public  void SetRightPlayerInfors(List<NetChooseRoleInfor> value)
		{
			rightplayerinfors = value;
		}

		private List<NetChooseRoleInfor> rightplayerinfors;
	}
}

