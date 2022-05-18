using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{

    /// <summary>
    /// Represents a set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="Local.ILocalRedundantSet" />
    /// <seealso cref="Upstream.IUpstreamRedundantSet" />
    /// <seealso cref="IBinaryPropertySet.RedundantSets" />
    /// <seealso cref="IRedundancy.RedundantSet" />
    /// <seealso cref="IDbContext.RedundantSets" />
    public interface IRedundantSet : IRedundantSetRow, IEquatable<IRedundantSet>
    {
        /// <summary>
        /// Gets the binary properties in common with all files in the current redundant set.
        /// </summary>
        /// <value>The binary properties in common with all files in the current redundant set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        IBinaryPropertySet BinaryProperties { get; }

        /// <summary>
        /// Gets the redundancy entities which represent links to redundant files.
        /// </summary>
        /// <value>The redundancy entities which represent links to redundant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancies), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IRedundancy> Redundancies { get; }

        /// <summary>
        /// Attempts the get the primary key of the associated binary properties entity.
        /// </summary>
        /// <param name="binaryPropertiesId">The <see cref="IHasSimpleIdentifier.Id"/> value of the associated <see cref="IBinaryPropertySet"/>.</param>
        /// <returns><see langword="true"/> if <see cref="BinaryProperties"/> has a primary key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetBinaryPropertiesId(out Guid binaryPropertiesId);
    }
}
