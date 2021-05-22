using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalFileSystem : IFileSystem, ILocalDbEntity
    {
        new IEnumerable<ILocalVolume> Volumes { get; }

        new IEnumerable<ILocalSymbolicName> SymbolicNames { get; }
    }
}
