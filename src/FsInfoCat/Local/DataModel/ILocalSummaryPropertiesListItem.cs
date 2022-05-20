namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="ILocalSummaryPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="ISummaryPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamSummaryPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalSummaryPropertiesListItem")]
    public interface ILocalSummaryPropertiesListItem : ILocalSummaryPropertiesRow, ILocalPropertiesListItem, ISummaryPropertiesListItem { }
}
