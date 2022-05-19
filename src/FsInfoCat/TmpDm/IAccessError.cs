using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for access error entities.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IHasSimpleIdentifier" />
    /// <seealso cref="IFileAccessError" />
    /// <seealso cref="ISubdirectoryAccessError" />
    /// <seealso cref="IVolumeAccessError" />
    /// <seealso cref="Local.ILocalAccessError" />
    /// <seealso cref="Upstream.IUpstreamAccessError" />
    /// <seealso cref="IDbFsItem.AccessErrors" />
    public interface IAccessError : IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>The <see cref="FsInfoCat.ErrorCode" /> value that represents the numeric error code.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ErrorCode), ResourceType = typeof(Properties.Resources))]
        ErrorCode ErrorCode { get; }

        /// <summary>
        /// Gets the brief error message.
        /// </summary>
        /// <value>The brief error message.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Message), ResourceType = typeof(Properties.Resources))]
        string Message { get; }

        /// <summary>
        /// Gets the error detail text.
        /// </summary>
        /// <value>The error detail text.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Details), ResourceType = typeof(Properties.Resources))]
        string Details { get; }

        /// <summary>
        /// Gets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        IDbEntity Target { get; }

        /// <summary>
        /// Gets the unique identifier of the <see cref="Target" /> entity if it has been assigned.
        /// </summary>
        /// <param name="id">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the unique identifier of the <see cref="Target" /> entity has been set; otherwise, <see langword="false" />.</returns>
        bool TryGetTargetId(out Guid id);
    }
}
