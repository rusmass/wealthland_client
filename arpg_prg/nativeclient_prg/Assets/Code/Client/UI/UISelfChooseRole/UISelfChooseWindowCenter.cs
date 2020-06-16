using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
    public partial class UISelfChooseWindow
    {
        public void _OnInitCenter(GameObject go)
        {
            this.btn_closeScene = go.GetComponentEx<Button>(Layout.btn_closeMain);
            this.btn_modifyIncome = go.GetComponentEx<Button>(Layout.btn_modifyIncome);
            this.btn_modifyDebt = go.GetComponentEx<Button>(Layout.btn_modifyDebt);
            this.btn_sureGame = go.GetComponentEx<Button>(Layout.btn_sureGame);

            this.img_debt = go.GetComponentEx<Image>(Layout.img_debt);
            this.img_income = go.GetComponentEx<Image>(Layout.img_income);

            this.txt_flow = go.GetComponentEx<Text>(Layout.txt_flow);
            this.txt_totalPay = go.GetComponentEx<Text>(Layout.txt_totalPay);
            this.txt_totalIncome = go.GetComponentEx<Text>(Layout.txt_totalIncome);

            this.input_career = go.GetComponentEx<InputField>(Layout.input_career);
        }

        public void _OnShowCenter()
        {


            EventTriggerListener.Get(this.btn_closeScene.gameObject).onClick +=_OnCloseMainHandler;
            EventTriggerListener.Get(this.btn_modifyDebt.gameObject).onClick +=_OnModifyDebtHandler;
            EventTriggerListener.Get(this.btn_modifyIncome.gameObject).onClick +=_OnModifyIncomeHandler;
            EventTriggerListener.Get(this.btn_sureGame.gameObject).onClick +=_OnSureGameHandler;

            input_career.onEndEdit.AddListener(_OnInputCareerEnded);
            this._initCenterData();
        }



        public void _OnHideCenter()
        {
            EventTriggerListener.Get(this.btn_closeScene.gameObject).onClick -= _OnCloseMainHandler;
            EventTriggerListener.Get(this.btn_modifyDebt.gameObject).onClick -= _OnModifyDebtHandler;
            EventTriggerListener.Get(this.btn_modifyIncome.gameObject).onClick -= _OnModifyIncomeHandler;
            EventTriggerListener.Get(this.btn_sureGame.gameObject).onClick -= _OnSureGameHandler;

            input_career.onEndEdit.RemoveListener(_OnInputCareerEnded);
            
        }

        /// <summary>
        /// 初始化玩家的数据
        /// </summary>
        private void _initCenterData()
        {
            this.newInfor = _controller.GetPlayerInfor();
            this.newInfor.additionalPay = 0;
            this.newInfor.fixAdditionalDebt = 0;            

            this._SetTotalIncome(0);
            this._SetTotalDebt(0);
            //this.txt_totalIncome.text = "0";
            //this.txt_totalPay.text = "0";

            _SetInitPlayerData();
            _InitPlayerDebtData();

        }

        /// <summary>
        /// 设置玩家初始信息
        /// </summary>
        private void _SetInitPlayerData()
        {
            this.input_career.text = this.newInfor.careers;
            this._UpdateIncomeAndPay();

            this.input_wages.text = this.newInfor.cashFlow.ToString();
            this.txt_income_total.text = this.newInfor.cashFlow.ToString();

            

        }

        /// <summary>
        /// 设置玩家的总收入
        /// </summary>
        /// <param name="value"></param>
        private void _SetTotalIncome( float value )
        {
            this.txt_totalIncome.text = value.ToString();
        }

        /// <summary>
        /// 设置玩家的总支出
        /// </summary>
        /// <param name="value"></param>
        private void _SetTotalDebt(float value)
        {
            this.txt_totalPay.text = value.ToString();
        }

        /// <summary>
        /// 玩家输入职业结束
        /// </summary>
        /// <param name="value"></param>
        private void _OnInputCareerEnded(string value)
        {
            if(value=="")
            {
                this.newInfor.careers = "";
                return;
            }
            this.newInfor.careers = value;
            //Console.Error.WriteLine("玩家的职业：" + this.newInfor.careers);
        }

        /// <summary>
        /// 刷新收入和支出的逻辑
        /// </summary>
        private void _UpdateIncomeAndPay()
        {
            var tmpIncome = this._TotalIncome();
            var tmpPay = this._MonthPay();

            txt_totalIncome.text = tmpIncome.ToString();
            txt_totalPay.text = tmpPay.ToString();
            //Console.Error.WriteLine("当前的流动现金的值是:"+tmpIncome.ToString()+"----:" + tmpPay.ToString());
            txt_flow.text =(tmpIncome- tmpPay).ToString();
            
        }


        /// <summary>
        /// 关闭主界面
        /// </summary>
        /// <param name="go"></param>
        private void _OnCloseMainHandler(GameObject go)
        {
            this._controller.setVisible(false);
        }
        /// <summary>
        /// 修改收入的方法
        /// </summary>
        /// <param name="go"></param>
        private void _OnModifyIncomeHandler(GameObject go)
        {
            this.img_income.SetActiveEx(true);
        }

        /// <summary>
        /// 修改支出
        /// </summary>
        /// <param name="go"></param>
        private void _OnModifyDebtHandler(GameObject go)
        {
            this.img_debt.SetActiveEx(true);
        }

        private void _OnSureGameHandler(GameObject go)
        {
            
            Audio.AudioManager.Instance.BtnMusic();

            if (this.newInfor.careers == "")
            {
                MessageHint.Show("请输入您的职业");
                return;
            }

            if(this._MonthPay()>this._TotalIncome())
            {
                MessageHint.Show("您的总支出大于了您的收入，请重新输入");
                return;
            }


                this._DownNewPlayerGuid();
            Audio.AudioManager.Instance.Stop();
            if (null != _controller)
            {
                var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
                controller.setVisible(true);
                controller.LoadBattleUI();

                var controller2 = Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
                controller2.setVisible(true);
            }

            this._controller.setVisible(false);

            PlayerManager.Instance.SetPlayerHero(this.newInfor, _controller.GetChooseIndex(),GameModel.GetInstance.myHandInfor.nickName);
        }

        private void _DownNewPlayerGuid()
        {
            if (GameGuidManager.GetInstance.DoneGameSelect == false)
            {
                GameGuidManager.GetInstance.DoneGameSelect = true;
            }
        }

        #region 用到的数据
        /// <summary>
        /// 新角色的信息
        /// </summary>
        private PlayerInitData newInfor =new PlayerInitData();

        /// <summary>
        /// 返回玩家总是收入
        /// </summary>
        /// <returns></returns>
        private int _TotalIncome()
        {
            return this.newInfor.cashFlow;
        }

        /// <summary>
        /// 返回玩家的每月支出
        /// </summary>
        /// <returns></returns>
        private int _MonthPay()
        {
            return   this.newInfor.taxPay+this.newInfor.housePay+this.newInfor.educationPay+this.newInfor.carPay+this.newInfor.cardPay+this.newInfor.nessPay;
        }
        
        
        #endregion


        #region UI参数
        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button btn_closeScene;
        /// <summary>
        /// 职业
        /// </summary>
        private InputField input_career;
        /// <summary>
        /// 总收入
        /// </summary>
        private Text txt_totalIncome;
        /// <summary>
        /// 总支出
        /// </summary>
        private Text txt_totalPay;
        /// <summary>
        /// 修改收入
        /// </summary>
        private Button btn_modifyIncome;
        /// <summary>
        /// 修改支出
        /// </summary>
        private Button btn_modifyDebt;
        /// <summary>
        /// 流动现金
        /// </summary>
        private Text txt_flow;
        /// <summary>
        /// 确定进入游戏
        /// </summary>
        private Button btn_sureGame;
        /// <summary>
        /// 收入的面板
        /// </summary>
        private Image img_income;

        /// <summary>
        /// 负债支出面板
        /// </summary>
        private Image img_debt;

#endregion


    }

}
