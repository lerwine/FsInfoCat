using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="IUpstreamRedundantSetRow" />
    /// <seealso cref="IRedundantSet" />
    /// <seealso cref="IEquatable{IUpstreamRedundantSet}" />
    /// <seealso cref="Local.ILocalRedundantSet" />
    public interface IUpstreamRedundantSet : IUpstreamRedundantSetRow, IRedundantSet, IEquatable<IUpstreamRedundantSet>
    {
        /// <summary>
        /// Gets the binary properties in common with all files in the current redundant set.
        /// </summary>
        /// <value>The binary properties in common with all files in the current redundant set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamBinaryPropertySet BinaryProperties { get; }

        /// <summary>
        /// Gets the redundancy entities which represent links to redundant files.
        /// </summary>
        /// <value>The redundancy entities which represent links to redundant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancies), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamRedundancy> Redundancies { get; }
    }
}
