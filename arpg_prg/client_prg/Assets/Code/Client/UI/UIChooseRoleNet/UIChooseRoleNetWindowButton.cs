using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Metadata;
using Core.Web;
using DG.Tweening;
namespace Client.UI
{
	public partial class UIChooseRoleNetWindow
	{
		private void _OnInitButton(GameObject go)
		{
			_btnStart = go.GetComponentEx<Button>(Layout.btn_start);

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

			_imgRole1 = go.GetComponentEx<RawImage> (Layout.btn_HeadOne+"/RawImage");
			_imgRole2 = go.GetComponentEx<RawImage> (Layout.btn_HeadTwo+"/RawImage");
			_imgRole3 = go.GetComponentEx<RawImage> (Layout.btn_HeadThree+"/RawImage");
			_imgRole4 = go.GetComponentEx<RawImage> (Layout.btn_HeadFour+"/RawImage");

			_txtPrompt = go.GetComponentEx<Text>(Layout.txt_prompt);

			for (var i = 0; i < 4; i++)
			{
				var rawimg = go.GetComponentEx<RawImage> ("rawimg"+(i+1).ToString());

				rawimg.SetActiveEx(false);
				_rawImgArr [i] = rawimg;
			}
			_rightRoleNameList.Clear ();
			for (var i = 0; i < 4; i++)
			{
				var tmpTxt = go.GetComponentEx<Text> ("img_player"+(i+1).ToString()+"/lb_selectname");
				_rightRoleNameList.Add (tmpTxt);
			}

			_readyImgList=new List<Image>();
			for (var i = 0; i < 4; i++)
			{
				var tmpimg = go.GetComponentEx<Image> ("img_player"+(i+1).ToString()+"/readyIcon");
				_readyImgList.Add (tmpimg);
			}


			img_tipboardupOne = go.GetComponentEx<Image> (Layout.img_tipboardupone);
			img_tipboardupTwo = go.GetComponentEx<Image> (Layout.img_tipboarduptwo);

			lb_defaultInduct = go.GetComponentEx<Text> (Layout.lb_defaultIntroduc);
			img_defaultRole=go.GetComponentEx<Image>(Layout.img_defaultRole);

			SetPalyAttribute(go);
		}

		private void _OnShowButton()
		{
			GameModel.GetInstance.HideNetLoading ();

			Audio.AudioManager.Instance.StartMusic ();
			EventTriggerListener.Get(_btnStart.gameObject).onClick += _OnBtnStartClick;
			EventTriggerListener.Get(_btnHeadOne.gameObject).onClick += _OnBtnHeadOneClick;
			EventTriggerListener.Get(_btnHeadTwo.gameObject).onClick += _OnBtnHeadTwoClick;
			EventTriggerListener.Get(_btnHeadThree.gameObject).onClick += _OnBtnHeadThreeClick;
			EventTriggerListener.Get(_btnHeadFour.gameObject).onClick += _OnBtnHeadFourClick; 

			_initPlayerInitDatas = _controller.GetInitData ();

			for (var i = 0; i < _selectedState.Length; i++)
			{
				_selectedState [i] = 0;
			}

			_boardBill.SetActiveEx (false);
//			_boardChuang.SetActiveEx (false);

			_chooseVoList = _controller.GetRigthPlayerInfors ();

			for (var i = 0; i < _chooseVoList.Count; i++)
			{
				var tmpVo = _chooseVoList [i];
				_rightRoleNameList [i].text = tmpVo.nickName;
			}

			GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNetChooseState;

			chooseRole (null);
		}

		/// <summary>
		/// Inits the data.游戏初始化时的显示
		/// </summary>
		private void initData()
		{
			img_tipboardupOne.SetActiveEx (false);
			img_tipboardupTwo.SetActiveEx (false);

			lb_defaultInduct.SetActiveEx (true);
			img_defaultRole.SetActiveEx (true);			
		}

