using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a file in its hierarchical structure.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    public interface IFile : IDbEntity
    {
        Guid Id { get; set; }

        string Name { get; set; }

        FileCrawlOptions Options { get; set; }

        DateTime LastAccessed { get; set; }

        DateTime? LastHashCalculation { get; set; }

        string Notes { get; set; }

#warning Replace with status of Normal, AccessError, Deleted
        bool Deleted { get; set; }

        IContentInfo Content { get; set; }

        IExtendedProperties ExtendedProperties { get; set; }

        ISubdirectory Parent { get; set; }

        IRedundancy Redundancy { get; }

        IEnumerable<IComparison> ComparisonSources { get; }

        IEnumerable<IComparison> ComparisonTargets { get; }

        IEnumerable<IAccessError<IFile>> AccessErrors { get; }
    }
}
