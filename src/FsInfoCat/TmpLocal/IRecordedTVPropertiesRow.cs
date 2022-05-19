using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="M.IRecordedTVPropertiesRow" />
    /// <seealso cref="Upstream.Model.IRecordedTVPropertiesRow" />
    public interface ILocalRecordedTVPropertiesRow : ILocalPropertiesRow, M.IRecordedTVPropertiesRow { }
}
