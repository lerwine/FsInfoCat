using System;

namespace FsInfoCat
{
    /// <summary>
    /// Configuration of a file system crawl instance.
    /// </summary>
    public interface ICrawlConfiguration : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
        Guid Id { get; set; }

        string DisplayName { get; set; }

        string Notes { get; set; }

        ISubdirectory Root { get; }

        ushort MaxRecursionDepth { get; set; }

        ulong MaxTotalItems { get; set; }

        bool IsInactive { get; set; }
    }
}
