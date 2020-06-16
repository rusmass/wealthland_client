using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Client.UI
{
	public partial class UIGameHallWindow
	{
		private void _InitTop(GameObject go)
		{
			img_head = go.GetComponentEx<Image> (Layout.btn_head);
			img_headPlay = new UIImageDisplay (img_head);
			txt_id = go.GetComponentEx<Text> (Layout.txt_id);
			txt_name = go.GetComponentEx<Text> (Layout.txt_name);
			btn_mail = go.GetComponentEx<Button> (Layout.btn_mail);
			btn_gonggao = go.GetComponentEx<Button> (Layout.btn_gonggao);
			btn_head = go.GetComponentEx<Button> (Layout.btn_head);
			btn_share = go.GetComponentEx<Button> (Layout.btn_share);
			var tmpimg = go.GetComponentEx<Image> (Layout.img_herosex);
			img_herosex = new UIImageDisplay (tmpimg);
		}

		private void _ShowTop()
		{
			btn_mail.SetActiveEx (false);
//			btn_mail.SetActiveEx (true);

			EventTriggerListener.Get(btn_gonggao.gameObject).onClick+=_OnClickGonggao;
			EventTriggerListener.Get(btn_mail.gameObject).onClick+=_OnClickMail;
			EventTriggerListener.Get (btn_head.gameObject).onClick += _OnClickHeadHandler;
			EventTriggerListener.Get (btn_share.gameObject).onClick += _OnClickShareHanler;

			GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNormalState;
			GameModel.GetInstance.isNetGameSetShow = false;

//			Console.Error.WriteLine ("开始进入游戏大厅界面啦啦啦啦啦");

//			if (GameModel.GetInstance.isFirstInGameHall == true)
//			{
//				if (GameModel.GetInstance.gonggaoList.Count > 0)
//				{
//					var gonggao = UIControllerManager.Instance.GetController<UIGongGaoController> ();
//					gonggao.inforList = GameModel.GetInstance.gonggaoList;
//					gonggao.setVisible (true);
//				}
//
//			}

			UpdatePlayerHeadInfor ();
		}
        /// <summary>
        /// 更新玩家头像信息
        /// </summary>
		public void UpdatePlayerHeadInfor()
		{
			Console.WriteLine ("gggggggggggggggggggggggggggggggggg");
			if (null != img_head)
			{               
                //Debug.Log ("sdfsfdsdfsdfsfd---------------"+GameModel.GetInstance.myHandInfor.headImg);
                //img_headPlay.Load (GameModel.GetInstance.myHandInfor.headImg);
                AsyncImageDownload.Instance.SetAsyncImage(GameModel.GetInstance.myHandInfor.headImg, img_head);
                Resources.UnloadUnusedAssets();
            }

			if (null != img_herosex)
			{
				if (GameModel.GetInstance.myHandInfor.sex == 0)
				{
					img_herosex.Load (_herosexwoman);
				}
				else
				{
					img_herosex.Load (_herosexman);
				}

			}

			txt_name.text = GameModel.GetInstance.myHandInfor.nickName;
		}

		private void _HideTop()
		{
			EventTriggerListener.Get(btn_gonggao.gameObject).onClick-=_OnClickGonggao;
			EventTriggerListener.Get(btn_mail.gameObject).onClick-=_OnClickMail;
			EventTriggerListener.Get (btn_head.gameObject).onClick -= _OnClickHeadHandler;
			EventTriggerListener.Get (btn_share.gameObject).onClick -= _OnClickShareHanler;
		}



		private void _OnDisposeTop()
		{
			if (null != img_headPlay)
			{
				img_headPlay.Dispose ();
			}

			if (null != img_herosex)
			{
				img_herosex.Dispose ();
			}

            Resources.UnloadUnusedAssets();
        }
        /// <summary>
        /// 点击分享游戏
        /// </summary>
        /// <param name="go"></param>
		private void _OnClickShareHanler(GameObject go)
		{
			Console.WriteLine ("点击游戏分享");          

			if (null == ShareContentInfor.Instance.normalTitleContent)
			{
				NetWorkScript.getInstance ().GetNormalShareData ();
			}
			else
			{
				var _shareboardController = UIControllerManager.Instance.GetController<UIShareBoardWindowController> ();
                _shareboardController.ShareWindow = 0;
                _shareboardController.setVisible (true);
			}
		}

        /// <summary>
        /// 点击游戏公告
        /// </summary>
        /// <param name="go"></param>
		private void _OnClickGonggao(GameObject go)
		{
            //Console.WriteLine ("点击公告");
            //NetWorkScript.getInstance().GetShareDreamData();
            //return;

            //var controller = UIControllerManager.Instance.GetController<UICheckOutController>();
            //controller.setVisible(true);
            //return;           
            showGongao ();
		}
        /// <summary>
        /// 点击发送邮件
        /// </summary>
        /// <param name="go"></param>
		private void _OnClickMail(GameObject go)
		{
			Console.WriteLine ("点击邮件");

			var mailController = UIControllerManager.Instance.GetController<UIGameMailWindowController> ();
			mailController.setVisible (true);

		}

        /// <summary>
        /// 显示人物信息
        /// </summary>
        /// <param name="go"></param>
		private void  _OnClickHeadHandler(GameObject go)
		{
			Console.WriteLine ("显示人物信息");
            
            NetWorkScript.getInstance().GetGameRecordData();
            return;     

            //var controller = UIControllerManager.Instance.GetController<UIFellingWindowControll>();
            //controller.setVisible(true);

            //var webController = UIControllerManager.Instance.GetController<UINativeWebController>();
            //webController.SetTargetUrl("http://t.asdyf.com/");
            //webController.setVisible(true);
            //return;
            //return;         

            //var newInforPlayer = UIControllerManager.Instance.GetController<UIPlayerInforController>();
            //newInforPlayer.setVisible(true);

        }

		private void showGongao()
		{
			var gonggao = UIControllerManager.Instance.GetController<UIGongGaoController> ();
			gonggao.inforList = GameModel.GetInstance.gonggaoList;
			gonggao.setVisible (true);
		}


		private readonly string _herosexman = "share/atlas/battle/gamehall/heroman.ab";

		private readonly string _herosexwoman = "share/atlas/battle/gamehall/herowoman.ab";


		private Image img_head;

		private Button btn_head;

		private UIImageDisplay img_headPlay;

		private Text txt_name;

		private Text txt_id;

		private Button btn_gonggao;

		private Button btn_mail;

		private Button btn_share;

		private UIImageDisplay img_herosex;

	}
}

