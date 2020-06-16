
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class ConsultingTemplate : ILoadable
#if UNITY_EDITOR
    , ISavable
#endif
    {
#if UNITY_EDITOR
        [Export(ExportFlags.AutoCode)]
        public override void Save (IOctetsWriter writer)
        {
            writer.Write(id);
            writer.Write(questionGroupName);
            writer.Write(typeID);
            writer.Write(questionName);
            writer.Write(questionDescribe);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            id = reader.ReadInt32();
            questionGroupName = reader.ReadString();
            typeID = reader.ReadInt32();
            questionName = reader.ReadString();
            questionDescribe = reader.ReadString();
        }

        public override string ToString ()
        {
            return string.Format("[ConsultingTemplate:ToString()] id={0}, questionGroupName={1}, typeID={2}, questionName={3}, questionDescribe={4}", id, questionGroupName, typeID, questionName, questionDescribe);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}