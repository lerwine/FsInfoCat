namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of recorded TV files.
    /// </summary>
    /// <seealso cref="IRecordedTVProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalRecordedTVPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamRecordedTVPropertySet"/>
    /// <seealso cref="IFile.RecordedTVProperties"/>
    public interface IRecordedTVPropertySet : IRecordedTVProperties, IPropertySet
    {
    }
}
