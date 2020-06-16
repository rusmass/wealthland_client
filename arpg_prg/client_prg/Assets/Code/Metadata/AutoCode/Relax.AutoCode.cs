
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class Relax : ILoadable
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
            writer.Write(payment);
            writer.Write(profit);
            writer.Write(income);
            writer.Write(timeScore);
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
            payment = reader.ReadSingle();
            profit = reader.ReadString();
            income = reader.ReadSingle();
            timeScore = reader.ReadSingle();
            rankScore = reader.ReadInt32();
            quitScore = reader.ReadInt32();
        }

        public override string ToString ()
        {
            return string.Format("[Relax:ToString()] id={0}, title={1}, cardPath={2}, desc={3}, payment={4}, profit={5}, income={6}, timeScore={7}, rankScore={8}, quitScore={9}", id, title, cardPath, desc, payment, profit, income, timeScore, rankScore, quitScore);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}