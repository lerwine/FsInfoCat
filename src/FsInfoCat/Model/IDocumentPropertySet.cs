using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of document files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IDocumentProperties" />
    /// <seealso cref="Local.Model.ILocalDocumentPropertySet" />
    /// <seealso cref="Upstream.Model.IUpstreamDocumentPropertySet" />
    /// <seealso cref="IFile.DocumentProperties" />
    /// <seealso cref="IDbContext.DocumentPropertySets" />
    public interface IDocumentPropertySet : IPropertySet, IDocumentPropertiesRow, IEquatable<IDocumentPropertySet> { }
}
