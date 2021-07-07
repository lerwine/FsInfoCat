using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a set of files that have the same file size and cryptographic hash.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IFile.BinaryProperties"/>
    public interface IBinaryPropertySet : IPropertySet
    {
        /// <summary>
        /// Gets or sets the file length.
        /// </summary>
        /// <value>
        /// The file length in bytes.
        /// </value>
        // TODO: Add [Display(Name = nameof(Properties.Resources.DisplayName_Length), ResourceType = typeof(Properties.Resources))]
        long Length { get; set; }

        /// <summary>
        /// Gets the MD5 hash of the file's contents.
        /// </summary>
        /// <value>
        /// The MD5 hash of the file's contents or <see langword="null"/> if the hash has not yet been calculated.
        /// </value>
        // TODO: Add [Display(Name = nameof(Properties.Resources.DisplayName_Hash), ResourceType = typeof(Properties.Resources))]
        MD5Hash? Hash { get; set; }

        /// <summary>
        /// Gets the sets of files which were determined to be duplicates.
        /// </summary>
        /// <value>
        /// The sets of files which were determined to be duplicates.
        /// </value>
        // TODO: Add [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSets), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IRedundantSet> RedundantSets { get; }
    }
}
