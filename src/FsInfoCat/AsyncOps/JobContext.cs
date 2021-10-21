using System;
using System.Threading;

namespace FsInfoCat.AsyncOps
{
    public class JobContext
    {
        private readonly IJobResult _jobResult;

        public CancellationToken CancellationToken { get; }

        public DateTime Started => _jobResult.Started;

        public TimeSpan Elapsed => _jobResult.Elapsed;

        public JobContext(IJobResult jobResult, CancellationToken cancellationToken)
        {
            _jobResult = jobResult;
            CancellationToken = cancellationToken;
        }
    }
}
