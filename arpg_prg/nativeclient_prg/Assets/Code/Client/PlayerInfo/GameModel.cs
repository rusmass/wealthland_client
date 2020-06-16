﻿using System;
using GameNet;
using System.Collections.Generic;
using LitJson;
using Metadata;
using UnityEngine;
using UnityEngine.UI;
using Client.UI;

namespace Client
{
	public class GamePlayingState
	{
		/// <summary>
		/// The state of the game normal. 游戏正常状态
		/// </summary>
		public const int GameNormalState = 0;

		/// <summary>
		/// The state of the game wait.人物在排队中
		/// </summary>
		public const int GameWaitState = 1;

		/// <summary>
		/// The state of the game room.  处在房间中
		/// </summary>
		public const int GameRoomState = 2;

		/// <summary>
		/// The state of the game choose. 选择人物状态
		/// </summary>
		public const int GameNetChooseState = 3;

		/// <summary>
		/// The state of the game net game. 网络游戏对战
		/// </summary>
		public const int GameNetGameState = 4;

		/// <summary>
		/// The state of the game single game. 正常单机游戏
		/// </summary>
		public const int GameSingleGameState = 5; 

		/// <summary>
		/// The state of the game choose. 单机的选人界面
		/// </summary>
		public const int GameChooseState = 900;

		/// <summary>
		/// The state of the game over. 判断游戏结束了
		/// </summary>
		public const int GameOverState = 901;

	}

	public class GameModel
	{
        /// <summary>
        /// 卡牌展示时间最小值
        /// </summary>
        public static readonly int minRangeTime = 6;
        /// <summary>
        /// 卡牌展示时间最大值
        /// </summary>
        public static readonly int maxRangeTime = 10;


		private static GameModel _instance;

		public static GameModel GetInstance
		{
			get
			{
				if (null == _instance)
				{
					_instance = new GameModel ();
				}

				return _instance;
			}

		}
		public GameModel ()
		{
		}

        
        /// <summary>
        /// 删除游戏大厅里所有应该有的界面
        /// </summary>
        public void DeleteGamehallScene()
        {
            var gameHalll = UIControllerManager.Instance.GetController<Client.UI.UIGameHallWindowController>();
            if(gameHalll.getVisible()==true)
            {
                gameHalll.setVisible(false);
            }

            var controller = UIControllerManager.Instance.GetController<UIGameHallSetController>();
            if(controller.getVisible()==true)
            {
                controller.setVisible(false);
            }

            var tmpController = UIControllerManager.Instance.GetController<UIGameHallChatController>();
            if(tmpController.getVisible()==true)
            {
                tmpController.setVisible(false);
            }

            var gametip = UIControllerManager.Instance.GetController<UIGongGaoController>();
            if(gametip.getVisible()==true)
            {
                gametip.setVisible(false);
            }

            var gameshare = UIControllerManager.Instance.GetController<UIShareBoardWindowController>();
            if(gameshare.getVisible())
            {
                gameshare.setVisible(false);
            }

            var webController = UIControllerManager.Instance.GetController<UINativeWebController>();
            if(webController.getVisible())
            {
                webController.setVisible(false);
            }

            var conlusion = UIControllerManager.Instance.GetController<UIConsultingWindowController>();
            if(conlusion.getVisible())
            {
                conlusion.setVisible(false);
            }



            var personController = UIControllerManager.Instance.GetController<UIPersonalController>();
            if(personController.getVisible()==true)
            {
                personController.setVisible(false);
            }

            var personDetaiController = UIControllerManager.Instance.GetController<UIPersonGameRecodeController>();
            if(personDetaiController.getVisible()==true)
            {
                personDetaiController.setVisible(false);
            }


            var matchController = UIControllerManager.Instance.GetController<UIFightCatchController>();
            if(matchController.getVisible())
            {
                matchController.setVisible(false);
            }

            var fightController = UIControllerManager.Instance.GetController<UIFightroomController>();
            if(fightController.getVisible())
            {
                fightController.setVisible(false);
            }

            var netSelectRole = UIControllerManager.Instance.GetController<UIChooseRoleNetWindowController>();
            if(netSelectRole.getVisible())
            {
                netSelectRole.setVisible(false);
            }



        }

        /// <summary>
        /// 删除所有的选择角色界面
        /// </summary>
        public void DeleteSelectScene()
        {
            var control = Client.UIControllerManager.Instance.GetController<Client.UI.UIChooseRoleWindowController>();
            control.setVisible(false);
        }

        /// <summary>
        /// 删除游戏状态中可能出现的页面并返回引导页
        /// </summary>
        public void DeleteGameScene()
        {
            var gameoverstate = UIControllerManager.Instance.GetController<UIGameOverWindowController>();
            if(gameoverstate.getVisible())
            {
                gameoverstate.setVisible(false);
            }

            var conclusionDetai = UIControllerManager.Instance.GetController<UIConclusionDetailController>();
            if(conclusionDetai.getVisible())
            {
                conclusionDetai.setVisible(false);
            }

            var battle = UIControllerManager.Instance.GetController<UIBattleController>();
            if(battle.getVisible())
            {
                battle.setVisible(false);
            }

            MessageManager.getInstance().netExitGameDeleteCards();
        }


