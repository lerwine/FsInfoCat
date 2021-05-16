using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FsInfoCat.LocalDb
{
    public class FsInfoCatContext : DbContext, ILocalDbContext
    {
        public static Func<ILocalDbContext> GetContextFactory(string dbFileName, Assembly assembly)
        {
            AssemblyCompanyAttribute companyAttr = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyAttr.Company);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            AssemblyName name = assembly.GetName();
            path = Path.Combine(path, name.Name);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, name.Version.ToString());
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, dbFileName ?? Services.DEFAULT_LOCAL_DB_FILENAME);
            return new Func<ILocalDbContext>(() => Open(path));
        }

        public virtual DbSet<FsSymbolicName> FsSymbolicNames { get; set; }

        IQueryable<ILocalSymbolicName> ILocalDbContext.FsSymbolicNames => FsSymbolicNames;

        IQueryable<IFsSymbolicName> IDbContext.FsSymbolicNames => FsSymbolicNames;

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        IQueryable<ILocalFileSystem> ILocalDbContext.FileSystems => FileSystems;

        IQueryable<IFileSystem> IDbContext.FileSystems => FileSystems;

        public virtual DbSet<Volume> Volumes { get; set; }

        IQueryable<ILocalVolume> ILocalDbContext.Volumes => Volumes;

        IQueryable<IVolume> IDbContext.Volumes => Volumes;

        public virtual DbSet<FsDirectory> FsDirectories { get; set; }

        IQueryable<ILocalSubDirectory> ILocalDbContext.Subdirectories => FsDirectories;

        IQueryable<ISubDirectory> IDbContext.Subdirectories => FsDirectories;

        public virtual DbSet<Comparison> Comparisons { get; set; }

        IQueryable<ILocalFileComparison> ILocalDbContext.Comparisons => Comparisons;

        IQueryable<IFileComparison> IDbContext.Comparisons => Comparisons;

        public virtual DbSet<HashCalculation> HashCalculations { get; set; }

        IQueryable<ILocalHashCalculation> ILocalDbContext.Checksums => HashCalculations;

        IQueryable<IHashCalculation> IDbContext.Checksums => HashCalculations;

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        IQueryable<ILocalRedundancy> ILocalDbContext.Redundancies => Redundancies;

        IQueryable<IRedundancy> IDbContext.Redundancies => Redundancies;

        public virtual DbSet<FsFile> FsFiles { get; set; }

        IQueryable<ILocalFile> ILocalDbContext.Files => FsFiles;

        IQueryable<IFile> IDbContext.Files => FsFiles;

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
            base.OnModelCreating(modelBuilder);
        }
    }
}
