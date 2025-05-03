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

namespace FsInfoCat.Model
{
    /// <summary>
    /// Base database context for local and upstream databases.
    /// </summary>
    public abstract class BaseDbContext : DbContext, IDbContext
    {
        private static readonly object _syncRoot = new();
        private readonly ILogger<BaseDbContext> _logger;
        private static readonly Dictionary<Guid, List<int>> _scopes = [];
        private readonly IDisposable _loggerScope;

        /// <summary>
        /// Instantiates a new <see cref="BaseDbContext" />.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <seealso cref="DbContext(DbContextOptions)" />
        protected BaseDbContext([NotNull] DbContextOptions options) : base(options)
        {
            _logger = Hosting.ServiceProvider.GetRequiredService<ILogger<BaseDbContext>>();
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
                    list = [];
                    _scopes.Add(ContextId.InstanceId, list);
                }
                list.Add(ContextId.Lease);
            }

            _loggerScope = _logger.BeginScope($"{{{nameof(Database.ProviderName)}}}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}, ConnectionString={{ConnectionString}}",
                Database.ProviderName, ContextId.InstanceId, ContextId.Lease, Database.GetConnectionString());
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private async Task<(EntityEntry Entry, EntityState OldState)[]> RaiseBeforeSaveAsync(CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope(nameof(RaiseBeforeSaveAsync)))
            {
                cancellationToken.ThrowIfCancellationRequested();
                List<(EntityEntry Entry, EntityState OldState)> toSave = [];
                foreach (EntityEntry e in ChangeTracker.Entries())
                {
                    object entity = e.Entity;
                    switch (e.State)
                    {
                        case EntityState.Added:
                            _logger.LogTrace("Inserting {Name}: {Entity}", e.Metadata.Name, entity);
                            if (entity is IDbEntity adding)
                                OnBeforeAddEntity(adding, cancellationToken);
                            if (entity is IDbEntityHandlesBeforeSave beforeSave)
                                await beforeSave.BeforeSaveAsync(cancellationToken);
                            if (entity is IDbEntityHandlesBeforeInsert beforeInsert)
                                await beforeInsert.BeforeInsertAsync(cancellationToken);
                            toSave.Add((Entry: e, OldState: EntityState.Added));
                            break;
                        case EntityState.Modified:
                            _logger.LogTrace("Saving {Name}: {Entity}", e.Metadata.Name, entity);
                            if (entity is IDbEntity saving)
                                OnBeforeSaveChanges(saving, cancellationToken);
                            if (entity is IDbEntityHandlesBeforeSave beforeSave2)
                                await beforeSave2.BeforeSaveAsync(cancellationToken);
                            if (entity is IDbEntityHandlesBeforeSaveChanges beforeSaveChanges)
                                await beforeSaveChanges.BeforeSaveChangesAsync(cancellationToken);
                            toSave.Add((Entry: e, OldState: EntityState.Modified));
                            break;
                        case EntityState.Deleted:
                            _logger.LogTrace("Deleting {Name}: {Entity}", e.Metadata.Name, entity);
                            if (entity is IDbEntityHandlesBeforeDelete beforeDelete)
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
                return [.. toSave];
            }
        }

        private async Task RaiseAfterSaveAsync((EntityEntry Entry, EntityState OldState)[] saved, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope(nameof(RaiseAfterSaveAsync)))
            {
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
                                if (entity is IDbEntityHandlesAfterSave afterSave)
                                    await afterSave.AfterSaveAsync(cancellationToken);
                                if (entity is IDbEntityHandlesAfterInsert afterInsert)
                                    await afterInsert.AfterInsertAsync(cancellationToken);
                            }
                            break;
                        case EntityState.Modified:
                            if (entry.State != EntityState.Unchanged)
                                _logger.LogWarning("Failed to update {Name}: State = {State}; Entity = {Entity}", entry.Metadata.Name, entry.State, entity);
                            else
                            {
                                _logger.LogTrace("Updated {Name}: {Entity}", entry.Metadata.Name, entity);
                                if (entity is IDbEntityHandlesAfterSave afterSave2)
                                    await afterSave2.AfterSaveAsync(cancellationToken);
                                if (entity is IDbEntityHandlesAfterSaveChanges afterSaveChanges)
                                    await afterSaveChanges.AfterSaveChangesAsync(cancellationToken);
                            }
                            break;
                        case EntityState.Deleted:
                            if (entry.State != EntityState.Detached)
                                _logger.LogWarning("Failed to delete {Name}: State = {State}; Entity = {Entity}", entry.Metadata.Name, entry.State, entity);
                            else
                            {
                                _logger.LogTrace("Deleted {Name}: {Entity}", entry.Metadata.Name, entity);
                                if (entity is IDbEntityHandlesAfterDelete afterDelete)
                                    await afterDelete.AfterDeleteAsync(cancellationToken);
                            }
                            break;
                        default:
                            continue;
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }

