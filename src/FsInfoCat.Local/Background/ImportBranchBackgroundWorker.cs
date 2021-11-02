using FsInfoCat.Services;
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
        private readonly IFSIOQueueService _fsIOQueueService;

        [ServiceBuilderHandler(Priority = 200)]
        private static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(ImportBranchBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<ImportBranchBackgroundWorker>();
        }

        public ImportBranchBackgroundWorker([DisallowNull] ILogger<ImportBranchBackgroundWorker> logger, [DisallowNull] IFileSystemDetailService fileSystemDetailService, [DisallowNull] IFSIOQueueService fsIOQueueService)
        {
            _fileSystemDetailService = fileSystemDetailService;
            _logger = logger;
            _fsIOQueueService = fsIOQueueService;
            _logger.LogDebug($"{nameof(ImportBranchBackgroundWorker)} Service instantiated");
        }

        public ImportBranchBackgroundJob EnqueueAsync([DisallowNull] DirectoryInfo directoryInfo, bool markNewAsCompleted = false, IProgress<string> onReportProgress = null)
        {
            if (directoryInfo is null)
                throw new ArgumentNullException(nameof(directoryInfo));
            return new ImportBranchBackgroundJob(_logger, _fileSystemDetailService, _fsIOQueueService, directoryInfo, markNewAsCompleted, onReportProgress);
        }
    }
}