		/// <summary>
		/// Chooses the data. 选择角色后的显示
		/// </summary>
		private void chooseData()
		{
			img_tipboardupOne.SetActiveEx (true);
			img_tipboardupTwo.SetActiveEx (true);

			lb_defaultInduct.SetActiveEx (false);
			img_defaultRole.SetActiveEx (false);		
		}

		private void _OnHideButton()
		{
			EventTriggerListener.Get(_btnStart.gameObject).onClick -= _OnBtnStartClick;		
			EventTriggerListener.Get(_btnHeadOne.gameObject).onClick -= _OnBtnHeadOneClick;
			EventTriggerListener.Get(_btnHeadTwo.gameObject).onClick -= _OnBtnHeadTwoClick;
			EventTriggerListener.Get(_btnHeadThree.gameObject).onClick -= _OnBtnHeadThreeClick;
			EventTriggerListener.Get(_btnHeadFour.gameObject).onClick -= _OnBtnHeadFourClick; 
		}

		private void _OnBtnStartClick(GameObject go)
		{
//			_controller.setVisible (false);
//			return;

			if (_tmpSelectIndex < 0)
			{
				MessageHint.Show ("请选选择角色");
				return;
			}

			if (isReady == false)
			{
				isReady = true;

				_btnStart.GetComponent<Image>().color=_grayColor;
				NetWorkScript.getInstance ().NetSureSelect ();
			}
		}
	

		private void _OnBtnHeadOneClick(GameObject go)
		{
			Console.WriteLine("_OnBtnHeadOneClick");

			if (isReady == true)
			{
				return;
			}

			if (isTimeSelect == false)
			{
				return;
			}

			isTimeSelect = false;

			var tmpIndex = 0;

			if (_selectedState [tmpIndex] == 0)
			{
				//选择
				NetWorkScript.getInstance().NetSelectRole(_controller.SelectRole(tmpIndex).id);
			}
			else if(_selectedState [tmpIndex] == 1)
			{
				//不可选
			}
			else if(_selectedState [tmpIndex] == 2)
			{
				//放弃选择
				NetWorkScript.getInstance().NetCancleSelectRole(_controller.SelectRole(tmpIndex).id);
			}
		}

		private void _OnBtnHeadTwoClick(GameObject go)
		{
			Console.WriteLine("_OnBtnHeadTwoClick");

			if (isReady == true)
			{
				return;
			}

			if (isTimeSelect == false)
			{
				return;
			}

			isTimeSelect = false;

			var tmpIndex = 1;

			if (_selectedState [tmpIndex] == 0)
			{
				//选择
				NetWorkScript.getInstance().NetSelectRole(_controller.SelectRole(tmpIndex).id);
			}
			else if(_selectedState [tmpIndex] == 1)
			{
				//不可选
			}
			else if(_selectedState [tmpIndex] == 2)
			{
				//放弃选择
				NetWorkScript.getInstance().NetCancleSelectRole(_controller.SelectRole(tmpIndex).id);
			}
		}

		private void _OnBtnHeadThreeClick(GameObject go)
		{			

			if (isReady == true)
			{
				return;
			}

			if (isTimeSelect == false)
			{
				return;
			}

			isTimeSelect = false;

			var tmpIndex = 2;

			if (_selectedState [tmpIndex] == 0)
			{
				//选择
				NetWorkScript.getInstance().NetSelectRole(_controller.SelectRole(tmpIndex).id);
			}
			else if(_selectedState [tmpIndex] == 1)
			{
				//不可选
			}
			else if(_selectedState [tmpIndex] == 2)
			{
				//放弃选择
				NetWorkScript.getInstance().NetCancleSelectRole(_controller.SelectRole(tmpIndex).id);
			}
		}

