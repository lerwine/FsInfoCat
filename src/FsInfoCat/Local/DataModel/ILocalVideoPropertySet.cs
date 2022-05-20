using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Contains extended video file property values.
    /// </summary>
    /// <seealso cref="ILocalVideoPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IVideoPropertySet" />
    /// <seealso cref="IEquatable{ILocalVideoPropertySet}" />
    /// <seealso cref="Upstream.IUpstreamVideoPropertySet" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalVideoPropertySet")]
    public interface ILocalVideoPropertySet : ILocalVideoPropertiesRow, ILocalPropertySet, IVideoPropertySet, IEquatable<ILocalVideoPropertySet> { }
}

