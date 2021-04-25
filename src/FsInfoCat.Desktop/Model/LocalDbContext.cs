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
            if (!System.IO.File.Exists(path))
                throw new InvalidOperationException("Database not initialized.");
            SqlCeConnectionStringBuilder connectionStringBuilder = new SqlCeConnectionStringBuilder();
            connectionStringBuilder.DataSource = path;
            return connectionStringBuilder.ConnectionString;
        }

        public static LocalDbContext GetDbContext()
        {
            string path = GetDbFilePath();
            if (string.IsNullOrWhiteSpace(path))
                throw new InvalidOperationException("Database path not specified.");
            SqlCeConnectionStringBuilder connectionStringBuilder = new SqlCeConnectionStringBuilder();
            connectionStringBuilder.DataSource = path;
            if (System.IO.File.Exists(path))
                return new LocalDbContext(connectionStringBuilder.ConnectionString);
            using (SqlCeEngine sqlCeEngine = new SqlCeEngine(connectionStringBuilder.ConnectionString))
                sqlCeEngine.CreateDatabase();
            LocalDbContext dbContext = new LocalDbContext(connectionStringBuilder.ConnectionString);
            try { dbContext.Database.Initialize(true); }
            catch
            {
                dbContext.Dispose();
                System.IO.File.Delete(path);
                throw;
            }
            return dbContext;
        }

        public LocalDbContext() : base(GetConnectionString())
        {
        }

        private LocalDbContext(string connectionString) : base(string.IsNullOrWhiteSpace(connectionString) ? GetConnectionString() : connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalVolume>().HasKey(v => v.Id);
            modelBuilder.Entity<LocalVolume>().Property(v => v.DisplayName).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<LocalVolume>().Property(v => v.RootPathName).HasMaxLength(1024).IsRequired();
            modelBuilder.Entity<LocalVolume>().Property(v => v.DriveFormat).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<LocalVolume>().Property(v => v.VolumeName).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<LocalVolume>().Property(v => v.Identifier).HasMaxLength(1024).IsRequired();
            modelBuilder.Entity<LocalVolume>().Property(v => v.Notes).IsMaxLength();
            modelBuilder.Entity<LocalDirectory>().HasKey(d => d.Id);
            modelBuilder.Entity<LocalDirectory>().Property(d => d.Name).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<LocalDirectory>().HasRequired(d => d.Volume).WithMany(v => v.SubDirectories);
            modelBuilder.Entity<LocalDirectory>().HasOptional(d => d.ParentDirectory).WithMany(d => d.SubDirectories);
            modelBuilder.Entity<LocalFile>().HasKey(f => f.Id);
            modelBuilder.Entity<LocalFile>().Property(f => f.Name).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<LocalFile>().HasRequired(f => f.ParentDirectory).WithMany(d => d.Files);
            modelBuilder.Entity<LocalFile>().HasOptional(f => f.ChecksumCalculation).WithMany(c => c.Files);
            modelBuilder.Entity<LocalFile>().HasMany(f => f.Comparisons1).WithRequired(c => c.File1).HasForeignKey(c => c.FileId1);
            // BUG: The referential relationship will result in a cyclical reference that is not allowed. [ Constraint name = FK_dbo.LocalComparisons_dbo.LocalFiles_FileId2 ]
            modelBuilder.Entity<LocalFile>().HasMany(f => f.Comparisons2).WithRequired(c => c.File2).HasForeignKey(c => c.FileId2);
            modelBuilder.Entity<LocalChecksumCalculation>().HasKey(v => v.Id);
            modelBuilder.Entity<LocalChecksumCalculation>().Property(v => v.Checksum).HasMaxLength(MD5Checksum.MD5ByteSize).IsFixedLength().IsRequired();
            modelBuilder.Entity<LocalComparison>().HasKey(c => new { c.FileId1, c.FileId2 });
            base.OnModelCreating(modelBuilder);
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
