using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Generic interface for access error entities.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IAccessError : IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>Gets the error code.</summary>
        /// <value>The <see cref="AccessErrorCode" /> value that represents the numeric error code.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ErrorCode), ResourceType = typeof(Properties.Resources))]
        AccessErrorCode ErrorCode { get; }

        /// <summary>Gets the brief error message.</summary>
        /// <value>The brief error message.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Message), ResourceType = typeof(Properties.Resources))]
        string Message { get; }

        /// <summary>Gets the error detail text.</summary>
        /// <value>The error detail text.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Details), ResourceType = typeof(Properties.Resources))]
        string Details { get; }

        /// <summary>Gets the target entity to which the access error applies.</summary>
        /// <value>The <see cref="IDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        IDbEntity Target { get; }
    }
}

