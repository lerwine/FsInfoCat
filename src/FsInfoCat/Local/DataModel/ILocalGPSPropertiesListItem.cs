namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="ILocalGPSPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IGPSPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamGPSPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalGPSPropertiesListItem")]
    public interface ILocalGPSPropertiesListItem : ILocalGPSPropertiesRow, ILocalPropertiesListItem, IGPSPropertiesListItem { }
}
