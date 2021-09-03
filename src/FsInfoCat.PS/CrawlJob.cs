using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.PS
{
    public class CrawlJob : Job
    {
        private readonly CrawlTaskManager _crawlWorker;
        private readonly bool _disposeWorker;

        // DEFERRED: Implement HasMoreData
        public override bool HasMoreData => throw new NotImplementedException();

        // DEFERRED: Implement Location
        public override string Location => throw new NotImplementedException();

        // DEFERRED: Implement StatusMessage
        public override string StatusMessage => throw new NotImplementedException();

        internal CrawlJob([DisallowNull] Local.CrawlTaskManager crawlWorker, bool doNotDisposeWorker = false)
        {
            _crawlWorker = crawlWorker;
            _disposeWorker = !doNotDisposeWorker;
        }

        public override void StopJob()
        {
            // DEFERRED: Implement StopJob()
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _disposeWorker)
                _crawlWorker.Dispose();
            base.Dispose(disposing);
        }
    }
}