        /// <summary>
        /// 安卓下载链接
        /// </summary>
        public string androidDownPath = "";
        /// <summary>
        /// ios的下载链接
        /// </summary>
        public string iosDownPath = "";

        /// <summary>
        /// 当前游戏宽高比
        /// </summary>
        public float screenWidhtHightRate =540/960f;

        /// <summary>
        /// 修改UGUI画布canvas宽高比例值，试用与目标比设定值宽的条件
        /// </summary>
        /// <param name="go"></param>
        /// <param name="matchRate"></param>
        public void MathWidthOrHeightByCondition(GameObject go,float matchRate=0 )
        {
            var realHight = (float)Screen.height;
            var realWidth = Screen.width;
            if (realHight / realWidth > GameModel.GetInstance.screenWidhtHightRate)
            {               
                var canvasScaler = go.GetComponent<CanvasScaler>();
                canvasScaler.matchWidthOrHeight = matchRate;
            }
        }

        /// <summary>
        /// 直接修改UGUI画布canvas宽高比例值
        /// </summary>
        /// <param name="go"></param>
        /// <param name="matchRate"></param>
        public void MathWidthOrHeightDierctor(GameObject go,float matchRate=0)
        {
            var canvasScaler = go.GetComponent<CanvasScaler>();
            canvasScaler.matchWidthOrHeight = matchRate;
        }
        
        /// <summary>
        /// Alows the game count.允许倒计时当前，显示设置面板的时候不能倒计时
        /// </summary>
        /// <returns><c>true</c>, if game count was alowed, <c>false</c> otherwise.</returns>
        public bool AlowGameCount()
		{
			var isAlow = true;
			if (isPlayNet == true)
			{
				if (isNetGameSetShow == true)//||isNetGameSetShow==true
				{
					isAlow = false;
				}
			}
			return isAlow;
		}

		/// <summary>
		/// The is playing game. 判断玩家是否是进入了游戏界面 0是没有  1是游戏中
		/// </summary>
		public int IsPlayingGame=0;

        /// <summary>
        /// 登陆的状态，显示当前玩家是第一次登陆，还是反复登陆过
        /// </summary>
        public static int loginStatus = 0;

        public static readonly float CurVersion = 3f;
        //(2.2.23new,3)  2.2.27    
        public readonly string version="2.2.28";



		/// <summary>
		/// The state of the heart beat.心跳机制的模式 ， 正常界面是计时器心跳机制 0 ， 游戏界面是ui心跳机制
		/// </summary>
		public int heartBeatState = 0;

		/// <summary>
		/// The is need new version.是否需要下载新版本
		/// </summary>
		public bool isNeedNewVersion = false;

		public bool isNetGameSetShow=false;
//		public bool isNetGameBackShow=false;

		/// <summary>
		/// The is play net. 是联网游戏还是单机游戏
		/// </summary>
		public bool isPlayNet=false;

        /// <summary>
        /// 联网游戏的模式，0是好友对战模式，1匹配模式
        /// </summary>
        public int playNetMode = 0;

		public bool hasLoadTarget=false;
		public bool hasLoadBalanceAndIncome = false;
		public bool hasLoadDebtAndPay=false;
		public bool hasLoadSaleInfor =false;
		public bool hasLoadCheck = false;

        
        /// <summary>
		/// The http acount.  玩家访问的唯一标识符，类似session
		/// </summary>
		public static string HttpAcount = "";

        /// <summary>
        /// The borrow board time. 网络版记录借贷界面多久消失
        /// </summary>
        public float borrowBoardTime = -10;

		public string curRoomId="";

		public string reConnectRoomId = "";

		

		/// <summary>
		/// The index of the current start. 判断当前游戏玩家的开始索引
		/// </summary>
		public int curStartIndex;

		/// <summary>
		/// The is game real start.   当前游戏的游戏进行的状态 0 没有开始打开 ， 1正事开始了
		/// </summary>
		public int isGameRealStart=0;

		/// <summary>
		/// The game active rank list. 排行列表数组
		/// </summary>
		public List<GameRankVo> gameActiveRankList = new List<GameRankVo> ();
		public GameRankVo selfRank = new GameRankVo ();

		/// <summary>
		/// The type of the send card.发送卡牌的类型
		/// </summary>
		public int sendCardType=-1;

//		public int recieveCardType = -1;
		/// <summary>
		/// The type of the recieve card.后台返回来的相应的id相应的卡牌的ID
		/// </summary>
		public int ShowCardType=-1;

		public string NetCurrentPlayerId="";

