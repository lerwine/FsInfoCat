using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public interface IHasCompoundIdentifier
    {
        [NotNull]
        IEnumerable<Guid> Id { get; }
    }
}
