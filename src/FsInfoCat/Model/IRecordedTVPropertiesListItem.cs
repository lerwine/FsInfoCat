using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    /// <seealso cref="IRecordedTVPropertySet" />
    /// <seealso cref="IEquatable{IRecordedTVPropertiesListItem}" />
    /// <seealso cref="Local.Model.ILocalRecordedTVPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamRecordedTVPropertiesListItem" />
    /// <seealso cref="IDbContext.RecordedTVPropertiesListing" />
    public interface IRecordedTVPropertiesListItem : IPropertiesListItem, IRecordedTVPropertiesRow, IEquatable<IRecordedTVPropertiesListItem> { }
}
