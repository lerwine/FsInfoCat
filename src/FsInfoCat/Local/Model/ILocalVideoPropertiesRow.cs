using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IVideoPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamVideoPropertiesRow" />
    public interface ILocalVideoPropertiesRow : ILocalPropertiesRow, IVideoPropertiesRow { }
}
