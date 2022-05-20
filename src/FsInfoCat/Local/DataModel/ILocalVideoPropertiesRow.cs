namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IVideoPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamVideoPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalVideoPropertiesRow")]
    public interface ILocalVideoPropertiesRow : ILocalPropertiesRow, IVideoPropertiesRow { }
}
