using FsInfoCat.AsyncOps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class ImportBranchBackgroundJob : IAsyncResult, IProgress<IJobResult<Subdirectory>>
    {
        private readonly ILogger<ImportBranchBackgroundWorker> _logger;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        private readonly IProgress<string> _onReportProgress;
        private readonly IJobResult<Subdirectory> _workItem;

        public Task<Subdirectory> Task => _workItem.GetTask();

        public DateTime Started { get; private set; }

        public AsyncJobStatus JobStatus { get; private set; }

        public TimeSpan Elapsed => _workItem?.Elapsed ?? TimeSpan.Zero;

        public bool MarkNewAsCompleted { get; }

        public DirectoryInfo Source { get; }

        public IJobResult<bool> WorkItem { get; }

        public object AsyncState => ((IAsyncResult)Task).AsyncState;

        public WaitHandle AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;

        public bool CompletedSynchronously => ((IAsyncResult)Task).CompletedSynchronously;

        public bool IsCompleted => ((IAsyncResult)Task).IsCompleted;

        internal ImportBranchBackgroundJob([DisallowNull] ILogger<ImportBranchBackgroundWorker> logger, IFileSystemDetailService fileSystemDetailService, [DisallowNull] JobQueue jobQueueService, [DisallowNull] DirectoryInfo source, bool markNewAsCompleted, IProgress<string> onReportProgress)
        {
            _fileSystemDetailService = fileSystemDetailService;
            _logger = logger;
            MarkNewAsCompleted = markNewAsCompleted;
            Source = source;
            _onReportProgress = onReportProgress;
            _workItem = jobQueueService.Enqueue(async context =>
            {
                Started = context.Started;
                if (!source.Exists)
                    throw new DirectoryNotFoundException();
                using IServiceScope serviceScope = Hosting.CreateScope();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                return (await DoWorkAsync(source, MarkNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete, dbContext, context.CancellationToken))?.Entity;
            });
        }

        private async Task<EntityEntry<Subdirectory>> DoWorkAsync(DirectoryInfo directoryInfo, DirectoryStatus defaultStatus, LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            string name = directoryInfo.Name;
            Subdirectory subdirectory;
            if (directoryInfo.Parent is null)
            {
                _onReportProgress?.Report($"Importing {directoryInfo.FullName}.");
                EntityEntry<Volume> volume = await Volume.ImportVolumeAsync(directoryInfo, _fileSystemDetailService, dbContext, cancellationToken);
                if (volume.State == EntityState.Added)
                    await dbContext.SaveChangesAsync(cancellationToken);
                Guid id = volume.Entity.Id;
                if ((subdirectory = await dbContext.Subdirectories.FirstOrDefaultAsync(s => s.Name == name && s.VolumeId == id, cancellationToken)) is null)
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
                if ((subdirectory = await dbContext.Subdirectories.FirstOrDefaultAsync(s => s.Name == name && s.VolumeId == id, cancellationToken)) is null)
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

        void IProgress<IJobResult<Subdirectory>>.Report(IJobResult<Subdirectory> value)
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
