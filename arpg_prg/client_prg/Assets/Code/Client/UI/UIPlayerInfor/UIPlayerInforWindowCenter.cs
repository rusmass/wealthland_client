using System;
using UnityEngine;
using UnityEngine.UI;
using LitJson;


namespace Client.UI
{
	public partial class UIPlayerInforWindow
	{

		private void _InitCenter(GameObject go)
		{
			btn_man = go.GetComponentEx<Button> (Layout.btn_man);
			btn_woman = go.GetComponentEx<Button> (Layout.btn_woman);

			input_name = go.GetComponentEx<InputField> (Layout.input_name);

			btn_head1 = go.GetComponentEx<Button> (Layout.btn_head1);
			btn_head2 = go.GetComponentEx<Button> (Layout.btn_head2);
			btn_head3 = go.GetComponentEx<Button> (Layout.btn_head3);
			btn_head4 = go.GetComponentEx<Button> (Layout.btn_head4);

			btn_close = go.GetComponentEx<Button> (Layout.btn_close);
			btn_sure = go.GetComponentEx<Button> (Layout.btn_sure);

			btn_selecthead = go.GetComponentEx<Button> (Layout.btn_headselect);
			img_head = go.GetComponentEx<Image> (Layout.img_head);
			lb_tip = go.GetComponentEx<Text> (Layout.lb_tip);

			img_selecthead =new UIImageDisplay(go.GetComponentEx<Image> (Layout.btn_headselect));

//			_imgTitle =new UIImageDisplay( go.GetComponentEx<Image> (Layout.img_title));
			txt_title = go.GetComponentEx<Text> (Layout.txt_title);

		}

		private void _ShowCenter()
		{
			_hideImgHead ();

			lb_tip.SetActiveEx (false);

			_SelectSexHand (sexone);

			EventTriggerListener.Get (btn_head1.gameObject).onClick += _OnScelectHeadHandler;
			EventTriggerListener.Get (btn_head2.gameObject).onClick += _OnScelectHeadHandler;
			EventTriggerListener.Get (btn_head3.gameObject).onClick += _OnScelectHeadHandler;
			EventTriggerListener.Get (btn_head4.gameObject).onClick += _OnScelectHeadHandler;

			EventTriggerListener.Get (btn_man.gameObject).onClick += _OnSelectSexHandler;
			EventTriggerListener.Get (btn_woman.gameObject).onClick += _OnSelectSexHandler;

			EventTriggerListener.Get (btn_close.gameObject).onClick += _OnCloseHandler;
			EventTriggerListener.Get (btn_sure.gameObject).onClick += _OnSureHandler;

			EventTriggerListener.Get (btn_selecthead.gameObject).onClick += _OnClickHeadHandler;

			if (_controller.windowType == 1)
			{
//				_imgTitle.Load (playerInforPath);
				txt_title.text = "人物信息";
			}

			var name = GameModel.GetInstance.myHandInfor.nickName;
			if (null != "")
			{
				input_name.text =name;
			}

			var _sex = GameModel.GetInstance.myHandInfor.sex;
			_SelectSexHand (_sex);
			//sexone = _sex;

			var _headPath = GameModel.GetInstance.myHandInfor.headImg;
			if(_headPath!="")
			{
				img_selecthead.Load (_headPath);
				headpath = _headPath;
			}
			else
			{
				img_selecthead.Load (defaultPath);
//				img_selecthead.Load (headPath1);
//				headpath = headPath1;
			}


		}

		private void _HideCenter()
		{
			EventTriggerListener.Get (btn_head1.gameObject).onClick -= _OnScelectHeadHandler;
			EventTriggerListener.Get (btn_head2.gameObject).onClick -= _OnScelectHeadHandler;
			EventTriggerListener.Get (btn_head3.gameObject).onClick -= _OnScelectHeadHandler;
			EventTriggerListener.Get (btn_head4.gameObject).onClick -= _OnScelectHeadHandler;

			EventTriggerListener.Get (btn_man.gameObject).onClick -= _OnSelectSexHandler;
			EventTriggerListener.Get (btn_woman.gameObject).onClick -= _OnSelectSexHandler;

			EventTriggerListener.Get (btn_close.gameObject).onClick -= _OnCloseHandler;
			EventTriggerListener.Get (btn_sure.gameObject).onClick -= _OnSureHandler;

			EventTriggerListener.Get (btn_selecthead.gameObject).onClick -= _OnClickHeadHandler;
		}

		private void _DisposeCenter()
		{
//			if (null != _imgTitle)
//			{
//				_imgTitle.Dispose ();
//			}

			if (null != img_selecthead)
			{
				img_selecthead.Dispose ();
			}
		}

