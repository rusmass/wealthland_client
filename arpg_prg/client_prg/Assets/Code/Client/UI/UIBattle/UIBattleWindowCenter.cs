using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;
using Client.Unit;
using Core.Web;

namespace Client.UI
{
	public partial class UIBattleWindow
	{
		private void _InitCenter(GameObject go)
		{
			_btnCraps = go.GetComponentEx<Button>(Layout.btn_craps);
			_animTransform = go.DeepFindEx (Layout.anim_craps);
            _tfCrapsDisplay = go.DeepFindEx(Layout.crapsdisplay);
            _tfBtnCraps = go.DeepFindEx(Layout.btn_craps);

            var image = go.GetComponentEx<Image> (Layout.image_icon);
			_imageDisplay = new UIImageDisplay (image);

			_btnStart = go.GetComponentEx<Button>(Layout.btnStart);
			_startBoard = go.GetComponentEx<Image>(Layout.startBoard);

			//_ShowOtherCraps ();

			//2016-9-21  zll  fix
			//2016-9-28  zll  注释掉
//			_gameStarted = true;
//			_btnStart.SetActiveEx (false);

			// ytf新加提示面吧
//			btn_tipBtn = go.GetComponentEx<Button>(Layout.btn_tipbtn);
//			lb_tiptxt = go.GetComponentEx<Text> (Layout.lb_tipTxt);
//			img_tipBoard = go.GetComponentEx<Image> (Layout.img_tipBoard);


			//2016-09-28 zll add
			_startBoard.SetActiveEx (true);


			//2016-11-02 zll add
			_objSaizi = go.DeepFindEx(Layout.saizi).gameObject;
			canvasScaler = go.GetComponent<CanvasScaler>();
        }

		private void _OnCenterShow ()
		{
			EventTriggerListener.Get(_btnCraps.gameObject).onClick += _CrapsBtnClick;
			EventTriggerListener.Get(_btnStart.gameObject).onClick += _OnStartBtnClick;         
//			EventTriggerListener.Get(btn_tipBtn.gameObject).onClick+=_OnHideGameTipBoard;

			_tfCrapsDisplay.SetActiveEx(false);
			// ytf0926 
//			img_tipBoard.SetActiveEx(false);
			Audio.AudioManager.Instance.GameBgMusic ();
        }

		private void _OnCenterHide()
		{
			EventTriggerListener.Get(_btnCraps.gameObject).onClick -= _CrapsBtnClick;
			if (null != _btnStart)
			{
				EventTriggerListener.Get(_btnStart.gameObject).onClick -= _OnStartBtnClick;
			}
        
//			EventTriggerListener.Get(btn_tipBtn.gameObject).onClick-=_OnHideGameTipBoard;

			_canRoll=false;
			_gameStarted=false;
        }

		private void _OnHideGameTipBoard(GameObject go)
		{
//			if (null != img_tipBoard)
//			{
//				img_tipBoard.SetActiveEx (false);
//			}
		}

		/// <summary>
		/// 游戏提示小面板，暂时隐藏的
		/// </summary>
		/// <param name="value">Value.</param>
		public void OnShowGameTip(string value)
		{
			ShowRunTip (value);
//			return;
//			if (null != img_tipBoard) 
//			{
//				img_tipBoard.SetActiveEx (true);
//				lb_tiptxt.text = value;
//			}
		}

        private void _OnCenterDispose()
		{
			if (null != _imageDisplay) 
			{
				_imageDisplay.Dispose ();
			}
		}

		private void _CrapsBtnClick(GameObject go)
		{
			//2016-9-21  zll  fix
			//2016-9-28  zll  注释掉
//			if(_gameStarted == true)
//			{
//				_battleController.Send_StartGame();
//				_gameStarted = false;
//			}

			Console.WriteLine ("点击掷色子喽");

            if (_canRoll)
            {
				if (GameModel.GetInstance.isPlayNet == false)
				{
					Send_RequestRoll ();
				}
				else
				{
					NetWorkScript.getInstance ().RequestRollCraps (GameModel.GetInstance.myHandInfor.uuid, GameModel.GetInstance.curRoomId);
				}
			
				_canRoll = false;
				_isPlayerStand = false;
				Audio.AudioManager.Instance.BtnMusic ();

				_btnCraps.SetActiveEx (false);
            }
        }

		/// <summary>
		/// Hides the craps button.隐藏按钮
		/// </summary>
		public void HideCrapsBtn()
		{
			if (null != _btnCraps)
			{
				_btnCraps.SetActiveEx (false);
			}
		}

