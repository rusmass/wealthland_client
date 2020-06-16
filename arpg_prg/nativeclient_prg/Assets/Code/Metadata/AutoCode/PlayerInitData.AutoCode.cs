
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
            writer.Write(fixHouseDebt);
            writer.Write(fixEducationDebt);
            writer.Write(fixCarDebt);
            writer.Write(fixCardDebt);
            writer.Write(fixAdditionalDebt);
            writer.Write(taxPay);
            writer.Write(housePay);
            writer.Write(educationPay);
            writer.Write(carPay);
            writer.Write(cardPay);
            writer.Write(additionalPay);
            writer.Write(nessPay);
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
            fixHouseDebt = reader.ReadInt32();
            fixEducationDebt = reader.ReadInt32();
            fixCarDebt = reader.ReadInt32();
            fixCardDebt = reader.ReadInt32();
            fixAdditionalDebt = reader.ReadInt32();
            taxPay = reader.ReadInt32();
            housePay = reader.ReadInt32();
            educationPay = reader.ReadInt32();
            carPay = reader.ReadInt32();
            cardPay = reader.ReadInt32();
            additionalPay = reader.ReadInt32();
            nessPay = reader.ReadInt32();
            infor = reader.ReadString();
            modelPath = reader.ReadString();
            playerSex = reader.ReadInt32();
            playerGift = reader.ReadString();
        }

        public override string ToString ()
        {
            return string.Format("[PlayerInitData:ToString()] id={0}, playName={1}, headPath={2}, playerImgPath={3}, oneChildPrise={4}, careers={5}, initAge={6}, modelResID={7}, fixBankSaving={8}, cashFlow={9}, fixHouseDebt={10}, fixEducationDebt={11}, fixCarDebt={12}, fixCardDebt={13}, fixAdditionalDebt={14}, taxPay={15}, housePay={16}, educationPay={17}, carPay={18}, cardPay={19}, additionalPay={20}, nessPay={21}, infor={22}, modelPath={23}, playerSex={24}, playerGift={25}", id, playName, headPath, playerImgPath, oneChildPrise, careers, initAge, modelResID, fixBankSaving, cashFlow, fixHouseDebt, fixEducationDebt, fixCarDebt, fixCardDebt, fixAdditionalDebt, taxPay, housePay, educationPay, carPay, cardPay, additionalPay, nessPay, infor, modelPath, playerSex, playerGift);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}