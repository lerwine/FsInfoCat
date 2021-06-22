namespace FsInfoCat
{
    /// <summary>
    /// Represents the status of file redundancy mediation.
    /// </summary>
    public enum RedundancyRemediationStatus
    {
        /// <summary>
        /// File is not actually redundant with any other files in the same <see cref="IRedundantSet"/>.
        /// </summary>
        NotRedundant = 0,

        /// <summary>
        /// The initial state for newly discovered redundancies.
        /// </summary>
        Unconfirmed = 1,

        /// <summary>
        /// Acknowledged; Validation in progress.
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
        /// Indicates that files in the current <see cref="IRedundantSet"/> should be deleted.
        /// </summary>
        Insupportable = 5,

        /// <summary>
        /// Elevated status for unauthorized copies of files that need further attention.
        /// </summary>
        Violation = 6,

        /// <summary>
        /// Indicates that files in the current <see cref="IRedundantSet"/> have been scheduled/tasked to be deleted, but not yet confirmed.
        /// </summary>
        Attrition = 7,

        /// <summary>
        /// Files in the current <see cref="IRedundantSet"/> have been deleted.
        /// </summary>
        Deleted = 8
    }
}
