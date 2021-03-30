using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using static FsInfoCat.PS.VolumeInfoRegistration;

namespace FsInfoCat.PS.Commands
{
    // Unregister-FsVolumeInfo
    [Cmdlet(VerbsLifecycle.Unregister, "FsVolumeInfo", DefaultParameterSetName = PARAMETER_SET_NAME_VOLUME_INFO)]
    public class UnregisterFsVolumeInfoCommand : FsVolumeInfoCommand
    {
        public const string PARAMETER_SET_NAME_VOLUME_IDENTIFIER = "VolumeIdentifier";
        public const string PARAMETER_SET_NAME_ROOT_PATH = "RootPath";
        public const string PARAMETER_SET_NAME_VOLUME_INFO = "VolumeInfo";
        private const string HELP_MESSAGE_PASSTHRU = "Return the IVolumeInfo object of the un-registered volume.";

        [Parameter(HelpMessage = "The full path name of the root directory for the volume to un-register. Case-sensitivity is dependent upon the volume registration.", Mandatory = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_ROOT_PATH)]
        [ValidateNotNullOrEmpty()]
        [Alias("RootPath", "FullName")]
        public string[] RootPathName { get; set; }

        [Parameter(HelpMessage = "The volume UUID or VSN to un-register. For remote file shares, this should be the URN of the remote source. This is not case-senstive.", Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_VOLUME_IDENTIFIER)]
        [Alias("SerialNumber", "VolumeId", "VSN", "UUID")]
        public object[] Identifier { get; set; }

        [Parameter(HelpMessage = "The volume information object to un-register.", Mandatory = true, ValueFromPipeline = true,
            ParameterSetName = PARAMETER_SET_NAME_VOLUME_INFO)]
        public IVolumeInfo[] VolumeInfo { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_PASSTHRU, ParameterSetName = PARAMETER_SET_NAME_VOLUME_IDENTIFIER)]
        [Parameter(HelpMessage = HELP_MESSAGE_PASSTHRU, ParameterSetName = PARAMETER_SET_NAME_ROOT_PATH)]
        public SwitchParameter PassThru { get; set; }

        /// <summary>
        /// This gets called whenever a non-existent-path is encountered.
        /// </summary>
        /// <param name="path">the non-existent filesystem path.</param>
        /// <param name="exc">The exception that was thrown or <see langword="null"/> if existence validation failed without an exception being thrown.</param>
        /// <remarks>This gets called by <see cref="FsVolumeInfoCommand.ResolveDirectoryFromLiteralPath(System.Collections.Generic.IEnumerable{string})"/>, <see cref="FsVolumeInfoCommand.ResolveDirectoryFromWcPath(string)"/>
        /// and <see cref="FsVolumeInfoCommand.TryResolveDirectoryFromLiteralPath(string, out string)"/> when trying to resolve a path that does not exist.</remarks>
        protected override void OnItemNotFoundException(string path, ItemNotFoundException exc)
        {
            WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Subdirectory not found"), ErrorCategory.ObjectNotFound,
                nameof(RootPathName), path));
        }

        /// <summary>
        /// This gets called whenever a path is encountered with refers to a file rather than a subdirectory.
        /// </summary>
        /// <param name="path">The path to a file.</param>
        /// <remarks>This gets called by <see cref="FsVolumeInfoCommand.ResolveDirectoryFromLiteralPath(System.Collections.Generic.IEnumerable{string})"/>, <see cref="FsVolumeInfoCommand.ResolveDirectoryFromWcPath(string)"/>
        /// and <see cref="FsVolumeInfoCommand.TryResolveDirectoryFromLiteralPath(string, out string)"/> when a path was successfully resolved, but it did not refer to a subdirectory.</remarks>
        protected override void OnPathIsFileError(string path)
        {
            WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Path refers does not refer to a subdirectory"),
                ErrorCategory.ObjectNotFound, nameof(RootPathName), path));
        }

        /// <summary>
        /// This gets called whenever an invalid path string is enountered.
        /// </summary>
        /// <param name="path">The path string that could not be resolved.</param>
        /// <param name="exc">The exeption that was thrown.</param>
        /// <remarks>This gets called by <see cref="FsVolumeInfoCommand.ResolveDirectoryFromLiteralPath(System.Collections.Generic.IEnumerable{string})"/>, <see cref="FsVolumeInfoCommand.ResolveDirectoryFromWcPath(string)"/>
        /// and <see cref="FsVolumeInfoCommand.TryResolveDirectoryFromLiteralPath(string, out string)"/> when trying to resolve a path that is not supported by the local filesystem.</remarks>
        protected override void OnProviderNotSupportedException(string path, Exception exc)
        {
            WriteError(MessageId.InvalidPath.ToErrorRecord("Path references an unsupported provider type", exc, ErrorCategory.InvalidArgument, nameof(RootPathName), path));
        }

        /// <summary>
        /// This gets called when an unexpected exception is thrown while trying to resolve a wildcard-supported path string.
        /// </summary>
        /// <param name="path">The path string that could not be resolved.</param>
        /// <param name="exc">The exeption that was thrown.</param>
        /// <remarks>This gets called by <see cref="FsVolumeInfoCommand.ResolveDirectoryFromWcPath(string)"/> when an unexpected exception is thrown while trying to resolve a wildcard-supported path string.</remarks>
        protected override void OnResolveError(string path, Exception exc)
        {
            WriteError(MessageId.InvalidPath.ToErrorRecord(exc, nameof(RootPathName), path));
        }

        protected override void ProcessRecord()
        {
            VolumeInfoRegistration volumeRegistration = GetVolumeRegistration(SessionState);
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_ROOT_PATH:
                    foreach (string path in RootPathName.Select(p => GetUnresolvedProviderPathFromPSPath(p)))
                    {
                        FileUri fileUri;
                        try { fileUri = new FileUri(new DirectoryInfo(path)); }
                        catch (Exception exc)
                        {
                            WriteError(MessageId.UnexpectedError.ToErrorRecord(exc, ErrorCategory.ReadError, nameof(RootPathName), path));
                            continue;
                        }
                        if (volumeRegistration.TryGetValue(fileUri, out RegisteredVolumeItem item))
                        {
                            volumeRegistration.Remove(item);
                            if (PassThru.IsPresent)
                                WriteObject(item, false);
                        }
                        else
                            WriteWarning($"Root path '{path}' was not registered.");
                    }
                    break;
                case PARAMETER_SET_NAME_VOLUME_IDENTIFIER:
                    foreach (object id in Identifier)
                    {
                        if (VolumeIdentifier.TryCreate((id is PSObject psObj) ? psObj.BaseObject : Identifier, out VolumeIdentifier volumeIdentifer))
                        {
                            if (volumeRegistration.TryGetValue(volumeIdentifer, out RegisteredVolumeItem item))
                            {
                                volumeRegistration.Remove(item);
                                if (PassThru.IsPresent)
                                    WriteObject(item, false);
                            }
                            else
                                WriteWarning($"Volume identifier '{volumeIdentifer}' was not registered.");
                        }
                        else
                            WriteError(MessageId.InvalidIdentifier.ToArgumentOutOfRangeError(nameof(Identifier), Identifier));
                    }
                    break;
                default:
                    foreach (IVolumeInfo volume in VolumeInfo)
                    {
                        if (volumeRegistration.TryFindMatching(volume, out RegisteredVolumeItem item))
                        {
                            volumeRegistration.Remove(item);
                            if (PassThru.IsPresent)
                                WriteObject(item, false);
                        }
                        else
                            WriteWarning($"Volume '{volume}' was not registered.");
                    }
                    break;
            }

        }
    }
}
