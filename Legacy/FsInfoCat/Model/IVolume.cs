using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat.Model
{
    public interface IVolume : ITimeStampedEntity, IValidatableObject
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier that is used as the prmary database key.
        /// </value>
        Guid Id { get; }
        string DisplayName { get; }
        string VolumeName { get; }
        string Identifier { get; }
        DriveType Type { get; }
        bool? CaseSensitiveSearch { get; }
        bool? ReadOnly { get; }
        long? MaxNameLength { get; }
        string Notes { get; }
        bool IsInactive { get; }
        ISubDirectory RootDirectory { get; }
        IFileSystem FileSystem { get; }
    }
}