		private void _OnBtnHeadFourClick(GameObject go)
		{
			if (isReady == true)
			{
				return;
			}

			if (isTimeSelect == false)
			{
				return;
			}

			isTimeSelect = false;

			var tmpIndex = 3;

			if (_selectedState [tmpIndex] == 0)
			{
				//选择
				NetWorkScript.getInstance().NetSelectRole(_controller.SelectRole(tmpIndex).id);
			}
			else if(_selectedState [tmpIndex] == 1)
			{
				//不可选
			}
			else if(_selectedState [tmpIndex] == 2)
			{
				//放弃选择
				NetWorkScript.getInstance().NetCancleSelectRole(_controller.SelectRole(tmpIndex).id);
			}
		}


		public void SetReadyImg(string value)
		{
			for (var i = 0; i < _chooseVoList.Count; i++)
			{
				var tmpVo = _chooseVoList [i];
				if (tmpVo.playerId == value)
				{
					_readyImgList [i].SetActiveEx (true);
					break;
				}
			}
		}


		/// <summary>
		/// Nets the select infor.选择某个头像，自己选择和其他人选择
		/// </summary>
		/// <param name="value">Value.</param>
		public void NetSelectInfor(NetChooseRoleInfor value)
		{
			var initIndex = 0;

			for (var i = 0; i < _initPlayerInitDatas.Count; i++)
			{
				if (value.careerId == _initPlayerInitDatas [i].id)
				{
					initIndex = i;
					break;
				}
			}

			for (var i = 0; i < _chooseVoList.Count; i++)
			{
				var tmpvo = _chooseVoList [i];
				if (tmpvo.playerId == value.playerId)
				{
					for (var j = 0; j < _initPlayerInitDatas.Count; j++)
					{
						var tmpInitData = _initPlayerInitDatas [j];
						if (tmpvo.careerId == tmpInitData.id)
						{
							_selectedState [j] = 0;
							break;
						}
					}
					tmpvo.careerId = value.careerId;
					break;
				}
			}

			Image _tmpRoleNameBg=null;
			RawImage _tmpImgRole=null;
			GameObject tmpObjRole = null;

			if (initIndex == 0)
			{
				_tmpRoleNameBg = _imgRoleNameBgOne;
				_tmpImgRole = _imgRole1;
				tmpObjRole = _objRoleOne;
			}
			else if(initIndex==1)
			{
				_tmpRoleNameBg = _imgRoleNameBgTwo;
				_tmpImgRole = _imgRole2;
				tmpObjRole = _objRoleTwo;
			}
			else if(initIndex==2)
			{
				_tmpRoleNameBg = _imgRoleNameBgThree;
				_tmpImgRole = _imgRole3;
				tmpObjRole = _objRoleThree;
			}
			else if(initIndex==3)
			{
				_tmpRoleNameBg = _imgRoleNameBgFour;
				_tmpImgRole = _imgRole4;
				tmpObjRole = _objRoleFour;
			}			
			
			if (value.playerId == GameModel.GetInstance.myHandInfor.uuid)
			{
				if (_tmpSelectIndex >= 0)
				{
					_selectedState [_tmpSelectIndex] = 0;
				}
				if (null != _imgTmpSelectRole)
				{
					_SetHeadImgBright (_imgTmpSelectRole);
				}

				SetUpRoleMask(_tmpRoleNameBg);
				chooseRole(tmpObjRole);
				_selectedState [initIndex] = 2;

				_tmpSelectIndex = initIndex;
				_imgTmpSelectRole = _tmpImgRole;

				_boardBill.SetActiveEx (true);
				_boardChuang.SetActiveEx (true);

				if (null != _controller) 
				{
					_playerData = _controller.SelectRole(initIndex);
					_OnShowHeroInfor(_playerData);
				}
			}	
			else
			{
				_selectedState [initIndex] = 1;
			}
			_SetHeadImgGray (_tmpImgRole);

			if (initIndex >= 0)
			{
				var tmpPlayerdata = _controller.SelectRole(initIndex);
				for (var i = 0; i < _chooseVoList.Count; i++)
				{
					var tmpVo = _chooseVoList [i];
					if (tmpPlayerdata.id == tmpVo.careerId)
					{
						_rawImgArr [i].SetActiveEx (true);
						loadRightHeadImg (_rawImgArr [i], tmpPlayerdata.headPath);
						break;
					}
				}
			}		
		}

