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
    public class DeleteVolumeBackgroundJob : IAsyncResult, IProgress<DbOperationService.WorkItem<bool>>
    {
        private readonly ILogger<DeleteVolumeBackgroundWorker> _logger;
        private readonly IProgress<string> _onReportProgress;

        public bool DoNotUseTransaction { get; }

        public Task<bool> Task { get; }

        private DbOperationService.WorkItem<bool> _workItem;

        public DateTime Started { get; private set; }

        public IVolumeRow Target { get; }

        public bool ForceDelete { get; }

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

        public DeleteVolumeBackgroundJob(ILogger<DeleteVolumeBackgroundWorker> logger, DbOperationService dbOperationService, DeleteBranchBackgroundWorker deleteBranchService, IVolumeRow target, bool forceDelete, IProgress<string> onReportProgress, bool doNotUseTransaction)
        {
            _logger = logger;
            Target = target;
            ForceDelete = forceDelete;
            _onReportProgress = onReportProgress;
            DoNotUseTransaction = doNotUseTransaction;
            Task = GetResult(dbOperationService.EnqueueAsync(async (LocalDbContext dbContext, CancellationToken cancellationToken) =>
            {
                if (target is not Volume volume)
                {
                    Guid id = target.Id;
                    if ((volume = await dbContext.Volumes.FindAsync(new object[] { cancellationToken }, cancellationToken)) is null)
                        return false;
                }
                if (doNotUseTransaction)
                    return await DoWorkAsync(volume, dbContext, deleteBranchService, cancellationToken);
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                bool result = await DoWorkAsync(volume, dbContext, deleteBranchService, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }, this));
        }

        private async Task<bool> DoWorkAsync(Volume volume, LocalDbContext dbContext, DeleteBranchBackgroundWorker deleteBranchService, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting {Volume}", volume);
            EntityEntry<Volume> entry = dbContext.Entry(volume);
            switch (entry.State)
            {
                case EntityState.Detached:
                case EntityState.Deleted:
                    return false;
            }
            Subdirectory root = await entry.GetRelatedReferenceAsync(v => v.RootDirectory, cancellationToken);
            bool rootDeleted = root is null || await deleteBranchService.EnqueueAsync(root, true,  false, _onReportProgress, true).Task;
            VolumeAccessError[] accessErrors = (await entry.GetRelatedCollectionAsync(v => v.AccessErrors, cancellationToken)).ToArray();
            if (accessErrors.Length > 0)
                dbContext.VolumeAccessErrors.RemoveRange(accessErrors);
            PersonalVolumeTag[] personalTags = (await entry.GetRelatedCollectionAsync(v => v.PersonalTags, cancellationToken)).ToArray();
            if (personalTags.Length > 0)
                dbContext.PersonalVolumeTags.RemoveRange(personalTags);
            SharedVolumeTag[] sharedTags = (await entry.GetRelatedCollectionAsync(v => v.SharedTags, cancellationToken)).ToArray();
            if (sharedTags.Length > 0)
                dbContext.SharedVolumeTags.RemoveRange(sharedTags);
            if (ForceDelete || (rootDeleted && !volume.UpstreamId.HasValue))
            {
                if (dbContext.ChangeTracker.HasChanges())
                    await dbContext.SaveChangesAsync(cancellationToken);
                dbContext.Volumes.Remove(volume);
                return true;
            }
            else if (rootDeleted && !volume.UpstreamId.HasValue)
            {
                if (dbContext.ChangeTracker.HasChanges())
                    await dbContext.SaveChangesAsync(cancellationToken);
                dbContext.Volumes.Remove(volume);
            }
            if (volume.Status != VolumeStatus.Deleted)
                volume.Status = VolumeStatus.Deleted;
            else if (!dbContext.ChangeTracker.HasChanges())
                return false;
            await dbContext.SaveChangesAsync(cancellationToken);
            return false;
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
