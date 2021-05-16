using System.Linq;

namespace FsInfoCat.Model.Local
{
    public interface ILocalDbContext : IDbContext
    {
        new IQueryable<ILocalHashCalculation> Checksums { get; }
        new IQueryable<ILocalFileComparison> Comparisons { get; }
        new IQueryable<ILocalFile> Files { get; }
        new IQueryable<ILocalSubDirectory> Subdirectories { get; }
        new IQueryable<ILocalVolume> Volumes { get; }
        new IQueryable<ILocalSymbolicName> FsSymbolicNames { get; }
        new IQueryable<ILocalFileSystem> FileSystems { get; }
        new IQueryable<ILocalRedundancy> Redundancies { get; }
    }
}
