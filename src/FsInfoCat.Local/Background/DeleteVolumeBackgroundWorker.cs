using FsInfoCat.AsyncOps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteVolumeBackgroundWorker
    {
        private readonly ILogger<DeleteVolumeBackgroundWorker> _logger;
        private readonly JobQueue _jobQueueService;
        private readonly DeleteBranchBackgroundWorker _deleteBranchService;

        [ServiceBuilderHandler(Priority = 400)]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DeleteVolumeBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<DeleteVolumeBackgroundWorker>();
        }

        public DeleteVolumeBackgroundWorker([DisallowNull] ILogger<DeleteVolumeBackgroundWorker> logger, [DisallowNull] JobQueue jobQueueService, [DisallowNull] DeleteBranchBackgroundWorker deleteBranchService)
        {
            _logger = logger;
            _jobQueueService = jobQueueService;
            _deleteBranchService = deleteBranchService;
            _logger.LogDebug($"{nameof(DeleteVolumeBackgroundWorker)} Service instantiated");
        }

        public DeleteVolumeBackgroundJob EnqueueAsync(Volume target, bool forceDelete = false, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            return new DeleteVolumeBackgroundJob(_logger, _jobQueueService, _deleteBranchService, target, forceDelete, onReportProgress, doNotUseTransaction);
        }
    }
}
