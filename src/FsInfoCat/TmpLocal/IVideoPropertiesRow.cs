using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="M.IVideoPropertiesRow" />
    /// <seealso cref="Upstream.Model.IVideoPropertiesRow" />
    public interface ILocalVideoPropertiesRow : ILocalPropertiesRow, M.IVideoPropertiesRow { }
}
