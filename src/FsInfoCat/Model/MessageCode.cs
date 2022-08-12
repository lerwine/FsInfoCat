using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Unique value to represent a specific message code.
    /// </summary>
    /// <remarks>
    ///   <list type="bullet">
    ///     <item>The <see cref="StatusMessageLevelAttribute" /> determines the related <see cref="StatusMessageLevel" /> for the enumerated member.
    ///         Use <see cref="StatusMessageLevelAttribute.TryGetLevel{TEnum}(TEnum, out StatusMessageLevel)"/> to determine the appropriate value.</item>
    ///     <item>The <see cref="ErrorCodeAttribute"/> indicates the related <see cref="ErrorCode"/> if present; otherwise, the enuemrated member has no
    ///         related <see cref="ErrorCode"/>. Use <see cref="ErrorCodeAttribute.TryGetCode{TEnum}(TEnum, out ErrorCode)"/> to determine
    ///         what, if any, <see cref="ErrorCode"/> is associated.</item>
    ///   </list>
    /// </remarks>
    public enum MessageCode2 : int
    {
        /// <summary>
        /// An unexpected error has occurred.
        /// </summary>
        /// <remarks>This field must remain the default field and indicate an unexpected error so the default message code of <see cref="ErrorCode"/> values will make logical
        /// sense.</remarks>
        [StatusMessageLevel(StatusMessageLevel.Error)]
        [ErrorCode(ErrorCode.Unexpected)]
        [Display(Name = nameof(Properties.Resources.UnexpectedError), ShortName = nameof(Properties.Resources.UnexpectedError), Description = nameof(Properties.Resources.UnexpectedError), ResourceType = typeof(Properties.Resources))]
        UnexpectedError = 0,

        /// <summary>
        /// An <see cref="System.IO.IOException" /> has occurred.
        /// </summary>
        [StatusMessageLevel(StatusMessageLevel.Error)]
        [ErrorCode(ErrorCode.IOError)]
        [Display(Name = nameof(Properties.Resources.IOError), ShortName = nameof(Properties.Resources.IOError), Description = nameof(Properties.Resources.ErrorMessage_IOError), ResourceType = typeof(Properties.Resources))]
        IOError = 1,

        /// <summary>
        /// An <see cref="System.UnauthorizedAccessException" /> has occurred.
        /// </summary>
        [StatusMessageLevel(StatusMessageLevel.Error)]
        [ErrorCode(ErrorCode.UnauthorizedAccess)]
        [Display(Name = nameof(Properties.Resources.UnauthorizedAccessError), ShortName = nameof(Properties.Resources.Unauthorized), Description = nameof(Properties.Resources.UnauthorizedAccessError), ResourceType = typeof(Properties.Resources))]
        UnauthorizedAccess = 2,

        /// <summary>
        /// A <see cref="System.Security.SecurityException" /> has occurred.
        /// </summary>
        [StatusMessageLevel(StatusMessageLevel.Error)]
        [ErrorCode(ErrorCode.SecurityException)]
        [Display(Name = nameof(Properties.Resources.SecurityException), ShortName = nameof(Properties.Resources.AccessDenied), Description = nameof(Properties.Resources.ErrorMessage_SecurityException), ResourceType = typeof(Properties.Resources))]
        SecurityException = 3,

        /// <summary>
        /// The name of one or more files or subdirectories in the path contain a invalid character(s).
        /// </summary>
        [StatusMessageLevel(StatusMessageLevel.Error)]
        [ErrorCode(ErrorCode.InvalidPath)]
        [Display(Name = nameof(Properties.Resources.InvalidPathError), ShortName = nameof(Properties.Resources.InvalidPath), Description = nameof(Properties.Resources.ErrorMessage_InvalidPathError), ResourceType = typeof(Properties.Resources))]
        InvalidPath = 4,

        /// <summary>
        /// A <see cref="System.IO.PathTooLongException" /> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.
        /// </summary>
        [StatusMessageLevel(StatusMessageLevel.Error)]
        [ErrorCode(ErrorCode.PathTooLong)]
        [Display(Name = nameof(Properties.Resources.DisplayName_PathTooLongError), ShortName = nameof(Properties.Resources.TooLong), Description = nameof(Properties.Resources.PathTooLongError), ResourceType = typeof(Properties.Resources))]
        PathTooLong = 5,

        /// <summary>
        /// Crawl operation has failed and terminated before completion.
        /// </summary>
        [StatusMessageLevel(StatusMessageLevel.Error)]
        [ErrorCode(ErrorCode.CrawlOperationFailed)]
        [Display(Name = nameof(Properties.Resources.CrawlOperationFailed), ShortName = nameof(Properties.Resources.CrawlFailed), Description = nameof(Properties.Resources.ErrorMessage_CrawlOperationFailed), ResourceType = typeof(Properties.Resources))]
        CrawlOperationFailed = 6,

        /// <summary>
        /// A <see cref="System.IO.DirectoryNotFoundException"/> has occurred.
        /// </summary>
        [StatusMessageLevel(StatusMessageLevel.Error)]
        [ErrorCode(ErrorCode.DirectoryNotFound)]
        [Display(Name = nameof(Properties.Resources.DirectoryNotFound), ShortName = nameof(Properties.Resources.NotFound), Description = nameof(Properties.Resources.ErrorMessage_DirectoryNotFound), ResourceType = typeof(Properties.Resources))]
        DirectoryNotFound = 7,

    // TODO: Document MessageCode members
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [StatusMessageLevel(StatusMessageLevel.Error)]
        [ErrorCode(ErrorCode.GetLogicalDisksFailure)]
        [Display(Name = nameof(Properties.Resources.GetLogicalDisksFailure), ShortName = nameof(Properties.Resources.GetLogicalDisksFailure), Description = nameof(Properties.Resources.ErrorMessage_GetLogicalDisksFailure), ResourceType = typeof(Properties.Resources))]
        GetLogicalDisksFailure = 8,

        [Display(Name = nameof(Properties.Resources.BackgroundJobPending), ShortName = nameof(Properties.Resources.Pending), Description = nameof(Properties.Resources.BackgroundJobPending), ResourceType = typeof(Properties.Resources))]
        BackgroundJobPending = 9,

        [Display(Name = nameof(Properties.Resources.BackgroundJobStarted), ShortName = nameof(Properties.Resources.Started), Description = nameof(Properties.Resources.BackgroundJobStarted), ResourceType = typeof(Properties.Resources))]
        BackgroundJobStarted = 10,

        [Display(Name = nameof(Properties.Resources.BackgroundJobCompleted), ShortName = nameof(Properties.Resources.Completed), Description = nameof(Properties.Resources.BackgroundJobCompleted), ResourceType = typeof(Properties.Resources))]
        BackgroundJobCompleted = 11,

        [StatusMessageLevel(StatusMessageLevel.Warning)]
        [Display(Name = nameof(Properties.Resources.CancellingBackgroundJob), ShortName = nameof(Properties.Resources.Cancelling), Description = nameof(Properties.Resources.CancellingBackgroundJob), ResourceType = typeof(Properties.Resources))]
        CancellingBackgroundJob = 12,

        [StatusMessageLevel(StatusMessageLevel.Warning)]
        [Display(Name = nameof(Properties.Resources.BackgroundJobCanceled), ShortName = nameof(Properties.Resources.Canceled), Description = nameof(Properties.Resources.BackgroundJobCanceled), ResourceType = typeof(Properties.Resources))]
        BackgroundJobCanceled = 13,

        [StatusMessageLevel(StatusMessageLevel.Error)]
        [ErrorCode(ErrorCode.BackgroundJobFaulted)]
        [Display(Name = nameof(Properties.Resources.BackgroundJobFaulted), ShortName = nameof(Properties.Resources.Failed), Description = nameof(Properties.Resources.BackgroundJobFaulted), ResourceType = typeof(Properties.Resources))]
        BackgroundJobFaulted = 14,

        [StatusMessageLevel(StatusMessageLevel.Warning)]
        [Display(Name = nameof(Properties.Resources.ItemLimitReached), ShortName = nameof(Properties.Resources.ItemLimitReached), Description = nameof(Properties.Resources.ItemLimitReached), ResourceType = typeof(Properties.Resources))]
        ItemLimitReached = 15,

        [StatusMessageLevel(StatusMessageLevel.Warning)]
        [Display(Name = nameof(Properties.Resources.TimeLimitReached), ShortName = nameof(Properties.Resources.TimeLimitReached), Description = nameof(Properties.Resources.TimeLimitReached), ResourceType = typeof(Properties.Resources))]
        TimeLimitReached = 16,

        [Display(Name = nameof(Properties.Resources.CrawlingSubdirectory), ShortName = nameof(Properties.Resources.CrawlingSubdirectory), Description = nameof(Properties.Resources.CrawlingSubdirectory), ResourceType = typeof(Properties.Resources))]
        CrawlingSubdirectory = 17,

        [Display(Name = nameof(Properties.Resources.ReadingFileInformation), ShortName = nameof(Properties.Resources.ReadingFileInformation), Description = nameof(Properties.Resources.ReadingFileInformation), ResourceType = typeof(Properties.Resources))]
        ReadingFileInformation = 18
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}

