
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class SubtitleCount : ILoadable
#if UNITY_EDITOR
    , ISavable
#endif
    {
#if UNITY_EDITOR
        [Export(ExportFlags.AutoCode)]
        public override void Save (IOctetsWriter writer)
        {
            writer.Write(buyChanceCard2);
            writer.Write(quitChanceCard2);
            writer.Write(salseFateCard3);
            writer.Write(quitFateCard1);
            writer.Write(effectShareTwoInOne2);
            writer.Write(effectShareOneInTwo2);
            writer.Write(effectMutipleReduceIncome3);
            writer.Write(effectIncreaseIncone3);
            writer.Write(effectReduceIncome3);
            writer.Write(riskCoast3);
            writer.Write(getCheckOutMoney2);
            writer.Write(getChildDesc2);
            writer.Write(involveCharity2);
            writer.Write(involveHealth2);
            writer.Write(involeStudy2);
            writer.Write(timeAndMondeyGetMoney3);
            writer.Write(timeAndMoneyGateMoneyAndScore4);
            writer.Write(qualityGetSocre4);
            writer.Write(investmentGetMoney3);
            writer.Write(getOpportChance1);
            writer.Write(innerFateShareLose2);
            writer.Write(innerFateShareGet2);
            writer.Write(innerFateSafe2);
            writer.Write(innerFateLose2);
            writer.Write(lackOfGold);
            writer.Write(cardChance);
            writer.Write(cardFate);
            writer.Write(cardRisk);
            writer.Write(cardCheckOut);
            writer.Write(cardGetChild);
            writer.Write(cardCharity);
            writer.Write(cardHealth);
            writer.Write(cardStudy);
            writer.Write(cardTimeAndMoney);
            writer.Write(cardQualityLife);
            writer.Write(cardFreeChoice);
            writer.Write(cardInvestment);
            writer.Write(cardOpportunity);
            writer.Write(lackOfTimeScore);
            writer.Write(AllEffect);
            writer.Write(userIsEffectedByFate);
            writer.Write(pigCashflow);
            writer.Write(pigIncome);
            writer.Write(pigTimetip);
            writer.Write(pigQualitytip);
            writer.Write(enterCard);
            writer.Write(noChoiceToCheck);
            writer.Write(moreChildForBoard);
            writer.Write(moreChildForTip);
            writer.Write(rollSuccessInvestment);
            writer.Write(rollSuccessInnerfate);
            writer.Write(rollFail);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            buyChanceCard2 = reader.ReadString();
            quitChanceCard2 = reader.ReadString();
            salseFateCard3 = reader.ReadString();
            quitFateCard1 = reader.ReadString();
            effectShareTwoInOne2 = reader.ReadString();
            effectShareOneInTwo2 = reader.ReadString();
            effectMutipleReduceIncome3 = reader.ReadString();
            effectIncreaseIncone3 = reader.ReadString();
            effectReduceIncome3 = reader.ReadString();
            riskCoast3 = reader.ReadString();
            getCheckOutMoney2 = reader.ReadString();
            getChildDesc2 = reader.ReadString();
            involveCharity2 = reader.ReadString();
            involveHealth2 = reader.ReadString();
            involeStudy2 = reader.ReadString();
            timeAndMondeyGetMoney3 = reader.ReadString();
            timeAndMoneyGateMoneyAndScore4 = reader.ReadString();
            qualityGetSocre4 = reader.ReadString();
            investmentGetMoney3 = reader.ReadString();
            getOpportChance1 = reader.ReadString();
            innerFateShareLose2 = reader.ReadString();
            innerFateShareGet2 = reader.ReadString();
            innerFateSafe2 = reader.ReadString();
            innerFateLose2 = reader.ReadString();
            lackOfGold = reader.ReadString();
            cardChance = reader.ReadString();
            cardFate = reader.ReadString();
            cardRisk = reader.ReadString();
            cardCheckOut = reader.ReadString();
            cardGetChild = reader.ReadString();
            cardCharity = reader.ReadString();
            cardHealth = reader.ReadString();
            cardStudy = reader.ReadString();
            cardTimeAndMoney = reader.ReadString();
            cardQualityLife = reader.ReadString();
            cardFreeChoice = reader.ReadString();
            cardInvestment = reader.ReadString();
            cardOpportunity = reader.ReadString();
            lackOfTimeScore = reader.ReadString();
            AllEffect = reader.ReadString();
            userIsEffectedByFate = reader.ReadString();
            pigCashflow = reader.ReadString();
            pigIncome = reader.ReadString();
            pigTimetip = reader.ReadString();
            pigQualitytip = reader.ReadString();
            enterCard = reader.ReadString();
            noChoiceToCheck = reader.ReadString();
            moreChildForBoard = reader.ReadString();
            moreChildForTip = reader.ReadString();
            rollSuccessInvestment = reader.ReadString();
            rollSuccessInnerfate = reader.ReadString();
            rollFail = reader.ReadString();
        }

        public override string ToString ()
        {
            return string.Format("[SubtitleCount:ToString()] buyChanceCard2={0}, quitChanceCard2={1}, salseFateCard3={2}, quitFateCard1={3}, effectShareTwoInOne2={4}, effectShareOneInTwo2={5}, effectMutipleReduceIncome3={6}, effectIncreaseIncone3={7}, effectReduceIncome3={8}, riskCoast3={9}, getCheckOutMoney2={10}, getChildDesc2={11}, involveCharity2={12}, involveHealth2={13}, involeStudy2={14}, timeAndMondeyGetMoney3={15}, timeAndMoneyGateMoneyAndScore4={16}, qualityGetSocre4={17}, investmentGetMoney3={18}, getOpportChance1={19}, innerFateShareLose2={20}, innerFateShareGet2={21}, innerFateSafe2={22}, innerFateLose2={23}, lackOfGold={24}, cardChance={25}, cardFate={26}, cardRisk={27}, cardCheckOut={28}, cardGetChild={29}, cardCharity={30}, cardHealth={31}, cardStudy={32}, cardTimeAndMoney={33}, cardQualityLife={34}, cardFreeChoice={35}, cardInvestment={36}, cardOpportunity={37}, lackOfTimeScore={38}, AllEffect={39}, userIsEffectedByFate={40}, pigCashflow={41}, pigIncome={42}, pigTimetip={43}, pigQualitytip={44}, enterCard={45}, noChoiceToCheck={46}, moreChildForBoard={47}, moreChildForTip={48}, rollSuccessInvestment={49}, rollSuccessInnerfate={50}, rollFail={51}", buyChanceCard2, quitChanceCard2, salseFateCard3, quitFateCard1, effectShareTwoInOne2, effectShareOneInTwo2, effectMutipleReduceIncome3, effectIncreaseIncone3, effectReduceIncome3, riskCoast3, getCheckOutMoney2, getChildDesc2, involveCharity2, involveHealth2, involeStudy2, timeAndMondeyGetMoney3, timeAndMoneyGateMoneyAndScore4, qualityGetSocre4, investmentGetMoney3, getOpportChance1, innerFateShareLose2, innerFateShareGet2, innerFateSafe2, innerFateLose2, lackOfGold, cardChance, cardFate, cardRisk, cardCheckOut, cardGetChild, cardCharity, cardHealth, cardStudy, cardTimeAndMoney, cardQualityLife, cardFreeChoice, cardInvestment, cardOpportunity, lackOfTimeScore, AllEffect, userIsEffectedByFate, pigCashflow, pigIncome, pigTimetip, pigQualitytip, enterCard, noChoiceToCheck, moreChildForBoard, moreChildForTip, rollSuccessInvestment, rollSuccessInnerfate, rollFail);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}