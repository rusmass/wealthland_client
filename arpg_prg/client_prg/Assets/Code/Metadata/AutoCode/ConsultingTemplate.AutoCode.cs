
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
            writer.Write(questionName);
            writer.Write(questionDescribe);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            id = reader.ReadInt32();
            questionGroupName = reader.ReadString();
            questionName = reader.ReadString();
            questionDescribe = reader.ReadString();
        }

        public override string ToString ()
        {
            return string.Format("[ConsultingTemplate:ToString()] id={0}, questionGroupName={1}, questionName={2}, questionDescribe={3}", id, questionGroupName, questionName, questionDescribe);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}