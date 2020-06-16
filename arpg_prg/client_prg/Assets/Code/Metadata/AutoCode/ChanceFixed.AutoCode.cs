
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class ChanceFixed : ILoadable
#if UNITY_EDITOR
    , ISavable
#endif
    {
#if UNITY_EDITOR
        [Export(ExportFlags.AutoCode)]
        public override void Save (IOctetsWriter writer)
        {
            writer.Write(id);
            writer.Write(belongsTo);
            writer.Write(title);
            writer.Write(cardPath);
            writer.Write(desc);
            writer.Write(baseNumber);
            writer.Write(coast);
            writer.Write(sale);
            writer.Write(payment);
            writer.Write(profit);
            writer.Write(mortgage);
            writer.Write(scoreType);
            writer.Write(scoreNumber);
            writer.Write(income);
            writer.Write(rankScore);
            writer.Write(quitScore);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            id = reader.ReadInt32();
            belongsTo = reader.ReadInt32();
            title = reader.ReadString();
            cardPath = reader.ReadString();
            desc = reader.ReadString();
            baseNumber = reader.ReadInt32();
            coast = reader.ReadString();
            sale = reader.ReadString();
            payment = reader.ReadSingle();
            profit = reader.ReadString();
            mortgage = reader.ReadSingle();
            scoreType = reader.ReadInt32();
            scoreNumber = reader.ReadInt32();
            income = reader.ReadSingle();
            rankScore = reader.ReadInt32();
            quitScore = reader.ReadInt32();
        }

        public override string ToString ()
        {
            return string.Format("[ChanceFixed:ToString()] id={0}, belongsTo={1}, title={2}, cardPath={3}, desc={4}, baseNumber={5}, coast={6}, sale={7}, payment={8}, profit={9}, mortgage={10}, scoreType={11}, scoreNumber={12}, income={13}, rankScore={14}, quitScore={15}", id, belongsTo, title, cardPath, desc, baseNumber, coast, sale, payment, profit, mortgage, scoreType, scoreNumber, income, rankScore, quitScore);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}