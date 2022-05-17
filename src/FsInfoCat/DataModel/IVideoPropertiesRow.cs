using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IVideoProperties" />
    /// <seealso cref="IEquatable{IVideoPropertiesRow}" />
    /// <seealso cref="Local.ILocalVideoPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamVideoPropertiesRow" />
    public interface IVideoPropertiesRow : IPropertiesRow, IVideoProperties, IEquatable<IVideoPropertiesRow> { }
}