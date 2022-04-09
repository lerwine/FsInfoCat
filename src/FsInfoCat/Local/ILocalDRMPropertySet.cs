namespace FsInfoCat.Local
{
    /// <summary>Contains extended DRM property values.</summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IDRMPropertySet" />
    public interface ILocalDRMPropertySet : ILocalDRMPropertiesRow, ILocalPropertySet, IDRMPropertySet { }
}
