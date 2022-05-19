using M = FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a set of files that have the same file size and cryptographic hash.
    /// </summary>
    /// <seealso cref="M.IBinaryPropertySet" />
    /// <seealso cref="IEquatable{ILocalBinaryPropertySet}" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ILocalDbContext.BinaryPropertySets" />
    /// <seealso cref="ILocalFile.BinaryProperties" />
    /// <seealso cref="ILocalRedundantSet.BinaryProperties" />
    /// <seealso cref="Upstream.Model.IBinaryPropertySet" />
    public interface ILocalBinaryPropertySet : ILocalDbEntity, M.IBinaryPropertySet, IEquatable<ILocalBinaryPropertySet>
    {
        /// <summary>
        /// Gets the files which have the same length and cryptographic hash.
        /// </summary>
        /// <value>The files which have the same length and cryptographic hash..</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalFile> Files { get; }

        /// <summary>
        /// Gets the sets of files which were determined to be duplicates.
        /// </summary>
        /// <value>The sets of files which were determined to be duplicates.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSets), ShortName = nameof(Properties.Resources.ShortName_RedundantSets),
            ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalRedundantSet> RedundantSets { get; }
    }
}
