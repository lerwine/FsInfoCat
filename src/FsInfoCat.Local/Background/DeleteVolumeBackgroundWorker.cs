using FsInfoCat.Background;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteVolumeBackgroundWorker
    {
        private readonly ILogger<DeleteVolumeBackgroundWorker> _logger;
        private readonly DbOperationService _dbOperationService;
        private readonly DeleteBranchBackgroundWorker _deleteBranchService;

        [ServiceBuilderHandler()]
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddSingleton<DeleteVolumeBackgroundWorker>();
        }

        public DeleteVolumeBackgroundWorker([DisallowNull] ILogger<DeleteVolumeBackgroundWorker> logger, [DisallowNull] DbOperationService dbOperationService, [DisallowNull] DeleteBranchBackgroundWorker deleteBranchService)
        {
            _logger = logger;
            _dbOperationService = dbOperationService;
            _deleteBranchService = deleteBranchService;
        }

        public DeleteVolumeBackgroundJob EnqueueAsync(Volume target, bool forceDelete = false, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            return new DeleteVolumeBackgroundJob(_logger, _dbOperationService, _deleteBranchService, target, forceDelete, onReportProgress, doNotUseTransaction);
        }
    }
}
