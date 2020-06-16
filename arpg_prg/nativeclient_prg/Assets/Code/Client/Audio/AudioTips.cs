using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metadata;

namespace Audio
{
    public partial class AudioManager
    {
        //////不同玩家的配音效果

        /// <summary>
        /// 提示音乐的路径share/audio/bgm.ab
        /// </summary>
        private readonly string audioTipPath = "share/audio/tips/{0}/{1}";
        /// <summary>
        /// 提示购买保险
        /// </summary>
        private readonly string _buyEnsurence = "buyinsurance.ab";

        /// <summary>
        /// 可以还款
        /// </summary>
        private readonly string _canPayBack = "canpayback.ab";

        /// <summary>
        /// 机会卡牌提示音
        /// </summary>
        private readonly string _chance = "chance.ab";
        /// <summary>
        /// 结账日提示
        /// </summary>
        private readonly string _checkday = "checkday.ab";
        /// <summary>
        /// 命运提示音
        /// </summary>
        private readonly string _fateTip = "fate.ab";

        /// <summary>
        /// 生孩子
        /// </summary>
        private readonly string _giveChild = "givechild.ab";
        /// <summary>
        /// 投资失败
        /// </summary>
        private readonly string _investmentLose = "investmentlose.ab";
        /// <summary>
        /// 投资成功
        /// </summary>
        private readonly string _investmentWin = "investmentwin.ab";
        /// <summary>
        ///  品质生活
        /// </summary>
        private readonly string _qualitylife = "quality.ab";
        /// <summary>
        /// 发红包
        /// </summary>
        private readonly string _redPackage = "redpackage.ab";
        /// <summary>
        /// 有钱有闲
        /// </summary>
        private readonly string _relax = "relax.ab";
        /// <summary>
        /// 风险
        /// </summary>
        private readonly string _risk = "risk.ab";
        /// <summary>
        ///  轮到自己了
        /// </summary>
        private readonly string _returnMe = "turnedme.ab";

        /// <summary>
        ///  购买保险
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_BuyEnsurence(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _buyEnsurence);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  可以还款了
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_CanPayback(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _canPayBack);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  进入机会卡牌的提示
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_Chance(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _chance);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  进入结账日卡牌的提示
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_CheckDay(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _checkday);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  进入命运的提示
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_Fate(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _fateTip);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  生孩子提示
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_GiveChild(string modelID)
        {
            //var tmpPath = string.Format(audioTipPath, modelID, _giveChild);
            //PlaySound(tmpPath);
           PlaySound(SubTitleManager.Instance.musicData.getBaby);
        }

        /// <summary>
        ///  投资失败配音
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_InvestmentLose(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _investmentLose);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  投资成功配音
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_InvestmentWin(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _investmentWin);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  品质生活配音
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_Quality(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _qualitylife);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  发红包提示
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_RedPackage(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _redPackage);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  有钱有闲声音提示
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_Relax(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _relax);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  风险提示音
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_Risk(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _risk);
            PlaySound(tmpPath);
        }

        /// <summary>
        ///  轮到自己掷筛子的提示音
        /// </summary>
        /// <param name="modelID"></param>
        public void Tip_ReturnedMe(string modelID)
        {
            var tmpPath = string.Format(audioTipPath, modelID, _returnMe);
            PlaySound(tmpPath);
        }

    }
}
