using Core.IO;

namespace Metadata
{
    public interface ISavable
    {
        void Save(IOctetsWriter writer);
    }
}