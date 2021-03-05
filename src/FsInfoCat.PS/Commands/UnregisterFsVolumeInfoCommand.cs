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
        private const string HELP_MESSAGE_PASSTHRU = "Return un-registered volume";

        [Parameter(HelpMessage = "The full, case-sensitive path name of the volume root directory.", Mandatory = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_ROOT_PATH)]
        [ValidateNotNullOrEmpty()]
        public string[] RootPathName { get; set; }

        [Parameter(HelpMessage = "The volume serial number or UUID.", Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_VOLUME_IDENTIFIER)]
        [Alias("SerialNumber")]
        public object[] Identifier { get; set; }

        [Parameter(HelpMessage = "The volume information object.", Mandatory = true, ValueFromPipeline = true,
            ParameterSetName = PARAMETER_SET_NAME_VOLUME_INFO)]
        public IVolumeInfo[] VolumeInfo { get; set; }

        [Parameter(HelpMessage = HELP_MESSAGE_PASSTHRU, ParameterSetName = PARAMETER_SET_NAME_VOLUME_IDENTIFIER)]
        [Parameter(HelpMessage = HELP_MESSAGE_PASSTHRU, ParameterSetName = PARAMETER_SET_NAME_ROOT_PATH)]
        public SwitchParameter PassThru { get; set; }

        protected override void ProcessRecord()
        {
            Collection<PSObject> volumeRegistration = GetVolumeRegistration();
            switch (ParameterSetName)
            {
                case PARAMETER_SET_NAME_ROOT_PATH:
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
