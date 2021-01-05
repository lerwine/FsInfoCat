using System;
using System.Management.Automation;

namespace FsInfoCat.PS.Commands
{
    // Start-FsCrawlJob
    [Cmdlet(VerbsLifecycle.Start, "FsCrawlJob")]
    public class Start_FsCrawlJob : PSCmdlet
    {
        [Parameter(HelpMessage = "Root crawl path", Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty()]
        public string[] RootPath { get; set; }

        [Parameter(HelpMessage = "Maximum crawl depth", Mandatory = true)]
        public int MaxDepth { get; set; }

        [Parameter(HelpMessage = "Maximum number of files", Mandatory = true)]
        public long MaxFiles { get; set; }

        [Parameter(HelpMessage = "Maximum result packet size in bytes", Mandatory = true)]
        public long MaxSize { get; set; }

        protected override void ProcessRecord()
        {
            // TODO: Implement this
        }
    }
}
