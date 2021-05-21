using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IVolume : IDbEntity
    {
        string DisplayName { get; set; }

        string VolumeName { get; set; }

        string Identifier { get; set; }

        System.IO.DriveType Type { get; set; }

        bool? CaseSensitiveSearch { get; set; }

        bool? ReadOnly { get; set; }

        long? MaxNameLength { get; set; }

        string Notes { get; set; }

        bool IsInactive { get; set; }

        IFileSystem FileSystem { get; set; }

        IEnumerable<IFile> RootFiles { get; }

        IEnumerable<ISubdirectory> RootDirectories { get; }
    }
    public interface ISubdirectory : IDbEntity
    {
        string Name { get; set; }

        DirectoryCrawlOptions Options { get; set; }

        string Notes { get; set; }

        bool Deleted { get; set; }

        ISubdirectory Parent { get; set; }

        IVolume Volume { get; set; }

        IEnumerable<IFile> Files { get; }

        IEnumerable<ISubdirectory> ChildDirectories { get; }
    }
    public interface IFile : IDbEntity
    {
        string Name { get; set; }

        FileCrawlOptions Options { get; set; }

        DateTime LastAccessed { get; set; }

        DateTime LastHashCalculation { get; set; }

        string Notes { get; set; }

        bool Deleted { get; set; }

        IRedundancy Redundancy { get; set; }

        IContentInfo Content { get; set; }

        ISubdirectory Parent { get; set; }

        IVolume Volume { get; set; }
    }
    public interface IContentInfo : IDbEntity
    {
        long Length { get; set; }

        IReadOnlyList<byte> Hash { get; }

        IEnumerable<IFile> Files { get; }
    }
    public interface IRedundantSet : IDbEntity
    {
        string Notes { get; set; }
    }
    public interface IRedundancy : IDbEntity
    {

    }
    public interface IComparison : IDbEntity
    {

    }
}
