using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="ISummaryPropertiesRow" />
    /// <seealso cref="Upstream.Model.ISummaryPropertiesRow" />
    public interface ILocalSummaryPropertiesRow : ILocalPropertiesRow, ISummaryPropertiesRow { }
}
