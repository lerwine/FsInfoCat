namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of document files.
    /// </summary>
    /// <seealso cref="IDocumentProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalDocumentPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamDocumentPropertySet"/>
    public interface IDocumentPropertySet : IDocumentProperties, IPropertySet
    {
    }
}
