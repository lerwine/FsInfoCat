namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Contains extended DRM (Digital Rights Management) property values.
    /// </summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IDRMPropertySet" />
    public interface IUpstreamDRMPropertySet : IUpstreamPropertySet, IDRMPropertySet { }
}
