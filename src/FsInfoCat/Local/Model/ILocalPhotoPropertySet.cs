using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Contains extended photo file property values.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamPhotoPropertySet" />
public interface ILocalPhotoPropertySet : ILocalPhotoPropertiesRow, ILocalPropertySet, IPhotoPropertySet, IEquatable<ILocalPhotoPropertySet> { }
