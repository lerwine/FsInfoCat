using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended recorded TV file property values.
    /// </summary>
    /// <seealso cref="IUpstreamRecordedTVPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="M.IRecordedTVPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamRecordedTVPropertySet}" />
    /// <seealso cref="Local.Model.IRecordedTVPropertySet" />
    public interface IUpstreamRecordedTVPropertySet : IUpstreamRecordedTVPropertiesRow, IUpstreamPropertySet, M.IRecordedTVPropertySet, IEquatable<IUpstreamRecordedTVPropertySet> { }
}
