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

        [ServiceBuilderHandler(Priority = 200)]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DeleteCrawlConfigurationBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<DeleteBranchBackgroundWorker>();
        }

        // TODO: Use FsInfoCat.AsyncOps.JobQueue instead of FsInfoCat.Local.Background.DbOperationService #105
        public DeleteCrawlConfigurationBackgroundWorker([DisallowNull] ILogger<DeleteCrawlConfigurationBackgroundWorker> logger, [DisallowNull] DbOperationService dbOperationService)
        {
            _logger = logger;
            _dbOperationService = dbOperationService;
            _logger.LogDebug($"{nameof(DeleteCrawlConfigurationBackgroundWorker)} Service instantiated");
        }

        public DeleteCrawlConfigurationBackgroundJob EnqueueAsync(ICrawlConfigurationRow crawlConfiguration, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (crawlConfiguration is null)
                throw new ArgumentNullException(nameof(crawlConfiguration));
            return new DeleteCrawlConfigurationBackgroundJob(_logger, _dbOperationService, crawlConfiguration, onReportProgress, doNotUseTransaction);
        }
    }
}
