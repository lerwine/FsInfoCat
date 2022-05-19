using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="M.IRecordedTVPropertiesRow" />
    /// <seealso cref="Local.Model.IRecordedTVPropertiesRow" />
    public interface IUpstreamRecordedTVPropertiesRow : IUpstreamPropertiesRow, M.IRecordedTVPropertiesRow { }
}
