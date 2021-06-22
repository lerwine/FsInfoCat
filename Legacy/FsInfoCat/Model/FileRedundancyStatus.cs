namespace FsInfoCat
{
    public enum FileRedundancyStatus
    {
        /// <summary>
        /// File has not been determined to be redundant of any other files.
        /// </summary>
        NotRedundant,

        /// <summary>
        /// The initial state for newly discovered redundanc.ies
        /// </summary>
        Unconfirmed,

        /// <summary>
        /// Awaiting validation.
        /// </summary>
        PendingValidation,

        /// <summary>
        /// Final determination deferred.
        /// </summary>
        Deferred,

        /// <summary>
        /// File duplicaton is justified.
        /// </summary>
        Justified,

        /// <summary>
        /// Indicates that file should be deleted.
        /// </summary>
        Insupportable,

        /// <summary>
        /// Elevated
        /// </summary>
        Violation,

        /// <summary>
        /// Indicates that file has been scheduled/tasked to be deleted, but not yet confirmed.
        /// </summary>
        Attrition,

        /// <summary>
        /// File has been deleted.
        /// </summary>
        Deleted
    }
}
