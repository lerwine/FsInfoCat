using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Threading;
using FsInfoCat.Models.Crawl;

namespace FsInfoCat.PS.Commands
{
    // Start-FsCrawlJob
    [Cmdlet(VerbsLifecycle.Start, "FsCrawlJob")]
    [CmdletBinding(DefaultParameterSetName = PARAMETER_SET_NAME_EXPIRE_BY_AGE)]
    [OutputType(typeof(FsCrawlJob))]
    public class Start_FsCrawlJob : PSCmdlet
    {
        private const string Property_Name_All_Paths = "AllPaths";
        private const string PARAMETER_SET_NAME_EXPIRE_BY_AGE = "ExpireByAge";
        private const string PARAMETER_SET_NAME_EXPIRE_AT_DATE_TIME = "ExpireAtDateTime";
        public const int DEFAULT_MAX_DEPTH = 512;
        public const long DEFAULT_MAX_ITEMS = 4294967295L;
        public const long MAX_VALUE_TTL = 9223372036854775L;

        [Parameter(HelpMessage = "Root crawl path", Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty()]
        public string[] RootPath { get; set; }

        [Parameter(HelpMessage = "Maximum crawl depth. Default is 512")]
        [ValidateRange(0L, int.MaxValue)]
        public int MaxDepth { get; set; } = DEFAULT_MAX_DEPTH;

        [Parameter(HelpMessage = "Maximum items. Default is 4294967295")]
        [ValidateRange(1L, long.MaxValue)]
        public long MaxItems { get; set; } = DEFAULT_MAX_ITEMS;

        [Parameter(HelpMessage = "Number of minutes to allow crawl to run or 0 for no limit.", ParameterSetName = PARAMETER_SET_NAME_EXPIRE_BY_AGE)]
        [ValidateRange(0L, MAX_VALUE_TTL)]
        public long Ttl { get; set; } = 0L;

        [Parameter(HelpMessage = "Date/Time when crawl will be stopped if it has not already completed.", ParameterSetName = PARAMETER_SET_NAME_EXPIRE_AT_DATE_TIME)]
        [ValidateNotNull()]
        public DateTime StopAt { get; set; }

        [Parameter(HelpMessage = "Job name.")]
        [ValidateNotNullOrEmpty()]
        public string Name { get; set; }

        protected override void BeginProcessing()
        {
            this.SessionState.PSVariable.Set(Property_Name_All_Paths, new Collection<string>());
        }

        protected override void ProcessRecord()
        {
            Collection<string> allPaths = (Collection<string>)SessionState.PSVariable.GetValue(Property_Name_All_Paths);
            foreach (string p in RootPath)
                allPaths.Add(p);
        }

        protected override void EndProcessing()
        {
            try
            {
                FsCrawlJob crawlJob;
                if (ParameterSetName == PARAMETER_SET_NAME_EXPIRE_AT_DATE_TIME)
                    crawlJob = new FsCrawlJob(Name, MaxDepth, MaxItems, StopAt, (Collection<string>)SessionState.PSVariable.GetValue(Property_Name_All_Paths));
                else
                    crawlJob = new FsCrawlJob(Name, MaxDepth, MaxItems, Ttl, (Collection<string>)SessionState.PSVariable.GetValue(Property_Name_All_Paths));
                JobRepository.Add(crawlJob);
                WriteObject(crawlJob);
                ThreadPool.QueueUserWorkItem(crawlJob.StartJob);
            }
            catch (Exception e)
            {
                WriteError(new ErrorRecord(e, Enum.GetName(typeof(MessageId), MessageId.UnexpectedError), ErrorCategory.NotSpecified, RootPath));
            }
        }
    }
}
