using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of document files.
    /// </summary>
    /// <seealso cref="IDocumentPropertiesListItem" />
    /// <seealso cref="IDbContext.DocumentPropertySets"/>
    public interface IDocumentPropertySet : IPropertySet, IDocumentPropertiesRow, IEquatable<IDocumentPropertySet> { }
}
