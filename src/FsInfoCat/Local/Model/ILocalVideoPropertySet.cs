using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Contains extended video file property values.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamVideoPropertySet" />
public interface ILocalVideoPropertySet : ILocalVideoPropertiesRow, ILocalPropertySet, IVideoPropertySet, IEquatable<ILocalVideoPropertySet> { }

