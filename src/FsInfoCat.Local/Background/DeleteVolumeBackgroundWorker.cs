using FsInfoCat.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteVolumeBackgroundWorker
    {
        private readonly ILogger<DeleteVolumeBackgroundWorker> _logger;
        private readonly IFSIOQueueService _fsIOQueueService;
        private readonly DeleteBranchBackgroundWorker _deleteBranchService;

        [ServiceBuilderHandler(Priority = 400)]
        private static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DeleteVolumeBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<DeleteVolumeBackgroundWorker>();
        }

        public DeleteVolumeBackgroundWorker([DisallowNull] ILogger<DeleteVolumeBackgroundWorker> logger, [DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] DeleteBranchBackgroundWorker deleteBranchService)
        {
            _logger = logger;
            _fsIOQueueService = fsIOQueueService;
            _deleteBranchService = deleteBranchService;
            _logger.LogDebug($"{nameof(DeleteVolumeBackgroundWorker)} Service instantiated");
        }

        public DeleteVolumeBackgroundJob EnqueueAsync([DisallowNull] Volume target, bool forceDelete = false, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            return new DeleteVolumeBackgroundJob(_logger, _fsIOQueueService, _deleteBranchService, target, forceDelete, onReportProgress, doNotUseTransaction);
        }
    }
}
