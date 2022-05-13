namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="ISummaryPropertiesRow" />
    public interface ILocalSummaryPropertiesRow : ILocalPropertiesRow, ISummaryPropertiesRow { }
}
