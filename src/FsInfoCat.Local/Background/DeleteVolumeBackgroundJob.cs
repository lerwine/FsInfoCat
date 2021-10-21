using FsInfoCat.AsyncOps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class DeleteVolumeBackgroundJob : IAsyncResult, IProgress<IJobResult<bool>>
    {
        private readonly ILogger<DeleteVolumeBackgroundWorker> _logger;
        private readonly IProgress<string> _onReportProgress;
        private readonly IJobResult<bool> _workItem;

        public bool DoNotUseTransaction { get; }

        public Task<bool> Task => _workItem.GetTask();

        public DateTime Started { get; private set; }

        public IVolumeRow Target { get; }

        public bool ForceDelete { get; }

        public object AsyncState => ((IAsyncResult)Task).AsyncState;

        public WaitHandle AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;

        public bool CompletedSynchronously => ((IAsyncResult)Task).CompletedSynchronously;

        public bool IsCompleted => ((IAsyncResult)Task).IsCompleted;

        public AsyncJobStatus JobStatus { get; private set; }

        public DeleteVolumeBackgroundJob(ILogger<DeleteVolumeBackgroundWorker> logger, JobQueue jobQueueService, DeleteBranchBackgroundWorker deleteBranchService, IVolumeRow target, bool forceDelete, IProgress<string> onReportProgress, bool doNotUseTransaction)
        {
            _logger = logger;
            Target = target;
            ForceDelete = forceDelete;
            _onReportProgress = onReportProgress;
            DoNotUseTransaction = doNotUseTransaction;
            _workItem = jobQueueService.Enqueue(async context =>
            {
                Started = context.Started;
                return await DoWorkAsync(deleteBranchService, target, doNotUseTransaction, context.CancellationToken);
            });
        }

        private async Task<bool> DoWorkAsync(DeleteBranchBackgroundWorker deleteBranchService, IVolumeRow target, bool doNotUseTransaction, CancellationToken cancellationToken)
        {
            using IServiceScope serviceScope = Services.CreateScope();
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

        void IProgress<IJobResult<bool>>.Report(IJobResult<bool> value)
        {
            JobStatus = value.Status;
            switch (JobStatus)
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
