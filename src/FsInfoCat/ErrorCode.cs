using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Values for application error codes.</summary>
    public enum ErrorCode : int
    {
        /// <summary>An unexpected error has occurred.</summary>
        [AmbientValue(MessageCode.UnexpectedError)]
        [Display(Name = nameof(Properties.Resources.DisplayName_UnexpectedError), ShortName = nameof(Properties.Resources.DisplayName_UnexpectedError), Description = nameof(Properties.Resources.ErrorMessage_UnexpectedError), ResourceType = typeof(Properties.Resources))]
        Unexpected = 0,

        /// <summary>An <see cref="System.IO.IOException" /> has occurred.</summary>
        [AmbientValue(MessageCode.IOError)]
        [Display(Name = nameof(Properties.Resources.DisplayName_IOError), ShortName = nameof(Properties.Resources.DisplayName_IOError), Description = nameof(Properties.Resources.ErrorMessage_IOError), ResourceType = typeof(Properties.Resources))]
        IOError = 1,

        /// <summary>An <see cref="System.IO.UnauthorizedAccessException" /> has occurred.</summary>
        [AmbientValue(MessageCode.UnauthorizedAccess)]
        [Display(Name = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), ShortName = nameof(Properties.Resources.DisplayName_Unauthorized), Description = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), ResourceType = typeof(Properties.Resources))]
        UnauthorizedAccess = 2,

        /// <summary>An <see cref="System.IO.SecurityException" /> has occurred.</summary>
        [AmbientValue(MessageCode.SecurityException)]
        [Display(Name = nameof(Properties.Resources.DisplayName_SecurityException), ShortName = nameof(Properties.Resources.DisplayName_AccessDenied), Description = nameof(Properties.Resources.ErrorMessage_SecurityException), ResourceType = typeof(Properties.Resources))]
        SecurityException = 3,

        /// <summary>The name of one or more files or subdirectories in the path contain a invalid character(s).</summary>
        [AmbientValue(MessageCode.InvalidPath)]
        [Display(Name = nameof(Properties.Resources.DisplayName_InvalidPathError), ShortName = nameof(Properties.Resources.DisplayName_InvalidPath), Description = nameof(Properties.Resources.ErrorMessage_InvalidPathError), ResourceType = typeof(Properties.Resources))]
        InvalidPath = 4,

        /// <summary>A <see cref="System.IO.PathTooLongException" /> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.</summary>
        [AmbientValue(MessageCode.PathTooLong)]
        [Display(Name = nameof(Properties.Resources.DisplayName_PathTooLongError), ShortName = nameof(Properties.Resources.DisplayName_TooLong), Description = nameof(Properties.Resources.ErrorMessage_PathTooLongError), ResourceType = typeof(Properties.Resources))]
        PathTooLong = 5,

        /// <summary>Crawl operation has failed and terminated before completion.</summary>
        [AmbientValue(MessageCode.CrawlOperationFailed)]
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlOperationFailed), ShortName = nameof(Properties.Resources.DisplayName_CrawlFailed), Description = nameof(Properties.Resources.ErrorMessage_CrawlOperationFailed), ResourceType = typeof(Properties.Resources))]
        CrawlOperationFailed = 6,

        /// <summary>A <see cref="System.IO.DirectoryNotFoundException"/> has occurred.</summary>
        [AmbientValue(MessageCode.DirectoryNotFound)]
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryNotFound), ShortName = nameof(Properties.Resources.DisplayName_NotFound), Description = nameof(Properties.Resources.ErrorMessage_DirectoryNotFound), ResourceType = typeof(Properties.Resources))]
        DirectoryNotFound = 7,

        [AmbientValue(MessageCode.GetLogicalDisksFailure)]
        [Display(Name = nameof(Properties.Resources.DisplayName_GetLogicalDisksFailure), ShortName = nameof(Properties.Resources.DisplayName_GetLogicalDisksFailure), Description = nameof(Properties.Resources.ErrorMessage_GetLogicalDisksFailure), ResourceType = typeof(Properties.Resources))]
        GetLogicalDisksFailure = 8
    }
    public enum MessageCode : int
    {
        [AmbientValue(StatusMessageLevel.Error)]
        /// <summary>An unexpected error has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UnexpectedError), ShortName = nameof(Properties.Resources.DisplayName_UnexpectedError), Description = nameof(Properties.Resources.ErrorMessage_UnexpectedError), ResourceType = typeof(Properties.Resources))]
        UnexpectedError = 0,

        [AmbientValue(StatusMessageLevel.Error)]
        /// <summary>An <see cref="System.IO.IOException" /> has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_IOError), ShortName = nameof(Properties.Resources.DisplayName_IOError), Description = nameof(Properties.Resources.ErrorMessage_IOError), ResourceType = typeof(Properties.Resources))]
        IOError = 1,

        [AmbientValue(StatusMessageLevel.Error)]
        /// <summary>An <see cref="System.IO.UnauthorizedAccessException" /> has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), ShortName = nameof(Properties.Resources.DisplayName_Unauthorized), Description = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), ResourceType = typeof(Properties.Resources))]
        UnauthorizedAccess = 2,

        [AmbientValue(StatusMessageLevel.Error)]
        /// <summary>An <see cref="System.IO.SecurityException" /> has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_SecurityException), ShortName = nameof(Properties.Resources.DisplayName_AccessDenied), Description = nameof(Properties.Resources.ErrorMessage_SecurityException), ResourceType = typeof(Properties.Resources))]
        SecurityException = 3,

        [AmbientValue(StatusMessageLevel.Error)]
        /// <summary>The name of one or more files or subdirectories in the path contain a invalid character(s).</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_InvalidPathError), ShortName = nameof(Properties.Resources.DisplayName_InvalidPath), Description = nameof(Properties.Resources.ErrorMessage_InvalidPathError), ResourceType = typeof(Properties.Resources))]
        InvalidPath = 4,

        [AmbientValue(StatusMessageLevel.Error)]
        /// <summary>A <see cref="System.IO.PathTooLongException" /> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PathTooLongError), ShortName = nameof(Properties.Resources.DisplayName_TooLong), Description = nameof(Properties.Resources.ErrorMessage_PathTooLongError), ResourceType = typeof(Properties.Resources))]
        PathTooLong = 5,

        [AmbientValue(StatusMessageLevel.Error)]
        /// <summary>Crawl operation has failed and terminated before completion.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlOperationFailed), ShortName = nameof(Properties.Resources.DisplayName_CrawlFailed), Description = nameof(Properties.Resources.ErrorMessage_CrawlOperationFailed), ResourceType = typeof(Properties.Resources))]
        CrawlOperationFailed = 6,

        [AmbientValue(StatusMessageLevel.Error)]
        /// <summary>A <see cref="System.IO.DirectoryNotFoundException"/> has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryNotFound), ShortName = nameof(Properties.Resources.DisplayName_NotFound), Description = nameof(Properties.Resources.ErrorMessage_DirectoryNotFound), ResourceType = typeof(Properties.Resources))]
        DirectoryNotFound = 7,

        [AmbientValue(StatusMessageLevel.Error)]
        [Display(Name = nameof(Properties.Resources.DisplayName_GetLogicalDisksFailure), ShortName = nameof(Properties.Resources.DisplayName_GetLogicalDisksFailure), Description = nameof(Properties.Resources.ErrorMessage_GetLogicalDisksFailure), ResourceType = typeof(Properties.Resources))]
        GetLogicalDisksFailure = 8,

        /// <summary>An unknown/unspecified error has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_BackgroundJobPending), ShortName = nameof(Properties.Resources.DisplayName_Pending), Description = nameof(Properties.Resources.DisplayName_BackgroundJobPending), ResourceType = typeof(Properties.Resources))]
        BackgroundJobPending = 9,

        /// <summary>An unknown/unspecified error has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_BackgroundJobStarted), ShortName = nameof(Properties.Resources.DisplayName_Started), Description = nameof(Properties.Resources.DisplayName_BackgroundJobStarted), ResourceType = typeof(Properties.Resources))]
        BackgroundJobStarted = 10,

        [Display(Name = nameof(Properties.Resources.DisplayName_BackgroundJobCompleted), ShortName = nameof(Properties.Resources.DisplayName_Completed), Description = nameof(Properties.Resources.DisplayName_BackgroundJobCompleted), ResourceType = typeof(Properties.Resources))]
        BackgroundJobCompleted = 11,

        [Display(Name = nameof(Properties.Resources.DisplayName_CancellingBackgroundJob), ShortName = nameof(Properties.Resources.DisplayName_Cancelling), Description = nameof(Properties.Resources.DisplayName_CancellingBackgroundJob), ResourceType = typeof(Properties.Resources))]
        CancellingBackgroundJob = 12,

        [Display(Name = nameof(Properties.Resources.DisplayName_BackgroundJobCanceled), ShortName = nameof(Properties.Resources.DisplayName_Canceled), Description = nameof(Properties.Resources.DisplayName_BackgroundJobCanceled), ResourceType = typeof(Properties.Resources))]
        BackgroundJobCanceled = 13,

        [Display(Name = nameof(Properties.Resources.DisplayName_BackgroundJobFailed), ShortName = nameof(Properties.Resources.DisplayName_Failed), Description = nameof(Properties.Resources.DisplayName_BackgroundJobFailed), ResourceType = typeof(Properties.Resources))]
        BackgroundJobFaulted = 14
    }
}

