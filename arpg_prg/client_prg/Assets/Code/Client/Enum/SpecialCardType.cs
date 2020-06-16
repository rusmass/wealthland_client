using System;

/// <summary>
///  卡牌类型
/// </summary>
public enum SpecialCardType
{
    None = -1000,
    CharityType, //慈善事业 -999
    StudyType, //进修学习   -998
    HealthType, //健康管理  -997
    CheckDayType, //结账日  -996
    GiveChildType, //生孩子 -995  

	UpGradType, // 外圈升级内圈 弹出卡牌 -994

    InnerStudyType, //内圈进修学习 -993
    InnerCheckDayType, //内圈结账日 -992
    InnerHealthType, //内圈健康管理 -991

    SuccessType, //胜利弹出卡牌 -990

	SpecailType, //特殊开牌  -989
	ChanceType,  //机会卡牌  -988 

	SelectChance,//选择大机会，还是小机会  -987
	smallChance,     //小机会总称           -986
	fixedChance,        //固定资产   小机会 -985
	sharesChance,       //股票      小机会  -984
	bigChance,          //大机会             -983
	outFate,            //外圈命运          -982
	risk,               //风险               -981
	        
	SelectInnerFree,//内圈自由选择          -980

	inFate,             //内圈命运          - 979
	richRelax,          //有闲有钱          -978
	investment,         //投资              -977
	qualityLife,        //品质生活          -976
	    

}