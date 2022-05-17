namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="ILocalRecordedTVPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IRecordedTVPropertiesListItem" />
    /// <seealso cref="Upstream.IUpstreamRecordedTVPropertiesListItem" />
    public interface ILocalRecordedTVPropertiesListItem : ILocalRecordedTVPropertiesRow, ILocalPropertiesListItem, IRecordedTVPropertiesListItem { }
}
