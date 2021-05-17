using System.Linq;

namespace FsInfoCat.Model.Local
{
    public interface ILocalDbContext : IDbContext
    {
        new IQueryable<ILocalHashCalculation> HashCalculations { get; }
        new IQueryable<ILocalFileComparison> Comparisons { get; }
        new IQueryable<ILocalFile> Files { get; }
        new IQueryable<ILocalSubDirectory> Subdirectories { get; }
        new IQueryable<ILocalVolume> Volumes { get; }
        new IQueryable<ILocalSymbolicName> SymbolicNames { get; }
        new IQueryable<ILocalFileSystem> FileSystems { get; }
        new IQueryable<ILocalRedundancy> Redundancies { get; }

        void AddHashCalculation(ILocalHashCalculation hashCalculation);
        void UpdateHashCalculation(ILocalHashCalculation hashCalculation);
        void RemoveHashCalculation(ILocalHashCalculation hashCalculation);
        void AddComparison(ILocalFileComparison fileComparison);
        void UpdateComparison(ILocalFileComparison fileComparison);
        void RemoveComparison(ILocalFileComparison fileComparison);
        void AddFile(ILocalFile file);
        void UpdateFile(ILocalFile file);
        void RemoveFile(ILocalFile file);
        void AddSubDirectory(ILocalSubDirectory subDirectory);
        void UpdateSubDirectory(ILocalSubDirectory subDirectory);
        void RemoveSubDirectory(ILocalSubDirectory subDirectory);
        void AddVolume(ILocalVolume volume);
        void UpdateVolume(ILocalVolume volume);
        void RemoveVolume(ILocalVolume volume);
        void AddSymbolicName(ILocalSymbolicName symbolicName);
        void UpdateSymbolicName(ILocalSymbolicName symbolicName);
        void RemoveSymbolicName(ILocalSymbolicName symbolicName);
        void AddFileSystem(ILocalFileSystem fileSystem);
        void UpdateFileSystem(ILocalFileSystem fileSystem);
        void RemoveFileSystem(ILocalFileSystem fileSystem);
        void AddRedundancy(ILocalRedundancy redundancy);
        void UpdateRedundancy(ILocalRedundancy redundancy);
        void RemoveRedundancy(ILocalRedundancy redundancy);
    }
}
