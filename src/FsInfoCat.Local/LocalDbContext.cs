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
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public partial class LocalDbContext : DbContext, ILocalDbContext
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

        public virtual DbSet<SummaryPropertySet> SummaryPropertySets { get; }

        public virtual DbSet<DocumentPropertySet> DocumentPropertySets { get; }

        public virtual DbSet<AudioPropertySet> AudioPropertySets { get; }

        public virtual DbSet<DRMPropertySet> DRMPropertySets { get; }

        public virtual DbSet<GPSPropertySet> GPSPropertySets { get; }

        public virtual DbSet<ImagePropertySet> ImagePropertySets { get; }

        public virtual DbSet<MediaPropertySet> MediaPropertySets { get; }

        public virtual DbSet<MusicPropertySet> MusicPropertySets { get; }

        public virtual DbSet<PhotoPropertySet> PhotoPropertySets { get; }

        public virtual DbSet<RecordedTVPropertySet> RecordedTVPropertySets { get; }

        public virtual DbSet<VideoPropertySet> VideoPropertySets { get; }

        public virtual DbSet<BinaryPropertySet> BinaryPropertySets { get; set; }

        public virtual DbSet<FileComparison> Comparisons { get; set; }

        public virtual DbSet<RedundantSet> RedundantSets { get; set; }

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        public virtual DbSet<CrawlConfiguration> CrawlConfigurations { get; set; }

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
            modelBuilder.Entity<SymbolicName>(SymbolicName.BuildEntity);
            modelBuilder.Entity<Volume>(Volume.BuildEntity);
            modelBuilder.Entity<Subdirectory>(Subdirectory.BuildEntity);
            modelBuilder.Entity<DbFile>(DbFile.BuildEntity);
            modelBuilder.Entity<BinaryPropertySet>(BinaryPropertySet.BuildEntity);
            modelBuilder.Entity<FileComparison>(FileComparison.BuildEntity);
            modelBuilder.Entity<RedundantSet>(RedundantSet.BuildEntity);
            modelBuilder.Entity<Redundancy>(Redundancy.BuildEntity);
            modelBuilder.Entity<SummaryPropertySet>(SummaryPropertySet.BuildEntity);
            modelBuilder.Entity<DocumentPropertySet>(DocumentPropertySet.BuildEntity);
            modelBuilder.Entity<GPSPropertySet>(GPSPropertySet.BuildEntity);
            modelBuilder.Entity<MediaPropertySet>(MediaPropertySet.BuildEntity);
            modelBuilder.Entity<MusicPropertySet>(MusicPropertySet.BuildEntity);
            modelBuilder.Entity<PhotoPropertySet>(PhotoPropertySet.BuildEntity);
            modelBuilder.Entity<VideoPropertySet>(VideoPropertySet.BuildEntity);
        }

        public static void AddDbContextPool(IServiceCollection services, Assembly assembly, string dbFileName) =>
            AddDbContextPool(services, GetDbFilePath(assembly, dbFileName));

        public static void AddDbContextPool(IServiceCollection services, string dbPath)
        {
            string connectionString = GetConnectionString(dbPath);
            services.AddDbContextPool<LocalDbContext>(options =>
            {
                options.AddInterceptors(Interceptor.Instance);
                options.UseSqlite(connectionString);
            });
            // this.Model.GetDefaultSchema();
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

        public async Task ImportAsync(XDocument document)
        {
            if (document is null)
                throw new ArgumentNullException(nameof(document));

            var redundancySets = document.Root.Elements(nameof(BinaryPropertySets)).Select(e => BinaryPropertySet.ImportAsync(this, _logger, e).Result).SelectMany(rs => rs).ToArray();
            foreach (XElement fileSystemElement in document.Root.Elements(nameof(FileSystem)))
                await FileSystem.ImportAsync(this, _logger, fileSystemElement);
            foreach (var (redundantSetId, redundancies) in redundancySets)
                foreach (XElement element in redundancies)
                    await Redundancy.ImportAsync(this, _logger, redundantSetId, element);
        }

        [Obsolete("Use ForceDeleteBinaryPropertySetAsync")]
        public void ForceDeleteBinaryPropertySet(BinaryPropertySet target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            EntityEntry<BinaryPropertySet> targetEntry = Entry(target);
            var redundantSets = (targetEntry.GetRelatedCollectionAsync(t => t.RedundantSets).Result).ToArray();
            if (redundantSets.Length > 0)
            {
                foreach (var r in redundantSets)
                    ForceDeleteRedundantSet(r);
                SaveChanges();
            }
            var files = target.Files.AsEnumerable().Select(f => Entry(f)).ToArray();
            if (files.Length > 0)
            {
                SummaryPropertySet[] summaryProperties = files.Select(e => e.Entity.SummaryPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.SummaryProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                DocumentPropertySet[] documentProperties = files.Select(e => e.Entity.DocumentPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.DocumentProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                AudioPropertySet[] audioProperties = files.Select(e => e.Entity.AudioPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.AudioProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                DRMPropertySet[] drmProperties = files.Select(e => e.Entity.DRMPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.DRMProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                GPSPropertySet[] gpsProperties = files.Select(e => e.Entity.GPSPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.GPSProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                ImagePropertySet[] imageProperties = files.Select(e => e.Entity.ImagePropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.ImageProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                MediaPropertySet[] mediaProperties = files.Select(e => e.Entity.MediaPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.MediaProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                MusicPropertySet[] musicProperties = files.Select(e => e.Entity.MusicPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.MusicProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                PhotoPropertySet[] photoProperties = files.Select(e => e.Entity.PhotoPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.PhotoProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                RecordedTVPropertySet[] recordedTVProperties = files.Select(e => e.Entity.RecordedTVPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.RecordedTVProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                VideoPropertySet[] videoProperties = files.Select(e => e.Entity.VideoPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.VideoProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                FileComparison[] comparisons = files.SelectMany(e => e.GetRelatedCollectionAsync(f => f.ComparisonSources).Result
                    .Concat(e.GetRelatedCollectionAsync(f => f.ComparisonTargets).Result)).Distinct().ToArray();
                FileAccessError[] accessErrors = files.SelectMany(e => e.GetRelatedCollectionAsync(f => f.AccessErrors).Result).ToArray();
                bool hasChanges = comparisons.Length > 0;
                if (hasChanges)
                    Comparisons.RemoveRange(comparisons);
                if (accessErrors.Length > 0)
                {
                    FileAccessErrors.RemoveRange(accessErrors);
                    hasChanges = true;
                }
                if (hasChanges)
                    SaveChanges();
                Files.RemoveRange(files.Select(f => f.Entity));
                SaveChanges();
#pragma warning disable CA1827 // Do not use Count() or LongCount() when Any() can be used
                hasChanges = summaryProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0;
                if (documentProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (audioProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (drmProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (gpsProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (imageProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (mediaProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (musicProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (photoProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (recordedTVProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (videoProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
#pragma warning restore CA1827 // Do not use Count() or LongCount() when Any() can be used
                if (hasChanges)
                    SaveChanges();
            }

            BinaryPropertySets.Remove(target);
        }

        public void ForceDeleteRedundantSet(RedundantSet target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var redundancies = target.Redundancies.AsEnumerable().ToArray();
            if (redundancies.Length > 0)
            {
                Redundancies.RemoveRange(redundancies);
                SaveChanges();
            }
            RedundantSets.Remove(target);
        }

        public void ForceDeleteFileSystem(FileSystem target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var symbolicNames = target.SymbolicNames.AsEnumerable().ToArray();
            if (symbolicNames.Length > 0)
            {
                SymbolicNames.RemoveRange(symbolicNames);
                SaveChanges();
            }
            var volumes = target.Volumes.AsEnumerable().ToArray();
            if (volumes.Length > 0)
            {
                foreach (var v in volumes)
                    ForceDeleteVolume(v);
                SaveChanges();
            }
            FileSystems.Remove(target);
        }

        private void ForceDeleteVolume(Volume target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (target.RootDirectory is not null)
            {
                ForceDeleteSubdirectory(target.RootDirectory);
                SaveChanges();
            }
            var accessErrors = target.AccessErrors.AsEnumerable().ToArray();
            if (accessErrors.Length > 0)
            {
                VolumeAccessErrors.RemoveRange(accessErrors);
                SaveChanges();
            }
            Volumes.Remove(target);
        }

        private void ForceDeleteSubdirectory(Subdirectory target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var subdirectories = target.SubDirectories.AsEnumerable().ToArray();
            if (subdirectories.Length > 0)
            {
                foreach (var s in subdirectories)
                    ForceDeleteSubdirectory(s);
                SaveChanges();
            }
            var files = target.Files.AsEnumerable().ToArray();
            if (files.Length > 0)
            {
                foreach (var f in files)
#pragma warning disable CS0618 // Type or member is obsolete
                    ForceDeleteFileAsync(f).Wait();
#pragma warning restore CS0618 // Type or member is obsolete
                SaveChanges();
            }
            var accessErrors = target.AccessErrors.AsEnumerable().ToArray();
            if (accessErrors.Length > 0)
            {
                SubdirectoryAccessErrors.RemoveRange(accessErrors);
                SaveChanges();
            }
            Subdirectories.Remove(target);
        }

        [Obsolete("Pass cancellation token")]
        private async Task ForceDeleteFileAsync(DbFile target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (target.Redundancy is not null)
            {
                ForceDeleteRedundancy(target.Redundancy);
                SaveChanges();
            }
            EntityEntry<DbFile> fileEntry = Entry(target);
            var comparisons = (await fileEntry.GetRelatedCollectionAsync(p => p.ComparisonSources)).ToArray();
            bool hasChanges = comparisons.Length > 0;
            if (hasChanges)
                Comparisons.RemoveRange(comparisons);

            var accessErrors = (await fileEntry.GetRelatedCollectionAsync(p => p.AccessErrors)).ToArray();
            if (accessErrors.Length > 0)
            {
                FileAccessErrors.RemoveRange(accessErrors);
                hasChanges = true;
            }
            if (hasChanges)
                await SaveChangesAsync();
            var content = await fileEntry.GetRelatedReferenceAsync(f => f.BinaryProperties);
            var summaryProperties = target.SummaryPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.SummaryProperties) : null;
            var documentProperties = target.DocumentPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.DocumentProperties) : null;
            var audioProperties = target.AudioPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.AudioProperties) : null;
            var drmProperties = target.DRMPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.DRMProperties) : null;
            var gpsProperties = target.GPSPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.GPSProperties) : null;
            var imageProperties = target.ImagePropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.ImageProperties) : null;
            var mediaProperties = target.MediaPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.MediaProperties) : null;
            var musicProperties = target.MusicPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.MusicProperties) : null;
            var photoProperties = target.PhotoPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.PhotoProperties) : null;
            var recordedTVProperties = target.RecordedTVPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.RecordedTVProperties) : null;
            var videoProperties = target.VideoPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.VideoProperties) : null;
            Files.Remove(target);
            await SaveChangesAsync();

            hasChanges = await RemoveIfNoReferencesAsync(content);
            if (content.Files.Count == 0)
                BinaryPropertySets.Remove(content);
            if (summaryProperties is not null && await RemoveIfNoReferencesAsync(summaryProperties))
                hasChanges = true;
            if (documentProperties is not null && await RemoveIfNoReferencesAsync(documentProperties))
                hasChanges = true;
            if (audioProperties is not null && await RemoveIfNoReferencesAsync(audioProperties))
                hasChanges = true;
            if (drmProperties is not null && await RemoveIfNoReferencesAsync(drmProperties))
                hasChanges = true;
            if (gpsProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (imageProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (mediaProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (musicProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (photoProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (recordedTVProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (videoProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (hasChanges)
                await SaveChangesAsync();
        }

        private async Task ForceDeleteFileAsync(DbFile target, CancellationToken cancellationToken)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            cancellationToken.ThrowIfCancellationRequested();
            if (target.Redundancy is not null)
            {
                ForceDeleteRedundancy(target.Redundancy);
                await SaveChangesAsync(cancellationToken);
            }
            EntityEntry<DbFile> fileEntry = Entry(target);
            var comparisons = (await fileEntry.GetRelatedCollectionAsync(p => p.ComparisonSources, cancellationToken)).ToArray();
            bool hasChanges = comparisons.Length > 0;
            if (hasChanges)
                Comparisons.RemoveRange(comparisons);

            var accessErrors = (await fileEntry.GetRelatedCollectionAsync(p => p.AccessErrors, cancellationToken)).ToArray();
            if (accessErrors.Length > 0)
            {
                FileAccessErrors.RemoveRange(accessErrors);
                hasChanges = true;
            }
            if (hasChanges)
                await SaveChangesAsync(cancellationToken);
            var content = await fileEntry.GetRelatedReferenceAsync(f => f.BinaryProperties, cancellationToken);
            var summaryProperties = target.SummaryPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.SummaryProperties, cancellationToken) : null;
            var documentProperties = target.DocumentPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.DocumentProperties, cancellationToken) : null;
            var audioProperties = target.AudioPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.AudioProperties, cancellationToken) : null;
            var drmProperties = target.DRMPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.DRMProperties, cancellationToken) : null;
            var gpsProperties = target.GPSPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.GPSProperties, cancellationToken) : null;
            var imageProperties = target.ImagePropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.ImageProperties, cancellationToken) : null;
            var mediaProperties = target.MediaPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.MediaProperties, cancellationToken) : null;
            var musicProperties = target.MusicPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.MusicProperties, cancellationToken) : null;
            var photoProperties = target.PhotoPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.PhotoProperties, cancellationToken) : null;
            var recordedTVProperties = target.RecordedTVPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.RecordedTVProperties, cancellationToken) : null;
            var videoProperties = target.VideoPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.VideoProperties, cancellationToken) : null;
            Files.Remove(target);
            await SaveChangesAsync(cancellationToken);

            hasChanges = await RemoveIfNoReferencesAsync(content, cancellationToken);
            if (content.Files.Count == 0)
                BinaryPropertySets.Remove(content);
            if (summaryProperties is not null && await RemoveIfNoReferencesAsync(summaryProperties, cancellationToken))
                hasChanges = true;
            if (documentProperties is not null && await RemoveIfNoReferencesAsync(documentProperties, cancellationToken))
                hasChanges = true;
            if (audioProperties is not null && await RemoveIfNoReferencesAsync(audioProperties, cancellationToken))
                hasChanges = true;
            if (drmProperties is not null && await RemoveIfNoReferencesAsync(drmProperties, cancellationToken))
                hasChanges = true;
            if (gpsProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties, cancellationToken))
                hasChanges = true;
            if (imageProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties, cancellationToken))
                hasChanges = true;
            if (mediaProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties, cancellationToken))
                hasChanges = true;
            if (musicProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties, cancellationToken))
                hasChanges = true;
            if (photoProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties, cancellationToken))
                hasChanges = true;
            if (recordedTVProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties, cancellationToken))
                hasChanges = true;
            if (videoProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties, cancellationToken))
                hasChanges = true;
            if (hasChanges)
                await SaveChangesAsync(cancellationToken);
        }

        [Obsolete("Pass cancellation token")]
        public async Task<IEnumerable<TProperty>> GetRelatedCollectionAsync<TEntity, TProperty>([NotNull] TEntity entity,
            [NotNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression)
            where TEntity : class
            where TProperty : class => await Entry(entity).GetRelatedCollectionAsync(propertyExpression);

        public async Task<IEnumerable<TProperty>> GetRelatedCollectionAsync<TEntity, TProperty>([NotNull] TEntity entity,
            [NotNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class => await Entry(entity).GetRelatedCollectionAsync(propertyExpression, cancellationToken);

        public async Task<SummaryPropertySet> FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;

            throw new NotImplementedException();
        }

        public async Task<DocumentPropertySet> FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;
            throw new NotImplementedException();
        }

        public async Task<AudioPropertySet> FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;
            throw new NotImplementedException();
        }

        public async Task<DRMPropertySet> FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;
            throw new NotImplementedException();
        }

        public async Task<GPSPropertySet> FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;
            throw new NotImplementedException();
        }

        public async Task<ImagePropertySet> FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;
            throw new NotImplementedException();
        }

        public async Task<MediaPropertySet> FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;
            throw new NotImplementedException();
        }

        public async Task<MusicPropertySet> FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;
            throw new NotImplementedException();
        }

        public async Task<PhotoPropertySet> FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;
            throw new NotImplementedException();
        }

        public async Task<RecordedTVPropertySet> FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;
            throw new NotImplementedException();
        }

        public async Task<VideoPropertySet> FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrEmpty())
                return null;
            throw new NotImplementedException();
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(BinaryPropertySet binaryProperties)
        {
            EntityEntry<BinaryPropertySet> entry = Entry(binaryProperties);
            if ((await entry.GetRelatedCollectionAsync(p => p.Files)).Any() || !(await entry.GetRelatedCollectionAsync(p => p.RedundantSets)).Any())
                return false;
            BinaryPropertySets.Remove(binaryProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(BinaryPropertySet binaryProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EntityEntry<BinaryPropertySet> entry = Entry(binaryProperties);
            if ((await entry.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any() || !(await entry.GetRelatedCollectionAsync(p => p.RedundantSets, cancellationToken)).Any())
                return false;
            BinaryPropertySets.Remove(binaryProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(VideoPropertySet videoProperties)
        {
            if ((await GetRelatedCollectionAsync(videoProperties, p => p.Files)).Any())
                return false;
            VideoPropertySets.Remove(videoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(VideoPropertySet videoProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(videoProperties, p => p.Files, cancellationToken)).Any())
                return false;
            VideoPropertySets.Remove(videoProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(RecordedTVPropertySet recordedTVProperties)
        {
            if ((await GetRelatedCollectionAsync(recordedTVProperties, p => p.Files)).Any())
                return false;
            RecordedTVPropertySets.Remove(recordedTVProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(RecordedTVPropertySet recordedTVProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(recordedTVProperties, p => p.Files, cancellationToken)).Any())
                return false;
            RecordedTVPropertySets.Remove(recordedTVProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(PhotoPropertySet photoProperties)
        {
            if ((await GetRelatedCollectionAsync(photoProperties, p => p.Files)).Any())
                return false;
            PhotoPropertySets.Remove(photoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(PhotoPropertySet photoProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(photoProperties, p => p.Files, cancellationToken)).Any())
                return false;
            PhotoPropertySets.Remove(photoProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(MusicPropertySet musicProperties)
        {
            if ((await GetRelatedCollectionAsync(musicProperties, p => p.Files)).Any())
                return false;
            MusicPropertySets.Remove(musicProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MusicPropertySet musicProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(musicProperties, p => p.Files, cancellationToken)).Any())
                return false;
            MusicPropertySets.Remove(musicProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(MediaPropertySet mediaProperties)
        {
            if ((await GetRelatedCollectionAsync(mediaProperties, p => p.Files)).Any())
                return false;
            MediaPropertySets.Remove(mediaProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MediaPropertySet mediaProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(mediaProperties, p => p.Files, cancellationToken)).Any())
                return false;
            MediaPropertySets.Remove(mediaProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(ImagePropertySet imageProperties)
        {
            if ((await GetRelatedCollectionAsync(imageProperties, p => p.Files)).Any())
                return false;
            ImagePropertySets.Remove(imageProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(ImagePropertySet imageProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(imageProperties, p => p.Files, cancellationToken)).Any())
                return false;
            ImagePropertySets.Remove(imageProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(GPSPropertySet gpsProperties)
        {
            if ((await GetRelatedCollectionAsync(gpsProperties, p => p.Files)).Any())
                return false;
            GPSPropertySets.Remove(gpsProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(GPSPropertySet gpsProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(gpsProperties, p => p.Files, cancellationToken)).Any())
                return false;
            GPSPropertySets.Remove(gpsProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(DRMPropertySet drmProperties)
        {
            if ((await GetRelatedCollectionAsync(drmProperties, p => p.Files)).Any())
                return false;
            DRMPropertySets.Remove(drmProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DRMPropertySet drmProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(drmProperties, p => p.Files, cancellationToken)).Any())
                return false;
            DRMPropertySets.Remove(drmProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(AudioPropertySet audioProperties)
        {
            if ((await GetRelatedCollectionAsync(audioProperties, p => p.Files)).Any())
                return false;
            AudioPropertySets.Remove(audioProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(AudioPropertySet audioProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(audioProperties, p => p.Files, cancellationToken)).Any())
                return false;
            AudioPropertySets.Remove(audioProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(DocumentPropertySet documentProperties)
        {
            if ((await GetRelatedCollectionAsync(documentProperties, p => p.Files)).Any())
                return false;
            DocumentPropertySets.Remove(documentProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DocumentPropertySet documentProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(documentProperties, p => p.Files, cancellationToken)).Any())
                return false;
            DocumentPropertySets.Remove(documentProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(SummaryPropertySet summaryProperties)
        {
            if ((await GetRelatedCollectionAsync(summaryProperties, p => p.Files)).Any())
                return false;
            SummaryPropertySets.Remove(summaryProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(SummaryPropertySet summaryProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(summaryProperties, p => p.Files, cancellationToken)).Any())
                return false;
            SummaryPropertySets.Remove(summaryProperties);
            return true;
        }

        public async Task<bool> ForceDeleteBinaryPropertySetAsync(BinaryPropertySet target, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ForceDeleteRedundantSetAsync(RedundantSet target, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ForceDeleteFileSystemAsync(FileSystem target, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private void ForceDeleteRedundancy(Redundancy target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var redundantSet = target.RedundantSet;
            Redundancies.Remove(target);
            SaveChanges();
            if (redundantSet.Redundancies.Count == 0)
                RedundantSets.Remove(redundantSet);
        }

        public void ForceDeleteBinaryPropertySet(ILocalBinaryPropertySet target)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForceDeleteBinaryPropertySetAsync(ILocalBinaryPropertySet target, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void ForceDeleteRedundantSet(ILocalRedundantSet target)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForceDeleteRedundantSetAsync(ILocalRedundantSet targe, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void ForceDeleteFileSystem(ILocalFileSystem target)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForceDeleteFileSystemAsync(ILocalFileSystem target, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #region Explicit Members

        IEnumerable<ILocalComparison> ILocalDbContext.Comparisons => Comparisons.Cast<ILocalComparison>();

        IEnumerable<ILocalBinaryPropertySet> ILocalDbContext.BinaryPropertySets => BinaryPropertySets.Cast<ILocalBinaryPropertySet>();

        IEnumerable<IAccessError<ILocalFile>> ILocalDbContext.FileAccessErrors => FileAccessErrors.Cast<IAccessError<ILocalFile>>();

        IEnumerable<ILocalFile> ILocalDbContext.Files => Files.Cast<ILocalFile>();

        IEnumerable<ILocalFileSystem> ILocalDbContext.FileSystems => FileSystems.Cast<ILocalFileSystem>();

        IEnumerable<ILocalRedundancy> ILocalDbContext.Redundancies => Redundancies.Cast<ILocalRedundancy>();

        IEnumerable<ILocalRedundantSet> ILocalDbContext.RedundantSets => RedundantSets.Cast<ILocalRedundantSet>();

        IEnumerable<ILocalSubdirectory> ILocalDbContext.Subdirectories => Subdirectories.Cast<ILocalSubdirectory>();

        IEnumerable<IAccessError<ILocalSubdirectory>> ILocalDbContext.SubdirectoryAccessErrors => SubdirectoryAccessErrors.Cast<IAccessError<ILocalSubdirectory>>();

        IEnumerable<ILocalSymbolicName> ILocalDbContext.SymbolicNames => SymbolicNames.Cast<ILocalSymbolicName>();

        IEnumerable<IAccessError<ILocalVolume>> ILocalDbContext.VolumeAccessErrors => VolumeAccessErrors.Cast<IAccessError<ILocalVolume>>();

        IEnumerable<ILocalVolume> ILocalDbContext.Volumes => Volumes.Cast<ILocalVolume>();

        IEnumerable<ILocalCrawlConfiguration> ILocalDbContext.CrawlConfigurations => CrawlConfigurations.Cast<ILocalCrawlConfiguration>();

        IEnumerable<IComparison> IDbContext.Comparisons => Comparisons.Cast<IComparison>();

        IEnumerable<IBinaryPropertySet> IDbContext.BinaryPropertySets => BinaryPropertySets.Cast<IBinaryPropertySet>();

        IEnumerable<IAccessError<IFile>> IDbContext.FileAccessErrors => FileAccessErrors.Cast<IAccessError<IFile>>();

        IEnumerable<IFile> IDbContext.Files => Files.Cast<IFile>();

        IEnumerable<IFileSystem> IDbContext.FileSystems => FileSystems.Cast<IFileSystem>();

        IEnumerable<IRedundancy> IDbContext.Redundancies => Redundancies.Cast<IRedundancy>();

        IEnumerable<IRedundantSet> IDbContext.RedundantSets => RedundantSets.Cast<IRedundantSet>();

        IEnumerable<ISubdirectory> IDbContext.Subdirectories => Subdirectories.Cast<ISubdirectory>();

        IEnumerable<IAccessError<ISubdirectory>> IDbContext.SubdirectoryAccessErrors => SubdirectoryAccessErrors.Cast<IAccessError<ISubdirectory>>();

        IEnumerable<ISymbolicName> IDbContext.SymbolicNames => SymbolicNames.Cast<ISymbolicName>();

        IEnumerable<IAccessError<IVolume>> IDbContext.VolumeAccessErrors => VolumeAccessErrors.Cast<IAccessError<IVolume>>();

        IEnumerable<IVolume> IDbContext.Volumes => Volumes.Cast<IVolume>();

        IEnumerable<ICrawlConfiguration> IDbContext.CrawlConfigurations => CrawlConfigurations.Cast<ICrawlConfiguration>();

        IEnumerable<ILocalSummaryPropertySet> ILocalDbContext.SummaryPropertySets => SummaryPropertySets.Cast<ILocalSummaryPropertySet>();

        IEnumerable<ILocalDocumentPropertySet> ILocalDbContext.DocumentPropertySets => DocumentPropertySets.Cast<ILocalDocumentPropertySet>();

        IEnumerable<ILocalAudioPropertySet> ILocalDbContext.AudioPropertySets => AudioPropertySets.Cast<ILocalAudioPropertySet>();

        IEnumerable<ILocalDRMPropertySet> ILocalDbContext.DRMPropertySets => DRMPropertySets.Cast<ILocalDRMPropertySet>();

        IEnumerable<ILocalGPSPropertySet> ILocalDbContext.GPSPropertySets => GPSPropertySets.Cast<ILocalGPSPropertySet>();

        IEnumerable<ILocalImagePropertySet> ILocalDbContext.ImagePropertySets => ImagePropertySets.Cast<ILocalImagePropertySet>();

        IEnumerable<ILocalMediaPropertySet> ILocalDbContext.MediaPropertySets => MediaPropertySets.Cast<ILocalMediaPropertySet>();

        IEnumerable<ILocalMusicPropertySet> ILocalDbContext.MusicPropertySets => MusicPropertySets.Cast<ILocalMusicPropertySet>();

        IEnumerable<ILocalPhotoPropertySet> ILocalDbContext.PhotoPropertySets => PhotoPropertySets.Cast<ILocalPhotoPropertySet>();

        IEnumerable<ILocalRecordedTVPropertySet> ILocalDbContext.RecordedTVPropertySets => RecordedTVPropertySets.Cast<ILocalRecordedTVPropertySet>();

        IEnumerable<ILocalVideoPropertySet> ILocalDbContext.VideoPropertySets => VideoPropertySets.Cast<ILocalVideoPropertySet>();

        IEnumerable<ISummaryPropertySet> IDbContext.SummaryPropertySets => SummaryPropertySets.Cast<ISummaryPropertySet>();

        IEnumerable<IDocumentPropertySet> IDbContext.DocumentPropertySets => DocumentPropertySets.Cast<IDocumentPropertySet>();

        IEnumerable<IAudioPropertySet> IDbContext.AudioPropertySets => AudioPropertySets.Cast<IAudioPropertySet>();

        IEnumerable<IDRMPropertySet> IDbContext.DRMPropertySets => DRMPropertySets.Cast<IDRMPropertySet>();

        IEnumerable<IGPSPropertySet> IDbContext.GPSPropertySets => GPSPropertySets.Cast<IGPSPropertySet>();

        IEnumerable<IImagePropertySet> IDbContext.ImagePropertySets => ImagePropertySets.Cast<IImagePropertySet>();

        IEnumerable<IMediaPropertySet> IDbContext.MediaPropertySets => MediaPropertySets.Cast<IMediaPropertySet>();

        IEnumerable<IMusicPropertySet> IDbContext.MusicPropertySets => MusicPropertySets.Cast<IMusicPropertySet>();

        IEnumerable<IPhotoPropertySet> IDbContext.PhotoPropertySets => PhotoPropertySets.Cast<IPhotoPropertySet>();

        IEnumerable<IRecordedTVPropertySet> IDbContext.RecordedTVPropertySets => RecordedTVPropertySets.Cast<IRecordedTVPropertySet>();

        IEnumerable<IVideoPropertySet> IDbContext.VideoPropertySets => VideoPropertySets.Cast<IVideoPropertySet>();

        Task<ILocalSummaryPropertySet> ILocalDbContext.FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ILocalDocumentPropertySet> ILocalDbContext.FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ILocalAudioPropertySet> ILocalDbContext.FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ILocalDRMPropertySet> ILocalDbContext.FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ILocalGPSPropertySet> ILocalDbContext.FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ILocalImagePropertySet> ILocalDbContext.FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ILocalMediaPropertySet> ILocalDbContext.FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ILocalMusicPropertySet> ILocalDbContext.FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ILocalPhotoPropertySet> ILocalDbContext.FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ILocalRecordedTVPropertySet> ILocalDbContext.FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ILocalVideoPropertySet> ILocalDbContext.FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ISummaryPropertySet> IDbContext.FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IDocumentPropertySet> IDbContext.FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IAudioPropertySet> IDbContext.FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IDRMPropertySet> IDbContext.FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IGPSPropertySet> IDbContext.FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IImagePropertySet> IDbContext.FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IMediaPropertySet> IDbContext.FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IMusicPropertySet> IDbContext.FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IPhotoPropertySet> IDbContext.FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IRecordedTVPropertySet> IDbContext.FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IVideoPropertySet> IDbContext.FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IDbContext.ForceDeleteBinaryPropertySet(IBinaryPropertySet target)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDbContext.ForceDeleteBinaryPropertySetAsync(IBinaryPropertySet target, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IDbContext.ForceDeleteRedundantSet(IRedundantSet target)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDbContext.ForceDeleteRedundantSetAsync(IRedundantSet target, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
