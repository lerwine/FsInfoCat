namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamRecordedTVPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalRecordedTVPropertiesRow")]
    public interface ILocalRecordedTVPropertiesRow : ILocalPropertiesRow, IRecordedTVPropertiesRow { }
}
