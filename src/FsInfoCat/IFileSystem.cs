using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IFileSystem : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
        Guid Id { get; set; }

        string DisplayName { get; set; }

        bool CaseSensitiveSearch { get; set; }

        bool ReadOnly { get; set; }

        uint MaxNameLength { get; set; }

        System.IO.DriveType? DefaultDriveType { get; set; }

        string Notes { get; set; }

        bool IsInactive { get; set; }

        IEnumerable<IVolume> Volumes { get; }

        IEnumerable<ISymbolicName> SymbolicNames { get; }
    }
}
