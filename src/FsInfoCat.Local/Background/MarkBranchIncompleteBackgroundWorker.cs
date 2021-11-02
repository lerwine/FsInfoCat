using FsInfoCat.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class MarkBranchIncompleteBackgroundWorker
    {
        private readonly ILogger<MarkBranchIncompleteBackgroundWorker> _logger;
        private readonly IFSIOQueueService _fsIOQueueService;

        [ServiceBuilderHandler(Priority = 200)]
        private static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(MarkBranchIncompleteBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<MarkBranchIncompleteBackgroundWorker>();
        }

        public MarkBranchIncompleteBackgroundWorker([DisallowNull] ILogger<MarkBranchIncompleteBackgroundWorker> logger, [DisallowNull] IFSIOQueueService fsIOQueueService)
        {
            _logger = logger;
            _fsIOQueueService = fsIOQueueService;
            _logger.LogDebug($"{nameof(MarkBranchIncompleteBackgroundWorker)} Service instantiated");
        }

        public MarkBranchIncompleteBackgroundJob EnqueueAsync([DisallowNull] Subdirectory subdirectory, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (subdirectory is null)
                throw new ArgumentNullException(nameof(subdirectory));
            return new MarkBranchIncompleteBackgroundJob(_logger, _fsIOQueueService, subdirectory, onReportProgress, doNotUseTransaction);
        }
    }
}
