using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended video file property values.
    /// </summary>
    /// <seealso cref="ILocalVideoPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IVideoPropertySet" />
    /// <seealso cref="IEquatable{ILocalVideoPropertySet}" />
    /// <seealso cref="Upstream.Model.IUpstreamVideoPropertySet" />
    public interface ILocalVideoPropertySet : ILocalVideoPropertiesRow, ILocalPropertySet, IVideoPropertySet, IEquatable<ILocalVideoPropertySet> { }
}