		public void Send_RequestRoll()
		{
            _battleController.Send_RequestRoll();
			_imageDisplay.SetActive (false);
            _tfCrapsDisplay.SetActiveEx(true);
            _animTransform.SetActiveEx (true);

			if(null != _currentPlayer)
			{
				if(_currentPlayer.isThreeRoll==true)
				{
					Console.WriteLine ("扔三个筛子");
					_ShowOtherCraps ();
				}
				else
				{
					_HideOtherCraps ();
				}
			}
		}

		private void _ShowOtherCraps()
		{

			if (null == _leftCrapsDisplay)
			{
				Console.WriteLine ("扔三个筛子");

				_leftCrapsDisplay = _tfCrapsDisplay.gameObject.CloneEx (false).transform;

				_leftAnimTransform = _leftCrapsDisplay.DeepFindEx (Layout.anim_crapsclone);
				var tmpImg = _leftCrapsDisplay.gameObject.GetComponentEx<Image> (Layout.image_iconclone);
				_leftImgDisplay = new UIImageDisplay (tmpImg);
				var tmpVec3 = _tfCrapsDisplay.transform.localRotation;
				var tmpRect = _tfCrapsDisplay.GetComponent<RectTransform> ().sizeDelta;
//				_leftCrapsDisplay.GetComponent<RectTransform> ().sizeDelta = tmpRect;
				_leftCrapsDisplay.SetParent (_tfCrapsDisplay.parent);
				_leftCrapsDisplay.transform.localPosition = new Vector3 (tmpVec3.x - tmpRect.x + 10, tmpVec3.y, tmpVec3.z);
				_leftCrapsDisplay.localScale =Vector3.one;

				_rightCrapsDisplay = _tfCrapsDisplay.gameObject.CloneEx (false).transform;
				_rightAnimTransform = _rightCrapsDisplay.DeepFindEx (Layout.anim_crapsclone);
				tmpImg = _rightCrapsDisplay.gameObject.GetComponentEx<Image> (Layout.image_iconclone);
				_rightImgDisplay = new UIImageDisplay (tmpImg);			
//				_rightCrapsDisplay.GetComponent<RectTransform> ().sizeDelta = tmpRect;
				_rightCrapsDisplay.SetParent (_tfCrapsDisplay.parent);
				_rightCrapsDisplay.transform.localPosition = new Vector3 (tmpVec3.x + tmpRect.x - 10, tmpVec3.y, tmpVec3.z);
				_rightCrapsDisplay.localScale = Vector3.one;

				_ShowTwoCraps ();
			}
			else
			{
				_ShowTwoCraps ();
			}
		}

		private void _ShowTwoCraps ()
		{
			if(null !=_leftCrapsDisplay)
			{
				_leftImgDisplay.SetActive (false);
				_leftCrapsDisplay.SetActiveEx (true);
				_leftAnimTransform.SetActiveEx (true);
			}

			if(null != _rightCrapsDisplay)
			{
				_rightImgDisplay.SetActive (false);
				_rightCrapsDisplay.SetActiveEx (true);
				_rightAnimTransform.SetActiveEx (true);
			}
		}

		private void _HideOtherCraps()
		{
			if(null !=_leftCrapsDisplay)
			{
				_leftImgDisplay.SetActive (false);
				_leftCrapsDisplay.SetActiveEx (false);
				_leftAnimTransform.SetActiveEx (false);
			}

			if(null != _rightCrapsDisplay)
			{
				_rightImgDisplay.SetActive (false);
				_rightCrapsDisplay.SetActiveEx (false);
				_rightAnimTransform.SetActiveEx (false);
			}
		}

		// ytf0929 掷一个筛子筛子
		public void Re_RequestRoll(int points)
		{
			_imageDisplay.Load (string.Format(_tempImagePath, points));
			_imageDisplay.SetActive (true);
			_animTransform.SetActiveEx (false);

			_ThrollCrapsMusic ();
		}

		// ytf0929 掷三个筛子
		public void Re_RequestRollS(int[] arrs)
		{
			_imageDisplay.Load (string.Format(_tempImagePath, arrs[1]));
			_imageDisplay.SetActive (true);
			_animTransform.SetActiveEx (false);

			if(null !=_leftCrapsDisplay)
			{
				_leftImgDisplay.Load(string.Format(_tempImagePath, arrs[0]));
				_leftImgDisplay.SetActive (true);
				_leftAnimTransform.SetActiveEx (false);
			}

			if(null != _rightCrapsDisplay)
			{
				_rightImgDisplay.Load(string.Format(_tempImagePath, arrs[2]));
				_rightImgDisplay.SetActive (true);
				_rightAnimTransform.SetActiveEx (false);
			}

			if(null != _currentPlayer)
			{
				_currentPlayer.isThreeRoll = false;
			}

			_ThrollCrapsMusic ();
		}

