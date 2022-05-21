using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    /// <seealso cref="Local.Model.ILocalRecordedTVPropertiesRow" />
    public interface IUpstreamRecordedTVPropertiesRow : IUpstreamPropertiesRow, IRecordedTVPropertiesRow { }
}
