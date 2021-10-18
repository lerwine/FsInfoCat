using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Background
{
    public class MarkBranchIncompleteBackgroundWorker
    {
        private readonly ILogger<MarkBranchIncompleteBackgroundWorker> _logger;
        private readonly DbOperationService _dbOperationService;

        [ServiceBuilderHandler()]
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddSingleton<MarkBranchIncompleteBackgroundWorker>();
        }

        public MarkBranchIncompleteBackgroundWorker([DisallowNull] ILogger<MarkBranchIncompleteBackgroundWorker> logger, [DisallowNull] DbOperationService dbOperationService)
        {
            _logger = logger;
            _dbOperationService = dbOperationService;
        }

        public MarkBranchIncompleteBackgroundJob EnqueueAsync(Subdirectory subdirectory, IProgress<string> onReportProgress = null, bool doNotUseTransaction = false)
        {
            if (subdirectory is null)
                throw new ArgumentNullException(nameof(subdirectory));
            return new MarkBranchIncompleteBackgroundJob(_logger, _dbOperationService, subdirectory, onReportProgress, doNotUseTransaction);
        }
    }
}