		/// <summary>
		/// The net game received red plauer identifier.网络版游戏中接受红包的人的id
		/// </summary>
		public string NetGameReceivedRedPlauerId="";
		/// <summary>
		/// The net read package json. 显示收取红包的信息
		/// </summary>
		public JsonData NetReadPackageJson;
		/// <summary>
		/// The current roll points.掷骰子数
		/// </summary>
		public List<int> curRollPoints=new List<int>();
		/// <summary>
		/// My hand infor.自己的人物信息
		/// </summary>
		public PlayerHeadInfor myHandInfor=new PlayerHeadInfor();
		/// <summary>
		/// The inner card roll point.内圈投资和命运掷骰子数
		/// </summary>
		public int innerCardRollPoint=0;

		/// <summary>
		/// The is give child. 是否是生孩子， 0 是发红包 ， 1 是罚款
		/// </summary>
		public int isGiveChild = -1;

//		public float checkDatMoney = 0;

		/// <summary>
		/// The net fixed chanc card. 保持当前类型的卡牌
		/// </summary>
		public ChanceFixed netFixedChancCard;
		public ChanceShares netChanceShareCard;
		public Risk netRiskCard;
		public Opportunity netOpportunityCard;
		public OuterFate netOuterFateCard;

		public SpecialCard netSpecialCard;

		public InnerFate netInnerFateCard;
		public Investment netInvestmentCard;
		public QualityLife netQualityCard;
		public Relax netRelaxCard;

		/// <summary>
		/// The user sex. 性别  0 男  1 女
		/// </summary>
		//private int userSex = 0;

		public void setPlayerInfor(string name ,int sex , string _uuid , string _headPath,string _birthday)
		{
			myHandInfor.nickName = name;
			myHandInfor.sex = sex;
			myHandInfor.uuid = _uuid;
			myHandInfor.headImg = _headPath;
            myHandInfor.birthday = _birthday;
		}

		public void UpdatePlayerInfor()
		{
			myHandInfor.nickName = tmpModifyPlayerInfor.nickName;
			myHandInfor.sex = tmpModifyPlayerInfor.sex;
			myHandInfor.headImg = tmpModifyPlayerInfor.headImg;
		}

		public PlayerHeadInfor tmpModifyPlayerInfor=new PlayerHeadInfor();

		/// <summary>
		/// The is first in game hall. 判断是否是第一次进入游戏，如果是，填出公告公告板子
		/// </summary>
		public bool isFirstInGameHall=true;

		/// <summary>
		/// The is reconnec to game.是否是二次连接的 0不重连正常流程玩游戏   1、正在重连  2、完成重连
		/// </summary>
		public int isReconnecToGame=0;

		/// <summary>
		/// The is room all ready. 房间里是不是所有人都初始化完成
		/// </summary>
		public bool isRoomAllReady = false;

		/// <summary>
		/// The room player infor list.房间里面人物的信息
		/// </summary>
		public List<PlayerHeadInfor> roomPlayerInforList=new List<PlayerHeadInfor>();

		public List<GonggaoVo> gonggaoList=new List<GonggaoVo>();

		/// <summary>
		/// Inits the net game back data.玩家从游戏界面返回的时候，初始化的一些信息
		/// </summary>
		public void InitNetGameBackData()
		{
			isGameRealStart = 0;
			isReconnecToGame = 0;
			IsPlayingGame = GamePlayingState.GameNormalState;
			isRoomAllReady = false;
			NetCurrentPlayerId = "";

            curRoomId = "";
            
		}

		/// <summary>
		/// Shows the net loading. 向后台发送消息时，显示loading，防止误触
		/// </summary>
		public void ShowNetLoading()
		{
			if (null == _loadController)
			{
				_loadController = UIControllerManager.Instance.GetController<Client.UI.UIGameNetLoadingWindowController> ();
			}
			_loadController.setVisible (true);
		}

		/// <summary>
		/// Hides the net loading. 收到后台的提示后，隐藏loading，可以继续操作
		/// </summary>
		public void HideNetLoading()
		{
			if (null == _loadController)
			{
				_loadController = UIControllerManager.Instance.GetController<Client.UI.UIGameNetLoadingWindowController> ();
			}
			_loadController.setVisible (false);
		}

	
		private Client.UI.UIGameNetLoadingWindowController _loadController;

		/// <summary>
		/// Determines if is telephone the specified str_telephone.判断是不是正确的手机号
		/// </summary>
		/// <returns><c>true</c> if is telephone the specified str_telephone; otherwise, <c>false</c>.</returns>
		/// <param name="str_telephone">String telephone.</param>
		public static bool IsTelephone(string str_telephone)
		{
			//@^1[3|4|5|7|8][0-9]{9}$
			return System.Text.RegularExpressions.Regex.IsMatch(str_telephone,@"^[1]+[3,4,5,7,8]+\d{9}" );
//			return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, ); //~1[358]\d{9}$
//			return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");

		} 

		public string UserId
		{
			get{
				return "UserID";
			}
		}

        /// <summary>
        /// 玩家输入的密码
        /// </summary>
        public string UserPassword
        {
            get
            {
                return "Password";
            }
        }


        public string WeChatLastLoginTime
		{
			get
			{ 
				return "LastTime";
			}
		}


       
	}
}
