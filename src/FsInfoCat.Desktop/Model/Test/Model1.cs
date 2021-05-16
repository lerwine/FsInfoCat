using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace FsInfoCat.Desktop.Model.Test
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Comparison> Comparisons { get; set; }
        public virtual DbSet<Directory> Directories { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<FileSystem> FileSystems { get; set; }
        public virtual DbSet<FsSymbolicName> FsSymbolicNames { get; set; }
        public virtual DbSet<HashCalculation> HashCalculations { get; set; }
        public virtual DbSet<Redundancy> Redundancies { get; set; }
        public virtual DbSet<Volume> Volumes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Directory>()
                .HasMany(e => e.Files)
                .WithRequired(e => e.Directory)
                .HasForeignKey(e => e.ParentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Directory>()
                .HasMany(e => e.Volumes)
                .WithRequired(e => e.Directory)
                .HasForeignKey(e => e.RootDirectory_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Directory>()
                .HasMany(e => e.Directories1)
                .WithOptional(e => e.Directory1)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<File>()
                .HasMany(e => e.Comparisons)
                .WithRequired(e => e.File)
                .HasForeignKey(e => e.FileId1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<File>()
                .HasMany(e => e.Comparisons1)
                .WithRequired(e => e.File1)
                .HasForeignKey(e => e.FileId2)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<File>()
                .HasMany(e => e.Redundancies)
                .WithMany(e => e.Files)
                .Map(m => m.ToTable("RedundancyFile").MapLeftKey("Files_Id").MapRightKey("Redundancies_Id"));

            modelBuilder.Entity<FileSystem>()
                .HasMany(e => e.FsSymbolicNames)
                .WithRequired(e => e.FileSystem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FileSystem>()
                .HasMany(e => e.Volumes)
                .WithRequired(e => e.FileSystem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HashCalculation>()
                .Property(e => e.Data)
                .IsFixedLength();

            modelBuilder.Entity<HashCalculation>()
                .HasMany(e => e.Files)
                .WithRequired(e => e.HashCalculation)
                .WillCascadeOnDelete(false);
        }
    }
}
