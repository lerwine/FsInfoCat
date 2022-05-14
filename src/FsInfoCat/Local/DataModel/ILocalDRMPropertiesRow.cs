namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IDRMPropertiesRow" />
    public interface ILocalDRMPropertiesRow : ILocalPropertiesRow, IDRMPropertiesRow { }
}
