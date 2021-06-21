using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a set of files that have the same size, Hash and remediation status.
    /// </summary>
    public interface IRedundantSet : IDbEntity
    {
        Guid Id { get; set; }

        RedundancyRemediationStatus RemediationStatus { get; }

        string Reference { get; set; }

        string Notes { get; set; }

        IBinaryProperties BinaryProperties { get; set; }

        IEnumerable<IRedundancy> Redundancies { get; }
    }
}
