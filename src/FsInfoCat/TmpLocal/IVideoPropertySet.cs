using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Contains extended video file property values.
    /// </summary>
    /// <seealso cref="ILocalVideoPropertiesRow" />
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="M.IVideoPropertySet" />
    /// <seealso cref="IEquatable{ILocalVideoPropertySet}" />
    /// <seealso cref="Upstream.Model.IVideoPropertySet" />
    public interface ILocalVideoPropertySet : ILocalVideoPropertiesRow, ILocalPropertySet, M.IVideoPropertySet, IEquatable<ILocalVideoPropertySet> { }
}

