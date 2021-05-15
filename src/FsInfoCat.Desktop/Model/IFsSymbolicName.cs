using System;

namespace FsInfoCat.Desktop.Model
{
    // TODO: Move to FsInfoCat module
    public interface IFsSymbolicName
    {
        Guid Id { get; }
        string Name { get; }
        Guid FileSystemId { get; }
        string Notes { get; }
        bool IsInactive { get; }
    }
}
