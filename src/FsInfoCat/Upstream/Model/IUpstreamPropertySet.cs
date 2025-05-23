using FsInfoCat.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Base interface for entities that represent a grouping of extended file properties.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.Model.ILocalPropertySet" />
    public interface IUpstreamPropertySet : IUpstreamDbEntity, IPropertySet
    {
        /// <summary>
        /// Gets the files that share the same property values as this property set.
        /// </summary>
        /// <value>The <see cref="IUpstreamFile">files</see> that share the same property values as this property set.</value>
        [Display(Name = nameof(Properties.Resources.Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamFile> Files { get; }
    }
}
