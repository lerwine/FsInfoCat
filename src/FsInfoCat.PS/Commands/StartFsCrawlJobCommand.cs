using FsInfoCat.Models.Crawl;
using Microsoft.PowerShell.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;

namespace FsInfoCat.PS.Commands
{
    // Start-FsCrawlJob
    [Cmdlet(VerbsLifecycle.Start, "FsCrawlJob", DefaultParameterSetName = PARAMETER_SET_NAME_BY_AGE_TRUE)]
    [OutputType(typeof(FsCrawlJob))]
    public class StartFsCrawlJobCommand : FsVolumeInfoCommand
    {
        public const string PS_PROPERTY_NAME_ALL_PATHS = "AllPaths";
        public const string PARAMETER_SET_NAME_NONE_TRUE = "none:true";
        public const string PARAMETER_SET_NAME_BY_AGE_TRUE = "age:true";
        public const string PARAMETER_SET_NAME_DATE_TIME_TRUE = "datetime:true";
        public const string PARAMETER_SET_NAME_NONE_FALSE = "none:false";
        public const string PARAMETER_SET_NAME_BY_AGE_FALSE = "age:false";
        public const string PARAMETER_SET_NAME_DATE_TIME_FALSE = "datetime:false";
        public const int DEFAULT_MAX_DEPTH = 32;
        public const long DEFAULT_MAX_ITEMS = 4294967295L;
        public const long MAX_VALUE_TTL = 4320L;
        private const string HELP_MESSAGE_ROOT_PATH = "The starting subdirectory to crawl (supports wildcards).";
        private const string HELP_MESSAGE_LITERAL_PATH = "The literal path of the starting subdirectory to crawl.";
        private const string HELP_MESSAGE_TTL = "The maximum number of minutes to allow the background crawl job to run. The default value is 4,320 (3 days) unless the StopAt parameter or NoExpire switch is used.";
        private const string HELP_MESSAGE_STOP_AT = "Date and Time when the background crawl job will be stopped if it has not already terminated.";
        private const string HELP_MESSAGE_NO_EXPIRE = "Job does not expire and continues to crawl files util it is completed, is explicitly stopped, or it encounters a terminal error.";
        private string[] _fileSystemProviders = new string[0];

        [Parameter(HelpMessage = HELP_MESSAGE_ROOT_PATH, Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_NONE_TRUE)]
        [Parameter(HelpMessage = HELP_MESSAGE_ROOT_PATH, Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_TRUE)]
        [Parameter(HelpMessage = HELP_MESSAGE_ROOT_PATH, Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_TRUE)]
        [ValidateNotNullOrEmpty()]
        public string[] Path { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_LITERAL_PATH, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_NONE_FALSE)]
        [Parameter(HelpMessage = HELP_MESSAGE_LITERAL_PATH, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_FALSE)]
        [Parameter(HelpMessage = HELP_MESSAGE_LITERAL_PATH, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_FALSE)]
        [ValidateNotNullOrEmpty()]
        public string[] LiteralPath { get; set; }

        [Parameter(HelpMessage = "Machine-specific unique identifier", Mandatory = true)]
        [ValidateNotNullOrEmpty()]
        public string MachineIdentifier { get; set; }

        [Parameter(HelpMessage = "Maximum depth of sub-folders to crawl, A value less than 1 only crawls the root path. The default is 32.")]
        [ValidateRange(0, int.MaxValue)]
        public int MaxDepth { get; set; } = DEFAULT_MAX_DEPTH;

        [Parameter(HelpMessage = "Maximum number of files and subdirectories to process. The default is 4,294,967,295.")]
        [ValidateRange(1L, long.MaxValue)]
        public long MaxItems { get; set; } = DEFAULT_MAX_ITEMS;

