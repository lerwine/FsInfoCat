namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IVideoPropertiesRow" />
    public interface IUpstreamVideoPropertiesRow : IUpstreamPropertiesRow, IVideoPropertiesRow { }
}
