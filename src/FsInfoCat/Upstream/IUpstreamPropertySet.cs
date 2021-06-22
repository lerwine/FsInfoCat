using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamPropertySet : IUpstreamDbEntity, IPropertySet
    {
        /// <summary>
        /// Gets the files that have this property set.
        /// </summary>
        new IEnumerable<IUpstreamFile> Files { get; }
    }
}
