using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Contains extended summary file property values.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamSummaryPropertySet" />
public interface ILocalSummaryPropertySet : ILocalSummaryPropertiesRow, ILocalPropertySet, ISummaryPropertySet, IEquatable<ILocalSummaryPropertySet> { }
