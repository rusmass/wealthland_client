using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
using DG.Tweening;
namespace Client.UI
{
	public partial class UIRedPacketWindow
    {
        #region Const
        private const string packPacketPrompt = "朋友XXX喜得贵子!\t快发个红包祝贺一下吧.";
        private const int MaxCashCount = 10000;
        private const int MinCashCount = 1;
        private const int MaxPacketCount = 3;
        private const int WaitTime = 30;
        #endregion
        #region Componment
        private Transform _getpacketpanel;
        private Transform _sendpacketpanel;
        private Transform _packpacketpanel;

        private Transform _redpacketPrefab;

        private Button _btnsend;
        private Button _btnpack;
		private Button _btncancel;

        private Text _lbtimerecord;
        private Text _lbpackprompt;

        private InputField _inputcash;

		private Transform _transformClock;
        #endregion
        #region Normal
        private Vector3[] pos = { new Vector3(-255, 19, 0), new Vector3(2, 19, 0), new Vector3(262, 19, 0) };

        private UIRPGetPacketInfo[] getPacketInfos = new UIRPGetPacketInfo[MaxPacketCount];
        private Counter _timer;
        
        private int getPacketCount = 0;

		private int packetType=0;
        #endregion

        //TODO
        private Counter _tempTimer;


        private void _OnInitCenter(GameObject go)
		{
            _getpacketpanel = go.GetComponentEx<Transform>(Layout.getpacketpanel);
            _sendpacketpanel = go.GetComponentEx<Transform>(Layout.sendpacketpanel);
            _packpacketpanel = go.GetComponentEx<Transform>(Layout.packpacketpanel);

			_transformClock = go.GetComponentEx<Transform>(Layout.item_clock);
			_lbtimerecord = go.GetComponentEx<Text>(Layout.lb_timerecord);

            InitGetPacketPanel(_getpacketpanel.gameObject);
            InitSendPacketPanel(_sendpacketpanel.gameObject);
            InitPackPacketPanel(_packpacketpanel.gameObject);

        }
        private void InitGetPacketPanel(GameObject go)
        {
          
            _redpacketPrefab = go.GetComponentEx<Transform>(Layout.obj_redpacket);
            
            for (int i = 0; i < getPacketInfos.Length; i++)
            {
                getPacketInfos[i] = new UIRPGetPacketInfo();
                PlayerInfo playerInfo = PlayerManager.Instance.Players[i + 1];
                if (i == 0)
                    getPacketInfos[i].InitInfo(_redpacketPrefab.gameObject,playerInfo, _OnGetPacketEndHandler);
                else
                {
                    GameObject clone = (GameObject)_redpacketPrefab.gameObject.InstantiateEx();
                    clone.name = _redpacketPrefab.name;
                    clone.transform.SetParent(go.transform);
                    clone.transform.localPosition = pos[i];
                    clone.transform.localScale = Vector3.one;
                    getPacketInfos[i].InitInfo(clone, playerInfo,_OnGetPacketEndHandler);
                }
            }
        }
        private void InitSendPacketPanel(GameObject go)
        {
            _inputcash = go.GetComponentEx<InputField>(Layout.input_cash);

            _btnsend = go.GetComponentEx<Button>(Layout.btn_send);
            EventTriggerListener.Get(_btnsend.gameObject).onClick = _OnSendPacketHandler;
        }
        private void InitPackPacketPanel(GameObject go)
        {
            _lbpackprompt = go.GetComponentEx<Text>(Layout.lb_packprompt);

            _btnpack = go.GetComponentEx<Button>(Layout.btn_pack);

			_btncancel = go.GetComponentEx<Button> (Layout.btn_cancle);

            EventTriggerListener.Get(_btnpack.gameObject).onClick = _OnPackPacketHandler;
			EventTriggerListener.Get (_btncancel.gameObject).onClick = _OnLeaveSendPacket;
        }
        private void RefreshGetPacketPanel()
        {
            getPacketCount = 0;
			_timer = new Counter(WaitTime);
        }
        private void RefreshSendPacketPanel()
        {
			
        }
        private void RefreshPackPacketPanel()
        {
            SetTextContext(_lbpackprompt, packPacketPrompt.Replace("XXX", _controller.player.playerName));
			_timer = new Counter(WaitTime);
			_transformClock.localPosition = new Vector3 (-95,172,0);
        }

