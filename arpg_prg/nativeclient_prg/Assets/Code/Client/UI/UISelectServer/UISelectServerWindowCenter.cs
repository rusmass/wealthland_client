using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client.UI
{
	public partial class UISelectServerWindow
	{
		private void _initCenter(GameObject go)
		{
			_txtCurServer = go.GetComponentEx<Text> (Layout.txt_server);
			_btnServer = go.GetComponentEx<Button> (Layout.btn_server);
			btn_close = go.GetComponentEx<Button> (Layout.btn_close);
		}

		private void _ShowCenter()
		{
			this._txtCurServer.text = _controller.curServer;

			var tmpList = _controller.serverList;

			for (int i=0; i<tmpList.Count; i++)
			{
				Button tmpBtn;
				if (i == 0)
				{
					tmpBtn = _btnServer;
				}
				else
				{
					tmpBtn =_btnServer.gameObject.CloneEx().GetComponent<Button>()  ;
					//tmpBtn =(Button) _btnServer.transform CloneEx(false)  ;
					tmpBtn.transform.SetParent (_btnServer.transform.parent);
					tmpBtn.transform.position = _btnServer.transform.position;
					tmpBtn.transform.localScale = Vector3.one;
					tmpBtn.transform.rotation =_btnServer.transform.rotation;
				}
				tmpBtn.gameObject.GetComponentEx<Text> ("Text").text = tmpList[i];
				tmpBtn.name = "server" + i;
				EventTriggerListener.Get (tmpBtn.gameObject).onClick += _OnClickServer;
			
				_serverList.Add (tmpBtn);
			}

			EventTriggerListener.Get (btn_close.gameObject).onClick+=_OnClickCloseHandler;
		}

		private void _OnClickServer(GameObject go)
		{
			var tmpIndex =int.Parse(go.name.Substring (6, 1));
			///Console.WriteLine ("当前点击的服务器名字是--------"+tmpIndex);
			/// 
			var tmpStr=_controller.serverList[tmpIndex];
			_txtCurServer.text = tmpStr;

			UIControllerManager.Instance.GetController<UILoginController> ().SetServerName (tmpStr,true);
		}

		private void _OnClickCloseHandler(GameObject go)
		{
			this._controller.setVisible (false);
		}

		private void _HideCenter()
		{
			EventTriggerListener.Get (btn_close.gameObject).onClick-=_OnClickCloseHandler;

			for (int i=0; i<_serverList.Count; i++)
			{
				var tmpBtn = _serverList[i];
				EventTriggerListener.Get (tmpBtn.gameObject).onClick -= _OnClickServer;
			}
		}
		           
		private void _DisposeCenter()
		{
			
		}


		private Text _txtCurServer;

		private Button _btnServer;

		private Button btn_close;

		private List<Button> _serverList=new List<Button>();
	}
}

