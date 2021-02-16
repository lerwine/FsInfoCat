using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using FsInfoCat.Models.Volumes;

namespace FsInfoCat.PS.Commands
{
    // Register-FsVolumeInfo
    [Cmdlet(VerbsCommon.Get, "RegisteredFsVolumeInfo", DefaultParameterSetName = PARAMETER_SET_NAME_GET_ALL)]
    [OutputType(typeof(IVolumeInfo))]
    public class GetRegisteredFsVolumeInfoCommand : FsVolumeInfoCommand
    {
        public const string PARAMETER_SET_NAME_BY_ROOT_DIRECTORY = "ByRootDirectory";
        public const string PARAMETER_SET_NAME_BY_VOLUME_NAME = "ByVolumeName";
        public const string PARAMETER_SET_NAME_BY_DRIVE_FORMAT = "ByDriveFormat";
        public const string PARAMETER_SET_NAME_BY_IDENTIFIER = "ByIdentifier";
        public const string PARAMETER_SET_NAME_GET_ALL = "GetAll";

        [Parameter(HelpMessage = "Find by full, case-sensitive path name of the volume root directory.", Mandatory = true,
            ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_BY_ROOT_DIRECTORY)]
        [Alias("FullName", "Path")]
        [ValidateNotNullOrEmpty()]
        public string[] RootPathName { get; set; }

        [Parameter(HelpMessage = "Find by name of the volume.", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_VOLUME_NAME)]
        [ValidateNotNullOrEmpty()]
        public string[] VolumeName { get; set; }

        [Parameter(HelpMessage = "Find by name of the drive format.", Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_DRIVE_FORMAT)]
        [ValidateNotNullOrEmpty()]
        public string[] DriveFormat { get; set; }

        [Parameter(HelpMessage = "Find by volume identifier.", Mandatory = true, ValueFromPipelineByPropertyName = true,
            ParameterSetName = PARAMETER_SET_NAME_BY_IDENTIFIER)]
        [Alias("SerialNumber")]
        public object[] Identifier { get; set; }

        [Parameter(HelpMessage = "Get all registered volumes", ParameterSetName = PARAMETER_SET_NAME_GET_ALL)]
        public SwitchParameter All { get; set; }

        private Collection<IVolumeInfo> _volumeInfos;
        protected override void BeginProcessing()
        {
            _volumeInfos = new Collection<IVolumeInfo>();
            foreach (IVolumeInfo v in GetVolumeInfos())
                _volumeInfos.Add(v);
        }

        protected override void ProcessRecord()
        {
            if (_volumeInfos.Count == 0)
                return;
            IEnumerable<IVolumeInfo> matching;
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_BY_ROOT_DIRECTORY:
                    matching = _volumeInfos.Where(v => RootPathName.Any(p => p == v.RootPathName));
                    break;
                case PARAMETER_SET_NAME_BY_VOLUME_NAME:
                    matching = _volumeInfos.Where(v => VolumeName.Any(n => n.Equals(v.VolumeName, StringComparison.InvariantCultureIgnoreCase)));
                    break;
                case PARAMETER_SET_NAME_BY_DRIVE_FORMAT:
                    matching = _volumeInfos.Where(v => DriveFormat.Any(f => f.Equals(v.DriveFormat, StringComparison.InvariantCultureIgnoreCase)));
                    break;
                case PARAMETER_SET_NAME_BY_IDENTIFIER:
                    VolumeIdentifier[] identifiers = Identifier.Select(o => (o is PSObject) ? ((PSObject)o).BaseObject : o).Select(o =>
                        (VolumeIdentifier.TryCreate(o, out VolumeIdentifier volumeIdentifer)) ? (object)volumeIdentifer : null
                    ).OfType<VolumeIdentifier>().ToArray();
                    matching = (identifiers.Length == 0) ? new IVolumeInfo[0] : _volumeInfos.Where(v => identifiers.Any(i => v.Identifier.Equals(i)));
                    break;
                default:
                    foreach (IVolumeInfo v in GetVolumeInfos())
                        WriteObject(v);
                    return;
            }
            foreach (IVolumeInfo v in matching.ToArray())
            {
                _volumeInfos.Remove(v);
                WriteObject(v);
            }
        }
    }
}
