namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file GPS property values.
    /// </summary>
    /// <seealso cref="IGPSProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalGPSPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamGPSPropertySet"/>
    public interface IGPSPropertySet : IGPSProperties, IPropertySet
    {
    }
}
