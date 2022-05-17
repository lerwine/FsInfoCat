using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of document files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IDocumentProperties" />
    /// <seealso cref="Local.ILocalDocumentPropertySet" />
    /// <seealso cref="Upstream.IUpstreamDocumentPropertySet" />
    public interface IDocumentPropertySet : IPropertySet, IDocumentPropertiesRow, IEquatable<IDocumentPropertySet> { }
}
