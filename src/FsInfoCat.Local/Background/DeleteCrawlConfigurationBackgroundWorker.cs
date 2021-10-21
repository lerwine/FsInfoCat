using FsInfoCat.AsyncOps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteCrawlConfigurationBackgroundWorker
    {
        private readonly ILogger<DeleteCrawlConfigurationBackgroundWorker> _logger;
        private readonly JobQueue _jobQueueService;

        [ServiceBuilderHandler(Priority = 200)]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DeleteCrawlConfigurationBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<DeleteBranchBackgroundWorker>();
        }

        public DeleteCrawlConfigurationBackgroundWorker([DisallowNull] ILogger<DeleteCrawlConfigurationBackgroundWorker> logger, [DisallowNull] JobQueue jobQueueService)
        {
            _logger = logger;
            _jobQueueService = jobQueueService;
            _logger.LogDebug($"{nameof(DeleteCrawlConfigurationBackgroundWorker)} Service instantiated");
        }

        public DeleteCrawlConfigurationBackgroundJob EnqueueAsync(ICrawlConfigurationRow crawlConfiguration, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (crawlConfiguration is null)
                throw new ArgumentNullException(nameof(crawlConfiguration));
            return new DeleteCrawlConfigurationBackgroundJob(_logger, _jobQueueService, crawlConfiguration, onReportProgress, doNotUseTransaction);
        }
    }
}
