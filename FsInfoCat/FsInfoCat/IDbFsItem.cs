using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IDbFsItem : IDbEntity
    {
        Guid Id { get; set; }

        string Name { get; set; }

        DateTime LastAccessed { get; set; }

        string Notes { get; set; }

        DateTime CreationTime { get; set; }

        DateTime LastWriteTime { get; set; }

        ISubdirectory Parent { get; set; }

        IEnumerable<IAccessError> AccessErrors { get; }
    }
}
