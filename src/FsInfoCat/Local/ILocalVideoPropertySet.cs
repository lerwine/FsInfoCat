namespace FsInfoCat.Local
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of video files.
    /// Implements the <see cref="ILocalPropertySet" />
    /// Implements the <see cref="IVideoPropertySet" />
    /// </summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IVideoPropertySet" />
    public interface ILocalVideoPropertySet : ILocalPropertySet, IVideoPropertySet
    {
    }
}
