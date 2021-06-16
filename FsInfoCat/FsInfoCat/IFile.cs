using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a file in its hierarchical structure.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    public interface IFile : IDbFsItem
    {
        FileCrawlOptions Options { get; set; }

        DateTime? LastHashCalculation { get; set; }

        bool Deleted { get; }

        IContentInfo Content { get; set; }

        IExtendedProperties ExtendedProperties { get; set; }

        IRedundancy Redundancy { get; }

        IEnumerable<IComparison> ComparisonSources { get; }

        IEnumerable<IComparison> ComparisonTargets { get; }

        new IEnumerable<IAccessError<IFile>> AccessErrors { get; }
    }
}
