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
	/// The WOSHO w responce.  二次握手
	/// </summary>
	public const int HEART_BEAT=1;

	/// <summary>
	/// The game re connect to game. 重新连接进入游戏
	/// </summary>
	public const int Game_ReConnectToGame = 2;

	/// <summary>
	/// The game disconnected. 掉线，目前用于判断在游戏中掉线的情况
	/// </summary>
	public const int Game_Disconnected = 3;

	/// <summary>
	/// The game agree re connect. 同意重新连接进入游戏
	/// </summary>
	public const int Game_AgreeReConnect = 4;

	/// <summary>
	/// The game refuse re connect. 
	/// </summary>
	public const int Game_RefuseReConnect = 6;

	/// <summary>
	/// The game re connected inited. 重新进入游戏，完成初始化
	/// </summary>
	public const int Game_ReConnectedInited=5;

	/// <summary>
	/// The game tips. 游戏公告
	/// </summary>
	public const int Game_Tips = 1000;

	/// <summary>
	/// The game mail. 游戏邮箱
	/// </summary>
	public const int Game_Mail = 1500;

	/// <summary>
	/// The game rank. 游戏活跃排行榜
	/// </summary>
	public const int Game_Active_Rank = 2000;

	/// <summary>
	/// The game level rank.游戏等级排行榜
	/// </summary>
	public const int Game_Level_Rank = 2001;

	/// <summary>
	/// The game asset rank.
	/// </summary>
	public const int Game_Asset_Rank = 2002;

	/// <summary>
	/// The game match. 游戏匹配
	/// </summary>
	public const int Game_Match = 4000;

	/// <summary>
	/// The game match cancle. 取消游戏匹配
	/// </summary>
	public const int Game_Match_Cancle=4001;

	public const int Game_Player_Ready=4002;

	/// <summary>
	/// The game friend createroom.创建房间
	/// </summary>
	public const int Game_Friend_Createroom=5000;

	/// <summary>
	/// The game friend enter room.加入房间
	/// </summary>
	public const int Game_Friend_EnterRoom = 5001;

	/// <summary>
	/// The game friend exit room.退出房间
	/// </summary>
	public const int Game_Friend_ExitRoom =5002 ;

	/// <summary>
	/// The game friend ready room.玩家准备
	/// </summary>
	public const int Game_Friend_ReadyRoom=5003;

	/// <summary>
	/// The game friend enter game scene.进入游戏界面
	/// </summary>
	public const int Game_Friend_EnterGameScene=5004;

	/// <summary>
	/// The game receive role infor.
	/// </summary>
	public const int Game_ReceiveSelectRoleInfor = 5005;

	/// <summary>
	/// The game select role. 选人界面选择某人
	/// </summary>
	public const int Game_SelectRole=5006;

	/// <summary>
	/// The game cancle select. 选人界面选择
	/// </summary>
	public const int Game_CancleSelect=5007;

	/// <summary>
	/// The game sure select.选人界面确定已经选择
	/// </summary>
	public const int Game_SureSelect=5008;

	/// <summary>
	/// The game auto select. 倒计时为0时自动选择
	/// </summary>
	public const int Game_AutoSelect = 5009;

	/// <summary>
	/// The game agree quit game.同意解散游戏
	/// </summary>
	public const int Game_AgreeQuitGame=5010;

	/// <summary>
	/// The game refuse quit game. 拒绝解散游戏
	/// </summary>
	public const int Game_RefuseQuitGame=5011;


	/// <summary>
	/// The game initlaize.初始化完成
	/// </summary>
	public const int Game_initlaize =6000;

	/// <summary>
	/// The game get borrow infor. 借贷模块信息
	/// </summary>
	public const int Game_GetBorrowInfor = 6001;

	/// <summary>
	/// The game get hero target infor.获取人物目标信息
	/// </summary>
	public const int Game_GetHeroTargetInfor=6002;

	/// <summary>
	/// The game get assets income infor. 获得资产收入的信息
	/// </summary>
	public const int Game_GetAssetsIncomeInfor = 6003;

	/// <summary>
	/// The game get player debt and pay infor. 获得负债和支出的信息
	/// </summary>
	public const int Game_GetPlayerDebtAndPayInfor = 6004;

	/// <summary>
	/// The game get player sale record infor. 获取人物出售人物信息
	/// </summary>
	public const int Game_GetPlayerSaleRecordInfor = 6005;

	/// <summary>
	/// The game get check data infor. 获取结算面板信息
	/// </summary>
	public const int Game_GetCheckDataInfor = 6006;

	/// <summary>
	/// The game borrow money. 贷款协议号
	/// </summary>
	public const int Game_BorrowMoney = 6007;

	/// <summary>
	/// The game pay back money. 还款协议
	/// </summary>
	public const int Game_PayBackMoney = 6008;


	/// <summary>
	/// The game rool craps.掷色子
	/// </summary>
	public const int Game_RollCraps = 7000;

	/// <summary>
	/// The game send card. 抽取卡牌
	/// </summary>
	public const int Game_SelectCard=7001;

	/// <summary>
	/// The game buy card. 购买卡牌
	/// </summary>
	//public const int Game_BuyCard=6003;

	/// <summary>
	/// The game single round over. 单人回合结束
	/// </summary>
	public const int Game_SingleRoundEnd=7002;
	/// <summary>
	/// The game multi round end. 多人参与的回合结束
	/// </summary>
	public const int Game_MultiRoundEnd=7003;


	public const int Game_BuyFixedCard = 7004;               //购买小机会资产卡牌

	public const int Game_BuyChanceShareCard = 7005;         //购买小机会股票卡牌

	public const int Game_BuyOpportunityCard = 7006;         //购买大机会卡牌

	public const int Game_BuyOuterFateCard = 7007;                 //购买外圈命运卡牌

	public const int Game_BuyRiskCard = 7008;                       //购买外圈风险卡牌

	public const int Game_BuyCharityCard = 7009;               //购买外圈慈善卡牌

	public const int Game_BuyStudyCard = 7010;                //购买外圈学习卡牌

	public const int Game_BuyHealthCard = 7011;               //购买外圈健康卡牌

	public const int Game_BuyRelaxCard = 7012;                 //购买有闲有钱卡牌

	public const int Game_BuyQualityCard = 7013;               //购买品质生活卡牌

	public const int Game_BuyInvestmentCard = 7014;                 //购买投资卡牌

	public const int Game_BuyInnerFateCard = 7015;                 //购买内圈命运卡牌

	public const int Game_BuyCheckDayCard  = 7016;                        //结账日

	/// <summary>
	/// The game. 放弃卡牌
	/// </summary>
	public const int Game_QuitCard=7017;

	/// <summary>
	/// The game sale card. 出售卡牌
	/// </summary>
	public const int Game_SaleFixedCard=7018;

	/// <summary>
	/// The game sale chance share card. 出售股票卡牌
	/// </summary>
	public const int Game_SaleChanceShareCard = 7019;


	/// <summary>
	/// The game buy give child card.生孩子
	/// </summary>
	public const int Game_BuyGiveChildCard = 7020;

	/// <summary>
	/// The game send red packeg. 发送红包
	/// </summary>
	public const int Game_SendRedPackeg=7021;



	/// <summary>
	/// The game update fished.
	/// </summary>
	public const int Game_UpdateInnerFished=7022;

	/// <summary>
	/// The gamm buy insurance. 购买保险
	/// </summary>
	public const int Game_BuyInsurance= 7023;

	/// <summary>
	/// The game game holl chat. 游戏大厅聊天
	/// </summary>
	public const int Game_GameHollChat = 8000;

	/// <summary>
	/// The game room chat.房间聊天
	/// </summary>
	public const int Game_RoomChat=8001;


	/// <summary>
	/// The game game over.
	/// </summary>
	public const int Game_GameOver=60016;

	/// <summary>
	/// The game friend log out. 注销
	/// </summary>
	public const int Game_LogOut=100;

	/// <summary>
	/// The game modify data. 修改人物信息
	/// </summary>
	public const int Game_ModifyData=101;

	public const int Game_GetNornormalShareInfor=9000;

	public const int Game_GetRoomShareInfor=9001;

	/// <summary>
	/// The game choose role lost. 选择人物界面掉线
	/// </summary>
	public const int Game_ChooseRoleLost = 10000;

	public const int Game_ContinuneGame=800003;

	public const int Game_ContinuneGameReinit=800004;

}