        [Parameter(HelpMessage = HELP_MESSAGE_TTL, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_TRUE)]
        [Parameter(HelpMessage = HELP_MESSAGE_TTL, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_FALSE)]
        [ValidateRange(1L, MAX_VALUE_TTL)]
        public long Ttl { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_STOP_AT, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_TRUE)]
        [Parameter(HelpMessage = HELP_MESSAGE_STOP_AT, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_FALSE)]
        [ValidateNotNull()]
        public DateTime StopAt { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_NO_EXPIRE, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_NONE_TRUE)]
        [Parameter(HelpMessage = HELP_MESSAGE_NO_EXPIRE, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_NONE_FALSE)]
        public SwitchParameter NoExpire { get; set; }

        [Parameter(HelpMessage = "Specifies a friendly name for the new job. You can use the name to identify the job to other job cmdlets, such as the Stop-Job cmdlet.")]
        [ValidateNotNullOrEmpty()]
        public string Name { get; set; }

        protected override void BeginProcessing()
        {
            Type t = typeof(FileSystemProvider);
            _fileSystemProviders = SessionState.Provider.GetAll().Where(p => t.IsAssignableFrom(p.ImplementingType)).Select(p => p.Name).ToArray();
            WriteDebug($"Retrieved {_fileSystemProviders.Length} file system providers: {{ \"{string.Join("\", \"", _fileSystemProviders)}\" }}");
            SessionState.PSVariable.Set(PS_PROPERTY_NAME_ALL_PATHS, new Collection<string>());
        }

        protected override void ProcessRecord()
        {
            Collection<string> allPaths = (Collection<string>)SessionState.PSVariable.GetValue(PS_PROPERTY_NAME_ALL_PATHS);
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_NONE_TRUE:
                case PARAMETER_SET_NAME_BY_AGE_TRUE:
                case PARAMETER_SET_NAME_DATE_TIME_TRUE:
                    foreach (string rPath in Path)
                    {
                        WriteDebug($"Processing RootPath: \"{rPath}\"");
                        if (string.IsNullOrWhiteSpace(rPath))
                            WriteError(MessageId.InvalidPath.ToErrorRecord("Path cannot be empty", new PSArgumentNullException("RootPath"),
                                ErrorCategory.InvalidArgument, nameof(Path), rPath));
                        else
                        {
                            foreach (string p in ResolveDirectoryFromWcPath(rPath))
                                allPaths.Add(p);
                        }
                    }
                    break;
                default:
                    foreach (string p in ResolveDirectoryFromLiteralPath(LiteralPath))
                        allPaths.Add(p);
                    break;
            }
        }

        protected override void EndProcessing()
        {
            try
            {
                FsCrawlJob crawlJob;
                IEnumerable<string> allPaths = (IEnumerable<string>)SessionState.PSVariable.GetValue(PS_PROPERTY_NAME_ALL_PATHS);
                switch (ParameterSetName)
                {
                    case PARAMETER_SET_NAME_BY_AGE_TRUE:
                    case PARAMETER_SET_NAME_BY_AGE_FALSE:
                        WriteDebug($"Creating new FsCrawlJob(startingDirectories: {{ \"{string.Join("\", \"", allPaths.ToArray())}\" }}, maxDepth: {MaxDepth}, maxItems: {MaxItems}, ttl: {Ttl}, machineIdentifier: \"{MachineIdentifier}\", getVolumes: GetVolumeInfos, friendlyName=\"{Name}\")");
                        crawlJob = new FsCrawlJob(allPaths, MaxDepth, MaxItems, Ttl, MachineIdentifier, GetVolumeInfos, Name);
                        break;
                    case PARAMETER_SET_NAME_DATE_TIME_TRUE:
                    case PARAMETER_SET_NAME_DATE_TIME_FALSE:
                        WriteDebug($"Creating new FsCrawlJob(startingDirectories: {{ \"{string.Join("\", \"", allPaths.ToArray())}\" }}, maxDepth: {MaxDepth}, maxItems: {MaxItems}, stopAt: {StopAt}, machineIdentifier: \"{MachineIdentifier}\", getVolumes: GetVolumeInfos, friendlyName=\"{Name}\")");
                        crawlJob = new FsCrawlJob(allPaths, MaxDepth, MaxItems, StopAt, MachineIdentifier, GetVolumeInfos, Name);
                        break;
                    default:
                        WriteDebug($"Creating new FsCrawlJob(startingDirectories: {{ \"{string.Join("\", \"", allPaths.ToArray())}\" }}, maxDepth: {MaxDepth}, maxItems: {MaxItems}, ttl: -1L, machineIdentifier: \"{MachineIdentifier}\", getVolumes: GetVolumeInfos, friendlyName=\"{Name}\")");
                        crawlJob = new FsCrawlJob(allPaths, MaxDepth, MaxItems, -1L, MachineIdentifier, GetVolumeInfos, Name);
                        break;
                }
                JobRepository.Add(crawlJob);
                WriteObject(crawlJob);
                WriteDebug("Queueing work item (CrawlJob.StartJob)");
                ThreadPool.QueueUserWorkItem(crawlJob.StartJob);
            }
            catch (Exception e)
            {
                WriteError(new ErrorRecord(e, Enum.GetName(typeof(MessageId), MessageId.UnexpectedError), ErrorCategory.NotSpecified, Path));
            }
        }

        protected override void OnProviderNotSupportedException(string path, Exception exc)
        {
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_NONE_TRUE:
                case PARAMETER_SET_NAME_BY_AGE_TRUE:
                case PARAMETER_SET_NAME_DATE_TIME_TRUE:
                    WriteError(MessageId.InvalidPath.ToErrorRecord("Path references an unsupported provider type", exc, ErrorCategory.InvalidArgument, nameof(Path), path));
                    break;
                default:
                    WriteError(MessageId.InvalidPath.ToErrorRecord("Path references an unsupported provider type", exc, ErrorCategory.InvalidArgument, nameof(LiteralPath), path));
                    break;
            }
        }

        protected override void OnItemNotFoundException(string path, ItemNotFoundException exc)
        {
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_NONE_TRUE:
                case PARAMETER_SET_NAME_BY_AGE_TRUE:
                case PARAMETER_SET_NAME_DATE_TIME_TRUE:
                    WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Subdirectory not found"), ErrorCategory.ObjectNotFound,
                        nameof(Path), path));
                    break;
                default:
                    WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Subdirectory not found"), ErrorCategory.ObjectNotFound,
                        nameof(LiteralPath), path));
                    break;
            }
        }

        protected override void OnResolveError(string path, Exception exc)
        {
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_NONE_TRUE:
                case PARAMETER_SET_NAME_BY_AGE_TRUE:
                case PARAMETER_SET_NAME_DATE_TIME_TRUE:
                    WriteError(MessageId.InvalidPath.ToErrorRecord(exc, nameof(Path), path));
                    break;
                default:
                    WriteError(MessageId.InvalidPath.ToErrorRecord(exc, nameof(LiteralPath), path));
                    break;
            }
        }

        protected override void OnPathIsFileError(string providerPath)
        {
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_NONE_TRUE:
                case PARAMETER_SET_NAME_BY_AGE_TRUE:
                case PARAMETER_SET_NAME_DATE_TIME_TRUE:
                    WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Path refers does not refer to a subdirectory"),
                        ErrorCategory.ObjectNotFound, nameof(Path), providerPath));
                    break;
                default:
                    WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Path refers does not refer to a subdirectory"),
                        ErrorCategory.ObjectNotFound, nameof(Path), providerPath));
                    break;
            }
        }
    }
}
