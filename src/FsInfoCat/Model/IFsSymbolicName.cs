using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IFsSymbolicName : ITimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        string Name { get; }
        string Notes { get; }
        bool IsInactive { get; }
        IFileSystem FileSystem { get; }
        IReadOnlyCollection<IFileSystem> DefaultFileSystems { get; }
    }
}
