using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IRecordedTVProperties" />
    /// <seealso cref="IEquatable{IRecordedTVPropertiesRow}" />
    /// <seealso cref="Local.Model.ILocalRecordedTVPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamRecordedTVPropertiesRow" />
    public interface IRecordedTVPropertiesRow : IPropertiesRow, IRecordedTVProperties, IEquatable<IRecordedTVPropertiesRow> { }
}
