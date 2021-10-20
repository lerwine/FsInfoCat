using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeleteFileBackgroundWorker
    {
        private readonly ILogger<DeleteFileBackgroundWorker> _logger;
        private readonly DbOperationService _dbOperationService;

        [ServiceBuilderHandler(Priority = 200)]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DeleteFileBackgroundWorker).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<DeleteFileBackgroundWorker>();
        }

        // TODO: Use FsInfoCat.AsyncOps.JobQueue instead of FsInfoCat.Local.Background.DbOperationService #105
        public DeleteFileBackgroundWorker([DisallowNull] ILogger<DeleteFileBackgroundWorker> logger, [DisallowNull] DbOperationService dbOperationService)
        {
            _logger = logger;
            _dbOperationService = dbOperationService;
            _logger.LogDebug($"{nameof(DeleteFileBackgroundWorker)} Service instantiated");
        }

        public DeleteFileBackgroundJob EnqueueAsync(IFileRow file, bool forceDelete = false, bool deleteEmptyParent = false, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));
            return new DeleteFileBackgroundJob(_logger, _dbOperationService, file, forceDelete, deleteEmptyParent, onReportProgress, doNotUseTransaction);
        }
    }
}
