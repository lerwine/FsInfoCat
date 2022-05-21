using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes length and hash information as well as a delimited listing of parent subdirectory names.
    /// </summary>
    /// <seealso cref="IFileListItemWithBinaryPropertiesAndAncestorNames" />
    /// <seealso cref="ILocalFileListItemWithAncestorNames" />
    /// <seealso cref="Upstream.Model.IFileListItemWithBinaryPropertiesAndAncestorNames" />
    public interface ILocalFileListItemWithBinaryPropertiesAndAncestorNames : IFileListItemWithBinaryPropertiesAndAncestorNames, ILocalFileListItemWithAncestorNames { }
}
