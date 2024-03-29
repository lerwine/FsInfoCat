using FsInfoCat.Model;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for access error entities that from the local host system database.
    /// </summary>
    /// <seealso cref="IAccessError" />
    /// <seealso cref="ILocalFileAccessError" />
    /// <seealso cref="ILocalSubdirectoryAccessError" />
    /// <seealso cref="ILocalVolumeAccessError" />
    /// <seealso cref="ILocalDbFsItem.AccessErrors" />
    /// <seealso cref="Upstream.Model.IUpstreamAccessError" />
    public interface ILocalAccessError : IAccessError
    {
        /// <summary>
        /// Gets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <see cref="ILocalDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.Target), ResourceType = typeof(Properties.Resources))]
        new ILocalDbEntity Target { get; }
    }
}
