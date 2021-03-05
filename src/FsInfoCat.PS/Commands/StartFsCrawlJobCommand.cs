using FsInfoCat.Models.Crawl;
using FsInfoCat.Util;
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
        private string[] _fileSystemProviders = new string[0];

        [Parameter(HelpMessage = "Root crawl path", Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_NONE_TRUE)]
        [Parameter(HelpMessage = "Root crawl path", Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_TRUE)]
        [Parameter(HelpMessage = "Root crawl path", Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_TRUE)]
        [ValidateNotNullOrEmpty()]
        public string[] RootPath { get; set; }

        [Parameter(HelpMessage = "Root crawl path", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_NONE_FALSE)]
        [Parameter(HelpMessage = "Root crawl path", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_FALSE)]
        [Parameter(HelpMessage = "Root crawl path", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_FALSE)]
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

        [Parameter(HelpMessage = "Number of minutes to allow crawl to run.", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_TRUE)]
        [Parameter(HelpMessage = "Number of minutes to allow crawl to run.", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_AGE_FALSE)]
        [ValidateRange(1L, MAX_VALUE_TTL)]
        public long Ttl { get; set; }

        [Parameter(HelpMessage = "Date/Time when crawl will be stopped if it has not already completed.", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_TRUE)]
        [Parameter(HelpMessage = "Date/Time when crawl will be stopped if it has not already completed.", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_DATE_TIME_FALSE)]
        [ValidateNotNull()]
        public DateTime StopAt { get; set; }

        [Parameter(HelpMessage = "Job does not expire", ParameterSetName = PARAMETER_SET_NAME_NONE_TRUE)]
        [Parameter(HelpMessage = "Job does not expire", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_NONE_FALSE)]
        public SwitchParameter NoExpire { get; set; }

        [Parameter(HelpMessage = "Job name.")]
        [ValidateNotNullOrEmpty()]
        public string Name { get; set; }

        protected override void BeginProcessing()
        {
            Type t = typeof(FileSystemProvider);
            _fileSystemProviders = SessionState.Provider.GetAll().Where(p => t.IsAssignableFrom(p.ImplementingType)).Select(p => p.Name).ToArray();
            this.SessionState.PSVariable.Set(PS_PROPERTY_NAME_ALL_PATHS, new Collection<string>());
        }

        protected override void ProcessRecord()
        {
            Collection<string> allPaths = (Collection<string>)SessionState.PSVariable.GetValue(PS_PROPERTY_NAME_ALL_PATHS);
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_NONE_TRUE:
                case PARAMETER_SET_NAME_BY_AGE_TRUE:
                case PARAMETER_SET_NAME_DATE_TIME_TRUE:
                    foreach (string p in RootPath)
                    {
                        if (string.IsNullOrWhiteSpace(p))
                            WriteError(MessageId.InvalidPath.ToErrorRecord("Path cannot be empty", new PSArgumentNullException("RootPath"),
                                ErrorCategory.InvalidArgument, nameof(RootPath), p));
                        else
                        {
                            Collection<PathInfo> resolvedPaths;
                            try
                            {
                                resolvedPaths = SessionState.Path.GetResolvedPSPathFromPSPath(p);
                            }
                            catch (NotSupportedException exc)
                            {
                                WriteError(MessageId.InvalidPath.ToErrorRecord("Path references an unsupported provider type", exc, ErrorCategory.InvalidArgument, nameof(RootPath), p));
                                continue;
                            }
                            catch (ItemNotFoundException exc)
                            {
                                WriteError(MessageId.PathNotFound.ToErrorRecord(exc, ErrorCategory.ObjectNotFound, nameof(RootPath), p));
                                continue;
                            }
                            catch (Exception exc)
                            {
                                if (exc is System.Management.Automation.DriveNotFoundException || exc is ProviderNotFoundException)
                                    WriteError(MessageId.PathNotFound.ToErrorRecord(exc, ErrorCategory.ObjectNotFound, nameof(RootPath), p));
                                else
                                    WriteError(MessageId.InvalidPath.ToErrorRecord(exc, nameof(RootPath), p));
                                continue;
                            }

                            foreach (PathInfo pathInfo in resolvedPaths)
                            {
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
                                        fileExists = File.Exists(pathInfo.ProviderPath);
                                    }
                                    catch (Exception exc)
                                    {
                                        WriteError(MessageId.InvalidPath.ToErrorRecord(exc, ErrorCategory.InvalidArgument, nameof(RootPath), p));
                                        continue;
                                    }
                                    if (fileExists)
                                        WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Path refers does not refer to a subdirectory"),
                                            ErrorCategory.ObjectNotFound, nameof(RootPath), p));
                                    else
                                        WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Subdirectory not found"), ErrorCategory.ObjectNotFound,
                                            nameof(RootPath), p));
                                }
                            }
                        }
                    }
                    break;
                default:
                    foreach (string p in RootPath)
                    {
                        if (string.IsNullOrWhiteSpace(p))
                            WriteError(MessageId.InvalidPath.ToErrorRecord("Path cannot be empty", new PSArgumentNullException(nameof(RootPath)),
                                ErrorCategory.InvalidArgument, nameof(RootPath), p));
                        else
                        {
                            try
                            {
                                string pp = GetUnresolvedProviderPathFromPSPath(p);
                                if (Directory.Exists(pp))
                                    allPaths.Add(pp);
                                else
                                {
                                    if (File.Exists(pp))
                                        throw new DirectoryNotFoundException("Path does not refer to a subdirectory");
                                    throw new DirectoryNotFoundException("Subdirectory not found");
                                }
                            }
                            catch (DirectoryNotFoundException exc)
                            {
                                WriteError(MessageId.PathNotFound.ToErrorRecord(exc, ErrorCategory.ObjectNotFound, nameof(RootPath), p));
                            }
                            catch (Exception exc)
                            {
                                WriteError(MessageId.InvalidPath.ToErrorRecord("Cannot determine provider path", exc, ErrorCategory.InvalidArgument, nameof(RootPath), p));
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
                switch (ParameterSetName)
                {
                    case PARAMETER_SET_NAME_BY_AGE_TRUE:
                    case PARAMETER_SET_NAME_BY_AGE_FALSE:
                        crawlJob = new FsCrawlJob((IEnumerable<string>)SessionState.PSVariable.GetValue(PS_PROPERTY_NAME_ALL_PATHS), MaxDepth, MaxItems, Ttl, MachineIdentifier, GetVolumeInfos, Name);
                        break;
                    case PARAMETER_SET_NAME_DATE_TIME_TRUE:
                    case PARAMETER_SET_NAME_DATE_TIME_FALSE:
                        crawlJob = new FsCrawlJob((IEnumerable<string>)SessionState.PSVariable.GetValue(PS_PROPERTY_NAME_ALL_PATHS), MaxDepth, MaxItems, StopAt, MachineIdentifier, GetVolumeInfos, Name);
                        break;
                    default:
                        crawlJob = new FsCrawlJob((IEnumerable<string>)SessionState.PSVariable.GetValue(PS_PROPERTY_NAME_ALL_PATHS), MaxDepth, MaxItems, -1L, MachineIdentifier, GetVolumeInfos, Name);
                        break;
                }
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
