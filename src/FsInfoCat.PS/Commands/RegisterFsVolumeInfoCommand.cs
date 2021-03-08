using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
            throw new NotImplementedException();
        }

        protected override void OnPathIsFileError(string providerPath)
        {
            throw new NotImplementedException();
        }

        protected override void OnProviderNotSupportedException(string path, Exception exc)
        {
            throw new NotImplementedException();
        }

        protected override void OnResolveError(string path, Exception exc)
        {
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
                // TODO: Need to use GetUnresolvedProviderPathFromPSPath on RootPathName
                directoryInfo = new DirectoryInfo(RootPathName);
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

            Collection<PSObject> volumeRegistration = GetVolumeRegistration();
            IEnumerable<RegisteredVolumeInfo> registeredVolumes = volumeRegistration.Select(o => o.BaseObject as RegisteredVolumeInfo);
            RegisteredVolumeInfo volumeInfo = registeredVolumes.FirstOrDefault(v => v.Identifier.Equals(volumeIdentifer));
            StringComparer ignoreCaseComparer = StringComparer.InvariantCultureIgnoreCase;
            string uriString = fileUri.ToString();
            if (volumeInfo is null)
            {
                IEnumerable<RegisteredVolumeInfo> matching = registeredVolumes.Where(v => v.RootUri.Equals(fileUri, v.PathComparer));
                if (matching.Any())
                {
                    WriteError(MessageId.DirectoryRootAlreadyRegistered.ToArgumentOutOfRangeError(nameof(RootPathName), RootPathName));
                    return;
                }
                matching = registeredVolumes.Where(v => ignoreCaseComparer.Equals(v.VolumeName, VolumeName));
                if (matching.Any())
                {
                    WriteError(MessageId.VolumeNameAlreadyRegistered.ToArgumentOutOfRangeError(nameof(VolumeName), VolumeName));
                    return;
                }
                volumeInfo = new RegisteredVolumeInfo
                {
                    CaseSensitive = CaseSensitive.IsPresent,
                    Identifier = volumeIdentifer,
                    RootUri = fileUri,
                    VolumeName = VolumeName,
                    DriveFormat = DriveFormat
                };
                volumeRegistration.Add(PSObject.AsPSObject(volumeInfo));
            }
            else
            {
                if (!Force.IsPresent)
                {
                    WriteError(MessageId.VolumeIdAlreadyRegistered.ToArgumentOutOfRangeError(nameof(Identifier), Identifier));
                    return;
                }
                IEnumerable<RegisteredVolumeInfo> matching = registeredVolumes.Where(v => !ReferenceEquals(volumeInfo, v) && v.RootUri.Equals(fileUri, v.PathComparer));
                if (matching.Any())
                {
                    WriteError(MessageId.DirectoryRootAlreadyRegistered.ToArgumentOutOfRangeError(nameof(RootPathName), RootPathName));
                    return;
                }
                matching = registeredVolumes.Where(v => !ReferenceEquals(volumeInfo, v) && ignoreCaseComparer.Equals(v.VolumeName, VolumeName));
                if (matching.Any())
                {
                    WriteError(MessageId.VolumeNameAlreadyRegistered.ToArgumentOutOfRangeError(nameof(VolumeName), VolumeName));
                    return;
                }
                volumeInfo.CaseSensitive = CaseSensitive.IsPresent;
                volumeInfo.RootUri = fileUri;
                volumeInfo.VolumeName = VolumeName;
                volumeInfo.DriveFormat = DriveFormat;
            }
            if (PassThru.IsPresent)
                WriteObject(volumeInfo);
        }
    }
}
