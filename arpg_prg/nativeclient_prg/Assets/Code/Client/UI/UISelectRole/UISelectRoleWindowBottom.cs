using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
namespace Client.UI
{
	public partial class UISelectRoleWindow
	{
		private void _OnInitBottom(GameObject go)
		{
			_btnSelect = go.GetComponentEx<Button> (Layout.btn_selectrole);
			_btnInGame = go.GetComponentEx<Button> (Layout.btn_clickingame);
		}

		private void _OnShowBottom()
		{
			Audio.AudioManager.Instance.StartMusic ();
			_btnInGame.SetActiveEx (false);
			EventTriggerListener.Get (_btnSelect.gameObject).onClick += _OnSelectRoleHandler;
			EventTriggerListener.Get (_btnInGame.gameObject).onClick += _OnIntoGameHandler;
		}

		private void _OnHideBottom()
		{
			
		}

		private void _OnSelectRoleHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			Audio.AudioManager.Instance.SelectedRoleMusic ();
			Console.WriteLine ("onselect role");
			_btnSelect.SetActiveEx (false);
			_rotateImg.SetActive (false);
			_ShowAnimatorRotation ();

			_careerIndex = MathUtility.Random (1,RangeNum);
			_timer = new Counter (_waitTime);
			//_OnSelectedRole ();


		}

		private void _OnSelectedRole()
		{
			// 有选择结果之后
			_btnInGame.SetActiveEx (true);

			animtor_rotate.SetActiveEx (false);


			if (null != _controller) 
			{
				_playerData= _controller.SelectRole (_careerIndex-1);
				_OnLoadHeroImg (_playerData.playerImgPath);

				_OnShowHeroInfor (_playerData);
			}

		
			_OnLoadRoteImg (string.Format(loadImgPath,_imgArrs[_careerIndex-1]));						
		}

		private void _OnIntoGameHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			Audio.AudioManager.Instance.Stop();
			if (null != _controller)
			{
				// 2016-9-21 zll fix
				var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
				controller.setVisible (true);
				controller.LoadBattleUI();
			}
		}

		// 2016-9-21 zll fix
		public void setSelectedImageButton()
		{
			_controller.IsSelectedIngame = true;
			_controller.setVisible (false);
//			PlayerManager.Instance.SetPlayerHero(_playerData,_careerIndex-1);
		}

		private void _BottomTick(float deltaTime)
		{
			if (null != _timer &&  _timer.Increase (deltaTime))
			{
				_timer = null;
				_OnSelectedRole ();

			}
		}

		private Counter _timer;
		private float _waitTime=2f;

		private PlayerInitData _playerData;
		private int _careerIndex;

		private int RangeNum=4;


		private Button _btnSelect;
		private Button _btnInGame;

	}
}

