using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of recorded TV files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IRecordedTVProperties" />
    /// <seealso cref="Local.Model.ILocalRecordedTVPropertySet" />
    /// <seealso cref="Upstream.Model.IUpstreamRecordedTVPropertySet" />
    /// <seealso cref="IFile.RecordedTVProperties" />
    /// <seealso cref="IDbContext.RecordedTVPropertySets" />
    public interface IRecordedTVPropertySet : IPropertySet, IRecordedTVPropertiesRow, IEquatable<IRecordedTVPropertySet> { }
}