using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IFile : IDbEntity
    {
        string Name { get; set; }

        FileCrawlOptions Options { get; set; }

        DateTime LastAccessed { get; set; }

        DateTime? LastHashCalculation { get; set; }

        string Notes { get; set; }

        bool Deleted { get; set; }

        IContentInfo Content { get; set; }

        ISubdirectory Parent { get; set; }

        IEnumerable<IComparison> ComparisonSources { get; }

        IEnumerable<IComparison> ComparisonTargets { get; }

        IEnumerable<IRedundancy> Redundancies { get; }
    }
}
