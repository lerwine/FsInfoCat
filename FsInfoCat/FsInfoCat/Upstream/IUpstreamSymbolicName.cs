using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamSymbolicName : ISymbolicName, IUpstreamDbEntity
    {
        new IUpstreamFileSystem FileSystem { get; set; }
    }
}
