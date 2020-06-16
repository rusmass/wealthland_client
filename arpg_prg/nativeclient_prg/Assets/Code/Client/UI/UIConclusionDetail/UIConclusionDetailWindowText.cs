using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Core.Web;
using Metadata;
using System.Collections.Generic;

namespace Client.UI
{
	public partial class UIConclusionDetailWindow
    {
		private void _OnInitText(GameObject go)
		{
            lb_base = go.GetComponentEx<Text>(Layout.txt_base);
            lb_create = go.GetComponentEx<Text>(Layout.txt_create);
            lb_manager = go.GetComponentEx<Text>(Layout.txt_manager);
            lb_use = go.GetComponentEx<Text>(Layout.txt_use);
            lb_overstep = go.GetComponentEx<Text>(Layout.txt_overstep);

            lb_total = go.GetComponentEx<Text>(Layout.txt_total);

			GetScoreList();
		}

		private void _OnShowWindowTxt()
		{
			SetBaseText (_controller.player);
            SetChuangzaoText(_controller.player);
            SetGuanliText(_controller.player);

            SetYunyongText(_controller.player);
            SetChaoyueText(_controller.player);	
			SetScoreText(_controller.player);					
		}
		

		/// <summary>
		/// Sets the base text.  设置基本信息
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetBaseText(PlayerInfo info)
		{
            var tmpStr = "\n\n亲爱的{0}您好,\n" +
                "您本次游戏选择了{1}的职业，\n" +
                "当前的现金是{2},\n" +
                "游戏中总的资产:{3},\n" +
                "总收入:{4},\n" +
                "负债:{5},\n" +
                "费劳务收入:{6},\n" +
                "总支出:{7}" +
                "您在游戏中的经历了{8}年,\n" +
                "在外圈的游戏时间{9}分钟,\n" +
                "在内圈的游戏时间{10}分钟.";

            var name = info.playerName;
            var career= info.career;
            var baseCash= info.totalMoney.ToString("F0");
            var baseAssets= info.CurrentIncome.ToString("F0");
            var baseShouRu = info.totalIncome.ToString("F0");
            var fuzhai = (info.bankIncome + info.creditIncome).ToString("F0");
            var BaseFeiLaoWu= info.totalIncome.ToString("F0");			
			var BaseZhiChu = info.totalPayment.ToString("F0");

            var BaseAge = info.RoundNumber.ToString();
            var outertime = GameTimerManager.Instance.getOuterTime(info.playerID);
            var innertime = GameTimerManager.Instance.getInnerTime(info.playerID);

            this.lb_base.text = string.Format(tmpStr, name, career, baseCash, baseAssets, baseShouRu, fuzhai, BaseFeiLaoWu, BaseZhiChu, BaseAge, outertime, innertime);

        }

		/// <summary>
		/// Sets the spiritual text. 超越财富
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="mstate">If set to <c>true</c> mstate.</param>
		private void SetChaoyueText(PlayerInfo info)
		{
			int _learnNum = info.Settlement.LearnScore;	
			int _healthNum = info.Settlement.HealthScore;			
			int _unemploymentNum = info.Settlement.UnemploymenyScore;			
			int _auditNum = info.Settlement.AuditScore;			
			int _divorceNum = info.Settlement.DivorceScore;
			int _moneyLoss = info.Settlement.LossMoneyScore;

            _chaoyueNumArr[0] = _learnNum;
            _chaoyueNumArr[1] = _healthNum;
            _chaoyueNumArr[2] = _unemploymentNum;
            _chaoyueNumArr[3] = _auditNum;
            _chaoyueNumArr[4] = _divorceNum;
            _chaoyueNumArr[5] = _moneyLoss;

            //{"进修学习:","健康管理:","失业次数:","审计:","离婚:","金融风暴:" };
            var tmpStr = "\n\n您在游戏中，遇到进修学习{0}次，健康管理{1}次，失业{2}次,被审计{3}次,离婚{4}次,遭遇金融风暴{5}。" +
              "您超越财富的总得分是{6}。\n" +
              "你的详细评价如下:\n" +
              "{7}";

            var tmplen = _chaoyueArr.Length;
			var tmpNum = 0;
			for (var i = 0; i < tmplen; i++)
			{
				tmpNum += _chaoyueNumArr [i];
			}
			_starNumChaoyue = _GetStarForChengzhang (tmpNum);
            //_starChaoyue.SetScores (tmpNum,_starNumChaoyue);
            var tmpTip = GameTipManager.Instance.GetStarTipForChengzhang(_starNumChaoyue);

            this.lb_overstep.text = string.Format(tmpStr, _learnNum, _healthNum, _unemploymentNum, _auditNum, _divorceNum, _moneyLoss, tmpNum, tmpTip);

        }


