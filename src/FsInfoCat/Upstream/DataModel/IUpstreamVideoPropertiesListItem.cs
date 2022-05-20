namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IUpstreamVideoPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IVideoPropertiesListItem" />
    /// <seealso cref="Local.ILocalVideoPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamVideoPropertiesListItem")]
    public interface IUpstreamVideoPropertiesListItem : IUpstreamVideoPropertiesRow, IUpstreamPropertiesListItem, IVideoPropertiesListItem { }
}
