
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class PlayerInitData : ILoadable
#if UNITY_EDITOR
    , ISavable
#endif
    {
#if UNITY_EDITOR
        [Export(ExportFlags.AutoCode)]
        public override void Save (IOctetsWriter writer)
        {
            writer.Write(id);
            writer.Write(playName);
            writer.Write(headPath);
            writer.Write(playerImgPath);
            writer.Write(oneChildPrise);
            writer.Write(careers);
            writer.Write(initAge);
            writer.Write(modelResID);
            writer.Write(fixBankSaving);
            writer.Write(cashFlow);
            writer.Write(fixHouseMortgages);
            writer.Write(fixEducationLoan);
            writer.Write(fixCarLoan);
            writer.Write(fixCardDebt);
            writer.Write(fixAdditionalDebt);
            writer.Write(fixTax);
            writer.Write(houseMortgages);
            writer.Write(educationLoan);
            writer.Write(carLoan);
            writer.Write(cardDebt);
            writer.Write(additionalDebt);
            writer.Write(otherSpend);
            writer.Write(infor);
            writer.Write(modelPath);
            writer.Write(playerSex);
            writer.Write(playerGift);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            id = reader.ReadInt32();
            playName = reader.ReadString();
            headPath = reader.ReadString();
            playerImgPath = reader.ReadString();
            oneChildPrise = reader.ReadSingle();
            careers = reader.ReadString();
            initAge = reader.ReadInt32();
            modelResID = reader.ReadInt32();
            fixBankSaving = reader.ReadInt32();
            cashFlow = reader.ReadInt32();
            fixHouseMortgages = reader.ReadInt32();
            fixEducationLoan = reader.ReadInt32();
            fixCarLoan = reader.ReadInt32();
            fixCardDebt = reader.ReadInt32();
            fixAdditionalDebt = reader.ReadInt32();
            fixTax = reader.ReadInt32();
            houseMortgages = reader.ReadInt32();
            educationLoan = reader.ReadInt32();
            carLoan = reader.ReadInt32();
            cardDebt = reader.ReadInt32();
            additionalDebt = reader.ReadInt32();
            otherSpend = reader.ReadInt32();
            infor = reader.ReadString();
            modelPath = reader.ReadString();
            playerSex = reader.ReadInt32();
            playerGift = reader.ReadString();
        }

        public override string ToString ()
        {
            return string.Format("[PlayerInitData:ToString()] id={0}, playName={1}, headPath={2}, playerImgPath={3}, oneChildPrise={4}, careers={5}, initAge={6}, modelResID={7}, fixBankSaving={8}, cashFlow={9}, fixHouseMortgages={10}, fixEducationLoan={11}, fixCarLoan={12}, fixCardDebt={13}, fixAdditionalDebt={14}, fixTax={15}, houseMortgages={16}, educationLoan={17}, carLoan={18}, cardDebt={19}, additionalDebt={20}, otherSpend={21}, infor={22}, modelPath={23}, playerSex={24}, playerGift={25}", id, playName, headPath, playerImgPath, oneChildPrise, careers, initAge, modelResID, fixBankSaving, cashFlow, fixHouseMortgages, fixEducationLoan, fixCarLoan, fixCardDebt, fixAdditionalDebt, fixTax, houseMortgages, educationLoan, carLoan, cardDebt, additionalDebt, otherSpend, infor, modelPath, playerSex, playerGift);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}