		/// <summary>
		/// Sets the EQ text.  品质指数 运用财富
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetYunyongText(PlayerInfo info)
		{						
			int _richleisureNum = info.Settlement.RelaxScore;	           
			int _qualityNum = _qualityNum = info.Settlement.QualityScore; 	                         
			int _charity = info.Settlement.CharityScore;

            _yunyongnumArr[0] = _richleisureNum;
            _yunyongnumArr[1] = _qualityNum;
            _yunyongnumArr[2] = _charity;

            //private string[] _yunyongArr = { "有钱有闲:", "品质生活:", "慈善事业:" };

            var tmpStr = "\n\n您在游戏中，遇到有钱有闲{0}次，品质生活{1}次，慈善事业{2}次。" +
               "您运用财富的总得分是{3}。\n" +
               "你的详细评价如下:\n" +
               "{4}";        

			var arrLen = _yunyongnumArr.Length;
			var tmpNum = 0;
			for (var i = 0; i < arrLen; i++)
			{
				tmpNum += _yunyongnumArr[i];
			}
			_starNumYunyong = _GetStarForPingzhi (tmpNum);
            //_starYunyong.SetScores(tmpNum,_starNumYunyong);

            var tmpTip = GameTipManager.Instance.GetStarTipForPinzhi(_starNumYunyong);
            this.lb_use.text = string.Format(tmpStr, _richleisureNum, _qualityNum, _charity, tmpNum, tmpTip);

        }
	
		/// <summary>
		/// Sets the financia text. 财商指数 创造财富
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetChuangzaoText(PlayerInfo info)
		{
			int _bigOpportunitiesNum = info.Settlement.OpportunityScore;          
            int _smallOpportunitiesNum = info.Settlement.smallChanceFixed;                     
            int _settleOuter = info.Settlement.OutCheckScore;
            int _salenum = info.Settlement.SaleNumScore;//(int) (info.Settlement._saleNums *2.1f) ;

            //{ "大机会:","小机会:","买卖交易次数:","外圈结算次数:" };

            var tmpStr = "\n\n您在游戏中，遇到大机会{0}次，小机会{1}次，买卖交易{2}次，外圈结算{3}次。" +
                "您创造财富的总得分是{4}。\n" +
                "你的详细评价如下:\n" +
                "{5}";


            _chaungzaonumArr[0] = _bigOpportunitiesNum;
            _chaungzaonumArr[1] = _smallOpportunitiesNum;
            _chaungzaonumArr[2] = _salenum;
            _chaungzaonumArr[3] = _settleOuter;

            var arrLen = _chaungzaonumArr.Length;
			var tmpNum = 0;
			for (var i = 0; i < arrLen; i++)
			{
				tmpNum += _chaungzaonumArr [i];
			}
			_starNumChuangzao = _GetStarForCaishang (tmpNum);

            var tmpTip = GameTipManager.Instance.GetStarTipForCaishang(_starNumChuangzao);
            this.lb_create.text = string.Format(tmpStr, _bigOpportunitiesNum, _smallOpportunitiesNum, _settleOuter, _salenum, tmpNum, tmpTip);
           
		}		

