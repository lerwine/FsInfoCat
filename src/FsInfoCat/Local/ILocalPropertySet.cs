using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalPropertySet : ILocalDbEntity, IPropertySet
    {
        /// <summary>
        /// Gets the files that have this property set.
        /// </summary>
        new IEnumerable<ILocalFile> Files { get; }
    }
}
