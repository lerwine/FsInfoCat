namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IUpstreamDRMPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IDRMPropertiesListItem" />
    /// <seealso cref="Local.ILocalDRMPropertiesListItem" />
    public interface IUpstreamDRMPropertiesListItem : IUpstreamDRMPropertiesRow, IUpstreamPropertiesListItem, IDRMPropertiesListItem { }
}
