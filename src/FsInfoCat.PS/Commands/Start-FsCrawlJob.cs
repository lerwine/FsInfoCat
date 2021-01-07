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

        [Parameter(HelpMessage = "Maximum crawl depth")]
        public int MaxDepth { get; set; }

        protected override void ProcessRecord()
        {
            try {
                throw new NotImplementedException();
            } catch (Exception e)
            {
                WriteError(new ErrorRecord(e, "1", ErrorCategory.NotImplemented, RootPath));
            }

        }
    }
}
