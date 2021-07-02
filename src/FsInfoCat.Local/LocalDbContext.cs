using FsInfoCat.Collections;
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
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string applicationName = properties.ApplicationName.TrimmedOrNullIfWhiteSpace();
            MultiStringValue author = MultiStringValue.NullIfNotAny(properties.Author);
            // TODO: Implement FindMatchingAsync(ISummaryProperties, CancellationToken);
            return await SummaryPropertySets.FirstOrDefaultAsync(p => p.ApplicationName == applicationName);
        }

        public async Task<DocumentPropertySet> FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string clientID = properties.ClientID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IDocumentProperties, CancellationToken);
            return await DocumentPropertySets.FirstOrDefaultAsync(p => p.ClientID == clientID);
        }

        public async Task<AudioPropertySet> FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string format = properties.Format.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IAudioProperties, CancellationToken);
            return await AudioPropertySets.FirstOrDefaultAsync(p => p.Format == format);
        }

        public async Task<DRMPropertySet> FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string description = properties.Description.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IDRMProperties, CancellationToken);
            return await DRMPropertySets.FirstOrDefaultAsync(p => p.Description == description);
        }

        public async Task<GPSPropertySet> FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string areaInformation = properties.AreaInformation.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IGPSProperties, CancellationToken);
            return await GPSPropertySets.FirstOrDefaultAsync(p => p.AreaInformation == areaInformation);
        }

        public async Task<ImagePropertySet> FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string imageID = properties.ImageID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IImageProperties, CancellationToken);
            return await ImagePropertySets.FirstOrDefaultAsync(p => p.ImageID == imageID);
        }

        public async Task<MediaPropertySet> FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string dvdID = properties.DVDID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IMediaProperties, CancellationToken);
            return await MediaPropertySets.FirstOrDefaultAsync(p => p.DVDID == dvdID);
        }

        public async Task<MusicPropertySet> FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string displayArtist = properties.DisplayArtist.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IMusicProperties, CancellationToken);
            return await MusicPropertySets.FirstOrDefaultAsync(p => p.DisplayArtist == displayArtist);
        }

        public async Task<PhotoPropertySet> FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string exifVersion = properties.EXIFVersion.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IPhotoProperties, CancellationToken);
            return await PhotoPropertySets.FirstOrDefaultAsync(p => p.EXIFVersion == exifVersion);
        }

        public async Task<RecordedTVPropertySet> FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string episodeName = properties.EpisodeName.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IRecordedTVProperties, CancellationToken);
            return await RecordedTVPropertySets.FirstOrDefaultAsync(p => p.EpisodeName == episodeName);
        }

        public async Task<VideoPropertySet> FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            string compression = properties.Compression.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IVideoProperties, CancellationToken);
            return await VideoPropertySets.FirstOrDefaultAsync(p => p.Compression == compression);
        }

        public async Task<SummaryPropertySet> GetMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            SummaryPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(ISummaryProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<DocumentPropertySet> GetMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            DocumentPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IDocumentProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<AudioPropertySet> GetMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            AudioPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IAudioProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<DRMPropertySet> GetMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            DRMPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IDRMProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<GPSPropertySet> GetMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            GPSPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IGPSProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<ImagePropertySet> GetMatchingAsync(IImageProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            ImagePropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IImageProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<MediaPropertySet> GetMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            MediaPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IMediaProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<MusicPropertySet> GetMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            MusicPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IMusicProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<PhotoPropertySet> GetMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            PhotoPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IPhotoProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<RecordedTVPropertySet> GetMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            RecordedTVPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IRecordedTVProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<VideoPropertySet> GetMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.ArePropertiesNullOrEmpty())
                return null;
            VideoPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IVideoProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
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
