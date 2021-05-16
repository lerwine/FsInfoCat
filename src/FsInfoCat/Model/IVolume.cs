using System;
using System.Collections.Generic;

namespace FsInfoCat.Model
{
    // TODO: Move to FsInfoCat module
    public interface IVolume : ITimeStampedEntity
    {
        Guid Id { get; }
        bool? CaseSensitiveSearch { get; }
        string DisplayName { get; }
        string Identifier { get; }
        bool IsInactive { get; }
        long? MaxNameLength { get; }
        string Notes { get; }
        ISubDirectory RootDirectory { get; }
        IFileSystem FileSystem { get; }
        string VolumeName { get; }
    }
}
