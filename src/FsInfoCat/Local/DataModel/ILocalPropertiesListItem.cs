namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalPropertiesListItem")]
    public interface ILocalPropertiesListItem : ILocalPropertiesRow, IPropertiesListItem { }
}
