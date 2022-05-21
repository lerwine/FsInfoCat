using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Contains extended recorded TV file property values.
    /// </summary>
    /// <seealso cref="IUpstreamRecordedTVPropertiesRow" />
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IRecordedTVPropertySet" />
    /// <seealso cref="IEquatable{IUpstreamRecordedTVPropertySet}" />
    /// <seealso cref="Local.Model.ILocalRecordedTVPropertySet" />
    public interface IUpstreamRecordedTVPropertySet : IUpstreamRecordedTVPropertiesRow, IUpstreamPropertySet, IRecordedTVPropertySet, IEquatable<IUpstreamRecordedTVPropertySet> { }
}
