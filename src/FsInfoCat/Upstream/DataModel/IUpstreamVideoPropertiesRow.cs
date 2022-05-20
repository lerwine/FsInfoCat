namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IVideoPropertiesRow" />
    /// <seealso cref="Local.ILocalVideoPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamVideoPropertiesRow")]
    public interface IUpstreamVideoPropertiesRow : IUpstreamPropertiesRow, IVideoPropertiesRow { }
}
