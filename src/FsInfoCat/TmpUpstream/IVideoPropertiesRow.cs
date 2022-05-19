using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="M.IVideoPropertiesRow" />
    /// <seealso cref="Local.Model.IVideoPropertiesRow" />
    public interface IUpstreamVideoPropertiesRow : IUpstreamPropertiesRow, M.IVideoPropertiesRow { }
}