		/// <summary>
		/// 领取红包的回调
		/// </summary>
        private void _OnGetPacketEndHandler()
        {
            getPacketCount++;
            if (getPacketCount >= MaxPacketCount)
            {
                getPacketCount = 0;
                MessageHint.Show("全部已领取");

//				CardManager.Instance.NetBuyCard (Protocol.Game_BuyGiveChildCard , (int)SpecialCardType.GiveChildType, (int)SpecialCardType.GiveChildType, 1);
				if (GameModel.GetInstance.isPlayNet == true)
				{
					NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
				}

                //TODO临时使用代码，模拟等待飞金币，此处可删
                _tempTimer = new Counter(1);
                //_ClosePanel();                
            }
        }
        private void _OnShowCenter()
		{
			if (null != _controller)
			{
				SetMenuType (_controller.menuType);
			}

        }
        
        private void _OnTick(float deltaTime)
        {
			if (GameModel.GetInstance.AlowGameCount () == false)
			{
				return;
			}

            _TimeUpdate();
            if (null != _timer && _timer.Increase(deltaTime))
            {
                _timer = null;
				if (packetType == 0)
				{
					_TimeOutAutoGetPacket();
				}
				else if(packetType==1)
				{
					_OnTimeOutAutoSendpackey ();
				}
              
            }

            //TODO
            if (null != _tempTimer && _tempTimer.Increase(deltaTime))
            {
                _tempTimer = null;
//				if (GameModel.GetInstance.isPlayNet == true)
//				{
//					NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
//				}
                _ClosePanel();
            }
        }

		private void _OnHideCenter()
		{
			
		}
        

		private void _OnDisposeCenter()
		{
            _timer = null;
        }
        private int GetCashCount()
        {
            return _inputcash == null ? 
                    0 : (string.IsNullOrEmpty(_inputcash.text) ? 
                    0 : int.Parse(_inputcash.text));
        }
        private void _TimeUpdate()
        {
            if (_timer == null)
                return;

            SetTextContext(_lbtimerecord, Mathf.CeilToInt(_timer.Total - _timer.Current).ToString());
        }
        private void SetTextContext(Text text,string context)
        {
            if (text != null)
                text.text = context;
        }
        private void _TimeOutAutoGetPacket()
        {
            SetTextContext(_lbtimerecord, "0");

            if (getPacketInfos == null)
                return;
            for (int i = 0; i < getPacketInfos.Length; i++)
            {
                if(getPacketInfos[i] != null)
                    getPacketInfos[i].TimeOutOpenPacket();
            }
        }
        private void _ClosePanel()
        {
            Client.Unit.BattleController.Instance.Send_RoleSelected(1);
            _controller.setVisible(false);
        }

		/// <summary>
		/// Ons the leave send packet.直接取消发红包
		/// </summary>
		/// <param name="go">Go.</param>
		private void _OnLeaveSendPacket(GameObject go)
		{			
			if (GameModel.GetInstance.isPlayNet == true)
			{
				NetWorkScript.getInstance ().Send_RedPocket (GameModel.GetInstance.curRoomId,0);
			}
			_ClosePanel();
		}

