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
    public class DeleteCrawlConfigurationBackgroundJob : IAsyncResult, IProgress<IQueuedBgOperation<bool>>
    {
        private readonly ILogger<DeleteCrawlConfigurationBackgroundWorker> _logger;
        private readonly IProgress<string> _onReportProgress;
        private readonly IQueuedBgOperation<bool> _workItem;

        public bool DoNotUseTransaction { get; }

        public ICrawlConfigurationRow Target { get; }

        internal Task<bool> Task => _workItem.Task;

        object IAsyncResult.AsyncState => _workItem.AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => _workItem.AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => _workItem.CompletedSynchronously;

        bool IAsyncResult.IsCompleted => _workItem.IsCompleted;

        public DeleteCrawlConfigurationBackgroundJob([DisallowNull] ILogger<DeleteCrawlConfigurationBackgroundWorker> logger, [DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] ICrawlConfigurationRow target, IProgress<string> onReportProgress, bool doNotUseTransaction)
        {
            _logger = logger;
            Target = target;
            _onReportProgress = onReportProgress;
            DoNotUseTransaction = doNotUseTransaction;
            _workItem = fsIOQueueService.Enqueue(async cancellationToken => await DoWorkAsync(target, doNotUseTransaction, cancellationToken));
        }

        private async Task<bool> DoWorkAsync([DisallowNull] ICrawlConfigurationRow target, bool doNotUseTransaction, CancellationToken cancellationToken)
        {
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
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
        }

        private async Task<bool> DoWorkAsync([DisallowNull] CrawlConfiguration crawlConfig, [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken)
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
