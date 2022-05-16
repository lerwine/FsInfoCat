using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an entity that represents a group of <see cref="IFile"/> entities which are considered redundant.
    /// </summary>
    /// <seealso cref="IRedundantSetRow" />
    /// <seealso cref="IEquatable{IRedundantSetListItem}" />
    public interface IRedundantSetListItem : IRedundantSetRow, IEquatable<IRedundantSetListItem>
    {
        /// <summary>
        /// Gets the length that all files in this redundant set have in common.
        /// </summary>
        /// <value>The <see cref="IBinaryPropertySet.Length"/> value that all files in this redundant set have in common.</value>
        long Length { get; }

        /// <summary>
        /// Gets the MD5 hash value that all files in this redundant set have in common.
        /// </summary>
        /// <value>The <see cref="IBinaryPropertySet.Length"/> value that all files in this redundant set have in common or <see langword="null"/> if the MD5 hash has not be
        /// calculated for the files in this set.</value>
        MD5Hash? Hash { get; }

        /// <summary>
        /// Gets the redundancy count.
        /// </summary>
        /// <value>The number of <see cref="IFile"/> entities in this redundant set.</value>
        long RedundancyCount { get; }
    }
}
