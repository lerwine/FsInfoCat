namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    /// <seealso cref="Local.ILocalRecordedTVPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamRecordedTVPropertiesRow")]
    public interface IUpstreamRecordedTVPropertiesRow : IUpstreamPropertiesRow, IRecordedTVPropertiesRow { }
}