		/// <summary>
		/// Nets the cancle infor. 放弃头像，自己放弃或者其他人放弃
		/// </summary>
		/// <param name="value">Value.</param>
		public void NetCancleInfor(NetChooseRoleInfor value)
		{
			var initIndex = -1;

			for (var i = 0; i < _initPlayerInitDatas.Count; i++)
			{
				if (value.careerId == _initPlayerInitDatas [i].id)
				{
					initIndex = i;
					break;
				}
			}

			for (var i = 0; i < _chooseVoList.Count; i++)
			{
				var tmpvo = _chooseVoList [i];
				if (tmpvo.playerId == value.playerId)
				{
					tmpvo.careerId = 0;
				}
			}

			Image _tmpRoleNameBg=null;
			RawImage _tmpImgRole=null;
			GameObject tmpObjRole = null;

			if (initIndex == 0)
			{
				_tmpRoleNameBg = _imgRoleNameBgOne;
				_tmpImgRole = _imgRole1;
				tmpObjRole = _objRoleOne;

			}
			else if(initIndex==1)
			{
				_tmpRoleNameBg = _imgRoleNameBgTwo;
				_tmpImgRole = _imgRole2;
				tmpObjRole = _objRoleTwo;
			}
			else if(initIndex==2)
			{
				_tmpRoleNameBg = _imgRoleNameBgThree;
				_tmpImgRole = _imgRole3;
				tmpObjRole = _objRoleThree;
			}
			else if(initIndex==3)
			{
				_tmpRoleNameBg = _imgRoleNameBgFour;
				_tmpImgRole = _imgRole4;
				tmpObjRole = _objRoleFour;
			}

			if (value.playerId == GameModel.GetInstance.myHandInfor.uuid)
			{				
				SetUpRoleMask(null);
				chooseRole(null);

				_tmpSelectIndex = -1;
				_imgTmpSelectRole = null;

				_boardBill.SetActiveEx (false);
//				_boardChuang.SetActiveEx (false);
			}	

			_selectedState [initIndex] = 0;
			_SetHeadImgBright (_tmpImgRole);

			if (initIndex >= 0)
			{
//				_rawImgArr [initIndex].SetActiveEx (false);
				var tmpPlayerdata = _controller.SelectRole(initIndex);
				for (var i = 0; i < _chooseVoList.Count; i++)
				{
					var tmpVo = _chooseVoList [i];
					if (tmpVo.playerId==value.playerId)
					{
						_rawImgArr [i].SetActiveEx (false);
						break;
					}
				}
			}
		}	


		private void loadRightHeadImg(RawImage value ,string path)
		{
			WebManager.Instance.LoadWebItem (path, item => {
				using (item)
				{
					value.texture = item.texture;
				}
			});	
		}

		private void chooseRole(GameObject go)
		{
			_objRoleOne.SetActiveEx(false);
			_objRoleTwo.SetActiveEx(false);
			_objRoleThree.SetActiveEx(false);
			_objRoleFour.SetActiveEx(false);
			if (null != go)
			{
				go.SetActiveEx(true);
				chooseData ();
			}
			else
			{
				initData ();

			}
		}

		/// <summary>
		/// 设置遮罩
		/// </summary>
		/// <param name="img">Image.</param>
		private void SetUpRoleMask(Image img)
		{
			_imgRoleNameBgOne.SetActiveEx(false);
			_imgRoleNameBgTwo.SetActiveEx(false);
			_imgRoleNameBgThree.SetActiveEx(false);
			_imgRoleNameBgFour.SetActiveEx(false);

			if (null != img)
			{
				img.SetActiveEx(true);
			}

		}

