using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FsInfoCat.LocalDb
{
    public class FsInfoCatContext : DbContext, ILocalDbContext
    {
        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<FsDirectory> Directories { get; set; }

        public virtual DbSet<FileComparison> Comparisons { get; set; }

        public virtual DbSet<ContentHash> HashCalculations { get; set; }

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        public virtual DbSet<FsFile> Files { get; set; }

        #region Explicit Members

        IQueryable<ILocalSymbolicName> ILocalDbContext.SymbolicNames => SymbolicNames;

        IQueryable<IFsSymbolicName> IDbContext.SymbolicNames => SymbolicNames;

        IQueryable<ILocalFileSystem> ILocalDbContext.FileSystems => FileSystems;

        IQueryable<IFileSystem> IDbContext.FileSystems => FileSystems;

        IQueryable<ILocalVolume> ILocalDbContext.Volumes => Volumes;

        IQueryable<IVolume> IDbContext.Volumes => Volumes;

        IQueryable<ILocalSubDirectory> ILocalDbContext.Subdirectories => Directories;

        IQueryable<ISubDirectory> IDbContext.Subdirectories => Directories;

        IQueryable<ILocalFileComparison> ILocalDbContext.Comparisons => Comparisons;

        IQueryable<IFileComparison> IDbContext.Comparisons => Comparisons;

        IQueryable<ILocalContentHash> ILocalDbContext.HashCalculations => HashCalculations;

        IQueryable<IContentHash> IDbContext.HashCalculations => HashCalculations;

        IQueryable<ILocalRedundancy> ILocalDbContext.Redundancies => Redundancies;

        IQueryable<IRedundancy> IDbContext.Redundancies => Redundancies;

        IQueryable<ILocalFile> ILocalDbContext.Files => Files;

        IQueryable<IFile> IDbContext.Files => Files;

        #endregion

        private FsInfoCatContext(DbContextOptions options)
        {

        }

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
            modelBuilder.Entity<SymbolicName>(SymbolicName.BuildEntity);
            modelBuilder.Entity<Volume>(Volume.BuildEntity);
            modelBuilder.Entity<FsDirectory>(FsDirectory.BuildEntity);
            modelBuilder.Entity<ContentHash>(ContentHash.BuildEntity);
            modelBuilder.Entity<FsFile>(FsFile.BuildEntity);
            modelBuilder.Entity<FileComparison>(FileComparison.BuildEntity);
            modelBuilder.Entity<Redundancy>(Redundancy.BuildEntity);
            base.OnModelCreating(modelBuilder);
        }

        public IDbContextTransaction BeginTransaction() => Database.BeginTransaction();

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel) => Database.BeginTransaction(isolationLevel);

        internal EntityEntry<ContentHash> AddHashCalculation(ContentHash hashCalculation)
        {
            HashCalculations.Attach(hashCalculation ?? throw new ArgumentNullException(nameof(hashCalculation)));
            return HashCalculations.Add(hashCalculation);
        }

        internal EntityEntry<ContentHash> UpdateHashCalculation(ContentHash hashCalculation)
        {
            HashCalculations.Attach(hashCalculation ?? throw new ArgumentNullException(nameof(hashCalculation)));
            return HashCalculations.Update(hashCalculation);
        }

        internal EntityEntry<ContentHash> RemoveHashCalculation(ContentHash hashCalculation)
        {
            HashCalculations.Attach(hashCalculation ?? throw new ArgumentNullException(nameof(hashCalculation)));
            return HashCalculations.Remove(hashCalculation);
        }

        internal EntityEntry<FileComparison> AddComparison(FileComparison fileComparison)
        {
            Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return Comparisons.Add(fileComparison);
        }

        internal EntityEntry<FileComparison> UpdateComparison(FileComparison fileComparison)
        {
            Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return Comparisons.Update(fileComparison);
        }

        internal EntityEntry<FileComparison> RemoveComparison(FileComparison fileComparison)
        {
            Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return Comparisons.Remove(fileComparison);
        }

        internal EntityEntry<FsFile> AddFile(FsFile file)
        {
            Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return Files.Add(file);
        }

        internal EntityEntry<FsFile> UpdateFile(FsFile file)
        {
            Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return Files.Update(file);
        }

        internal EntityEntry<FsFile> RemoveFile(FsFile file)
        {
            Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return Files.Remove(file);
        }

        internal EntityEntry<FsDirectory> AddSubDirectory(FsDirectory subdirectory)
        {
            Directories.Attach(subdirectory ?? throw new ArgumentNullException(nameof(subdirectory)));
            return Directories.Add(subdirectory);
        }

        internal EntityEntry<FsDirectory> UpdateSubDirectory(FsDirectory subdirectory)
        {
            Directories.Attach(subdirectory ?? throw new ArgumentNullException(nameof(subdirectory)));
            return Directories.Update(subdirectory);
        }

        internal EntityEntry<FsDirectory> RemoveSubDirectory(FsDirectory subdirectory)
        {
            Directories.Attach(subdirectory ?? throw new ArgumentNullException(nameof(subdirectory)));
            return Directories.Remove(subdirectory);
        }

        internal EntityEntry<Volume> AddVolume(Volume volume)
        {
            Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return Volumes.Add(volume);
        }

        internal EntityEntry<Volume> UpdateVolume(Volume volume)
        {
            Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return Volumes.Update(volume);
        }

        internal EntityEntry<Volume> RemoveVolume(Volume volume)
        {
            Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return Volumes.Remove(volume);
        }

        internal EntityEntry<SymbolicName> AddSymbolicName(SymbolicName symbolicName)
        {
            SymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return SymbolicNames.Add(symbolicName);
        }

        internal EntityEntry<SymbolicName> UpdateSymbolicName(SymbolicName symbolicName)
        {
            SymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return SymbolicNames.Update(symbolicName);
        }

        internal EntityEntry<SymbolicName> RemoveSymbolicName(SymbolicName symbolicName)
        {
            SymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return SymbolicNames.Remove(symbolicName);
        }

        internal EntityEntry<FileSystem> AddFileSystem(FileSystem fileSystem)
        {
            FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return FileSystems.Add(fileSystem);
        }

        internal EntityEntry<FileSystem> UpdateFileSystem(FileSystem fileSystem)
        {
            FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return FileSystems.Update(fileSystem);
        }

        internal EntityEntry<FileSystem> RemoveFileSystem(FileSystem fileSystem)
        {
            FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return FileSystems.Remove(fileSystem);
        }

        internal EntityEntry<Redundancy> AddRedundancy(Redundancy redundancy)
        {
            Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return Redundancies.Add(redundancy);
        }

        internal EntityEntry<Redundancy> UpdateRedundancy(Redundancy redundancy)
        {
            Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return Redundancies.Update(redundancy);
        }

        internal EntityEntry<Redundancy> RemoveRedundancy(Redundancy redundancy)
        {
            Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return Redundancies.Remove(redundancy);
        }

        void ILocalDbContext.AddHashCalculation(ILocalContentHash hashCalculation) => AddHashCalculation((ContentHash)hashCalculation);

        void ILocalDbContext.UpdateHashCalculation(ILocalContentHash hashCalculation) => UpdateHashCalculation((ContentHash)hashCalculation);

        void ILocalDbContext.RemoveHashCalculation(ILocalContentHash hashCalculation) => RemoveHashCalculation((ContentHash)hashCalculation);

        void ILocalDbContext.AddComparison(ILocalFileComparison fileComparison) => AddComparison((FileComparison)fileComparison);

        void ILocalDbContext.UpdateComparison(ILocalFileComparison fileComparison) => UpdateComparison((FileComparison)fileComparison);

        void ILocalDbContext.RemoveComparison(ILocalFileComparison fileComparison) => RemoveComparison((FileComparison)fileComparison);

        void ILocalDbContext.AddFile(ILocalFile file) => AddFile((FsFile)file);

        void ILocalDbContext.UpdateFile(ILocalFile file) => UpdateFile((FsFile)file);

        void ILocalDbContext.RemoveFile(ILocalFile file) => RemoveFile((FsFile)file);

        void ILocalDbContext.AddSubDirectory(ILocalSubDirectory subDirectory) => AddSubDirectory((FsDirectory)subDirectory);

        void ILocalDbContext.UpdateSubDirectory(ILocalSubDirectory subDirectory) => UpdateSubDirectory((FsDirectory)subDirectory);

        void ILocalDbContext.RemoveSubDirectory(ILocalSubDirectory subDirectory) => RemoveSubDirectory((FsDirectory)subDirectory);

        void ILocalDbContext.AddVolume(ILocalVolume volume) => AddVolume((Volume)volume);

        void ILocalDbContext.UpdateVolume(ILocalVolume volume) => UpdateVolume((Volume)volume);

        void ILocalDbContext.RemoveVolume(ILocalVolume volume) => RemoveVolume((Volume)volume);

        void ILocalDbContext.AddSymbolicName(ILocalSymbolicName symbolicName) => AddSymbolicName((SymbolicName)symbolicName);

        void ILocalDbContext.UpdateSymbolicName(ILocalSymbolicName symbolicName) => UpdateSymbolicName((SymbolicName)symbolicName);

        void ILocalDbContext.RemoveSymbolicName(ILocalSymbolicName symbolicName) => RemoveSymbolicName((SymbolicName)symbolicName);

        void ILocalDbContext.AddFileSystem(ILocalFileSystem fileSystem) => AddFileSystem((FileSystem)fileSystem);

        void ILocalDbContext.UpdateFileSystem(ILocalFileSystem fileSystem) => UpdateFileSystem((FileSystem)fileSystem);

        void ILocalDbContext.RemoveFileSystem(ILocalFileSystem fileSystem) => RemoveFileSystem((FileSystem)fileSystem);

        void ILocalDbContext.AddRedundancy(ILocalRedundancy redundancy) => AddRedundancy((Redundancy)redundancy);

        void ILocalDbContext.UpdateRedundancy(ILocalRedundancy redundancy) => UpdateRedundancy((Redundancy)redundancy);

        void ILocalDbContext.RemoveRedundancy(ILocalRedundancy redundancy) => RemoveRedundancy((Redundancy)redundancy);

        public bool HasChanges() => ChangeTracker.HasChanges();
    }
}
