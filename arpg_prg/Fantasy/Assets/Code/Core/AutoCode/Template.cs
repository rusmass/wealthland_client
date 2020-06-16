 using System;
using System.IO;
using Core.IO;

namespace Metadata
{
    [Serializable]
    public abstract class Template: IMetadata, ILoadable
    {
        public Template ()
        {
        }

        public virtual void Load (IOctetsReader reader)
        {
			throw new NotSupportedException("Should be overrided by subclass. type=" + GetType().FullName);
		}

        public virtual void Save(IOctetsWriter writer)
        {
            throw new NotSupportedException("Should be overrided by subclass. type=" + GetType().FullName);
        }

        public virtual ushort GetMetadataType()
        {
            throw new NotSupportedException("Should be overrided by subclass. type=" + GetType().FullName);
        }

        public int id;
    }
}