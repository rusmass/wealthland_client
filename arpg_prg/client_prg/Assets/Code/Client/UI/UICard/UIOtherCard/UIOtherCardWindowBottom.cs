using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIOtherCardWindow
	{
		private void _OnitBottom (GameObject go)
		{
			_btnSure = go.GetComponentEx<Button> (Layout.btn_sure);
			_btnCancle = go.GetComponentEx<Button> (Layout.btn_cancle);
			_btnBorrow = go.GetComponentEx<Button> (Layout.btn_borrow);
		}


		private void _OnShowBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick += _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick += _onCancleHandler;
			if (!_playerManager.IsHostPlayerTurn())
			{
				var waitTime = MathUtility.Random(2, 5);
				_timer = new Counter(waitTime);
			}

			_btnBorrow.SetActiveEx (false);

			if ((int)SpecialCardType.GiveChildType == _controller.cardID ||(int)SpecialCardType.CheckDayType == _controller.cardID||(int)SpecialCardType.InnerCheckDayType == _controller.cardID)
			{
				_btnCancle.SetActiveEx (false);
				_btnSure.transform.localPosition = new Vector3(0,0,0);
				_isMustSure = true;

				if((int)SpecialCardType.GiveChildType == _controller.cardID)
				{
					_isGiveChild = true;
				}
				_SetSureBtnContent (_btnSure);
			}
			else
			{
				if (_controller.EnoughMoney () == false)
				{
					_btnSure.SetActiveEx (false);
					_btnCancle.transform.localPosition = Vector3.zero;
				}				
			}
		}

		private void _SetSureBtnContent(Button _button)
		{
			var img = _button.gameObject.GetComponentEx<Image> ("Image");

			_imgLoad = new UIImageDisplay (img);

			_imgLoad.Load (UIOtherCardWindowController.imgSurePath);

		}

		private void _OnHideBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick -= _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick -= _onCancleHandler;
		}


		private void _onSureHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_selfQuit == true) 
			{
				return;
			}
			if (_playerManager.IsHostPlayerTurn())
			{
				//TODO HostPlayer Behaviour
				_handleSuccess=true;

				if (_isGiveChild == true )
				{
					if(GameModel.GetInstance.isPlayNet==false)
					{
						if(_controller.player.childNum<_controller.player.childNumMax)
						{
							var redController = Client.UIControllerManager.Instance.GetController<UIRedPacketWindowController>();
							redController.player =_controller.player;
							redController.OpenGetRedPacket();	
							redController.setVisible(true);		
							_controller.HandlerCardData ();
						}
						else
						{						
							_controller.HandlerCardData ();
//							_controller.NetBuyCard ();
							Client.Unit.BattleController.Instance.Send_RoleSelected (1);
						}		
					}
					else
					{
						_controller.HandlerCardData ();
						NetWorkScript.getInstance ().SureGiveChildCard (Protocol.Game_BuyGiveChildCard, GameModel.GetInstance.curRoomId, _controller.cardID, (int)SpecialCardType.GiveChildType, 1, GameModel.GetInstance.isGiveChild);
					}
				}
			}
			_HideBgImg ();
			TweenTools.MoveAndScaleTo("uiothercard/Content", "uibattle/top/financementor", _CloseHandler);

		}

		private void _SelfCloseHandler()
		{
			_controller.setVisible(false);
			if(_isGiveChild==false)
			{
				_controller.NetBuyCard ();
				_controller.HandlerCardData ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
			}
			else
			{
				if (GameModel.GetInstance.isPlayNet == false)
				{
					if (_controller.player.childNum < _controller.player.childNumMax)
					{
						var redController = Client.UIControllerManager.Instance.GetController<UIRedPacketWindowController>();
						redController.player =_controller.player;			
						redController.OpenGetRedPacket();
						redController.setVisible(true);
						_controller.HandlerCardData ();
					}
					else
					{
//						_controller.NetBuyCard ();
						_controller.HandlerCardData ();
						Client.Unit.BattleController.Instance.Send_RoleSelected (1);
					}
				} 
				else
				{
					_controller.HandlerCardData ();
					NetWorkScript.getInstance ().SureGiveChildCard (Protocol.Game_BuyGiveChildCard, GameModel.GetInstance.curRoomId, _controller.cardID, (int)SpecialCardType.GiveChildType, 1, GameModel.GetInstance.isGiveChild);
				}
			}
		}

		private void _CloseHandler()
		{			
			_controller.setVisible(false);
			if(_isGiveChild==false)
			{
				_controller.HandlerCardData ();
				_controller.NetBuyCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
			}		
		}

		private void _onCancleHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
		
			if (_selfQuit == true) 
			{
				return;
			}

			if (_playerManager.IsHostPlayerTurn())
			{
				_handleSuccess = true;
				_controller.NetQuitCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
				_controller.setVisible(false);
			}
		}

		private void _SelfHandler()
		{
			if (_isMustSure==true)
			{
				if (_playerManager.IsHostPlayerTurn())
				{					
					_HideBgImg ();
					TweenTools.MoveAndScaleTo("uiothercard/Content", "uibattle/top/financementor", _SelfCloseHandler);
				}
			}
			else
			{
				_controller.NetQuitCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
				_controller.setVisible(false);
			}

		}

		private void _OnDisposeBottom()
		{
			if(null != _imgLoad)
			{
				_imgLoad.Dispose ();
			}
		}


		private void _OnBottomTick(float deltaTime)
		{
			if (null != _timer && _timer.Increase(deltaTime))
			{
				_timer = null;
				//TODO NPC Behaviour
				_controller.HandlerCardData ();
				_controller.NetBuyCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
				_controller.setVisible(false);
			}
		}

		private Button _btnSure;
		private Button _btnCancle;
		private Button _btnBorrow;

		private bool _isMustSure=false;
		private bool _isGiveChild=false;

		private UIImageDisplay _imgLoad;

		private Counter _timer;
		private PlayerManager _playerManager = PlayerManager.Instance;
	}
}