		private void _HideRoleMask()
		{
			_imgRoleNameBgOne.SetActiveEx(false);
			_imgRoleNameBgTwo.SetActiveEx(false);
			_imgRoleNameBgThree.SetActiveEx(false);
			_imgRoleNameBgFour.SetActiveEx(false);
		}

		private void _SetHeadImgGray(RawImage value)
		{
			if (_selectedState [0] == 0)
			{
				_imgRole1.color = _brightColor;
			}
			else
			{
				_imgRole1.color = _grayColor;
			}

			if (_selectedState [1] == 0)
			{
				_imgRole2.color = _brightColor;
			}
			else
			{
				_imgRole2.color = _grayColor;
			}


			if (_selectedState [2] == 0)
			{
				_imgRole3.color = _brightColor;
			}
			else
			{
				_imgRole3.color = _grayColor;
			}


			if (_selectedState [3] == 0)
			{
				_imgRole4.color = _brightColor;
			}
			else
			{
				_imgRole4.color = _grayColor;
			}

		}

		private void _SetHeadImgBright(RawImage value)
		{
			if (_selectedState [0] == 0)
			{
				_imgRole1.color = _brightColor;
			}
			else
			{
				_imgRole1.color = _grayColor;
			}

			if (_selectedState [1] == 0)
			{
				_imgRole2.color = _brightColor;
			}
			else
			{
				_imgRole2.color = _grayColor;
			}


			if (_selectedState [2] == 0)
			{
				_imgRole3.color = _brightColor;
			}
			else
			{
				_imgRole3.color = _grayColor;
			}


			if (_selectedState [3] == 0)
			{
				_imgRole4.color = _brightColor;
			}
			else
			{
				_imgRole4.color = _grayColor;
			}
		}

		private void _HideSelfSelectHead()
		{
			
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

		private PlayerInitData _playerData; 
//
		private Button _btnStart;

		private Button _btnHeadOne;
		private Button _btnHeadTwo;
		private Button _btnHeadThree;
		private Button _btnHeadFour;

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

		/// <summary>
		/// The image tmp select role.临时选择的角色
		/// </summary>
		private RawImage _imgTmpSelectRole=null;
		/// <summary>
		/// The index of the tmp select. 自己选择的角色
		/// </summary>
		private int _tmpSelectIndex = -1;


		private bool isReady=false;


		//人物头像头像信息
		private RawImage _imgRole1;
		private RawImage _imgRole2;
		private RawImage _imgRole3;
		private RawImage _imgRole4;

		private RawImage[] _rawImgArr=new RawImage[4];
		private List<NetChooseRoleInfor> _chooseVoList=new List<NetChooseRoleInfor>();
		private List<Text> _rightRoleNameList=new List<Text>();
		private List<Image> _readyImgList = new List<Image> ();

		/// <summary>
		/// The state of the selected. 选择的状态 0 是未选择，1其他人选择, 2自己选择
		/// </summary>
		private int[] _selectedState={0,0,0,0};
		private List<PlayerInitData> _initPlayerInitDatas;

		private Image img_tipboardupOne;
		private Image img_tipboardupTwo;

		private Text lb_defaultInduct;
		private Image img_defaultRole;


		private Text _txtPrompt;


		private string imgIconBaoAn_path = "share/atlas/battle/chooserole/baoan_icon.ab";
		private string imgIconLvShi_path = "share/atlas/battle/chooserole/lvshi_icon.ab";
		private string imgIconYiSheng_path = "share/atlas/battle/chooserole/yisheng_icon.ab";
		private string imgIconJSiji_path = "share/atlas/battle/chooserole/siji.ab";

		private Color _brightColor = new Color (255f/255,255f/255,255f/255,1f);
		private Color _grayColor = new Color(85f/255,85f/255,85f/255,1f);

		private int roleIndex = 0;
	}
}

