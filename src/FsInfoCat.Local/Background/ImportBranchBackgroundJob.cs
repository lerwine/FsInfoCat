using FsInfoCat.Services;
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
    public class ImportBranchBackgroundJob : IAsyncResult, IProgress<IQueuedBgOperation<Subdirectory>>
    {
        private readonly ILogger<ImportBranchBackgroundWorker> _logger;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        private readonly IProgress<string> _onReportProgress;
        private readonly IQueuedBgOperation<Subdirectory> _workItem;

        public bool MarkNewAsCompleted { get; }

        public DirectoryInfo Source { get; }

        internal Task<Subdirectory> Task => _workItem.Task;

        object IAsyncResult.AsyncState => ((IAsyncOperationInfo)_workItem).AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => _workItem.AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => _workItem.CompletedSynchronously;

        bool IAsyncResult.IsCompleted => _workItem.IsCompleted;

        internal ImportBranchBackgroundJob([DisallowNull] ILogger<ImportBranchBackgroundWorker> logger, [DisallowNull] IFileSystemDetailService fileSystemDetailService, [DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] DirectoryInfo source, bool markNewAsCompleted, IProgress<string> onReportProgress)
        {
            _fileSystemDetailService = fileSystemDetailService;
            _logger = logger;
            MarkNewAsCompleted = markNewAsCompleted;
            Source = source;
            _onReportProgress = onReportProgress;
            _workItem = fsIOQueueService.Enqueue(ActivityCode.ImportingSubdirectory, async cancellationToken =>
            {
                if (!source.Exists)
                    throw new DirectoryNotFoundException();
                using IServiceScope serviceScope = Hosting.CreateScope();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                return (await DoWorkAsync(source, MarkNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete, dbContext, cancellationToken))?.Entity;
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

        void IProgress<IQueuedBgOperation<Subdirectory>>.Report(IQueuedBgOperation<Subdirectory> value)
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
