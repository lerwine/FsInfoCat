namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a tag entity that can be associated with <see cref="ILocalFFile"/>, <see cref="ILocalFSubdirectory"/> or <see cref="ILocalFVolume"/> entities.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ITagDefinitionRow" />
    public interface ILocalTagDefinitionRow : ILocalDbEntity, ITagDefinitionRow { }
}
