namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities that contain the name of a file system node and a delimited list of parent subdirectory names.
    /// </summary>
    /// <seealso cref="IHasSimpleIdentifier" />
    [System.Obsolete("Use FsInfoCat.Model.IDbFsItemAncestorName")]
    public interface IDbFsItemAncestorName : IHasSimpleIdentifier
    {
        /// <summary>
        /// Gets the name of teh current file system node.
        /// </summary>
        /// <value>The name current file system node (file or directory).</value>
         string Name { get; }

        /// <summary>
        /// Gets the ancestor subdirectory names.
        /// </summary>
        /// <value>The result of a calculated column that contains the names of the parent subdirectories, separated by slash (<c>/</c>) characters, and in reverse order from
        /// typical file system path segments.</value>
        string AncestorNames { get; }
    }
}
