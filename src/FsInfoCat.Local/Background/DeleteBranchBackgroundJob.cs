using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class DeleteBranchBackgroundJob : IAsyncResult, IProgress<DbOperationService.WorkItem<bool>>
    {
        private readonly ILogger<DeleteBranchBackgroundWorker> _logger;
        private readonly IProgress<string> _onReportProgress;
        private readonly DeleteFileBackgroundWorker _deleteFileService;
        private readonly DeleteCrawlConfigurationBackgroundWorker _deleteCrawlConfigurationService;

        public bool DoNotUseTransaction { get; }

        public Task<bool> Task { get; }

        private DbOperationService.WorkItem<bool> _workItem;

        public DateTime Started { get; private set; }

        public ISubdirectoryRow Target { get; }

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

        public DeleteBranchBackgroundJob([DisallowNull] ILogger<DeleteBranchBackgroundWorker> logger, [DisallowNull] DbOperationService dbOperationService, [DisallowNull] DeleteFileBackgroundWorker deleteFileService, [DisallowNull] DeleteCrawlConfigurationBackgroundWorker deleteCrawlConfigurationService,
            ISubdirectoryRow target, bool forceDelete, bool deleteEmptyParent, IProgress<string> onReportProgress, bool doNotUseTransaction)
        {
            _logger = logger;
            Target = target;
            ForceDelete = forceDelete;
            DeleteEmptyParent = deleteEmptyParent;
            _onReportProgress = onReportProgress;
            DoNotUseTransaction = doNotUseTransaction;
            _deleteFileService = deleteFileService;
            _deleteCrawlConfigurationService = deleteCrawlConfigurationService;
            Task = GetResult(dbOperationService.EnqueueAsync(async (LocalDbContext dbContext, CancellationToken cancellationToken) =>
            {
                if (target is not Subdirectory subdirectory)
                {
                    Guid id = target.Id;
                    if ((subdirectory = await dbContext.Subdirectories.FindAsync(new object[] { target.Id }, cancellationToken)) is null)
                        return false;
                }
                if (doNotUseTransaction)
                    return await DoWorkAsync(target.Name, subdirectory, dbContext, cancellationToken);
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                bool result = await DoWorkAsync(target.Name, subdirectory, dbContext, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }, this));
        }

        private async Task<bool> DoWorkAsync(string relativePath, Subdirectory target, LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing {RelativePath}", relativePath);
            EntityEntry<Subdirectory> entry = dbContext.Entry(target);
            switch (entry.State)
            {
                case EntityState.Detached:
                case EntityState.Deleted:
                    return false;
            }

            SubdirectoryAccessError[] accessErrors = (await entry.GetRelatedCollectionAsync(s => s.AccessErrors, cancellationToken)).ToArray();
            if (accessErrors.Length > 0)
                dbContext.SubdirectoryAccessErrors.RemoveRange(accessErrors);
            PersonalSubdirectoryTag[] personalTags = (await entry.GetRelatedCollectionAsync(s => s.PersonalTags, cancellationToken)).ToArray();
            if (personalTags.Length > 0)
                dbContext.PersonalSubdirectoryTags.RemoveRange(personalTags);
            SharedSubdirectoryTag[] sharedTags = (await entry.GetRelatedCollectionAsync(s => s.SharedTags, cancellationToken)).ToArray();
            if (sharedTags.Length > 0)
                dbContext.SharedSubdirectoryTags.RemoveRange(sharedTags);
            bool result = true;
            DbFile[] files = (await entry.GetRelatedCollectionAsync(s => s.Files, cancellationToken)).ToArray();
            if (files.Length > 0)
                foreach (DbFile f in files)
                {
                    DeleteFileBackgroundJob deleteFileJob = _deleteFileService.EnqueueAsync(f, ForceDelete, false, _onReportProgress, true);
                    if (!(await deleteFileJob.Task))
                        result = false;
                }
            Subdirectory[] subdirectories = (await entry.GetRelatedCollectionAsync(s => s.SubDirectories, cancellationToken)).ToArray();
            if (subdirectories.Length > 0)
                foreach (Subdirectory subdirectory in subdirectories)
                {
                    if (!(await DoWorkAsync(Path.Combine(relativePath, subdirectory.Name), subdirectory, dbContext, cancellationToken)))
                        result = false;
                }
            CrawlConfiguration crawlConfiguration = await entry.GetRelatedReferenceAsync(s => s.CrawlConfiguration, cancellationToken);
            if (crawlConfiguration is not null)
            {
                DeleteCrawlConfigurationBackgroundJob deleteCrawlConfigurationJob = _deleteCrawlConfigurationService.EnqueueAsync(crawlConfiguration, _onReportProgress, true);
                if (!(await deleteCrawlConfigurationJob.Task))
                    result = false;
            }

            if (DeleteEmptyParent)
            {
                Guid id = target.Id;
                EntityEntry<Subdirectory> parent = await entry.GetRelatedTargetEntryAsync(s => s.Parent, cancellationToken);
                if ((await parent.GetRelatedCollectionAsync(s => s.Files, cancellationToken)).Any() || (await parent.GetRelatedCollectionAsync(s => s.SubDirectories, cancellationToken)).Any(s => s.Id != id))
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
