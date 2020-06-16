using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Core.Web;
using Metadata;
using System.Collections.Generic;

namespace Client.UI
{
	public partial class UIConclusionWindow
	{
		private void _OnInitText(GameObject go)
		{

			_txtTitle = go.GetComponentEx<Text>(Layout.txt_title);

			_txtBaseZhiYe = go.GetComponentEx<Text>(Layout.txt_base_zhiye);
			_txtBaseAge = go.GetComponentEx<Text>(Layout.txt_base_age);
			_txtBaseFeiLaoWu = go.GetComponentEx<Text>(Layout.txt_base_feilaowu);
			_txtBaseCash = go.GetComponentEx<Text>(Layout.txt_base_cash);
			_txtBaseAssets = go.GetComponentEx<Text>(Layout.txt_base_assets);
			_txtBaseLiabilities = go.GetComponentEx<Text>(Layout.txt_base_liabilities);
			_txtBaseShouRu = go.GetComponentEx<Text>(Layout.txt_base_shouru);
			_txtBaseZhiChu = go.GetComponentEx<Text>(Layout.txt_base_zhichu);

//			_txtEqCharity = go.GetComponentEx<Text>(Layout.txt_eq_charity);
//			_txtEqChild = go.GetComponentEx<Text>(Layout.txt_eq_child);
//			_txtEqQuality = go.GetComponentEx<Text>(Layout.txt_eq_quality);
//			_txtEqRichLeisuret = go.GetComponentEx<Text>(Layout.txt_eq_richLeisuret);


//			_txtSpiritualOuterTime = go.GetComponentEx<Text>(Layout.txt_spiritual_outerTime);
//			_txtSpiritualOuterQuality = go.GetComponentEx<Text>(Layout.txt_spiritual_outerQuality);
//			_txtSpiritualInnerQuality = go.GetComponentEx<Text>(Layout.txt_spiritual_Quality);
//			_txtSpiritualHealth = go.GetComponentEx<Text>(Layout.txt_spiritual_health);
//			_txtSpiritualLearning = go.GetComponentEx<Text>(Layout.txt_spiritual_learning);
//			_txtSpiritualInnerTime = go.GetComponentEx<Text>(Layout.txt_spiritual_innerTime);

			_txtSpiritualEndGame = go.GetComponentEx<Text>(Layout.txt_spiritual_endGame);
			_txtSpiritualIntoinner = go.GetComponentEx<Text>(Layout.txt_spiritual_intoinnertitle);

//			_txtAdversityDivorce = go.GetComponentEx<Text>(Layout.txt_adversity_divorce);
//			_txtAdversityAudit = go.GetComponentEx<Text>(Layout.txt_adversity_audit);
//			_txtAdversityUnemployment = go.GetComponentEx<Text>(Layout.txt_adversity_unemployment);

//			_txtFinancialBig = go.GetComponentEx<Text>(Layout.txt_financial_big);
//			_txtFinancialSmall = go.GetComponentEx<Text>(Layout.txt_financial_small);
//			_txtFinancialSettlement = go.GetComponentEx<Text>(Layout.txt_financial_settlement);
//			_txtFinancialInvestment = go.GetComponentEx<Text>(Layout.txt_financial_Investment);

			_imgLevel  = go.GetComponentEx<Image>(Layout.img_level);
			_txtEvaluation =  go.GetComponentEx<Text>(Layout.txt_evaluation);
			_txtScore = go.GetComponentEx<Text>(Layout.txt_score);

			_starYunyong = new UIConclusionStarBarItem (go.DeepFindEx(Layout.obj_pinzhi).gameObject);
			_starChaoyue = new UIConclusionStarBarItem (go.DeepFindEx (Layout.obj_chengzhang).gameObject);
			_starChuangzao = new UIConclusionStarBarItem (go.DeepFindEx(Layout.obj_caishang).gameObject);
			_starGuanli = new UIConclusionStarBarItem (go.DeepFindEx(Layout.obj_fengxian).gameObject);
			_starPingjia = new UIConclusionStarBarItem (go.DeepFindEx(Layout.obj_pingjia).gameObject);

			_starTip = new UIConclusionStarTip (go.GetComponentEx<Image> (Layout.obj_gametip).gameObject);

			GetScoreList();
		}

