namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IAudioPropertiesRow" />
    /// <seealso cref="IPropertiesListItem" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IDocumentPropertiesRow" />
    /// <seealso cref="IDRMPropertiesRow" />
    /// <seealso cref="IGPSPropertiesRow" />
    /// <seealso cref="IImagePropertiesRow" />
    /// <seealso cref="IMediaPropertiesRow" />
    /// <seealso cref="IMusicPropertiesRow" />
    /// <seealso cref="IPhotoPropertiesRow" />
    /// <seealso cref="IRecordedTVPropertiesRow" />
    /// <seealso cref="ISummaryPropertiesRow" />
    /// <seealso cref="IVideoPropertiesRow" />
    public interface IPropertiesRow : IDbEntity, IHasSimpleIdentifier { }
}
