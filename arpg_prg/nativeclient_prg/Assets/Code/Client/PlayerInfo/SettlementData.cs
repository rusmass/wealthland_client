using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI
{
    public class SettlementData
    {
        public SettlementData()
        {

        }

        #region 创造财富
        
        /// <summary>
        /// 大机会得分计算
        /// </summary>
        public int OpportunityScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    var tmpNum = 0;
                    if (_bigOpportunitiesTotalNum > 0)
                    {
                        tmpNum = (int)(_bigOpportunitiesNum * (1 + 0.1f + _bigOpportunitiesNum / _bigOpportunitiesTotalNum));
                    }
                    return tmpNum;
                }

                return _opportunityscore;               
            }           

            set
            {
                _opportunityscore = value;
            }
        }
        private int _opportunityscore = 0;





        /// <summary>
        /// 小机会得分计算
        /// </summary>
        public int smallChanceFixed
        {
            get
            {
                if(GameModel.GetInstance.isPlayNet==false)
                {
                    var tmpNum = 0;
                    if (_smallOpportunitiesTotalNum > 0)
                    {
                        tmpNum = (int)(_smallOpportunitiesNum * (1 + 0.1f + _smallOpportunitiesNum / _smallOpportunitiesTotalNum));
                    }
                    return tmpNum;
                }

                return _smallchance;               
            }
            set
            {
                _smallchance = value;
            }
        }
        private int _smallchance = 0;

        /// <summary>
        /// 外圈结账日得分计算
        /// </summary>
        public int OutCheckScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    return (int)(_settleOuterNum * 2.1f);
                }
                return _outercheckscore;
            }

            set
            {
                _outercheckscore = value;
            }
        }
        private int _outercheckscore = 0;

        /// <summary>
        /// 卖出资产的积分
        /// </summary>
        public int SaleNumScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    return (int)(_saleNums * 2.1f);

                }
                return _saleNumberScore;               
            }

            set
            {
                _saleNumberScore = value;
            }            
        }
        private int _saleNumberScore;
        #endregion

        #region 管理财富

        /// <summary>
        /// 投资得分计算
        /// </summary>
        public int InvestmentScore
        {
           get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    var tmpNum = 0;
                    if (_investmentTotalNum > 0)
                    {
                        tmpNum = (int)(_investmentNum * (1 + 0.2f + _investmentNum / _investmentTotalNum));
                    }
                    return tmpNum;// 
                }
                return _investmentScore;                
            }

            set
            {
                _investmentScore = value;
            }
        }
        private int _investmentScore;


        /// <summary>
        /// 购买保险的积分计算
        /// </summary>
        public int BuyCareScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    return (int)(_buyCareNum * (2.2f));
                }
                return _buyCareScore;                
            }
            set
            {
                _buyCareScore = value;
            }
        }
        private int _buyCareScore = 0;
	
        /// <summary>
        /// 内圈结算积分
        /// </summary>
        public int InnerCheckScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    return (int)(_settleInnerNum * 2.2f);

                }
                return _innerCheckScore;
            }
            set
            {
                _innerCheckScore = value;
            }
        }
        private int _innerCheckScore = 0;

    #endregion


    #region 运用财富的数据

    /// <summary>
    /// 获取有钱有闲的得分情况
    /// </summary>
    public int RelaxScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    var tmpNum = 0;

                    if (_richleisureTotalNum > 0)
                    {
                        tmpNum = (int)(_richleisureNum * (1 + 0.3f + _richleisureNum / _richleisureTotalNum));
                    }
                    return tmpNum;
                }
                return _relaxScore;  
       
            }

            set
            {
                _relaxScore = value;
            }
        }
        private int _relaxScore;

        /// <summary>
        /// 品质积分的得分情况
        /// </summary>
        public int QualityScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    var tmpNum = 0;
                    if (_qualityTotalNum > 0)
                    {
                        tmpNum = (int)(_qualityNum * (1 + 0.3f + _qualityNum / _qualityTotalNum));
                    }
                    return tmpNum;
                }
                return _qualityScore;              
            }

            set
            {
                _qualityScore = 0;
            }
        }

        private int _qualityScore = 0;

        /// <summary>
        /// 慈善事业得分情况
        /// </summary>
        public int CharityScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    var tmpNum = 0;

                    if (_charityTotalNum > 0)
                    {
                        tmpNum = (int)(_charityNum * (1 + 0.3f + _charityNum / _charityTotalNum));
                    }
                    return tmpNum;
                }
                return _charityScore;               
            }

            set
            {
                _charityScore = value;
            }
        }
        private int _charityScore;

        #endregion

        #region 超越财富

        /// <summary>
        /// 进修学习积分计算
        /// </summary>
        public int LearnScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    int tmpNum = 0;
                    if (_learnTotalNum > 0)
                    {
                        tmpNum = (int)(_learnNum * (1 + 0.4f + _learnNum / _learnTotalNum));
                    }
                    return tmpNum;
                }
                return _learnScore;               
            }
            set
            {
                _learnScore = value;
            }
        }
        private int _learnScore = 0;


        public int HealthScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    int tmpNum = 0;
                    if (_healthTotalNum > 0)
                    {
                        tmpNum = (int)(_healthNum * (1 + 0.4f + _healthNum / _healthTotalNum));
                    }
                    return tmpNum;
                }
                return _healthScore;               
            }

            set
            {
                _healthScore = value;
            }
        }
        private int _healthScore=0;

        /// <summary>
        /// 失业分数计算
        /// </summary>
        public int UnemploymenyScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    return (int)(_unemploymentNum * 2.4f);
                }
                return _unemployScore;
            }
            set
            {
                _unemployScore = value;
            }
        }
        private int _unemployScore;

        /// <summary>
        /// 审计得分计算
        /// </summary>
        public int AuditScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    int tmpNum = 0;
                    if (_auditTotalNum > 0)
                    {
                        tmpNum = (int)(_auditNum * (1 + 0.4f + _auditNum / _auditTotalNum));
                    }
                    return tmpNum;
                }
                return _auditScore;                        
            }            
            set
            {
                _auditScore = value;
            }
        }
        private int _auditScore = 0;

        /// <summary>
        /// 离婚得分计算
        /// </summary>
        public int DivorceScore
        {
            get
            {
                if(GameModel.GetInstance.isPlayNet==false)
                {
                    int tmpNum = 0;
                    if (_divorceTotalNum > 0)
                    {
                        tmpNum = (int)(_divorceNum * (1 + 0.4f + _divorceNum / _divorceTotalNum));
                    }
                    return tmpNum;
                }
                return _divorceScore;                
            }

            set
            {
                _divorceScore = value;
            }
        }
        private int _divorceScore = 0;


        /// <summary>
        /// 金融风暴的得分计算
        /// </summary>
        public int LossMoneyScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    int tmpNum = 0;

                    if (_moneyLossTotalNum > 0)
                    {
                        tmpNum = (int)(_moneyLoss * (1 + 0.4f + _moneyLoss / _moneyLossTotalNum));
                    }
                    return tmpNum;
                }
                return _lossMoneyScore;                
            }
            set
            {
                _lossMoneyScore = value;
            }
        }
        private int _lossMoneyScore = 0;

        #endregion

        /// <summary>
        /// 总的得分数
        /// </summary>
        public int totoScore
        {
            get
            {
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    //超越财富
                    var eqIntegral = _charityNum * 1 + _childNum * 1 + _richleisureNum * 1 + _qualityNum * 1;
                    var spiritualIntegral = 20;
                    var adversityIntegral = _unemploymentNum * 1 + _auditNum * 1 + _divorceNum * 1;
                    var financialIntegral = _bigIntegral + _smallIntegral + _investmentIntegral + _settlementNum * 1;
                    int integral = eqIntegral * 20 + spiritualIntegral * 30 + adversityIntegral * 5 + financialIntegral * 40;
                    return integral;
                }
                return _totalScore;               
            }
            set
            {
                _totalScore = value;
            }
        }
        private int _totalScore = 0;

