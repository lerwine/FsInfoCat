using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for a tag entity that can be associated with <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/> entities.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IHasSimpleIdentifier" />
    /// <seealso cref="Local.Model.ILocalTagDefinitionRow" />
    /// <seealso cref="Upstream.Model.IUpstreamTagDefinitionRow" />
    public interface ITagDefinitionRow : IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>
        /// Gets the name of item tag.
        /// </summary>
        /// <value>The name of the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary>
        /// Gets the description of the current tag.
        /// </summary>
        /// <value>The custom description of the current tag.</value>
        [Display(Name = nameof(Properties.Resources.Description), ResourceType = typeof(Properties.Resources))]
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the tag is inactive (archived).
        /// </summary>
        /// <value><see langword="true"/> if the curren tag is inactive; otherwise, <see langword="false"/>.</value>
        [Display(Name = nameof(Properties.Resources.Description), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }
    }
}