		private void _OnClickHeadHandler(GameObject go)
		{
			_showImgHead ();
		}

		private void _showImgHead()
		{
			img_head.SetActiveEx (true);
		}

		private void _hideImgHead()
		{
			img_head.SetActiveEx (false);
			img_selecthead.Load (headpath);
		}

		private void _OnSelectSexHandler(GameObject go)
		{
			var sexstri = go.name;

			switch (sexstri)
			{
			case "btn_woman":
				this.sexname = "0";
				_SelectSexHand (0);
				break;
			case "btn_man":
				this.sexname = "1";
				_SelectSexHand (1);
				break;
			default:
				break;
			}
		}

		private void _SelectSexHand(int value)
		{
			sexone = value;

			if (sexone == 1)
			{
				btn_man.transform.localScale = Vector3.one * 1.2f;
				btn_woman.transform.localScale = Vector3.one;
			}
			else
			{
				btn_woman.transform.localScale = Vector3.one * 1.2f;
				btn_man.transform.localScale = Vector3.one;
			}
			
		}

		private void _OnScelectHeadHandler(GameObject go)
		{
			var head = go.name;

			switch (head)
			{
			case "btn_head1":
				this.headpath = headPath1;
				break;
			case "btn_head2":
				this.headpath = headPath2;
				break;
			case "btn_head3":
				this.headpath = headPath3;
				break;
			case "btn_head4":
				this.headpath = headPath4;
				break;
			default:
				break;
			}

			_hideImgHead ();
		}

		private void _OnCloseHandler(GameObject go)
		{
			_controller.setVisible (false);
		}

		private void _OnSureHandler(GameObject go)
		{
			var data = new JsonData ();

			if (input_name.text == "")
			{
				MessageHint.Show ("请输入姓名");
				return;
			}

			if (headpath == "")
			{
				MessageHint.Show ("请选择人物头像");
				return;
			}

			data["gender"]=this.sexname;
			data["playerImg"]=this.headpath;
			data["nick"]=input_name.text ;

			if (_controller.windowType == 0)
			{
				HttpRequestManager.GetInstance ().GetPlayerInfor (data.ToJson(),_CreateRoleSuccess);
			}
			else
			{
				var tmpInfor = GameModel.GetInstance.tmpModifyPlayerInfor;
				tmpInfor.nickName = input_name.text;
				tmpInfor.sex =int.Parse(this.sexname);
				tmpInfor.headImg = this.headpath;
				NetWorkScript.getInstance ().ModifyPlayerInfor (input_name.text ,tmpInfor.sex,headpath,GameModel.GetInstance.myHandInfor.uuid);
			}



		}

		private void _CreateRoleSuccess()
		{
			var logcontroller = Client.UIControllerManager.Instance.GetController<UILoginController> ();
			if (logcontroller.getVisible () == true)
			{
				logcontroller.setVisible (false);
			}

			var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
			controller.setVisible (true);
			//controller.LoadSeletRoleUI();
			controller.LoadGameHallUI();
			_controller.setVisible (false);
			NetWorkScript.getInstance ().ConnetServer (GameModel.GetInstance.myHandInfor.uuid);
			_controller.setVisible (false);
		}

		private void _hideTipText()
		{
			lb_tip.SetActiveEx (false);
		}

		private void _showTip(string value)
		{
			lb_tip.SetActiveEx (true);
			lb_tip.text =value;
		}

		/// <summary>
		/// The sexone. 0 女  1 男
		/// </summary>
		private int sexone = 0;


		private string headpath="";

		private string sexname="0";

		private string namestring="";

		private InputField input_name;

		private Button btn_man;

		private Button btn_woman;

		private Button btn_head1;

		private Button btn_head2;

		private Button btn_head3;

		private Button btn_head4;

		private Button btn_close;

		private Button btn_sure;

		private Image img_head;

		private Text lb_tip;

		private Button btn_selecthead; 
		private UIImageDisplay  img_selecthead;


//		private UIImageDisplay _imgTitle;
		private Text txt_title;

		private string newPlayerPath="share/atlas/battle/playerinfor/chuangjianjiaose.ab";
		private string playerInforPath="share/atlas/battle/playerinfor/renwuxinxi.ab";

		private string headPath1 = "share/atlas/battle/playerhead/head1.ab";
		private string headPath2 = "share/atlas/battle/playerhead/head2.ab";
		private string headPath3 = "share/atlas/battle/playerhead/head3.ab";
		private string headPath4 = "share/atlas/battle/playerhead/head4.ab";
		private string defaultPath = "share/atlas/battle/createrole/defaultheadbg.ab";


	}
}

