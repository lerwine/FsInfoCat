using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    // TODO: Move to FsInfoCat module
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
