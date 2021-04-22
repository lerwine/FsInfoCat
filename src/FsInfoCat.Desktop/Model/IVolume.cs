using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    public interface IVolume
    {
        bool CaseSensitive { get; }
        DateTime CreatedOn { get; }
        string DisplayName { get; }
        string DriveFormat { get; }
        Guid Id { get; }
        string Identifier { get; }
        bool IsInactive { get; }
        long MaxNameLength { get; }
        DateTime ModifiedOn { get; }
        string Notes { get; }
        string RootPathName { get; }
        IReadOnlyCollection<ISubDirectory> SubDirectories { get; }
        string VolumeName { get; }
    }
}
