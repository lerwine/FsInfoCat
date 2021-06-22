using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace FsInfoCat.LocalDb
{

    public class LocalDbContext : DbContext
    {
        private static readonly ILogger<LocalDbContext> _logger = Services.GetLoggingService().CreateLogger<LocalDbContext>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<FileSystem>(FileSystem.BuildEntity);
            modelBuilder.Entity<SymbolicName>(SymbolicName.BuildEntity);
            modelBuilder.Entity<Volume>(Volume.BuildEntity);
            modelBuilder.Entity<FsDirectory>(FsDirectory.BuildEntity);
            modelBuilder.Entity<FsFile>(FsFile.BuildEntity);
            modelBuilder.Entity<ContentInfo>(ContentInfo.BuildEntity);
            modelBuilder.Entity<RedundantSet>(RedundantSet.BuildEntity);
            modelBuilder.Entity<Redundancy>(Redundancy.BuildEntity);
            modelBuilder.Entity<FileComparison>(FileComparison.BuildEntity);
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
            _logger.LogInformation($"Initializing {nameof(ILocalDbService)} with {{{nameof(SqlCeConnectionStringBuilder.ConnectionString)}}}", builder.ConnectionString);
            localDbService.SetConnectionString(builder.ConnectionString);
            localDbService.SetContextFactory(() => new SharedDbContext());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Services.GetLocalDbService().GetConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string not initialized");
            SqlCeConnectionStringBuilder builder = new SqlCeConnectionStringBuilder(connectionString);
            if (File.Exists(builder.DataSource))
                _logger.LogInformation($"Local database file found: {{{nameof(SqlCeConnectionStringBuilder.DataSource)}}}", builder.DataSource);
            else
            {
                _logger.LogInformation($"{{{nameof(SqlCeConnectionStringBuilder.DataSource)}}} does not exist. Creating database", builder.DataSource);
                using (SqlCeEngine engine = new SqlCeEngine(builder.ConnectionString))
                    engine.CreateDatabase();
                using (SqlCeConnection connection = new SqlCeConnection(builder.ConnectionString))
                {
                    XDocument document = XDocument.Parse(Properties.Resources.SqlCommands);
                    _logger.LogInformation($"Opening {nameof(SqlCeConnection)}: {{{nameof(SqlCeConnectionStringBuilder.ConnectionString)}}}", builder.ConnectionString);
                    connection.Open();
                    foreach (XElement element in document.Root.Elements("CreateTables").Elements("Text").Where(e => !e.IsEmpty && e.Value.Trim().Length > 0))
                    {
                        string text = element.Value.Trim();
                        _logger.LogInformation($"{element.Attributes("Message").Select(a => a.Value).DefaultIfEmpty("Executing SQL command").First()}: {{{nameof(SqlCeCommand.CommandText)}}}", text);
                        using (SqlCeCommand command = connection.CreateCommand())
                        {
                            command.CommandType = System.Data.CommandType.Text;
                            command.CommandText = text;
                            command.ExecuteNonQuery();
                        }
                    }
                    _logger.LogInformation($"Closing {nameof(SqlCeConnection)}", builder.ConnectionString);
                }
            }

            _logger.LogInformation($"Configuring {nameof(LocalDbContext)}: {{{nameof(SqlCeConnectionStringBuilder.ConnectionString)}}}", builder.ConnectionString);
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
