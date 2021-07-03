using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Base interface for entities that represent a grouping of extended file properties.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IAudioPropertySet" />
    /// <seealso cref="IBinaryPropertySet" />
    /// <seealso cref="IDocumentPropertySet" />
    /// <seealso cref="IDRMPropertySet" />
    /// <seealso cref="IGPSPropertySet" />
    /// <seealso cref="IImagePropertySet" />
    /// <seealso cref="IMediaPropertySet" />
    /// <seealso cref="IMusicPropertySet" />
    /// <seealso cref="IPhotoPropertySet" />
    /// <seealso cref="IRecordedTVPropertySet" />
    /// <seealso cref="ISummaryPropertySet" />
    /// <seealso cref="IVideoPropertySet" />
    public interface IPropertySet : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets the files that share the same property values as this property set.
        /// </summary>
        /// <value>The <see cref="IFile">files</see> that share the same property values as this property set.</value>
        IEnumerable<IFile> Files { get; }
    }
}
