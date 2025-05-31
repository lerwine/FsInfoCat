using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Contains extended document file property values.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamDocumentPropertySet" />
public interface ILocalDocumentPropertySet : ILocalDocumentPropertiesRow, ILocalPropertySet, IDocumentPropertySet, IEquatable<ILocalDocumentPropertySet> { }
