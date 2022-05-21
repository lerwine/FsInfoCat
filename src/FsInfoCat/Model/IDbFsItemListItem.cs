namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for database view entities derived from the <see cref="IDbContext.Files"/> or <see cref="IDbContext.Subdirectories"/> table.
    /// </summary>
    /// <seealso cref="IDbFsItemRow" />
    /// <seealso cref="Local.Model.ILocalDbFsItemListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamDbFsItemListItem" />
    public interface IDbFsItemListItem : IDbFsItemRow
    {
        /// <summary>
        /// Gets the access error count.
        /// </summary>
        /// <value>Gets the number of access errors that occurred while attempting to access the current filesystem node.</value>
        long AccessErrorCount { get; }

        /// <summary>
        /// Gets the personal tag count.
        /// </summary>
        /// <value>The number personal personal tags associated with the current filesystem node.</value>
        long PersonalTagCount { get; }

        /// <summary>
        /// Gets the shared tag count.
        /// </summary>
        /// <value>The number shared tags associated with the current filesystem node.</value>
        long SharedTagCount { get; }
    }
}
