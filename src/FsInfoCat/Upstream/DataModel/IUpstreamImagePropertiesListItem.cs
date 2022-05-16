namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IUpstreamImagePropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IImagePropertiesListItem" />
    /// <seealso cref="Local.ILocalImagePropertiesListItem" />
    public interface IUpstreamImagePropertiesListItem : IUpstreamImagePropertiesRow, IUpstreamPropertiesListItem, IImagePropertiesListItem { }
}
