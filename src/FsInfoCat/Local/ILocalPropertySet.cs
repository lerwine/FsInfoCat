using System.Collections.Generic;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Interface ILocalPropertySet
    /// Implements the <see cref="ILocalDbEntity" />
    /// Implements the <see cref="IPropertySet" />
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IPropertySet" />
    public interface ILocalPropertySet : ILocalDbEntity, IPropertySet
    {
        /// <summary>
        /// Gets the files that share the same property values as this property set.
        /// </summary>
        /// <value>The <see cref="ILocalFile">files</see> that share the same property values as this property set.</value>
        new IEnumerable<ILocalFile> Files { get; }
    }
}
