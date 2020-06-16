using System;

/// <summary>
///  卡牌类型
/// </summary>
public enum SpecialCardType
{
    /// <summary>
    /// 默认卡牌类型，初始值定义-1000，后面的按顺序递增
    /// </summary>
    None = -1000,
    /// <summary>
    /// 慈善事业 -999
    /// </summary>
    CharityType,
    /// <summary>
    /// 进修学习   -998
    /// </summary>
    StudyType,
    /// <summary>
    /// 健康管理  -997
    /// </summary>
    HealthType,
    /// <summary>
    /// 结账日  -996
    /// </summary>
    CheckDayType,
    /// <summary>
    /// 生孩子 -995
    /// </summary>
    GiveChildType,
    /// <summary>
    /// 外圈升级内圈 弹出卡牌 -994
    /// </summary>
    UpGradType,
    /// <summary>
    /// 内圈进修学习 -993
    /// </summary>
    InnerStudyType,
    /// <summary>
    /// 内圈结账日 -992
    /// </summary>
    InnerCheckDayType,
    /// <summary>
    /// 内圈健康管理 -991
    /// </summary>
    InnerHealthType,

    /// <summary>
    /// 胜利弹出卡牌 -990
    /// </summary>
    SuccessType,

    /// <summary>
    /// 特殊开牌  -989
    /// </summary>
    SpecailType,
    /// <summary>
    /// 机会卡牌  -988
    /// </summary>
    ChanceType,
    /// <summary>
    /// 选择大机会，还是小机会  -987
    /// </summary>
    SelectChance,
    /// <summary>
    /// 小机会总称     -986
    /// </summary>
    smallChance,
    /// <summary>
    /// 固定资产   小机会 -985
    /// </summary>
    fixedChance,
    /// <summary>
    /// 股票      小机会  -984
    /// </summary>
    sharesChance,
    /// <summary>
    /// 大机会    -983
    /// </summary>
	bigChance,
    /// <summary>
    /// 外圈命运  -982
    /// </summary>
    outFate,
    /// <summary>
    /// 风险     -981
    /// </summary>
    risk,
    /// <summary>
    ///内圈自由选择          -980
    /// </summary>        
    SelectInnerFree,

    /// <summary>
    ///内圈命运          - 979
    /// </summary>
	inFate,  
    /// <summary>
    ///有闲有钱          -978
    /// </summary>
	richRelax,
    /// <summary>
    ///投资   -977
    /// </summary>
    investment,
    /// <summary>
    ///品质生活          -976
    /// </summary>
    qualityLife,       
	    

}