namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IUpstreamMediaPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IMediaPropertiesListItem" />
    /// <seealso cref="Local.ILocalMediaPropertiesListItem" />
    public interface IUpstreamMediaPropertiesListItem : IUpstreamMediaPropertiesRow, IUpstreamPropertiesListItem, IMediaPropertiesListItem { }
}