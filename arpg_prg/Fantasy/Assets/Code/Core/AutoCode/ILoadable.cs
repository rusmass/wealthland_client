using Core.IO;

namespace Metadata
{
    public interface ILoadable
    {
        void Load(IOctetsReader reader);
    }
}