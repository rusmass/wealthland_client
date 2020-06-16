
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class ModelTemplate : ILoadable
#if UNITY_EDITOR
    , ISavable
#endif
    {
#if UNITY_EDITOR
        [Export(ExportFlags.AutoCode)]
        public override void Save (IOctetsWriter writer)
        {
            writer.Write(id);
            writer.Write(modelName);
            writer.Write(modelPath);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            id = reader.ReadInt32();
            modelName = reader.ReadString();
            modelPath = reader.ReadString();
        }

        public override string ToString ()
        {
            return string.Format("[ModelTemplate:ToString()] id={0}, modelName={1}, modelPath={2}", id, modelName, modelPath);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}