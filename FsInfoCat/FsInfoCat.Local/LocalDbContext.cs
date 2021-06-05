using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public partial class LocalDbContext : DbContext
    {
        private static readonly object _syncRoot = new();
        private static bool _connectionStringValidated;
        private readonly ILogger<LocalDbContext> _logger;
        private readonly IDisposable _loggerScope;

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<VolumeAccessError> VolumeAccessErrors { get; set; }

        public virtual DbSet<Subdirectory> Subdirectories { get; set; }

        public virtual DbSet<SubdirectoryAccessError> SubdirectoryAccessErrors { get; set; }

        public virtual DbSet<DbFile> Files { get; set; }

        public virtual DbSet<FileAccessError> FileAccessErrors { get; set; }

        public virtual DbSet<ExtendedProperties> ExtendedProperties { get; set; }

        public virtual DbSet<ContentInfo> ContentInfos { get; set; }

        public virtual DbSet<FileComparison> Comparisons { get; set; }

        public virtual DbSet<RedundantSet> RedundantSets { get; set; }

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        {
            _logger = Services.ServiceProvider.GetRequiredService<ILogger<LocalDbContext>>();
            _loggerScope = _logger.BeginScope(ContextId);
            _logger.LogInformation($"Creating new {nameof(LocalDbContext)}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}",
                ContextId.InstanceId, ContextId.Lease);
            lock (_syncRoot)
            {
                if (!_connectionStringValidated)
                {
                    _connectionStringValidated = true;
                    SqliteConnectionStringBuilder builder = new(Database.GetConnectionString());
                    string connectionString = builder.ConnectionString;
                    _logger.LogInformation($"Using {nameof(SqliteConnectionStringBuilder.ConnectionString)} {{{nameof(SqliteConnectionStringBuilder.ConnectionString)}}}",
                        connectionString);
                    if (!File.Exists(builder.DataSource))
                    {
                        builder.Mode = SqliteOpenMode.ReadWriteCreate;
                        _logger.LogInformation("Initializing new database");
                        using SqliteConnection connection = new(builder.ConnectionString);
                        connection.Open();
                        foreach (var element in XDocument.Parse(Properties.Resources.DbCommands).Root.Elements("DbCreation").Elements("Text"))
                        {
                            _logger.LogInformation($"{{Message}}; {nameof(SqliteCommand)}={{{nameof(SqliteCommand.CommandText)}}}",
                                element.Attributes("Message").Select(a => a.Value).DefaultIfEmpty("").First(), element.Value);
                            using SqliteCommand command = connection.CreateCommand();
                            command.CommandText = element.Value;
                            command.CommandType = System.Data.CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Inherited class will have called SuppressFinalize if necessary.")]
        public override void Dispose()
        {
            _logger.LogInformation($"Disposing {nameof(LocalDbContext)}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}",
                ContextId.InstanceId, ContextId.Lease);
            base.Dispose();
            _loggerScope.Dispose();
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var e in entries)
            {
                ValidationContext validationContext = new(e.Entity, new DbContextServiceProvider(this, e), null);
                if (e.Entity is IDbEntity dbEntity)
                    dbEntity.BeforeSave(validationContext);
                Validator.ValidateObject(e.Entity, validationContext, true);
            }
            int result = base.SaveChanges();
            foreach (var e in entries)
            {
                if (e.State == EntityState.Unchanged && e.Entity is IDbEntity dbEntity)
                    dbEntity.AcceptChanges();

            }
            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            //modelBuilder.Entity<FileSystem>(FileSystem.BuildEntity);
            modelBuilder.Entity<SymbolicName>(SymbolicName.BuildEntity);
            modelBuilder.Entity<Volume>(Volume.BuildEntity);
            modelBuilder.Entity<Subdirectory>(Subdirectory.BuildEntity);
            modelBuilder.Entity<DbFile>(DbFile.BuildEntity);
            modelBuilder.Entity<ContentInfo>(ContentInfo.BuildEntity);
            modelBuilder.Entity<FileComparison>(FileComparison.BuildEntity);
            modelBuilder.Entity<RedundantSet>(RedundantSet.BuildEntity);
            modelBuilder.Entity<Redundancy>(Redundancy.BuildEntity);
        }

        public static void ConfigureServices(IServiceCollection services, Assembly assembly, string dbFileName) =>
            ConfigureServices(services, GetDbFilePath(assembly, dbFileName));

        public static void ConfigureServices(IServiceCollection services, string dbPath)
        {
            string connectionString = GetConnectionString(dbPath);
            services.AddDbContextPool<LocalDbContext>(options =>
            {
                options.AddInterceptors(Interceptor.Instance);
                options.UseSqlite(connectionString);
            });
        }

        public static string GetConnectionString(Assembly assembly, string dbFileName) => GetConnectionString(GetDbFilePath(assembly, dbFileName));

        public static string GetConnectionString(string dbPath)
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                ForeignKeys = true,
                Mode = SqliteOpenMode.ReadWrite
            };
            return builder.ConnectionString;
        }

        public static string GetDbFilePath(Assembly assembly, string dbFileName)
        {
            if (string.IsNullOrWhiteSpace(dbFileName))
                dbFileName = Services.DEFAULT_LOCAL_DB_FILENAME;
            if (Path.IsPathFullyQualified(dbFileName))
                return Path.GetFullPath(dbFileName);
            return Path.Combine(Services.GetAppDataPath(assembly), dbFileName);
        }
    }
}
