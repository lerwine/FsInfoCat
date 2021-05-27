using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class LocalDbContext : DbContext
    {
        private static readonly object _syncRoot = new object();
        private static bool _connectionStringValidated;
        private readonly ILogger<LocalDbContext> _logger;
        private readonly IDisposable _loggerScope;

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<Subdirectory> Subdirectories { get; set; }

        public virtual DbSet<DbFile> Files { get; set; }

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
                    SqliteConnectionStringBuilder builder = new(Database.GetConnectionString());
                    string connectionString = builder.ConnectionString;
                    _logger.LogInformation($"Using {nameof(SqliteConnectionStringBuilder.ConnectionString)} {{{nameof(SqliteConnectionStringBuilder.ConnectionString)}}}",
                        connectionString);
                    if (!File.Exists(builder.DataSource))
                    {
                        builder.Mode = SqliteOpenMode.ReadWriteCreate;
                        _logger.LogInformation("Initializing new database");
                        using (SqliteConnection connection = new(builder.ConnectionString))
                        {
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
        }

        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize",
            Justification = "Inherited class would have called SuppressFinalize if it were necessary")]
        public override void Dispose()
        {
            _logger.LogInformation($"Disposing {nameof(LocalDbContext)}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}",
                ContextId.InstanceId, ContextId.Lease);
            base.Dispose();
            _loggerScope.Dispose();
        }

        public override int SaveChanges()
        {
            var entities = from e in ChangeTracker.Entries() where e.State == EntityState.Added || e.State == EntityState.Modified select e.Entity;
            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).Select(e => e.Entity))
                Validator.ValidateObject(entity, new ValidationContext(entity), true);
            return base.SaveChanges();
        }

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

        internal delegate void EntityEntryNormalizationHandler<T>([NotNull] EntityEntry<T> entityEntry, [NotNull] LocalDbContext dbContext)
            where T : class, ILocalDbEntity;

        internal delegate void EntityEntryValidationHandler<T>([NotNull] EntityEntry<T> entityEntry, [NotNull] LocalDbContext dbContext,
            [NotNull] List<ValidationResult> validationResults) where T : class, ILocalDbEntity;

        internal static List<ValidationResult> GetBasicLocalDbEntityValidationResult<T>([NotNull] T entity, [NotNull] LocalDbContext dbContext, [NotNull] out EntityEntry<T> entityEntry)
            where T : class, ILocalDbEntity
        {
            entityEntry = dbContext.Entry(entity);
            List<ValidationResult> result = new();
            switch (entityEntry.State)
            {
                case EntityState.Deleted:
                    return result;
                case EntityState.Added:
                    entity.CreatedOn = entity.ModifiedOn = DateTime.Now;
                    if (entity.UpstreamId.HasValue && !entity.LastSynchronizedOn.HasValue)
                        entity.LastSynchronizedOn = entity.ModifiedOn;
                    break;
                case EntityState.Unchanged:
                    if (entity.UpstreamId.HasValue && !entity.LastSynchronizedOn.HasValue)
                        entity.LastSynchronizedOn = entity.ModifiedOn = DateTime.Now;
                    break;
                default:
                    entity.ModifiedOn = DateTime.Now;
                    if (entity.UpstreamId.HasValue && !entity.LastSynchronizedOn.HasValue)
                        entity.LastSynchronizedOn = entity.ModifiedOn;
                    break;
            }
            if (entity.CreatedOn.CompareTo(entity.ModifiedOn) > 0)
                result.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, new string[] { nameof(ILocalDbEntity.CreatedOn) }));
            DateTime? lastSynchronizedOn = entity.LastSynchronizedOn;
            if (lastSynchronizedOn.HasValue)
            {
                if (lastSynchronizedOn.Value.CompareTo(entity.CreatedOn) < 0)
                    result.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, new string[] { nameof(ILocalDbEntity.LastSynchronizedOn) }));
                if (lastSynchronizedOn.Value.CompareTo(entity.ModifiedOn) > 0)
                    result.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, new string[] { nameof(ILocalDbEntity.LastSynchronizedOn) }));
            }
            return result;
        }

        //internal static void ValidateLocalDbEntity([NotNull] ILocalDbEntity entity, [NotNull] ICollection<ValidationResult> validationResults)
        //{
        //    if (entity.CreatedOn.CompareTo(entity.ModifiedOn) > 0)
        //    {
        //        validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, new string[] { nameof(ILocalDbEntity.CreatedOn) }));
        //        if (entity.UpstreamId.HasValue && !entity.LastSynchronizedOn.HasValue)
        //            validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, new string[] { nameof(ILocalDbEntity.LastSynchronizedOn) }));
        //    }
        //    else if (entity.LastSynchronizedOn.HasValue)
        //    {
        //        if (entity.LastSynchronizedOn.Value.CompareTo(entity.ModifiedOn) > 0)
        //            validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, new string[] { nameof(ILocalDbEntity.LastSynchronizedOn) }));
        //        else if (entity.LastSynchronizedOn.Value.CompareTo(entity.CreatedOn) < 0)
        //            validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, new string[] { nameof(ILocalDbEntity.LastSynchronizedOn) }));
        //    }
        //    else if (entity.UpstreamId.HasValue)
        //        validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, new string[] { nameof(ILocalDbEntity.LastSynchronizedOn) }));
        //}

        internal static List<ValidationResult> GetBasicLocalDbEntityValidationResult<T>([NotNull] T entity, [MaybeNull] ValidationContext validationContext, [NotNull] EntityEntryValidationHandler<T> onValidate)
            where T : class, ILocalDbEntity
        {
            List<ValidationResult> result;
            if (!(validationContext is null) && validationContext.ObjectInstance is LocalDbContext dbContext)
            {
                result = GetBasicLocalDbEntityValidationResult(entity, dbContext, out EntityEntry<T> entityEntry);
                onValidate(entityEntry, dbContext, result);
            }
            else
                using (dbContext = Services.ServiceProvider.GetService<LocalDbContext>())
                {
                    result = GetBasicLocalDbEntityValidationResult(entity, dbContext, out EntityEntry<T> entityEntry);
                    onValidate(entityEntry, dbContext, result);
                }
            return result;
        }

        internal static List<ValidationResult> GetBasicLocalDbEntityValidationResult<T>([NotNull] T entity, [MaybeNull] ValidationContext validationContext, [NotNull] out EntityEntry<T> entityEntry)
            where T : class, ILocalDbEntity
        {
            if (!(validationContext is null) && validationContext.ObjectInstance is LocalDbContext dbContext)
                return GetBasicLocalDbEntityValidationResult(entity, dbContext, out entityEntry);
            using (dbContext = Services.ServiceProvider.GetService<LocalDbContext>())
                return GetBasicLocalDbEntityValidationResult(entity, dbContext, out entityEntry);
        }

        public static void ConfigureServices(IServiceCollection services, Assembly assembly, string dbFileName)
        {
            string connectionString = GetConnectionString(assembly, dbFileName);
            services.AddDbContextPool<LocalDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
        }

        public static string GetConnectionString(Assembly assembly, string dbFileName)
        {
            // BUG: Cannot do this - Services not yet built.
            //var logger = Services.ServiceProvider.GetRequiredService<ILogger<LocalDbContext>>();
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = GetDbFilePath(assembly, dbFileName),
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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlite(connectionString);
        //}
    }
}
