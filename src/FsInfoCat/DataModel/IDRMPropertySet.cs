using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file DRM property values.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IDRMProperties" />
    /// <seealso cref="Local.ILocalDRMPropertySet" />
    /// <seealso cref="Upstream.IUpstreamDRMPropertySet" />
    /// <seealso cref="IFile.DRMProperties" />
    /// <seealso cref="IDbContext.DRMPropertySets" />
    [System.Obsolete("Use FsInfoCat.Model.IDRMPropertySet")]
    public interface IDRMPropertySet : IPropertySet, IDRMPropertiesRow, IEquatable<IDRMPropertySet> { }
}
