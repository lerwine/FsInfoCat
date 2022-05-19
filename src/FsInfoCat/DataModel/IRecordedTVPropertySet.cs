using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of recorded TV files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IRecordedTVProperties" />
    /// <seealso cref="Local.ILocalRecordedTVPropertySet" />
    /// <seealso cref="Upstream.IUpstreamRecordedTVPropertySet" />
    /// <seealso cref="IFile.RecordedTVProperties" />
    /// <seealso cref="IDbContext.RecordedTVPropertySets" />
    [System.Obsolete("Use FsInfoCat.Model.IRecordedTVPropertySet")]
    public interface IRecordedTVPropertySet : IPropertySet, IRecordedTVPropertiesRow, IEquatable<IRecordedTVPropertySet> { }
}
