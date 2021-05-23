using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IFileSystem : IDbEntity
    {
        string DisplayName { get; set; }

        bool CaseSensitiveSearch { get; set; }

        bool ReadOnly { get; set; }

        long MaxNameLength { get; set; }

        System.IO.DriveType? DefaultDriveType { get; set; }

        string Notes { get; set; }

        bool IsInactive { get; set; }

        IEnumerable<IVolume> Volumes { get; }

        IEnumerable<ISymbolicName> SymbolicNames { get; }

        Guid Id { get; set; }
    }
}
