using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Desktop.Model.Local
{
    public class LocalDbContainer : DbContext
    {
        // Your context has been configured to use a 'LocalDbContainer' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'FsInfoCat.Desktop.Model.Local.LocalDbContainer' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'LocalDbContainer' 
        // connection string in the application configuration file.
        public LocalDbContainer()
            : base("name=LocalDbContainer")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<FsSymbolicName> FsSymbolicNames { get; set; }
        public virtual DbSet<FileSystem> FileSystems { get; set; }
        public virtual DbSet<Volume> Volumes { get; set; }
        public virtual DbSet<Directory> Directories { get; set; }
        public virtual DbSet<Comparison> Comparisons { get; set; }
        public virtual DbSet<HashCalculation> HashCalculations { get; set; }
        public virtual DbSet<Redundancy> Redundancies { get; set; }
        public virtual DbSet<File> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<FileSystem>().HasKey(p => p.Id);
            modelBuilder.Entity<FileSystem>().Property(t => t.DisplayName).HasMaxLength(128).IsRequired().IsVariableLength();
            modelBuilder.Entity<FileSystem>().Property(t => t.DefaultDriveType).IsOptional();
            modelBuilder.Entity<FileSystem>().Property(t => t.Notes).IsMaxLength().IsRequired().IsVariableLength();
            modelBuilder.Entity<FileSystem>().HasRequired(t => t.DefaultSymbolicName).WithMany(d => d.DefaultFileSystems).HasForeignKey(t => t.DefaultSymbolicNameId);
            modelBuilder.Entity<FsSymbolicName>().HasKey(p => p.Id);
            modelBuilder.Entity<FsSymbolicName>().Property(t => t.Name).HasMaxLength(128).IsRequired().IsVariableLength();
            modelBuilder.Entity<FsSymbolicName>().Property(t => t.Notes).IsMaxLength().IsRequired().IsVariableLength();
            modelBuilder.Entity<FsSymbolicName>().HasRequired(p => p.FileSystem).WithMany(d => d.SymbolicNames).HasForeignKey(p => p.FileSystemId);
            modelBuilder.Entity<Volume>().HasKey(p => p.Id);
            modelBuilder.Entity<Volume>().Property(t => t.DisplayName).HasMaxLength(128).IsRequired().IsVariableLength();
            modelBuilder.Entity<Volume>().Property(t => t.VolumeName).HasMaxLength(128).IsRequired().IsVariableLength();
            modelBuilder.Entity<Volume>().Property(t => t.Identifier).HasMaxLength(1024).IsRequired().IsVariableLength();
            modelBuilder.Entity<Volume>().Property(t => t.CaseSensitiveSearch).IsOptional();
            modelBuilder.Entity<Volume>().Property(t => t.ReadOnly).IsOptional();
            modelBuilder.Entity<Volume>().Property(t => t.MaxNameLength).IsOptional();
            modelBuilder.Entity<Volume>().Property(t => t.Notes).IsMaxLength().IsRequired().IsVariableLength();
            modelBuilder.Entity<Volume>().HasRequired(p => p.FileSystem).WithMany(d => d.Volumes);
            modelBuilder.Entity<Volume>().HasRequired(p => p.RootDirectory).WithOptional(d => d.Volume);
            modelBuilder.Entity<Directory>().HasKey(p => p.Id);
            modelBuilder.Entity<Directory>().Property(t => t.Name).HasMaxLength(128).IsRequired().IsVariableLength();
            modelBuilder.Entity<Directory>().HasOptional(p => p.Parent).WithMany(d => d.SubDirectories).HasForeignKey(p => p.ParentId);
            modelBuilder.Entity<Directory>().HasOptional(p => p.SourceRelocationTask).WithMany(d => d.SourceDirectories).HasForeignKey(p => p.SourceRelocationTaskId);
            modelBuilder.Entity<HashCalculation>().HasKey(p => p.Id);
            modelBuilder.Entity<HashCalculation>().Property(t => t.Data).HasMaxLength(16).IsFixedLength().IsOptional();
            modelBuilder.Entity<File>().HasKey(p => p.Id);
            modelBuilder.Entity<File>().Property(t => t.Name).HasMaxLength(1024).IsRequired().IsVariableLength();
            modelBuilder.Entity<File>().HasRequired(p => p.Parent).WithMany(d => d.Files).HasForeignKey(p => p.ParentId);
            modelBuilder.Entity<File>().HasRequired(p => p.HashCalculation).WithMany(d => d.Files).HasForeignKey(p => p.HashCalculationId);
            modelBuilder.Entity<File>().HasOptional(p => p.FileRelocateTask).WithMany(d => d.Files).HasForeignKey(p => p.FileRelocateTaskId);
            modelBuilder.Entity<Comparison>().HasKey(p => p.Id);
            modelBuilder.Entity<Comparison>().HasRequired(p => p.File1).WithMany(d => d.Comparisons1).Map(m =>
            {
                m.ToTable($"{nameof(File)}{nameof(Comparison)}1");
                m.MapKey(nameof(Comparison.FileId1));
            });
            modelBuilder.Entity<Comparison>().HasRequired(p => p.File2).WithMany(d => d.Comparisons2).Map(m =>
            {
                m.ToTable($"{nameof(File)}{nameof(Comparison)}2");
                m.MapKey(nameof(Comparison.FileId2));
            });
            modelBuilder.Entity<Redundancy>().HasKey(p => p.Id);
            modelBuilder.Entity<Redundancy>().HasMany(p => p.Files).WithMany(d => d.Redundancies).Map(m =>
            {
                m.ToTable($"{nameof(Redundancy)}{nameof(File)}");
                m.MapLeftKey(nameof(Redundancy.Id));
                m.MapRightKey(nameof(File.Id));
            });
            modelBuilder.Entity<DirectoryRelocateTask>().HasKey(p => p.Id);
            modelBuilder.Entity<DirectoryRelocateTask>().HasRequired(p => p.TargetDirectory).WithMany(d => d.TargetDirectoryRelocationTasks);
            modelBuilder.Entity<FileRelocateTask>().HasKey(p => p.Id);
            modelBuilder.Entity<FileRelocateTask>().HasRequired(p => p.TargetDirectory).WithMany(d => d.FileRelocationTasks);
            base.OnModelCreating(modelBuilder);
        }

        private static LocalDbContainer _localDbContainer;
        private static readonly object _syncRoot = new object();
        internal static LocalDbContainer GetDbContext()
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (_localDbContainer is null)
                    _localDbContainer = new LocalDbContainer();
            }
            finally { Monitor.Exit(_syncRoot); }
            return _localDbContainer;
        }
    }
}
