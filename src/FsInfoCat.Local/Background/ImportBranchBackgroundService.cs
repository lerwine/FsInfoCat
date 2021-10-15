using FsInfoCat.Background;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class ImportBranchBackgroundService : LongRunningAsyncService<Task<Subdirectory>>, IImportBranchBackgroundService<Subdirectory>
    {
        private readonly LocalDbContext _dbContext;

        public DirectoryInfo Source { get; set; }

        Task<ISubdirectory> IImportBranchBackgroundService.Task => throw new NotImplementedException();

        public IProgress<string> ReportProgressHandler { get; set; }

        public ImportBranchBackgroundService(ILogger<ImportBranchBackgroundService> logger, LocalDbContext dbContext)
            : base(logger)
        {
            _dbContext = dbContext;
        }

        protected async override Task<Subdirectory> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
