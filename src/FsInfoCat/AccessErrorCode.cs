using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public enum AccessErrorCode : byte
    {
        Unspecified = 0,

        /// <summary>
        /// An <see cref="System.IO.IOException"/> has occurred.
        /// </summary>
        [AmbientValue(ErrorCode.IOError)]
        [Display(Name = nameof(Properties.Resources.DisplayName_IOError), ResourceType = typeof(Properties.Resources))]
        IOError = ErrorCode.IOError,

        /// <summary>
        /// An <see cref="System.UnauthorizedAccessException"/> has occurred.
        /// </summary>
        [AmbientValue(ErrorCode.UnauthorizedAccess)]
        [Display(Name = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), ResourceType = typeof(Properties.Resources))]
        UnauthorizedAccess = ErrorCode.UnauthorizedAccess,

        /// <summary>
        /// A <see cref="System.Security.SecurityException"/> has occurred.
        /// </summary>
        [AmbientValue(ErrorCode.SecurityException)]
        [Display(Name = nameof(Properties.Resources.DisplayName_SecurityException), ResourceType = typeof(Properties.Resources))]
        SecurityException = ErrorCode.SecurityException,

        /// <summary>
        /// The name of one or more files or subdirectories in the path contain a invalid character(s).
        /// </summary>
        [AmbientValue(ErrorCode.InvalidPath)]
        [Display(Name = nameof(Properties.Resources.DisplayName_InvalidPathError), ResourceType = typeof(Properties.Resources))]
        InvalidPath = ErrorCode.InvalidPath,

        /// <summary>
        /// A <see cref="System.IO.PathTooLongException"/> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.
        /// </summary>
        [AmbientValue(ErrorCode.PathTooLong)]
        [Display(Name = nameof(Properties.Resources.DisplayName_PathTooLongError), ResourceType = typeof(Properties.Resources))]
        PathTooLong = ErrorCode.PathTooLong
    }
}
