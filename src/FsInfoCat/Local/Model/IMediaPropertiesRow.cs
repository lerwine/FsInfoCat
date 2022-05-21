using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IMediaPropertiesRow" />
    /// <seealso cref="Upstream.Model.IMediaPropertiesRow" />
    public interface ILocalMediaPropertiesRow : ILocalPropertiesRow, IMediaPropertiesRow { }
}
