using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Represents file system access error codes.</summary>
    [System.Obsolete("Use ErrorCode or MessageCode")]
    public enum AccessErrorCode : byte
    {
        /// <summary>An unknown/unspecified error has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UnexpectedError), ShortName = nameof(Properties.Resources.DisplayName_UnexpectedError), Description = nameof(Properties.Resources.Description_UnexpectedError),
            ResourceType = typeof(Properties.Resources))]
        Unspecified = 0,

        /// <summary>An <see cref="System.IO.IOException" /> has occurred.</summary>
        [AmbientValue(ErrorCode.IOError)]
        [Display(Name = nameof(Properties.Resources.DisplayName_IOError), ShortName = nameof(Properties.Resources.DisplayName_IOError), Description = nameof(Properties.Resources.Description_IOError),
            ResourceType = typeof(Properties.Resources))]
        IOError = 1,

        /// <summary>An <see cref="System.UnauthorizedAccessException" /> has occurred.</summary>
        [AmbientValue(ErrorCode.UnauthorizedAccess)]
        [Display(Name = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), ShortName = nameof(Properties.Resources.DisplayName_Unauthorized),
            Description = nameof(Properties.Resources.Description_UnauthorizedAccessError), ResourceType = typeof(Properties.Resources))]
        UnauthorizedAccess = 2,

        /// <summary>A <see cref="System.Security.SecurityException" /> has occurred.</summary>
        [AmbientValue(ErrorCode.SecurityException)]
        [Display(Name = nameof(Properties.Resources.DisplayName_SecurityException), ShortName = nameof(Properties.Resources.DisplayName_AccessDenied), Description = nameof(Properties.Resources.Description_SecurityException),
            ResourceType = typeof(Properties.Resources))]
        SecurityException = 3,

        /// <summary>The name of one or more files or subdirectories in the path contain a invalid character(s).</summary>
        [AmbientValue(ErrorCode.InvalidPath)]
        [Display(Name = nameof(Properties.Resources.DisplayName_InvalidPathError), ShortName = nameof(Properties.Resources.DisplayName_InvalidPath), Description = nameof(Properties.Resources.Description_InvalidPathError),
            ResourceType = typeof(Properties.Resources))]
        InvalidPath = 4,

        /// <summary>A <see cref="System.IO.PathTooLongException" /> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.</summary>
        [AmbientValue(ErrorCode.PathTooLong)]
        [Display(Name = nameof(Properties.Resources.DisplayName_PathTooLongError), ShortName = nameof(Properties.Resources.DisplayName_TooLong), Description = nameof(Properties.Resources.Description_PathTooLongError),
             ResourceType = typeof(Properties.Resources))]
        PathTooLong = 5
    }

}

