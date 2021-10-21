using FsInfoCat.AsyncOps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Local.Background
{
    public class ImportBranchBackgroundWorker
    {
        private readonly ILogger<ImportBranchBackgroundWorker> _logger;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        private readonly JobQueue _jobQueueService;

        [ServiceBuilderHandler(Priority = 200)]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(ImportBranchBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<ImportBranchBackgroundWorker>();
        }

        public ImportBranchBackgroundWorker([DisallowNull] ILogger<ImportBranchBackgroundWorker> logger, IFileSystemDetailService fileSystemDetailService, [DisallowNull] JobQueue jobQueueService)
        {
            _fileSystemDetailService = fileSystemDetailService;
            _logger = logger;
            _jobQueueService = jobQueueService;
            _logger.LogDebug($"{nameof(ImportBranchBackgroundWorker)} Service instantiated");
        }

        public ImportBranchBackgroundJob EnqueueAsync(DirectoryInfo directoryInfo, bool markNewAsCompleted = false, IProgress<string> onReportProgress = null)
        {
            if (directoryInfo is null)
                throw new ArgumentNullException(nameof(directoryInfo));
            return new ImportBranchBackgroundJob(_logger, _fileSystemDetailService, _jobQueueService, directoryInfo, markNewAsCompleted, onReportProgress);
        }
    }
}
