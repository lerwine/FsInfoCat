using FsInfoCat.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class DeleteVolumeBackgroundJob : IAsyncResult, IProgress<IQueuedBgOperation<bool>>
    {
        private readonly ILogger<DeleteVolumeBackgroundWorker> _logger;
        private readonly IProgress<string> _onReportProgress;
        private readonly IQueuedBgOperation<bool> _workItem;

        public bool DoNotUseTransaction { get; }

        public IVolumeRow Target { get; }

        public bool ForceDelete { get; }

        internal Task<bool> Task => _workItem.Task;

        object IAsyncResult.AsyncState => ((IAsyncOperationInfo)_workItem).AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => _workItem.AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => _workItem.CompletedSynchronously;

        bool IAsyncResult.IsCompleted => _workItem.IsCompleted;

        public DeleteVolumeBackgroundJob([DisallowNull] ILogger<DeleteVolumeBackgroundWorker> logger, [DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] DeleteBranchBackgroundWorker deleteBranchService, [DisallowNull] IVolumeRow target, bool forceDelete, IProgress<string> onReportProgress, bool doNotUseTransaction)
        {
            _logger = logger;
            Target = target;
            ForceDelete = forceDelete;
            _onReportProgress = onReportProgress;
            DoNotUseTransaction = doNotUseTransaction;
            _workItem = fsIOQueueService.Enqueue(ActivityCode.DeletingVolume, async cancellationToken => await DoWorkAsync(deleteBranchService, target, doNotUseTransaction, cancellationToken));
        }

        private async Task<bool> DoWorkAsync([DisallowNull] DeleteBranchBackgroundWorker deleteBranchService, [DisallowNull] IVolumeRow target, bool doNotUseTransaction, CancellationToken cancellationToken)
        {
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
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
        }

        private async Task<bool> DoWorkAsync([DisallowNull] Volume volume, [DisallowNull] LocalDbContext dbContext, [DisallowNull] DeleteBranchBackgroundWorker deleteBranchService, CancellationToken cancellationToken)
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
            bool rootDeleted = root is null || await deleteBranchService.EnqueueAsync(root, true, false, _onReportProgress, true).Task;
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

        void IProgress<IQueuedBgOperation<bool>>.Report(IQueuedBgOperation<bool> value)
        {
            switch (value.Status)
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
