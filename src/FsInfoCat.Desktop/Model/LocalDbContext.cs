using System.Data.Entity;
using System.Linq;

namespace FsInfoCat.Desktop.Model
{
    public class LocalDbContext : DbContext, IDbContext
    {
        public LocalDbContext()
            : base("name=FsInfoCat.Desktop.Properties.Settings.LocalDbConnectionString")
        {
        }

        public DbSet<LocalVolume> Volumes { get; set; }
        public DbSet<LocalDirectory> Subdirectories { get; set; }
        public DbSet<LocalFile> Files { get; set; }
        public DbSet<LocalChecksumCalculation> ChecksumCalculations { get; set; }
        public DbSet<LocalComparison> Comparisons { get; set; }

        IQueryable<IChecksumCalculation> IDbContext.Checksums => ChecksumCalculations.Cast<IChecksumCalculation>();

        IQueryable<IFileComparison> IDbContext.Comparisons => Comparisons.Cast<IFileComparison>();

        IQueryable<IFile> IDbContext.Files => Files.Cast<IFile>();

        IQueryable<ISubDirectory> IDbContext.Subdirectories => Subdirectories.Cast<ISubDirectory>();

        IQueryable<IVolume> IDbContext.Volumes => Volumes.Cast<IVolume>();
    }
}
