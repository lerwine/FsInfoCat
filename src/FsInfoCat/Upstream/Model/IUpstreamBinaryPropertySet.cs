using FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a set of files that have the same file size and cryptographic hash.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IBinaryPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamBinaryPropertySet}" />
    /// <seealso cref="Local.Model.ILocalBinaryPropertySet" />
    public interface IUpstreamBinaryPropertySet : IUpstreamDbEntity, IBinaryPropertySet, IEquatable<IUpstreamBinaryPropertySet>
    {
        /// <summary>
        /// Gets the files which have the same length and cryptographic hash.
        /// </summary>
        /// <value>The files which have the same length and cryptographic hash..</value>
        [Display(Name = nameof(Properties.Resources.Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamFile> Files { get; }

        /// <summary>
        /// Gets the sets of files which were determined to be duplicates.
        /// </summary>
        /// <value>The sets of files which were determined to be duplicates.</value>
        [Display(Name = nameof(Properties.Resources.BinaryRedundantSets), ShortName = nameof(Properties.Resources.RedundantSets),
            ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamRedundantSet> RedundantSets { get; }
    }
}
