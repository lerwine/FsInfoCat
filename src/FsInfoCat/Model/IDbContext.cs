using System.Linq;

namespace FsInfoCat.Model
{
    public interface IDbContext
    {
        IQueryable<IHashCalculation> Checksums { get; }
        IQueryable<IFileComparison> Comparisons { get; }
        IQueryable<IFile> Files { get; }
        IQueryable<ISubDirectory> Subdirectories { get; }
        IQueryable<IVolume> Volumes { get; }
        IQueryable<IFsSymbolicName> FsSymbolicNames { get; }
        IQueryable<IFileSystem> FileSystems { get; }
        IQueryable<IRedundancy> Redundancies { get; }
    }
}
