using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public class LocalDbContext : DbContext
    {
        public virtual DbSet<FileSystem> FileSystems { get; set; }

        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<Subdirectory> Subdirectories { get; set; }

        public virtual DbSet<DbFile> Files { get; set; }

        public virtual DbSet<ContentInfo> ContentInfos { get; set; }

        public virtual DbSet<FileComparison> Comparison { get; set; }

        public virtual DbSet<RedundantSet> RedundantSets { get; set; }

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        {
            //SavingChanges += LocalDbContext_SavingChanges;
        }

        //private void LocalDbContext_SavingChanges(object sender, SavingChangesEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            //modelBuilder.Entity<FileSystem>(FileSystem.BuildEntity);
            modelBuilder.Entity<SymbolicName>(SymbolicName.BuildEntity);
            modelBuilder.Entity<Volume>(Volume.BuildEntity);
            modelBuilder.Entity<Subdirectory>(Subdirectory.BuildEntity);
            modelBuilder.Entity<DbFile>(DbFile.BuildEntity);
            //modelBuilder.Entity<ContentInfo>(ContentInfo.BuildEntity);
            modelBuilder.Entity<FileComparison>(FileComparison.BuildEntity);
            modelBuilder.Entity<RedundantSet>(RedundantSet.BuildEntity);
            modelBuilder.Entity<Redundancy>(Redundancy.BuildEntity);
        }

        public static void ConfigureServices(IServiceCollection services, Assembly assembly, string dbFileName)
        {
            string connectionString = GetConnectionString(assembly, dbFileName);
            services.AddDbContext<LocalDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
        }

        public static string GetConnectionString(Assembly assembly, string dbFileName, bool doNotCreate = false)
        {
            var builder = new SqliteConnectionStringBuilder();
            builder.DataSource = GetDbFilePath(assembly, dbFileName, doNotCreate);
            builder.ForeignKeys = true;
            builder.Mode = SqliteOpenMode.ReadWrite;
            string connectionString = builder.ConnectionString;
            if (doNotCreate || File.Exists(builder.DataSource))
                return connectionString;
            builder.Mode = SqliteOpenMode.ReadWriteCreate;
            using (SqliteConnection connection = new SqliteConnection(builder.ConnectionString))
            {
                connection.Open();
                foreach (var element in XDocument.Parse(Properties.Resources.DbCommands).Root.Elements("DbCreation").Elements("Text"))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = element.Value;
                        command.CommandType = System.Data.CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }
            }
            return connectionString;
        }

        public static string GetDbFilePath(Assembly assembly, string dbFileName, bool doNotCreate = false)
        {
            if (string.IsNullOrWhiteSpace(dbFileName))
                dbFileName = Services.DEFAULT_LOCAL_DB_FILENAME;
            if (Path.IsPathFullyQualified(dbFileName))
                return Path.GetFullPath(dbFileName);

            return Path.Combine(Services.GetAppDataPath(assembly, doNotCreate), dbFileName);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlite(connectionString);
        //}
    }
}
