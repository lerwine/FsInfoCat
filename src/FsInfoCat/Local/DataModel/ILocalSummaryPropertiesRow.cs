namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="ISummaryPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamSummaryPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalSummaryPropertiesRow")]
    public interface ILocalSummaryPropertiesRow : ILocalPropertiesRow, ISummaryPropertiesRow { }
}
