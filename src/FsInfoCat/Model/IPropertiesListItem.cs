using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IAudioPropertiesListItem" />
    /// <seealso cref="IDocumentPropertiesListItem" />
    /// <seealso cref="IDRMPropertiesListItem" />
    /// <seealso cref="IGPSPropertiesListItem" />
    /// <seealso cref="IImagePropertiesListItem" />
    /// <seealso cref="IMediaPropertiesListItem" />
    /// <seealso cref="IMusicPropertiesListItem" />
    /// <seealso cref="IPhotoPropertiesListItem" />
    /// <seealso cref="IRecordedTVPropertiesListItem" />
    /// <seealso cref="ISummaryPropertiesListItem" />
    /// <seealso cref="IVideoPropertiesListItem" />
    /// <seealso cref="Local.Model.ILocalPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamPropertiesListItem" />
    public interface IPropertiesListItem : IPropertiesRow
    {
        /// <summary>
        /// Gets the number of non-deleted files associated with the current property set.
        /// </summary>
        /// <value>The number of non-deleted files associated with the current property set.</value>
        [Display(Name = nameof(Properties.Resources.Files), ResourceType = typeof(Properties.Resources))]
        long ExistingFileCount { get; }

        /// <summary>
        /// Gets the total number of file entities associated with the current property set.
        /// </summary>
        /// <value>The number of files associated with the current property set, including entities representing deleted files.</value>
        [Display(Name = nameof(Properties.Resources.TotalFileCount), ResourceType = typeof(Properties.Resources))]
        long TotalFileCount { get; }
    }
}
