using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metadata;

namespace Client.UI
{
    public class UISelfChooseController:UIController<UISelfChooseWindow,UISelfChooseController>
    {
        protected override string _windowResource
        {
            get
            {
                return "prefabs/ui/scene/selfcreaterole.ab";
            }
        }

        /// <summary>
        /// 设置玩家的基本数据、 选择的索引值
        /// </summary>
        /// <param name="initdata"></param>
        /// <param name="index"></param>
        public void SetPlayerInfo(PlayerInitData initdata,int index)
        {
            _initPlayerData(initdata);
            //_playerData = initdata;
            //_playerInfor.SetPlayerInitData(initdata);
            _chooseIndex = index;
        }

        private void _initPlayerData(PlayerInitData data)
        {
            _playerData = new PlayerInitData();

            _playerData.id = data.id;
            _playerData.playName = data.playName;
            _playerData.headPath = data.headPath;
            _playerData.playerImgPath = data.playerImgPath;

            _playerData.oneChildPrise = data.oneChildPrise;
            _playerData.careers = data.careers;
            _playerData.initAge = data.initAge;
            _playerData.modelResID = data.modelResID;

            _playerData.fixBankSaving = data.fixBankSaving;
            _playerData.cashFlow= data.cashFlow;
            _playerData.fixHouseDebt = data.fixHouseDebt;
            _playerData.fixEducationDebt = data.fixEducationDebt;
            _playerData.fixCarDebt = data.fixCarDebt;
            _playerData.fixCardDebt = data.fixCardDebt;
            _playerData.fixAdditionalDebt = data.fixAdditionalDebt;
            _playerData.taxPay = data.taxPay;
            _playerData.housePay = data.housePay;
            _playerData.educationPay = data.educationPay;
            _playerData.carPay = data.carPay;
            _playerData.cardPay = data.cardPay;
            _playerData.additionalPay = data.additionalPay;
            _playerData.nessPay = data.nessPay;
            _playerData.infor= data.infor;
            _playerData.modelPath= data.modelPath;
            _playerData.playerSex= data.playerSex;
            _playerData.playerGift= data.playerGift;
    }

        /// <summary>
        /// 获取游戏的玩家信息
        /// </summary>
        /// <returns></returns>
        public PlayerInitData GetPlayerInfor()
        {
            return _playerData;
        }

        /// <summary>
        /// 返回选择玩家的索引值
        /// </summary>
        /// <returns></returns>
        public int GetChooseIndex()
        {
            return _chooseIndex;
        }


        /// <summary>
        /// 记录传值过来的索引值
        /// </summary>
        private int _chooseIndex;

        /// <summary>
        /// 
        /// </summary>
        private PlayerInitData _playerData;

             
    }
}
