using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public class FsInfoCatOptions
    {
        public const string FsInfoCat = nameof(FsInfoCat);

        public string LocalDbFile { get; set; }

        public string UpstreamDbConnectionString { get; set; }
    }
}
