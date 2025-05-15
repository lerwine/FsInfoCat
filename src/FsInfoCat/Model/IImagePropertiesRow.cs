using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IImagePropertiesListItem" />
    /// <seealso cref="IImagePropertySet" />
    public interface IImagePropertiesRow : IPropertiesRow, IImageProperties, IEquatable<IImagePropertiesRow> { }
}
