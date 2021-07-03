using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public abstract class BaseDbContext : DbContext, IDbContext
    {
        private async Task<(object Entity, EntityState OldState)[]> RaiseBeforeSaveAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<(object Entity, EntityState OldState)> toSave = new();
            foreach (EntityEntry e in ChangeTracker.Entries())
            {
                object entity = e.Entity;
                ValidationContext validationContext = new(entity, new DbContextServiceProvider(this, e), null);
                switch (e.State)
                {
                    case EntityState.Added:
                        if (entity is IDbEntity adding)
                            OnBeforeAddEntity(adding, cancellationToken);
                        if (entity is IDbEntityBeforeSave beforeSave)
                            await beforeSave.BeforeSaveAsync(cancellationToken);
                        if (entity is IDbEntityBeforeInsert beforeInsert)
                            await beforeInsert.BeforeInsertAsync(cancellationToken);
                        toSave.Add((Entity: entity, OldState: EntityState.Added));
                        break;
                    case EntityState.Modified:
                        if (entity is IDbEntity saving)
                            OnBeforeSaveChanges(saving, cancellationToken);
                        if (entity is IDbEntityBeforeSave beforeSave2)
                            await beforeSave2.BeforeSaveAsync(cancellationToken);
                        if (entity is IDbEntityBeforeSaveChanges beforeSaveChanges)
                            await beforeSaveChanges.BeforeSaveChangesAsync(cancellationToken);
                        toSave.Add((Entity: entity, OldState: EntityState.Modified));
                        break;
                    case EntityState.Deleted:
                        if (entity is IDbEntityBeforeDelete beforeDelete)
                            await beforeDelete.BeforeDeleteAsync(cancellationToken);
                        toSave.Add((Entity: entity, OldState: EntityState.Deleted));
                        break;
                }
                cancellationToken.ThrowIfCancellationRequested();
                Validator.ValidateObject(entity, validationContext, true);
            }
            cancellationToken.ThrowIfCancellationRequested();
            return toSave.ToArray();
        }

        private async Task RaiseAfterSaveAsync((object Entity, EntityState OldState)[] saved, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach ((object Entity, EntityState OldState) e in saved)
            {
                object entity = e.Entity;
                switch (e.OldState)
                {
                    case EntityState.Added:
                        if (entity is IDbEntityAfterSave afterSave)
                            await afterSave.AfterSaveAsync(cancellationToken);
                        if (entity is IDbEntityAfterInsert afterInsert)
                            await afterInsert.AfterInsertAsync(cancellationToken);
                        break;
                    case EntityState.Modified:
                        if (entity is IDbEntityAfterSave afterSave2)
                            await afterSave2.AfterSaveAsync(cancellationToken);
                        if (entity is IDbEntityAfterSaveChanges afterSaveChanges)
                            await afterSaveChanges.AfterSaveChangesAsync(cancellationToken);
                        break;
                    case EntityState.Deleted:
                        if (entity is IDbEntityAfterDelete afterDelete)
                            await afterDelete.AfterDeleteAsync(cancellationToken);
                        break;
                }
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
            (object Entity, EntityState OldState)[] changes = RaiseBeforeSaveAsync().Result;
            int result = base.SaveChanges();
            RaiseAfterSaveAsync(changes).Wait();
            return result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            (object Entity, EntityState OldState)[] changes = RaiseBeforeSaveAsync().Result;
            int result = base.SaveChanges(acceptAllChangesOnSuccess);
            RaiseAfterSaveAsync(changes).Wait();
            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            (object Entity, EntityState OldState)[] changes = await RaiseBeforeSaveAsync(cancellationToken);
            int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await RaiseAfterSaveAsync(changes, cancellationToken);
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            (object Entity, EntityState OldState)[] changes = await RaiseBeforeSaveAsync(cancellationToken);
            int result = await base.SaveChangesAsync(cancellationToken);
            await RaiseAfterSaveAsync(changes, cancellationToken);
            return result;
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

        protected abstract IEnumerable<IAccessError<IFile>> GetGenericFileAccessErrors();

        protected abstract IEnumerable<IFile> GetGenericFiles();

        protected abstract IEnumerable<IFileSystem> GetGenericFileSystems();

        protected abstract IEnumerable<IRedundancy> GetGenericRedundancies();

        protected abstract IEnumerable<IRedundantSet> GetGenericRedundantSets();

        protected abstract IEnumerable<ISubdirectory> GetGenericSubdirectories();

        protected abstract IEnumerable<IAccessError<ISubdirectory>> GetGenericSubdirectoryAccessErrors();

        protected abstract IEnumerable<ISymbolicName> GetGenericSymbolicNames();

        protected abstract IEnumerable<IAccessError<IVolume>> GetGenericVolumeAccessErrors();

        protected abstract IEnumerable<IVolume> GetGenericVolumes();

        protected abstract IEnumerable<ICrawlConfiguration> GetGenericCrawlConfigurations();

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
        IEnumerable<IAccessError<IFile>> IDbContext.FileAccessErrors => GetGenericFileAccessErrors();
        IEnumerable<IFile> IDbContext.Files => GetGenericFiles();
        IEnumerable<IFileSystem> IDbContext.FileSystems => GetGenericFileSystems();
        IEnumerable<IRedundancy> IDbContext.Redundancies => GetGenericRedundancies();
        IEnumerable<IRedundantSet> IDbContext.RedundantSets => GetGenericRedundantSets();
        IEnumerable<ISubdirectory> IDbContext.Subdirectories => GetGenericSubdirectories();
        IEnumerable<IAccessError<ISubdirectory>> IDbContext.SubdirectoryAccessErrors => GetGenericSubdirectoryAccessErrors();
        IEnumerable<ISymbolicName> IDbContext.SymbolicNames => GetGenericSymbolicNames();
        IEnumerable<IAccessError<IVolume>> IDbContext.VolumeAccessErrors => GetGenericVolumeAccessErrors();
        IEnumerable<IVolume> IDbContext.Volumes => GetGenericVolumes();
        IEnumerable<ICrawlConfiguration> IDbContext.CrawlConfigurations => GetGenericCrawlConfigurations();

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
    }
}
