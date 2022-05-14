namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IPropertiesRow" />
    public interface ILocalPropertiesRow : ILocalDbEntity, IPropertiesRow { }
}
