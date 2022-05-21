using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IVideoProperties" />
    /// <seealso cref="IEquatable{IVideoPropertiesRow}" />
    /// <seealso cref="Local.Model.ILocalVideoPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamVideoPropertiesRow" />
    public interface IVideoPropertiesRow : IPropertiesRow, IVideoProperties, IEquatable<IVideoPropertiesRow> { }
}
