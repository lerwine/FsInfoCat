namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of audio files.
    /// </summary>
    /// <seealso cref="IAudioProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalAudioPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamAudioPropertySet"/>
    public interface IAudioPropertySet : IAudioProperties, IPropertySet
    {
    }
}
