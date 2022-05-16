namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    /// <seealso cref="Local.ILocalRecordedTVPropertiesRow" />
    public interface IUpstreamRecordedTVPropertiesRow : IUpstreamPropertiesRow, IRecordedTVPropertiesRow { }
}
