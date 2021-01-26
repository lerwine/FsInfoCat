using System.Collections.Generic;
using System.Management.Automation;
using FsInfoCat.Models.Volumes;

namespace FsInfoCat.PS.Commands
{
    // Unregister-FsVolumeInfo
    [Cmdlet(VerbsLifecycle.Unregister, "FsVolumeInfo", DefaultParameterSetName = PARAMETER_SET_NAME_SERIAL_NUMBER)]
    public class UnregisterFsVolumeInfoCommand : FsVolumeInfoCommand
    {
        private const string PARAMETER_SET_NAME_SERIAL_NUMBER = "SerialNumber";
        private const string PARAMETER_SET_NAME_ROOT_PATH = "RootPath";
        private const string PARAMETER_SET_NAME_VOLUME_INFO = "VolumeInfo";

        [Parameter(HelpMessage = "The full, case-sensitive path name of the volume root directory.", Mandatory = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_ROOT_PATH)]
        [ValidateNotNullOrEmpty()]
        public string[] RootPathName { get; set; }

        [Parameter(HelpMessage = "The volume serial number.", Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_SERIAL_NUMBER)]
        public uint[] SerialNumber { get; set; }

        [Parameter(HelpMessage = "The volume serial number.", Mandatory = true, ValueFromPipelineByPropertyName = true,
            ParameterSetName = PARAMETER_SET_NAME_VOLUME_INFO)]
        public IVolumeInfo[] VolumeInfo { get; set; }

        protected override void ProcessRecord()
        {
            IEnumerable<IVolumeInfo> volumeInfos = GetVolumeInfos();
        }
    }
}
