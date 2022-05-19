using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IUpstreamRecordedTVPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="M.IRecordedTVPropertiesListItem" />
    /// <seealso cref="Local.Model.IRecordedTVPropertiesListItem" />
    public interface IUpstreamRecordedTVPropertiesListItem : IUpstreamRecordedTVPropertiesRow, IUpstreamPropertiesListItem, M.IRecordedTVPropertiesListItem { }
}
