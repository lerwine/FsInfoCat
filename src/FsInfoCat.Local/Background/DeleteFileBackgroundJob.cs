using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class DeleteFileBackgroundJob : IAsyncResult, IProgress<DbOperationService.WorkItem<bool>>
    {
        private readonly ILogger<DeleteFileBackgroundWorker> _logger;
        private readonly IProgress<string> _onReportProgress;

        public bool DoNotUseTransaction { get; }

        public Task<bool> Task { get; }

        private DbOperationService.WorkItem<bool> _workItem;

        public DateTime Started { get; private set; }

        public IFileRow Target { get; }

        public bool ForceDelete { get; }
        public bool DeleteEmptyParent { get; }

        public object AsyncState => ((IAsyncResult)Task).AsyncState;

        public WaitHandle AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;

        public bool CompletedSynchronously => ((IAsyncResult)Task).CompletedSynchronously;

        public bool IsCompleted => ((IAsyncResult)Task).IsCompleted;

        public AsyncJobStatus JobStatus { get; private set; }

        private async Task<bool> GetResult(Task<DbOperationService.WorkItem<bool>> task)
        {
            _workItem = await task;
            Started = _workItem.Started;
            return await _workItem.Task;
        }

        public DeleteFileBackgroundJob(ILogger<DeleteFileBackgroundWorker> logger, DbOperationService dbOperationService, IFileRow target, bool forceDelete, bool deleteEmptyParent, IProgress<string> onReportProgress, bool doNotUseTransaction)
        {
            _logger = logger;
            Target = target;
            ForceDelete = forceDelete;
            DeleteEmptyParent = deleteEmptyParent;
            _onReportProgress = onReportProgress;
            DoNotUseTransaction = doNotUseTransaction;
            Task = GetResult(dbOperationService.EnqueueAsync(async (LocalDbContext dbContext, CancellationToken cancellationToken) =>
            {
                if (target is not DbFile file)
                {
                    Guid id = target.Id;
                    if ((file = await dbContext.Files.FindAsync(new object[] { target.Id }, cancellationToken)) is null)
                        return false;
                }
                if (doNotUseTransaction)
                    return await DoWorkAsync(target.Name, file, dbContext, cancellationToken);
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                bool result = await DoWorkAsync(target.Name, file, dbContext, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }, this));
        }

        private async Task<bool> DoWorkAsync(string relativePath, DbFile file, LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing {RelativePath}", relativePath);
            EntityEntry<DbFile> entry = dbContext.Entry(file);
            switch (entry.State)
            {
                case EntityState.Detached:
                case EntityState.Deleted:
                    return false;
            }

            Guid id = file.Id;
            FileAccessError[] fileAccessErrors = (await entry.GetRelatedCollectionAsync(s => s.AccessErrors, cancellationToken)).ToArray();
            if (fileAccessErrors.Length > 0)
                dbContext.FileAccessErrors.RemoveRange(fileAccessErrors);
            PersonalFileTag[] personalFileTags = (await entry.GetRelatedCollectionAsync(s => s.PersonalTags, cancellationToken)).ToArray();
            if (personalFileTags.Length > 0)
                dbContext.PersonalFileTags.RemoveRange(personalFileTags);
            SharedFileTag[] sharedFileTags = (await entry.GetRelatedCollectionAsync(s => s.SharedTags, cancellationToken)).ToArray();
            if (sharedFileTags.Length > 0)
                dbContext.SharedFileTags.RemoveRange(sharedFileTags);
            FileComparison[] fileComparisons = (await entry.GetRelatedCollectionAsync(s => s.BaselineComparisons, cancellationToken)).Concat(await entry.GetRelatedCollectionAsync(s => s.CorrelativeComparisons, cancellationToken)).ToArray();
            if (fileComparisons.Length > 0)
                dbContext.Comparisons.RemoveRange(fileComparisons);
            EntityEntry<BinaryPropertySet> binaryProperties = await entry.GetRelatedTargetEntryAsync(s => s.BinaryProperties, cancellationToken);
            if (!(await binaryProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.BinaryPropertySets.Remove(binaryProperties.Entity);
            EntityEntry<AudioPropertySet> audioProperties = await entry.GetRelatedTargetEntryAsync(s => s.AudioProperties, cancellationToken);
            if (!(await audioProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.AudioPropertySets.Remove(audioProperties.Entity);
            EntityEntry<DocumentPropertySet> documentProperties = await entry.GetRelatedTargetEntryAsync(s => s.DocumentProperties, cancellationToken);
            if (!(await documentProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.DocumentPropertySets.Remove(documentProperties.Entity);
            EntityEntry<DRMPropertySet> drmProperties = await entry.GetRelatedTargetEntryAsync(s => s.DRMProperties, cancellationToken);
            if (!(await drmProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.DRMPropertySets.Remove(drmProperties.Entity);
            EntityEntry<GPSPropertySet> gpsProperties = await entry.GetRelatedTargetEntryAsync(s => s.GPSProperties, cancellationToken);
            if (!(await gpsProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.GPSPropertySets.Remove(gpsProperties.Entity);
            EntityEntry<ImagePropertySet> imageProperties = await entry.GetRelatedTargetEntryAsync(s => s.ImageProperties, cancellationToken);
            if (!(await imageProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.ImagePropertySets.Remove(imageProperties.Entity);
            EntityEntry<MediaPropertySet> mediaProperties = await entry.GetRelatedTargetEntryAsync(s => s.MediaProperties, cancellationToken);
            if (!(await mediaProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.MediaPropertySets.Remove(mediaProperties.Entity);
            EntityEntry<MusicPropertySet> musicProperties = await entry.GetRelatedTargetEntryAsync(s => s.MusicProperties, cancellationToken);
            if (!(await musicProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.MusicPropertySets.Remove(musicProperties.Entity);
            EntityEntry<PhotoPropertySet> photoProperties = await entry.GetRelatedTargetEntryAsync(s => s.PhotoProperties, cancellationToken);
            if (!(await photoProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.PhotoPropertySets.Remove(photoProperties.Entity);
            EntityEntry<RecordedTVPropertySet> recordedTVProperties = await entry.GetRelatedTargetEntryAsync(s => s.RecordedTVProperties, cancellationToken);
            if (!(await recordedTVProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.RecordedTVPropertySets.Remove(recordedTVProperties.Entity);
            EntityEntry<SummaryPropertySet> summaryProperties = await entry.GetRelatedTargetEntryAsync(s => s.SummaryProperties, cancellationToken);
            if (!(await summaryProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.SummaryPropertySets.Remove(summaryProperties.Entity);
            EntityEntry<VideoPropertySet> videoProperties = await entry.GetRelatedTargetEntryAsync(s => s.VideoProperties, cancellationToken);
            if (!(await videoProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                dbContext.VideoPropertySets.Remove(videoProperties.Entity);

            bool result = ForceDelete || !file.UpstreamId.HasValue;
            if (result)
            {
                if (dbContext.ChangeTracker.HasChanges())
                    await dbContext.SaveChangesAsync(cancellationToken);
                dbContext.Files.Remove(file);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            else if (file.Status != FileCorrelationStatus.Deleted)
            {
                file.Status = FileCorrelationStatus.Deleted;
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            
            if (DeleteEmptyParent)
            {
                id = file.Id;
                EntityEntry<Subdirectory> parent = await entry.GetRelatedTargetEntryAsync(s => s.Parent, cancellationToken);
                if ((await parent.GetRelatedCollectionAsync(s => s.Files, cancellationToken)).Any(f => f.Id != id) || (await parent.GetRelatedCollectionAsync(s => s.SubDirectories, cancellationToken)).Any())
                    return result;
                bool allDeleted = result;
                Volume volume = null;
                do
                {
                    Subdirectory subdirectory = parent.Entity;
                    id = subdirectory.Id;
                    if ((parent = await parent.GetRelatedTargetEntryAsync(s => s.Parent, cancellationToken)) is null)
                        volume = await parent.GetRelatedReferenceAsync(s => s.Volume, cancellationToken);
                    else if ((await parent.GetRelatedCollectionAsync(s => s.Files, cancellationToken)).Any() || (await parent.GetRelatedCollectionAsync(s => s.SubDirectories, cancellationToken)).Any(s => s.Id != id))
                    {
                        if (!dbContext.ChangeTracker.HasChanges())
                            await dbContext.SaveChangesAsync(cancellationToken);
                        return result;
                    }
                    if (allDeleted)
                    {
                        dbContext.Remove(subdirectory);
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                    else if (subdirectory.Status != DirectoryStatus.Deleted)
                        subdirectory.Status = DirectoryStatus.Deleted;
                }
                while (parent is not null);
                if (volume is not null)
                {
                    EntityEntry<Volume> volEntry = dbContext.Entry(volume);
                    VolumeAccessError[] volAccessErrors = (await volEntry.GetRelatedCollectionAsync(v => v.AccessErrors, cancellationToken)).ToArray();
                    if (volAccessErrors.Length > 0)
                        dbContext.VolumeAccessErrors.RemoveRange(volAccessErrors);
                    PersonalVolumeTag[] volPersonalTags = (await volEntry.GetRelatedCollectionAsync(v => v.PersonalTags, cancellationToken)).ToArray();
                    if (volPersonalTags.Length > 0)
                        dbContext.PersonalVolumeTags.RemoveRange(volPersonalTags);
                    SharedVolumeTag[] volSharedTags = (await volEntry.GetRelatedCollectionAsync(v => v.SharedTags, cancellationToken)).ToArray();
                    if (volSharedTags.Length > 0)
                        dbContext.SharedVolumeTags.RemoveRange(volSharedTags);
                    if (allDeleted && !volume.UpstreamId.HasValue)
                    {
                        if (dbContext.ChangeTracker.HasChanges())
                            await dbContext.SaveChangesAsync(cancellationToken);
                        dbContext.Volumes.Remove(volume);
                    }
                    else if (volume.Status != VolumeStatus.Deleted)
                        volume.Status = VolumeStatus.Deleted;
                    else if (!dbContext.ChangeTracker.HasChanges())
                        return result;
                }
                else if (!dbContext.ChangeTracker.HasChanges())
                    return result;
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            return result;
        }

        void IProgress<DbOperationService.WorkItem<bool>>.Report(DbOperationService.WorkItem<bool> value)
        {
            JobStatus = value.JobStatus;
            switch (value.JobStatus)
            {
                case AsyncJobStatus.Cancelling:
                    _onReportProgress?.Report("Cancelling background job...");
                    break;
                case AsyncJobStatus.Canceled:
                    _onReportProgress?.Report("Background job canceled.");
                    break;
                case AsyncJobStatus.Faulted:
                    _onReportProgress?.Report("Background job failed.");
                    break;
                case AsyncJobStatus.Succeeded:
                    _onReportProgress?.Report("Background job completed.");
                    break;
            }
        }
    }
}
