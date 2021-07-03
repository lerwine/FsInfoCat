namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file summary property values.
    /// </summary>
    /// <seealso cref="ISummaryProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalSummaryPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamSummaryPropertySet"/>
    /// <seealso cref="IFile.SummaryProperties"/>
    public interface ISummaryPropertySet : ISummaryProperties, IPropertySet
    {
    }
}
