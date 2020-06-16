using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIOuterFateCardWindow
	{

		private void _OnitBottom (GameObject go)
		{
			_btnSure = go.GetComponentEx<Button> (Layout.btn_sure);
			_btnCancle = go.GetComponentEx<Button> (Layout.btn_cancle);
		}


		private void _OnShowBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick += _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick += _onCancleHandler;
//			if (!_playerManager.IsHostPlayerTurn())
//			{
//				var waitTime = MathUtility.Random(2, 5);
//				_timer = new Counter(waitTime);
//			}

			if(null != _controller)
			{
				if(_controller.IsEffectAll())
				{
					_SetSureBtnCenter ();
				}
				else
				{
					if(_controller.IsFateToSale()==false)
					{
						_ShowNoCondition ();
						_SetCancleBtnCenter ();
					}
				}
			}
		}

		private void _OnHideBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick -= _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick -= _onCancleHandler;
		}


		private void _onSureHandler(GameObject go)
		{
//			if (_playerManager.IsHostPlayerTurn())
//			{
				//TODO HostPlayer Behaviour
//				_controller.HandlerCardData ();

			Audio.AudioManager.Instance.BtnMusic ();

			if (_selfQuit == true) 
			{
				return;
			}

				if (_controller.IsFateToSale () == true)
				{
					_OnShowSaleBoard();
				}
				else
				{
				    _handleSuccess = true;
								
					_controller.HandlerCardData ();
				     _HideBgImg ();
					TweenTools.MoveAndScaleTo("outerfatecard/Content", "uibattle/top/financementor", _CloseHandler);
				}
//			}
		}

		private void _CloseHandler()
		{
			_controller.NetBuyCard ();
			_controller.setVisible(false);

			if (GameModel.GetInstance.isPlayNet == true)
			{
				MessageHint.Show ("其他玩家正在操作");
			}

			Client.Unit.BattleController.Instance.Send_RoleSelected (1);
			Particle.Instance.DestroyCardParticle();
		}

		private void _onCancleHandler(GameObject go)
		{
//			if (_playerManager.IsHostPlayerTurn())
//			{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_selfQuit == true) 
			{
				return;
			}
			_controller.NetQuitCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
			_handleSuccess = true;
			_controller.setVisible(false);
				_controller.QuitCard ();
//			}

			Particle.Instance.DestroyCardParticle();
		}


		private void _SetSureBtnCenter()
		{
			_btnCancle.SetActiveEx (false);
			var btnPosition = _btnSure.transform.localPosition;
			var tmpPoint=new Vector3(_pointXSureBtnCenter,btnPosition.y,btnPosition.z);
			_btnSure.transform.localPosition = tmpPoint;

			if(null != _controller)
			{
				if(_controller.cardData.handleRange==2)
				{
					_SetSureBtnContent (_btnSure);
				}
			}
		}

		private void _SetSureBtnContent(Button _button)
		{
			var img = _button.gameObject.GetComponentEx<Image> ("Image");

			_imgLoadBtn = new UIImageDisplay (img);

			_imgLoadBtn.Load (UIOtherCardWindowController.imgSurePath);
		}

		private void _SetCancleBtnCenter()
		{
			_btnSure.SetActiveEx (false);
			var btnPosition = _btnCancle.transform.localPosition;
//			Console.WriteLine (string.Format("new vector3=({0},{1},{2})",btnPosition.x.ToString(),btnPosition.y.ToString(),btnPosition.z.ToString()));
			var tmpPoint = new Vector3 (_pointXCancleBtnCenter,btnPosition.y,btnPosition.z);
			_btnCancle.transform.localPosition = tmpPoint;
		}


		private void _OnBottomTick(float deltaTime)
		{
			if (null != _timer && _timer.Increase(deltaTime))
			{
				_timer = null;
				//TODO NPC Behaviour
//				_controller.HandlerCardData ();
//				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
//				_controller.setVisible(false);
			}
		}

		private void _SelfHandler()
		{
			if (_controller.IsEffectAll() == true)
			{
				_controller.HandlerCardData ();
				_HideBgImg ();
				TweenTools.MoveAndScaleTo("outerfatecard/Content", "uibattle/top/financementor", _CloseHandler);
			}
			else
			{
				_controller.NetQuitCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
				if (GameModel.GetInstance.isPlayNet == true)
				{
					MessageHint.Show ("其他玩家正在操作");
				}
				_controller.setVisible(false);
				Particle.Instance.DestroyCardParticle();
			}
		}

		private void _OnDisposeBottom()
		{
			if(null != _imgLoadBtn)
			{
				_imgLoadBtn.Dispose ();
			}
		}

		private Button _btnSure;
		private Button _btnCancle;

		private float _pointXSureBtnCenter=0;
		private float _pointXCancleBtnCenter =0;

		private UIImageDisplay _imgLoadBtn;

		private Counter _timer;
//		private PlayerManager _playerManager = PlayerManager.Instance;

	}
}

