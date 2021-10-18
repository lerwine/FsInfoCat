using FsInfoCat.Background;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteBranchBackgroundWorker
    {
        private readonly ILogger<DeleteBranchBackgroundWorker> _logger;
        private readonly DbOperationService _dbOperationService;
        private readonly DeletFileBackgroundWorker _deleteFileService;
        private readonly DeleteCrawlConfigurationBackgroundWorker _deleteCrawlConfigurationService;

        [ServiceBuilderHandler()]
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddSingleton<DeleteBranchBackgroundWorker>();
        }

        public DeleteBranchBackgroundWorker([DisallowNull] ILogger<DeleteBranchBackgroundWorker> logger, [DisallowNull] DbOperationService dbOperationService, [DisallowNull] DeletFileBackgroundWorker deleteFileService, [DisallowNull] DeleteCrawlConfigurationBackgroundWorker deleteCrawlConfigurationService)
        {
            _logger = logger;
            _dbOperationService = dbOperationService;
            _deleteFileService = deleteFileService;
            _deleteCrawlConfigurationService = deleteCrawlConfigurationService;
        }

        public DeleteBranchBackgroundJob EnqueueAsync(ISubdirectoryRow subdirectory, bool forceDelete = false, bool deleteEmptyParent = false, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (subdirectory is null)
                throw new ArgumentNullException(nameof(subdirectory));
            return new DeleteBranchBackgroundJob(_logger, _dbOperationService, _deleteFileService, _deleteCrawlConfigurationService, subdirectory, forceDelete, deleteEmptyParent, onReportProgress, doNotUseTransaction);
        }
    }
}
