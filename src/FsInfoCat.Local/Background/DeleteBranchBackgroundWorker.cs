using FsInfoCat.AsyncOps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteBranchBackgroundWorker
    {
        private readonly ILogger<DeleteBranchBackgroundWorker> _logger;
        private readonly JobQueue _jobQueueService;
        private readonly DeleteFileBackgroundWorker _deleteFileService;
        private readonly DeleteCrawlConfigurationBackgroundWorker _deleteCrawlConfigurationService;

        [ServiceBuilderHandler(Priority = 300)]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DeleteBranchBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<DeleteBranchBackgroundWorker>();
        }

        public DeleteBranchBackgroundWorker([DisallowNull] ILogger<DeleteBranchBackgroundWorker> logger, [DisallowNull] JobQueue jobQueueService, [DisallowNull] DeleteFileBackgroundWorker deleteFileService, [DisallowNull] DeleteCrawlConfigurationBackgroundWorker deleteCrawlConfigurationService)
        {
            _logger = logger;
            _jobQueueService = jobQueueService;
            _deleteFileService = deleteFileService;
            _deleteCrawlConfigurationService = deleteCrawlConfigurationService;
            _logger.LogDebug($"{nameof(DeleteBranchBackgroundWorker)} Service instantiated");
        }

        public DeleteBranchBackgroundJob EnqueueAsync(ISubdirectoryRow subdirectory, bool forceDelete = false, bool deleteEmptyParent = false, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (subdirectory is null)
                throw new ArgumentNullException(nameof(subdirectory));
            return new DeleteBranchBackgroundJob(_logger, _jobQueueService, _deleteFileService, _deleteCrawlConfigurationService, subdirectory, forceDelete, deleteEmptyParent, onReportProgress, doNotUseTransaction);
        }
    }
}