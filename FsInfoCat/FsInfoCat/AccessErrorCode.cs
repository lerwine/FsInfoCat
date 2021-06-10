namespace FsInfoCat
{
    public enum AccessErrorCode
    {
        Unspecified = 0,

        /// <summary>
        /// An <see cref="System.IO.IOException"/> has occurred.
        /// </summary>
        IOError = 1,

        /// <summary>
        /// An <see cref="System.UnauthorizedAccessException"/> has occurred.
        /// </summary>
        UnauthorizedAccess = 2,

        /// <summary>
        /// A <see cref="System.Security.SecurityException"/> has occurred.
        /// </summary>
        SecurityException = 3,

        /// <summary>
        /// The name of one or more files or subdirectories in the path contain a invalid character(s).
        /// </summary>
        InvalidPath = 4,

        /// <summary>
        /// A <see cref="System.IO.PathTooLongException"/> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.
        /// </summary>
        PathTooLong = 5
    }
}
