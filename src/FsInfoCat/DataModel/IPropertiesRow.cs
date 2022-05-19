using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IHasSimpleIdentifier" />
    /// <seealso cref="Local.ILocalPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamPropertiesRow" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="ISummaryPropertiesRow" />
    /// <seealso cref="IDocumentPropertiesRow" />
    /// <seealso cref="IAudioPropertiesRow" />
    /// <seealso cref="IDRMPropertiesRow" />
    /// <seealso cref="IGPSPropertiesRow" />
    /// <seealso cref="IImagePropertiesRow" />
    /// <seealso cref="IMediaPropertiesRow" />
    /// <seealso cref="IMusicPropertiesRow" />
    /// <seealso cref="IPhotoPropertiesRow" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    /// <seealso cref="IVideoPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Model.IPropertiesRow")]
    public interface IPropertiesRow : IDbEntity, IHasSimpleIdentifier { }
}
