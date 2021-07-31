using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Represents file system access error codes.</summary>
    public enum AccessErrorCode : byte
    {
        /// <summary>An unknown/unspecified error has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UnexpectedError), ResourceType = typeof(Properties.Resources))]
        Unspecified = 0,

        /// <summary>An <see cref="System.IO.IOException" /> has occurred.</summary>
        [AmbientValue(ErrorCode.IOError)]
        [Display(Name = nameof(Properties.Resources.DisplayName_IOError), ResourceType = typeof(Properties.Resources))]
        IOError = 1,

        /// <summary>An <see cref="System.UnauthorizedAccessException" /> has occurred.</summary>
        [AmbientValue(ErrorCode.UnauthorizedAccess)]
        [Display(Name = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), ResourceType = typeof(Properties.Resources))]
        UnauthorizedAccess = 2,

        /// <summary>A <see cref="System.Security.SecurityException" /> has occurred.</summary>
        [AmbientValue(ErrorCode.SecurityException)]
        [Display(Name = nameof(Properties.Resources.DisplayName_SecurityException), ResourceType = typeof(Properties.Resources))]
        SecurityException = 3,

        /// <summary>The name of one or more files or subdirectories in the path contain a invalid character(s).</summary>
        [AmbientValue(ErrorCode.InvalidPath)]
        [Display(Name = nameof(Properties.Resources.DisplayName_InvalidPathError), ResourceType = typeof(Properties.Resources))]
        InvalidPath = 4,

        /// <summary>A <see cref="System.IO.PathTooLongException" /> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.</summary>
        [AmbientValue(ErrorCode.PathTooLong)]
        [Display(Name = nameof(Properties.Resources.DisplayName_PathTooLongError), ResourceType = typeof(Properties.Resources))]
        PathTooLong = 5
    }

}

