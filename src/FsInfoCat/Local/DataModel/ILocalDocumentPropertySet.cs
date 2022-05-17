using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended document file property values.
    /// </summary>
    /// <seealso cref="ILocalDocumentPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IDocumentPropertySet" />
    /// <seealso cref="IEquatable{ILocalDocumentPropertySet}" />
    /// <seealso cref="Upstream.IUpstreamDocumentPropertySet" />
    public interface ILocalDocumentPropertySet : ILocalDocumentPropertiesRow, ILocalPropertySet, IDocumentPropertySet, IEquatable<ILocalDocumentPropertySet> { }
}
