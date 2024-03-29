using FsInfoCat.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base interface for entities that represent a grouping of extended file properties.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Upstream.Model.IUpstreamPropertySet" />
    public interface ILocalPropertySet : ILocalPropertiesRow, IPropertySet
    {
        /// <summary>
        /// Gets the files that share the same property values as this property set.
        /// </summary>
        /// <value>The <see cref="ILocalFile">files</see> that share the same property values as this property set.</value>
        [Display(Name = nameof(Properties.Resources.Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalFile> Files { get; }
    }
}
