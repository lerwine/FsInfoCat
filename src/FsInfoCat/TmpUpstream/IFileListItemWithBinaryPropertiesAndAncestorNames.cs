using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for file list item entities which also includes length and hash information as well as a delimited listing of parent subdirectory names.
    /// </summary>
    /// <seealso cref="M.IFileListItemWithBinaryPropertiesAndAncestorNames" />
    /// <seealso cref="IUpstreamFileListItemWithAncestorNames" />
    /// <seealso cref="Local.Model.IFileListItemWithBinaryPropertiesAndAncestorNames" />
    public interface IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames : M.IFileListItemWithBinaryPropertiesAndAncestorNames, IUpstreamFileListItemWithAncestorNames { }
}
