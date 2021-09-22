namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for database view entities derived from the <see cref="IDbContext.Files"/> or <see cref="IDbContext.Subdirectories"/> table.
    /// </summary>
    /// <seealso cref="IDbFsItemRow" />
    public interface IDbFsItemListItem : IDbFsItemRow
    {
        /// <summary>
        /// Gets the access error count.
        /// </summary>
        /// <value>Gets the number of access errors that occurred while attempting to access the curent file.</value>
        long AccessErrorCount { get; }

        /// <summary>
        /// Gets the personal tag count.
        /// </summary>
        /// <value>The number personal personal tags associated with the current file.</value>
        long PersonalTagCount { get; }

        /// <summary>
        /// Gets the shared tag count.
        /// </summary>
        /// <value>The number shared tags associated with the current file.</value>
        long SharedTagCount { get; }
    }
}

