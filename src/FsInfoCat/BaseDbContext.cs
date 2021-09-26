using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public abstract class BaseDbContext : DbContext, IDbContext
    {
        private static readonly object _syncRoot = new();
        private readonly ILogger<BaseDbContext> _logger;
        private static readonly Dictionary<Guid, List<int>> _scopes = new();
        private readonly IDisposable _loggerScope;

        protected BaseDbContext([NotNull] DbContextOptions options) : base(options)
        {
            _logger = Services.ServiceProvider.GetRequiredService<ILogger<BaseDbContext>>();
            lock (_syncRoot)
            {
                if (_scopes.TryGetValue(ContextId.InstanceId, out List<int> list))
                {
                    if (list.Contains(ContextId.Lease))
                    {
                        _loggerScope = null;
                        return;
                    }
                }
                else
                {
                    list = new();
                    _scopes.Add(ContextId.InstanceId, list);
                }
                list.Add(ContextId.Lease);
            }
            ChangeTracker.Tracked += ChangeTracker_Tracked;

            _loggerScope = _logger.BeginScope($"{{{nameof(Database.ProviderName)}}}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}, ConnectionString={{ConnectionString}}",
                Database.ProviderName, ContextId.InstanceId, ContextId.Lease, Database.GetConnectionString());
        }

        private void ChangeTracker_Tracked(object sender, EntityTrackedEventArgs e)
        {
            if (e.Entry.Entity is RevertibleChangeTracking rtc && e.Entry.State == EntityState.Unchanged)
                rtc.AcceptChanges();
        }

        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Inherited class will have called SuppressFinalize if necessary.")]
        public override void Dispose()
        {
            base.Dispose();
            if (_loggerScope is not null)
            {
                lock (_syncRoot)
                {
                    if (!(_scopes.TryGetValue(ContextId.InstanceId, out List<int> list) && list.Remove(ContextId.Lease)))
                        return;
                    if (list.Count == 0)
                        _ = _scopes.Remove(ContextId.InstanceId);
                }
                _loggerScope.Dispose();
            }
        }

        private async Task<(EntityEntry Entry, EntityState OldState)[]> RaiseBeforeSaveAsync(CancellationToken cancellationToken = default)
        {
            using IDisposable scope = _logger.BeginScope(nameof(RaiseBeforeSaveAsync));
            cancellationToken.ThrowIfCancellationRequested();
            List<(EntityEntry Entry, EntityState OldState)> toSave = new();
            foreach (EntityEntry e in ChangeTracker.Entries())
            {
                object entity = e.Entity;
                switch (e.State)
                {
                    case EntityState.Added:
                        _logger.LogTrace("Inserting {Name}: {Entity}", e.Metadata.Name, entity);
                        if (entity is IDbEntity adding)
                            OnBeforeAddEntity(adding, cancellationToken);
                        if (entity is IDbEntityBeforeSave beforeSave)
                            await beforeSave.BeforeSaveAsync(cancellationToken);
                        if (entity is IDbEntityBeforeInsert beforeInsert)
                            await beforeInsert.BeforeInsertAsync(cancellationToken);
                        toSave.Add((Entry: e, OldState: EntityState.Added));
                        break;
                    case EntityState.Modified:
                        _logger.LogTrace("Saving {Name}: {Entity}", e.Metadata.Name, entity);
                        if (entity is IDbEntity saving)
                            OnBeforeSaveChanges(saving, cancellationToken);
                        if (entity is IDbEntityBeforeSave beforeSave2)
                            await beforeSave2.BeforeSaveAsync(cancellationToken);
                        if (entity is IDbEntityBeforeSaveChanges beforeSaveChanges)
                            await beforeSaveChanges.BeforeSaveChangesAsync(cancellationToken);
                        toSave.Add((Entry: e, OldState: EntityState.Modified));
                        break;
                    case EntityState.Deleted:
                        _logger.LogTrace("Deleting {Name}: {Entity}", e.Metadata.Name, entity);
                        if (entity is IDbEntityBeforeDelete beforeDelete)
                        {
                            await beforeDelete.BeforeDeleteAsync(cancellationToken);
                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        toSave.Add((Entry: e, OldState: EntityState.Deleted));
                        continue;
                    default:
                        continue;
                }
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogTrace("Validating {Name}: {Entity}", e.Metadata.Name, entity);
                ValidationContext validationContext = new(entity, new DbContextServiceProvider(this, e), null);
                Validator.ValidateObject(entity, validationContext, true);
            }
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogTrace("Returning {Count} items", toSave.Count);
            return toSave.ToArray();
        }

        private async Task RaiseAfterSaveAsync((EntityEntry Entry, EntityState OldState)[] saved, CancellationToken cancellationToken = default)
        {
            using IDisposable scope = _logger.BeginScope(nameof(RaiseAfterSaveAsync));
            cancellationToken.ThrowIfCancellationRequested();
            foreach ((EntityEntry Entry, EntityState OldState) e in saved)
            {
                EntityEntry entry = e.Entry;
                object entity = entry.Entity;
                switch (e.OldState)
                {
                    case EntityState.Added:
                        if (entry.State != EntityState.Unchanged)
                            _logger.LogWarning("Failed to insert {Name}: State = {State}; Entity = {Entity}", entry.Metadata.Name, entry.State, entity);
                        else
                        {
                            _logger.LogTrace("Inserted {Name}: {Entity}", entry.Metadata.Name, entity);
                            if (entity is IDbEntityAfterSave afterSave)
                                await afterSave.AfterSaveAsync(cancellationToken);
                            if (entity is IDbEntityAfterInsert afterInsert)
                                await afterInsert.AfterInsertAsync(cancellationToken);
                        }
                        break;
                    case EntityState.Modified:
                        if (entry.State != EntityState.Unchanged)
                            _logger.LogWarning("Failed to update {Name}: State = {State}; Entity = {Entity}", entry.Metadata.Name, entry.State, entity);
                        else
                        {
                            _logger.LogTrace("Updated {Name}: {Entity}", entry.Metadata.Name, entity);
                            if (entity is IDbEntityAfterSave afterSave2)
                                await afterSave2.AfterSaveAsync(cancellationToken);
                            if (entity is IDbEntityAfterSaveChanges afterSaveChanges)
                                await afterSaveChanges.AfterSaveChangesAsync(cancellationToken);
                        }
                        break;
                    case EntityState.Deleted:
                        if (entry.State != EntityState.Detached)
                            _logger.LogWarning("Failed to delete {Name}: State = {State}; Entity = {Entity}", entry.Metadata.Name, entry.State, entity);
                        else
                        {
                            _logger.LogTrace("Deleted {Name}: {Entity}", entry.Metadata.Name, entity);
                            if (entity is IDbEntityAfterDelete afterDelete)
                                await afterDelete.AfterDeleteAsync(cancellationToken);
                        }
                        break;
                    default:
                        continue;
                }
                if (entity is IDbEntity dbEntity)
                    dbEntity.AcceptChanges();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        protected virtual void OnBeforeAddEntity(IDbEntity entity, CancellationToken cancellationToken)
        {
            entity.CreatedOn = entity.ModifiedOn = DateTime.Now;
        }

        protected virtual void OnBeforeSaveChanges(IDbEntity entity, CancellationToken cancellationToken)
        {
            entity.ModifiedOn = DateTime.Now;
        }

        public override int SaveChanges()
        {
            using IDisposable scope = _logger.BeginScope("{MethodName}()", nameof(SaveChanges));
            (EntityEntry Entry, EntityState OldState)[] changes = RaiseBeforeSaveAsync().Result;
            int returnValue = base.SaveChanges();
            RaiseAfterSaveAsync(changes).Wait();
            _logger.LogTrace("Returning {ReturnValue}", returnValue);
            return returnValue;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            using IDisposable scope = _logger.BeginScope($"{{MethodName}}({nameof(acceptAllChangesOnSuccess)}: {{{nameof(acceptAllChangesOnSuccess)}}})", nameof(SaveChanges),
                acceptAllChangesOnSuccess);
            (EntityEntry Entry, EntityState OldState)[] changes = RaiseBeforeSaveAsync().Result;
            int returnValue = base.SaveChanges(acceptAllChangesOnSuccess);
            RaiseAfterSaveAsync(changes).Wait();
            _logger.LogTrace("Returning {ReturnValue}", returnValue);
            return returnValue;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            using IDisposable scope = _logger.BeginScope($"{{MethodName}}({nameof(acceptAllChangesOnSuccess)}: {{{nameof(acceptAllChangesOnSuccess)}}})", nameof(SaveChangesAsync),
                acceptAllChangesOnSuccess);
            (EntityEntry Entry, EntityState OldState)[] changes = await RaiseBeforeSaveAsync(cancellationToken);
            int returnValue = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await RaiseAfterSaveAsync(changes, cancellationToken);
            _logger.LogTrace("Returning {ReturnValue}", returnValue);
            return returnValue;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            using IDisposable scope = _logger.BeginScope("{MethodName}()", nameof(SaveChangesAsync));
            (EntityEntry Entry, EntityState OldState)[] changes = await RaiseBeforeSaveAsync(cancellationToken);
            int returnValue = await base.SaveChangesAsync(cancellationToken);
            await RaiseAfterSaveAsync(changes, cancellationToken);
            _logger.LogTrace("Returning {ReturnValue}", returnValue);
            return returnValue;
        }

        protected abstract IEnumerable<IComparison> GetGenericComparisons();

        protected abstract IEnumerable<IBinaryPropertySet> GetGenericBinaryPropertySets();

        protected abstract IEnumerable<ISummaryPropertySet> GetGenericSummaryPropertySets();

        protected abstract IEnumerable<IDocumentPropertySet> GetGenericDocumentPropertySets();

        protected abstract IEnumerable<IAudioPropertySet> GetGenericAudioPropertySets();

        protected abstract IEnumerable<IDRMPropertySet> GetGenericDRMPropertySets();

        protected abstract IEnumerable<IGPSPropertySet> GetGenericGPSPropertySets();

        protected abstract IEnumerable<IImagePropertySet> GetGenericImagePropertySets();

        protected abstract IEnumerable<IMediaPropertySet> GetGenericMediaPropertySets();

        protected abstract IEnumerable<IMusicPropertySet> GetGenericMusicPropertySets();

        protected abstract IEnumerable<IPhotoPropertySet> GetGenericPhotoPropertySets();

        protected abstract IEnumerable<IRecordedTVPropertySet> GetGenericRecordedTVPropertySets();

        protected abstract IEnumerable<IVideoPropertySet> GetGenericVideoPropertySets();

        protected abstract IEnumerable<IFileAccessError> GetGenericFileAccessErrors();

        protected abstract IEnumerable<IFile> GetGenericFiles();

        protected abstract IEnumerable<IFileSystem> GetGenericFileSystems();

        protected abstract IEnumerable<IRedundancy> GetGenericRedundancies();

        protected abstract IEnumerable<IRedundantSet> GetGenericRedundantSets();

        protected abstract IEnumerable<ISubdirectory> GetGenericSubdirectories();

        protected abstract IEnumerable<ISubdirectoryAccessError> GetGenericSubdirectoryAccessErrors();

        protected abstract IEnumerable<ISymbolicName> GetGenericSymbolicNames();

        protected abstract IEnumerable<IVolumeAccessError> GetGenericVolumeAccessErrors();

        protected abstract IEnumerable<IVolume> GetGenericVolumes();

        protected abstract IEnumerable<ICrawlConfiguration> GetGenericCrawlConfigurations();

        protected abstract IEnumerable<IFileAccessError> GetFileAccessErrors();

        protected abstract IEnumerable<ISubdirectoryAccessError> GetSubdirectoryAccessErrors();

        protected abstract IEnumerable<IVolumeAccessError> GetVolumeAccessErrors();

        protected abstract IEnumerable<IPersonalTagDefinition> GetPersonalTagDefinitions();

        protected abstract IEnumerable<IPersonalFileTag> GetPersonalFileTags();

        protected abstract IEnumerable<IPersonalSubdirectoryTag> GetPersonalSubdirectoryTags();

        protected abstract IEnumerable<IPersonalVolumeTag> GetPersonalVolumeTags();

        protected abstract IEnumerable<ISharedTagDefinition> GetSharedTagDefinitions();

        protected abstract IEnumerable<ISharedFileTag> GetSharedFileTags();

        protected abstract IEnumerable<ISharedSubdirectoryTag> GetSharedSubdirectoryTags();

        protected abstract IEnumerable<ISharedVolumeTag> GetSharedVolumeTags();

        protected abstract IEnumerable<ICrawlJobLog> GetCrawlJobLogs();

        protected abstract IEnumerable<ISymbolicNameListItem> GetSymbolicNameListing();

        protected abstract IEnumerable<IFileSystemListItem> GetFileSystemListing();

        protected abstract IEnumerable<ITagDefinitionListItem> GetPersonalTagDefinitionListing();

        protected abstract IEnumerable<ITagDefinitionListItem> GetSharedTagDefinitionListing();

        protected abstract IEnumerable<IRedundantSetListItem> GetRedundantSetListing();

        protected abstract IEnumerable<IVolumeListItem> GetVolumeListing();

        protected abstract IEnumerable<IVolumeListItemWithFileSystem> GetVolumeListingWithFileSystem();

        protected abstract IEnumerable<ISubdirectoryListItem> GetSubdirectoryListing();

        protected abstract IEnumerable<ISubdirectoryListItemWithAncestorNames> GetSubdirectoryListingWithAncestorNames();

        protected abstract IEnumerable<ISubdirectoryAncestorName> GetSubdirectoryAncestorNames();

        protected abstract IEnumerable<IFileListItemWithAncestorNames> GetFileListingWithAncestorNames();

        protected abstract IEnumerable<IFileListItemWithBinaryProperties> GetFileListingWithBinaryProperties();

        protected abstract IEnumerable<IFileListItemWithBinaryPropertiesAndAncestorNames> GetFileListingWithBinaryPropertiesAndAncestorNames();

        protected abstract IEnumerable<ICrawlConfigurationListItem> GetCrawlConfigListing();

        protected abstract IEnumerable<ICrawlConfigReportItem> GetCrawlConfigReport();

        protected abstract IEnumerable<ICrawlJobListItem> GetCrawlJobListing();

        protected abstract IEnumerable<ISummaryPropertiesListItem> GetSummaryPropertiesListing();

        protected abstract IEnumerable<IDocumentPropertiesListItem> GetDocumentPropertiesListing();

        protected abstract IEnumerable<IAudioPropertiesListItem> GetAudioPropertiesListing();

        protected abstract IEnumerable<IDRMPropertiesListItem> GetDRMPropertiesListing();

        protected abstract IEnumerable<IGPSPropertiesListItem> GetGPSPropertiesListing();

        protected abstract IEnumerable<IImagePropertiesListItem> GetImagePropertiesListing();

        protected abstract IEnumerable<IMediaPropertiesListItem> GetMediaPropertiesListing();

        protected abstract IEnumerable<IMusicPropertiesListItem> GetMusicPropertiesListing();

        protected abstract IEnumerable<IPhotoPropertiesListItem> GetPhotoPropertiesListing();

        protected abstract IEnumerable<IRecordedTVPropertiesListItem> GetRecordedTVPropertiesListing();

        protected abstract IEnumerable<IVideoPropertiesListItem> GetVideoPropertiesListing();

        protected abstract IEnumerable<IItemTagListItem> GetPersonalVolumeTagListing();

        protected abstract IEnumerable<IItemTagListItem> GetSharedVolumeTagListing();

        protected abstract IEnumerable<IItemTagListItem> GetPersonalSubdirectoryTagListing();

        protected abstract IEnumerable<IItemTagListItem> GetSharedSubdirectoryTagListing();

        protected abstract IEnumerable<IItemTagListItem> GetPersonalFileTagListing();

        protected abstract IEnumerable<IItemTagListItem> GetSharedFileTagListing();

        protected abstract Task<IGPSPropertySet> FindGenericMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken);

        protected abstract Task<IImagePropertySet> FindGenericMatchingAsync(IImageProperties properties, CancellationToken cancellationToken);

        protected abstract Task<IMediaPropertySet> FindGenericMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken);

        protected abstract Task<IMusicPropertySet> FindGenericMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken);

        protected abstract Task<IPhotoPropertySet> FindGenericMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken);

        protected abstract Task<IRecordedTVPropertySet> FindGenericMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken);

        protected abstract Task<IVideoPropertySet> FindGenericMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken);

        protected abstract Task<ISummaryPropertySet> FindGenericMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken);

        protected abstract Task<IDocumentPropertySet> FindGenericMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken);

        protected abstract Task<IAudioPropertySet> FindGenericMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken);

        protected abstract Task<IDRMPropertySet> FindGenericMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken);

        #region Explicit Members

        IEnumerable<IComparison> IDbContext.Comparisons => GetGenericComparisons();
        IEnumerable<IBinaryPropertySet> IDbContext.BinaryPropertySets => GetGenericBinaryPropertySets();
        IEnumerable<ISummaryPropertySet> IDbContext.SummaryPropertySets => GetGenericSummaryPropertySets();
        IEnumerable<IDocumentPropertySet> IDbContext.DocumentPropertySets => GetGenericDocumentPropertySets();
        IEnumerable<IAudioPropertySet> IDbContext.AudioPropertySets => GetGenericAudioPropertySets();
        IEnumerable<IDRMPropertySet> IDbContext.DRMPropertySets => GetGenericDRMPropertySets();
        IEnumerable<IGPSPropertySet> IDbContext.GPSPropertySets => GetGenericGPSPropertySets();
        IEnumerable<IImagePropertySet> IDbContext.ImagePropertySets => GetGenericImagePropertySets();
        IEnumerable<IMediaPropertySet> IDbContext.MediaPropertySets => GetGenericMediaPropertySets();
        IEnumerable<IMusicPropertySet> IDbContext.MusicPropertySets => GetGenericMusicPropertySets();
        IEnumerable<IPhotoPropertySet> IDbContext.PhotoPropertySets => GetGenericPhotoPropertySets();
        IEnumerable<IRecordedTVPropertySet> IDbContext.RecordedTVPropertySets => GetGenericRecordedTVPropertySets();
        IEnumerable<IVideoPropertySet> IDbContext.VideoPropertySets => GetGenericVideoPropertySets();
        IEnumerable<IFile> IDbContext.Files => GetGenericFiles();
        IEnumerable<IFileSystem> IDbContext.FileSystems => GetGenericFileSystems();
        IEnumerable<IRedundancy> IDbContext.Redundancies => GetGenericRedundancies();
        IEnumerable<IRedundantSet> IDbContext.RedundantSets => GetGenericRedundantSets();
        IEnumerable<ISubdirectory> IDbContext.Subdirectories => GetGenericSubdirectories();
        IEnumerable<ISymbolicName> IDbContext.SymbolicNames => GetGenericSymbolicNames();
        IEnumerable<IVolume> IDbContext.Volumes => GetGenericVolumes();
        IEnumerable<ICrawlConfiguration> IDbContext.CrawlConfigurations => GetGenericCrawlConfigurations();
        IEnumerable<IFileAccessError> IDbContext.FileAccessErrors => GetFileAccessErrors();
        IEnumerable<ISubdirectoryAccessError> IDbContext.SubdirectoryAccessErrors => GetSubdirectoryAccessErrors();
        IEnumerable<IVolumeAccessError> IDbContext.VolumeAccessErrors => GetVolumeAccessErrors();
        IEnumerable<IPersonalTagDefinition> IDbContext.PersonalTagDefinitions => GetPersonalTagDefinitions();
        IEnumerable<IPersonalFileTag> IDbContext.PersonalFileTags => GetPersonalFileTags();
        IEnumerable<IPersonalSubdirectoryTag> IDbContext.PersonalSubdirectoryTags => GetPersonalSubdirectoryTags();
        IEnumerable<IPersonalVolumeTag> IDbContext.PersonalVolumeTags => GetPersonalVolumeTags();
        IEnumerable<ISharedTagDefinition> IDbContext.SharedTagDefinitions => GetSharedTagDefinitions();
        IEnumerable<ISharedFileTag> IDbContext.SharedFileTags => GetSharedFileTags();
        IEnumerable<ISharedSubdirectoryTag> IDbContext.SharedSubdirectoryTags => GetSharedSubdirectoryTags();
        IEnumerable<ISharedVolumeTag> IDbContext.SharedVolumeTags => GetSharedVolumeTags();

        IEnumerable<ICrawlJobLog> IDbContext.CrawlJobLogs => GetCrawlJobLogs();

        IEnumerable<ISymbolicNameListItem> IDbContext.SymbolicNameListing => GetSymbolicNameListing();

        IEnumerable<IFileSystemListItem> IDbContext.FileSystemListing => GetFileSystemListing();

        IEnumerable<ITagDefinitionListItem> IDbContext.PersonalTagDefinitionListing => GetPersonalTagDefinitionListing();

        IEnumerable<ITagDefinitionListItem> IDbContext.SharedTagDefinitionListing => GetSharedTagDefinitionListing();

        IEnumerable<IRedundantSetListItem> IDbContext.RedundantSetListing => GetRedundantSetListing();

        IEnumerable<IVolumeListItem> IDbContext.VolumeListing => GetVolumeListing();

        IEnumerable<IVolumeListItemWithFileSystem> IDbContext.VolumeListingWithFileSystem => GetVolumeListingWithFileSystem();

        IEnumerable<ISubdirectoryListItem> IDbContext.SubdirectoryListing => GetSubdirectoryListing();

        IEnumerable<ISubdirectoryListItemWithAncestorNames> IDbContext.SubdirectoryListingWithAncestorNames => GetSubdirectoryListingWithAncestorNames();

        IEnumerable<ISubdirectoryAncestorName> IDbContext.SubdirectoryAncestorNames => GetSubdirectoryAncestorNames();

        IEnumerable<IFileListItemWithAncestorNames> IDbContext.FileListingWithAncestorNames => GetFileListingWithAncestorNames();

        IEnumerable<IFileListItemWithBinaryProperties> IDbContext.FileListingWithBinaryProperties => GetFileListingWithBinaryProperties();

        IEnumerable<IFileListItemWithBinaryPropertiesAndAncestorNames> IDbContext.FileListingWithBinaryPropertiesAndAncestorNames => GetFileListingWithBinaryPropertiesAndAncestorNames();

        IEnumerable<ICrawlConfigurationListItem> IDbContext.CrawlConfigListing => GetCrawlConfigListing();

        IEnumerable<ICrawlConfigReportItem> IDbContext.CrawlConfigReport => GetCrawlConfigReport();

        IEnumerable<ICrawlJobListItem> IDbContext.CrawlJobListing => GetCrawlJobListing();

        IEnumerable<ISummaryPropertiesListItem> IDbContext.SummaryPropertiesListing => GetSummaryPropertiesListing();

        IEnumerable<IDocumentPropertiesListItem> IDbContext.DocumentPropertiesListing => GetDocumentPropertiesListing();

        IEnumerable<IAudioPropertiesListItem> IDbContext.AudioPropertiesListing => GetAudioPropertiesListing();

        IEnumerable<IDRMPropertiesListItem> IDbContext.DRMPropertiesListing => GetDRMPropertiesListing();

        IEnumerable<IGPSPropertiesListItem> IDbContext.GPSPropertiesListing => GetGPSPropertiesListing();

        IEnumerable<IImagePropertiesListItem> IDbContext.ImagePropertiesListing => GetImagePropertiesListing();

        IEnumerable<IMediaPropertiesListItem> IDbContext.MediaPropertiesListing => GetMediaPropertiesListing();

        IEnumerable<IMusicPropertiesListItem> IDbContext.MusicPropertiesListing => GetMusicPropertiesListing();

        IEnumerable<IPhotoPropertiesListItem> IDbContext.PhotoPropertiesListing => GetPhotoPropertiesListing();

        IEnumerable<IRecordedTVPropertiesListItem> IDbContext.RecordedTVPropertiesListing => GetRecordedTVPropertiesListing();

        IEnumerable<IVideoPropertiesListItem> IDbContext.VideoPropertiesListing => GetVideoPropertiesListing();

        IEnumerable<IItemTagListItem> IDbContext.PersonalVolumeTagListing => GetPersonalVolumeTagListing();

        IEnumerable<IItemTagListItem> IDbContext.SharedVolumeTagListing => GetSharedVolumeTagListing();

        IEnumerable<IItemTagListItem> IDbContext.PersonalSubdirectoryTagListing => GetPersonalSubdirectoryTagListing();

        IEnumerable<IItemTagListItem> IDbContext.SharedSubdirectoryTagListing => GetSharedSubdirectoryTagListing();

        IEnumerable<IItemTagListItem> IDbContext.PersonalFileTagListing => GetPersonalFileTagListing();

        IEnumerable<IItemTagListItem> IDbContext.SharedFileTagListing => GetSharedFileTagListing();

        Task<ISummaryPropertySet> IDbContext.FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);
        Task<IDocumentPropertySet> IDbContext.FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);
        Task<IAudioPropertySet> IDbContext.FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);
        Task<IDRMPropertySet> IDbContext.FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);
        Task<IGPSPropertySet> IDbContext.FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);
        Task<IImagePropertySet> IDbContext.FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);
        Task<IMediaPropertySet> IDbContext.FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);
        Task<IMusicPropertySet> IDbContext.FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);
        Task<IPhotoPropertySet> IDbContext.FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);
        Task<IRecordedTVPropertySet> IDbContext.FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);
        Task<IVideoPropertySet> IDbContext.FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken)
            => FindGenericMatchingAsync(properties, cancellationToken);

        #endregion

        private class DbContextServiceProvider : ProxyServiceProvider
        {
            private readonly object _entity;
            private readonly BaseDbContext _dbContext;
            private readonly Type _entityType;

            internal DbContextServiceProvider([DisallowNull] BaseDbContext dbContext, [DisallowNull] object entity)
                : base(Services.ServiceProvider)
            {
                _dbContext = dbContext;
                _entityType = (_entity = entity).GetType();
            }

            protected override bool TryGetService(Type serviceType, out object service)
            {
                if (serviceType is null)
                {
                    service = null;
                    return false;
                }
                if (serviceType.IsInstanceOfType(_entity))
                    service = _entity;
                else if (serviceType.IsInstanceOfType(_dbContext))
                    service = _dbContext;
                else if (serviceType.Equals(typeof(EntityEntry)))
                    service = _dbContext.Entry(_entity);
                else
                {
                    service = Services.ServiceProvider.GetService(serviceType);
                    return service is not null;
                }
                return true;
            }
        }
        public class Interceptor : DbCommandInterceptor
        {
            private ILogger<BaseDbContext> _logger;

            public ILogger<BaseDbContext> Logger
            {
                get
                {
                    if (_logger is null)
                        _logger = Services.ServiceProvider.GetRequiredService<ILogger<BaseDbContext>>();
                    return _logger;
                }
            }

            public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
            {
                Logger.LogInformation("NonQueryExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.NonQueryExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
                CancellationToken cancellationToken = default)
            {
                Logger.LogInformation("NonQueryExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
            {
                Logger.LogInformation("ReaderExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ReaderExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
            {
                Logger.LogInformation("ReaderExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
            {
                Logger.LogInformation("ScalarExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ScalarExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
            {
                Logger.LogInformation("ScalarExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
            {
                Logger.LogInformation("CommandCreated: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    result.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.CommandCreated(eventData, result);
            }

            public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
            {
                Logger.LogError(eventData.Exception, "CommandFailed: {Message}; CommandText={CommandText}, ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode, eventData.Exception.Message);
                base.CommandFailed(command, eventData);
            }

            public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
            {
                Logger.LogError(eventData.Exception, "CommandFailedAsync: {Message}; CommandText={CommandText}, ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode, eventData.Exception.Message);
                return base.CommandFailedAsync(command, eventData, cancellationToken);
            }
        }
    }
}
