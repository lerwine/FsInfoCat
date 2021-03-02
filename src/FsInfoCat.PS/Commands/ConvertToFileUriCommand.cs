using FsInfoCat.Models.Crawl;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace FsInfoCat.PS.Commands
{
    // ConvertTo-FileUri
    [Cmdlet(VerbsData.ConvertTo, "FileUri")]
    [OutputType(typeof(FileUri))]
    public class ConvertToFileUriCommand : PSCmdlet
    {
        public const string PARAMETER_SET_NAME_ASSUME_URI = "AssumeUri";
        public const string PARAMETER_SET_NAME_ASSUME_LOCAL = "AssumeLocal";
        public const string PARAMETER_SET_NAME_PATH = "Path";
        public const string PARAMETER_SET_NAME_URI = "Uri";
        private const string HELP_MESSAGE_FROM_FILE_URI = "Gets the root path URI of the volume contained by the specified path URI.";
        private const string HELP_MESSAGE_FROM_LOCAL_PATH = "Gets the root path URI of the volume contained by the specified local path.";
        private const string ERROR_MESSAGE_INVALID_FILE_URI = "Invalid file URI.";
        private const string ERROR_MESSAGE_INVALID_PATH_STRING = "Invalid path string.";
        private string _paramName;
        private string _conversionErrorMessage;
        private string _conversionErrorId;

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
            string errorId;
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_ASSUME_LOCAL:
                    _paramName = nameof(InputObject);
                    _conversionErrorMessage = MessageId.InvalidPath.GetDescription(out errorId);
                    break;
                case PARAMETER_SET_NAME_URI:
                    _paramName = nameof(Uri);
                    _conversionErrorMessage = MessageId.InvalidAbsoluteFileUri.GetDescription(out errorId);
                    break;
                case PARAMETER_SET_NAME_PATH:
                    _paramName = nameof(LocalPath);
                    _conversionErrorMessage = MessageId.InvalidPath.GetDescription(out errorId);
                    break;
                default:
                    _paramName = nameof(InputObject);
                    _conversionErrorMessage = MessageId.InvalidAbsoluteFileUri.GetDescription(out errorId);
                    break;
            }
            _conversionErrorId = errorId;
        }

        protected override void ProcessRecord()
        {
            FileUri fileUri;
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_ASSUME_LOCAL:
                    foreach (object obj in InputObject)
                        ProcessItem(obj, obj.TryCoerceToFileUri(true, out fileUri) ? fileUri : null);
                    break;
                case PARAMETER_SET_NAME_URI:
                    foreach (object obj in Uri)
                        ProcessItem(obj, obj.TryCoerceToFileUri(false, out fileUri) ? fileUri : null);
                    break;
                case PARAMETER_SET_NAME_PATH:
                    foreach (object obj in LocalPath)
                        ProcessItem(obj, obj.TryCoerceToFileUri(true, out fileUri) ? fileUri : null);
                    break;
                default:
                    foreach (object obj in InputObject)
                        ProcessItem(obj, obj.TryCoerceToFileUri(false, out fileUri) ? fileUri : null);
                    break;
            }
        }

        private void ProcessItem(object source, FileUri fileUri)
        {
            try
            {
                if (fileUri is null || !fileUri.IsAbsolute)
                    throw new PSArgumentOutOfRangeException(_paramName, source, _conversionErrorMessage);
                WriteObject(fileUri);
            }
            catch (PSArgumentOutOfRangeException exc)
            {
                if (_paramName.Equals(exc.ParamName) && ReferenceEquals(source, exc.ActualValue))
                    WriteError(new ErrorRecord(exc, _conversionErrorId, ErrorCategory.InvalidArgument, source));
                else
                {
                    ErrorRecord errorRecord = exc.ErrorRecord;
                    if (errorRecord is null)
                        WriteError(new ErrorRecord(exc, MessageId.UnexpectedError.ToString("F"), ErrorCategory.InvalidArgument, exc.ActualValue));
                    else
                        WriteError(new ErrorRecord(exc, errorRecord.FullyQualifiedErrorId, errorRecord.CategoryInfo.Category, errorRecord.TargetObject));
                }
            }
            catch (Exception exc)
            {
                ErrorRecord errorRecord = (exc is IContainsErrorRecord ce) ? ce.ErrorRecord : null;
                if (errorRecord is null)
                    WriteError(new ErrorRecord(exc, MessageId.UnexpectedError.ToString("F"), ErrorCategory.InvalidArgument, source));
                else
                    WriteError(new ErrorRecord(exc, errorRecord.FullyQualifiedErrorId, errorRecord.CategoryInfo.Category, errorRecord.TargetObject));
            }
        }
    }
}
