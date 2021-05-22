namespace FsInfoCat
{
    public enum FileRedundancyStatus : byte
    {
        /// <summary>
        /// File has not been determined to be redundant of any other files.
        /// </summary>
        NotRedundant = 0,

        /// <summary>
        /// The initial state for newly discovered redundanc.ies
        /// </summary>
        Unconfirmed = 1,

        /// <summary>
        /// Awaiting validation.
        /// </summary>
        PendingValidation = 2,

        /// <summary>
        /// Final determination deferred.
        /// </summary>
        Deferred = 3,

        /// <summary>
        /// File duplicaton is justified.
        /// </summary>
        Justified = 4,

        /// <summary>
        /// Indicates that file should be deleted.
        /// </summary>
        Insupportable = 5,

        /// <summary>
        /// Elevated
        /// </summary>
        Violation = 6,

        /// <summary>
        /// Indicates that file has been scheduled/tasked to be deleted, but not yet confirmed.
        /// </summary>
        Attrition = 7,

        /// <summary>
        /// File has been deleted.
        /// </summary>
        Deleted = 8
    }
}