		private void _ThrollCrapsMusic()
		{
//			if (_currentPlayerIndex == 0)
//			{
//				Audio.AudioManager.Instance.DiceMusic();
//			}
		}

        private void _OnStartBtnClick(GameObject go)
        {
			Audio.AudioManager.Instance.BtnMusic ();
			if (GameModel.GetInstance.isPlayNet == false)
			{
				HideStartBtn ();
				StartGameAction ();
			}
			else
			{
				if (GameModel.GetInstance.isReconnecToGame == 0)
				{
					NetWorkScript.getInstance ().RequestInitlaiziGame (GameModel.GetInstance.myHandInfor.uuid, GameModel.GetInstance.curRoomId);
				}
				else
				{
					NetWorkScript.getInstance ().ReConnectInited ();

					HideStartBtn ();
					StartGameAction ();

					_btnCraps.SetActiveEx (false);
				}

			}
        }


		public void HideStartBtn()
		{
			if (null != _btnStart)
			{
				EventTriggerListener.Get(_btnStart.gameObject).onClick -= _OnStartBtnClick;
				_btnStart.SetActiveEx(false);
			}

			if (null != _startBoard)
			{
				_startBoard.SetActiveEx (false);

				if (null != _startBoard.gameObject)
				{
					GameObject.Destroy (_startBoard.gameObject);
				}
			}

			_startBoard=null;
		}

		public void StartGameAction()
		{
			_OnHideCountdown();

			if (!_gameStarted)
			{
				_battleController.Send_StartGame();
				_gameStarted = true;
				//				_startBoard.SetActiveEx (false);
				LocalDataManager.Instance.IsNormalEnded = false;
			}

			//			var condition = UIControllerManager.Instance.GetController<UIConditionWindowController> ();
			//			condition.showConditionType = 0;
			//			condition.setVisible (true);

			TimeManager.Instance.ContinueGame();
		}



		public void ConditionStartGame()
		{
			if (!_gameStarted)
			{
				_battleController.Send_StartGame();
				_gameStarted = true;
//				_startBoard.SetActiveEx (false);
				Audio.AudioManager.Instance.GameBgMusic ();
			}
		}

        public void ShowCraps(bool flag)
        {
			_tfCrapsDisplay.SetActiveEx(false);
            _tfBtnCraps.SetActiveEx(flag);
            _canRoll = flag;
			setSaiZiPos(Room.Instance.getCurrentPlay().getPlayPos());
			_HideOtherCraps ();
        }

		private void setSaiZiPos(Transform tra)
		{
			Vector3 pos = new Vector3 (WorldToUI(tra.position).x,WorldToUI(tra.position).y + 60,WorldToUI(tra.position).z);
			_objSaizi.transform.localPosition = pos;
		}

		private  Vector3 WorldToUI(Vector3 pos){  

			float resolutionX = canvasScaler.referenceResolution.x;  
			float resolutionY = canvasScaler.referenceResolution.y;  

			Vector3 viewportPos = Camera.main.WorldToViewportPoint(pos);  

			Vector3 uiPos = new Vector3(viewportPos.x * resolutionX - resolutionX * 0.5f,  
				viewportPos.y * resolutionY - resolutionY * 0.5f,0);  

			return uiPos;  
		}  

		private void _CenterEnterEnner()
		{
//			var vec3 = _btnStart.transform.localPosition;
//			_btnStart.transform.localPosition = new Vector3 (vec3.x,_pointBtnStartInnerY,vec3.z);
		}
			

		private string _tempImagePath = "share/atlas/battle/battlemain/dian_{0}.ab";//"share/atlas/battle/battlemain/dianshu_{0}.ab";
       
		private bool _canRoll;
		private bool _gameStarted;
        
		private Button _btnStart;
		private Button _btnCraps;
		private UIImageDisplay _imageDisplay;
		private Transform _animTransform;
        private Transform _tfCrapsDisplay;
        private Transform _tfBtnCraps;
      
        private BattleController _battleController = BattleController.Instance;

		private Transform _leftAnimTransform;
		private Transform _leftCrapsDisplay;
		private UIImageDisplay _leftImgDisplay;

		private Transform _rightAnimTransform;
		private Transform _rightCrapsDisplay;
		private UIImageDisplay _rightImgDisplay;

		private float _pointBtnStartInnerY = 35;

		// ytf 新加游戏提示框
//		private Image img_tipBoard;
//		private Text lb_tiptxt;
//		private Button btn_tipBtn;

		//2016-09-28
		private Image _startBoard;

		//2016-11-02
		private GameObject _objSaizi;
		private CanvasScaler  canvasScaler;
    }
}

