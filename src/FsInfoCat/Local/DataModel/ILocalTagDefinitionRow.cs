namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a tag entity that can be associated with <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/> or <see cref="ILocalVolume"/> entities.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ITagDefinitionRow" />
    public interface ILocalTagDefinitionRow : ILocalDbEntity, ITagDefinitionRow { }
}
