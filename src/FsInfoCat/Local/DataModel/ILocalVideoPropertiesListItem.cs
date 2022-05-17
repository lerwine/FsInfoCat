namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="ILocalVideoPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IVideoPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamVideoPropertiesListItem" />
    public interface ILocalVideoPropertiesListItem : ILocalVideoPropertiesRow, ILocalPropertiesListItem, IVideoPropertiesListItem { }
}
