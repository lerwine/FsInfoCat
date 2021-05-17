using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    [System.Obsolete("Use FsInfoCat.Model.IVolume")]
    public interface IVolume
    {
        Guid Id { get; }
        bool? CaseSensitiveSearch { get; }
        DateTime CreatedOn { get; }
        string DisplayName { get; }
        string Identifier { get; }
        bool IsInactive { get; }
        long? MaxNameLength { get; }
        DateTime ModifiedOn { get; }
        string Notes { get; }
        ISubDirectory RootDirectory { get; }
        IFileSystem FileSystem { get; }
        string VolumeName { get; }
    }
}
