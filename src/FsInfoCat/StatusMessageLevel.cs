namespace FsInfoCat
{
    /// <summary>
    /// Indicates the severity of a status message.
    /// </summary>
    public enum StatusMessageLevel : byte
    {
        /// <summary>
        /// Indicates that the associated message is informational.
        /// </summary>
        Information,

        /// <summary>
        /// Indicates that the associated message is a warning.
        /// </summary>
        Warning,

        /// <summary>
        /// Indicates that the associated message is an error message.
        /// </summary>
        Error
    }
}
