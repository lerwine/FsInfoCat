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
        private readonly CrawlWorker _crawlWorker;
        private readonly bool _disposeWorker;

        public override bool HasMoreData => throw new NotImplementedException();

        public override string Location => throw new NotImplementedException();

        public override string StatusMessage => throw new NotImplementedException();

        internal CrawlJob([DisallowNull] Local.CrawlWorker crawlWorker, bool doNotDisposeWorker = false)
        {
            _crawlWorker = crawlWorker;
            _disposeWorker = !doNotDisposeWorker;
        }

        public override void StopJob()
        {
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
