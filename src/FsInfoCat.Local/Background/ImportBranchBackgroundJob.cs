using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    // TODO: Use FsInfoCat.AsyncOps.IJobResult<Subdirectory> instead of FsInfoCat.Local.Background.DbOperationService.WorkItem<Subdirectory> #105
    public class ImportBranchBackgroundJob : IAsyncResult, IProgress<DbOperationService.WorkItem<Subdirectory>>
    {
        private readonly ILogger<ImportBranchBackgroundWorker> _logger;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        private readonly IProgress<string> _onReportProgress;
        private DbOperationService.WorkItem<Subdirectory> _workItem;

        public Task<Subdirectory> Task { get; }

        public DateTime Started { get; private set; }

        public AsyncJobStatus JobStatus { get; private set; }

        public TimeSpan Elapsed => _workItem?.Elapsed ?? TimeSpan.Zero;

        private async Task<Subdirectory> GetResult(Task<DbOperationService.WorkItem<Subdirectory>> task)
        {
            _workItem = await task;
            Started = _workItem.Started;
            return await _workItem.Task;
        }

        // TODO: Use FsInfoCat.AsyncOps.JobQueue instead of FsInfoCat.Local.Background.DbOperationService #105
        internal ImportBranchBackgroundJob([DisallowNull] ILogger<ImportBranchBackgroundWorker> logger, IFileSystemDetailService fileSystemDetailService, [DisallowNull] DbOperationService dbOperationService, [DisallowNull] DirectoryInfo source, bool markNewAsCompleted, IProgress<string> onReportProgress)
        {
            _fileSystemDetailService = fileSystemDetailService;
            _logger = logger;
            MarkNewAsCompleted = markNewAsCompleted;
            Source = source;
            _onReportProgress = onReportProgress;
            Task = GetResult(dbOperationService.EnqueueAsync(async (LocalDbContext dbContext, CancellationToken cancellationToken) => (await DoWorkAsync(source, MarkNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete, dbContext, cancellationToken)).Entity, this));
        }

        public bool MarkNewAsCompleted { get; }

        public DirectoryInfo Source { get; }

        public DbOperationService.WorkItem<Subdirectory> WorkItem { get; }

        public object AsyncState => ((IAsyncResult)Task).AsyncState;

        public WaitHandle AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;

        public bool CompletedSynchronously => ((IAsyncResult)Task).CompletedSynchronously;

        public bool IsCompleted => ((IAsyncResult)Task).IsCompleted;

        private async Task<EntityEntry<Subdirectory>> DoWorkAsync(DirectoryInfo directoryInfo, DirectoryStatus defaultStatus, LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException();
            string name = directoryInfo.Name;
            Subdirectory subdirectory;
            if (directoryInfo.Parent is null)
            {
                _onReportProgress?.Report($"Importing {directoryInfo.FullName}.");
                EntityEntry<Volume> volume = await Volume.ImportVolumeAsync(directoryInfo, _fileSystemDetailService, dbContext, cancellationToken);
                if (volume.State == EntityState.Added)
                    await dbContext.SaveChangesAsync(cancellationToken);
                Guid id = volume.Entity.Id;
                if ((subdirectory = await dbContext.Subdirectories.FirstOrDefaultAsync(s => s.Name == name && s.VolumeId == id)) is null)
                    return dbContext.Subdirectories.Add(new()
                    {
                        Name = name,
                        CreationTime = directoryInfo.CreationTime,
                        LastWriteTime = directoryInfo.LastWriteTime,
                        Status = defaultStatus,
                        Volume = volume.Entity
                    });
            }
            else
            {
                EntityEntry<Subdirectory> parent = await DoWorkAsync(directoryInfo.Parent, defaultStatus, dbContext, cancellationToken);
                if (parent.State == EntityState.Added)
                    await dbContext.SaveChangesAsync(cancellationToken);
                _onReportProgress?.Report($"Importing {directoryInfo.FullName}.");
                Guid id = parent.Entity.Id;
                if ((subdirectory = await dbContext.Subdirectories.FirstOrDefaultAsync(s => s.Name == name && s.VolumeId == id)) is null)
                    return dbContext.Subdirectories.Add(new()
                    {
                        Name = name,
                        CreationTime = directoryInfo.CreationTime,
                        LastWriteTime = directoryInfo.LastWriteTime,
                        Status = defaultStatus,
                        Parent = parent.Entity
                    });
            }
            return dbContext.Entry(subdirectory);
        }

        void IProgress<DbOperationService.WorkItem<Subdirectory>>.Report(DbOperationService.WorkItem<Subdirectory> value)
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
