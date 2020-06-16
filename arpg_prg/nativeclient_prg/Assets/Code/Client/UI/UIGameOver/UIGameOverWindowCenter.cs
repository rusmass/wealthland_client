using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace Client.UI
{
	public partial class UIGameOverWindow
	{
		private void _OnInitCenter(GameObject go)
		{
            GameModel.GetInstance.MathWidthOrHeightByCondition(go,0);

			btn_rediet = go.GetComponentEx<Button> (Layout.btn_close);
			btn_return = go.GetComponentEx<Button> (Layout.btn_back);
			btn_share = go.GetComponentEx<Button> (Layout.btn_share);
			img_win = go.GetComponentEx<Image> (Layout.img_win);
			img_lose = go.GetComponentEx<Image> (Layout.img_lose);
//			btn_close.SetActiveEx (false);
			bg_bgimg = go.GetComponentEx<Image> (Layout.img_bgimg);
//			img_wintor = go.GetComponentEx<Image> (Layout.img_guan);
//			_imgSun = go.GetComponentEx<Animator> (Layout.animation_sun);
		}

		private void _OnShowCenter()
		{
            GameTimerManager.Instance.Stop();
            Audio.AudioManager.Instance.Stop ();
			LocalDataManager.Instance.IsNormalEnded = true;
			GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameOverState;
			var quitController = UIControllerManager.Instance.GetController<UIQuitFightGameWindowController> ();
			if (quitController.getVisible ())
			{
				quitController.setVisible (false);
			}
			_initTimeSan = 0;
			_isShowEffect = true;
//			_imgSun.enabled = true;
			EventTriggerListener.Get (btn_rediet.gameObject).onClick += _OnCloseWindow;
			EventTriggerListener.Get (btn_return.gameObject).onClick += _OnBackHandler;
			EventTriggerListener.Get (btn_share.gameObject).onClick += _OnClickShareHandler;
			var playerArr = PlayerManager.Instance.Players;
			var isUseFail = false;		
			Console.WriteLine ("当前的执行的次数哈哈哈哈哈"+playerArr.Length.ToString());

            //for (var i = 0; i < playerArr.Length; i++) 
            //{
            //	var playInfor=playerArr[i];
            //	var cansuccess = playInfor.CanInnerSuccess ();
            //	if (cansuccess == true)
            //	{
            //		if (i == 0)
            //		{
            //			Audio.AudioManager.Instance.VictoryMusic ();
            //		}
            //		_SetHeorCardData (img_win,playInfor,i,true);

            //	}
            //	else
            //	{
            //		if (i == 0)
            //		{
            //			//Audio.AudioManager.Instance.FailureMusic();
            //                     Audio.AudioManager.Instance.VictoryMusic();
            //                 }
            //		if (isUseFail == false)
            //		{
            //			_SetHeorCardData (img_lose,playInfor,i,false);
            //			isUseFail = true;
            //		}
            //		else
            //		{
            //			var tmpGo = img_lose.gameObject.CloneEx (true);
            //			var img = tmpGo.GetComponentEx<Image> (tmpGo.name);
            //			tmpGo.transform.SetParent(img_lose.gameObject.transform.parent);
            //			_SetHeorCardData (img,playInfor,i,false);
            //		}
            //	}
            //}

            for (var i = 0; i < playerArr.Length; i++)
            {
                var playInfor = playerArr[i];
                var cansuccess = playInfor.CanInnerSuccess();
                Audio.AudioManager.Instance.VictoryMusic();
             
               if (i == 0)
               {
                    _SetHeorCardData(img_win, playInfor, i, true);
               }               
               else
               {                   
                    var tmpGo = img_win.gameObject.CloneEx(true);
                    var img = tmpGo.GetComponentEx<Image>(tmpGo.name);
                    tmpGo.transform.SetParent(img_win.gameObject.transform.parent);
                    _SetHeorCardData(img, playInfor, i, false);                    
               }
            }
        }

		private void _SetHeorCardData(Image img,PlayerInfo player,int index,bool isWin=false)
		{
			var go = img.gameObject;
			var lb_name = go.GetComponentEx<Text> (Layout.lb_username);
			lb_name.text = player.playerName;

			var lb_money = go.GetComponentEx<Text> (Layout.lb_money);
			var tmpStr = player.totalMoney.ToString();
			if(player.totalMoney>=10000)
			{
				tmpStr = string.Format("{0}{1}",((float)((int)(player.totalMoney * 100 / 10000) / 100)).ToString (),"万") ;
			}
			lb_money.text = tmpStr;

			var lb_time = go.GetComponentEx<Text> (Layout.lb_timescore);
			lb_time.text = player.timeScore.ToString();

			var lb_quality = go.GetComponentEx<Text> (Layout.lb_qualityscore);
			lb_quality.text = player.qualityScore.ToString ();

			var imgload = go.GetComponentEx<Image> (Layout.img_roleImg);

			var roleimg = new UIImageDisplay (imgload);

			roleimg.Load (player.playerImgPath);

			_loadImgList.Add (roleimg);

            var txtLevel = go.GetComponentEx<Text>(Layout.txt_level);
            var levStr = "外圈";
            if(player.isEnterInner==true)
            {
                levStr = "内圈";
                if(player.CanInnerSuccess() ||player.IsSuccess)
                {
                    levStr = "核心圈";
                }               
            }

            txtLevel.text = levStr;

            var rankTxt = go.GetComponentEx<Text> (Layout.lb_rank);



			if(isWin==true)
			{
				rankTxt.text = "No.1";
			}
			else if (isWin == false)
			{
				rankIndex++;
				rankTxt.text = "No."+rankIndex.ToString ();

				var tmpPosition =img_win.transform.localPosition;
				go.transform.localScale = Vector3.one;

				Console.WriteLine ("当前的排名是:"+rankIndex.ToString());
				go.transform.localPosition = new Vector3 (_positionX[rankIndex-1],tmpPosition.y,tmpPosition.z);
			}
		}


		private void _TickCenter(float delatime)
		{
//			if (_isShowEffect == false)
//			{
//				return;
//			}
//
//			_initTimeSan += delatime;
//			if (_initTimeSan >= _targetTimeSun)
//			{
//				_initTimeSan = 0;
//				_isShowEffect = false;
//				_imgSun.SetActiveEx (false);
//				_imgSun.enabled = false;
//			}
		}

		public void ShowOverScene()
		{
			if (null != bg_bgimg)
			{
				bg_bgimg.SetActiveEx (true);
			}
		}

		public void HideOverScene()
		{
			if (null != bg_bgimg)
			{
				bg_bgimg.SetActiveEx (false);
			}
		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get (btn_rediet.gameObject).onClick -= _OnCloseWindow;
			EventTriggerListener.Get (btn_return.gameObject).onClick -= _OnBackHandler;
			EventTriggerListener.Get (btn_share.gameObject).onClick -= _OnClickShareHandler;

			for (var i = 0; i < _imgDisplay.Length; i++)
			{
				if (null != _imgDisplay [i])
				{
					_imgDisplay [i].Dispose ();
				}
			}
		}

		private void _OnClickShareHandler(GameObject go)
		{
			Console.WriteLine ("分享游戏");

			if (null == ShareContentInfor.Instance.normalTitleContent)
			{
				NetWorkScript.getInstance ().GetNormalShareData ();
			}
			else
			{
				var _shareboardController = UIControllerManager.Instance.GetController<UIShareBoardWindowController> ();
				_shareboardController.setVisible (true);
			}


		}

		private void _OnCloseWindow(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			HideOverScene ();
			var couclusionController = Client.UIControllerManager.Instance.GetController<UIConclusionController>();
			couclusionController.setUpBaseText(PlayerManager.Instance.HostPlayerInfo);
			couclusionController.setTiletText(true);	
			couclusionController.setVisible (true);

		}


		private void _OnBackHandler(GameObject go)
		{

			Audio.AudioManager.Instance.BtnMusic ();
			LocalDataManager.Instance.IsNormalEnded = true;
			Client.Unit.BattleController.Instance.Dispose();
			VirtualServer.Instance.Dispose ();
			MessageHint.Dispose ();

			var effectController = UIControllerManager.Instance.GetController<UISpecialeffectsWindowController> ();
			effectController.ReInitConttoller ();
			effectController.setVisible (false);		

			var battlerController = UIControllerManager.Instance.GetController<UIBattleController> ();
			if (null != battlerController)
			{
				battlerController.setVisible (false);
				battlerController.RestartList ();
			}

			Client.Scenes.SceneManager.Instance.CurrentScene.Unload();
			PlayerManager.Instance.Dispose ();

			GameModel.GetInstance.InitNetGameBackData ();

			MessageManager.getInstance ().netExitGameDeleteCards();

			var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
			controller.setVisible (true);
			controller.LoadGameHallUI();

//			if (GameModel.GetInstance.isPlayNet==true)
//			{
//				NetWorkScript.getInstance ().RequestExitRoom (GameModel.GetInstance.myHandInfor.uuid,GameModel.GetInstance.curRoomId);
//			}

			if(null != _controller)
			{
				_controller.setVisible (false);
			}

		}

		private void _OnDisposeCenter()
		{
			for (var i = 0; i < _loadImgList.Count; i++) 
			{
				var tmpValue=_loadImgList[i];
				if (null != tmpValue)
				{
					tmpValue.Dispose ();
				}
			}
			_loadImgList.Clear ();
		}

		private Button btn_rediet;

		private Button btn_return;

		private Button btn_share;

//		private Animator _imgSun;
		//private float _targetTimeSun=5f;
		private float _initTimeSan = 0;
		private bool _isShowEffect=true;

		private Image img_win;
		private Image img_lose;

		private Image bg_bgimg;

		private UIImageDisplay[] _imgDisplay=new UIImageDisplay[4];

//		private Image img_wintor;

		private int rankIndex = 1;

		private List<UIImageDisplay> _loadImgList = new List<UIImageDisplay> ();
		private float[] _positionX={-350f,-115f,121f,357f};


	}
}

