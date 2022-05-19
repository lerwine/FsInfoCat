using FsInfoCat.Collections;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for media files.
    /// </summary>
    [EntityInterface]
    [System.Obsolete("Use FsInfoCat.Model.IMediaProperties")]
    public interface IMediaProperties : IEquatable<IMediaProperties>
    {
        /// <summary>
        /// Gets the Content Distributor.
        /// </summary>
        /// <value>The Content Distributor.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Content Distributor</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>18</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-contentdistributor">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ContentDistributor), ResourceType = typeof(Properties.Resources))]
        string ContentDistributor { get; }

        /// <summary>
        /// Gets the Creator Application.
        /// </summary>
        /// <value>The creator application.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Creator Application/Tool</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>27</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-creatorapplication">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreatorApplication), ResourceType = typeof(Properties.Resources))]
        string CreatorApplication { get; }

        /// <summary>
        /// Gets the Creator Application Version.
        /// </summary>
        /// <value>The creator application version.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Creator Application/Tool Version</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>28</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-creatorapplicationversion">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreatorApplicationVersion), ResourceType = typeof(Properties.Resources))]
        string CreatorApplicationVersion { get; }

        /// <summary>
        /// Gets the Date Released.
        /// </summary>
        /// <value>The release data.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Date Released</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{DE41CC29-6971-4290-B472-F59F2E2F31E2} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-datereleased">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DateReleased), ResourceType = typeof(Properties.Resources))]
        string DateReleased { get; }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>100ns units, not milliseconds The actual play time of a media file and is measured in 100ns units, not milliseconds.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Duration</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-duration">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Duration), ResourceType = typeof(Properties.Resources))]
        ulong? Duration { get; }

        /// <summary>
        /// Gets the DVD ID.
        /// </summary>
        /// <value>The DVD ID.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>DVD ID</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>15</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-dvdid">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DVDID), ResourceType = typeof(Properties.Resources))]
        string DVDID { get; }

        /// <summary>
        /// Indicates the frame count for the image.
        /// </summary>
        /// <value>Indicates the frame count for the image.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Frame Count</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>12</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-framecount">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FrameCount), ResourceType = typeof(Properties.Resources))]
        uint? FrameCount { get; }

        /// <summary>
        /// Gets the Producer.
        /// </summary>
        /// <value>The producer.</value>
        /// <remarks>
        /// Media.Producer
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Producer</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>22</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-producer">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Producer), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Producer { get; }

        /// <summary>
        /// Gets the Protection Type.
        /// </summary>
        /// <value>If media is protected, how is it protected? Describes the type of media protection.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Protection Type</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>38</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-protectiontype">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ProtectionType), ResourceType = typeof(Properties.Resources))]
        string ProtectionType { get; }

        /// <summary>
        /// Gets the Provider Rating.
        /// </summary>
        /// <value>Rating value ranges from 0 to 99, supplied by metadata provider The rating (0 - 99) supplied by metadata provider.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Provider Rating</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>39</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-providerrating">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ProviderRating), ResourceType = typeof(Properties.Resources))]
        string ProviderRating { get; }

        /// <summary>
        /// Style of music or video.
        /// </summary>
        /// <value>Supplied by metadata provider The style of music or video, supplied by metadata provider.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Provider Style</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>40</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-providerstyle">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ProviderStyle), ResourceType = typeof(Properties.Resources))]
        string ProviderStyle { get; }

        /// <summary>
        /// Gets the Publisher.
        /// </summary>
        /// <value>The Publisher.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Publisher</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>30</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-publisher">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Publisher), ResourceType = typeof(Properties.Resources))]
        string Publisher { get; }

        /// <summary>
        /// Gets the Subtitle.
        /// </summary>
        /// <value>The sub-title.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Subtitle</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>38</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-subtitle">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Subtitle), ResourceType = typeof(Properties.Resources))]
        string Subtitle { get; }

        /// <summary>
        /// Gets the Writer.
        /// </summary>
        /// <value>The writer.</value>
        /// <remarks>
        /// Media.Writer
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Writer</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>23</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-writer">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Writer), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Writer { get; }

        /// <summary>
        /// Gets the Publication Year.
        /// </summary>
        /// <value>The publication year.</value>
        /// <remarks>
        /// Media.Year
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Publication Year</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>5</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-year">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Year), ResourceType = typeof(Properties.Resources))]
        uint? Year { get; }
    }
}
