namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of media files.
    /// </summary>
    /// <seealso cref="IMediaProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalMediaPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamMediaPropertySet"/>
    public interface IMediaPropertySet : IMediaProperties, IPropertySet
    {
    }
}
