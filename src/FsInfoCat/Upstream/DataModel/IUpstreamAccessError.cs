using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for access error entities that from the remote host system database.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IUpstreamFileAccessError" />
    /// <seealso cref="IUpstreamSubdirectoryAccessError" />
    /// <seealso cref="IUpstreamVolumeAccessError" />
    /// <seealso cref="IAccessError" />
    /// <seealso cref="Local.ILocalAccessError" />
    /// <seealso cref="IUpstreamDbFsItem.AccessErrors" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamAccessError")]
    public interface IUpstreamAccessError : IAccessError
    {
        /// <summary>
        /// Gets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IUpstreamDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamDbEntity Target { get; }
    }
}
