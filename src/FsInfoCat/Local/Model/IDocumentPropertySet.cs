using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended document file property values.
    /// </summary>
    /// <seealso cref="ILocalDocumentPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IDocumentPropertySet" />
    /// <seealso cref="IEquatable{ILocalDocumentPropertySet}" />
    /// <seealso cref="Upstream.Model.IDocumentPropertySet" />
    public interface ILocalDocumentPropertySet : ILocalDocumentPropertiesRow, ILocalPropertySet, IDocumentPropertySet, IEquatable<ILocalDocumentPropertySet> { }
}
