using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    // TODO: Use FsInfoCat.AsyncOps.IJobResult<bool> instead of FsInfoCat.Local.Background.DbOperationService.WorkItem<bool> #105
    public class MarkBranchIncompleteBackgroundJob : IAsyncResult, IProgress<DbOperationService.WorkItem<bool>>
    {
        private readonly ILogger<MarkBranchIncompleteBackgroundWorker> _logger;
        private readonly IProgress<string> _onReportProgress;

        public bool DoNotUseTransaction { get; }

        public Task<bool> Task { get; }

        private DbOperationService.WorkItem<bool> _workItem;

        public DateTime Started { get; private set; }

        public Subdirectory Target { get; }

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

        // TODO: Use FsInfoCat.AsyncOps.JobQueue instead of FsInfoCat.Local.Background.DbOperationService #105
        public MarkBranchIncompleteBackgroundJob(ILogger<MarkBranchIncompleteBackgroundWorker> logger, DbOperationService dbOperationService, Subdirectory subdirectory, IProgress<string> onReportProgress, bool doNotUseTransaction)
        {
            _logger = logger;
            Target = subdirectory;
            _onReportProgress = onReportProgress;
            DoNotUseTransaction = doNotUseTransaction;
            Task = GetResult(dbOperationService.EnqueueAsync(async (LocalDbContext dbContext, CancellationToken cancellationToken) =>
            {
                if (doNotUseTransaction)
                    return await DoWorkAsync(subdirectory, dbContext, cancellationToken);
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                bool result = await DoWorkAsync(subdirectory, dbContext, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }, this));
        }

        private async Task<bool> DoWorkAsync(string relativePath, Subdirectory parent, LocalDbContext dbContext, CancellationToken cancellationToken)
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
                        string path = Path.Combine(relativePath, subdirectory.Name);
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

        private async Task<bool> DoWorkAsync(Subdirectory parent, LocalDbContext dbContext, CancellationToken cancellationToken)
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
