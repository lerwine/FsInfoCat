using FsInfoCat.PS.Export;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.PS.Commands
{
    /// <summary>
    /// Open-Image
    /// </summary>
    [Cmdlet(VerbsCommon.Open, "Image", DefaultParameterSetName = ParameterSetName_WcPath, RemotingCapability = RemotingCapability.None)]
    [OutputType(typeof(ExportSet))]
    public class Import_FileSystemInfo : PSCmdlet
    {
        public const string ParameterSetName_WcPath = "WcPath";
        public const string ParameterSetName_LiteralPath = "LiteralPath";
        private const string VariableName_ExportSet = nameof(ExportSet);

        /// <summary>
        /// Specifies a path to one or more locations. Wildcards are permitted. The default location is the current directory (.).
        /// </summary>
        [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = ParameterSetName_WcPath,
            HelpMessageResourceId = nameof(Properties.Resources.Import_FileSystemInfo_Path))]
        [ValidateNotNullOrEmpty()]
        public string[] Path { get; set; }

        /// <summary>
        /// Specifies, as a string array, a path to one or more locations.
        /// </summary>
        /// <remarks>Unlike the Path parameter, the value of the LiteralPath parameter is used exactly as it is typed. No characters are interpreted as wildcards.
        /// If the path includes escape characters, enclose it in single quotation marks.
        /// Single quotation marks tell PowerShell not to interpret any characters as escape sequences.</remarks>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSetName_LiteralPath,
            HelpMessageResourceId = nameof(Properties.Resources.Import_FileSystemInfo_LiteralPath))]
        [ValidateNotNullOrEmpty()]
        [Alias("PSPath", "Path", "FullName")]
        public string[] LiteralPath { get; set; }

        /// <summary>
        /// Specifies a filter in the provider's format or language.
        /// </summary>
        /// <remarks>The value of this parameter qualifies the Path parameter. The syntax of the filter, including the use of wildcards, depends on the provider.
        /// Filters are more efficient than other parameters, because the provider applies them when retrieving the objects, rather than having PowerShell filter the
        /// objects after they are retrieved.</remarks>
        [Parameter(Position = 1, HelpMessageResourceId = nameof(Properties.Resources.Import_FileSystemInfo_Filter))]
        public string Filter { get; set; }

        /// <summary>
        /// Specifies, as a string array, an item or items that this cmdlet excludes in the operation.
        /// </summary>
        /// <remarks>The value of this parameter qualifies the Path parameter. Enter a path element or pattern, such as *.txt.Wildcards are permitted.</remarks>
        [Parameter(HelpMessageResourceId = nameof(Properties.Resources.Import_FileSystemInfo_Exclude))]
        public string[] Exclude { get; set; }

        /// <summary>
        /// Specifies the depth of recursion of items to import.
        /// </summary>
        [Parameter(HelpMessageResourceId = nameof(Properties.Resources.Import_FileSystemInfo_Depth))]
        public uint Depth { get; set; }

        /// <summary>
        /// Import hidden and system folders and files.
        /// </summary>
        /// <remarks>By default, `Import-FileSystemInfo` doesn't import hidden or system files and folders.</remarks>
        [Parameter(HelpMessageResourceId = nameof(Properties.Resources.Import_FileSystemInfo_Force))]
        public SwitchParameter Force { get; set; }

        public ExportSet ExportSet { get; set; }

        protected override void BeginProcessing()
        {
            if (ExportSet is null)
                ExportSet = new ExportSet();
            else
                ExportSet.SetAllProcessedFlags(false);
        }

        protected override void ProcessRecord()
        {
            Collection<PSObject> items;
            if (ParameterSetName == ParameterSetName_LiteralPath)
            {
                if (MyInvocation.BoundParameters.ContainsKey(nameof(Depth)))
                {
                    if (Depth > 0)
                        items = InvokeProvider.ChildItem.Get(LiteralPath, true, Depth, Force.IsPresent, true);
                    else
                        items = InvokeProvider.ChildItem.Get(LiteralPath, false, 0, Force.IsPresent, true);
                }
                else
                    items = InvokeProvider.ChildItem.Get(LiteralPath, true, uint.MaxValue, Force.IsPresent, true);
            }
            else if (MyInvocation.BoundParameters.ContainsKey(nameof(Depth)))
            {
                if (Depth > 0)
                    items = InvokeProvider.ChildItem.Get(Path, true, Depth, Force.IsPresent, false);
                else
                    items = InvokeProvider.ChildItem.Get(Path, false, 0, Force.IsPresent, false);
            }
            else
                items = InvokeProvider.ChildItem.Get(Path, true, uint.MaxValue, Force.IsPresent, false);
            foreach (PSObject obj in items)
            {
                if (obj.BaseObject is FileInfo fileInfo)
                    ExportSet.Import(fileInfo);
                else if (obj.BaseObject is DirectoryInfo directoryInfo)
                    ExportSet.Import(directoryInfo);
            }    
        }

        protected override void EndProcessing()
        {
            WriteObject(ExportSet);
        }
    }
}
