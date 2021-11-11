using FsInfoCat.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class MarkBranchIncompleteBackgroundJob : IAsyncResult, IProgress<IQueuedBgOperation<bool>>
    {
        private readonly ILogger<MarkBranchIncompleteBackgroundWorker> _logger;
        private readonly IProgress<string> _onReportProgress;
        private readonly IQueuedBgProducer<bool> _workItem;

        public bool DoNotUseTransaction { get; }

        public Subdirectory Target { get; }

        internal Task<bool> Task => _workItem.Task;

        object IAsyncResult.AsyncState => ((IAsyncOperationInfo)_workItem).AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => _workItem.AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => _workItem.CompletedSynchronously;

        bool IAsyncResult.IsCompleted => _workItem.IsCompleted;

        public MarkBranchIncompleteBackgroundJob([DisallowNull] ILogger<MarkBranchIncompleteBackgroundWorker> logger, [DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] Subdirectory subdirectory, IProgress<string> onReportProgress, bool doNotUseTransaction)
        {
            _logger = logger;
            Target = subdirectory;
            _onReportProgress = onReportProgress;
            DoNotUseTransaction = doNotUseTransaction;
            _workItem = fsIOQueueService.Enqueue(ActivityCode.SettingBranchIncomplete, async cancellationToken => await DoWorkAsync(subdirectory, doNotUseTransaction, cancellationToken));
        }

        private async Task<bool> DoWorkAsync([DisallowNull] Subdirectory subdirectory, bool doNotUseTransaction, CancellationToken cancellationToken)
        {
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            if (doNotUseTransaction)
                return await DoWorkAsync(subdirectory, dbContext, cancellationToken);
            using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            bool result = await DoWorkAsync(subdirectory, dbContext, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }

        private async Task<bool> DoWorkAsync([DisallowNull] string relativePath, [DisallowNull] Subdirectory parent, [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing {RelativePath}", relativePath);
            EntityEntry<Subdirectory> entry = dbContext.Entry(parent);
            bool result = false;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    DbFile[] dbFiles = await entry.QueryRelatedCollectionAsync(s => s.Files, f => f.Status == FileCorrelationStatus.Correlated || f.Status == FileCorrelationStatus.AccessError, cancellationToken);
                    if (dbFiles.Length > 0)
                    {
                        result = true;
                        foreach (DbFile file in dbFiles)
                        {
                            _logger.LogInformation("Processing file {RelativePath}", Path.Combine(relativePath, file.Name));
                            if (file.Status == FileCorrelationStatus.AccessError)
                                dbContext.FileAccessErrors.RemoveRange(await dbContext.Entry(file).GetRelatedCollectionAsync(f => f.AccessErrors, cancellationToken));
                            file.Status = FileCorrelationStatus.Dissociated;
                        }
                    }
                    foreach (Subdirectory subdirectory in (await entry.GetRelatedCollectionAsync(s => s.SubDirectories, cancellationToken)).ToArray())
                    {
                        if (await DoWorkAsync(Path.Combine(relativePath, subdirectory.Name), subdirectory, dbContext, cancellationToken))
                            result = true;
                    }
                    break;
            }
            switch (parent.Status)
            {
                case DirectoryStatus.Complete:
                    _onReportProgress?.Report($"Marking {relativePath} incomplete");
                    _logger.LogInformation("Marking {RelativePath} incomplete", relativePath);
                    parent.Status = DirectoryStatus.Incomplete;
                    return true;
                case DirectoryStatus.AccessError:
                    _onReportProgress?.Report($"Marking {relativePath} incomplete");
                    _logger.LogInformation("Marking {RelativePath} incomplete", relativePath);
                    dbContext.SubdirectoryAccessErrors.RemoveRange(await entry.GetRelatedCollectionAsync(s => s.AccessErrors, cancellationToken));
                    parent.Status = DirectoryStatus.Incomplete;
                    return true;
            }
            return result;
        }

        private async Task<bool> DoWorkAsync([DisallowNull] Subdirectory parent, [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            string name = parent.Name;
            _logger.LogInformation("Processing {RelativePath}", name);
            EntityEntry<Subdirectory> entry = dbContext.Entry(parent);
            bool result = false;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    DbFile[] dbFiles = await entry.QueryRelatedCollectionAsync(s => s.Files, f => f.Status == FileCorrelationStatus.Correlated, cancellationToken);
                    if (dbFiles.Length > 0)
                    {
                        result = true;
                        foreach (DbFile file in dbFiles)
                            file.Status = FileCorrelationStatus.Dissociated;
                    }
                    foreach (Subdirectory subdirectory in (await entry.GetRelatedCollectionAsync(s => s.SubDirectories, cancellationToken)).ToArray())
                    {
                        if (await DoWorkAsync(name, subdirectory, dbContext, cancellationToken))
                            result = true;
                    }
                    break;
            }
            switch (parent.Status)
            {
                case DirectoryStatus.Complete:
                    _onReportProgress?.Report($"Marking {name} incomplete");
                    _logger.LogInformation("Marking {RelativePath} incomplete", name);
                    parent.Status = DirectoryStatus.Incomplete;
                    result = true;
                    break;
                case DirectoryStatus.AccessError:
                    _onReportProgress?.Report($"Marking {name} incomplete");
                    _logger.LogInformation("Marking {RelativePath} incomplete", name);
                    dbContext.SubdirectoryAccessErrors.RemoveRange(await entry.GetRelatedCollectionAsync(s => s.AccessErrors, cancellationToken));
                    parent.Status = DirectoryStatus.Incomplete;
                    result = true;
                    break;
            }
            if (result)
            {
                _onReportProgress?.Report("Saving changes to database...");
                _logger.LogInformation("Saving changes to database");
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            return result;
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
