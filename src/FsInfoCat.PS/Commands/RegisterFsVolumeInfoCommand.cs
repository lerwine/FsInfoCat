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
        [Parameter(HelpMessage = "The full, case-sensitive path name of the volume root directory.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string RootPathName { get; set; }

        [Parameter(HelpMessage = "The name of the volume.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
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
        [Parameter(HelpMessage = "The volume serial number or UUID.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Alias("SerialNumber")]
        public object Identifier { get; set; }

        [Parameter(HelpMessage = "Do case-sensitive path matching.")]
        public SwitchParameter CaseSensitive { get; set; }

        [Parameter(HelpMessage = "Return registered volume")]
        public SwitchParameter PassThru { get; set; }

        [Parameter(HelpMessage = "Updates volume info if it has already been been registered. Also registers volume information even if the subdirectory does not exists.")]
        public SwitchParameter Force { get; set; }

        protected override void ProcessRecord()
        {
            if (!(VolumeIdentifier.TryCreate((Identifier is PSObject psObj) ? psObj.BaseObject : Identifier, out VolumeIdentifier volumeIdentifer)))
            {
                // TODO: Write error
            }

            DirectoryInfo directoryInfo;
            try { directoryInfo = new DirectoryInfo(RootPathName); }
            catch (Exception exc)
            {
                // TODO: Write error
                return;
            }
            if (!(Force.IsPresent || directoryInfo.Exists))
            {
                // TODO: Write error
            }
            FileUri fileUri = new FileUri(directoryInfo);

            Collection<PSObject> volumeRegistration = GetVolumeRegistration();
            IEnumerable<RegisteredVolumeInfo> registeredVolumes = volumeRegistration.Select(o => o.BaseObject as RegisteredVolumeInfo);
            RegisteredVolumeInfo volumeInfo = registeredVolumes.FirstOrDefault(v => v.Identifier.Equals(volumeIdentifer));
            StringComparer ignoreCaseComparer = StringComparer.InvariantCultureIgnoreCase;
            string uriString = fileUri.ToString();
            if (volumeInfo is null)
            {
                IEnumerable<RegisteredVolumeInfo> matching = registeredVolumes.Where(v => !ReferenceEquals(volumeInfo, v) && v.RootUri.Equals(fileUri, v.SegmentNameComparer));
                if (matching.Any())
                {
                    // TODO: Write error
                }
                matching = registeredVolumes.Where(v => !ReferenceEquals(volumeInfo, v) && ignoreCaseComparer.Equals(v.VolumeName, VolumeName));
                if (matching.Any())
                {
                    // TODO: Write error
                }
                volumeInfo = new RegisteredVolumeInfo
                {
                    CaseSensitive = CaseSensitive.IsPresent,
                    Identifier = volumeIdentifer,
                    RootUri = fileUri,
                    VolumeName = VolumeName,
                    DriveFormat = DriveFormat
                };
            }
            else
            {
                if (!Force.IsPresent)
                {
                    // TODO: Write error
                }
                IEnumerable<RegisteredVolumeInfo> matching = registeredVolumes.Where(v => v.RootUri.Equals(fileUri, v.SegmentNameComparer));
                if (matching.Any())
                {
                    // TODO: Write error
                }
                matching = registeredVolumes.Where(v => ignoreCaseComparer.Equals(v.VolumeName, VolumeName));
                if (matching.Any())
                {
                    // TODO: Write error
                }
                volumeInfo.CaseSensitive = CaseSensitive.IsPresent;
                volumeInfo.RootUri = fileUri;
                volumeInfo.VolumeName = VolumeName;
                volumeInfo.DriveFormat = DriveFormat;
            }
            if (!(volumeInfo is null) && PassThru.IsPresent)
                WriteObject(volumeInfo);
        }
    }
}
