using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IUpstreamRecordedTVPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IRecordedTVPropertiesListItem" />
    /// <seealso cref="Local.Model.ILocalRecordedTVPropertiesListItem" />
    public interface IUpstreamRecordedTVPropertiesListItem : IUpstreamRecordedTVPropertiesRow, IUpstreamPropertiesListItem, IRecordedTVPropertiesListItem { }
}
