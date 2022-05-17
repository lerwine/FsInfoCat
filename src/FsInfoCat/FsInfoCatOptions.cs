using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat
{
    // TODO: Document FsInfoCatOptions class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class FsInfoCatOptions
    {
        public const string FsInfoCat = nameof(FsInfoCat);

        public string LocalDbFile { get; set; }

        public string UpstreamDbConnectionString { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
