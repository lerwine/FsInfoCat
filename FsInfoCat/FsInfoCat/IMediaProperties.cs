namespace FsInfoCat
{
    public interface IMediaProperties : IPropertySet
    {
        /// <summary>
        /// Gets the Content Distributor
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 18 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string ContentDistributor { get; set; }

        /// <summary>
        /// Gets the Creator Application
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 27 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string CreatorApplication { get; set; }

        /// <summary>
        /// Gets the Creator Application Version
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 28 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string CreatorApplicationVersion { get; set; }

        /// <summary>
        /// Gets the Date Released
        /// </summary>
        /// <remarks>ID: {DE41CC29-6971-4290-B472-F59F2E2F31E2}, 100</remarks>
        string DateReleased { get; set; }

        /// <summary>
        /// Gets the duration
        /// </summary>
        /// <remarks>100ns units, not milliseconds
        /// <para>ID: {64440490-4C8B-11D1-8B70-080036B11A03}, 3 (AudioSummaryInformation)</para></remarks>
        ulong? Duration { get; set; }

        /// <summary>
        /// Gets the DVD ID
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 15 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string DVDID { get; set; }

        /// <summary>
        /// Indicates the frame count for the image.
        /// </summary>
        /// <remarks>ID: {6444048F-4C8B-11D1-8B70-080036B11A03}, 12 (IMAGESUMMARYINFORMATION)</remarks>
        uint? FrameCount { get; set; }

        /// <summary>
        /// Gets the Producer
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 22 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string[] Producer { get; set; }

        /// <summary>
        /// Gets the Protection Type
        /// </summary>
        /// <remarks>If media is protected, how is it protected?
        /// <para>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 38 (MEDIAFILESUMMARYINFORMATION)</para></remarks>
        string ProtectionType { get; set; }

        /// <summary>
        /// Gets the Provider Rating
        /// </summary>
        /// <remarks>Rating value ranges from 0 to 99, supplied by metadata provider
        /// <para>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 39 (MEDIAFILESUMMARYINFORMATION)</para></remarks>
        string ProviderRating { get; set; }

        /// <summary>
        /// Style of music or video
        /// </summary>
        /// <remarks>Supplied by metadata provider
        /// <para>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 40 (MEDIAFILESUMMARYINFORMATION)</para></remarks>
        string ProviderStyle { get; set; }

        /// <summary>
        /// Gets the Publisher
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 30 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string Publisher { get; set; }

        /// <summary>
        /// Gets the Subtitle
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 38 (MUSIC)</remarks>
        string Subtitle { get; set; }

        /// <summary>
        /// Gets the Writer
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 23 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string[] Writer { get; set; }

        /// <summary>
        /// Gets the Year
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 5 (MUSIC)</remarks>
        uint? Year { get; set; }
    }
}
