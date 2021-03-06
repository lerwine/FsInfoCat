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
    [Cmdlet(VerbsLifecycle.Start, "FsCrawlJob", DefaultParameterSetName = PARAMETER_SET_NAME_NONE_TRUE)]
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
        public const int DEFAULT_MAX_DEPTH = 512;
        public const long DEFAULT_MAX_ITEMS = 4294967295L;
        public const long MAX_VALUE_TTL = 9223372036854775L;
        private const string HELP_MESSAGE_ROOT_PATH = "Root crawl path";
        private const string HELP_MESSAGE_LITERAL_PATH = "Literal Root crawl path";
        private const string HELP_MESSAGE_TTL = "Number of minutes to allow crawl to run.";
        private const string HELP_MESSAGE_STOP_AT = "Date/Time when crawl will be stopped if it has not already completed.";
        private const string HELP_MESSAGE_NO_EXPIRE = "Job does not expire";
        private string[] _fileSystemProviders = new string[0];

        [Parameter(HelpMessage = HELP_MESSAGE_ROOT_PATH, Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_NONE_TRUE)]
        [Parameter(HelpMessage = HELP_MESSAGE_ROOT_PATH, Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_TRUE)]
        [Parameter(HelpMessage = HELP_MESSAGE_ROOT_PATH, Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_TRUE)]
        [ValidateNotNullOrEmpty()]
        public string[] RootPath { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_LITERAL_PATH, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_NONE_FALSE)]
        [Parameter(HelpMessage = HELP_MESSAGE_LITERAL_PATH, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_FALSE)]
        [Parameter(HelpMessage = HELP_MESSAGE_LITERAL_PATH, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_FALSE)]
        [ValidateNotNullOrEmpty()]
        public string[] LiteralPath { get; set; }

        [Parameter(HelpMessage = "Machine-specific unique identifier", Mandatory = true)]
        [ValidateNotNullOrEmpty()]
        public string MachineIdentifier { get; set; }

        [Parameter(HelpMessage = "Maximum crawl depth. Default is 512")]
        [ValidateRange(0, int.MaxValue)]
        public int MaxDepth { get; set; } = DEFAULT_MAX_DEPTH;

        [Parameter(HelpMessage = "Maximum items. Default is 4294967295")]
        [ValidateRange(1L, long.MaxValue)]
        public long MaxItems { get; set; } = DEFAULT_MAX_ITEMS;

        [Parameter(HelpMessage = HELP_MESSAGE_TTL, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_TRUE)]
        [Parameter(HelpMessage = HELP_MESSAGE_TTL, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_FALSE)]
        [ValidateRange(1L, MAX_VALUE_TTL)]
        public long Ttl { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_STOP_AT, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_TRUE)]
        [Parameter(HelpMessage = HELP_MESSAGE_STOP_AT, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_FALSE)]
        [ValidateNotNull()]
        public DateTime StopAt { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_NO_EXPIRE, ParameterSetName = PARAMETER_SET_NAME_NONE_TRUE)]
        [Parameter(HelpMessage = HELP_MESSAGE_NO_EXPIRE, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_NONE_FALSE)]
        public SwitchParameter NoExpire { get; set; }

        [Parameter(HelpMessage = "Job name.")]
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
                    foreach (string rPath in RootPath)
                    {
                        WriteDebug($"Processing RootPath: \"{rPath}\"");
                        if (string.IsNullOrWhiteSpace(rPath))
                            WriteError(MessageId.InvalidPath.ToErrorRecord("Path cannot be empty", new PSArgumentNullException("RootPath"),
                                ErrorCategory.InvalidArgument, nameof(RootPath), rPath));
                        else
                        {
                            Collection<PathInfo> resolvedPaths;
                            try
                            {
                                resolvedPaths = SessionState.Path.GetResolvedPSPathFromPSPath(rPath);
                                WriteDebug($"Resolved {resolvedPaths.Count} paths: \"{string.Join("\", \"", resolvedPaths.Select(p => p.ProviderPath))}\"");
                            }
                            catch (NotSupportedException exc)
                            {
                                WriteError(MessageId.InvalidPath.ToErrorRecord("Path references an unsupported provider type", exc, ErrorCategory.InvalidArgument, nameof(RootPath), rPath));
                                continue;
                            }
                            catch (ItemNotFoundException exc)
                            {
                                WriteError(MessageId.PathNotFound.ToErrorRecord(exc, ErrorCategory.ObjectNotFound, nameof(RootPath), rPath));
                                continue;
                            }
                            catch (Exception exc)
                            {
                                if (exc is System.Management.Automation.DriveNotFoundException || exc is ProviderNotFoundException)
                                    WriteError(MessageId.PathNotFound.ToErrorRecord(exc, ErrorCategory.ObjectNotFound, nameof(RootPath), rPath));
                                else
                                    WriteError(MessageId.InvalidPath.ToErrorRecord(exc, nameof(RootPath), rPath));
                                continue;
                            }

                            foreach (PathInfo pathInfo in resolvedPaths)
                            {
                                WriteDebug($"Processing resolved path: \"{pathInfo.ProviderPath}\"");
                                if (null != pathInfo.Provider && _fileSystemProviders.Contains(pathInfo.Provider.Name, StringComparer.InvariantCultureIgnoreCase))
                                {
                                    bool fileExists;
                                    try
                                    {
                                        if (Directory.Exists(pathInfo.ProviderPath))
                                        {
                                            allPaths.Add(pathInfo.ProviderPath);
                                            continue;
                                        }
                                        WriteDebug($"Directory not found: \"{pathInfo.ProviderPath}\"");
                                        fileExists = File.Exists(pathInfo.ProviderPath);
                                    }
                                    catch (Exception exc)
                                    {
                                        WriteError(MessageId.InvalidPath.ToErrorRecord(exc, ErrorCategory.InvalidArgument, nameof(RootPath), rPath));
                                        continue;
                                    }
                                    if (fileExists)
                                        WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Path refers does not refer to a subdirectory"),
                                            ErrorCategory.ObjectNotFound, nameof(RootPath), rPath));
                                    else
                                        WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Subdirectory not found"), ErrorCategory.ObjectNotFound,
                                            nameof(RootPath), rPath));
                                }
                                else
                                    WriteWarning($"Path is not for a supported provider: {pathInfo.ProviderPath}");
                            }
                        }
                    }
                    break;
                default:
                    foreach (string lPath in LiteralPath)
                    {
                        WriteDebug($"Processing LiteralPath: \"{lPath}\"");
                        if (string.IsNullOrWhiteSpace(lPath))
                            WriteError(MessageId.InvalidPath.ToErrorRecord("Path cannot be empty", new PSArgumentNullException(nameof(LiteralPath)),
                                ErrorCategory.InvalidArgument, nameof(LiteralPath), lPath));
                        else
                        {
                            try
                            {
                                string pPath = GetUnresolvedProviderPathFromPSPath(lPath);
                                WriteDebug($"Unresolved provider path from LiteralPath: \"{pPath}\"");
                                if (Directory.Exists(pPath))
                                    allPaths.Add(pPath);
                                else
                                {
                                    if (File.Exists(pPath))
                                        throw new DirectoryNotFoundException("Path does not refer to a subdirectory");
                                    throw new DirectoryNotFoundException("Subdirectory not found");
                                }
                            }
                            catch (DirectoryNotFoundException exc)
                            {
                                WriteError(MessageId.PathNotFound.ToErrorRecord(exc, ErrorCategory.ObjectNotFound, nameof(LiteralPath), lPath));
                            }
                            catch (Exception exc)
                            {
                                WriteError(MessageId.InvalidPath.ToErrorRecord("Cannot determine provider path", exc, ErrorCategory.InvalidArgument, nameof(LiteralPath), lPath));
                            }
                        }
                    }
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
                WriteError(new ErrorRecord(e, Enum.GetName(typeof(MessageId), MessageId.UnexpectedError), ErrorCategory.NotSpecified, RootPath));
            }
        }
    }
}
