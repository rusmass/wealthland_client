using System;
/// <summary>
/// Protocol. 后台类型分类  登录，
/// </summary>
public class Protocol
{
	/// <summary>
	/// The WOSHO. 0 请求登录的握手响应
	/// </summary>
	public const int WOSHOU=0;

    /// <summary>
    /// 被踢除出服务器
    /// </summary>
    public const int TickedOutLine = 1;

	/// <summary>
	/// The WOSHO w responce.  二次握手，目前用于心跳机制
	/// </summary>
	public const int HEART_BEAT=2;

    /// <summary>
	/// The game game holl chat. 游戏大厅聊天
	/// </summary>
	public const int Game_GameHollChat = 3;
    /// <summary>
    /// The game room chat.房间聊天
    /// </summary>
    public const int Game_RoomChat = 4;
    /// <summary>
    /// 游戏大厅和游戏结束后的分享
    /// </summary>
    public const int Game_GetNornormalShareInfor = 5;

    /// <summary>
    /// 游戏房间号的分享
    /// </summary>
    public const int Game_GetRoomShareInfor = 6;

    /// <summary>
    /// The game friend log out. 注销
    /// </summary>
    public const int Game_LogOut = 7;

    /// <summary>
    /// 分享梦想板的内容
    /// </summary>
    public const int Game_ShareDream = 8;

    /// <summary>
    /// 玩家在游戏房间中中掉线
    /// </summary>
    public const int Game_LostInRoom = 1000;

    /// <summary>
	/// The game choose role lost. 选择人物界面掉线
	/// </summary>
	public const int Game_ChooseRoleLost = 1001;
   
    /// <summary>
    /// The game disconnected. 掉线，目前用于判断在游戏初始化中掉线的情况
    /// </summary>
    public const int Game_DisconnectInReady = 1002;

    /// <summary>
    /// 在游戏中掉线
    /// </summary>
    public const int Game_Disconnect = 1003;

    /// <summary>
    /// The game re connect to game. 重新连接进入游戏
    /// </summary>
    public const int Game_ReConnectToGame = 2001;

    /// <summary>
    /// The game agree re connect. 同意重新连接进入游戏
    /// </summary>
    public const int Game_AgreeReConnect = 2002;

    /// <summary>
    /// The game re connected inited. 重新进入游戏，完成初始化
    /// </summary>
    public const int Game_ReConnectedInited = 2003;

    /// <summary>
    /// 正式完成重连时，刷新玩家最新数据
    /// </summary>
    public const int Game_ReConnectedFresh = 2004;

    /// <summary>
    /// The game refuse re connect. 拒绝重连
    /// </summary>
    public const int Game_RefuseReConnect = 2005;    
    

    /// <summary>
    /// The game tips. 游戏公告（暂时未）放到http登录系统里
    /// </summary>
    public const int Game_Tips = 10000010;

    /// <summary>
    /// The game mail. 游戏邮箱 （暂时未）
    /// </summary>
    public const int Game_Mail = 1500000;

    /// <summary>
    /// The game rank. 游戏活跃排行榜（暂时未）
    /// </summary>
    public const int Game_Active_Rank = 2000000;

    /// <summary>
    /// The game level rank.游戏等级排行榜（暂时未）
    /// </summary>
    public const int Game_Level_Rank = 2001000;

    /// <summary>
    /// The game asset rank.（暂时未）
    /// </summary>
    public const int Game_Asset_Rank = 2002000;

    

    /// <summary>
    /// （暂时未）
    /// </summary>
	public const int Game_Player_Ready=400200;

	/// <summary>
	/// The game friend createroom.创建房间
	/// </summary>
	public const int Game_Friend_Createroom=3000;

	/// <summary>
	/// The game friend enter room.加入房间
	/// </summary>
	public const int Game_Friend_EnterRoom = 3001;

	/// <summary>
	/// The game friend exit room.退出房间
	/// </summary>
	public const int Game_Friend_ExitRoom =3002 ;

