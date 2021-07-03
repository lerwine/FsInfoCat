using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a set of files that have the same size, Hash and remediation status.
    /// </summary>
    public interface IRedundantSet : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
        Guid Id { get; set; }

        RedundancyRemediationStatus RemediationStatus { get; }

        string Reference { get; set; }

        string Notes { get; set; }

        IBinaryPropertySet BinaryProperties { get; set; }

        IEnumerable<IRedundancy> Redundancies { get; }
    }
}
