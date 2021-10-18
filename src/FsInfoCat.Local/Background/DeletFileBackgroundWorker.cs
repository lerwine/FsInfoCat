using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class DeletFileBackgroundWorker
    {
        private readonly ILogger<DeletFileBackgroundWorker> _logger;
        private readonly DbOperationService _dbOperationService;

        [ServiceBuilderHandler()]
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddSingleton<DeletFileBackgroundWorker>();
        }

        public DeletFileBackgroundWorker([DisallowNull] ILogger<DeletFileBackgroundWorker> logger, [DisallowNull] DbOperationService dbOperationService)
        {
            _logger = logger;
            _dbOperationService = dbOperationService;
        }

        public DeleteFileBackgroundJob EnqueueAsync(IFileRow file, bool forceDelete = false, bool deleteEmptyParent = false, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));
            return new DeleteFileBackgroundJob(_logger, _dbOperationService, file, forceDelete, deleteEmptyParent, onReportProgress, doNotUseTransaction);
        }
    }
}
