using System;

namespace FsInfoCat.Util
{
    [Flags]
    public enum FsPathKind
    {
        /// <summary>
        /// Relative filesystem path string. Equivalent to <seealso cref="FsPathType.Relative"/> and <seealso cref="UriKind.Relative"/>.
        /// </summary>
        Relative = 0b0100,

        /// <summary>
        /// Absolute path string on the host device. Equivalent to <seealso cref="FsPathType.Local"/>.
        /// </summary>
        Local = 0b0001,

        /// <summary>
        /// UNC format remote path string. Equivalent to <seealso cref="FsPathType.UNC"/>.
        /// </summary>
        UNC = 0b0010,

        /// <summary>
        /// UNC format remote path string. Equivalent to  <seealso cref="UriKind.Absolute"/>.
        /// </summary>
        Absolute = 0b0011,

        /// <summary>
        /// UNC format remote path string. Equivalent to  <seealso cref="UriKind.RelativeOrAbsolute"/>.
        /// </summary>
        Any = 0b0111
    }
}
