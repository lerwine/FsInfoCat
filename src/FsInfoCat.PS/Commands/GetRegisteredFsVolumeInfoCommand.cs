using System.Collections.Generic;
using System.Management.Automation;
using FsInfoCat.Models.Volumes;

namespace FsInfoCat.PS.Commands
{
    // Register-FsVolumeInfo
    [Cmdlet(VerbsCommon.Get, "RegisteredFsVolumeInfo")]
    [OutputType(typeof(IVolumeInfo))]
    public class GetRegisteredFsVolumeInfoCommand : FsVolumeInfoCommand
    {
        [Parameter(HelpMessage = "Find by full, case-sensitive path name of the volume root directory.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string[] RootPathName { get; set; }

        [Parameter(HelpMessage = "Find by name of the volume.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string[] VolumeName { get; set; }

        [Parameter(HelpMessage = "Find by name of the drive format.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string[] DriveFormat { get; set; }

        [Parameter(HelpMessage = "Find by volume serial number.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public uint[] SerialNumber { get; set; }

        [Parameter(HelpMessage = "Get all registered volumes", Mandatory = true)]
        public SwitchParameter All { get; set; }

        protected override void ProcessRecord()
        {
            IEnumerable<IVolumeInfo> volumeInfos = GetVolumeInfos();
        }
    }
}
