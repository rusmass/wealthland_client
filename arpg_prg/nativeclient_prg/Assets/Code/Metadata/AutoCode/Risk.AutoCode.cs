
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class Risk : ILoadable
#if UNITY_EDITOR
    , ISavable
#endif
    {
#if UNITY_EDITOR
        [Export(ExportFlags.AutoCode)]
        public override void Save (IOctetsWriter writer)
        {
            writer.Write(id);
            writer.Write(riskTpye);
            writer.Write(title);
            writer.Write(cardPath);
            writer.Write(desc);
            writer.Write(tipType);
            writer.Write(payment);
            writer.Write(desc2);
            writer.Write(payment2);
            writer.Write(scoreType);
            writer.Write(scoreName);
            writer.Write(score);
            writer.Write(score_desc);
            writer.Write(rankScore);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            id = reader.ReadInt32();
            riskTpye = reader.ReadInt32();
            title = reader.ReadString();
            cardPath = reader.ReadString();
            desc = reader.ReadString();
            tipType = reader.ReadInt32();
            payment = reader.ReadSingle();
            desc2 = reader.ReadString();
            payment2 = reader.ReadSingle();
            scoreType = reader.ReadInt32();
            scoreName = reader.ReadString();
            score = reader.ReadInt32();
            score_desc = reader.ReadString();
            rankScore = reader.ReadInt32();
        }

        public override string ToString ()
        {
            return string.Format("[Risk:ToString()] id={0}, riskTpye={1}, title={2}, cardPath={3}, desc={4}, tipType={5}, payment={6}, desc2={7}, payment2={8}, scoreType={9}, scoreName={10}, score={11}, score_desc={12}, rankScore={13}", id, riskTpye, title, cardPath, desc, tipType, payment, desc2, payment2, scoreType, scoreName, score, score_desc, rankScore);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}