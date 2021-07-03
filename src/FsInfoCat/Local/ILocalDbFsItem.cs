using System.Collections.Generic;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Base interface for a database entity that represents a local file system node.
    /// </summary>
    /// <seealso cref="IDbFsItem" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ILocalSubdirectory" />
    /// <seealso cref="ILocalFile" />
    public interface ILocalDbFsItem : IDbFsItem, ILocalDbEntity
    {
        new IEnumerable<IAccessError<ILocalDbFsItem>> AccessErrors { get; }
    }
}
