using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;
using Microsoft.PowerShell.Commands;

namespace FsInfoCat.PS.Commands
{
    // Start-FsCrawlJob
    [Cmdlet(VerbsLifecycle.Start, "FsCrawlJob", DefaultParameterSetName = PARAMETER_SET_NAME_NONE_TRUE)]
    [OutputType(typeof(FsCrawlJob))]
    public class StartFsCrawlJobCommand : FsVolumeInfoCommand
    {
        private const string Property_Name_All_Paths = "AllPaths";
        private const string PARAMETER_SET_NAME_NONE_TRUE = "none:true";
        private const string PARAMETER_SET_NAME_BY_AGE_TRUE = "age:true";
        private const string PARAMETER_SET_NAME_DATE_TIME_TRUE = "datetime:true";
        private const string PARAMETER_SET_NAME_NONE_FALSE = "none:false";
        private const string PARAMETER_SET_NAME_BY_AGE_FALSE = "age:false";
        private const string PARAMETER_SET_NAME_DATE_TIME_FALSE = "datetime:false";
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

        [Parameter(HelpMessage = "Supplier function to get volumes", Mandatory = true)]
        [ValidateNotNull()]
        [Obsolete("Register-Volume is now used")]
        public Func<IEnumerable<IVolumeInfo>> GetVolumes { get; set; }

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
            this.SessionState.PSVariable.Set(Property_Name_All_Paths, new Collection<string>());
        }

        protected override void ProcessRecord()
        {
            Collection<string> allPaths = (Collection<string>)SessionState.PSVariable.GetValue(Property_Name_All_Paths);
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_NONE_TRUE:
                case PARAMETER_SET_NAME_BY_AGE_TRUE:
                case PARAMETER_SET_NAME_DATE_TIME_TRUE:
                    foreach (string p in RootPath)
                    {
                        if (string.IsNullOrWhiteSpace(p))
                            WriteError(new ErrorRecord(new PSArgumentNullException("RootPath"), MessageId.InvalidPath.ToString("F"), ErrorCategory.InvalidArgument, p)
                            {
                                ErrorDetails = new ErrorDetails("Path cannot be empty")
                            });
                        else
                        {
                            Collection<PathInfo> resolvedPaths;
                            try
                            {
                                resolvedPaths = SessionState.Path.GetResolvedPSPathFromPSPath(p);
                            }
                            catch (NotSupportedException exc)
                            {
                                WriteError(new ErrorRecord(exc, MessageId.InvalidPath.ToString("F"), ErrorCategory.InvalidArgument, p)
                                {
                                    ErrorDetails = new ErrorDetails("Path references an unsupported provider type")
                                });
                                continue;
                            }
                            catch (ItemNotFoundException exc)
                            {
                                WriteError(new ErrorRecord(exc, MessageId.PathNotFound.ToString("F"), ErrorCategory.ObjectNotFound, p));
                                continue;
                            }
                            catch (Exception exc)
                            {
                                if (exc is System.Management.Automation.DriveNotFoundException || exc is ProviderNotFoundException)
                                    WriteError(new ErrorRecord(exc, MessageId.PathNotFound.ToString("F"), ErrorCategory.ObjectNotFound, p));
                                else
                                    WriteError(new ErrorRecord(exc, MessageId.InvalidPath.ToString("F"), ErrorCategory.InvalidOperation, p));
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
                                        WriteError(new ErrorRecord(exc, MessageId.InvalidPath.ToString("F"), ErrorCategory.InvalidArgument, p));
                                        continue;
                                    }
                                    if (fileExists)
                                        WriteError(new ErrorRecord(new DirectoryNotFoundException("Path refers does not refer to a subdirectory"), MessageId.PathNotFound.ToString("F"), ErrorCategory.ObjectNotFound, p));
                                    else
                                        WriteError(new ErrorRecord(new DirectoryNotFoundException("Subdirectory not found"), MessageId.PathNotFound.ToString("F"), ErrorCategory.ObjectNotFound, p));
                                }
                            }
                        }
                    }
                    break;
                default:
                    foreach (string p in RootPath)
                    {
                        if (string.IsNullOrWhiteSpace(p))
                            WriteError(new ErrorRecord(new PSArgumentNullException("RootPath"), MessageId.InvalidPath.ToString("F"), ErrorCategory.InvalidArgument, p)
                            {
                                ErrorDetails = new ErrorDetails("Path cannot be empty")
                            });
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
                                        throw new DirectoryNotFoundException("Path refers does not refer to a subdirectory");
                                    throw new DirectoryNotFoundException("Subdirectory not found");
                                }
                            }
                            catch (DirectoryNotFoundException exc)
                            {
                                WriteError(new ErrorRecord(exc, MessageId.PathNotFound.ToString("F"), ErrorCategory.ObjectNotFound, p));
                            }
                            catch (Exception exc)
                            {
                                WriteError(new ErrorRecord(exc, MessageId.InvalidPath.ToString("F"), ErrorCategory.InvalidArgument, p)
                                {
                                    ErrorDetails = new ErrorDetails("Cannot determine provider path")
                                });
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
                        crawlJob = new FsCrawlJob((IEnumerable<string>)SessionState.PSVariable.GetValue(Property_Name_All_Paths), MaxDepth, MaxItems, Ttl, MachineIdentifier, GetVolumes, Name);
                        break;
                    case PARAMETER_SET_NAME_DATE_TIME_TRUE:
                    case PARAMETER_SET_NAME_DATE_TIME_FALSE:
                        crawlJob = new FsCrawlJob((IEnumerable<string>)SessionState.PSVariable.GetValue(Property_Name_All_Paths), MaxDepth, MaxItems, StopAt, MachineIdentifier, GetVolumes, Name);
                        break;
                    default:
                        crawlJob = new FsCrawlJob((IEnumerable<string>)SessionState.PSVariable.GetValue(Property_Name_All_Paths), MaxDepth, MaxItems, -1L, MachineIdentifier, GetVolumes, Name);
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
