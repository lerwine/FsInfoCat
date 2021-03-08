using FsInfoCat.Models.Crawl;
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
        public const string PARAMETER_SET_NAME_BY_PATH = "ByPath";
        public const string PARAMETER_SET_NAME_BY_LITERAL_PATH = "ByLiteralPath";
        public const string PARAMETER_SET_NAME_BY_ROOT_DIRECTORY = "ByRootDirectory";
        public const string PARAMETER_SET_NAME_BY_VOLUME_NAME = "ByVolumeName";
        public const string PARAMETER_SET_NAME_BY_IDENTIFIER = "ByIdentifier";
        public const string PARAMETER_SET_NAME_GET_ALL = "GetAll";

        private Collection<PSObject> _volumeInfos;

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
            _volumeInfos = GetVolumeRegistration();
        }

        protected override void OnItemNotFoundException(string path, ItemNotFoundException exc)
        {
            throw new NotImplementedException();
        }

        protected override void OnPathIsFileError(string providerPath)
        {
            throw new NotImplementedException();
        }

        protected override void OnProviderNotSupportedException(string path, Exception exc)
        {
            throw new NotImplementedException();
        }

        protected override void OnResolveError(string path, Exception exc)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessRecord()
        {
            if (_volumeInfos.Count == 0)
                return;
            IEnumerable<PSObject> matching;
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_BY_PATH:
                    var items = _volumeInfos.Select(v => new { Obj = v, V = (RegisteredVolumeInfo)v.BaseObject });
                    foreach (DirectoryInfo directoryInfo in Path.SelectMany(p => ResolveDirectoryFromWcPath(p)).Select(p => new DirectoryInfo(p)).Distinct())
                    {
                        for (DirectoryInfo d = directoryInfo; !(d is null); d = d.Parent)
                        {
                            FileUri fileUri = new FileUri(d);
                            matching = _volumeInfos.WhereBaseObjectOf<RegisteredVolumeInfo>(f => f.RootUri.Equals(fileUri, f.PathComparer));
                        }
                    }
                    break;
                case PARAMETER_SET_NAME_BY_LITERAL_PATH:
                    foreach (DirectoryInfo directoryInfo in ResolveDirectoryFromLiteralPath(RootPathName).Select(p => new DirectoryInfo(p)).Distinct())
                    {

                    }
                    break;
                case PARAMETER_SET_NAME_BY_ROOT_DIRECTORY:
                    FileUri[] uris = ResolveDirectoryFromLiteralPath(RootPathName).Select(p => new FileUri(new DirectoryInfo(p))).Distinct().ToArray();
                    if (uris.Length == 1)
                    {
                        FileUri u = uris[0];
                        matching = _volumeInfos.WhereBaseObjectOf<RegisteredVolumeInfo>(f => f.RootUri.Equals(u, f.PathComparer));
                    }
                    else if (uris.Length > 1)
                        matching = _volumeInfos.WhereBaseObjectOf<RegisteredVolumeInfo>(f => uris.Any(u => f.RootUri.Equals(u, f.PathComparer)));
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
            throw new NotImplementedException();
            // TODO: Finish implementation
            //foreach (PSObject p in matching.ToArray())
            //{
            //    _volumeInfos.Remove(p);
            //    WriteObject(p);
            //}
        }
    }
}
