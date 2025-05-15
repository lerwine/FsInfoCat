using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IMediaPropertiesListItem" />
    /// <seealso cref="IMediaPropertySet" />
    public interface IMediaPropertiesRow : IPropertiesRow, IMediaProperties, IEquatable<IMediaPropertiesRow> { }
}
