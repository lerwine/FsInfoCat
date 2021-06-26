namespace FsInfoCat
{
    /// <summary>
    /// Indicates status of files represented within the <see cref="IDbContext">local database</see>, as it pertains to local file processing.
    /// </summary>
    public enum FileCorrelationStatus : byte
    {
        /// <summary>
        /// File has been added to the database or modified and needs to be analyzed.
        /// </summary>
        Dissociated = 0,

        /// <summary>
        /// Correlations to other files are being established.
        /// </summary>
        /// <remarks>This status value is only for new files and those which have changed. Results of comparisons will be stored in the <see cref="IComparison"/> table, where the <see cref="IComparison.Correlative"/>
        /// is the new or changed file, and the <see cref="IComparison.Baseline"/> is the file that it is being compared to.</remarks>
        Analyzing = 2,

        /// <summary>
        /// File has been compared with all other files.
        /// </summary>
        Correlated = 3,

        /// <summary>
        /// The file has been deleted.
        /// </summary>
        Deleted = 4
    }
}
