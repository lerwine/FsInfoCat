using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Base interface for entities that represent a grouping of extended file properties.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    public interface IPropertySet : IPropertiesRow
    {
        /// <summary>
        /// Gets the files that share the same property values as this property set.
        /// </summary>
        /// <value>The <see cref="IFile">files</see> that share the same property values as this property set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFile> Files { get; }
    }
}