		private void _OnShowWindowTxt()
		{
			SetBaseText (_controller.player);
		

			SetYunyongText(_controller.player);

			SetChuangzaoText(_controller.player);

			SetChaoyueText(_controller.player,_controller.mstate);

			SetGuanliText(_controller.player);

			SetScoreText(_controller.player);
			SetScoreImage (_controller.player);
			SetGameTurnsNum(_controller.player);

			SetTitleName(_controller.mstate);
		}

		/// <summary>
		/// Sets the name of the title. 设置面板title
		/// </summary>
		/// <param name="mstate">If set to <c>true</c> mstate.</param>
		public void SetTitleName(bool mstate)
		{
			var controller = Client.UIControllerManager.Instance.GetController<UIEventLogController>();

			if(mstate == true)
			{
//				_txtTitle.text = string.Format("恭喜{0}获取最终胜利",PlayerManager.Instance.HostPlayerInfo.playerName);
				_txtTitle.text = "信息结算";

				controller.setBtnState(true);
			}
			else
			{
				_txtTitle.text = string.Format("恭喜{0}进入富人圈",PlayerManager.Instance.HostPlayerInfo.playerName);

				controller.setBtnState(false);
			}

			if (mstate == false)
			{
				_txtEvaluation.text = GameTipManager.Instance.GetEnterTip ();
			}
			else
			{
				_txtEvaluation.text = letter;//string.Format("您获得了{0}评价，您真的很厉害，可以出师了",letter);
			}
		}

		/// <summary>
		/// Sets the base text.  设置基本信息
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetBaseText(PlayerInfo info)
		{
			_txtBaseZhiYe.text = info.career;
			_txtBaseFeiLaoWu.text = info.totalIncome.ToString("F0");

			_txtBaseCash.text = info.totalMoney.ToString("F0");
			_txtBaseAssets.text = info.CurrentIncome.ToString("F0");

//			_txtBaseLiabilities.text = info.GetTotalDebt().ToString("F0");

			_txtBaseLiabilities.text = (info.bankIncome+info.creditIncome).ToString("F0");

			if (GameModel.GetInstance.isPlayNet == true)
			{				
				if (info.netIsSuccess == 0)
				{
					_txtBaseLiabilities.text="0";
				}
			}



			_txtBaseShouRu.text = info.totalIncome.ToString("F0");
			_txtBaseZhiChu.text = info.totalPayment.ToString("F0");
		}

