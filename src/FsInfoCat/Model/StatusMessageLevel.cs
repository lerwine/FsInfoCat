namespace FsInfoCat.Model
{
    /// <summary>
    /// Indicates the severity of a status message.
    /// </summary>
    public enum StatusMessageLevel : byte
    {
        /// <summary>
        /// Indicates that the associated message is informational.
        /// </summary>
        Information = 0,

        /// <summary>
        /// Indicates that the associated message is a warning.
        /// </summary>
        Warning = 1,

        /// <summary>
        /// Indicates that the associated message is an error message.
        /// </summary>
        Error = 2
    }
}
