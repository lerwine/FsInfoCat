using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a set of files that are identical.
    /// </summary>
    public interface IRedundantSet : IDbEntity
    {
        Guid Id { get; set; }

        string Reference { get; set; }

        string Notes { get; set; }

        IContentInfo ContentInfo { get; set; }

        IEnumerable<IRedundancy> Redundancies { get; }
    }
}
