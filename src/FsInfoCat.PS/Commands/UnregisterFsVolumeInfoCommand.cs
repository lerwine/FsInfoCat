using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;

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
            Collection<PSObject> volumeRegistration = GetVolumeRegistration();
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_ROOT_PATH:
                    // TODO: Need to use GetUnresolvedProviderPathFromPSPath on values from RootPathName
                    foreach (string path in RootPathName)
                    {
                        FileUri fileUri;
                        try { fileUri = new FileUri(new DirectoryInfo(path)); }
                        catch (Exception exc)
                        {
                            WriteError(MessageId.UnexpectedError.ToErrorRecord(exc, ErrorCategory.ReadError, nameof(RootPathName), path));
                            continue;
                        }
                        PSObject item = volumeRegistration.WhereBaseObjectOf<RegisteredVolumeInfo>(v => v.RootUri.Equals(fileUri, v.PathComparer)).FirstOrDefault();
                        if (item is null)
                            WriteWarning($"Root path '{path}' was not registered.");
                        else
                        {
                            volumeRegistration.Remove(item);
                            if (PassThru.IsPresent)
                                WriteObject(item, false);
                        }
                    }
                    break;
                case PARAMETER_SET_NAME_VOLUME_IDENTIFIER:
                    foreach (object id in Identifier)
                    {
                        if (VolumeIdentifier.TryCreate((id is PSObject psObj) ? psObj.BaseObject : Identifier, out VolumeIdentifier volumeIdentifer))
                        {
                            PSObject item = volumeRegistration.WhereBaseObjectOf<RegisteredVolumeInfo>(v => v.Identifier.Equals(volumeIdentifer)).FirstOrDefault();
                            if (item is null)
                                WriteWarning($"Volume identifier '{volumeIdentifer}' was not registered.");
                            else
                            {
                                volumeRegistration.Remove(item);
                                if (PassThru.IsPresent)
                                    WriteObject(item, false);
                            }
                        }
                        else
                            WriteError(MessageId.InvalidIdentifier.ToArgumentOutOfRangeError(nameof(Identifier), Identifier));
                    }
                    break;
                default:
                    foreach (IVolumeInfo volume in VolumeInfo)
                    {
                        PSObject item = volumeRegistration.WhereBaseObjectOf<RegisteredVolumeInfo>(v => v.Equals(volume)).FirstOrDefault();
                        if (item is null)
                            WriteWarning($"Volume '{volume}' was not registered.");
                        else
                        {
                            volumeRegistration.Remove(item);
                            if (PassThru.IsPresent)
                                WriteObject(item, false);
                        }
                    }
                    break;
            }

        }
    }
}
