using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamDbFsItem : IDbFsItem, IUpstreamDbEntity
    {
        new IEnumerable<IAccessError<IUpstreamDbFsItem>> AccessErrors { get; }
    }
}
