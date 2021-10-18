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
    public class DeleteCrawlConfigurationBackgroundJob : IAsyncResult, IProgress<DbOperationService.WorkItem<bool>>
    {
        private readonly ILogger<DeleteCrawlConfigurationBackgroundWorker> _logger;
        private readonly IProgress<string> _onReportProgress;

        public bool DoNotUseTransaction { get; }

        public Task<bool> Task { get; }

        private DbOperationService.WorkItem<bool> _workItem;

        public DateTime Started { get; private set; }

        public ICrawlConfigurationRow Target { get; }

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

        public DeleteCrawlConfigurationBackgroundJob(ILogger<DeleteCrawlConfigurationBackgroundWorker> logger, DbOperationService dbOperationService, ICrawlConfigurationRow target, IProgress<string> onReportProgress, bool doNotUseTransaction)
        {
            _logger = logger;
            Target = target;
            _onReportProgress = onReportProgress;
            DoNotUseTransaction = doNotUseTransaction;
            Task = GetResult(dbOperationService.EnqueueAsync(async (LocalDbContext dbContext, CancellationToken cancellationToken) =>
            {
                if (target is not CrawlConfiguration crawlConfig)
                {
                    Guid id = target.Id;
                    if ((crawlConfig = await dbContext.CrawlConfigurations.FindAsync(new object[] { target.Id }, cancellationToken)) is null)
                        return false;
                }
                if (doNotUseTransaction)
                    return await DoWorkAsync(crawlConfig, dbContext, cancellationToken);
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                bool result = await DoWorkAsync(crawlConfig, dbContext, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }, this));
        }

        private async Task<bool> DoWorkAsync(CrawlConfiguration crawlConfig, LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing {CrawlConfiguration}", crawlConfig);
            EntityEntry<CrawlConfiguration> entry = dbContext.Entry(crawlConfig);
            switch (entry.State)
            {
                case EntityState.Detached:
                case EntityState.Deleted:
                    return false;
            }

            if (await entry.GetRelatedReferenceAsync(c => c.Root, cancellationToken) is not null)
                throw new InvalidOperationException();
            CrawlJobLog[] crawlJobLogs = (await entry.GetRelatedCollectionAsync(s => s.Logs, cancellationToken)).ToArray();
            if (crawlJobLogs.Length > 0)
            {
                dbContext.CrawlJobLogs.RemoveRange(crawlJobLogs);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            dbContext.CrawlConfigurations.Remove(crawlConfig);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
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
