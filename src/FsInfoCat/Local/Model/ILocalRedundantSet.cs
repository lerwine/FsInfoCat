using FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="ILocalRedundantSetRow" />
    /// <seealso cref="IRedundantSet" />
    /// <seealso cref="IEquatable{ILocalRedundantSet}" />
    /// <seealso cref="Upstream.Model.IUpstreamRedundantSet" />
    public interface ILocalRedundantSet : ILocalRedundantSetRow, IRedundantSet, IEquatable<ILocalRedundantSet>
    {
        /// <summary>
        /// Gets the binary properties in common with all files in the current redundant set.
        /// </summary>
        /// <value>The binary properties in common with all files in the current redundant set.</value>
        [Display(Name = nameof(Properties.Resources.BinaryProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalBinaryPropertySet BinaryProperties { get; }

        /// <summary>
        /// Gets the redundancy entities which represent links to redundant files.
        /// </summary>
        /// <value>The redundancy entities which represent links to redundant files.</value>
        [Display(Name = nameof(Properties.Resources.Redundancies), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalRedundancy> Redundancies { get; }
    }
}
