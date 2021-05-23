using System;

namespace FsInfoCat
{
    public interface ISymbolicName : IDbEntity
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Notes { get; set; }

        int Priority { get; set; }

        bool IsInactive { get; set; }

        IFileSystem FileSystem { get; set; }
    }
}
