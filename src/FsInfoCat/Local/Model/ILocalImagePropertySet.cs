using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Contains extended image file property values.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamImagePropertySet" />
public interface ILocalImagePropertySet : ILocalImagePropertiesRow, ILocalPropertySet, IImagePropertySet, IEquatable<ILocalImagePropertySet> { }
