using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlServerCe;
using System.Linq;

namespace FsInfoCat.LocalDb
{
    public class FsInfoCatContext : DbContext, IDbContext
    {
        public virtual DbSet<FsSymbolicName> FsSymbolicNames { get; set; }

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<FsDirectory> FsDirectories { get; set; }

        public virtual DbSet<Comparison> Comparisons { get; set; }

        public virtual DbSet<HashCalculation> HashCalculations { get; set; }

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        public virtual DbSet<FsFile> FsFiles { get; set; }

        public virtual DbSet<FileRelocateTask> FileRelocateTasks { get; set; }

        public virtual DbSet<DirectoryRelocateTask> DirectoryRelocateTasks { get; set; }

        IQueryable<IHashCalculation> IDbContext.Checksums => HashCalculations;

        IQueryable<ISubDirectory> IDbContext.Subdirectories => FsDirectories;

        IQueryable<IFileComparison> IDbContext.Comparisons => Comparisons;

        IQueryable<IFile> IDbContext.Files => FsFiles;

        IQueryable<IVolume> IDbContext.Volumes => Volumes;

        private FsInfoCatContext(DbContextOptions options)
        {

        }

        public static FsInfoCatContext Open(string path)
        {
            SqlCeConnectionStringBuilder sqlCeConnectionStringBuilder = new SqlCeConnectionStringBuilder();
            sqlCeConnectionStringBuilder.DataSource = path;
            sqlCeConnectionStringBuilder.PersistSecurityInfo = true;
            if (!System.IO.File.Exists(path))
            {
                using (SqlCeEngine engine = new SqlCeEngine(sqlCeConnectionStringBuilder.ConnectionString))
                    engine.CreateDatabase();
            }
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: Need to change this
#warning Need to change connection string
            optionsBuilder.UseSqlCe(@"Data Source=C:\data\Blogging.sdf");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<FileSystem>(FileSystem.BuildEntity);
            modelBuilder.Entity<FsSymbolicName>(FsSymbolicName.BuildEntity);
            modelBuilder.Entity<Volume>(Volume.BuildEntity);
            modelBuilder.Entity<FsDirectory>(FsDirectory.BuildEntity);
            modelBuilder.Entity<HashCalculation>(HashCalculation.BuildEntity);
            modelBuilder.Entity<FsFile>(FsFile.BuildEntity);
            modelBuilder.Entity<Comparison>(Comparison.BuildEntity);
            modelBuilder.Entity<Redundancy>(Redundancy.BuildEntity);
            modelBuilder.Entity<DirectoryRelocateTask>(DirectoryRelocateTask.BuildEntity);
            modelBuilder.Entity<FileRelocateTask>(FileRelocateTask.BuildEntity);
            base.OnModelCreating(modelBuilder);
        }
    }
}
