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

        private Collection<PSObject> _volumeInfos;

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
                    FileUri[] uris = RootPathName.Select(p => new FileUri(new DirectoryInfo(p))).Distinct().ToArray();
                    if (uris.Length == 1)
                    {
                        FileUri u = uris[0];
                        matching = _volumeInfos.WhereBaseObjectOf<RegisteredVolumeInfo>(f => f.RootUri.Equals(u, f.SegmentNameComparer));
                    }
                    else if (uris.Length > 1)
                        matching = _volumeInfos.WhereBaseObjectOf<RegisteredVolumeInfo>(f => uris.Any(u => f.RootUri.Equals(u, f.SegmentNameComparer)));
                    else
                        matching = new PSObject[0];
                    break;
                case PARAMETER_SET_NAME_BY_VOLUME_NAME:
                    string[] names = VolumeName.Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray();
                    StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
                    if (names.Length == 1)
                    {
                        string n = names[0];
                        matching = _volumeInfos.WhereBaseObjectOf<RegisteredVolumeInfo>(f => comparer.Equals(f.VolumeName, n));
                    }
                    else if (names.Length > 1)
                        matching = _volumeInfos.WhereBaseObjectOf<RegisteredVolumeInfo>(f => names.Any(n => comparer.Equals(f.VolumeName, n)));
                    else
                        matching = new PSObject[0];
                    break;
                case PARAMETER_SET_NAME_BY_IDENTIFIER:
                    VolumeIdentifier[] identifiers = Identifier.Select(o => (o is PSObject psObj) ? psObj.BaseObject : o).Select(o =>
                        (VolumeIdentifier.TryCreate(o, out VolumeIdentifier volumeIdentifer)) ? (object)volumeIdentifer : null
                    ).OfType<VolumeIdentifier>().Distinct().ToArray();
                    if (identifiers.Length == 1)
                    {
                        VolumeIdentifier id = identifiers[0];
                        matching = _volumeInfos.WhereBaseObjectOf<RegisteredVolumeInfo>(f => f.Identifier.Equals(id));
                    }
                    else if (identifiers.Length > 1)
                        matching = _volumeInfos.WhereBaseObjectOf<RegisteredVolumeInfo>(f => identifiers.Any(id => f.Identifier.Equals(id)));
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
