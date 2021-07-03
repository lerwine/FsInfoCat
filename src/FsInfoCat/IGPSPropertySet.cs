namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file GPS property values.
    /// </summary>
    /// <seealso cref="IGPSProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalGPSPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamGPSPropertySet"/>
    /// <seealso cref="IFile.GPSProperties"/>
    public interface IGPSPropertySet : IGPSProperties, IPropertySet
    {
    }
}
