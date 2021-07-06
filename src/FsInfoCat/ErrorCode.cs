using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public enum ErrorCode
    {
        [Display(Name = nameof(Properties.Resources.DisplayName_UnexpectedError), Description = nameof(Properties.Resources.ErrorMessage_UnexpectedError), ResourceType = typeof(Properties.Resources))]
        Unexpected = 0,

        /// <summary>
        /// An <see cref="System.IO.IOException"/> has occurred.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_IOError), Description = nameof(Properties.Resources.ErrorMessage_IOError), ResourceType = typeof(Properties.Resources))]
        IOError = 1,

        /// <summary>
        /// An <see cref="System.UnauthorizedAccessException"/> has occurred.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), Description = nameof(Properties.Resources.ErrorMessage_UnauthorizedAccessError), ResourceType = typeof(Properties.Resources))]
        UnauthorizedAccess = 2,

        /// <summary>
        /// A <see cref="System.Security.SecurityException"/> has occurred.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_SecurityException), Description = nameof(Properties.Resources.ErrorMessage_SecurityException), ResourceType = typeof(Properties.Resources))]
        SecurityException = 3,

        /// <summary>
        /// The name of one or more files or subdirectories in the path contain a invalid character(s).
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_InvalidPathError), Description = nameof(Properties.Resources.ErrorMessage_InvalidPathError), ResourceType = typeof(Properties.Resources))]
        InvalidPath = 4,

        /// <summary>
        /// A <see cref="System.IO.PathTooLongException"/> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PathTooLongError), Description = nameof(Properties.Resources.ErrorMessage_PathTooLongError), ResourceType = typeof(Properties.Resources))]
        PathTooLong = 5,

        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlOperationFailed), Description = nameof(Properties.Resources.ErrorMessage_CrawlOperationFailed), ResourceType = typeof(Properties.Resources))]
        CrawlOperationFailed = 6
    }
}
