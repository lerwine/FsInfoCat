using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a set of files that have the same file size and cryptographic hash.
    /// </summary>
    /// <seealso cref="FsInfoCat.IDbEntity" />
    public interface IContentInfo : IDbEntity
    {
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the file length.
        /// </summary>
        /// <value>
        /// The file length in bytes.
        /// </value>
        long Length { get; set; }

        /// <summary>
        /// Gets the MD5 hash of the file's contents.
        /// </summary>
        /// <value>
        /// The MD5 hash of the file's contents or <see langword="null"/> if the hash has not yet been calculated.
        /// </value>
        MD5Hash? Hash { get; set; }

        /// <summary>
        /// Gets the files which have the same length and cryptographic hash.
        /// </summary>
        /// <value>
        /// The files which have the same length and cryptographic hash..
        /// </value>
        IEnumerable<IFile> Files { get; }

        /// <summary>
        /// Gets the sets of files which were determined to be duplicates.
        /// </summary>
        /// <value>
        /// The sets of files which were determined to be duplicates.
        /// </value>
        IEnumerable<IRedundantSet> RedundantSets { get; }
    }
}