		/// <summary>
		/// Sets the adversity text.  抗逆境指数  管理指数
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetGuanliText(PlayerInfo info)
		{
			int _investmentNum = info.Settlement.InvestmentScore;                
            int _buyCareNum = info.Settlement.BuyCareScore;// (int)(info.Settlement._buyCareNum * (2.2f)); 
            int _settleInnerNum =info.Settlement.InnerCheckScore;// (int)(info.Settlement._settleInnerNum * 2.2f);

            _guanlinumArr[0] = _investmentNum;
            _guanlinumArr[1] = _buyCareNum;
            _guanlinumArr[2] = _settleInnerNum;

            //{ "投资交易次数:","购买保险:","内圈结算次数:"};
            var tmpStr = "\n\n您在游戏中，投资交易{0}次，购买保险{1}次，内圈结算{2}次。" +
              "您管理财富的总得分是{3}。\n" +
              "你的详细评价如下:\n" +
              "{4}";          

			var tmpLen = _guanlinumArr.Length;
			var tmpNum = 0;
			for (var i = 0; i < tmpLen; i++)
			{
				tmpNum+=_guanlinumArr[i];				
			}
			_starNumGuanli = _GetStarForFengxian (tmpNum);
            //_starGuanli.SetScores (tmpNum,_starNumGuanli);
            var tmpTip = GameTipManager.Instance.GetStarTipForFengxian(_starNumGuanli);

            this.lb_manager.text = string.Format(tmpStr, _investmentNum, _buyCareNum, _settleInnerNum, _starNumGuanli, tmpTip);
        }


		/// <summary>
		/// Sets the score text.  设置总积分的
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetScoreText(PlayerInfo info)
		{           
            int integral=info.Settlement.totoScore;

         
            _starNumTotal = Mathf.FloorToInt((_starNumChuangzao + _starNumChaoyue + _starNumGuanli + _starNumYunyong) / 4f);

			if (_starNumTotal <= 1)
			{
				_starNumTotal = 1;
			}
			else if(_starNumTotal >=5)
			{
				_starNumTotal = 5;
			}

            var tmpStr = "\n\n您的综合得分是{0}，综合等级{1}\n" +
                "对您的综合评估如下:\n" +
                "{2}\n\n";
            //GameTipManager.Instance.GetEnterTip()
            this.lb_total.text = string.Format(tmpStr, integral, _starNumTotal,_controller.totalScoreTip );
                        
            //_starPingjia.SetScores (integral,_starNumTotal);
		}

	

		/// <summary>
		/// Gets the score list. 获取评分话术
		/// </summary>
		void GetScoreList()
		{
			var template = MetadataManager.Instance.GetTemplateTable<Score> ();
			var it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as Score;
				scoreList.Add(value.des);
			}
		}	

		/// <summary>
		/// Gets the star for pingzhi.  获得品质指数的星级
		/// </summary>
		/// <returns>The star for pingzhi.</returns>
		/// <param name="num">Number.</param>
		private int _GetStarForPingzhi(int num)
		{
			var tmpStar = 1;

			if (num >=40)
			{
				tmpStar = 5;
			}
			else if(num >=30 )
			{
				tmpStar = 4;
			}
			else if(num >=20 )
			{
				tmpStar = 3;
			}
			else if(num >=10)
			{
				tmpStar = 2;
			}
			else 
			{
				tmpStar = 1;
			}

			return tmpStar;
		}

		/// <summary>
		/// Gets the star for chengzhang. 获得成长指数的星级
		/// </summary>
		/// <returns>The star for chengzhang.</returns>
		/// <param name="num">Number.</param>
		private int _GetStarForChengzhang(int num)
		{
			var tmpStar = 1;
			if (num > 30)
			{
				tmpStar = 5;
			}
			else if(num >18 )
			{
				tmpStar = 4;
			}
			else if(num >12 )
			{
				tmpStar = 3;
			}
			else if(num >6)
			{
				tmpStar = 2;
			}
			else 
			{
				tmpStar = 1;
			}
			return tmpStar;
		}

