using FsInfoCat.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteCrawlConfigurationBackgroundWorker
    {
        private readonly ILogger<DeleteCrawlConfigurationBackgroundWorker> _logger;
        private readonly IFSIOQueueService _fsIOQueueService;

        [ServiceBuilderHandler(Priority = 200)]
        private static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DeleteCrawlConfigurationBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<DeleteBranchBackgroundWorker>();
        }

        public DeleteCrawlConfigurationBackgroundWorker([DisallowNull] ILogger<DeleteCrawlConfigurationBackgroundWorker> logger, [DisallowNull] IFSIOQueueService fsIOQueueService)
        {
            _logger = logger;
            _fsIOQueueService = fsIOQueueService;
            _logger.LogDebug($"{nameof(DeleteCrawlConfigurationBackgroundWorker)} Service instantiated");
        }

        public DeleteCrawlConfigurationBackgroundJob EnqueueAsync([DisallowNull] ICrawlConfigurationRow crawlConfiguration, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (crawlConfiguration is null)
                throw new ArgumentNullException(nameof(crawlConfiguration));
            return new DeleteCrawlConfigurationBackgroundJob(_logger, _fsIOQueueService, crawlConfiguration, onReportProgress, doNotUseTransaction);
        }
    }
}
