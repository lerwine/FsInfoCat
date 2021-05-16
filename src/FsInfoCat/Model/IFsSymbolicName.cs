using System;

namespace FsInfoCat.Model
{
    // TODO: Move to FsInfoCat module
    public interface IFsSymbolicName : ITimeStampedEntity
    {
        Guid Id { get; }
        string Name { get; }
        Guid FileSystemId { get; }
        string Notes { get; }
        bool IsInactive { get; }
    }
}
