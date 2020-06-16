
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class ConclusionStarTip : ILoadable
#if UNITY_EDITOR
    , ISavable
#endif
    {
#if UNITY_EDITOR
        [Export(ExportFlags.AutoCode)]
        public override void Save (IOctetsWriter writer)
        {
            writer.Write(tipPinzhi1);
            writer.Write(tipPinzhi2);
            writer.Write(tipPinzhi3);
            writer.Write(tipPinzhi4);
            writer.Write(tipPinzhi5);
            writer.Write(tipChengzhang1);
            writer.Write(tipChengzhang2);
            writer.Write(tipChengzhang3);
            writer.Write(tipChengzhang4);
            writer.Write(tipChengzhang5);
            writer.Write(tipCaishang1);
            writer.Write(tipCaishang2);
            writer.Write(tipCaishang3);
            writer.Write(tipCaishang4);
            writer.Write(tipCaishang5);
            writer.Write(tipFengxian1);
            writer.Write(tipFengxian2);
            writer.Write(tipFengxian3);
            writer.Write(tipFengxian4);
            writer.Write(tipFengxian5);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            tipPinzhi1 = reader.ReadString();
            tipPinzhi2 = reader.ReadString();
            tipPinzhi3 = reader.ReadString();
            tipPinzhi4 = reader.ReadString();
            tipPinzhi5 = reader.ReadString();
            tipChengzhang1 = reader.ReadString();
            tipChengzhang2 = reader.ReadString();
            tipChengzhang3 = reader.ReadString();
            tipChengzhang4 = reader.ReadString();
            tipChengzhang5 = reader.ReadString();
            tipCaishang1 = reader.ReadString();
            tipCaishang2 = reader.ReadString();
            tipCaishang3 = reader.ReadString();
            tipCaishang4 = reader.ReadString();
            tipCaishang5 = reader.ReadString();
            tipFengxian1 = reader.ReadString();
            tipFengxian2 = reader.ReadString();
            tipFengxian3 = reader.ReadString();
            tipFengxian4 = reader.ReadString();
            tipFengxian5 = reader.ReadString();
        }

        public override string ToString ()
        {
            return string.Format("[ConclusionStarTip:ToString()] tipPinzhi1={0}, tipPinzhi2={1}, tipPinzhi3={2}, tipPinzhi4={3}, tipPinzhi5={4}, tipChengzhang1={5}, tipChengzhang2={6}, tipChengzhang3={7}, tipChengzhang4={8}, tipChengzhang5={9}, tipCaishang1={10}, tipCaishang2={11}, tipCaishang3={12}, tipCaishang4={13}, tipCaishang5={14}, tipFengxian1={15}, tipFengxian2={16}, tipFengxian3={17}, tipFengxian4={18}, tipFengxian5={19}", tipPinzhi1, tipPinzhi2, tipPinzhi3, tipPinzhi4, tipPinzhi5, tipChengzhang1, tipChengzhang2, tipChengzhang3, tipChengzhang4, tipChengzhang5, tipCaishang1, tipCaishang2, tipCaishang3, tipCaishang4, tipCaishang5, tipFengxian1, tipFengxian2, tipFengxian3, tipFengxian4, tipFengxian5);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}