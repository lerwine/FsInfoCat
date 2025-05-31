using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Contains extended media file property values.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamMediaPropertySet" />
public interface ILocalMediaPropertySet : ILocalMediaPropertiesRow, ILocalPropertySet, IMediaPropertySet, IEquatable<ILocalMediaPropertySet> { }
