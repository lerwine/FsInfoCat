using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.IO;
using System.Management.Automation;
using static FsInfoCat.PS.VolumeInfoRegistration;

namespace FsInfoCat.PS.Commands
{
    // Register-FsVolumeInfo
    [Cmdlet(VerbsLifecycle.Register, "FsVolumeInfo")]
    [OutputType(typeof(IVolumeInfo))]
    public class RegisterFsVolumeInfoCommand : FsVolumeInfoCommand
    {
        [Parameter(HelpMessage = "The full path name of the volume root directory.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        [Alias("RootPath", "FullName")]
        public string RootPathName { get; set; }

        [Parameter(HelpMessage = "The name of the file system volume.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string VolumeName { get; set; }

        /// <summary>
        /// File System drive format name.
        /// </summary>
        [Parameter(HelpMessage = "The name of the drive format.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string DriveFormat { get; set; }

        /// <summary>
        /// Unique identifier for the file system volume.
        /// </summary>
        /// <remarks>
        /// The value provided to this parameter can be on of the following value types:
        /// <list type="bullet">
        /// <item><term>VSN</term>: An unsigned integer value of the Volume Serial Number.</item>
        /// <item><term>UUID</term>: A <seealso cref="Guid"/> object representing the 128-bit volume UUID.</item>
        /// <item><term><seealso cref="VolumeIdentifier"/></term>: Can representing either a VSN or a Volume UUID.</item>
        /// <item><term>Formatted String</term>: Formatted hexidecimal string that can be parsed as a VSN or Volume UUID.
        /// See <seealso cref="VolumeIdentifier"/> for more information on supported string formats.</item>
        /// </list>
        /// </remarks>
        [Parameter(HelpMessage = "The volume VSN or UUID that will be used as the unique identifier for the file system volume. For remote file shares, this should be the URN of the remote source.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Alias("SerialNumber", "VolumeId", "VSN", "UUID")]
        public object Identifier { get; set; }

        [Parameter(HelpMessage = "File name/path lookups are case-sensitive.")]
        public SwitchParameter CaseSensitive { get; set; }

        [Parameter(HelpMessage = "Return registered volume information object.")]
        public SwitchParameter PassThru { get; set; }

        [Parameter(HelpMessage = "Updates volume info event if it has already been been registered. Also registers volume information even if the subdirectory does not exist.")]
        public SwitchParameter Force { get; set; }

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
            if (!VolumeIdentifier.TryCreate((Identifier is PSObject psObj) ? psObj.BaseObject : Identifier, out VolumeIdentifier volumeIdentifer))
            {
                WriteError(MessageId.InvalidIdentifier.ToArgumentOutOfRangeError(nameof(Identifier), Identifier));
                return;
            }

            DirectoryInfo directoryInfo;
            FileUri fileUri;
            try
            {
                directoryInfo = new DirectoryInfo(GetUnresolvedProviderPathFromPSPath(RootPathName));
                fileUri = new FileUri(directoryInfo);
            }
            catch (Exception exc)
            {
                WriteError(MessageId.UnexpectedError.ToErrorRecord(exc, ErrorCategory.ReadError, nameof(RootPathName), RootPathName));
                return;
            }
            if (!(Force.IsPresent || directoryInfo.Exists))
            {
                WriteError(MessageId.PathNotFound.ToArgumentOutOfRangeError(nameof(RootPathName), RootPathName));
                return;
            }

            VolumeInfoRegistration volumeRegistration = GetVolumeRegistration(SessionState);
            if (volumeRegistration.TryGetValue(volumeIdentifer, out RegisteredVolumeItem volumeItem))
            {
                RegisteredVolumeInfo volumeInfo = (RegisteredVolumeInfo)volumeItem.BaseObject;
                if (!Force.IsPresent)
                {
                    WriteError(MessageId.VolumeIdAlreadyRegistered.ToArgumentOutOfRangeError(nameof(Identifier), Identifier));
                    return;
                }
                string inputUriString = fileUri.ToString();
                string actualUriString = volumeInfo.RootUri.ToString();
                if (!(inputUriString == actualUriString && CaseSensitive == volumeInfo.CaseSensitive && VolumeName == volumeInfo.VolumeName &&
                    DriveFormat == volumeInfo.DriveFormat))
                {
                    if (inputUriString != actualUriString && volumeRegistration.TryFindByRootURI(fileUri, out RegisteredVolumeItem matching) &&
                        !ReferenceEquals(matching, volumeInfo))
                    {
                        WriteError(MessageId.DirectoryRootAlreadyRegistered.ToArgumentOutOfRangeError(nameof(RootPathName), RootPathName));
                        return;
                    }
                    if (VolumeName != volumeInfo.VolumeName && volumeRegistration.TryFindByName(VolumeName, out matching) && !ReferenceEquals(matching, volumeInfo))
                    {
                        WriteError(MessageId.VolumeNameAlreadyRegistered.ToArgumentOutOfRangeError(nameof(VolumeName), VolumeName));
                        return;
                    }
                    volumeRegistration.Remove(volumeItem);
                    volumeItem = new RegisteredVolumeItem(new RegisteredVolumeInfo(fileUri, volumeIdentifer, VolumeName,
                        CaseSensitive, DriveFormat));
                    volumeRegistration.Add(volumeItem);
                }
            }
            else
            {
                if (volumeRegistration.ContainsRootUri(fileUri))
                {
                    WriteError(MessageId.DirectoryRootAlreadyRegistered.ToArgumentOutOfRangeError(nameof(RootPathName), RootPathName));
                    return;
                }
                if (volumeRegistration.ContainsVolumeName(VolumeName))
                {
                    WriteError(MessageId.VolumeNameAlreadyRegistered.ToArgumentOutOfRangeError(nameof(VolumeName), VolumeName));
                    return;
                }
                volumeItem = new RegisteredVolumeItem(new RegisteredVolumeInfo(fileUri, volumeIdentifer, VolumeName,
                    CaseSensitive, DriveFormat));
                volumeRegistration.Add(volumeItem);
            }
            if (PassThru.IsPresent)
                WriteObject(volumeItem);
        }
    }
}
