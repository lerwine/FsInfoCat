using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalExtendedProperties : IExtendedProperties, ILocalDbEntity
    {
        new IEnumerable<ILocalFile> Files { get; }
    }
}
