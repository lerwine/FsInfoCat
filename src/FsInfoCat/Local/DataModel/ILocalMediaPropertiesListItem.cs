namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="ILocalMediaPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IMediaPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamMediaPropertiesListItem" />
    public interface ILocalMediaPropertiesListItem : ILocalMediaPropertiesRow, ILocalPropertiesListItem, IMediaPropertiesListItem { }
}
