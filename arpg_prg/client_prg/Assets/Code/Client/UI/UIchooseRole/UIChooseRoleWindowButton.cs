using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using Core.Web;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIChooseRoleWindow 
	{
		private void _OnInitButton(GameObject go)
		{
			_btnStart = go.GetComponentEx<Button>(Layout.btn_start);
			_btnLook = go.GetComponentEx<Button>(Layout.btn_look);
			_btnSure = go.GetComponentEx<Button>(Layout.btn_sure);
			_btnBack = go.GetComponentEx<Button> (Layout.btn_back);

			_btnHeadOne = go.GetComponentEx<Button>(Layout.btn_HeadOne);
			_btnHeadTwo = go.GetComponentEx<Button>(Layout.btn_HeadTwo);
			_btnHeadThree = go.GetComponentEx<Button>(Layout.btn_HeadThree);
			_btnHeadFour = go.GetComponentEx<Button>(Layout.btn_HeadFour);

			_boardChuang = go.DeepFindEx(Layout.board_Chuang).gameObject;
			_boardBill = go.DeepFindEx(Layout.board_Bill).gameObject;

			_imgRole = go.GetComponentEx<RawImage>(Layout.img_role);

			_imgRoleNameBgOne = go.GetComponentEx<Image>(Layout.img_bgOne);
			_imgRoleNameBgTwo = go.GetComponentEx<Image>(Layout.img_bgTwo);
			_imgRoleNameBgThree = go.GetComponentEx<Image>(Layout.img_bgThree);
			_imgRoleNameBgFour = go.GetComponentEx<Image>(Layout.img_bgFour);

			_btnShou = go.GetComponentEx<Button>(Layout.btn_shou);

			_imgIcon = go.GetComponentEx<Image>(Layout.img_icon);

			_txtPrompt = go.GetComponentEx<Text>(Layout.txt_prompt);

			SetPalyAttribute(go);
		}

		private void _OnShowButton()
		{
			Audio.AudioManager.Instance.StartMusic ();

			EventTriggerListener.Get(_btnStart.gameObject).onClick += _OnBtnStartClick;
			EventTriggerListener.Get(_btnLook.gameObject).onClick  += _OnBtnLookClick;
			EventTriggerListener.Get(_btnSure.gameObject).onClick  += _OnBtnSureClick;

			EventTriggerListener.Get(_btnHeadOne.gameObject).onClick += _OnBtnHeadOneClick;
			EventTriggerListener.Get(_btnHeadTwo.gameObject).onClick += _OnBtnHeadTwoClick;
			EventTriggerListener.Get(_btnHeadThree.gameObject).onClick += _OnBtnHeadThreeClick;
			EventTriggerListener.Get(_btnHeadFour.gameObject).onClick += _OnBtnHeadFourClick; 

			EventTriggerListener.Get(_btnShou.gameObject).onClick += _OnBtnShouClick;

			EventTriggerListener.Get (_btnBack.gameObject).onClick += _OnClickBackHandler;

			roleIndex = 0;
			_playerData = _controller.SelectRole(roleIndex);
			_OnShowHeroInfor(_playerData);

            GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameChooseState;


//			Audio.AudioManager.Instance.Stop();

			//测试用的
//			if (null != _controller)
//			{
//				var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
//				controller.setVisible (true);
//				controller.LoadBattleUI();
//
//				var controller2 = Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>(); 
//				controller2.setVisible(true);
//
//				if (null != _controller) 
//				{
//					roleIndex = 1;
//					_playerData = _controller.SelectRole(roleIndex);
//					_OnShowHeroInfor(_playerData);
//				}
//
//				PlayerManager.Instance.SetPlayerHero(_playerData,roleIndex,_txtRoleName.text);
//
//			}
		}

		private void _OnHideButton()
		{
			EventTriggerListener.Get(_btnStart.gameObject).onClick -= _OnBtnStartClick;
			EventTriggerListener.Get(_btnLook.gameObject).onClick  -= _OnBtnLookClick;
			EventTriggerListener.Get(_btnSure.gameObject).onClick  -= _OnBtnSureClick;

			EventTriggerListener.Get(_btnHeadOne.gameObject).onClick -= _OnBtnHeadOneClick;
			EventTriggerListener.Get(_btnHeadTwo.gameObject).onClick -= _OnBtnHeadTwoClick;
			EventTriggerListener.Get(_btnHeadThree.gameObject).onClick -= _OnBtnHeadThreeClick;
			EventTriggerListener.Get(_btnHeadFour.gameObject).onClick -= _OnBtnHeadFourClick; 

			EventTriggerListener.Get(_btnShou.gameObject).onClick -= _OnBtnShouClick;
			EventTriggerListener.Get (_btnBack.gameObject).onClick -= _OnClickBackHandler;
		}

		private void _OnClickBackHandler(GameObject go)
		{
			_controller.setVisible (false);
			Game.Instance.SwithUIGameHall ();
		}

		private void _OnBtnStartClick(GameObject go)
		{
//			if(string.IsNullOrEmpty(_txtRoleName.text))
//			{
//				_txtPrompt.SetActiveEx(true);
//			}
//			else 
//			{
				Audio.AudioManager.Instance.BtnMusic ();
				Audio.AudioManager.Instance.Stop();
				if (null != _controller)
				{
					var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
					controller.setVisible (true);
					controller.LoadBattleUI();

					var controller2 = Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>(); 
					controller2.setVisible(true);
				}

				PlayerManager.Instance.SetPlayerHero(_playerData,roleIndex,GameModel.GetInstance.myHandInfor.nickName);
//			}

		}

		private void _OnBtnLookClick(GameObject go)
		{

			Tweener tweener = _boardBill.transform.DOLocalMove(Vector3.zero,1f);
			//设置这个Tween不受Time.scale影响
			tweener.SetUpdate(true);
			//设置移动类型
			tweener.SetEase(Ease.Linear);

			_btnLook.SetActiveEx(false);
		}
			
		private void _OnBtnShouClick(GameObject go)
		{
			Tweener tweener = _boardBill.transform.DOLocalMove(new Vector3(700,0,0),1f);
			//设置这个Tween不受Time.scale影响
			tweener.SetUpdate(true);
			//设置移动类型
			tweener.SetEase(Ease.Linear);

			_btnLook.SetActiveEx(true);
		}

		private void _OnBtnSureClick(GameObject go)
		{
			
		}

		private void _OnBtnHeadOneClick(GameObject go)
		{
			Console.WriteLine("_OnBtnHeadOneClick");

			SetHeadImageState(_btnHeadOne);
			SetUpRoleMask(_imgRoleNameBgOne);
			chooseRole(_objRoleOne);

			setUpIconImage(imgIconYiSheng_path);

			if (null != _controller) 
			{
				roleIndex = 0;
				_playerData = _controller.SelectRole(roleIndex);
				_OnShowHeroInfor(_playerData);
			}
		}

		private void _OnBtnHeadTwoClick(GameObject go)
		{
			Console.WriteLine("_OnBtnHeadTwoClick");

			SetHeadImageState(_btnHeadTwo);
			SetUpRoleMask(_imgRoleNameBgTwo);
			chooseRole(_objRoleTwo);
			setUpIconImage(imgIconLvShi_path);

			if (null != _controller) 
			{
				roleIndex = 1;
				_playerData = _controller.SelectRole(roleIndex);
				_OnShowHeroInfor(_playerData);
			}
		}

		private void _OnBtnHeadThreeClick(GameObject go)
		{
			SetHeadImageState(_btnHeadThree);
			SetUpRoleMask(_imgRoleNameBgThree);
			chooseRole(_objRoleThree);
			setUpIconImage(imgIconJiZhe_path);

			if (null != _controller) 
			{
				roleIndex = 4;
				_playerData = _controller.SelectRole(roleIndex);
				_OnShowHeroInfor(_playerData);
			}
		}

		private void _OnBtnHeadFourClick(GameObject go)
		{
			SetHeadImageState(_btnHeadFour);
			SetUpRoleMask(_imgRoleNameBgFour);
			chooseRole(_objRoleFour);
			setUpIconImage(imgIconBaoAn_path);

			if (null != _controller) 
			{
				roleIndex = 5;
				_playerData = _controller.SelectRole(roleIndex);
				_OnShowHeroInfor(_playerData);
			}

		}

		/// <summary>
		/// 设置头像的状态的
		/// </summary>
		/// <param name="btn">Button.</param>
		private void SetHeadImageState(Button btn)
		{
			WebManager.Instance.LoadWebItem (ImageHeiPath, item => {
				using (item)
				{
					_btnHeadOne.image.sprite = item.sprite;
					_btnHeadTwo.image.sprite = item.sprite;
					_btnHeadThree.image.sprite = item.sprite;
					_btnHeadFour.image.sprite = item.sprite;
				}
			});	

			WebManager.Instance.LoadWebItem (ImageCaiPath, item => {
				using (item)
				{
					btn.image.sprite = item.sprite;
				}
			});	
		}

		/// <summary>
		/// 设置详细页面的右上角icon图片
		/// </summary>
		/// <param name="imgPath">图片路径.</param>
		private void setUpIconImage(string imgPath)
		{
			WebManager.Instance.LoadWebItem (imgPath, item => {
				using (item)
				{
					_imgIcon.sprite = item.sprite;
				}
			});	
		}

		private void chooseRole(GameObject go)
		{
			_objRoleOne.SetActiveEx(false);
			_objRoleTwo.SetActiveEx(false);
			_objRoleThree.SetActiveEx(false);
			_objRoleFour.SetActiveEx(false);

			go.SetActiveEx(true);
		}

		/// <summary>
		/// 设置遮罩
		/// </summary>
		/// <param name="img">Image.</param>
		private void SetUpRoleMask(Image img)
		{
			_imgRoleNameBgOne.SetActiveEx(true);
			_imgRoleNameBgTwo.SetActiveEx(true);
			_imgRoleNameBgThree.SetActiveEx(true);
			_imgRoleNameBgFour.SetActiveEx(true);

			img.SetActiveEx(false);
		}

		private void addSprits(GameObject go)
		{
			go.AddComponent<RoleRotate>();
		}

		private void SetPalyAttribute(GameObject go)
		{
			_objRoleOne = go.DeepFindEx("Role01").gameObject;
			_objRoleTwo = go.DeepFindEx("Role02").gameObject;
			_objRoleThree = go.DeepFindEx("Role03").gameObject;
			_objRoleFour = go.DeepFindEx("Role04").gameObject;

			addSprits(_objRoleOne);
			addSprits(_objRoleTwo);
			addSprits(_objRoleThree);
			addSprits(_objRoleFour);
		}
			
		public void setSelectedImageButton()
		{
			_controller.IsSelectedIngame = true;
			_controller.setVisible (false);		
		}
			
		private PlayerInitData _playerData; 

		private Button _btnStart;
		private Button _btnSure;
		private Button _btnLook;

		private Button _btnBack;

		private Button _btnHeadOne;
		private Button _btnHeadTwo;
		private Button _btnHeadThree;
		private Button _btnHeadFour;

		private Button _btnShou;

		private Image _imgIcon;

		private RawImage _imgRole;
		private Image _imgRoleNameBgOne;
		private Image _imgRoleNameBgTwo;
		private Image _imgRoleNameBgThree;
		private Image _imgRoleNameBgFour;

		private GameObject _boardChuang;
		private GameObject _boardBill;

		private GameObject _objRoleOne;
		private GameObject _objRoleTwo;
		private GameObject _objRoleThree;
		private GameObject _objRoleFour;

		private Text _txtPrompt;

		private string ImageHeiPath = "share/atlas/battle/chooserole/heikuang.ab";
		private string ImageCaiPath = "share/atlas/battle/chooserole/caikuang.ab";

		private string imgIconBaoAn_path = "share/atlas/battle/chooserole/baoan_icon.ab";
		private string imgIconLvShi_path = "share/atlas/battle/chooserole/lvshi_icon.ab";
		private string imgIconYiSheng_path = "share/atlas/battle/chooserole/yisheng_icon.ab";
		private string imgIconJiZhe_path = "share/atlas/battle/chooserole/siji.ab";

		private int roleIndex = 0;
	}
}