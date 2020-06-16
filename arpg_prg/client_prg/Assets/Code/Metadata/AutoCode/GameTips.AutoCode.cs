
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class GameTips : ILoadable
#if UNITY_EDITOR
    , ISavable
#endif
    {
#if UNITY_EDITOR
        [Export(ExportFlags.AutoCode)]
        public override void Save (IOctetsWriter writer)
        {
            writer.Write(newTip1);
            writer.Write(newTip2);
            writer.Write(newTip3);
            writer.Write(newTip4);
            writer.Write(newTip5);
            writer.Write(newTip6);
            writer.Write(oldTip1);
            writer.Write(oldTip2);
            writer.Write(oldTip3);
            writer.Write(oldTip4);
            writer.Write(oldTip5);
            writer.Write(oldTip6);
            writer.Write(oldTip7);
            writer.Write(oldTip8);
            writer.Write(oldMoreTip1);
            writer.Write(oldMoreTip2);
            writer.Write(oldMoreTip3);
            writer.Write(oldMoreTip4);
            writer.Write(oldMoreTip5);
            writer.Write(oldMoreTip6);
            writer.Write(enterTip);
            writer.Write(innerTip1);
            writer.Write(innerTip2);
            writer.Write(innerTip3);
            writer.Write(innerTip4);
            writer.Write(innerTip5);
            writer.Write(innerTip6);
            writer.Write(innerTip7);
            writer.Write(innerTip8);
            writer.Write(innerTip9);
            writer.Write(innerTip10);
            writer.Write(innerTip11);
            writer.Write(innerTip12);
            writer.Write(innerTip13);
            writer.Write(innerTip14);
            writer.Write(innerTip15);
            writer.Write(enterResult1);
            writer.Write(enterResult2);
            writer.Write(enterResult3);
            writer.Write(overOuterCardRisk);
            writer.Write(overOuterCardOuerFate);
            writer.Write(overOuterCardSmallFixed);
            writer.Write(overOuterCardSmallShare);
            writer.Write(overOuterCardSellShare);
            writer.Write(overOuterCardChallenge);
            writer.Write(overOuterCardCharity);
            writer.Write(overOuterCardStudy);
            writer.Write(overOuterCardHealth);
            writer.Write(overOuterCardCheckOut);
            writer.Write(overOuterGiveChild);
            writer.Write(overOuterMoreChild);
            writer.Write(overOuterSendRed);
            writer.Write(overInnerRelax);
            writer.Write(overInnerQuality);
            writer.Write(overInnerInvestment);
            writer.Write(overInnerFate);
            writer.Write(overInnerStudy);
            writer.Write(overInnerCheckOut);
            writer.Write(overInnerHealth);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            newTip1 = reader.ReadString();
            newTip2 = reader.ReadString();
            newTip3 = reader.ReadString();
            newTip4 = reader.ReadString();
            newTip5 = reader.ReadString();
            newTip6 = reader.ReadString();
            oldTip1 = reader.ReadString();
            oldTip2 = reader.ReadString();
            oldTip3 = reader.ReadString();
            oldTip4 = reader.ReadString();
            oldTip5 = reader.ReadString();
            oldTip6 = reader.ReadString();
            oldTip7 = reader.ReadString();
            oldTip8 = reader.ReadString();
            oldMoreTip1 = reader.ReadString();
            oldMoreTip2 = reader.ReadString();
            oldMoreTip3 = reader.ReadString();
            oldMoreTip4 = reader.ReadString();
            oldMoreTip5 = reader.ReadString();
            oldMoreTip6 = reader.ReadString();
            enterTip = reader.ReadString();
            innerTip1 = reader.ReadString();
            innerTip2 = reader.ReadString();
            innerTip3 = reader.ReadString();
            innerTip4 = reader.ReadString();
            innerTip5 = reader.ReadString();
            innerTip6 = reader.ReadString();
            innerTip7 = reader.ReadString();
            innerTip8 = reader.ReadString();
            innerTip9 = reader.ReadString();
            innerTip10 = reader.ReadString();
            innerTip11 = reader.ReadString();
            innerTip12 = reader.ReadString();
            innerTip13 = reader.ReadString();
            innerTip14 = reader.ReadString();
            innerTip15 = reader.ReadString();
            enterResult1 = reader.ReadString();
            enterResult2 = reader.ReadString();
            enterResult3 = reader.ReadString();
            overOuterCardRisk = reader.ReadString();
            overOuterCardOuerFate = reader.ReadString();
            overOuterCardSmallFixed = reader.ReadString();
            overOuterCardSmallShare = reader.ReadString();
            overOuterCardSellShare = reader.ReadString();
            overOuterCardChallenge = reader.ReadString();
            overOuterCardCharity = reader.ReadString();
            overOuterCardStudy = reader.ReadString();
            overOuterCardHealth = reader.ReadString();
            overOuterCardCheckOut = reader.ReadString();
            overOuterGiveChild = reader.ReadString();
            overOuterMoreChild = reader.ReadString();
            overOuterSendRed = reader.ReadString();
            overInnerRelax = reader.ReadString();
            overInnerQuality = reader.ReadString();
            overInnerInvestment = reader.ReadString();
            overInnerFate = reader.ReadString();
            overInnerStudy = reader.ReadString();
            overInnerCheckOut = reader.ReadString();
            overInnerHealth = reader.ReadString();
        }

        public override string ToString ()
        {
            return string.Format("[GameTips:ToString()] newTip1={0}, newTip2={1}, newTip3={2}, newTip4={3}, newTip5={4}, newTip6={5}, oldTip1={6}, oldTip2={7}, oldTip3={8}, oldTip4={9}, oldTip5={10}, oldTip6={11}, oldTip7={12}, oldTip8={13}, oldMoreTip1={14}, oldMoreTip2={15}, oldMoreTip3={16}, oldMoreTip4={17}, oldMoreTip5={18}, oldMoreTip6={19}, enterTip={20}, innerTip1={21}, innerTip2={22}, innerTip3={23}, innerTip4={24}, innerTip5={25}, innerTip6={26}, innerTip7={27}, innerTip8={28}, innerTip9={29}, innerTip10={30}, innerTip11={31}, innerTip12={32}, innerTip13={33}, innerTip14={34}, innerTip15={35}, enterResult1={36}, enterResult2={37}, enterResult3={38}, overOuterCardRisk={39}, overOuterCardOuerFate={40}, overOuterCardSmallFixed={41}, overOuterCardSmallShare={42}, overOuterCardSellShare={43}, overOuterCardChallenge={44}, overOuterCardCharity={45}, overOuterCardStudy={46}, overOuterCardHealth={47}, overOuterCardCheckOut={48}, overOuterGiveChild={49}, overOuterMoreChild={50}, overOuterSendRed={51}, overInnerRelax={52}, overInnerQuality={53}, overInnerInvestment={54}, overInnerFate={55}, overInnerStudy={56}, overInnerCheckOut={57}, overInnerHealth={58}", newTip1, newTip2, newTip3, newTip4, newTip5, newTip6, oldTip1, oldTip2, oldTip3, oldTip4, oldTip5, oldTip6, oldTip7, oldTip8, oldMoreTip1, oldMoreTip2, oldMoreTip3, oldMoreTip4, oldMoreTip5, oldMoreTip6, enterTip, innerTip1, innerTip2, innerTip3, innerTip4, innerTip5, innerTip6, innerTip7, innerTip8, innerTip9, innerTip10, innerTip11, innerTip12, innerTip13, innerTip14, innerTip15, enterResult1, enterResult2, enterResult3, overOuterCardRisk, overOuterCardOuerFate, overOuterCardSmallFixed, overOuterCardSmallShare, overOuterCardSellShare, overOuterCardChallenge, overOuterCardCharity, overOuterCardStudy, overOuterCardHealth, overOuterCardCheckOut, overOuterGiveChild, overOuterMoreChild, overOuterSendRed, overInnerRelax, overInnerQuality, overInnerInvestment, overInnerFate, overInnerStudy, overInnerCheckOut, overInnerHealth);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}