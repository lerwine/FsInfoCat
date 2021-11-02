using FsInfoCat.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteBranchBackgroundWorker
    {
        private readonly ILogger<DeleteBranchBackgroundWorker> _logger;
        private readonly IFSIOQueueService _fsIOQueueService;
        private readonly DeleteFileBackgroundWorker _deleteFileService;
        private readonly DeleteCrawlConfigurationBackgroundWorker _deleteCrawlConfigurationService;

        [ServiceBuilderHandler(Priority = 300)]
        private static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DeleteBranchBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<DeleteBranchBackgroundWorker>();
        }

        public DeleteBranchBackgroundWorker([DisallowNull] ILogger<DeleteBranchBackgroundWorker> logger, [DisallowNull] IFSIOQueueService fsIOQueueService, [DisallowNull] DeleteFileBackgroundWorker deleteFileService, [DisallowNull] DeleteCrawlConfigurationBackgroundWorker deleteCrawlConfigurationService)
        {
            _logger = logger;
            _fsIOQueueService = fsIOQueueService;
            _deleteFileService = deleteFileService;
            _deleteCrawlConfigurationService = deleteCrawlConfigurationService;
            _logger.LogDebug($"{nameof(DeleteBranchBackgroundWorker)} Service instantiated");
        }

        public DeleteBranchBackgroundJob EnqueueAsync([DisallowNull] ISubdirectoryRow subdirectory, bool forceDelete = false, bool deleteEmptyParent = false, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (subdirectory is null)
                throw new ArgumentNullException(nameof(subdirectory));
            return new DeleteBranchBackgroundJob(_logger, _fsIOQueueService, _deleteFileService, _deleteCrawlConfigurationService, subdirectory, forceDelete, deleteEmptyParent, onReportProgress, doNotUseTransaction);
        }
    }
}
