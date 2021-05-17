using System;
using System.Collections.Generic;

namespace FsInfoCat.Model
{
    public interface IRedundancy
    {
        Guid Id { get; }

        IReadOnlyCollection<IFile> Files { get; }
    }
}
