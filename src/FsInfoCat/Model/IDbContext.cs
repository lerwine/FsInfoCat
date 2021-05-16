using System.Linq;

namespace FsInfoCat.Model
{
    // TODO: Move to FsInfoCat module
    public interface IDbContext
    {
        IQueryable<IHashCalculation> Checksums { get; }
        IQueryable<IFileComparison> Comparisons { get; }
        IQueryable<IFile> Files { get; }
        IQueryable<ISubDirectory> Subdirectories { get; }
        IQueryable<IVolume> Volumes { get; }
    }
}
