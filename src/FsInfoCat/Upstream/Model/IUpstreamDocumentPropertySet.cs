using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended document file property values.
    /// </summary>
    /// <seealso cref="IUpstreamDocumentPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IDocumentPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamDocumentPropertySet}" />
    /// <seealso cref="Local.Model.ILocalDocumentPropertySet" />
    public interface IUpstreamDocumentPropertySet : IUpstreamDocumentPropertiesRow, IUpstreamPropertySet, IDocumentPropertySet, IEquatable<IUpstreamDocumentPropertySet> { }
}
