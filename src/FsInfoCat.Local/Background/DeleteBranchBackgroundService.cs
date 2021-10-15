using FsInfoCat.Background;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public class DeleteBranchBackgroundService : LongRunningAsyncService<Task<bool>>, IDeleteBranchBackgroundService<Subdirectory>
    {
        private readonly LocalDbContext _dbContext;

        public Subdirectory Target { get; set; }

        ISubdirectory IDeleteBranchBackgroundService.Target => Target;

        public bool ForceDeleteFromDb { get; set; }

        Subdirectory IDeleteBranchBackgroundService<Subdirectory>.Target { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IProgress<string> ReportProgressHandler { get; set; }

        public DeleteBranchBackgroundService(ILogger<DeleteBranchBackgroundService> logger, LocalDbContext dbContext)
            : base(logger)
        {
            _dbContext = dbContext;
        }

        protected async override Task<bool> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
