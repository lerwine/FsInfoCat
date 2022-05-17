namespace FsInfoCat
{
    // TODO: Document RedundancyRemediationStatus enum
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum RedundancyRemediationStatus : byte
    {
        /// <summary>
        /// The initial state for newly discovered redundancies.
        /// </summary>
        Unconfirmed = 1,

        /// <summary>
        /// File has not been determined to be redundant of any other files.
        /// </summary>
        NotRedundant = 0,

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
