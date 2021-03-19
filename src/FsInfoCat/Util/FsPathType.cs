namespace FsInfoCat
{
    /// <summary>
    /// Filesystem path types.
    /// </summary>
    public enum FsPathType
    {
        /// <summary>
        /// Relative filesystem path string.
        /// </summary>
        Relative,

        /// <summary>
        /// Absolute path string on the host device.
        /// </summary>
        Local,

        /// <summary>
        /// UNC format remote path string.
        /// </summary>
        UNC
    }
}
