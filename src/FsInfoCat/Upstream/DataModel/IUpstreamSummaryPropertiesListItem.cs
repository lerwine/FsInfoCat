namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IUpstreamSummaryPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="ISummaryPropertiesListItem" />
    /// <seealso cref="Local.ILocalSummaryPropertiesListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamSummaryPropertiesListItem")]
    public interface IUpstreamSummaryPropertiesListItem : IUpstreamSummaryPropertiesRow, IUpstreamPropertiesListItem, ISummaryPropertiesListItem { }
}
