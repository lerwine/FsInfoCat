namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="ILocalSummaryPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="ISummaryPropertiesListItem" />
    public interface ILocalSummaryPropertiesListItem : ILocalSummaryPropertiesRow, ILocalPropertiesListItem, ISummaryPropertiesListItem { }
}
