namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    public interface IUpstreamRecordedTVPropertiesRow : IUpstreamPropertiesRow, IRecordedTVPropertiesRow { }
}
