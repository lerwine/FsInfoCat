using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="Local.ILocalPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Model.IPropertiesListItem")]
    public interface IPropertiesListItem : IPropertiesRow
    {
        /// <summary>
        /// Gets the number of non-deleted files associated with the current property set.
        /// </summary>
        /// <value>The number of non-deleted files associated with the current property set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        long ExistingFileCount { get; }

        /// <summary>
        /// Gets the total number of file entities associated with the current property set.
        /// </summary>
        /// <value>The number of files associated with the current property set, including entities representing deleted files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AllFiles), ResourceType = typeof(Properties.Resources))]
        long TotalFileCount { get; }
    }
}