	/// <summary>
	/// The game friend kick. 房间里，踢人（踢人）
	/// </summary>
	public const int Game_Friend_Kick = 3003;

	/// <summary>
	/// The game friend ready room.玩家准备
	/// </summary>
	public const int Game_Friend_ReadyRoom=3004;	

	/// <summary>
	/// The game receive role infor.
	/// </summary>
	public const int Game_ReceiveSelectRoleInfor = 3005;    


    /// <summary>
    /// The game select role. 选人界面选择某人
    /// </summary>
    public const int Game_SelectRole=4000;

	/// <summary>
	/// The game cancle select. 选人界面选择
	/// </summary>
	public const int Game_CancleSelect=4001;

	/// <summary>
	/// The game sure select.选人界面确定已经选择
	/// </summary>
	public const int Game_SureSelect=4002;

    /// <summary>
	/// The game friend enter game scene.进入游戏界面
	/// </summary>
	public const int Game_Friend_EnterGameScene = 4003;

    /// <summary>
    /// The game auto select. 倒计时为0时自动选择(暂时未)
    /// </summary>
    public const int Game_AutoSelect = 4004;

	/// <summary>
	/// The game agree quit game.同意解散游戏
	/// </summary>
	public const int Game_AgreeQuitGame=5032;

	/// <summary>
	/// The game refuse quit game. 拒绝解散游戏
	/// </summary>
	public const int Game_RefuseQuitGame=5033;


	/// <summary>
	/// The game initlaize.初始化完成
	/// </summary>
	public const int Game_initlaize =5000;

	/// <summary>
	/// The game get borrow infor. 借贷模块信息
	/// </summary>
	public const int Game_GetBorrowInfor = 5001;

	/// <summary>
	/// The game get hero target infor.获取人物目标信息
	/// </summary>
	public const int Game_GetHeroTargetInfor=5002;

	/// <summary>
	/// The game get assets income infor. 获得资产收入的信息
	/// </summary>
	public const int Game_GetAssetsIncomeInfor = 5003;

	/// <summary>
	/// The game get player debt and pay infor. 获得负债和支出的信息
	/// </summary>
	public const int Game_GetPlayerDebtAndPayInfor = 5004;

	/// <summary>
	/// The game get player sale record infor. 获取人物出售人物信息
	/// </summary>
	public const int Game_GetPlayerSaleRecordInfor = 5005;

	/// <summary>
	/// The game get check data infor. 获取结算面板信息
	/// </summary>
	public const int Game_GetCheckDataInfor = 5006;

	/// <summary>
	/// The game borrow money. 贷款协议号
	/// </summary>
	public const int Game_BorrowMoney = 5007;

	/// <summary>
	/// The game pay back money. 还款协议
	/// </summary>
	public const int Game_PayBackMoney = 5008;


	/// <summary>
	/// The game rool craps.掷色子
	/// </summary>
	public const int Game_RollCraps = 5009;

	/// <summary>
	/// The game single round over. 单人回合结束
	/// </summary>
	public const int Game_SingleRoundEnd=5010;
	/// <summary>
	/// The game multi round end. 多人参与的回合结束
	/// </summary>
	public const int Game_MultiRoundEnd=5011;

    /// <summary>
    /// 购买小机会资产卡牌
    /// </summary>
	public const int Game_BuyFixedCard = 5012;               //购买小机会资产卡牌
    /// <summary>
    /// 购买小机会股票卡牌
    /// </summary>
	public const int Game_BuyChanceShareCard = 5013;         //购买小机会股票卡牌
    /// <summary>
    /// 购买大机会卡牌
    /// </summary>
	public const int Game_BuyOpportunityCard = 5014;         //购买大机会卡牌
    /// <summary>
    /// 购买外圈风险卡牌
    /// </summary>
    public const int Game_BuyRiskCard = 5015;                 //购买外圈风险卡牌
    /// <summary>
    /// 购买外圈慈善卡牌
    /// </summary>
    public const int Game_BuyCharityCard = 5016;               //购买外圈慈善卡牌
    /// <summary>
    /// 购买外圈学习卡牌
    /// </summary>
    public const int Game_BuyStudyCard = 5017;                //购买外圈学习卡牌
    /// <summary>
    /// 购买外圈健康卡牌
    /// </summary>
    public const int Game_BuyHealthCard = 5018;               //购买外圈健康卡牌