        /// <summary>
        /// This gets called before a new entity is added to the associated database.
        /// </summary>
        /// <param name="entity">The <see cref="IDbEntity" /> that is about to be added.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe for task cancellation.</param>
        /// <remarks>This gets called before the entity is <see cref="Validator.ValidateObject(object, ValidationContext, bool)">validated</see>.</remarks>
        protected virtual void OnBeforeAddEntity(IDbEntity entity, CancellationToken cancellationToken)
        {
            entity.CreatedOn = entity.ModifiedOn = DateTime.Now;
        }

        /// <summary>
        /// This gets called before an existing entity is updated in the associated database.
        /// </summary>
        /// <param name="entity">The <see cref="IDbEntity" /> that is about to be updated.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe for task cancellation.</param>
        /// <remarks>This gets called before the entity is <see cref="Validator.ValidateObject(object, ValidationContext, bool)">validated</see>.</remarks>
        protected virtual void OnBeforeSaveChanges(IDbEntity entity, CancellationToken cancellationToken)
        {
            entity.ModifiedOn = DateTime.Now;
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="ValidationException">An entity is invalid.</exception>
        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.</exception>
        /// <seealso cref="DbContext.SaveChanges()" />
        public override int SaveChanges()
        {
            using (_logger.BeginScope("{MethodName}()", nameof(SaveChanges)))
            {
                (EntityEntry Entry, EntityState OldState)[] changes = RaiseBeforeSaveAsync().Result;
                int returnValue = base.SaveChanges();
                RaiseAfterSaveAsync(changes).Wait();
                _logger.LogTrace("Returning {ReturnValue}", returnValue);
                return returnValue;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="ChangeTracker.AcceptAllChanges" /> is called after the changes have been sent successfully to the database.</param>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="ValidationException">An entity is invalid.</exception>
        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.</exception>
        /// <seealso cref="DbContext.SaveChanges(bool)" />
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            using (_logger.BeginScope($"{{MethodName}}({nameof(acceptAllChangesOnSuccess)}: {{{nameof(acceptAllChangesOnSuccess)}}})", nameof(SaveChanges),
                acceptAllChangesOnSuccess))
            {
                (EntityEntry Entry, EntityState OldState)[] changes = RaiseBeforeSaveAsync().Result;
                int returnValue = base.SaveChanges(acceptAllChangesOnSuccess);
                RaiseAfterSaveAsync(changes).Wait();
                _logger.LogTrace("Returning {ReturnValue}", returnValue);
                return returnValue;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="ChangeTracker.AcceptAllChanges" /> is called after the changes have been sent successfully to the database.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation.
        /// <para>The task result contains the number of state entries written to the database.</para></returns>
        /// <exception cref="ValidationException">An entity is invalid.</exception>
        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.</exception>
        /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken" /> is canceled.</exception>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope($"{{MethodName}}({nameof(acceptAllChangesOnSuccess)}: {{{nameof(acceptAllChangesOnSuccess)}}})", nameof(SaveChangesAsync),
                acceptAllChangesOnSuccess))
            {
                (EntityEntry Entry, EntityState OldState)[] changes = await RaiseBeforeSaveAsync(cancellationToken);
                int returnValue = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                await RaiseAfterSaveAsync(changes, cancellationToken);
                _logger.LogTrace("Returning {ReturnValue}", returnValue);
                return returnValue;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation.
        /// <para>The task result contains the number of state entries written to the database.</para></returns>
        /// <exception cref="ValidationException">An entity is invalid.</exception>
        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.</exception>
        /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken" /> is canceled.</exception>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{MethodName}()", nameof(SaveChangesAsync)))
            {
                (EntityEntry Entry, EntityState OldState)[] changes = await RaiseBeforeSaveAsync(cancellationToken);
                int returnValue = await base.SaveChangesAsync(cancellationToken);
                await RaiseAfterSaveAsync(changes, cancellationToken);
                _logger.LogTrace("Returning {ReturnValue}", returnValue);
                return returnValue;
            }
        }

        /// <summary>
        /// Gets a generic enumeration of file comparison entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.Comparisons" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IComparison> GetGenericComparisons();

        /// <summary>
        /// Gets a generic enumeration of binary property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.BinaryPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IBinaryPropertySet> GetGenericBinaryPropertySets();

        /// <summary>
        /// Gets a generic enumeration of summary property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SummaryPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISummaryPropertySet> GetGenericSummaryPropertySets();

        /// <summary>
        /// Gets a generic enumeration of document property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.DocumentPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IDocumentPropertySet> GetGenericDocumentPropertySets();

        /// <summary>
        /// Gets a generic enumeration of audio property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.AudioPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IAudioPropertySet> GetGenericAudioPropertySets();

