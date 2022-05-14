namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a tag list item entity that can be associated with <see cref="ILocalFFile"/>, <see cref="ILocalFSubdirectory"/> or <see cref="ILocalFVolume"/> entities.
    /// </summary>
    /// <seealso cref="ITagDefinitionListItem" />
    /// <seealso cref="ILocalTagDefinitionRow" />
    public interface ILocalTagDefinitionListItem : ITagDefinitionListItem, ILocalTagDefinitionRow { }
}
