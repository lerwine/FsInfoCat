using System.ComponentModel;

namespace FsInfoCat
{
    public enum ErrorCode
    {
        [AmbientValue("Unexpected Error")]
        Unexpected = 0,

        /// <summary>
        /// An <see cref="System.IO.IOException"/> has occurred.
        /// </summary>
        [AmbientValue("IO Error")]
        IOError = 1,

        /// <summary>
        /// An <see cref="System.UnauthorizedAccessException"/> has occurred.
        /// </summary>
        [AmbientValue("Unauthorized Access Error")]
        UnauthorizedAccess = 2,

        /// <summary>
        /// A <see cref="System.Security.SecurityException"/> has occurred.
        /// </summary>
        [AmbientValue("Security Exception")]
        SecurityException = 3,

        /// <summary>
        /// The name of one or more files or subdirectories in the path contain a invalid character(s).
        /// </summary>
        [AmbientValue("Invalid Path Error")]
        InvalidPath = 4,

        /// <summary>
        /// A <see cref="System.IO.PathTooLongException"/> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.
        /// </summary>
        [AmbientValue("Path Too Long Error")]
        PathTooLong = 5,

        [AmbientValue("Crawl Operation Failed")]
        CrawlOperationFailed = 6
    }
}
