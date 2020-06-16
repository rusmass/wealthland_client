using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public partial class UIChooseRoleNetWindow
	{
		private void _OnInitText(GameObject go)
		{
			_txtAge = go.GetComponentEx<Text>(Layout.txt_age);
			_txtZhiYe = go.GetComponentEx<Text>(Layout.txt_zhiye);
			_txtShouRu = go.GetComponentEx<Text>(Layout.txt_shouru);
			_txtZhiChu = go.GetComponentEx<Text>(Layout.txt_zhichu);
			_txtXianJin = go.GetComponentEx<Text>(Layout.txt_xianjin);
			_txtShuoMing = go.GetComponentEx<Text>(Layout.txt_shuoming);

			_txtBrank = go.GetComponentEx<Text>(Layout.txt_brank);
			_txtWage  = go.GetComponentEx<Text>(Layout.txt_wage);

			_txtFuZhaiHouse = go.GetComponentEx<Text>(Layout.txt_fuzhai_house);
			_txtFuZhaiJiaoYu = go.GetComponentEx<Text>(Layout.txt_fuzhai_jiaoyu);
			_txtFuZhaiCar = go.GetComponentEx<Text>(Layout.txt_fuzhai_car);
			_txtFuZhaiCard = go.GetComponentEx<Text>(Layout.txt_fuzhai_card);
			_txtFuZhaiAdditional = go.GetComponentEx<Text>(Layout.txt_fuzhai_additional);
			_txtFuZhaiEstate = go.GetComponentEx<Text>(Layout.txt_fuzhai_estate); 


			_txtZhiChuHouse = go.GetComponentEx<Text>(Layout.txt_zhichu_house);
			_txtZhiChuJiaoYu = go.GetComponentEx<Text>(Layout.txt_zhichu_jiaoyu);
			_txtZhiChuCar = go.GetComponentEx<Text>(Layout.txt_zhichu_car);
			_txtZhiChuCard = go.GetComponentEx<Text>(Layout.txt_zhichu_card);
			_txtZhiChuAdditional = go.GetComponentEx<Text>(Layout.txt_zhichu_additional);
			_txtZhiChuQiTa  = go.GetComponentEx<Text>(Layout.txt_zhichu_qita);
			_txtZhiChuMortgage  = go.GetComponentEx<Text>(Layout.txt_zhichu_mortgage);

			_txtZongZhiChu = go.GetComponentEx<Text>(Layout.txt_zongzhichu);
			_txtZongShouRu = go.GetComponentEx<Text>(Layout.txt_zongshouru);

			_lbLeftTime = go.GetComponentEx<Text> (Layout.lb_lefttime);

		}


		public void _OnShowHeroInfor(PlayerInitData value)
		{
			//年龄
			_txtAge.text = value.initAge.ToString();
			//职业
			_txtZhiYe.text = value.careers;
			//总收入
			_txtShouRu.text = value.cashFlow.ToString ();
			//总支出
			var totalPay = value.cardDebt + value.carLoan + value.educationLoan + value.houseMortgages + value.otherSpend + value.additionalDebt + value.fixTax;
			_txtZhiChu.text = totalPay.ToString();
			//现金
			var income = value.cashFlow - totalPay;
			_txtXianJin.text = income.ToString();
			//职业说明
			_txtShuoMing.text =value.infor;

			//银行存款
			_txtBrank.text = value.fixBankSaving.ToString();
			//工资
			_txtWage.text = value.cashFlow.ToString();
			//住房负债
			_txtFuZhaiHouse.text = value.fixHouseMortgages.ToString();
			//教育负债
			_txtFuZhaiJiaoYu.text = value.fixEducationLoan.ToString();
			//车贷负债
			_txtFuZhaiCar.text = value.fixCarLoan.ToString();
			//信用卡负债
			_txtFuZhaiCard.text = value.fixCardDebt.ToString();
			//额外负债
			_txtFuZhaiAdditional.text = value.fixAdditionalDebt.ToString();
			//房产抵押负债
			_txtFuZhaiEstate.text = value.fixHouseMortgages.ToString();

			//住房支出
			_txtZhiChuHouse.text = value.houseMortgages.ToString();
			//教育支出
			_txtZhiChuJiaoYu.text = value.educationLoan.ToString();
			//车贷支出
			_txtZhiChuCar.text = value.carLoan.ToString();
			//信用卡支出
			_txtZhiChuCard.text = value.cardDebt.ToString();
			//额外支出
			_txtZhiChuAdditional.text = value.additionalDebt.ToString();
			//其他支出
			_txtZhiChuQiTa.text = value.otherSpend.ToString();
			//银行抵押支出
			_txtZhiChuMortgage.text = value.fixTax.ToString();

			//总支出
			_txtZongZhiChu.text = totalPay.ToString();
			//总收入
			_txtZongShouRu.text = value.cashFlow.ToString ();
		}

		/// <summary>
		/// Updates the time handler. 更新时间
		/// </summary>
		/// <param name="deltaTime">Delta time.</param>
		public void UpdateTimeHandler(float deltaTime)
		{
			if (isAutoSelect == true)
			{
				return;
			}

			if (isTimeSelect == false)
			{
				_selectTime += deltaTime;
				if (_selectTime >= _selectTimeMax)
				{
					_selectTime = 0;
					isTimeSelect = true;
				}
			}

			if (_leftTime > 0)
			{
				_leftTime -= deltaTime;
				if (null != _lbLeftTime)
				{
					_lbLeftTime.text ="00:"+ GetTime(_leftTime);
				}

			}
			else
			{
				if (null != _lbLeftTime)
				{
					_lbLeftTime.text ="00:00";
				}
				if (isAutoSelect == false && isReady==false)
				{
					isAutoSelect = true;
					isReady = true;
					_btnStart.GetComponent<Image>().color=_grayColor;
					NetWorkScript.getInstance ().NetAutoSelectRole ();

				}
			}
		}

		private string GetTime(float time)
		{
			return HandleNumToTimeTool.ChangeNumberToTime (time);
		}

		private string GetSecond(float time)
		{
			int timer = (int)((time % 3600) % 60);
			string timerStr;
			if (timer < 10)
				timerStr = "0" + timer.ToString();
			else
				timerStr = timer.ToString();

			return timerStr;
		}

		private float _leftTime=61f;
		private Text _lbLeftTime;
		private bool isAutoSelect=false;


		private bool isTimeSelect=true;
		private float _selectTime=0;
		private float _selectTimeMax=1;







		//基本信息
		private Text _txtAge;
		private Text _txtZhiYe;
		private Text _txtShouRu;
		private Text _txtZhiChu;
		private Text _txtXianJin;
		private Text _txtShuoMing;

		//详细信息
		private Text _txtBrank;
		private Text _txtWage;

		private Text _txtFuZhaiHouse;
		private Text _txtFuZhaiJiaoYu;
		private Text _txtFuZhaiCar;
		private Text _txtFuZhaiCard;
		private Text _txtFuZhaiAdditional;
		private Text _txtFuZhaiEstate;

		private Text _txtZhiChuHouse;
		private Text _txtZhiChuJiaoYu;
		private Text _txtZhiChuCar;
		private Text _txtZhiChuCard;
		private Text _txtZhiChuAdditional;
		private Text _txtZhiChuQiTa;
		private Text _txtZhiChuMortgage;

		private Text _txtZongZhiChu;
		private Text _txtZongShouRu;


	}
}