#region  游戏中遇到的次数

        //运用财富
        /// <summary>
        /// 有钱有闲计数
        /// </summary>
        public int _richleisureNum = 0;
        /// <summary>
        /// The richleisure total number.遇到有钱有闲的总次数
        /// </summary>
        public int _richleisureTotalNum = 0;

        /// <summary>
        /// 品质生活计数
        /// </summary>
        public int _qualityNum = 0;
        /// <summary>
        /// The quality total number.品质生活的总次数
        /// </summary>
        public int _qualityTotalNum = 0;

        /// <summary>
        /// 生孩子计数
        /// </summary>
        public int _childNum;
        /// <summary>
        /// 慈善计数
        /// </summary>
        public int _charityNum;
        /// <summary>
        /// The charity total number. 慈善的总次数
        /// </summary>
        public float _charityTotalNum = 0;

        //灵性指数
        /// <summary>
        /// 健康管理计数
        /// </summary>
        public int _healthNum;

        /// <summary>
        /// The health total number. 健康管理总的次数
        /// </summary>
        public float _healthTotalNum = 0;

        /// <summary>
        /// 学习进修计数
        /// </summary>
        public int _learnNum;
        /// <summary>
        /// The learn total number  进修学习总次数
        /// </summary>
        public float _learnTotalNum = 0;

        //逆境指数
        /// <summary>
        ///失业计数
        /// </summary>
        public int _unemploymentNum;
        /// <summary>
        ///审计计数
        /// </summary>
        public int _auditNum;
        /// <summary>
        /// 遇到审计的总次数
        /// </summary>
        public float _auditTotalNum = 0;

        /// <summary>
        /// 离婚计数
        /// </summary>
        public int _divorceNum;
        /// <summary>
        /// The divorce total number.离婚的总次数
        /// </summary>
        public float _divorceTotalNum = 0;

        /// <summary>
        /// The money loss. 金融风暴次数
        /// </summary>
        public int _moneyLoss = 0;
        /// <summary>
        /// 左右遭遇金融风暴的次数
        /// </summary>
        public float _moneyLossTotalNum = 0;

        //财商指数
        /// <summary>
        /// 大机会计数
        /// </summary>
        public int _bigOpportunitiesNum = 0;

        /// <summary>
        /// The big opportunities total number. 大机会总次数
        /// </summary>
        public float _bigOpportunitiesTotalNum = 0;

        /// <summary>
        /// 小机会计数
        /// </summary>
        public int _smallOpportunitiesNum;
        /// <summary>
        /// The small opportunities total number. 小机会总次数
        /// </summary>
        public float _smallOpportunitiesTotalNum = 0;

        /// <summary>
        /// 投资计数
        /// </summary>
        public int _investmentNum;
        /// <summary>
        /// The investment total number.投资总次数
        /// </summary>
        public float _investmentTotalNum = 0;
        /// <summary>
        /// 结算计数
        /// </summary>
        public int _settlementNum;

        /// <summary>
        /// The settle outer number. 外圈结账日数
        /// </summary>
        public int _settleOuterNum = 0;

        /// <summary>
        /// The settle inner number. 内圈结账日数
        /// </summary>
        public int _settleInnerNum = 0;

        /// <summary>
        /// The buy care number. 购买保险的次数
        /// </summary>
        public int _buyCareNum = 0;

        /// <summary>
        /// The sale nums. 成交的次数
        /// </summary>
        public int _saleNums = 0;

        #endregion


        /// <summary>
        ///  管理财富
        /// </summary>
        //private int spiritualIntegral;

        /// <summary>
        /// 超越财富
        /// </summary>
        //private int adversityIntegral;

        /// <summary>
        ///  创造财富
        /// </summary>
        // private int financialIntegral;

        /// <summary>
        /// 运用财富
        /// </summary>
        // private int eqIntegral;

        /// <summary>
        /// 大机会积分
        /// </summary>
        public int _bigIntegral;
        /// <summary>
        /// 小机会积分
        /// </summary>
        public int _smallIntegral;

        /// <summary>
        /// 遇到风险的卡牌积分
        /// </summary>
        public int _riskIntegral;

        /// <summary>
        /// 外圈命运的积分
        /// </summary>
        public int _outerFateIntegral;

        /// <summary>
        /// 内圈有钱有闲积分
        /// </summary>
        public int _relaxIntegral;

        /// <summary>
        /// 投资积分
        /// </summary>
        public int _investmentIntegral;

        /// <summary>
        /// 品质生活的积分
        /// </summary>
        public int _qualityIntegral;

        /// <summary>
        /// 内圈命运卡牌积分
        /// </summary>
        public int _innerFateIntegral;
        
    }
}
