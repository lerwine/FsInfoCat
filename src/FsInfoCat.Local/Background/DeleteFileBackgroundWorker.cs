using FsInfoCat.AsyncOps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteFileBackgroundWorker
    {
        private readonly ILogger<DeleteFileBackgroundWorker> _logger;
        private readonly JobQueue _jobQueueService;

        [ServiceBuilderHandler(Priority = 200)]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DeleteFileBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<DeleteFileBackgroundWorker>();
        }

        public DeleteFileBackgroundWorker([DisallowNull] ILogger<DeleteFileBackgroundWorker> logger, [DisallowNull] JobQueue jobQueueService)
        {
            _logger = logger;
            _jobQueueService = jobQueueService;
            _logger.LogDebug($"{nameof(DeleteFileBackgroundWorker)} Service instantiated");
        }

        public DeleteFileBackgroundJob EnqueueAsync(IFileRow file, bool forceDelete = false, bool deleteEmptyParent = false, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));
            return new DeleteFileBackgroundJob(_logger, _jobQueueService, file, forceDelete, deleteEmptyParent, onReportProgress, doNotUseTransaction);
        }
    }
}
