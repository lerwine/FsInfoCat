namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="Local.ILocalPropertiesListItem" />
    public interface IUpstreamPropertiesListItem : IUpstreamPropertiesRow, IPropertiesListItem { }
}