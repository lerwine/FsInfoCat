using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace FsInfoCat.PS.Commands
{
    // ConvertTo-FileUri
    [Cmdlet(VerbsData.ConvertTo, "FileUri")]
    [OutputType(typeof(FileUri))]
    public class ConvertToFileUriCommand : PSCmdlet
    {
    }
}