        /// <summary>
        /// Gets a generic enumeration of DRM property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.DRMPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IDRMPropertySet> GetGenericDRMPropertySets();

        /// <summary>
        /// Gets a generic enumeration of GPS property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.GPSPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IGPSPropertySet> GetGenericGPSPropertySets();

        /// <summary>
        /// Gets a generic enumeration of image property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.ImagePropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IImagePropertySet> GetGenericImagePropertySets();

        /// <summary>
        /// Gets a generic enumeration of media property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.MediaPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IMediaPropertySet> GetGenericMediaPropertySets();

        /// <summary>
        /// Gets a generic enumeration of music property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.MusicPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IMusicPropertySet> GetGenericMusicPropertySets();

        /// <summary>
        /// Gets a generic enumeration of photo property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.PhotoPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IPhotoPropertySet> GetGenericPhotoPropertySets();

        /// <summary>
        /// Gets a generic enumeration of recorded TV property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.RecordedTVPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IRecordedTVPropertySet> GetGenericRecordedTVPropertySets();

        /// <summary>
        /// Gets a generic enumeration of video property set entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.VideoPropertySets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IVideoPropertySet> GetGenericVideoPropertySets();

        /// <summary>
        /// Gets a generic enumeration of file access error entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.FileAccessErrors" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IFileAccessError> GetGenericFileAccessErrors();

        /// <summary>
        /// Gets a generic enumeration of file entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.Files" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IFile> GetGenericFiles();

        /// <summary>
        /// Gets a generic enumeration of filesystem entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.FileSystems" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IFileSystem> GetGenericFileSystems();

        /// <summary>
        /// Gets a generic enumeration of redundancy entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.Redundancies" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IRedundancy> GetGenericRedundancies();

        /// <summary>
        /// Gets a generic enumeration of redundant set  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.RedundantSets" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IRedundantSet> GetGenericRedundantSets();

        /// <summary>
        /// Gets a generic enumeration of subdirectory entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.Subdirectories" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISubdirectory> GetGenericSubdirectories();

        /// <summary>
        /// Gets a generic enumeration of subdirectory access error entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SubdirectoryAccessErrors" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISubdirectoryAccessError> GetGenericSubdirectoryAccessErrors();

        /// <summary>
        /// Gets a generic enumeration of symbolic name entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SymbolicNames" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISymbolicName> GetGenericSymbolicNames();

        /// <summary>
        /// Gets a generic enumeration of volume access error entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.VolumeAccessErrors" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IVolumeAccessError> GetGenericVolumeAccessErrors();

        /// <summary>
        /// Gets a generic enumeration of volume entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.Volumes" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IVolume> GetGenericVolumes();

        /// <summary>
        /// Gets a generic enumeration of crawl configuration entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.CrawlConfigurations" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ICrawlConfiguration> GetGenericCrawlConfigurations();

        /// <summary>
        /// Gets a generic enumeration of file access error entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.FileAccessErrors" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IFileAccessError> GetFileAccessErrors();

        /// <summary>
        /// Gets a generic enumeration of subdirectory access error entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SubdirectoryAccessErrors" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISubdirectoryAccessError> GetSubdirectoryAccessErrors();