        private void _OnSendPacketHandler(GameObject go)
        {
            Audio.AudioManager.Instance.BtnMusic();

			var player=PlayerManager.Instance.HostPlayerInfo;

            int cashCount = GetCashCount();
//            if (cashCount > MaxCashCount)
//            {
//                MessageHint.Show("红包金额过高。");
//            }
			if (cashCount >player.totalMoney)
            {
                MessageHint.Show("您没有这么多钱。");
            }
            else if (cashCount < MinCashCount)
            {
				MessageHint.Show("请输入大于0的金额");
            }
            else
            {
                //				string _redText="<color=#e53232>{0}</color>";
                //				string _greenText="<color=#00b050>+{0}</color>";
                //                MessageHint.Show("红包发送成功~！");
                //TODO 此处应该调用通用接口，玩家减少这些钱，NPC增加这些钱
                //MessageHint.Show(string.Format("您给<color=#00b050>{0}</color>包了一个<color=#f1df17>{1}</color>元的红包",_controller.player.playerName,cashCount.ToString()));
                MessageTips.Show(string.Format(GameTipManager.Instance.gameTips.overOuterSendRed, _controller.player.playerName));

                if (GameModel.GetInstance.isPlayNet == false)
				{
					player.totalMoney -= cashCount;
					var battleControll = UIControllerManager.Instance.GetController<UIBattleController>();
					if (null != battleControll)
					{
						battleControll.SetCashFlow ((int)player.totalMoney, -1);
						if (GameModel.GetInstance.isPlayNet == false)//单机状态下，刷新目标玩家的金币
						{
							_controller.player.totalMoney += cashCount;
							var index = Array.IndexOf (PlayerManager.Instance.Players, _controller.player);
							if (index > 0) 
							{
								battleControll.SetPersonInfor (_controller.player, index);
							}
						}
					}
				}

                if (cashCount > 0)
                {
                    Audio.AudioManager.Instance.Tip_RedPackage(PlayerManager.Instance.HostPlayerInfo.careerID);
                }

                if (GameModel.GetInstance.isPlayNet == true)
				{
					NetWorkScript.getInstance ().Send_RedPocket (GameModel.GetInstance.curRoomId,-cashCount);
				}
                _ClosePanel();
            }
        }


		private void _OnTimeOutAutoSendpackey()
		{
			SetMenuType(2);

			var player=PlayerManager.Instance.HostPlayerInfo;

			int cashCount =UnityEngine.Random.Range (0, Mathf.FloorToInt (player.totalMoney * 0.3f));

			_inputcash.text=cashCount.ToString();

            //MessageHint.Show(string.Format("您给<color=#00b050>{0}</color>包了一个<color=#f1df17>{1}</color>元的红包",_controller.player.playerName,cashCount.ToString()));

            //MessageTips.Show(GameTipManager.Instance.gameTips.overOuterSendRed);
            MessageTips.Show(string.Format(GameTipManager.Instance.gameTips.overOuterSendRed, _controller.player.playerName));

            if (GameModel.GetInstance.isPlayNet == false)
			{
				player.totalMoney -= cashCount;
				var battleControll = UIControllerManager.Instance.GetController<UIBattleController>();
				if (null != battleControll)
				{
					battleControll.SetCashFlow ((int)player.totalMoney, -1);

					if (GameModel.GetInstance.isPlayNet == false)//单机状态下，刷新目标玩家的金币
					{
						_controller.player.totalMoney += cashCount;
						var index = Array.IndexOf (PlayerManager.Instance.Players, _controller.player);
						if (index > 0) 
						{
							battleControll.SetPersonInfor (_controller.player, index);
						}
					}
				}
			}

			if (GameModel.GetInstance.isPlayNet == true)
			{
				NetWorkScript.getInstance ().Send_RedPocket (GameModel.GetInstance.curRoomId,-cashCount);
			}

			_tempTimer = new Counter(1);

		}


        private void _OnPackPacketHandler(GameObject go)
        {
            Audio.AudioManager.Instance.BtnMusic();
            SetMenuType(2);
        }
        /// <summary>
        /// 0领红包1包红包2发红包
        /// </summary>
        /// <param name="type"></param>
        public void SetMenuType(int type)
        {
            switch (type)
            {
			case 0:
				_getpacketpanel.SetActiveEx (true);
				_packpacketpanel.SetActiveEx (false);
				_sendpacketpanel.SetActiveEx (false);
				RefreshGetPacketPanel ();
				packetType = 0;
                    break;
			case 1:
				_getpacketpanel.SetActiveEx (false);
				_packpacketpanel.SetActiveEx (true);
				_sendpacketpanel.SetActiveEx (false);
				RefreshPackPacketPanel ();		
				packetType = 1;
                    break;
			case 2:
				_getpacketpanel.SetActiveEx (false);
				_packpacketpanel.SetActiveEx (false);
				_sendpacketpanel.SetActiveEx (true);
				RefreshSendPacketPanel ();

                    break;
            }
        }

