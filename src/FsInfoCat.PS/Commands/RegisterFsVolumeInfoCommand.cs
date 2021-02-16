using System;
using System.Collections.Generic;
using System.Management.Automation;
using FsInfoCat.Models.Volumes;

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

        // TODO: Use Identifier instead of SerialNumber
#warning Use Identifier instead of SerialNumber
        [Parameter(HelpMessage = "The volume serial number.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public uint SerialNumber { get; set; }

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

        [Parameter(HelpMessage = "Return registered volume", Mandatory = true)]
        public SwitchParameter PassThru { get; set; }

        protected override void ProcessRecord()
        {
            IEnumerable<IVolumeInfo> volumeInfos = GetVolumeInfos();
        }
    }
}
