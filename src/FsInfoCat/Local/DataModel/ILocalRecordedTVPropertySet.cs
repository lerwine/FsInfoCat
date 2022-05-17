using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended recorded TV file property values.
    /// </summary>
    /// <seealso cref="ILocalRecordedTVPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IRecordedTVPropertySet" />
    /// <seealso cref="IEquatable{ILocalRecordedTVPropertySet}" />
    /// <seealso cref="Upstream.IUpstreamRecordedTVPropertySet" />
    public interface ILocalRecordedTVPropertySet : ILocalRecordedTVPropertiesRow, ILocalPropertySet, IRecordedTVPropertySet, IEquatable<ILocalRecordedTVPropertySet> { }
}
