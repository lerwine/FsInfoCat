namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for file list item entities which also includes length and hash information as well as a delimited listing of parent subdirectory names.
    /// </summary>
    /// <seealso cref="IFileListItemWithBinaryPropertiesAndAncestorNames" />
    /// <seealso cref="ILocalFileListItemWithAncestorNames" />
    /// <seealso cref="Upstream.IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalFileListItemWithBinaryPropertiesAndAncestorNames")]
    public interface ILocalFileListItemWithBinaryPropertiesAndAncestorNames : IFileListItemWithBinaryPropertiesAndAncestorNames, ILocalFileListItemWithAncestorNames { }
}
