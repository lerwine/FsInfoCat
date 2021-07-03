using System;

namespace FsInfoCat
{
    public interface ISymbolicName : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
        Guid Id { get; set; }

        string Name { get; set; }

        string Notes { get; set; }

        int Priority { get; set; }

        bool IsInactive { get; set; }

        IFileSystem FileSystem { get; set; }
    }
}
