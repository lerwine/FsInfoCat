using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database objects that contain extended file DRM property values.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IDRMProperties" />
    /// <seealso cref="Local.Model.ILocalDRMPropertySet" />
    /// <seealso cref="Upstream.Model.IUpstreamDRMPropertySet" />
    /// <seealso cref="IFile.DRMProperties" />
    /// <seealso cref="IDbContext.DRMPropertySets" />
    public interface IDRMPropertySet : IPropertySet, IDRMPropertiesRow, IEquatable<IDRMPropertySet> { }
}