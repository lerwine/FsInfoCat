using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalSymbolicName : IFsSymbolicName, ILocalModel
    {
        new ILocalFileSystem FileSystem { get; }
        new IReadOnlyCollection<ILocalFileSystem> DefaultFileSystems { get; }
    }
}