        public class UIRPGetPacketInfo
        {
            private const string prompt = "庆祝孩子的降生\t您收到了XXX的红包\t快打开吧！";

            public GameObject _obj;

            private Transform _objnotgetpacket;
            private Transform _objgetpacket;

            public Text _lbgetprompt;
            public Text _lbgetCashCount;

            public Button _btnopen;

            private PlayerInfo _playerInfo;
            private Action _endCallBack;
            public bool IsEnd { get { return isEnd; } }
            private bool isEnd = false;

            public void InitInfo(GameObject obj,PlayerInfo playerInfo,Action endCallBack = null)
            {
                _obj = obj;
                _playerInfo = playerInfo;
                _endCallBack = endCallBack;
                isEnd = false;

                _objnotgetpacket = obj.GetComponentEx<Transform>(Layout.obj_notgetpacket);
                _objgetpacket = obj.GetComponentEx<Transform>(Layout.obj_getpacket);
                _objgetpacket.SetActiveEx(false);

                _lbgetprompt = obj.GetComponentEx<Text>(Layout.obj_notgetpacket + Layout.lb_getprompt);
                _lbgetCashCount = obj.GetComponentEx<Text>(Layout.obj_getpacket + Layout.lb_getCashCount);

                _btnopen = obj.GetComponentEx<Button>(Layout.obj_notgetpacket + Layout.btn_open);
                EventTriggerListener.Get(_btnopen.gameObject).onClick = _OnOpenPacketHandler;

                _lbgetprompt.text = prompt.Replace("XXX", _playerInfo.playerName);
            }
            public void TimeOutOpenPacket()
            {
                if (!isEnd)
                    OpenPacket();
            }
            private void _OnOpenPacketHandler(GameObject go)
            {
                Audio.AudioManager.Instance.BtnMusic();
                OpenPacket();
            }
            private void OpenPacket()
            {
				int random = 0;

				if (GameModel.GetInstance.isPlayNet == false)
				{
					if (_playerInfo.totalMoney > 0)
					{
						random=	UnityEngine.Random.Range (0, Mathf.FloorToInt (_playerInfo.totalMoney * 0.05f));
					}
					else
					{
						random = 0;
					}
				}
				else
				{
					if (null != GameModel.GetInstance.NetReadPackageJson)
					{
						var peopleMoney = 0;

						if (((IDictionary)GameModel.GetInstance.NetReadPackageJson).Contains (_playerInfo.playerID))
						{
							peopleMoney=int.Parse (GameModel.GetInstance.NetReadPackageJson [_playerInfo.playerID].ToString ());
						}
						random =Mathf.Abs(peopleMoney);
					}
				}
				
				if (GameModel.GetInstance.isPlayNet == false)
				{
					_playerInfo.totalMoney -= random;
					PlayerManager.Instance.HostPlayerInfo.totalMoney +=random;

					var index =Array.IndexOf ( PlayerManager.Instance.Players,_playerInfo);
					var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
					controller.SetCashFlow ((int)PlayerManager.Instance.HostPlayerInfo.totalMoney,-1);
					if (index > 0)
					{
						controller.SetPersonInfor (_playerInfo, index, false);
					}
				}

                _lbgetCashCount.text = string.Format("{0}元", random.ToString());
                //MessageHint.Show("播打开红包特效,调用飞金币接口.");

				MessageHint.Show (string.Format ("您收到了<color=#00b050>{0}</color>的<color=#f1df17>{1}</color>元红包", _playerInfo.playerName, random.ToString ()));

                _objnotgetpacket.SetActiveEx(false);
                _objgetpacket.SetActiveEx(true);

                //TODO此处应该等待打开红包特效结束，飞金币后调用关闭
                //TODO 此处应该调用通用接口，玩家减少这些钱，NPC增加这些钱
                EndCallBack();
            }
            private void EndCallBack()
            {
                isEnd = true;
                if (_endCallBack != null)
                    _endCallBack();
            }
        }
    }
}

