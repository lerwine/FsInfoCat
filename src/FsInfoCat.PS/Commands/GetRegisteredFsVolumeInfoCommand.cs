using FsInfoCat.Models.Crawl;
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
        private Collection<VolumeIdentifier> _yieldedIdentifiers;

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
            _yieldedIdentifiers = new Collection<VolumeIdentifier>();
            _volumeInfos = GetVolumeRegistration(SessionState);
        }

        /// <summary>
        /// This gets called whenever a non-existent-path is encountered.
        /// </summary>
        /// <param name="path">the non-existent filesystem path.</param>
        /// <param name="exc">The exception that was thrown or <see langword="null"/> if existence validation failed without an exception being thrown.</param>
        /// <remarks>This gets called by <see cref="FsVolumeInfoCommand.ResolveDirectoryFromLiteralPath(System.Collections.Generic.IEnumerable{string})"/>, <see cref="FsVolumeInfoCommand.ResolveDirectoryFromWcPath(string)"/>
        /// and <see cref="FsVolumeInfoCommand.TryResolveDirectoryFromLiteralPath(string, out string)"/> when trying to resolve a path that does not exist.</remarks>
        protected override void OnItemNotFoundException(string path, ItemNotFoundException exc)
        {
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_BY_LITERAL_PATH:
                    WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Subdirectory not found"), ErrorCategory.ObjectNotFound,
                        nameof(LiteralPath), path));
                    break;
                case PARAMETER_SET_NAME_BY_ROOT_DIRECTORY:
                    WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Subdirectory not found"), ErrorCategory.ObjectNotFound,
                        nameof(RootPathName), path));
                    break;
                default:
                    WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Subdirectory not found"), ErrorCategory.ObjectNotFound,
                        nameof(Path), path));
                    break;
            }
        }

        /// <summary>
        /// This gets called whenever a path is encountered with refers to a file rather than a subdirectory.
        /// </summary>
        /// <param name="path">The path to a file.</param>
        /// <remarks>This gets called by <see cref="FsVolumeInfoCommand.ResolveDirectoryFromLiteralPath(System.Collections.Generic.IEnumerable{string})"/>, <see cref="FsVolumeInfoCommand.ResolveDirectoryFromWcPath(string)"/>
        /// and <see cref="FsVolumeInfoCommand.TryResolveDirectoryFromLiteralPath(string, out string)"/> when a path was successfully resolved, but it did not refer to a subdirectory.</remarks>
        protected override void OnPathIsFileError(string path)
        {
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_BY_LITERAL_PATH:
                    WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Path refers does not refer to a subdirectory"),
                        ErrorCategory.ObjectNotFound, nameof(LiteralPath), path));
                    break;
                case PARAMETER_SET_NAME_BY_ROOT_DIRECTORY:
                    WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Path refers does not refer to a subdirectory"),
                        ErrorCategory.ObjectNotFound, nameof(RootPathName), path));
                    break;
                default:
                    WriteError(MessageId.PathNotFound.ToErrorRecord(new DirectoryNotFoundException("Path refers does not refer to a subdirectory"),
                        ErrorCategory.ObjectNotFound, nameof(Path), path));
                    break;
            }
        }

        /// <summary>
        /// This gets called whenever an invalid path string is enountered.
        /// </summary>
        /// <param name="path">The path string that could not be resolved.</param>
        /// <param name="exc">The exeption that was thrown.</param>
        /// <remarks>This gets called by <see cref="FsVolumeInfoCommand.ResolveDirectoryFromLiteralPath(System.Collections.Generic.IEnumerable{string})"/>, <see cref="FsVolumeInfoCommand.ResolveDirectoryFromWcPath(string)"/>
        /// and <see cref="FsVolumeInfoCommand.TryResolveDirectoryFromLiteralPath(string, out string)"/> when trying to resolve a path that is not supported by the local filesystem.</remarks>
        protected override void OnProviderNotSupportedException(string path, Exception exc)
        {
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_BY_LITERAL_PATH:
                    WriteError(MessageId.InvalidPath.ToErrorRecord("Path references an unsupported provider type", exc, ErrorCategory.InvalidArgument, nameof(LiteralPath), path));
                    break;
                case PARAMETER_SET_NAME_BY_ROOT_DIRECTORY:
                    WriteError(MessageId.InvalidPath.ToErrorRecord("Path references an unsupported provider type", exc, ErrorCategory.InvalidArgument, nameof(RootPathName), path));
                    break;
                default:
                    WriteError(MessageId.InvalidPath.ToErrorRecord("Path references an unsupported provider type", exc, ErrorCategory.InvalidArgument, nameof(Path), path));
                    break;
            }
        }

        /// <summary>
        /// This gets called when an unexpected exception is thrown while trying to resolve a wildcard-supported path string.
        /// </summary>
        /// <param name="path">The path string that could not be resolved.</param>
        /// <param name="exc">The exeption that was thrown.</param>
        /// <remarks>This gets called by <see cref="FsVolumeInfoCommand.ResolveDirectoryFromWcPath(string)"/> when an unexpected exception is thrown while trying to resolve a wildcard-supported path string.</remarks>
        protected override void OnResolveError(string path, Exception exc)
        {
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_BY_PATH:
                    WriteError(MessageId.InvalidPath.ToErrorRecord(exc, nameof(LiteralPath), path));
                    break;
                case PARAMETER_SET_NAME_BY_LITERAL_PATH:
                    WriteError(MessageId.InvalidPath.ToErrorRecord(exc, nameof(RootPathName), path));
                    break;
                default:
                    WriteError(MessageId.InvalidPath.ToErrorRecord(exc, nameof(Path), path));
                    break;
            }
        }

        protected override void ProcessRecord()
        {
            if (_volumeInfos.Count == 0)
                return;
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_BY_PATH:
                    foreach (DirectoryInfo directoryInfo in Path.SelectMany(p => ResolveDirectoryFromWcPath(p)).Select(p => new DirectoryInfo(p)).Distinct())
                    {
                        if (_volumeInfos.TryGetByChildURI(directoryInfo, out RegisteredVolumeItem volumeItem) && !_yieldedIdentifiers.Contains(((IVolumeInfo)volumeItem).Identifier))
                        {
                            _yieldedIdentifiers.Add(((IVolumeInfo)volumeItem).Identifier);
                            WriteObject(volumeItem);
                        }
                    }
                    break;
                case PARAMETER_SET_NAME_BY_LITERAL_PATH:
                    foreach (DirectoryInfo directoryInfo in ResolveDirectoryFromLiteralPath(RootPathName).Select(p => new DirectoryInfo(p)).Distinct())
                    {
                        if (_volumeInfos.TryGetByChildURI(directoryInfo, out RegisteredVolumeItem volumeItem) && !_yieldedIdentifiers.Contains(((IVolumeInfo)volumeItem).Identifier))
                        {
                            _yieldedIdentifiers.Add(((IVolumeInfo)volumeItem).Identifier);
                            WriteObject(volumeItem);
                        }
                    }
                    break;
                case PARAMETER_SET_NAME_BY_ROOT_DIRECTORY:
                    FileUri[] uris = ResolveDirectoryFromLiteralPath(RootPathName).Select(p => new FileUri(new DirectoryInfo(p))).Distinct().ToArray();
                    foreach (DirectoryInfo directoryInfo in ResolveDirectoryFromLiteralPath(RootPathName).Select(p => new DirectoryInfo(p)).Distinct())
                    {
                        if (_volumeInfos.TryGetValue(directoryInfo, out RegisteredVolumeItem volumeItem) && !_yieldedIdentifiers.Contains(((IVolumeInfo)volumeItem).Identifier))
                        {
                            _yieldedIdentifiers.Add(((IVolumeInfo)volumeItem).Identifier);
                            WriteObject(volumeItem);
                        }
                    }
                    break;
                case PARAMETER_SET_NAME_BY_VOLUME_NAME:
                    foreach (string name in VolumeName.Distinct(ComponentHelper.IGNORE_CASE_COMPARER))
                    {
                        if (_volumeInfos.TryGetValue(name, out RegisteredVolumeItem volumeItem) && !_yieldedIdentifiers.Contains(((IVolumeInfo)volumeItem).Identifier))
                        {
                            _yieldedIdentifiers.Add(((IVolumeInfo)volumeItem).Identifier);
                            WriteObject(volumeItem);
                        }
                    }
                    break;
                case PARAMETER_SET_NAME_BY_IDENTIFIER:
                    foreach (VolumeIdentifier identifier in Identifier.Select(o => (o is PSObject psObj) ? psObj.BaseObject : o).Select(o =>
                        (VolumeIdentifier.TryCreate(o, out VolumeIdentifier volumeIdentifer)) ? (object)volumeIdentifer : null
                    ).OfType<VolumeIdentifier>().Distinct())
                    {
                        if (_volumeInfos.TryGetValue(identifier, out RegisteredVolumeItem volumeItem) && !_yieldedIdentifiers.Contains(((IVolumeInfo)volumeItem).Identifier))
                        {
                            _yieldedIdentifiers.Add(((IVolumeInfo)volumeItem).Identifier);
                            WriteObject(volumeItem);
                        }
                    }
                    break;
                default:
                    foreach (RegisteredVolumeItem v in _volumeInfos)
                        WriteObject(v);
                    return;
            }
        }
    }
}
