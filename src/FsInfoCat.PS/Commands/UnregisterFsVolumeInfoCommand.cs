using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace FsInfoCat.PS.Commands
{
    // Unregister-FsVolumeInfo
    [Cmdlet(VerbsLifecycle.Unregister, "FsVolumeInfo", DefaultParameterSetName = PARAMETER_SET_NAME_VOLUME_INFO)]
    public class UnregisterFsVolumeInfoCommand : FsVolumeInfoCommand
    {
        public const string PARAMETER_SET_NAME_VOLUME_IDENTIFIER = "VolumeIdentifier";
        public const string PARAMETER_SET_NAME_ROOT_PATH = "RootPath";
        public const string PARAMETER_SET_NAME_VOLUME_INFO = "VolumeInfo";
        private const string HELP_MESSAGE_PASSTHRU = "Return the IVolumeInfo object of the un-registered volume.";

        [Parameter(HelpMessage = "The full path name of the root directory for the volume to un-register. Case-sensitivity is dependent upon the volume registration.", Mandatory = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_ROOT_PATH)]
        [ValidateNotNullOrEmpty()]
        [Alias("RootPath", "FullName")]
        public string[] RootPathName { get; set; }

        [Parameter(HelpMessage = "The volume UUID or VSN to un-register. For remote file shares, this should be the URN of the remote source. This is not case-senstive.", Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_VOLUME_IDENTIFIER)]
        [Alias("SerialNumber", "VolumeId", "VSN", "UUID")]
        public object[] Identifier { get; set; }

        [Parameter(HelpMessage = "The volume information object to un-register.", Mandatory = true, ValueFromPipeline = true,
            ParameterSetName = PARAMETER_SET_NAME_VOLUME_INFO)]
        public IVolumeInfo[] VolumeInfo { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_PASSTHRU, ParameterSetName = PARAMETER_SET_NAME_VOLUME_IDENTIFIER)]
        [Parameter(HelpMessage = HELP_MESSAGE_PASSTHRU, ParameterSetName = PARAMETER_SET_NAME_ROOT_PATH)]
        public SwitchParameter PassThru { get; set; }

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
            VolumeInfoRegistration volumeRegistration = GetVolumeRegistration(SessionState);
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_ROOT_PATH:
                    foreach (string path in RootPathName.Select(p => GetUnresolvedProviderPathFromPSPath(p)))
                    {
                        FileUri fileUri;
                        try { fileUri = new FileUri(new DirectoryInfo(path)); }
                        catch (Exception exc)
                        {
                            WriteError(MessageId.UnexpectedError.ToErrorRecord(exc, ErrorCategory.ReadError, nameof(RootPathName), path));
                            continue;
                        }
                        if (volumeRegistration.TryFindByRootURI(fileUri, out VolumeInfoRegistration.RegisteredVolumeItem item))
                        {
                            volumeRegistration.Remove(item);
                            if (PassThru.IsPresent)
                                WriteObject(item, false);
                        }
                        else
                            WriteWarning($"Root path '{path}' was not registered.");
                    }
                    break;
                case PARAMETER_SET_NAME_VOLUME_IDENTIFIER:
                    foreach (object id in Identifier)
                    {
                        if (VolumeIdentifier.TryCreate((id is PSObject psObj) ? psObj.BaseObject : Identifier, out VolumeIdentifier volumeIdentifer))
                        {
                            if (volumeRegistration.TryGetValue(volumeIdentifer, out VolumeInfoRegistration.RegisteredVolumeItem item))
                            {
                                volumeRegistration.Remove(item);
                                if (PassThru.IsPresent)
                                    WriteObject(item, false);
                            }
                            else
                                WriteWarning($"Volume identifier '{volumeIdentifer}' was not registered.");
                        }
                        else
                            WriteError(MessageId.InvalidIdentifier.ToArgumentOutOfRangeError(nameof(Identifier), Identifier));
                    }
                    break;
                default:
                    foreach (IVolumeInfo volume in VolumeInfo)
                    {
                        if (volumeRegistration.TryFindMatching(volume, out VolumeInfoRegistration.RegisteredVolumeItem item))
                        {
                            volumeRegistration.Remove(item);
                            if (PassThru.IsPresent)
                                WriteObject(item, false);
                        }
                        else
                            WriteWarning($"Volume '{volume}' was not registered.");
                    }
                    break;
            }

        }
    }
}
