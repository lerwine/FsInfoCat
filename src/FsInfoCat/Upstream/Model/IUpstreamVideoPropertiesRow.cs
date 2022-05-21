using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IVideoPropertiesRow" />
    /// <seealso cref="Local.Model.ILocalVideoPropertiesRow" />
    public interface IUpstreamVideoPropertiesRow : IUpstreamPropertiesRow, IVideoPropertiesRow { }
}
