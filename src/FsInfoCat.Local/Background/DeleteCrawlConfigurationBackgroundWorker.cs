using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteCrawlConfigurationBackgroundWorker
    {
        private readonly ILogger<DeleteCrawlConfigurationBackgroundWorker> _logger;
        private readonly DbOperationService _dbOperationService;

        [ServiceBuilderHandler()]
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddSingleton<DeleteBranchBackgroundWorker>();
        }

        public DeleteCrawlConfigurationBackgroundWorker([DisallowNull] ILogger<DeleteCrawlConfigurationBackgroundWorker> logger, [DisallowNull] DbOperationService dbOperationService)
        {
            _logger = logger;
            _dbOperationService = dbOperationService;
        }

        public DeleteCrawlConfigurationBackgroundJob EnqueueAsync(ICrawlConfigurationRow crawlConfiguration, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (crawlConfiguration is null)
                throw new ArgumentNullException(nameof(crawlConfiguration));
            return new DeleteCrawlConfigurationBackgroundJob(_logger, _dbOperationService, crawlConfiguration, onReportProgress, doNotUseTransaction);
        }
    }
}
