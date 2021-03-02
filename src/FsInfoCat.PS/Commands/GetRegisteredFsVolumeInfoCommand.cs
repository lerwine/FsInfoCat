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
    [Cmdlet(VerbsCommon.Get, "RegisteredFsVolumeInfo", DefaultParameterSetName = PARAMETER_SET_NAME_GET_ALL)]
    [OutputType(typeof(IVolumeInfo))]
    public class GetRegisteredFsVolumeInfoCommand : FsVolumeInfoCommand
    {
        public const string PARAMETER_SET_NAME_BY_ROOT_DIRECTORY = "ByRootDirectory";
        public const string PARAMETER_SET_NAME_BY_VOLUME_NAME = "ByVolumeName";
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

        [Parameter(HelpMessage = "Find by volume identifier.", Mandatory = true, ValueFromPipelineByPropertyName = true,
            ParameterSetName = PARAMETER_SET_NAME_BY_IDENTIFIER)]
        [Alias("SerialNumber")]
        public object[] Identifier { get; set; }

        [Parameter(HelpMessage = "Get all registered volumes", ParameterSetName = PARAMETER_SET_NAME_GET_ALL)]
        public SwitchParameter All { get; set; }

        [Parameter(HelpMessage = "Do case-sensitive path matching.", ParameterSetName = PARAMETER_SET_NAME_BY_ROOT_DIRECTORY)]
        public SwitchParameter CaseSensitive { get; set; }

        private Collection<PSObject> _volumeInfos;
        protected override void BeginProcessing()
        {
            _volumeInfos = GetVolumeRegistration();
        }

        protected override void ProcessRecord()
        {
            if (_volumeInfos.Count == 0)
                return;
            IEnumerable<PSObject> matching;
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_BY_ROOT_DIRECTORY:
                    FileUri[] uris = RootPathName.Select(p => FileUri.FromFileSystemInfo(new DirectoryInfo(p))).Where(f => f.IsAbsolute).Distinct().ToArray();
                    if (uris.Length == 1)
                        matching = _volumeInfos.FindVolumeByRootUri<RegisteredVolumeInfo>(uris[0]);
                    else if (uris.Length > 1)
                        matching = _volumeInfos.FindVolumeByRootUri<RegisteredVolumeInfo>(uris);
                    else
                        matching = new PSObject[0];
                    break;
                case PARAMETER_SET_NAME_BY_VOLUME_NAME:
                    string[] n = VolumeName.Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray();
                    if (n.Length == 1)
                        matching = _volumeInfos.FindVolumeByVolumeName<RegisteredVolumeInfo>(n[0]);
                    else if (n.Length > 1)
                        matching = _volumeInfos.FindVolumeByVolumeName<RegisteredVolumeInfo>(n);
                    else
                        matching = new PSObject[0];
                    break;
                case PARAMETER_SET_NAME_BY_IDENTIFIER:
                    VolumeIdentifier[] identifiers = Identifier.Select(o => (o is PSObject psObj) ? psObj.BaseObject : o).Select(o =>
                        (VolumeIdentifier.TryCreate(o, out VolumeIdentifier volumeIdentifer)) ? (object)volumeIdentifer : null
                    ).OfType<VolumeIdentifier>().Distinct().ToArray();
                    if (identifiers.Length == 1)
                        matching = _volumeInfos.FindVolumeByIdentifier<RegisteredVolumeInfo>(identifiers[0]);
                    else if (identifiers.Length > 1)
                        matching = _volumeInfos.FindVolumeByIdentifier<RegisteredVolumeInfo>(identifiers);
                    else
                        matching = new PSObject[0];
                    break;
                default:
                    foreach (IVolumeInfo v in _volumeInfos)
                        WriteObject(v);
                    return;
            }
            foreach (PSObject p in matching.ToArray())
            {
                _volumeInfos.Remove(p);
                WriteObject(p);
            }
        }
    }
}
