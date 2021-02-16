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

        [Parameter(HelpMessage = "The name of the drive format.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string DriveFormat { get; set; }

#warning Use Identifier instead of SerialNumber
        [Parameter(HelpMessage = "The volume serial number.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public uint SerialNumber { get; set; }

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
