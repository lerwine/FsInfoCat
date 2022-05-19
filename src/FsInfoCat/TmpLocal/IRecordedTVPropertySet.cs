using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended recorded TV file property values.
    /// </summary>
    /// <seealso cref="ILocalRecordedTVPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="M.IRecordedTVPropertySet" />
    /// <seealso cref="IEquatable{ILocalRecordedTVPropertySet}" />
    /// <seealso cref="Upstream.Model.IRecordedTVPropertySet" />
    public interface ILocalRecordedTVPropertySet : ILocalRecordedTVPropertiesRow, ILocalPropertySet, M.IRecordedTVPropertySet, IEquatable<ILocalRecordedTVPropertySet> { }
}
