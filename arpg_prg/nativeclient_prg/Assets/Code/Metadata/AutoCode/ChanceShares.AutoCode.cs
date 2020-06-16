
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class ChanceShares : ILoadable
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
            writer.Write(ticketCode);
            writer.Write(ticketName);
            writer.Write(payment);
            writer.Write(returnRate);
            writer.Write(shareOut);
            writer.Write(qualityScore);
            writer.Write(qualityDesc);
            writer.Write(income);
            writer.Write(priceRagne);
            writer.Write(shareNum);
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
            ticketCode = reader.ReadString();
            ticketName = reader.ReadString();
            payment = reader.ReadInt32();
            returnRate = reader.ReadString();
            shareOut = reader.ReadString();
            qualityScore = reader.ReadInt32();
            qualityDesc = reader.ReadString();
            income = reader.ReadInt32();
            priceRagne = reader.ReadString();
            shareNum = reader.ReadInt32();
            rankScore = reader.ReadInt32();
            quitScore = reader.ReadInt32();
        }

        public override string ToString ()
        {
            return string.Format("[ChanceShares:ToString()] id={0}, belongsTo={1}, title={2}, cardPath={3}, desc={4}, ticketCode={5}, ticketName={6}, payment={7}, returnRate={8}, shareOut={9}, qualityScore={10}, qualityDesc={11}, income={12}, priceRagne={13}, shareNum={14}, rankScore={15}, quitScore={16}", id, belongsTo, title, cardPath, desc, ticketCode, ticketName, payment, returnRate, shareOut, qualityScore, qualityDesc, income, priceRagne, shareNum, rankScore, quitScore);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}