        /// <summary>
        /// Gets a generic enumeration of volume access error entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.VolumeAccessErrors" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IVolumeAccessError> GetVolumeAccessErrors();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.PersonalTagDefinitions" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IPersonalTagDefinition> GetPersonalTagDefinitions();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.PersonalFileTags" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IPersonalFileTag> GetPersonalFileTags();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.PersonalSubdirectoryTags" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IPersonalSubdirectoryTag> GetPersonalSubdirectoryTags();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.PersonalVolumeTags" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IPersonalVolumeTag> GetPersonalVolumeTags();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SharedTagDefinitions" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISharedTagDefinition> GetSharedTagDefinitions();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SharedFileTags" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISharedFileTag> GetSharedFileTags();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SharedSubdirectoryTags" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISharedSubdirectoryTag> GetSharedSubdirectoryTags();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SharedVolumeTags" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISharedVolumeTag> GetSharedVolumeTags();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.CrawlJobLogs" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ICrawlJobLog> GetCrawlJobLogs();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SymbolicNameListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISymbolicNameListItem> GetSymbolicNameListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.FileSystemListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IFileSystemListItem> GetFileSystemListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.PersonalTagDefinitionListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ITagDefinitionListItem> GetPersonalTagDefinitionListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SharedTagDefinitionListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ITagDefinitionListItem> GetSharedTagDefinitionListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.RedundantSetListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IRedundantSetListItem> GetRedundantSetListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.VolumeListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IVolumeListItem> GetVolumeListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.VolumeListingWithFileSystem" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IVolumeListItemWithFileSystem> GetVolumeListingWithFileSystem();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SubdirectoryListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISubdirectoryListItem> GetSubdirectoryListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SubdirectoryListingWithAncestorNames" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISubdirectoryListItemWithAncestorNames> GetSubdirectoryListingWithAncestorNames();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SubdirectoryAncestorNames" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISubdirectoryAncestorName> GetSubdirectoryAncestorNames();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.FileListingWithAncestorNames" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IFileListItemWithAncestorNames> GetFileListingWithAncestorNames();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.FileListingWithBinaryProperties" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IFileListItemWithBinaryProperties> GetFileListingWithBinaryProperties();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.FileListingWithBinaryPropertiesAndAncestorNames" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IFileListItemWithBinaryPropertiesAndAncestorNames> GetFileListingWithBinaryPropertiesAndAncestorNames();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.CrawlConfigListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ICrawlConfigurationListItem> GetCrawlConfigListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.CrawlConfigReport" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ICrawlConfigReportItem> GetCrawlConfigReport();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.CrawlJobListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ICrawlJobListItem> GetCrawlJobListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SummaryPropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<ISummaryPropertiesListItem> GetSummaryPropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.DocumentPropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IDocumentPropertiesListItem> GetDocumentPropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.AudioPropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IAudioPropertiesListItem> GetAudioPropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.DRMPropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IDRMPropertiesListItem> GetDRMPropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.GPSPropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IGPSPropertiesListItem> GetGPSPropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.ImagePropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IImagePropertiesListItem> GetImagePropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.MediaPropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IMediaPropertiesListItem> GetMediaPropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.MusicPropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IMusicPropertiesListItem> GetMusicPropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.PhotoPropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IPhotoPropertiesListItem> GetPhotoPropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.RecordedTVPropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IRecordedTVPropertiesListItem> GetRecordedTVPropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.VideoPropertiesListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IVideoPropertiesListItem> GetVideoPropertiesListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.PersonalVolumeTagListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IItemTagListItem> GetPersonalVolumeTagListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SharedVolumeTagListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IItemTagListItem> GetSharedVolumeTagListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.PersonalSubdirectoryTagListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IItemTagListItem> GetPersonalSubdirectoryTagListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SharedSubdirectoryTagListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IItemTagListItem> GetSharedSubdirectoryTagListing();

        /// <summary>
        /// Gets a generic enumeration of  entities.
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.PersonalFileTagListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IItemTagListItem> GetPersonalFileTagListing();

        /// <summary>
        /// Get a generic representation of
        /// </summary>
        /// <returns>The concrete <see cref="IDbContext.SharedFileTagListing" /> as a generic <see cref="IEnumerable{T}" />.</returns>
        protected abstract IEnumerable<IItemTagListItem> GetSharedFileTagListing();

        /// <summary>
        /// Asynchronously finds a <see cref="IGPSPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IGPSProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="IGPSPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        protected abstract Task<IGPSPropertySet> FindGenericMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="IImagePropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IImageProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="IImagePropertySet" /> or <see langword="null" /> if not match was found.</returns>
        protected abstract Task<IImagePropertySet> FindGenericMatchingAsync(IImageProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="IMediaPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IMediaProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="IMediaPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        protected abstract Task<IMediaPropertySet> FindGenericMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="IMusicPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IMusicProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="IMusicPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        protected abstract Task<IMusicPropertySet> FindGenericMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="IPhotoPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IPhotoProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="IPhotoPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        protected abstract Task<IPhotoPropertySet> FindGenericMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="IRecordedTVPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IRecordedTVProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="IRecordedTVPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        protected abstract Task<IRecordedTVPropertySet> FindGenericMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="IVideoPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IVideoProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="IVideoPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        protected abstract Task<IVideoPropertySet> FindGenericMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="ISummaryPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="ISummaryProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="ISummaryPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        protected abstract Task<ISummaryPropertySet> FindGenericMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="IDocumentPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IDocumentProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="IDocumentPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        protected abstract Task<IDocumentPropertySet> FindGenericMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="IAudioPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IAudioProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="IAudioPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        protected abstract Task<IAudioPropertySet> FindGenericMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="IDRMPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IDRMProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="IDRMPropertySet" /> or <see langword="null" /> if not match was found.</returns>
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
                : base(Hosting.ServiceProvider)
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
                    service = Hosting.ServiceProvider.GetService(serviceType);
                    return service is not null;
                }
                return true;
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public class Interceptor : DbCommandInterceptor
        {
            private ILogger<BaseDbContext> _logger;

            public ILogger<BaseDbContext> Logger
            {
                get
                {
                    if (_logger is null)
                        _logger = Hosting.ServiceProvider.GetRequiredService<ILogger<BaseDbContext>>();
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
