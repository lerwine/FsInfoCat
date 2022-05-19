using M = FsInfoCat.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local.Model
{
    // TODO: Document LocalDbContext class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial class LocalDbContext : M.BaseDbContext, ILocalDbContext
    {
        private static readonly object _syncRoot = new();
        private static bool _connectionStringValidated;
        private readonly ILogger<LocalDbContext> _logger;

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<VolumeAccessError> VolumeAccessErrors { get; set; }

        public virtual DbSet<Subdirectory> Subdirectories { get; set; }

        public virtual DbSet<SubdirectoryAccessError> SubdirectoryAccessErrors { get; set; }

        public virtual DbSet<DbFile> Files { get; set; }

        public virtual DbSet<FileAccessError> FileAccessErrors { get; set; }

        public virtual DbSet<SummaryPropertySet> SummaryPropertySets { get; set; }

        public virtual DbSet<DocumentPropertySet> DocumentPropertySets { get; set; }

        public virtual DbSet<AudioPropertySet> AudioPropertySets { get; set; }

        public virtual DbSet<DRMPropertySet> DRMPropertySets { get; set; }

        public virtual DbSet<GPSPropertySet> GPSPropertySets { get; set; }

        public virtual DbSet<ImagePropertySet> ImagePropertySets { get; set; }

        public virtual DbSet<MediaPropertySet> MediaPropertySets { get; set; }

        public virtual DbSet<MusicPropertySet> MusicPropertySets { get; set; }

        public virtual DbSet<PhotoPropertySet> PhotoPropertySets { get; set; }

        public virtual DbSet<RecordedTVPropertySet> RecordedTVPropertySets { get; set; }

        public virtual DbSet<VideoPropertySet> VideoPropertySets { get; set; }

        public virtual DbSet<BinaryPropertySet> BinaryPropertySets { get; set; }

        public virtual DbSet<PersonalTagDefinition> PersonalTagDefinitions { get; set; }

        public virtual DbSet<PersonalFileTag> PersonalFileTags { get; set; }

        public virtual DbSet<PersonalSubdirectoryTag> PersonalSubdirectoryTags { get; set; }

        public virtual DbSet<PersonalVolumeTag> PersonalVolumeTags { get; set; }

        public virtual DbSet<SharedTagDefinition> SharedTagDefinitions { get; set; }

        public virtual DbSet<SharedFileTag> SharedFileTags { get; set; }

        public virtual DbSet<SharedSubdirectoryTag> SharedSubdirectoryTags { get; set; }

        public virtual DbSet<SharedVolumeTag> SharedVolumeTags { get; set; }

        public virtual DbSet<FileComparison> Comparisons { get; set; }

        public virtual DbSet<RedundantSet> RedundantSets { get; set; }

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        public virtual DbSet<CrawlConfiguration> CrawlConfigurations { get; set; }

        public virtual DbSet<CrawlJobLog> CrawlJobLogs { get; set; }

        public virtual DbSet<SymbolicNameListItem> SymbolicNameListing { get; set; }

        public virtual DbSet<FileSystemListItem> FileSystemListing { get; set; }

        public virtual DbSet<PersonalTagDefinitionListItem> PersonalTagDefinitionListing { get; set; }

        public virtual DbSet<SharedTagDefinitionListItem> SharedTagDefinitionListing { get; set; }

        public virtual DbSet<RedundantSetListItem> RedundantSetListing { get; set; }

        public virtual DbSet<VolumeListItem> VolumeListing { get; set; }

        public virtual DbSet<VolumeListItemWithFileSystem> VolumeListingWithFileSystem { get; set; }

        public virtual DbSet<SubdirectoryListItem> SubdirectoryListing { get; set; }

        public virtual DbSet<SubdirectoryListItemWithAncestorNames> SubdirectoryListingWithAncestorNames { get; set; }

        public virtual DbSet<SubdirectoryAncestorNames> SubdirectoryAncestorNames { get; set; }

        public virtual DbSet<FileWithAncestorNames> FileListingWithAncestorNames { get; set; }

        public virtual DbSet<FileWithBinaryProperties> FileListingWithBinaryProperties { get; set; }

        public virtual DbSet<FileWithBinaryPropertiesAndAncestorNames> FileListingWithBinaryPropertiesAndAncestorNames { get; set; }

        public virtual DbSet<CrawlConfigListItem> CrawlConfigListing { get; set; }

        public virtual DbSet<CrawlConfigReportItem> CrawlConfigReport { get; set; }

        public virtual DbSet<CrawlJobLogListItem> CrawlJobListing { get; set; }

        public virtual DbSet<SummaryPropertiesListItem> SummaryPropertiesListing { get; set; }

        public virtual DbSet<DocumentPropertiesListItem> DocumentPropertiesListing { get; set; }

        public virtual DbSet<AudioPropertiesListItem> AudioPropertiesListing { get; set; }

        public virtual DbSet<DRMPropertiesListItem> DRMPropertiesListing { get; set; }

        public virtual DbSet<GPSPropertiesListItem> GPSPropertiesListing { get; set; }

        public virtual DbSet<ImagePropertiesListItem> ImagePropertiesListing { get; set; }

        public virtual DbSet<MediaPropertiesListItem> MediaPropertiesListing { get; set; }

        public virtual DbSet<MusicPropertiesListItem> MusicPropertiesListing { get; set; }

        public virtual DbSet<PhotoPropertiesListItem> PhotoPropertiesListing { get; set; }

        public virtual DbSet<RecordedTVPropertiesListItem> RecordedTVPropertiesListing { get; set; }

        public virtual DbSet<VideoPropertiesListItem> VideoPropertiesListing { get; set; }

        public virtual DbSet<PersonalVolumeTagListItem> PersonalVolumeTagListing { get; set; }

        public virtual DbSet<SharedVolumeTagListItem> SharedVolumeTagListing { get; set; }

        public virtual DbSet<PersonalSubdirectoryTagListItem> PersonalSubdirectoryTagListing { get; set; }

        public virtual DbSet<SharedSubdirectoryTagListItem> SharedSubdirectoryTagListing { get; set; }

        public virtual DbSet<PersonalFileTagListItem> PersonalFileTagListing { get; set; }

        public virtual DbSet<SharedFileTagListItem> SharedFileTagListing { get; set; }

        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        {
            _logger = Hosting.ServiceProvider.GetRequiredService<ILogger<LocalDbContext>>();
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
                            _logger.LogTrace($"{{Message}}; {nameof(SqliteCommand)}={{{nameof(SqliteCommand.CommandText)}}}",
                                element.Attributes("Message").Select(a => a.Value).DefaultIfEmpty("").First(), element.Value);
                            using SqliteCommand command = connection.CreateCommand();
                            command.CommandText = element.Value;
                            command.CommandType = System.Data.CommandType.Text;
                            try { _ = command.ExecuteNonQuery(); }
                            catch (Exception exception)
                            {
#pragma warning disable CA2201 // Exception type System.Exception is not sufficiently specific
                                throw new Exception($"Error executing query '{element.Value}': {exception.Message}");
#pragma warning restore CA2201 // Exception type System.Exception is not sufficiently specific
                            }
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
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder//.HasDefaultSchema("dbo")
                .Entity<SymbolicName>(SymbolicName.OnBuildEntity)
                .Entity<Volume>(Volume.OnBuildEntity)
                .Entity<Subdirectory>(Subdirectory.OnBuildEntity)
                .Entity<CrawlConfiguration>(CrawlConfiguration.OnBuildEntity)
                .Entity<CrawlJobLog>(CrawlJobLog.OnBuildEntity)
                .Entity<DbFile>(DbFile.OnBuildEntity)
                .Entity<BinaryPropertySet>(BinaryPropertySet.OnBuildEntity)
                .Entity<FileComparison>(FileComparison.OnBuildEntity)
                .Entity<RedundantSet>(RedundantSet.OnBuildEntity)
                .Entity<Redundancy>(Redundancy.OnBuildEntity)
                .Entity<SummaryPropertySet>(SummaryPropertySet.OnBuildEntity)
                .Entity<DocumentPropertySet>(DocumentPropertySet.OnBuildEntity)
                .Entity<GPSPropertySet>(GPSPropertySet.OnBuildEntity)
                .Entity<MediaPropertySet>(MediaPropertySet.OnBuildEntity)
                .Entity<MusicPropertySet>(MusicPropertySet.OnBuildEntity)
                .Entity<PhotoPropertySet>(PhotoPropertySet.OnBuildEntity)
                .Entity<VideoPropertySet>(VideoPropertySet.OnBuildEntity)
                .Entity<PersonalFileTag>(PersonalFileTag.OnBuildEntity)
                .Entity<PersonalSubdirectoryTag>(PersonalSubdirectoryTag.OnBuildEntity)
                .Entity<PersonalVolumeTag>(PersonalVolumeTag.OnBuildEntity)
                .Entity<SharedFileTag>(SharedFileTag.OnBuildEntity)
                .Entity<SharedSubdirectoryTag>(SharedSubdirectoryTag.OnBuildEntity)
                .Entity<SharedVolumeTag>(SharedVolumeTag.OnBuildEntity)
                .Entity<FileAccessError>(FileAccessError.OnBuildEntity)
                .Entity<SubdirectoryAccessError>(SubdirectoryAccessError.OnBuildEntity)
                .Entity<VolumeAccessError>(VolumeAccessError.OnBuildEntity)
                .Entity<FileSystemListItem>(FileSystemListItem.OnBuildEntity)
                .Entity<SymbolicNameListItem>(SymbolicNameListItem.OnBuildEntity)
                .Entity<PersonalTagDefinitionListItem>(PersonalTagDefinitionListItem.OnBuildEntity)
                .Entity<SharedTagDefinitionListItem>(SharedTagDefinitionListItem.OnBuildEntity)
                .Entity<VolumeListItem>(VolumeListItem.OnBuildEntity)
                .Entity<VolumeListItemWithFileSystem>(VolumeListItemWithFileSystem.OnBuildEntity)
                .Entity<SubdirectoryListItem>(SubdirectoryListItem.OnBuildEntity)
                .Entity<SubdirectoryListItemWithAncestorNames>(SubdirectoryListItemWithAncestorNames.OnBuildEntity)
                .Entity<SubdirectoryAncestorNames>(Local.Model.SubdirectoryAncestorNames.OnBuildEntity)
                .Entity<CrawlConfigListItem>(CrawlConfigListItem.OnBuildEntity)
                .Entity<CrawlConfigReportItem>(CrawlConfigReportItem.OnBuildEntity)
                .Entity<CrawlJobLogListItem>(CrawlJobLogListItem.OnBuildEntity)
                .Entity<FileWithAncestorNames>(FileWithAncestorNames.OnBuildEntity)
                .Entity<FileWithBinaryProperties>(FileWithBinaryProperties.OnBuildEntity)
                .Entity<FileWithBinaryPropertiesAndAncestorNames>(FileWithBinaryPropertiesAndAncestorNames.OnBuildEntity)
                .Entity<RedundantSetListItem>(RedundantSetListItem.OnBuildEntity)
                .Entity<SummaryPropertiesListItem>(SummaryPropertiesListItem.OnBuildEntity)
                .Entity<DocumentPropertiesListItem>(DocumentPropertiesListItem.OnBuildEntity)
                .Entity<AudioPropertiesListItem>(AudioPropertiesListItem.OnBuildEntity)
                .Entity<DRMPropertiesListItem>(DRMPropertiesListItem.OnBuildEntity)
                .Entity<GPSPropertiesListItem>(GPSPropertiesListItem.OnBuildEntity)
                .Entity<ImagePropertiesListItem>(ImagePropertiesListItem.OnBuildEntity)
                .Entity<MediaPropertiesListItem>(MediaPropertiesListItem.OnBuildEntity)
                .Entity<MusicPropertiesListItem>(MusicPropertiesListItem.OnBuildEntity)
                .Entity<PhotoPropertiesListItem>(PhotoPropertiesListItem.OnBuildEntity)
                .Entity<RecordedTVPropertiesListItem>(RecordedTVPropertiesListItem.OnBuildEntity)
                .Entity<VideoPropertiesListItem>(VideoPropertiesListItem.OnBuildEntity)
                .Entity<PersonalVolumeTagListItem>(PersonalVolumeTagListItem.OnBuildEntity)
                .Entity<SharedVolumeTagListItem>(SharedVolumeTagListItem.OnBuildEntity)
                .Entity<PersonalSubdirectoryTagListItem>(PersonalSubdirectoryTagListItem.OnBuildEntity)
                .Entity<SharedSubdirectoryTagListItem>(SharedSubdirectoryTagListItem.OnBuildEntity)
                .Entity<PersonalFileTagListItem>(PersonalFileTagListItem.OnBuildEntity)
                .Entity<SharedFileTagListItem>(SharedFileTagListItem.OnBuildEntity);
        }

        public static void AddDbContextPool(IServiceCollection services, Assembly assembly, string dbFileName) => AddDbContextPool(services, GetDbFilePath(assembly, dbFileName));

        public static void AddDbContextPool(IServiceCollection services, string dbPath)
        {
            string connectionString = GetConnectionString(dbPath);
            _ = services.AddDbContextPool<LocalDbContext>(options => options.AddInterceptors(new Interceptor()).UseSqlite(connectionString));
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
                dbFileName = Hosting.DEFAULT_LOCAL_DB_FILENAME;
            return Path.IsPathFullyQualified(dbFileName) ? Path.GetFullPath(dbFileName) : Path.Combine(Hosting.GetAppDataPath(assembly), dbFileName);
        }

        public async Task<IEnumerable<TProperty>> GetRelatedCollectionAsync<TEntity, TProperty>([DisallowNull] TEntity entity,
            [DisallowNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class => await Entry(entity).GetRelatedCollectionAsync(propertyExpression, cancellationToken);

        public async Task<SummaryPropertySet> FindMatchingAsync(M.ISummaryProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string applicationName = properties.ApplicationName.TrimmedOrNullIfWhiteSpace();
            //MultiStringValue author = MultiStringValue.NullIfNotAny(properties.Author);
            // TODO: Implement FindMatchingAsync(M.ISummaryProperties, CancellationToken);
            return await SummaryPropertySets.FirstOrDefaultAsync(p => p.ApplicationName == applicationName, cancellationToken);
        }

        public async Task<DocumentPropertySet> FindMatchingAsync(M.IDocumentProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string clientID = properties.ClientID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(M.IDocumentProperties, CancellationToken);
            return await DocumentPropertySets.FirstOrDefaultAsync(p => p.ClientID == clientID, cancellationToken);
        }

        public async Task<AudioPropertySet> FindMatchingAsync(M.IAudioProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string format = properties.Format.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(M.IAudioProperties, CancellationToken);
            return await AudioPropertySets.FirstOrDefaultAsync(p => p.Format == format, cancellationToken);
        }

        public async Task<DRMPropertySet> FindMatchingAsync(M.IDRMProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string description = properties.Description.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(M.IDRMProperties, CancellationToken);
            return await DRMPropertySets.FirstOrDefaultAsync(p => p.Description == description, cancellationToken);
        }

        public async Task<GPSPropertySet> FindMatchingAsync(M.IGPSProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string areaInformation = properties.AreaInformation.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(M.IGPSProperties, CancellationToken);
            return await GPSPropertySets.FirstOrDefaultAsync(p => p.AreaInformation == areaInformation, cancellationToken);
        }

        public async Task<ImagePropertySet> FindMatchingAsync(M.IImageProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string imageID = properties.ImageID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(M.IImageProperties, CancellationToken);
            return await ImagePropertySets.FirstOrDefaultAsync(p => p.ImageID == imageID, cancellationToken);
        }

        public async Task<MediaPropertySet> FindMatchingAsync(M.IMediaProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string dvdID = properties.DVDID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(M.IMediaProperties, CancellationToken);
            return await MediaPropertySets.FirstOrDefaultAsync(p => p.DVDID == dvdID, cancellationToken);
        }

        public async Task<MusicPropertySet> FindMatchingAsync(M.IMusicProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string displayArtist = properties.DisplayArtist.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(M.IMusicProperties, CancellationToken);
            return await MusicPropertySets.FirstOrDefaultAsync(p => p.DisplayArtist == displayArtist, cancellationToken);
        }

        public async Task<PhotoPropertySet> FindMatchingAsync(M.IPhotoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string exifVersion = properties.EXIFVersion.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(M.IPhotoProperties, CancellationToken);
            return await PhotoPropertySets.FirstOrDefaultAsync(p => p.EXIFVersion == exifVersion, cancellationToken);
        }

        public async Task<RecordedTVPropertySet> FindMatchingAsync(M.IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string episodeName = properties.EpisodeName.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(M.IRecordedTVProperties, CancellationToken);
            return await RecordedTVPropertySets.FirstOrDefaultAsync(p => p.EpisodeName == episodeName, cancellationToken);
        }

        public async Task<VideoPropertySet> FindMatchingAsync(M.IVideoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string compression = properties.Compression.NullIfWhiteSpaceOrNormalized();
            // TODO: Implement FindMatchingAsync(M.IVideoProperties, CancellationToken);
            return await VideoPropertySets.FirstOrDefaultAsync(p => p.Compression == compression, cancellationToken);
        }

        public async Task<SummaryPropertySet> GetMatchingAsync(M.ISummaryProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            SummaryPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.ISummaryProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<DocumentPropertySet> GetMatchingAsync(M.IDocumentProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            DocumentPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.IDocumentProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<AudioPropertySet> GetMatchingAsync(M.IAudioProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            AudioPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.IAudioProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<DRMPropertySet> GetMatchingAsync(M.IDRMProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            DRMPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.IDRMProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<GPSPropertySet> GetMatchingAsync(M.IGPSProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            GPSPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.IGPSProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<ImagePropertySet> GetMatchingAsync(M.IImageProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            ImagePropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.IImageProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<MediaPropertySet> GetMatchingAsync(M.IMediaProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            MediaPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.IMediaProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<MusicPropertySet> GetMatchingAsync(M.IMusicProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            MusicPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.IMusicProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<PhotoPropertySet> GetMatchingAsync(M.IPhotoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            PhotoPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.IPhotoProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<RecordedTVPropertySet> GetMatchingAsync(M.IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            RecordedTVPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.IRecordedTVProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<VideoPropertySet> GetMatchingAsync(M.IVideoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            VideoPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(M.IVideoProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(BinaryPropertySet binaryProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EntityEntry<BinaryPropertySet> entry = Entry(binaryProperties);
            if ((await entry.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any() || !(await entry.GetRelatedCollectionAsync(p => p.RedundantSets, cancellationToken)).Any())
                return false;
            _ = BinaryPropertySets.Remove(binaryProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(VideoPropertySet videoProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(videoProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = VideoPropertySets.Remove(videoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(RecordedTVPropertySet recordedTVProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(recordedTVProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = RecordedTVPropertySets.Remove(recordedTVProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(PhotoPropertySet photoProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(photoProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = PhotoPropertySets.Remove(photoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MusicPropertySet musicProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(musicProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = MusicPropertySets.Remove(musicProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MediaPropertySet mediaProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(mediaProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = MediaPropertySets.Remove(mediaProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(ImagePropertySet imageProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(imageProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = ImagePropertySets.Remove(imageProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(GPSPropertySet gpsProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(gpsProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = GPSPropertySets.Remove(gpsProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DRMPropertySet drmProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(drmProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = DRMPropertySets.Remove(drmProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(AudioPropertySet audioProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(audioProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = AudioPropertySets.Remove(audioProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DocumentPropertySet documentProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(documentProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = DocumentPropertySets.Remove(documentProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(SummaryPropertySet summaryProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(summaryProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = SummaryPropertySets.Remove(summaryProperties);
            return true;
        }

        private void ForceDeleteRedundancy(Redundancy target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var redundantSet = target.RedundantSet;
            _ = Redundancies.Remove(target);
            _ = SaveChanges();
            if (redundantSet.Redundancies.Count == 0)
                _ = RedundantSets.Remove(redundantSet);
        }

        #region Overrides

        protected override IEnumerable<M.IComparison> GetGenericComparisons() => Comparisons;

        protected override IEnumerable<M.IBinaryPropertySet> GetGenericBinaryPropertySets() => BinaryPropertySets;

        protected override IEnumerable<M.ISummaryPropertySet> GetGenericSummaryPropertySets() => SummaryPropertySets;

        protected override IEnumerable<M.IDocumentPropertySet> GetGenericDocumentPropertySets() => DocumentPropertySets;

        protected override IEnumerable<M.IAudioPropertySet> GetGenericAudioPropertySets() => AudioPropertySets;

        protected override IEnumerable<M.IDRMPropertySet> GetGenericDRMPropertySets() => DRMPropertySets;

        protected override IEnumerable<M.IGPSPropertySet> GetGenericGPSPropertySets() => GPSPropertySets;

        protected override IEnumerable<M.IImagePropertySet> GetGenericImagePropertySets() => ImagePropertySets;

        protected override IEnumerable<M.IMediaPropertySet> GetGenericMediaPropertySets() => MediaPropertySets;

        protected override IEnumerable<M.IMusicPropertySet> GetGenericMusicPropertySets() => MusicPropertySets;

        protected override IEnumerable<M.IPhotoPropertySet> GetGenericPhotoPropertySets() => PhotoPropertySets;

        protected override IEnumerable<M.IRecordedTVPropertySet> GetGenericRecordedTVPropertySets() => RecordedTVPropertySets;

        protected override IEnumerable<M.IVideoPropertySet> GetGenericVideoPropertySets() => VideoPropertySets;

        protected override IEnumerable<M.IFileAccessError> GetGenericFileAccessErrors() => FileAccessErrors;

        protected override IEnumerable<M.IFile> GetGenericFiles() => Files;

        protected override IEnumerable<M.IFileSystem> GetGenericFileSystems() => FileSystems;

        protected override IEnumerable<M.IRedundancy> GetGenericRedundancies() => Redundancies;

        protected override IEnumerable<M.IRedundantSet> GetGenericRedundantSets() => RedundantSets;

        protected override IEnumerable<M.ISubdirectory> GetGenericSubdirectories() => Subdirectories;

        protected override IEnumerable<M.ISubdirectoryAccessError> GetGenericSubdirectoryAccessErrors() => SubdirectoryAccessErrors;

        protected override IEnumerable<M.ISymbolicName> GetGenericSymbolicNames() => SymbolicNames;

        protected override IEnumerable<M.IVolumeAccessError> GetGenericVolumeAccessErrors() => VolumeAccessErrors;

        protected override IEnumerable<M.IVolume> GetGenericVolumes() => Volumes;

        protected override IEnumerable<M.ICrawlConfiguration> GetGenericCrawlConfigurations() => CrawlConfigurations;

        protected override IEnumerable<M.IFileAccessError> GetFileAccessErrors() => FileAccessErrors;

        protected override IEnumerable<M.ISubdirectoryAccessError> GetSubdirectoryAccessErrors() => SubdirectoryAccessErrors;

        protected override IEnumerable<M.IVolumeAccessError> GetVolumeAccessErrors() => VolumeAccessErrors;

        protected override IEnumerable<M.IPersonalTagDefinition> GetPersonalTagDefinitions() => PersonalTagDefinitions;

        protected override IEnumerable<M.IPersonalFileTag> GetPersonalFileTags() => PersonalFileTags;

        protected override IEnumerable<M.IPersonalSubdirectoryTag> GetPersonalSubdirectoryTags() => PersonalSubdirectoryTags;

        protected override IEnumerable<M.IPersonalVolumeTag> GetPersonalVolumeTags() => PersonalVolumeTags;

        protected override IEnumerable<M.ISharedTagDefinition> GetSharedTagDefinitions() => SharedTagDefinitions;

        protected override IEnumerable<M.ISharedFileTag> GetSharedFileTags() => SharedFileTags;

        protected override IEnumerable<M.ISharedSubdirectoryTag> GetSharedSubdirectoryTags() => SharedSubdirectoryTags;

        protected override IEnumerable<M.ISharedVolumeTag> GetSharedVolumeTags() => SharedVolumeTags;

        protected override IEnumerable<M.ICrawlJobLog> GetCrawlJobLogs() => CrawlJobLogs;

        protected override IEnumerable<M.ISymbolicNameListItem> GetSymbolicNameListing() => SymbolicNameListing;

        protected override IEnumerable<M.IFileSystemListItem> GetFileSystemListing() => FileSystemListing;

        protected override IEnumerable<M.ITagDefinitionListItem> GetPersonalTagDefinitionListing() => PersonalTagDefinitionListing;

        protected override IEnumerable<M.ITagDefinitionListItem> GetSharedTagDefinitionListing() => SharedTagDefinitionListing;

        protected override IEnumerable<M.IRedundantSetListItem> GetRedundantSetListing() => RedundantSetListing;

        protected override IEnumerable<M.IVolumeListItem> GetVolumeListing() => VolumeListing;

        protected override IEnumerable<M.IVolumeListItemWithFileSystem> GetVolumeListingWithFileSystem() => VolumeListingWithFileSystem;

        protected override IEnumerable<M.ISubdirectoryListItem> GetSubdirectoryListing() => SubdirectoryListing;

        protected override IEnumerable<M.ISubdirectoryListItemWithAncestorNames> GetSubdirectoryListingWithAncestorNames() => SubdirectoryListingWithAncestorNames;

        protected override IEnumerable<M.ISubdirectoryAncestorName> GetSubdirectoryAncestorNames() => SubdirectoryAncestorNames;

        protected override IEnumerable<M.IFileListItemWithAncestorNames> GetFileListingWithAncestorNames() => FileListingWithAncestorNames;

        protected override IEnumerable<M.IFileListItemWithBinaryProperties> GetFileListingWithBinaryProperties() => FileListingWithBinaryProperties;

        protected override IEnumerable<M.IFileListItemWithBinaryPropertiesAndAncestorNames> GetFileListingWithBinaryPropertiesAndAncestorNames() => FileListingWithBinaryPropertiesAndAncestorNames;

        protected override IEnumerable<M.ICrawlConfigurationListItem> GetCrawlConfigListing() => CrawlConfigListing;

        protected override IEnumerable<M.ICrawlConfigReportItem> GetCrawlConfigReport() => CrawlConfigReport;

        protected override IEnumerable<M.ICrawlJobListItem> GetCrawlJobListing() => CrawlJobListing;

        protected override IEnumerable<M.ISummaryPropertiesListItem> GetSummaryPropertiesListing() => SummaryPropertiesListing;

        protected override IEnumerable<M.IDocumentPropertiesListItem> GetDocumentPropertiesListing() => DocumentPropertiesListing;

        protected override IEnumerable<M.IAudioPropertiesListItem> GetAudioPropertiesListing() => AudioPropertiesListing;

        protected override IEnumerable<M.IDRMPropertiesListItem> GetDRMPropertiesListing() => DRMPropertiesListing;

        protected override IEnumerable<M.IGPSPropertiesListItem> GetGPSPropertiesListing() => GPSPropertiesListing;

        protected override IEnumerable<M.IImagePropertiesListItem> GetImagePropertiesListing() => ImagePropertiesListing;

        protected override IEnumerable<M.IMediaPropertiesListItem> GetMediaPropertiesListing() => MediaPropertiesListing;

        protected override IEnumerable<M.IMusicPropertiesListItem> GetMusicPropertiesListing() => MusicPropertiesListing;

        protected override IEnumerable<M.IPhotoPropertiesListItem> GetPhotoPropertiesListing() => PhotoPropertiesListing;

        protected override IEnumerable<M.IRecordedTVPropertiesListItem> GetRecordedTVPropertiesListing() => RecordedTVPropertiesListing;

        protected override IEnumerable<M.IVideoPropertiesListItem> GetVideoPropertiesListing() => VideoPropertiesListing;

        protected override IEnumerable<M.IItemTagListItem> GetPersonalVolumeTagListing() => PersonalVolumeTagListing;

        protected override IEnumerable<M.IItemTagListItem> GetSharedVolumeTagListing() => SharedVolumeTagListing;

        protected override IEnumerable<M.IItemTagListItem> GetPersonalSubdirectoryTagListing() => PersonalSubdirectoryTagListing;

        protected override IEnumerable<M.IItemTagListItem> GetSharedSubdirectoryTagListing() => SharedSubdirectoryTagListing;

        protected override IEnumerable<M.IItemTagListItem> GetPersonalFileTagListing() => PersonalFileTagListing;

        protected override IEnumerable<M.IItemTagListItem> GetSharedFileTagListing() => SharedFileTagListing;

        protected async override Task<M.IGPSPropertySet> FindGenericMatchingAsync(M.IGPSProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<M.IImagePropertySet> FindGenericMatchingAsync(M.IImageProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<M.IMediaPropertySet> FindGenericMatchingAsync(M.IMediaProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<M.IMusicPropertySet> FindGenericMatchingAsync(M.IMusicProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<M.IPhotoPropertySet> FindGenericMatchingAsync(M.IPhotoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<M.IRecordedTVPropertySet> FindGenericMatchingAsync(M.IRecordedTVProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<M.IVideoPropertySet> FindGenericMatchingAsync(M.IVideoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<M.ISummaryPropertySet> FindGenericMatchingAsync(M.ISummaryProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<M.IDocumentPropertySet> FindGenericMatchingAsync(M.IDocumentProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<M.IAudioPropertySet> FindGenericMatchingAsync(M.IAudioProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<M.IDRMPropertySet> FindGenericMatchingAsync(M.IDRMProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        #endregion

        #region Explicit Members

        IEnumerable<ILocalComparison> ILocalDbContext.Comparisons => Comparisons;

        IEnumerable<ILocalBinaryPropertySet> ILocalDbContext.BinaryPropertySets => BinaryPropertySets;

        IEnumerable<ILocalSummaryPropertySet> ILocalDbContext.SummaryPropertySets => SummaryPropertySets;

        IEnumerable<ILocalDocumentPropertySet> ILocalDbContext.DocumentPropertySets => DocumentPropertySets;

        IEnumerable<ILocalAudioPropertySet> ILocalDbContext.AudioPropertySets => AudioPropertySets;

        IEnumerable<ILocalDRMPropertySet> ILocalDbContext.DRMPropertySets => DRMPropertySets;

        IEnumerable<ILocalGPSPropertySet> ILocalDbContext.GPSPropertySets => GPSPropertySets;

        IEnumerable<ILocalImagePropertySet> ILocalDbContext.ImagePropertySets => ImagePropertySets;

        IEnumerable<ILocalMediaPropertySet> ILocalDbContext.MediaPropertySets => MediaPropertySets;

        IEnumerable<ILocalMusicPropertySet> ILocalDbContext.MusicPropertySets => MusicPropertySets;

        IEnumerable<ILocalPhotoPropertySet> ILocalDbContext.PhotoPropertySets => PhotoPropertySets;

        IEnumerable<ILocalRecordedTVPropertySet> ILocalDbContext.RecordedTVPropertySets => RecordedTVPropertySets;

        IEnumerable<ILocalVideoPropertySet> ILocalDbContext.VideoPropertySets => VideoPropertySets;

        IEnumerable<ILocalFileAccessError> ILocalDbContext.FileAccessErrors => FileAccessErrors;

        IEnumerable<ILocalFile> ILocalDbContext.Files => Files;

        IEnumerable<ILocalFileSystem> ILocalDbContext.FileSystems => FileSystems;

        IEnumerable<ILocalRedundancy> ILocalDbContext.Redundancies => Redundancies;

        IEnumerable<ILocalRedundantSet> ILocalDbContext.RedundantSets => RedundantSets;

        IEnumerable<ILocalSubdirectory> ILocalDbContext.Subdirectories => Subdirectories;

        IEnumerable<ILocalSubdirectoryAccessError> ILocalDbContext.SubdirectoryAccessErrors => SubdirectoryAccessErrors;

        IEnumerable<ILocalSymbolicName> ILocalDbContext.SymbolicNames => SymbolicNames;

        IEnumerable<ILocalVolumeAccessError> ILocalDbContext.VolumeAccessErrors => VolumeAccessErrors;

        IEnumerable<ILocalVolume> ILocalDbContext.Volumes => Volumes;

        IEnumerable<ILocalCrawlConfiguration> ILocalDbContext.CrawlConfigurations => CrawlConfigurations;

        IEnumerable<ILocalPersonalTagDefinition> ILocalDbContext.PersonalTagDefinitions => PersonalTagDefinitions;

        IEnumerable<ILocalPersonalFileTag> ILocalDbContext.PersonalFileTags => PersonalFileTags;

        IEnumerable<ILocalPersonalSubdirectoryTag> ILocalDbContext.PersonalSubdirectoryTags => PersonalSubdirectoryTags;

        IEnumerable<ILocalPersonalVolumeTag> ILocalDbContext.PersonalVolumeTags => PersonalVolumeTags;

        IEnumerable<ILocalSharedTagDefinition> ILocalDbContext.SharedTagDefinitions => SharedTagDefinitions;

        IEnumerable<ILocalSharedFileTag> ILocalDbContext.SharedFileTags => SharedFileTags;

        IEnumerable<ILocalSharedSubdirectoryTag> ILocalDbContext.SharedSubdirectoryTags => SharedSubdirectoryTags;

        IEnumerable<ILocalSharedVolumeTag> ILocalDbContext.SharedVolumeTags => SharedVolumeTags;

        IEnumerable<ILocalCrawlJobLog> ILocalDbContext.CrawlJobLogs => CrawlJobLogs;

        IEnumerable<M.ICrawlJobLog> M.IDbContext.CrawlJobLogs => CrawlJobLogs;

        IEnumerable<ILocalFileSystemListItem> ILocalDbContext.FileSystemListing => FileSystemListing;

        IEnumerable<ILocalTagDefinitionListItem> ILocalDbContext.PersonalTagDefinitionListing => PersonalTagDefinitionListing;

        IEnumerable<ILocalTagDefinitionListItem> ILocalDbContext.SharedTagDefinitionListing => SharedTagDefinitionListing;

        IEnumerable<ILocalRedundantSetListItem> ILocalDbContext.RedundantSetListing => RedundantSetListing;

        IEnumerable<ILocalVolumeListItem> ILocalDbContext.VolumeListing => VolumeListing;

        IEnumerable<ILocalVolumeListItemWithFileSystem> ILocalDbContext.VolumeListingWithFileSystem => VolumeListingWithFileSystem;

        IEnumerable<ILocalSubdirectoryListItem> ILocalDbContext.SubdirectoryListing => SubdirectoryListing;

        IEnumerable<ILocalSubdirectoryListItemWithAncestorNames> ILocalDbContext.SubdirectoryListingWithAncestorNames => SubdirectoryListingWithAncestorNames;

        IEnumerable<ILocalFileListItemWithAncestorNames> ILocalDbContext.FileListingWithAncestorNames => FileListingWithAncestorNames;

        IEnumerable<ILocalFileListItemWithBinaryProperties> ILocalDbContext.FileListingWithBinaryProperties => FileListingWithBinaryProperties;

        IEnumerable<ILocalFileListItemWithBinaryPropertiesAndAncestorNames> ILocalDbContext.FileListingWithBinaryPropertiesAndAncestorNames => FileListingWithBinaryPropertiesAndAncestorNames;

        IEnumerable<ILocalCrawlConfigurationListItem> ILocalDbContext.CrawlConfigListing => CrawlConfigListing;

        IEnumerable<ILocalCrawlConfigReportItem> ILocalDbContext.CrawlConfigReport => CrawlConfigReport;

        IEnumerable<ILocalCrawlJobListItem> ILocalDbContext.CrawlJobListing => CrawlJobListing;

        IEnumerable<ILocalSummaryPropertiesListItem> ILocalDbContext.SummaryPropertiesListing => SummaryPropertiesListing;

        IEnumerable<ILocalDocumentPropertiesListItem> ILocalDbContext.DocumentPropertiesListing => DocumentPropertiesListing;

        IEnumerable<ILocalAudioPropertiesListItem> ILocalDbContext.AudioPropertiesListing => AudioPropertiesListing;

        IEnumerable<ILocalDRMPropertiesListItem> ILocalDbContext.DRMPropertiesListing => DRMPropertiesListing;

        IEnumerable<ILocalGPSPropertiesListItem> ILocalDbContext.GPSPropertiesListing => GPSPropertiesListing;

        IEnumerable<ILocalImagePropertiesListItem> ILocalDbContext.ImagePropertiesListing => ImagePropertiesListing;

        IEnumerable<ILocalMediaPropertiesListItem> ILocalDbContext.MediaPropertiesListing => MediaPropertiesListing;

        IEnumerable<ILocalMusicPropertiesListItem> ILocalDbContext.MusicPropertiesListing => MusicPropertiesListing;

        IEnumerable<ILocalPhotoPropertiesListItem> ILocalDbContext.PhotoPropertiesListing => PhotoPropertiesListing;

        IEnumerable<ILocalRecordedTVPropertiesListItem> ILocalDbContext.RecordedTVPropertiesListing => RecordedTVPropertiesListing;

        IEnumerable<ILocalVideoPropertiesListItem> ILocalDbContext.VideoPropertiesListing => VideoPropertiesListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.PersonalVolumeTagListing => PersonalVolumeTagListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.SharedVolumeTagListing => SharedVolumeTagListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.PersonalSubdirectoryTagListing => PersonalSubdirectoryTagListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.SharedSubdirectoryTagListing => SharedSubdirectoryTagListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.PersonalFileTagListing => PersonalFileTagListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.SharedFileTagListing => SharedFileTagListing;

        async Task<ILocalSummaryPropertySet> ILocalDbContext.FindMatchingAsync(M.ISummaryProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalDocumentPropertySet> ILocalDbContext.FindMatchingAsync(M.IDocumentProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalAudioPropertySet> ILocalDbContext.FindMatchingAsync(M.IAudioProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalDRMPropertySet> ILocalDbContext.FindMatchingAsync(M.IDRMProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalGPSPropertySet> ILocalDbContext.FindMatchingAsync(M.IGPSProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalImagePropertySet> ILocalDbContext.FindMatchingAsync(M.IImageProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalMediaPropertySet> ILocalDbContext.FindMatchingAsync(M.IMediaProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalMusicPropertySet> ILocalDbContext.FindMatchingAsync(M.IMusicProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalPhotoPropertySet> ILocalDbContext.FindMatchingAsync(M.IPhotoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalRecordedTVPropertySet> ILocalDbContext.FindMatchingAsync(M.IRecordedTVProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalVideoPropertySet> ILocalDbContext.FindMatchingAsync(M.IVideoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        #endregion
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
