using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalSymbolicName : IFsSymbolicName
    {
        new ILocalFileSystem FileSystem { get; }
        new IReadOnlyCollection<ILocalFileSystem> DefaultFileSystems { get; }
    }
}
