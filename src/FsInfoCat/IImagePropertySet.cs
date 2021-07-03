namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of image files.
    /// </summary>
    /// <seealso cref="IImageProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalImagePropertySet"/>
    /// <seealso cref="Upstream.IUpstreamImagePropertySet"/>
    /// <seealso cref="IFile.ImageProperties"/>
    public interface IImagePropertySet : IImageProperties, IPropertySet
    {
    }
}
