using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamVolume : IVolume, IUpstreamDbEntity
    {
        new IUpstreamFileSystem FileSystem { get; set; }
    }
}
