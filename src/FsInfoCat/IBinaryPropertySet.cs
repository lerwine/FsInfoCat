using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Represents a set of files that have the same file size and cryptographic hash.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IBinaryPropertySet : IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>Gets the file length.</summary>
        /// <value>The file length in bytes.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Length), ResourceType = typeof(Properties.Resources))]
        long Length { get; }

        /// <summary>Gets the MD5 hash of the file's contents.</summary>
        /// <value>The MD5 hash of the file's contents or <see langword="null" /> if the hash has not yet been calculated.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Hash), ResourceType = typeof(Properties.Resources))]
        MD5Hash? Hash { get; }

        /// <summary>Gets the files which have the same length and cryptographic hash.</summary>
        /// <value>The files which have the same length and cryptographic hash..</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFile> Files { get; }

        /// <summary>Gets the sets of files which were determined to be duplicates.</summary>
        /// <value>The sets of files which were determined to be duplicates.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSets), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IRedundantSet> RedundantSets { get; }
    }

}

