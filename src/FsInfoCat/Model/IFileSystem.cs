using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IFileSystem : ITimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        string DisplayName { get; }
        bool CaseSensitiveSearch { get; }
        bool ReadOnly { get; }
        long MaxNameLength { get; }
        System.IO.DriveType? DefaultDriveType { get; }
        string Notes { get; }
        bool IsInactive { get; }
        IReadOnlyCollection<IVolume> Volumes { get; }
        IReadOnlyCollection<IFsSymbolicName> SymbolicNames { get; }
        IFsSymbolicName DefaultSymbolicName { get; }
    }
}
