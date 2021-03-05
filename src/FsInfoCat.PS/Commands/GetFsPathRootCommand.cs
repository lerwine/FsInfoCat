using FsInfoCat.Models.Crawl;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace FsInfoCat.PS.Commands
{
    // Get-FsPathRoot
    [Cmdlet(VerbsCommon.Get, "FsPathRoot", DefaultParameterSetName = PARAMETER_SET_NAME_ASSUME_URI)]
    [OutputType(typeof(FileUri))]
    public class GetFsPathRootCommand : FsPathCommand
    {
        public const string PARAMETER_SET_NAME_ASSUME_URI = "AssumeUri";
        public const string PARAMETER_SET_NAME_ASSUME_LOCAL = "AssumeLocal";
        public const string PARAMETER_SET_NAME_PATH = "Path";
        public const string PARAMETER_SET_NAME_URI = "Uri";
        private const string HELP_MESSAGE_FROM_FILE_URI = "Gets the root path URI of the volume contained by the specified path URI.";
        private const string HELP_MESSAGE_FROM_LOCAL_PATH = "Gets the root path URI of the volume contained by the specified local path.";
        private string _paramName;

        [Parameter(HelpMessage = HELP_MESSAGE_FROM_FILE_URI, Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_ASSUME_URI)]
        [Parameter(HelpMessage = HELP_MESSAGE_FROM_LOCAL_PATH, Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_ASSUME_LOCAL)]
        [ValidateNotNullOrEmpty()]
        public object[] InputObject { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_FROM_LOCAL_PATH, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_PATH)]
        [Alias("FullName", "Path")]
        [ValidateNotNullOrEmpty()]
        public object[] LocalPath { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_FROM_FILE_URI, Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_URI)]
        [Alias("Url", "Address")]
        [ValidateNotNullOrEmpty()]
        public object[] Uri { get; set; }

        [Parameter(HelpMessage = "Assumes string values represent local path strings.", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_ASSUME_LOCAL)]
        [Alias("Url", "Address")]
        [ValidateNotNullOrEmpty()]
        public SwitchParameter AssumeLocalPath { get; set; }

        [Parameter(HelpMessage = "Assumes string values represent URI strings.", ParameterSetName = PARAMETER_SET_NAME_ASSUME_URI)]
        [Alias("Url", "Address")]
        [ValidateNotNullOrEmpty()]
        public SwitchParameter AssumeUri { get; set; }

        protected override void BeginProcessing()
        {
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_ASSUME_LOCAL:
                    _paramName = nameof(InputObject);
                    SetConversionMessageId(MessageId.InvalidPath);
                    break;
                case PARAMETER_SET_NAME_URI:
                    _paramName = nameof(Uri);
                    SetConversionMessageId(MessageId.InvalidAbsoluteFileUri);
                    break;
                case PARAMETER_SET_NAME_PATH:
                    _paramName = nameof(LocalPath);
                    SetConversionMessageId(MessageId.InvalidPath);
                    break;
                default:
                    _paramName = nameof(InputObject);
                    SetConversionMessageId(MessageId.InvalidAbsoluteFileUri);
                    break;
            }
        }

        protected override void ProcessRecord()
        {
            FileUri fileUri;
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_ASSUME_LOCAL:
                    foreach (object obj in InputObject)
                        ProcessItem(obj, CoersionHelper.TryCoerceToFileUri(obj, true, out fileUri) ? fileUri : null);
                    break;
                case PARAMETER_SET_NAME_URI:
                    foreach (object obj in Uri)
                        ProcessItem(obj, CoersionHelper.TryCoerceToFileUri(obj, false, out fileUri) ? fileUri : null);
                    break;
                case PARAMETER_SET_NAME_PATH:
                    foreach (object obj in LocalPath)
                        ProcessItem(obj, CoersionHelper.TryCoerceToFileUri(obj, true, out fileUri) ? fileUri : null);
                    break;
                default:
                    foreach (object obj in InputObject)
                        ProcessItem(obj, CoersionHelper.TryCoerceToFileUri(obj, false, out fileUri) ? fileUri : null);
                    break;
            }
        }

        private void ProcessItem(object source, FileUri fileUri)
        {
            if (!ValidateItem(source, fileUri, _paramName))
                return;


            throw new NotImplementedException();
        }
    }
}
