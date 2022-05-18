using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of image files.
    /// </summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IImageProperties" />
    /// <seealso cref="Local.ILocalImagePropertySet" />
    /// <seealso cref="Upstream.IUpstreamImagePropertySet" />
    /// <seealso cref="IFile.ImageProperties" />
    /// <seealso cref="IDbContext.ImagePropertySets" />
    public interface IImagePropertySet : IPropertySet, IImagePropertiesRow, IEquatable<IImagePropertySet> { }
}
