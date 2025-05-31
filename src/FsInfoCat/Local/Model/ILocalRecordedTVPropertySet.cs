using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Contains extended recorded TV file property values.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamRecordedTVPropertySet" />
public interface ILocalRecordedTVPropertySet : ILocalRecordedTVPropertiesRow, ILocalPropertySet, IRecordedTVPropertySet, IEquatable<ILocalRecordedTVPropertySet> { }
