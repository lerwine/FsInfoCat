using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities that represent a grouping of extended file properties.
    /// </summary>
    /// <seealso cref="IAudioPropertySet" />
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
    public interface IPropertySet : IPropertiesRow
    {
        /// <summary>
        /// Gets the files that share the same property values as this property set.
        /// </summary>
        /// <value>The <see cref="IFile">files</see> that share the same property values as this property set.</value>
        [Display(Name = nameof(Properties.Resources.Files), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFile> Files { get; }
    }
}
