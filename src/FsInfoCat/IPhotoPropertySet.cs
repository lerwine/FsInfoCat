namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of photo files.
    /// </summary>
    /// <seealso cref="IPhotoProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalPhotoPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamPhotoPropertySet"/>
    /// <seealso cref="IFile.PhotoProperties"/>
    public interface IPhotoPropertySet : IPhotoProperties, IPropertySet
    {
    }
}
