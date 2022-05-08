using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for file list item entities which also includes length and hash information.
    /// </summary>
    /// <seealso cref="IDbFsItemListItem" />
    /// <seealso cref="IFileRow" />
    /// <seealso cref="IEquatable{IFileListItemWithBinaryProperties}" />
    public interface IFileListItemWithBinaryProperties : IDbFsItemListItem, IFileRow, IEquatable<IFileListItemWithBinaryProperties>
    {
        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>The length of the file, in bytes.</value>
        long Length { get; }

        /// <summary>
        /// Gets the MD5 checksum hash.
        /// </summary>
        /// <value>The MD5 checksum hash value or <see langword="null"/> if no hash value has been calculated.</value>
        MD5Hash? Hash { get; }

        /// <summary>
        /// Gets the redundancy count.
        /// </summary>
        /// <value>The number of files that are considered redundant to this file.</value>
        long RedundancyCount { get; }

        /// <summary>
        /// Gets the comparison count.
        /// </summary>
        /// <value>The number of files that have been compared to this file.</value>
/        long ComparisonCount { get; }
    }
}
