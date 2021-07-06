using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for access error entities.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IAccessError{TTarget}" />
    /// <see cref="IVolume.AccessErrors"/>
    /// <see cref="ISubdirectory.AccessErrors"/>
    /// <see cref="IFile.AccessErrors"/>
    public interface IAccessError : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The <see cref="AccessErrorCode"/> value that represents the numeric error code.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ErrorCode), ResourceType = typeof(Properties.Resources))]
        AccessErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the brief error message.
        /// </summary>
        /// <value>The brief error message.</value>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets the error detail text.
        /// </summary>
        /// <value>The error detail text.</value>
        string Details { get; set; }

        /// <summary>
        /// Gets or sets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <see cref="IDbEntity"/> object that this error applies to.</value>
        IDbEntity Target { get; set; }
    }

    /// <summary>
    /// Generic interface for access error entities with a specific target type.
    /// </summary>
    /// <typeparam name="TTarget">The type of the entity that the access error applies to.</typeparam>
    /// <seealso cref="IAccessError" />
    /// <see cref="IVolume.AccessErrors"/>
    /// <see cref="ISubdirectory.AccessErrors"/>
    /// <see cref="IFile.AccessErrors"/>
    public interface IAccessError<TTarget> : IAccessError
        where TTarget : IDbEntity
    {
        /// <summary>
        /// Gets or sets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <typeparamref name="TTarget"/> object that this error applies to.</value>
        new TTarget Target { get; set; }
    }
}
