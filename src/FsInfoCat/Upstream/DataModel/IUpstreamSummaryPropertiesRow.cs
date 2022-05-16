namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="ISummaryPropertiesRow" />
    /// <seealso cref="Local.ILocalSummaryPropertiesRow" />
    public interface IUpstreamSummaryPropertiesRow : IUpstreamPropertiesRow, ISummaryPropertiesRow { }
}
