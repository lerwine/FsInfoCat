using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalFileSystem : IFileSystem, ILocalModel
    {
        new IReadOnlyCollection<ILocalVolume> Volumes { get; }
        new IReadOnlyCollection<ILocalSymbolicName> SymbolicNames { get; }
        new ILocalSymbolicName DefaultSymbolicName { get; }
    }
}
