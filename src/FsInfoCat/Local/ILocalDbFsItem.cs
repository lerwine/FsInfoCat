using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalDbFsItem : IDbFsItem, ILocalDbEntity
    {
        new IEnumerable<IAccessError<ILocalDbFsItem>> AccessErrors { get; }
    }
}
