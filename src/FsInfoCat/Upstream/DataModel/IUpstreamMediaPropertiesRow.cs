﻿namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IMediaPropertiesRow" />
    /// <seealso cref="Local.ILocalMediaPropertiesRow" />
    public interface IUpstreamMediaPropertiesRow : IUpstreamPropertiesRow, IMediaPropertiesRow { }
}