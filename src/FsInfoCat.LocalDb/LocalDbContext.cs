using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlServerCe;
using System.IO;
using System.Reflection;

namespace FsInfoCat.LocalDb
{

    public class LocalDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<FileSystem>(FileSystem.BuildEntity);
            modelBuilder.Entity<SymbolicName>(SymbolicName.BuildEntity);
            modelBuilder.Entity<Volume>(Volume.BuildEntity);
            modelBuilder.Entity<FsDirectory>(FsDirectory.BuildEntity);
            modelBuilder.Entity<ContentInfo>(ContentInfo.BuildEntity);
            modelBuilder.Entity<FsFile>(FsFile.BuildEntity);
            modelBuilder.Entity<FileComparison>(FileComparison.BuildEntity);
            modelBuilder.Entity<Redundancy>(Redundancy.BuildEntity);
            modelBuilder.Entity<RedundantSet>(RedundantSet.BuildEntity);
            base.OnModelCreating(modelBuilder);
        }

        internal LocalDbContext() { }

        public static void Initialize(string dbFileName, Assembly assembly)
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
            SqlCeConnectionStringBuilder builder = new SqlCeConnectionStringBuilder();
            builder.DataSource = path;
            builder.PersistSecurityInfo = true;
            Services.GetLocalDbService().SetConnectionString(builder.ConnectionString);
            if (!File.Exists(path))
            {
                using (SqlCeEngine engine = new SqlCeEngine(builder.ConnectionString))
                    engine.CreateDatabase();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlCe(Services.GetLocalDbService().GetConnectionString());
        }

        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<FsDirectory> Directories { get; set; }

        public virtual DbSet<FileComparison> Comparisons { get; set; }

        public virtual DbSet<ContentInfo> HashInfo { get; set; }

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        public virtual DbSet<RedundantSet> RedundantSets { get; set; }

        public virtual DbSet<FsFile> Files { get; set; }

    }
}
