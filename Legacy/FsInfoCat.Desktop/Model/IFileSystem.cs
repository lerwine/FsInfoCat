using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    [System.Obsolete("Use FsInfoCat.Model.IFileSystem")]
    public interface IFileSystem
    {
        Guid Id { get; }
        string DisplayName { get; }
        bool CaseSensitiveSearch { get; }
        bool ReadOnly { get; }
        long MaxNameLength { get; }
        System.IO.DriveType? DefaultDriveType { get; }
        string Notes { get; }
        bool IsInactive { get; }
        Guid DefaultSymbolicNameId { get; }
        IReadOnlyCollection<IVolume> Volumes { get; }
        IReadOnlyCollection<IFsSymbolicName> SymbolicNames { get; }
        IFsSymbolicName DefaultSymbolicName { get; }
    }
}