		/// <summary>
		/// Sets the spiritual text.  设置显示游戏时间和
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="mstate">If set to <c>true</c> mstate.</param>
		private void SetChaoyueText(PlayerInfo info,bool mstate)
		{
//			_txtEqCharity.text = info._charityNum.ToString();
//			if(mstate == true)
//			{
//				_txtSpiritualOuterTime.text = timeScore.ToString();
//				_txtSpiritualOuterQuality.text = qualityScore.ToString();
//				_txtSpiritualInnerQuality.text = info.qualityScore.ToString();
//				_txtSpiritualInnerTime.text = info.timeScore.ToString();


//				int _chairty = 0;
//				if (info._charityTotalNum >0)
//				{
//					_chairty=int.Parse((info._charityNum *(1 + info._charityNum/(float)info._charityTotalNum)).ToString());
//				}

//				_chaoyueNumArr[0]=_chairty;
//				_chaoyueNumArr[1]=(int)timeScore;
//				_chaoyueNumArr[2]=(int)qualityScore;
//				_chaoyueNumArr[3]=(int)info.timeScore;
//				_chaoyueNumArr[4]=(int)info.qualityScore;

//				_txtSpiritualIntoinner.text = string.Format("{0}m",timeEnterInner);
//				_txtSpiritualEndGame.text = string.Format("{0}m",TimeManager.Instance.GetEndTime());
//			}
//			else
//			{
//				timeScore = info.timeScore;
//				qualityScore = info.qualityScore;
//				timeEnterInner = TimeManager.Instance.GetEnterInnerTime();
//				_txtSpiritualOuterTime.text = timeScore.ToString();
//				_txtSpiritualOuterQuality.text = qualityScore.ToString();
//				_txtSpiritualInnerQuality.text = "0";
//				_txtSpiritualInnerTime.text = "0";

//				_txtSpiritualIntoinner.text = string.Format("{0}m",timeEnterInner);
//				_txtSpiritualEndGame.text = string.Format("{0}m",0);


//				int _chairty = 0;
//
//				if (info._charityTotalNum > 0)
//				{
//					_chairty=(int)(info._charityNum *(1 + info._charityNum/(float)info._charityTotalNum));
//				}			

//				_chaoyueNumArr[0]=_chairty;
//				_chaoyueNumArr[1]=(int)timeScore;
//				_chaoyueNumArr[2]=(int)qualityScore;
//				_chaoyueNumArr[3]=0;
//				_chaoyueNumArr[4]=0;
//			}

			int _learnNum = 0;
			if (info._learnTotalNum > 0)
			{
				_learnNum =(int) (info._learnNum * (1 + 0.4f + info._learnNum / info._learnTotalNum));
			}

			int _healthNum = 0;
			if (info._healthTotalNum > 0)
			{
				_healthNum =(int) (info._healthNum * (1+0.4f + info._healthNum / info._healthTotalNum));
			}
			int _unemploymentNum = 0;
			_unemploymentNum = (int)(info._unemploymentNum * 2.4f);

			int _auditNum = 0;
			if (info._auditTotalNum > 0)
			{
				_auditNum = (int)(info._auditNum * (1 + 0.4f + info._auditNum / info._auditTotalNum));				
			}

			int _divorceNum = 0;
			if (info._divorceTotalNum > 0)
			{
				_divorceNum = (int)(info._divorceNum *(1+0.4f +info._divorceNum / info._divorceTotalNum));
			}

			int _moneyLoss = 0;

			if (info._moneyLossTotalNum > 0)
			{
				_moneyLoss = (int) (info._moneyLoss * (1 + 0.4f +info._moneyLoss / info._moneyLossTotalNum));
			}

			_chaoyueNumArr[0]=_learnNum;
			_chaoyueNumArr[1]=_healthNum;
			_chaoyueNumArr[2]=_unemploymentNum;
			_chaoyueNumArr[3]=_auditNum;
			_chaoyueNumArr[4]=_divorceNum;
			_chaoyueNumArr[5]=_moneyLoss;

			var tmplen = _chaoyueArr.Length;
			var tmpNum = 0;
			for (var i = 0; i < tmplen; i++)
			{
				tmpNum += _chaoyueNumArr [i];
			}
			_starNumChaoyue = _GetStarForChengzhang (tmpNum);
			_starChaoyue.SetScores (tmpNum,_starNumChaoyue);
		}


		/// <summary>
		/// Sets the EQ text.  品质指数 运用财富
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetYunyongText(PlayerInfo info)
		{
//			_txtSpiritualHealth.text = info._healthNum.ToString();
//			_txtSpiritualLearning.text = info._learnNum.ToString();
//			_txtEqQuality.text = info._qualityNum.ToString();
//			_txtEqRichLeisuret.text = info._richleisureNum.ToString();
						
			int _richleisureNum = 0;
			if (info._richleisureTotalNum > 0)
			{
				_richleisureNum=(int) (info._richleisureNum *(1+0.3f + info._richleisureNum / info._richleisureTotalNum) );
			}

			int _qualityNum = 0;
			if (info._qualityTotalNum > 0)
			{
				_qualityNum=(int) (info._qualityNum * (1+0.3f +info._qualityNum / info._qualityTotalNum) );				
			}


			int _charity = 0;

			if (info._charityTotalNum > 0)
			{
				_charity= (int) (info._charityNum * (1+0.3f +info._charityNum / info._charityTotalNum)) ;
			}
		

			_yunyongnumArr [0] = _richleisureNum;
			_yunyongnumArr [1]=_qualityNum;
			_yunyongnumArr [2]=_charity;

			var arrLen = _yunyongnumArr.Length;

			var tmpNum = 0;
			for (var i = 0; i < arrLen; i++)
			{
				tmpNum += _yunyongnumArr[i];
			}

			_starNumYunyong = _GetStarForPingzhi (tmpNum);
			_starYunyong.SetScores(tmpNum,_starNumYunyong);
		}
	
