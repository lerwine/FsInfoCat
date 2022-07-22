using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Values for unique application error codes.
    /// </summary>
    /// <remarks>The <see cref="MessageCodeAttribute" /> determines the related <see cref="MessageCode" /> for the enumerated member.
    /// Use <see cref="MessageCodeAttribute.TryGetCode{TEnum}(TEnum, out MessageCode)"/> to determine the appropriate value.
    /// </remarks>
    public enum ErrorCode : int
    {
        /// <summary>
        /// An unexpected error has occurred.
        /// </summary>
        [MessageCode(MessageCode.UnexpectedError)]
        [Display(Name = nameof(Properties.Resources.UnexpectedError), ShortName = nameof(Properties.Resources.UnexpectedError), Description = nameof(Properties.Resources.ErrorMessage_UnexpectedError), ResourceType = typeof(Properties.Resources))]
        Unexpected = 0,

        /// <summary>
        /// An <see cref="System.IO.IOException" /> has occurred.
        /// </summary>
        [MessageCode(MessageCode.IOError)]
        [Display(Name = nameof(Properties.Resources.IOError), ShortName = nameof(Properties.Resources.IOError), Description = nameof(Properties.Resources.ErrorMessage_IOError), ResourceType = typeof(Properties.Resources))]
        IOError = 1,

        /// <summary>
        /// An <see cref="System.UnauthorizedAccessException" /> has occurred.
        /// </summary>
        [MessageCode(MessageCode.UnauthorizedAccess)]
        [Display(Name = nameof(Properties.Resources.UnauthorizedAccessError), ShortName = nameof(Properties.Resources.Unauthorized), Description = nameof(Properties.Resources.UnauthorizedAccessError), ResourceType = typeof(Properties.Resources))]
        UnauthorizedAccess = 2,

        /// <summary>
        /// A <see cref="System.Security.SecurityException" /> has occurred.
        /// </summary>
        [MessageCode(MessageCode.SecurityException)]
        [Display(Name = nameof(Properties.Resources.SecurityException), ShortName = nameof(Properties.Resources.AccessDenied), Description = nameof(Properties.Resources.ErrorMessage_SecurityException), ResourceType = typeof(Properties.Resources))]
        SecurityException = 3,

        /// <summary>
        /// The name of one or more files or subdirectories in the path contain a invalid character(s).
        /// </summary>
        [MessageCode(MessageCode.InvalidPath)]
        [Display(Name = nameof(Properties.Resources.InvalidPathError), ShortName = nameof(Properties.Resources.InvalidPath), Description = nameof(Properties.Resources.ErrorMessage_InvalidPathError), ResourceType = typeof(Properties.Resources))]
        InvalidPath = 4,

        /// <summary>
        /// A <see cref="System.IO.PathTooLongException" /> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.
        /// </summary>
        [MessageCode(MessageCode.PathTooLong)]
        [Display(Name = nameof(Properties.Resources.DisplayName_PathTooLongError), ShortName = nameof(Properties.Resources.TooLong), Description = nameof(Properties.Resources.ErrorMessage_PathTooLongError), ResourceType = typeof(Properties.Resources))]
        PathTooLong = 5,

        /// <summary>
        /// Crawl operation has failed and terminated before completion.
        /// </summary>
        [MessageCode(MessageCode.CrawlOperationFailed)]
        [Display(Name = nameof(Properties.Resources.CrawlOperationFailed), ShortName = nameof(Properties.Resources.CrawlFailed), Description = nameof(Properties.Resources.ErrorMessage_CrawlOperationFailed), ResourceType = typeof(Properties.Resources))]
        CrawlOperationFailed = 6,

        /// <summary>
        /// A <see cref="System.IO.DirectoryNotFoundException"/> has occurred.
        /// </summary>
        [MessageCode(MessageCode.DirectoryNotFound)]
        [Display(Name = nameof(Properties.Resources.DirectoryNotFound), ShortName = nameof(Properties.Resources.NotFound), Description = nameof(Properties.Resources.ErrorMessage_DirectoryNotFound), ResourceType = typeof(Properties.Resources))]
        DirectoryNotFound = 7,

        // TODO: Document ErrorCode fields
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [MessageCode(MessageCode.GetLogicalDisksFailure)]
        [Display(Name = nameof(Properties.Resources.GetLogicalDisksFailure), ShortName = nameof(Properties.Resources.GetLogicalDisksFailure), Description = nameof(Properties.Resources.ErrorMessage_GetLogicalDisksFailure), ResourceType = typeof(Properties.Resources))]
        GetLogicalDisksFailure = 8,

        [MessageCode(MessageCode.BackgroundJobFaulted)]
        [Display(Name = nameof(Properties.Resources.BackgroundJobFaulted), ShortName = nameof(Properties.Resources.Failed), Description = nameof(Properties.Resources.BackgroundJobFaulted), ResourceType = typeof(Properties.Resources))]
        BackgroundJobFaulted = 9
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}

