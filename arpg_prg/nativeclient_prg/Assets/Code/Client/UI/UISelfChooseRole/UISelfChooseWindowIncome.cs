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
        private void _OnInitIncome(GameObject go)
        {
            btn_closeIncome = go.GetComponentEx<Button>(Layout.btn_closeincome);
            btn_sureIncome = go.GetComponentEx<Button>(Layout.btn_sureincome);

            input_wages = go.GetComponentEx<InputField>(Layout.input_wages);
            txt_income_total = go.GetComponentEx<Text>(Layout.txt_income_totalincome);

        }

        private void _OnShowIncome()
        {
            EventTriggerListener.Get(btn_closeIncome.gameObject).onClick += _OnCloseIncomeHandler;
            EventTriggerListener.Get(btn_sureIncome.gameObject).onClick += _OnSureIncomeHandler;
            input_wages.onEndEdit.AddListener(_OnEndWagesHandler);
        }

        private void _OnHideIncome()
        {
            EventTriggerListener.Get(btn_closeIncome.gameObject).onClick -= _OnCloseIncomeHandler;
            EventTriggerListener.Get(btn_sureIncome.gameObject).onClick -= _OnSureIncomeHandler;
            input_wages.onEndEdit.RemoveListener(_OnEndWagesHandler);
        }

        /// <summary>
        /// 结束输入工资
        /// </summary>
        /// <param name="value"></param>
        private void _OnEndWagesHandler(string value)
        {
            var tmpwage = 0;
            if(value!="")
            {
                tmpwage = Math.Abs( int.Parse(value));
            }
            this.newInfor.cashFlow = tmpwage;
            this._SetWages(this.newInfor.cashFlow);          
        }

        /// <summary>
        /// 设置工资
        /// </summary>
        /// <param name="value"></param>
        private void  _SetWages(int value)
        {
            this.input_wages.text = value.ToString();
            this.txt_income_total.text = value.ToString();            
        }

        /// <summary>
        /// 关闭收入面板
        /// </summary>
        /// <param name="go"></param>
        private void _OnCloseIncomeHandler(GameObject go)
        {
            this.img_income.SetActiveEx(false);
            this._UpdateIncomeAndPay();
        }


        /// <summary>
        /// 确定修改收入
        /// </summary>
        /// <param name="go"></param>
        private void _OnSureIncomeHandler(GameObject go)
        {

        }

        /// <summary>
        /// 工资输入框
        /// </summary>
        private InputField input_wages;
        /// <summary>
        /// 收入面板总收入
        /// </summary>
        private Text txt_income_total;
        /// <summary>
        /// 关闭收入面板
        /// </summary>
        private Button btn_closeIncome;
        /// <summary>
        /// 确定修改
        /// </summary>
        private Button btn_sureIncome;




    }
}
