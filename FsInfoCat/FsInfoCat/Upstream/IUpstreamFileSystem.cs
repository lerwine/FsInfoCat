using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamFileSystem : IFileSystem, IUpstreamDbEntity
    {
        new IEnumerable<IUpstreamVolume> Volumes { get; }

        new IEnumerable<IUpstreamSymbolicName> SymbolicNames { get; }
    }
}
