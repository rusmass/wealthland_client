using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    public partial class UISelfChooseWindow
    {

        private void _OnInitDebt(GameObject go)
        {
            this.input_payTax = go.GetComponentEx<InputField>(Layout.input_payTax);
            this.input_payHouse = go.GetComponentEx<InputField>(Layout.input_payhouse);
            this.input_payEduc = go.GetComponentEx<InputField>(Layout.input_payeduc);
            this.input_payCar = go.GetComponentEx<InputField>(Layout.input_paycar);
            this.input_payCard = go.GetComponentEx<InputField>(Layout.input_paycard);
            this.input_payNess = go.GetComponentEx<InputField>(Layout.input_payness);

            this.input_payChild = go.GetComponentEx<InputField>(Layout.input_paychild);

            this.input_DebtHouse = go.GetComponentEx<InputField>(Layout.input_debthouse);
            this.input_DebtEduc = go.GetComponentEx<InputField>(Layout.input_debteduc);
            this.input_DebtCar = go.GetComponentEx<InputField>(Layout.input_debtcar);
            this.input_DebtCard = go.GetComponentEx<InputField>(Layout.input_debtcard);

            this.btn_closeDebt = go.GetComponentEx<Button>(Layout.btn_closepay);
            this.txt_debt_totaldebt = go.GetComponentEx<Text>(Layout.txt_pay_totalpay);

        }

        private void _OnShowDebt()
        {
            EventTriggerListener.Get(this.btn_closeDebt.gameObject).onClick += _OnCloseDebt;
            input_payTax.onEndEdit.AddListener(_OnModifyPayTax);
            input_payHouse.onEndEdit.AddListener(_OnModifyPayHouse);
            input_payEduc.onEndEdit.AddListener(_OnModifyPayEduc);
            input_payCar.onEndEdit.AddListener(_OnModifyPayCar);
            input_payCard.onEndEdit.AddListener(_OnModifyPayCard);
            input_payNess.onEndEdit.AddListener(_OnModifyPayNess);
            input_payChild.onEndEdit.AddListener(_OnModifyPayChild);

            input_DebtHouse.onEndEdit.AddListener(_OnModifyDebtHouse);
            input_DebtEduc.onEndEdit.AddListener(_OnModifyDebtEduc);
            input_DebtCar.onEndEdit.AddListener(_OnModifyDebtCar);
            input_DebtCard.onEndEdit.AddListener(_OnModifyDebtCard);

            _UpdateTotalPay();
        }

        private void _OnHideDebt()
        {
            EventTriggerListener.Get(this.btn_closeDebt.gameObject).onClick -= _OnCloseDebt;
            input_payTax.onEndEdit.RemoveListener(_OnModifyPayTax);
            input_payHouse.onEndEdit.RemoveListener(_OnModifyPayHouse);
            input_payEduc.onEndEdit.RemoveListener(_OnModifyPayEduc);
            input_payCar.onEndEdit.RemoveListener(_OnModifyPayCar);
            input_payCard.onEndEdit.RemoveListener(_OnModifyPayCard);
            input_payNess.onEndEdit.RemoveListener(_OnModifyPayNess);
            input_payChild.onEndEdit.RemoveListener(_OnModifyPayChild);

            input_DebtHouse.onEndEdit.RemoveListener(_OnModifyDebtHouse);
            input_DebtEduc.onEndEdit.RemoveListener(_OnModifyDebtEduc);
            input_DebtCar.onEndEdit.RemoveListener(_OnModifyDebtCar);
            input_DebtCard.onEndEdit.RemoveListener(_OnModifyDebtCard);
        }

        private void _UpdateTotalPay()
        {           
            this.txt_debt_totaldebt.text =this._MonthPay().ToString();
        }
             

        private void _OnCloseDebt(GameObject go)
        {
            this.img_debt.SetActiveEx(false);           
            this._UpdateIncomeAndPay();
        }

        private void _InitPlayerDebtData()
        {
            input_payTax.text= this.newInfor.taxPay.ToString();
            input_payHouse.text = this.newInfor.housePay.ToString();
            input_payEduc.text = this.newInfor.educationPay.ToString();
            input_payCar.text = this.newInfor.carPay.ToString();
            input_payCard.text = this.newInfor.cardPay.ToString();
            input_payNess.text = this.newInfor.nessPay.ToString();
            input_payChild.text = this.newInfor.oneChildPrise.ToString();

            input_DebtHouse.text = this.newInfor.fixHouseDebt.ToString();
            input_DebtEduc.text = this.newInfor.fixEducationDebt.ToString();
            input_DebtCar.text = this.newInfor.fixCarDebt.ToString();
            input_DebtCard.text = this.newInfor.fixCardDebt.ToString();
        }

        /// <summary>
        /// 修改税金
        /// </summary>
        /// <param name="value"></param>
        private void _OnModifyPayTax(string value)
        {
            var tmpTax = 0;
            if(""!=value)
            {
                tmpTax = Math.Abs(int.Parse(value));
            }
            
            input_payTax.text = tmpTax.ToString();            
            this.newInfor.taxPay = tmpTax;
            _UpdateTotalPay();
            //_SetPayTax(tmpTax);
        }

        /// <summary>
        /// 修改住房支出
        /// </summary>
        /// <param name="value"></param>
        private void _OnModifyPayHouse(string value)
        {
            var tmpPayHouse = 0;
            if(""!= value)
            {
                tmpPayHouse = Math.Abs(int.Parse(value));
            }
            input_payHouse.text = tmpPayHouse.ToString();
            this.newInfor.housePay = tmpPayHouse;           
            _UpdateTotalPay();
        }        

        /// <summary>
        /// 修改教育支出
        /// </summary>
        /// <param name="value"></param>
        private void _OnModifyPayEduc(string value)
        {
            var tmpPayEduc = 0;
            if(""!=value)
            {
                tmpPayEduc = Math.Abs(int.Parse(value));
            }
            
            input_payEduc.text = tmpPayEduc.ToString();            
            this.newInfor.educationPay = tmpPayEduc;
            _UpdateTotalPay();
        }
        /// <summary>
        /// 修改购车支出
        /// </summary>
        /// <param name="value"></param>
        private void _OnModifyPayCar(string value)
        {
            var tmpPayCar = 0;
            if(""!=value)
            {
                tmpPayCar = Math.Abs(int.Parse(value));
            }
           
           input_payCar.text = tmpPayCar.ToString();           
            this.newInfor.carPay = tmpPayCar;
            _UpdateTotalPay();
        }
        /// <summary>
        /// 修改银行卡支出
        /// </summary>
        /// <param name="value"></param>
        private void _OnModifyPayCard(string value)
        {
            var tmpPayCard = 0;
            if(""!=value)
            {
                tmpPayCard = Math.Abs(int.Parse(value));
            }
           
            input_payCard.text = tmpPayCard.ToString();
           
            this.newInfor.cardPay = tmpPayCard;
            _UpdateTotalPay();
        }

        /// <summary>
        /// 修改生活必备支出
        /// </summary>
        /// <param name="value"></param>
        private void _OnModifyPayNess(string value)
        {
            var tmpPayNess = 0;
            if(""!=value)
            {
                tmpPayNess= Math.Abs(int.Parse(value));
            }
            
            input_payNess.text = tmpPayNess.ToString();            
            this.newInfor.nessPay = tmpPayNess;
            _UpdateTotalPay();
        }

        private void _OnModifyPayChild(string value)
        {
            if(""== value)
            {
                //MessageHint.Show("生孩子费用不能为空");
                return;
            }

            this.newInfor.oneChildPrise = Math.Abs(int.Parse(value));
            _UpdateTotalPay();
        }

        /// <summary>
        /// 修改住房抵押贷款
        /// </summary>
        /// <param name="value"></param>
        private void _OnModifyDebtHouse(string value)
        {
            var tmpDebtHouse = 0;
            if(value!="")
            {
                tmpDebtHouse= Math.Abs(int.Parse(value));                                
            }
           
            this.input_DebtHouse.text = tmpDebtHouse.ToString();            
            this.newInfor.fixHouseDebt = tmpDebtHouse;
            _UpdateTotalPay();
        }


        /// <summary>
        /// 修改教育抵押贷款
        /// </summary>
        /// <param name="value"></param>
        private void _OnModifyDebtEduc(string value)
        {
            var tmpDebtEduc = 0;

            if (value != "")
            {
                tmpDebtEduc= Math.Abs(int.Parse(value));
            }
            
            input_DebtEduc.text = tmpDebtEduc.ToString();
            this.newInfor.fixEducationDebt = tmpDebtEduc;
            _UpdateTotalPay();
        }

        /// <summary>
        /// 修改购车抵押贷款
        /// </summary>
        /// <param name="value"></param>
        private void _OnModifyDebtCar(string value)
        {
            var tmpDebtCar = 0;

            if(""!=value)
            {
                tmpDebtCar= Math.Abs(int.Parse(value));
            }
            
            input_DebtCar.text = tmpDebtCar.ToString();
            
            this.newInfor.fixCarDebt = tmpDebtCar; 
            _UpdateTotalPay();
        }

        /// <summary>
        /// 修改信用卡抵押贷款
        /// </summary>
        /// <param name="value"></param>
        private void _OnModifyDebtCard(string value)
        {
            var tmpDebtCard = 0;
            if(""!=value)
            {
                tmpDebtCard= Math.Abs(int.Parse(value));
            }
            
            this.input_DebtCard.text = tmpDebtCard.ToString();
            this.newInfor.fixCardDebt = tmpDebtCard;
            _UpdateTotalPay();
        }


        /// <summary>
        /// 关闭负债面板
        /// </summary>
        private Button btn_closeDebt;
        /// <summary>
        /// 确定负债支出
        /// </summary>
        private Button btn_sureDebt;
        
        /// <summary>
        /// 税金
        /// </summary>
        private InputField input_payTax;
        /// <summary>
        /// 住房抵押贷款利息
        /// </summary>
        private InputField input_payHouse;
        /// <summary>
        /// 每月的教育支出
        /// </summary>
        private InputField input_payEduc;
        /// <summary>
        /// 每月的购车支出
        /// </summary>
        private InputField input_payCar;
        /// <summary>
        /// 每月的信用卡支出
        /// </summary>
        private InputField input_payCard;
        /// <summary>
        /// 每月的生活必要支出
        /// </summary>
        private InputField input_payNess;
        /// <summary>
        /// 生孩子支出
        /// </summary>
        private InputField input_payChild;

        /// <summary>
        /// 住房抵押贷款
        /// </summary>
        private InputField input_DebtHouse;
        /// <summary>
        /// 教育金贷款
        /// </summary>
        private InputField input_DebtEduc;
        /// <summary>
        /// 购车抵押贷款
        /// </summary>
        private InputField input_DebtCar;
        /// <summary>
        /// 信用卡抵押贷款
        /// </summary>
        private InputField input_DebtCard;
        /// <summary>
        /// 总支出
        /// </summary>
        private Text txt_debt_totaldebt;
    }
}
