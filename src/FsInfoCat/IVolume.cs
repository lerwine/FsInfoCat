using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IVolume : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
        Guid Id { get; set; }

        string DisplayName { get; set; }

        string VolumeName { get; set; }

        VolumeIdentifier Identifier { get; set; }

        bool? CaseSensitiveSearch { get; set; }

        bool? ReadOnly { get; set; }

        uint? MaxNameLength { get; set; }

        System.IO.DriveType Type { get; set; }

        string Notes { get; set; }

        VolumeStatus Status { get; set; }

        IFileSystem FileSystem { get; set; }

        ISubdirectory RootDirectory { get; }

        IEnumerable<IAccessError<IVolume>> AccessErrors { get; }
    }
}
