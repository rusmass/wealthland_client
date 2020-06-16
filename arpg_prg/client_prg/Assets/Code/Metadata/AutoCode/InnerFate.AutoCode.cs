
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class InnerFate : ILoadable
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
            writer.Write(paymenyMethod);
            writer.Write(paymenyType);
            writer.Write(paymeny);
            writer.Write(relateID);
            writer.Write(dice_prise);
            writer.Write(dice_condition);
            writer.Write(dice_number);
            writer.Write(dice_prise_type);
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
            paymenyMethod = reader.ReadInt32();
            paymenyType = reader.ReadInt32();
            paymeny = reader.ReadSingle();
            relateID = reader.ReadInt32();
            dice_prise = reader.ReadSingle();
            dice_condition = reader.ReadInt32();
            dice_number = reader.ReadInt32();
            dice_prise_type = reader.ReadInt32();
            rankScore = reader.ReadInt32();
            quitScore = reader.ReadInt32();
        }

        public override string ToString ()
        {
            return string.Format("[InnerFate:ToString()] id={0}, title={1}, cardPath={2}, desc={3}, fateType={4}, paymenyMethod={5}, paymenyType={6}, paymeny={7}, relateID={8}, dice_prise={9}, dice_condition={10}, dice_number={11}, dice_prise_type={12}, rankScore={13}, quitScore={14}", id, title, cardPath, desc, fateType, paymenyMethod, paymenyType, paymeny, relateID, dice_prise, dice_condition, dice_number, dice_prise_type, rankScore, quitScore);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}