		/// <summary>
		/// Gets the star for caishang. 获得财商指数的星级
		/// </summary>
		/// <returns>The star for caishang.</returns>
		/// <param name="num">Number.</param>
		private int _GetStarForCaishang(int num)
		{
			var tmpStar = 1;
			if (num >= 40)
			{
				tmpStar = 5;
			}
			else if(num >=30 )
			{
				tmpStar = 4;
			}
			else if(num >=20 )
			{
				tmpStar = 3;
			}
			else if(num >=10)
			{
				tmpStar = 2;
			}
			else 
			{
				tmpStar = 1;
			}
			return tmpStar;
		}

		/// <summary>
		/// Gets the star for fengxian. 获得风险指数的星级
		/// </summary>
		/// <returns>The star for fengxian.</returns>
		/// <param name="num">Number.</param>
		private int _GetStarForFengxian(int num)
		{
			var tmpStar = 1;
			if (num >= 40)
			{
				tmpStar = 5;
			}
			else if(num >=30 )
			{
				tmpStar = 4;
			}
			else if(num >=20 )
			{
				tmpStar = 3;
			}
			else if(num >=10)
			{
				tmpStar = 2;
			}
			else 
			{
				tmpStar = 1;
			}
			return tmpStar;
		}

		private static float timeScore;
		private static float qualityScore;
		private static string timeEnterInner;

		
		private int _starNumTotal = 1;

		private int _starNumYunyong=1;
		private int _starNumChaoyue=1;
		private int _starNumChuangzao = 1;
		private int _starNumGuanli = 1;


		private List <string> scoreList = new List<string>();
		/// <summary>
		/// The caishang arr. 创造财富  大机会成交数,小机会成交数,买卖成交次数,外圈结账日次数
		/// </summary>
		private string[] _chuangzaoArr = {"大机会:","小机会:","买卖交易次数:","外圈结算次数:" };
		/// <summary>
		/// The caishang arr.大机会成交数,小机会成交数,买卖成交次数,外圈结账日次数
		/// </summary>
		private int[] _chaungzaonumArr = {0,0,0,0};

		/// <summary>
		/// The guanli arr. 投资交易次数 ， 购买保险 ， 内圈结算次数
		/// </summary>
		private string[] _guanliArr = {"投资交易次数:","购买保险:","内圈结算次数:"};
		/// <summary>
		/// The guanli arr. 投资交易次数 ， 购买保险 ， 内圈结算次数
		/// </summary>
		private int[] _guanlinumArr={0,0,0};

		/// <summary>
		/// The pingzhi arr有钱有闲成交次数,品质生活成交次数，慈善事业
		/// </summary>
		private string[] _yunyongArr = {"有钱有闲:","品质生活:","慈善事业:" };
		/// <summary>
		/// The pingzhi arr  运用财富  有钱有闲成交次数,品质生活成交次数，慈善事业
		/// </summary>
		private int[] _yunyongnumArr={0,0,0};

		/// <summary>
		/// The chengzhang arr.  超越财富  进修学习,健康管理,失业次数,审计,离婚,金融风暴
		/// </summary>
		private string[] _chaoyueArr = {"进修学习:","健康管理:","失业次数:","审计:","离婚:","金融风暴:" };
		/// <summary>
		/// The chengzhang arr.进修学习,健康管理,失业次数,审计,离婚,金融风暴
		/// </summary>
		private int[] _chaoyueNumArr={0,0,0,0,0,0};



	



        /// <summary>
        /// 基础信息文本
        /// </summary>
        private Text lb_base;
        /// <summary>
        /// 创造财富文本
        /// </summary>
        private Text lb_create;
        /// <summary>
        /// 管理财富文本
        /// </summary>
        private Text lb_manager;
        /// <summary>
        /// 运用财富文本
        /// </summary>
        private Text lb_use;
        /// <summary>
        /// 超越财富文本
        /// </summary>
        private Text lb_overstep;

        /// <summary>
        /// 综合评分文本
        /// </summary>
        private Text lb_total;
	}
}
