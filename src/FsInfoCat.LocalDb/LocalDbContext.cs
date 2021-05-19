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
            ILocalDbService localDbService = Services.GetLocalDbService();
            localDbService.SetConnectionString(builder.ConnectionString);
            localDbService.SetContextFactory(() => new SharedDbContext());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Services.GetLocalDbService().GetConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string not initialized");
            SqlCeConnectionStringBuilder builder = new SqlCeConnectionStringBuilder(connectionString);
            if (!File.Exists(builder.DataSource))
            {
                using (SqlCeEngine engine = new SqlCeEngine(builder.ConnectionString))
                    engine.CreateDatabase();
                using (SqlCeConnection connection = new SqlCeConnection())
                {
                    using (SqlCeCommand command = connection.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = Properties.Resources.DbInitialization;
                        command.ExecuteNonQuery();
                    }
                }
            }

            optionsBuilder.UseSqlCe(connectionString);
        }

        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<FsDirectory> Directories { get; set; }

        public virtual DbSet<FileComparison> Comparisons { get; set; }

        public virtual DbSet<ContentInfo> ContentInfos { get; set; }

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        public virtual DbSet<RedundantSet> RedundantSets { get; set; }

        public virtual DbSet<FsFile> Files { get; set; }

    }
}
