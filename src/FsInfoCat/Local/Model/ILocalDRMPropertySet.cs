using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Contains extended DRM property values.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamDRMPropertySet" />
public interface ILocalDRMPropertySet : ILocalDRMPropertiesRow, ILocalPropertySet, IDRMPropertySet, IEquatable<ILocalDRMPropertySet> { }
