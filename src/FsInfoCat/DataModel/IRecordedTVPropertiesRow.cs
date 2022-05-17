using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IRecordedTVProperties" />
    /// <seealso cref="IEquatable{IRecordedTVPropertiesRow}" />
    /// <seealso cref="Local.ILocalRecordedTVPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamRecordedTVPropertiesRow" />
    public interface IRecordedTVPropertiesRow : IPropertiesRow, IRecordedTVProperties, IEquatable<IRecordedTVPropertiesRow> { }
}