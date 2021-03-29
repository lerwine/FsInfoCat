using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using static FsInfoCat.PS.VolumeInfoRegistration;

namespace FsInfoCat.PS.Commands
{
    // Register-FsVolumeInfo
    [Cmdlet(VerbsCommon.Get, "RegisteredFsVolumeInfo", DefaultParameterSetName = PARAMETER_SET_NAME_GET_ALL)]
    [OutputType(typeof(IVolumeInfo))]
    public class GetRegisteredFsVolumeInfoCommand : FsVolumeInfoCommand
    {
        public const string PARAMETER_SET_NAME_BY_PATH = "ByPath";
        public const string PARAMETER_SET_NAME_BY_LITERAL_PATH = "ByLiteralPath";
        public const string PARAMETER_SET_NAME_BY_ROOT_DIRECTORY = "ByRootDirectory";
        public const string PARAMETER_SET_NAME_BY_VOLUME_NAME = "ByVolumeName";
        public const string PARAMETER_SET_NAME_BY_IDENTIFIER = "ByIdentifier";
        public const string PARAMETER_SET_NAME_GET_ALL = "GetAll";

        private VolumeInfoRegistration _volumeInfos;

        [Parameter(HelpMessage = "Gets registered volume information for the volume that contains the specified path (wildcards accepted). Case-sensitivity is dependent upon the registered volume.",
            Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_NAME_BY_PATH)]
        [ValidateNotNullOrEmpty()]
        public string[] Path { get; set; }

        [Parameter(HelpMessage = "Gets registered volume information for the volume that contains the specified literal path. Case-sensitivity is dependent upon the registered volume.",
            Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_BY_LITERAL_PATH)]
        [Alias("FullName")]
        [ValidateNotNullOrEmpty()]
        public string[] LiteralPath { get; set; }

        [Parameter(HelpMessage = "Gets registered volume information that matches the specified full path name of the of the volume's root directory. Case-sensitivity is dependent upon the registered volume.",
            Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_BY_ROOT_DIRECTORY)]
        [ValidateNotNullOrEmpty()]
        [Alias("RootPath")]
        public string[] RootPathName { get; set; }

        [Parameter(HelpMessage = "Gets registered volume information that matches the specified volume name. This is not case-sensitive.",
            Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_BY_VOLUME_NAME)]
        [ValidateNotNullOrEmpty()]
        public string[] VolumeName { get; set; }

        [Parameter(HelpMessage = "Gets registered volume information that matches the specified volume guid / serial number. For remote file shares, this should be the URN of the remote source. This is not case-sensitive.",
            Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_BY_IDENTIFIER)]
        [Alias("SerialNumber", "VolumeId", "VSN", "UUID")]
        public object[] Identifier { get; set; }

        [Parameter(HelpMessage = "Gets all registered volumes. This is the default behavior if no other parameters are used.",
            ParameterSetName = PARAMETER_SET_NAME_GET_ALL)]
        public SwitchParameter All { get; set; }

        protected override void BeginProcessing()
        {
            _volumeInfos = GetVolumeRegistration(SessionState);
        }

        protected override void OnItemNotFoundException(string path, ItemNotFoundException exc)
        {
            // TODO: Implement OnItemNotFoundException
            throw new NotImplementedException();
        }

        protected override void OnPathIsFileError(string providerPath)
        {
            // TODO: Implement OnPathIsFileError
            throw new NotImplementedException();
        }

        protected override void OnProviderNotSupportedException(string path, Exception exc)
        {
            // TODO: Implement OnProviderNotSupportedException
            throw new NotImplementedException();
        }

        protected override void OnResolveError(string path, Exception exc)
        {
            // TODO: Implement OnResolveError
            throw new NotImplementedException();
        }

        protected override void ProcessRecord()
        {
            if (_volumeInfos.Count == 0)
                return;
            // Need to put this in Begin?
            Collection<IVolumeInfo> matching = new Collection<IVolumeInfo>();
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_BY_PATH:
                    foreach (DirectoryInfo directoryInfo in Path.SelectMany(p => ResolveDirectoryFromWcPath(p)).Select(p => new DirectoryInfo(p)).Distinct())
                    {
                        if (_volumeInfos.TryFind(new FileUri(directoryInfo), out IVolumeInfo volumeInfo) && !matching.Any(v => ReferenceEquals(v, volumeInfo)))
                        {
                            matching.Add(volumeInfo);
                            WriteObject(volumeInfo);
                        }
                    }
                    break;
                case PARAMETER_SET_NAME_BY_LITERAL_PATH:
                    foreach (DirectoryInfo directoryInfo in ResolveDirectoryFromLiteralPath(RootPathName).Select(p => new DirectoryInfo(p)).Distinct())
                    {
                        if (_volumeInfos.TryFind(new FileUri(directoryInfo), out IVolumeInfo volumeInfo) && !matching.Any(v => ReferenceEquals(v, volumeInfo)))
                        {
                            matching.Add(volumeInfo);
                            WriteObject(volumeInfo);
                        }
                    }
                    break;
                case PARAMETER_SET_NAME_BY_ROOT_DIRECTORY:
                    FileUri[] uris = ResolveDirectoryFromLiteralPath(RootPathName).Select(p => new FileUri(new DirectoryInfo(p))).Distinct().ToArray();
                    foreach (DirectoryInfo directoryInfo in ResolveDirectoryFromLiteralPath(RootPathName).Select(p => new DirectoryInfo(p)).Distinct())
                    {
                        if (_volumeInfos.TryFindByRootURI(new FileUri(directoryInfo), out IVolumeInfo volumeInfo) && !matching.Any(v => ReferenceEquals(v, volumeInfo)))
                        {
                            matching.Add(volumeInfo);
                            WriteObject(volumeInfo);
                        }
                    }
                    break;
                case PARAMETER_SET_NAME_BY_VOLUME_NAME:
                    foreach (string name in VolumeName.Distinct(ComponentHelper.IGNORE_CASE_COMPARER))
                    {
                        if (_volumeInfos.TryFindByName(name, out IVolumeInfo volumeInfo) && !matching.Any(v => ReferenceEquals(v, volumeInfo)))
                        {
                            matching.Add(volumeInfo);
                            WriteObject(volumeInfo);
                        }
                    }
                    break;
                case PARAMETER_SET_NAME_BY_IDENTIFIER:
                    foreach (VolumeIdentifier identifier in Identifier.Select(o => (o is PSObject psObj) ? psObj.BaseObject : o).Select(o =>
                        (VolumeIdentifier.TryCreate(o, out VolumeIdentifier volumeIdentifer)) ? (object)volumeIdentifer : null
                    ).OfType<VolumeIdentifier>().Distinct())
                    {
                        if (_volumeInfos.TryGetValue(identifier, out RegisteredVolumeItem volumeItem) && !matching.Any(v => ReferenceEquals(v, volumeItem)))
                        {
                            matching.Add(volumeItem);
                            WriteObject(volumeItem);
                        }
                    }
                    break;
                default:
                    foreach (IVolumeInfo v in _volumeInfos)
                        WriteObject(v);
                    return;
            }
        }
    }
}
