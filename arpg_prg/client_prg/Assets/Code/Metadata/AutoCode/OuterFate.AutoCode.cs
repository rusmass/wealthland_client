
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class OuterFate : ILoadable
#if UNITY_EDITOR
    , ISavable
#endif
    {
#if UNITY_EDITOR
        [Export(ExportFlags.AutoCode)]
        public override void Save (IOctetsWriter writer)
        {
            writer.Write(id);
            writer.Write(title);
            writer.Write(cardPath);
            writer.Write(desc);
            writer.Write(fateType);
            writer.Write(isHandle_peymeny);
            writer.Write(payment);
            writer.Write(handleRange);
            writer.Write(relateID);
            writer.Write(isHandle_income);
            writer.Write(handler_income_type);
            writer.Write(handler_income_number);
            writer.Write(rankScore);
            writer.Write(quitScore);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            id = reader.ReadInt32();
            title = reader.ReadString();
            cardPath = reader.ReadString();
            desc = reader.ReadString();
            fateType = reader.ReadInt32();
            isHandle_peymeny = reader.ReadInt32();
            payment = reader.ReadSingle();
            handleRange = reader.ReadInt32();
            relateID = reader.ReadInt32();
            isHandle_income = reader.ReadInt32();
            handler_income_type = reader.ReadInt32();
            handler_income_number = reader.ReadSingle();
            rankScore = reader.ReadInt32();
            quitScore = reader.ReadInt32();
        }

        public override string ToString ()
        {
            return string.Format("[OuterFate:ToString()] id={0}, title={1}, cardPath={2}, desc={3}, fateType={4}, isHandle_peymeny={5}, payment={6}, handleRange={7}, relateID={8}, isHandle_income={9}, handler_income_type={10}, handler_income_number={11}, rankScore={12}, quitScore={13}", id, title, cardPath, desc, fateType, isHandle_peymeny, payment, handleRange, relateID, isHandle_income, handler_income_type, handler_income_number, rankScore, quitScore);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}