		/// <summary>
		/// Sets the financia text. 财商指数 创造财富
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetChuangzaoText(PlayerInfo info)
		{
//			_txtFinancialBig.text = info._bigOpportunitiesNum.ToString();
//			_txtFinancialSmall.text = info._smallOpportunitiesNum.ToString();
//			_txtFinancialSettlement.text = info._settlementNum.ToString();
//			_txtFinancialInvestment.text = info._investmentNum.ToString();

			int _bigOpportunitiesNum = 0;
			if (info._bigOpportunitiesTotalNum>0)
			{
				_bigOpportunitiesNum= (int)(info._bigOpportunitiesNum * (1+0.1f + info._bigOpportunitiesNum/info._bigOpportunitiesTotalNum) );
			}

			int _smallOpportunitiesNum = 0;
			if (info._smallOpportunitiesTotalNum>0)
			{
				_smallOpportunitiesNum= (int)(info._smallOpportunitiesNum * (1 +0.1f +info._smallOpportunitiesNum/info._smallOpportunitiesTotalNum));
			}

			int _settleOuter = 0;
			if(info._settleOuterNum>0)
			{
				_settleOuter= (int)(info._settleOuterNum * 2.1f);
			}		

			int _salenum = (int) (info._saleNums *2.1f) ;



			_chaungzaonumArr[0]=_bigOpportunitiesNum;
			_chaungzaonumArr[1]=_smallOpportunitiesNum;
			_chaungzaonumArr[2]=_salenum;
			_chaungzaonumArr[3]=_settleOuter;

			var arrLen = _chaungzaonumArr.Length;
			var tmpNum = 0;
			for (var i = 0; i < arrLen; i++)
			{
				tmpNum += _chaungzaonumArr [i];
			}

			_starNumChuangzao = _GetStarForCaishang (tmpNum);
			_starChuangzao.SetScores (tmpNum,_starNumChuangzao);
		}

		/// <summary>
		/// Sets the grow text. 成长指数
 		/// </summary>
		/// <param name="info">Info.</param>
		private void SetGrowText(PlayerInfo info)
		{
			
		}

		/// <summary>
		/// Sets the adversity text.  抗逆境指数  管理指数
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetGuanliText(PlayerInfo info)
		{
//			_txtAdversityUnemployment.text = info._unemploymentNum.ToString();
//			_txtAdversityDivorce.text = info._divorceNum.ToString();
//			_txtAdversityAudit.text = info._auditNum.ToString();
//			_txtEqChild.text = info.childNum.ToString();

			int _investmentNum = 0;
			if (info._investmentTotalNum > 0)
			{
				_investmentNum=(int)(info._investmentNum *(1+0.2f + info._investmentNum/ info._investmentTotalNum));
			}
			

			int _buyCareNum = (int)(info._buyCareNum * (2.2f)); 

			int _settleInnerNum = (int)(info._settleInnerNum * 2.2f);


			_guanlinumArr[0]=_investmentNum;
			_guanlinumArr[1]=_buyCareNum;
			_guanlinumArr[2]=_settleInnerNum;

			var tmpLen = _guanlinumArr.Length;
			var tmpNum = 0;
			for (var i = 0; i < tmpLen; i++)
			{
				tmpNum+=_guanlinumArr[i];				
			}

			_starNumGuanli = _GetStarForFengxian (tmpNum);
			_starGuanli.SetScores (tmpNum,_starNumGuanli);
		}


		/// <summary>
		/// Sets the score text.  设置总积分的
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetScoreText(PlayerInfo info)
		{
			info.eqIntegral = info._charityNum * 1 + info._childNum * 1 + info._richleisureNum * 1 + info._qualityNum * 1;
			info.spiritualIntegral = 20;
			info.adversityIntegral = info._unemploymentNum * 1 + info._auditNum * 1 + info._divorceNum * 1;
			info.financialIntegral = info._bigIntegral + info._smallIntegral + info._investmentIntegral + info._settlementNum * 1;
			int integral = info.eqIntegral * 20 + info.spiritualIntegral * 30 + info.adversityIntegral * 5 + info.financialIntegral * 40;
			_txtScore.text = integral.ToString();

			_starNumTotal = Mathf.FloorToInt((_starNumChuangzao + _starNumChaoyue + _starNumGuanli + _starNumYunyong) / 4f);

			if (_starNumTotal <= 1)
			{
				_starNumTotal = 1;
			}
			else if(_starNumTotal >=5)
			{
				_starNumTotal = 5;
			}
			letter = scoreList[_starNumTotal-1];
			_starPingjia.SetScores (integral,_starNumTotal);


		}

