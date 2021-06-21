using System.Collections.Generic;

namespace FsInfoCat.Local
{
    [System.Obsolete]
    public interface ILocalExtendedProperties : IExtendedProperties, ILocalDbEntity
    {
        new IEnumerable<ILocalFile> Files { get; }
    }
}
