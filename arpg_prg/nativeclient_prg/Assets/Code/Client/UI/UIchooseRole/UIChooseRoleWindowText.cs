﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;


namespace Client.UI
{
	public partial class UIChooseRoleWindow
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

            _txtGift = go.GetComponentEx<Text>(Layout.txt_gift);

            //			_txtRoleName = go.GetComponentEx<InputField>(Layout.filed_name);
            //
            //			_txtRoleName.onEndEdit.AddListener(delegate {
            //				onSubmit();
            //			});


        }


		void onSubmit()
		{
//			_txtRoleName.text = _txtRoleName.text.Substring(0,4);
		}
		
        /// <summary>
        /// 设置角色信息
        /// </summary>
        /// <param name="value"></param>
		public void _OnShowHeroInfor(PlayerInitData value)
		{
			//年龄
			_txtAge.text = value.initAge.ToString();
			//职业
			_txtZhiYe.text = value.careers;
			//总收入
			_txtShouRu.text = value.cashFlow.ToString ();
			//总支出
			var totalPay = value.cardPay + value.carPay + value.educationPay + value.housePay + value.nessPay + value.additionalPay + value.taxPay;
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
			_txtFuZhaiHouse.text = value.fixHouseDebt.ToString();
			//教育负债
			_txtFuZhaiJiaoYu.text = value.fixEducationDebt.ToString();
			//车贷负债
			_txtFuZhaiCar.text = value.fixCarDebt.ToString();
			//信用卡负债
			_txtFuZhaiCard.text = value.fixCardDebt.ToString();
			//额外负债
			_txtFuZhaiAdditional.text = value.fixAdditionalDebt.ToString();
			//房产抵押负债
			_txtFuZhaiEstate.text = value.fixHouseDebt.ToString();

			//住房支出
			_txtZhiChuHouse.text = value.housePay.ToString();
			//教育支出
			_txtZhiChuJiaoYu.text = value.educationPay.ToString();
			//车贷支出
			_txtZhiChuCar.text = value.carPay.ToString();
			//信用卡支出
			_txtZhiChuCard.text = value.cardPay.ToString();
			//额外支出
			_txtZhiChuAdditional.text = value.additionalPay.ToString();
			//其他支出
			_txtZhiChuQiTa.text = value.nessPay.ToString();
			//银行抵押支出
			_txtZhiChuMortgage.text = value.taxPay.ToString();

			//总支出
			_txtZongZhiChu.text = totalPay.ToString();
			//总收入
			_txtZongShouRu.text = value.cashFlow.ToString ();

            _txtGift.text = value.playerGift;
        }

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

        /// <summary>
        /// 玩家天赋服说明文本
        /// </summary>
        private Text _txtGift;

        //		private InputField _txtRoleName;
    }
}