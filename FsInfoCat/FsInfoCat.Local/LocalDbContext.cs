using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public class LocalDbContext : DbContext
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
            foreach (var e in entities)
                // BUG: Relying primarily on IValidatableObject is less than optimal - current implementation gets a DbContext which could possible occur at an inopportune time.
                Validator.ValidateObject(e, new ValidationContext(e), true);
            return base.SaveChanges();
        }

        public class Interceptor : DbCommandInterceptor
        {
            private readonly ILogger<Interceptor> _logger;

            public static Interceptor Instance { get; } = new Interceptor();

            public Interceptor() { _logger = Services.ServiceProvider.GetRequiredService<ILogger<Interceptor>>(); }

            public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
            {
                _logger.LogInformation("NonQueryExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.NonQueryExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
                CancellationToken cancellationToken = default)
            {
                _logger.LogInformation("NonQueryExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
            {
                _logger.LogInformation("ReaderExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ReaderExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
            {
                _logger.LogInformation("ReaderExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
            {
                _logger.LogInformation("ScalarExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ScalarExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
            {
                _logger.LogInformation("ScalarExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
            {
                _logger.LogInformation("CommandCreated: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    result.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.CommandCreated(eventData, result);
            }

            public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
            {
                _logger.LogError(eventData.Exception, "CommandFailed: {Message}; CommandText={CommandText}, ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode, eventData.Exception.Message);
                base.CommandFailed(command, eventData);
            }

            public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
            {
                _logger.LogError(eventData.Exception, "CommandFailedAsync: {Message}; CommandText={CommandText}, ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode, eventData.Exception.Message);
                return base.CommandFailedAsync(command, eventData, cancellationToken);
            }
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

        // TODO: Separate normalization initiated by IValidatableObject.Validate(ValidationContext) from normalization within a database context.
        internal delegate void EntityEntryNormalizationHandler<T>([NotNull] EntityEntry<T> entityEntry, [NotNull] LocalDbContext dbContext)
            where T : class, IDbEntity;

        // TODO: Separate validation invoked by IValidatableObject.Validate(ValidationContext) from validation within a database context.
        internal delegate void EntityEntryValidationHandler<T>([NotNull] EntityEntry<T> entityEntry, [NotNull] LocalDbContext dbContext,
            [NotNull] List<ValidationResult> validationResults) where T : class, IDbEntity;

        // TODO: Separate validation invoked by IValidatableObject.Validate(ValidationContext) from validation within a database context.
        internal static List<ValidationResult> GetBasicLocalDbEntityValidationResult<T>([NotNull] T entity, [NotNull] LocalDbContext dbContext, [NotNull] out EntityEntry<T> entityEntry)
            where T : class, IDbEntity
        {
            entityEntry = dbContext.Entry(entity);
            List<ValidationResult> result = new();
            entityEntry.DetectChanges();
            DateTime now = DateTime.Now;
            switch (entityEntry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Deleted:
                    return result;
                case EntityState.Added:
                    if (entityEntry.Property(nameof(ILocalDbEntity.CreatedOn)).IsModified)
                    {
                        if (!entityEntry.Property(nameof(ILocalDbEntity.ModifiedOn)).IsModified)
                            entity.ModifiedOn = (entity.CreatedOn > now) ? entity.CreatedOn : now;
                    }
                    else if (entityEntry.Property(nameof(ILocalDbEntity.ModifiedOn)).IsModified)
                        entity.CreatedOn = (entity.ModifiedOn > now) ? now : entity.ModifiedOn;
                    else
                        entity.CreatedOn = entity.ModifiedOn = now;
                    if (entity is ILocalDbEntity localEntity1 && localEntity1.UpstreamId.HasValue &&
                        !entityEntry.Property(nameof(ILocalDbEntity.LastSynchronizedOn)).IsModified && localEntity1.CreatedOn <= localEntity1.ModifiedOn)
                        localEntity1.LastSynchronizedOn = (localEntity1.CreatedOn > now) ? localEntity1.CreatedOn :
                            ((localEntity1.ModifiedOn < now) ? localEntity1.ModifiedOn : now);
                    break;
                default:
                    if (!entityEntry.Property(nameof(ILocalDbEntity.ModifiedOn)).IsModified)
                        entity.ModifiedOn = (entity.CreatedOn > now) ? entity.CreatedOn : now;
                    if (entity is ILocalDbEntity localEntity2 && entityEntry.Property(nameof(ILocalDbEntity.UpstreamId)).IsModified &&
                            !entityEntry.Property(nameof(ILocalDbEntity.LastSynchronizedOn)).IsModified)
                        localEntity2.LastSynchronizedOn = (localEntity2.CreatedOn > now) ? localEntity2.CreatedOn :
                            ((localEntity2.ModifiedOn < now) ? localEntity2.ModifiedOn : now);
                    break;
            }
            if (entity is ILocalDbEntity localEntity3)
            {
                DateTime? lastSynchronizedOn = localEntity3.LastSynchronizedOn;
                if (localEntity3.CreatedOn.CompareTo(localEntity3.ModifiedOn) > 0)
                {
                    result.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, new string[] { nameof(ILocalDbEntity.CreatedOn) }));
                    if (localEntity3.UpstreamId.HasValue && !lastSynchronizedOn.HasValue)
                        result.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnRequired,
                            new string[] { nameof(ILocalDbEntity.LastSynchronizedOn) }));
                    return result;
                }
                if (localEntity3.UpstreamId.HasValue)
                {
                    if (!lastSynchronizedOn.HasValue)
                    {
                        result.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnRequired,
                            new string[] { nameof(ILocalDbEntity.LastSynchronizedOn) }));
                        return result;
                    }
                }
                else if (!lastSynchronizedOn.HasValue)
                    return result;
                if (lastSynchronizedOn.Value.CompareTo(localEntity3.CreatedOn) < 0)
                    result.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn,
                        new string[] { nameof(ILocalDbEntity.LastSynchronizedOn) }));
                if (lastSynchronizedOn.Value.CompareTo(localEntity3.ModifiedOn) > 0)
                    result.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn,
                        new string[] { nameof(ILocalDbEntity.LastSynchronizedOn) }));
            }
            else if (entity.CreatedOn.CompareTo(entity.ModifiedOn) > 0)
                result.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, new string[] { nameof(ILocalDbEntity.CreatedOn) }));
            return result;
        }

        // BUG: Bad idea to use DbContext on something called by IValidatableObject.Validate(ValidationContext) since it can't always be controlled when and how often validation might occur.
        internal static List<ValidationResult> GetBasicLocalDbEntityValidationResult<T>([NotNull] T entity, [NotNull] EntityEntryValidationHandler<T> onValidate)
            where T : class, IDbEntity
        {
            List<ValidationResult> result;
            using (LocalDbContext dbContext = Services.ServiceProvider.GetService<LocalDbContext>())
            {
                result = GetBasicLocalDbEntityValidationResult(entity, dbContext, out EntityEntry<T> entityEntry);
                onValidate(entityEntry, dbContext, result);
            }
            return result;
        }

        // BUG: Bad idea to use DbContext on something called by IValidatableObject.Validate(ValidationContext) since it can't always be controlled when and how often validation might occur.
        internal static List<ValidationResult> GetBasicLocalDbEntityValidationResult<T>([NotNull] T entity, [MaybeNull] ValidationContext validationContext, [NotNull] out EntityEntry<T> entityEntry)
            where T : class, IDbEntity
        {
            // BUG: ValidationContext.ObjectInstance should never be a DbContext object
            if (!(validationContext is null) && validationContext.ObjectInstance is LocalDbContext dbContext)
                return GetBasicLocalDbEntityValidationResult(entity, dbContext, out entityEntry);
            using (dbContext = Services.ServiceProvider.GetService<LocalDbContext>())
                return GetBasicLocalDbEntityValidationResult(entity, dbContext, out entityEntry);
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
