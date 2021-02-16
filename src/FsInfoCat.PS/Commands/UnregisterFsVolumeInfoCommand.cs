using System.Collections.Generic;
using System.Management.Automation;
using FsInfoCat.Models.Volumes;

namespace FsInfoCat.PS.Commands
{
    // Unregister-FsVolumeInfo
    [Cmdlet(VerbsLifecycle.Unregister, "FsVolumeInfo", DefaultParameterSetName = PARAMETER_SET_NAME_VOLUME_INFO)]
    public class UnregisterFsVolumeInfoCommand : FsVolumeInfoCommand
    {
        public const string PARAMETER_SET_NAME_VOLUME_IDENTIFIER = "VolumeIdentifier";
        public const string PARAMETER_SET_NAME_ROOT_PATH = "RootPath";
        public const string PARAMETER_SET_NAME_VOLUME_INFO = "VolumeInfo";

        [Parameter(HelpMessage = "The full, case-sensitive path name of the volume root directory.", Mandatory = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_ROOT_PATH)]
        [ValidateNotNullOrEmpty()]
        public string[] RootPathName { get; set; }

        // TODO: Use Identifier instead of SerialNumber
#warning Use Identifier instead of SerialNumber
        [Parameter(HelpMessage = "The volume serial number.", Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_VOLUME_IDENTIFIER)]
        public uint[] SerialNumber { get; set; }

        [Parameter(HelpMessage = "The volume serial number or UUID.", Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_VOLUME_IDENTIFIER)]
        [Alias("SerialNumber")]
        public object[] Identifier { get; set; }

        [Parameter(HelpMessage = "The volume information object.", Mandatory = true, ValueFromPipeline = true,
            ParameterSetName = PARAMETER_SET_NAME_VOLUME_INFO)]
        public IVolumeInfo[] VolumeInfo { get; set; }

        protected override void ProcessRecord()
        {
            IEnumerable<IVolumeInfo> volumeInfos = GetVolumeInfos();
            // TODO: Implement Unregister-FsVolumeInfo
#warning Implement Unregister-FsVolumeInfo
        }
    }
}
