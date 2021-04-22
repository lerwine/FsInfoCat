using System.Data.Entity;

namespace FsInfoCat.Desktop.Model
{
    public class LocalDbContext : DbContext
    {
        public LocalDbContext()
            : base("name=FsInfoCat.Desktop.Properties.Settings.LocalDbConnectionString")
        {
        }

        public DbSet<LocalVolume> Volumes { get; set; }
        public DbSet<LocalDirectory> Subdirectories { get; set; }
        public DbSet<LocalFile> Files { get; set; }
        public DbSet<LocalChecksumCalculation> Checksums { get; set; }
        public DbSet<LocalComparison> Comparisons { get; set; }
    }
}
