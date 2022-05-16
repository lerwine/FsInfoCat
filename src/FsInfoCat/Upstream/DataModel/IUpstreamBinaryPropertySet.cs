using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a set of files that have the same file size and cryptographic hash.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IBinaryPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamBinaryPropertySet}" />
    /// <seealso cref="Local.ILocalBinaryPropertySet" />
    public interface IUpstreamBinaryPropertySet : IUpstreamDbEntity, IBinaryPropertySet, IEquatable<IUpstreamBinaryPropertySet>
    {
        /// <summary>
        /// Gets the files which have the same length and cryptographic hash.
        /// </summary>
        /// <value>The files which have the same length and cryptographic hash..</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamFile> Files { get; }

        /// <summary>
        /// Gets the sets of files which were determined to be duplicates.
        /// </summary>
        /// <value>The sets of files which were determined to be duplicates.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSets), ShortName = nameof(Properties.Resources.ShortName_RedundantSets),
            ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamRedundantSet> RedundantSets { get; }
    }
}
