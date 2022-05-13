namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    public interface ILocalRecordedTVPropertiesRow : ILocalPropertiesRow, IRecordedTVPropertiesRow { }
}