		/// <summary>
		/// Sets the game turns number. 设置游戏回合数
		/// </summary>
		/// <param name="info">Info.</param>
		private void SetGameTurnsNum(PlayerInfo info)
		{
			_txtBaseAge.text = info.Age.ToString();
		}


//		void chooseLetter(PlayerInfo playerInfor)
//		{
//			if(playerInfor.PlayerIntegral>=361)
//			{
//				tmplitter = "settlementa";
//				letter = scoreList[4];
////				_starNumTotal = 5;
//			}
//			else if(playerInfor.PlayerIntegral<361 && playerInfor.PlayerIntegral>=241)
//			{
//				tmplitter = "settlementb";
////				letter = scoreList[3];
////				_starNumTotal = 4;
//			}
//			else if(playerInfor.PlayerIntegral<241 && playerInfor.PlayerIntegral>=141)
//			{
//				tmplitter = "settlementc";
////				letter = scoreList[2];
////				_starNumTotal = 3;
//			}
//			else if(playerInfor.PlayerIntegral<141 && playerInfor.PlayerIntegral>=61)
//			{
//				tmplitter = "settlementd";
////				letter = scoreList[1];
////				_starNumTotal = 2;
//			}
//			else if(playerInfor.PlayerIntegral<61 && playerInfor.PlayerIntegral>=0)
//			{
//				tmplitter = "settlemente";
////				letter = scoreList[0];
////				_starNumTotal = 1;
//			}
//
//		}


		void chooseLetter(int starNum)
		{
			if(starNum==5)
			{
				tmplitter = "settlementa";
			}
			else if(starNum==4)
			{
				tmplitter = "settlementb";
			}
			else if(starNum==3)
			{
				tmplitter = "settlementc";
			}
			else if(starNum==2)
			{
				tmplitter = "settlementd";
			}
			else 
			{
				tmplitter = "settlemente";
			}

		}


		private void SetScoreImage(PlayerInfo playerInfor)
		{
			chooseLetter(_starNumTotal);

			var tmpPath = string.Format ("share/atlas/battle/letter/{0}.ab",tmplitter);

			WebManager.Instance.LoadWebItem (tmpPath, item => {
				using (item)
				{
					_imgLevel.sprite = item.sprite;
				}
			});	


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

		private void _DisposeText()
		{
			if (null != _starYunyong)
			{
				_starYunyong.Dispose ();
			}

			if (null != _starChaoyue)
			{
				_starChaoyue.Dispose ();
			}

			if (null != _starChuangzao)
			{
				_starChuangzao.Dispose ();
			}

			if(null!=_starGuanli)
			{
				_starGuanli.Dispose ();
			}

			if(null !=_starPingjia)
			{
				_starYunyong.Dispose ();
			}

			if (null != _starTip)
			{
				_starTip.Dispose ();
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

		private Text _txtTitle;

		//基础信息
		private Text _txtBaseZhiYe;
		private Text _txtBaseAge;
		private Text _txtBaseFeiLaoWu;
		private Text _txtBaseCash ;
		private Text _txtBaseAssets;
		private Text _txtBaseLiabilities;
		private Text _txtBaseShouRu;
		private Text _txtBaseZhiChu;

		//情商指数
//		private Text _txtEqCharity;
//		private Text _txtEqChild;
//		private Text _txtEqQuality;
//		private Text _txtEqRichLeisuret;

		//灵性指数
//		private Text _txtSpiritualOuterTime;
//		private Text _txtSpiritualOuterQuality;
//		private Text _txtSpiritualInnerQuality;
//		private Text _txtSpiritualHealth;
//		private Text _txtSpiritualLearning;
//		private Text _txtSpiritualInnerTime;

		private Text _txtSpiritualEndGame;
		private Text _txtSpiritualIntoinner;

		//逆境指数
//		private Text _txtAdversityDivorce;
//		private Text _txtAdversityAudit;
//		private Text _txtAdversityUnemployment;

		//财商指数
//		private Text _txtFinancialBig;
//		private Text _txtFinancialSmall;
//		private Text _txtFinancialSettlement;
//		private Text _txtFinancialInvestment;

		private Image _imgLevel;
		private Text _txtEvaluation;
		private Text _txtScore;


		private static float timeScore;
		private static float qualityScore;
		private static string timeEnterInner;

		private string tmplitter="settlemente";
		private string letter;
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



		private UIConclusionStarBarItem _starYunyong;
		private UIConclusionStarBarItem _starChaoyue;
		private UIConclusionStarBarItem _starChuangzao;
		private UIConclusionStarBarItem _starGuanli;
		private UIConclusionStarBarItem _starPingjia;

		private UIConclusionStarTip _starTip;
	}
}
