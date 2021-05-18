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
        bool? CaseSensitiveSearch { get; }
        string DisplayName { get; }
        string Identifier { get; }
        bool IsInactive { get; }
        long? MaxNameLength { get; }
        bool? ReadOnly { get; }
        string Notes { get; }
        string VolumeName { get; }
        DriveType Type { get; }
        ISubDirectory RootDirectory { get; }
        IFileSystem FileSystem { get; }
    }
}
