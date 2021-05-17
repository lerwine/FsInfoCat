using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IVolume : ITimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        bool? CaseSensitiveSearch { get; }
        string DisplayName { get; }
        string Identifier { get; }
        bool IsInactive { get; }
        long? MaxNameLength { get; }
        string Notes { get; }
        string VolumeName { get; }
        ISubDirectory RootDirectory { get; }
        IFileSystem FileSystem { get; }
    }
}
