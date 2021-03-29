using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.IO;
using System.Management.Automation;

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

        protected override void OnItemNotFoundException(string path, ItemNotFoundException exc)
        {
            // TODO: Implement OnPathIsFileError
            throw new NotImplementedException();
        }

        protected override void OnPathIsFileError(string providerPath)
        {
            // TODO: Implement OnPathIsFileError
            throw new NotImplementedException();
        }

        protected override void OnProviderNotSupportedException(string path, Exception exc)
        {
            // TODO: Implement OnPathIsFileError
            throw new NotImplementedException();
        }

        protected override void OnResolveError(string path, Exception exc)
        {
            // TODO: Implement OnPathIsFileError
            throw new NotImplementedException();
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
            if (volumeRegistration.TryGetValue(volumeIdentifer, out VolumeInfoRegistration.RegisteredVolumeItem volumeItem))
            {
                VolumeInfoRegistration.RegisteredVolumeInfo volumeInfo = (VolumeInfoRegistration.RegisteredVolumeInfo)volumeItem.BaseObject;
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
                    if (inputUriString != actualUriString && volumeRegistration.TryFindByRootURI(fileUri, out VolumeInfoRegistration.RegisteredVolumeItem matching) &&
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
                    volumeItem = new VolumeInfoRegistration.RegisteredVolumeItem(new VolumeInfoRegistration.RegisteredVolumeInfo(fileUri, volumeIdentifer, VolumeName,
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
                volumeItem = new VolumeInfoRegistration.RegisteredVolumeItem(new VolumeInfoRegistration.RegisteredVolumeInfo(fileUri, volumeIdentifer, VolumeName,
                    CaseSensitive, DriveFormat));
                volumeRegistration.Add(volumeItem);
            }
            if (PassThru.IsPresent)
                WriteObject(volumeItem);
        }
    }
}