    /// <summary>
    /// The game send card. 抽取卡牌
    /// </summary>
    public const int Game_SelectCard = 5019;
    /// <summary>
    /// 购买有闲有钱卡牌
    /// </summary>
    public const int Game_BuyRelaxCard =5020;                 //购买有闲有钱卡牌
    /// <summary>
    /// 购买品质生活卡牌
    /// </summary>
    public const int Game_BuyQualityCard = 5021;               //购买品质生活卡牌
    /// <summary>
    /// 购买投资卡牌
    /// </summary>
    public const int Game_BuyInvestmentCard = 5022;                 //购买投资卡牌
    /// <summary>
    /// 购买内圈命运卡牌
    /// </summary>
    public const int Game_BuyInnerFateCard = 5023;                 //购买内圈命运卡牌
    /// <summary>
    /// 结账日
    /// </summary>
    public const int Game_BuyCheckDayCard = 5024;                        //结账日
    /// <summary>
    /// 购买外圈命运卡牌
    /// </summary>
    public const int Game_BuyOuterFateCard = 7007;                 //购买外圈命运卡牌

    /// <summary>
    /// The game update fished.
    /// </summary>
    public const int Game_UpdateInnerFished = 5025;

    /// <summary>
    /// The gamm buy insurance. 购买保险
    /// </summary>
    public const int Game_BuyInsurance = 5026;

    /// <summary>
    /// The game. 放弃卡牌
    /// </summary>
    public const int Game_QuitCard=5027;

	/// <summary>
	/// The game sale card. 出售卡牌
	/// </summary>
	public const int Game_SaleFixedCard=5028;

	/// <summary>
	/// The game sale chance share card. 出售股票卡牌
	/// </summary>
	public const int Game_SaleChanceShareCard = 5029;

	/// <summary>
	/// The game buy give child card.生孩子
	/// </summary>
	public const int Game_BuyGiveChildCard = 5030;

	/// <summary>
	/// The game send red packeg. 发送红包
	/// </summary>
	public const int Game_SendRedPackeg=5031;
   
    /// <summary>
    /// 发起向好友借款的请求
    /// </summary>
    public const int Game_ReadyBorrowFriend = 5034;
    /// <summary>
    /// 向好友钱的数目以及利率
    /// </summary>
    public const int Game_BorrowFriendMoney = 5035;
    /// <summary>
    /// 好友借款的提示面板
    /// </summary>
    public const int Game_BorrowFriendInfor = 5036;

    /// <summary>
    /// The game match. 游戏匹配游戏匹配模式
    /// </summary>
    public const int Game_Match = 6000;
    /// <summary>
    /// 游戏匹配四个人，进入房间了
    /// </summary>
    public const int Gane_Match_OK = 6001;   

    /// <summary>
    /// The game match cancle. 取消游戏匹配
    /// </summary>
    public const int Game_Match_Cancle = 6002;

    /// <summary>
    /// 匹配模式进入游戏
    /// </summary>
    public const int Game_Match_EnterGame = 6003;

    /// <summary>
    /// 匹配需要添加机器人了
    /// </summary>
    public const int Game_Math_NeedRobotGame = 6004;

    /// <summary>
    /// 返回对战记录的接口
    /// </summary>
    public const int Game_GameRecordInfor = 7000;

    /// <summary>
    /// 查看游戏对战的详情
    /// </summary>
    public const int Game_GameRecoredDetail = 7001;

    /// <summary>
    /// The game game over.
    /// </summary>
    public const int Game_GameOver=60016;

	/// <summary>
	/// The game modify data. 修改人物信息
	/// </summary>
	public const int Game_ModifyData=101;
}
