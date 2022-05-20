namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IUpstreamGPSPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IGPSPropertiesListItem" />
    /// <seealso cref="Local.ILocalGPSPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamGPSPropertiesListItem")]
    public interface IUpstreamGPSPropertiesListItem : IUpstreamGPSPropertiesRow, IUpstreamPropertiesListItem, IGPSPropertiesListItem { }
}
