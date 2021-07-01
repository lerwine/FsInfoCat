using FsInfoCat.Collections;

namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for media files.
    /// </summary>
    /// <seealso cref="IMediaPropertySet"/>
    /// <seealso cref="Local.ILocalMediaPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamMediaPropertySet"/>
    /// <seealso cref="FilePropertiesComparer.Equals(IMediaProperties, IMediaProperties)"/>
    /// <seealso cref="Local.IFileDetailProvider.GetMediaPropertiesAsync(System.Threading.CancellationToken)"/>
    /// <seealso cref="IDbContext.FindMatchingAsync(IMediaProperties, System.Threading.CancellationToken)"/>
    public interface IMediaProperties
    {
        /// <summary>
        /// Gets the Content Distributor
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Media.ContentDistributor
        /// <list type="bullet">
        /// <item><term>Name</term><description>Content Distributor</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>18</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-contentdistributor">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ContentDistributor { get; }

        /// <summary>
        /// Gets the Creator Application
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Media.CreatorApplication
        /// <list type="bullet">
        /// <item><term>Name</term><description>Creator Application/Tool</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>27</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-creatorapplication">[Reference Link]</a></description></item>
        /// </list></remarks>
        string CreatorApplication { get; }

        /// <summary>
        /// Gets the Creator Application Version
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Media.CreatorApplicationVersion
        /// <list type="bullet">
        /// <item><term>Name</term><description>Creator Application/Tool Version</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>28</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-creatorapplicationversion">[Reference Link]</a></description></item>
        /// </list></remarks>
        string CreatorApplicationVersion { get; }

        /// <summary>
        /// Gets the Date Released
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Media.DateReleased
        /// <list type="bullet">
        /// <item><term>Name</term><description>Date Released</description></item>
        /// <item><term>Format ID</term><description>{DE41CC29-6971-4290-B472-F59F2E2F31E2} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-datereleased">[Reference Link]</a></description></item>
        /// </list></remarks>
        string DateReleased { get; }

        /// <summary>
        /// Gets the duration
        /// </summary>
        /// <value>
        /// 100ns units, not milliseconds The actual play time of a media file and is measured in 100ns units, not milliseconds.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Duration</description></item>
        /// <item><term>Format ID</term><description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>3</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-duration">[Reference Link]</a></description></item>
        /// </list></remarks>
        ulong? Duration { get; }

        /// <summary>
        /// Gets the DVD ID
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Media.DVDID
        /// <list type="bullet">
        /// <item><term>Name</term><description>DVD ID</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>15</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-dvdid">[Reference Link]</a></description></item>
        /// </list></remarks>
        string DVDID { get; }

        /// <summary>
        /// Indicates the frame count for the image.
        /// </summary>
        /// <value>
        /// Indicates the frame count for the image.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Frame Count</description></item>
        /// <item><term>Format ID</term><description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>12</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-framecount">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? FrameCount { get; }

        /// <summary>
        /// Gets the Producer
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Media.Producer
        /// <list type="bullet">
        /// <item><term>Name</term><description>Producer</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>22</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-producer">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Producer { get; }

        /// <summary>
        /// Gets the Protection Type
        /// </summary>
        /// <value>
        /// If media is protected, how is it protected? Describes the type of media protection.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Protection Type</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>38</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-protectiontype">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ProtectionType { get; }

        /// <summary>
        /// Gets the Provider Rating
        /// </summary>
        /// <value>
        /// Rating value ranges from 0 to 99, supplied by metadata provider The rating (0 - 99) supplied by metadata provider.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Provider Rating</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>39</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-providerrating">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ProviderRating { get; }

        /// <summary>
        /// Style of music or video
        /// </summary>
        /// <value>
        /// Supplied by metadata provider The style of music or video, supplied by metadata provider.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Provider Style</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>40</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-providerstyle">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ProviderStyle { get; }

        /// <summary>
        /// Gets the Publisher
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Media.Publisher
        /// <list type="bullet">
        /// <item><term>Name</term><description>Publisher</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>30</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-publisher">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Publisher { get; }

        /// <summary>
        /// Gets the Subtitle
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Media.SubTitle
        /// <list type="bullet">
        /// <item><term>Name</term><description>Subtitle</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>38</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-subtitle">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Subtitle { get; }

        /// <summary>
        /// Gets the Writer
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Media.Writer
        /// <list type="bullet">
        /// <item><term>Name</term><description>Writer</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>23</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-writer">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Writer { get; }

        /// <summary>
        /// Gets the Publication Year
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Media.Year
        /// <list type="bullet">
        /// <item><term>Name</term><description>Publication Year</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>5</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-year">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? Year { get; }
    }
}
