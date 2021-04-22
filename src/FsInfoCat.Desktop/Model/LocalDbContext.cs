using System;
using System.Data.Entity;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;

namespace FsInfoCat.Desktop.Model
{
    public class LocalDbContext : DbContext, IDbContext
    {
        public static string GetDbFilePath()
        {
            string path = Properties.Settings.Default.LocalDbFile;
            if (string.IsNullOrWhiteSpace(path))
                return null;
            return Path.GetFullPath(Path.IsPathRooted(path) ? path : Path.Combine(App.EnsureAppDataPath().FullName, path));
        }

        public static string GetConnectionString()
        {
            string path = GetDbFilePath();
            if (string.IsNullOrWhiteSpace(path))
                throw new InvalidOperationException("Database path not specified.");
            SqlCeConnectionStringBuilder connectionStringBuilder = new SqlCeConnectionStringBuilder();
            connectionStringBuilder.DataSource = path;
            return connectionStringBuilder.ConnectionString;
        }

        public LocalDbContext() : base(GetConnectionString())
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
