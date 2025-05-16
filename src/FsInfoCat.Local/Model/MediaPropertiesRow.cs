using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Base class for entities containing extended file properties for media files.
/// </summary>
/// <seealso cref="MediaPropertiesListItem" />
/// <seealso cref="MediaPropertySet" />
/// <seealso cref="LocalDbContext.MediaPropertySets" />
/// <seealso cref="LocalDbContext.MediaPropertiesListing" />
public abstract class MediaPropertiesRow : PropertiesRow, ILocalMediaPropertiesRow
{
    #region Fields

    private string _contentDistributor = string.Empty;
    private string _creatorApplication = string.Empty;
    private string _creatorApplicationVersion = string.Empty;
    private string _dateReleased = string.Empty;
    private string _dvdID = string.Empty;
    private string _protectionType = string.Empty;
    private string _providerRating = string.Empty;
    private string _providerStyle = string.Empty;
    private string _publisher = string.Empty;
    private string _subtitle = string.Empty;

    #endregion

    #region Properties

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.ContentDistributor), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_contentDistributor))]
    public string ContentDistributor { get => _contentDistributor; set => _contentDistributor = value.AsWsNormalizedOrEmpty(); }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.CreatorApplication), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_creatorApplication))]
    public string CreatorApplication { get => _creatorApplication; set => _creatorApplication = value.AsWsNormalizedOrEmpty(); }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.CreatorApplicationVersion), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_creatorApplicationVersion))]
    public string CreatorApplicationVersion { get => _creatorApplicationVersion; set => _creatorApplicationVersion = value.AsWsNormalizedOrEmpty(); }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.DateReleased), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_dateReleased))]
    public string DateReleased { get => _dateReleased; set => _dateReleased = value.AsWsNormalizedOrEmpty(); }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Duration), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public ulong? Duration { get; set; }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.DVDID), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_dvdID))]
    public string DVDID { get => _dvdID; set => _dvdID = value.AsWsNormalizedOrEmpty(); }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.FrameCount), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public uint? FrameCount { get; set; }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Producer), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public MultiStringValue Producer { get; set; }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.ProtectionType), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_protectionType))]
    public string ProtectionType { get => _protectionType; set => _protectionType = value.AsWsNormalizedOrEmpty(); }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.ProviderRating), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_providerRating))]
    public string ProviderRating { get => _providerRating; set => _providerRating = value.AsWsNormalizedOrEmpty(); }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.ProviderStyle), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_providerStyle))]
    public string ProviderStyle { get => _providerStyle; set => _providerStyle = value.AsWsNormalizedOrEmpty(); }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Publisher), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_publisher))]
    public string Publisher { get => _publisher; set => _publisher = value.AsWsNormalizedOrEmpty(); }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Subtitle), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_subtitle))]
    public string Subtitle { get => _subtitle; set => _subtitle = value.AsWsNormalizedOrEmpty(); }

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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Writer), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public MultiStringValue Writer { get; set; }

    /// /// <summary>
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
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Year), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public uint? Year { get; set; }

    #endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    protected virtual string PropertiesToString() => $@"Subtitle=""{ExtensionMethods.EscapeCsString(_subtitle)}"", Duration={Duration}, FrameCount={FrameCount},
    Year={Year}, DateReleased=""{ExtensionMethods.EscapeCsString(_dateReleased)}"", DVDID=""{ExtensionMethods.EscapeCsString(_dvdID)}"",
    ProtectionType=""{ExtensionMethods.EscapeCsString(_protectionType)}"", ProviderRating=""{ExtensionMethods.EscapeCsString(_providerRating)}"", ProviderStyle=""{ExtensionMethods.EscapeCsString(_providerStyle)}"",
    Writer={Writer.ToCsString()}, Producer={Producer.ToCsString()}, Publisher=""{ExtensionMethods.EscapeCsString(_publisher)}"",
    ContentDistributor=""{ExtensionMethods.EscapeCsString(_contentDistributor)}"", CreatorApplication=""{ExtensionMethods.EscapeCsString(_creatorApplication)}"", CreatorApplicationVersion=""{ExtensionMethods.EscapeCsString(_creatorApplicationVersion)}""";

    public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ILocalMediaPropertiesRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] ILocalMediaPropertiesRow other) => ArePropertiesEqual((IMediaPropertiesRow)other) &&
        EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        LastSynchronizedOn == other.LastSynchronizedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IMediaPropertiesRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] IMediaPropertiesRow other) => ArePropertiesEqual((IMediaProperties)other) &&
        CreatedOn == other.CreatedOn &&
        ModifiedOn == other.ModifiedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IMediaProperties" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] IMediaProperties other) => _contentDistributor == other.ContentDistributor &&
        _creatorApplication == other.CreatorApplication &&
        _creatorApplicationVersion == other.CreatorApplicationVersion &&
        _dateReleased == other.DateReleased &&
        _dvdID == other.DVDID &&
        _protectionType == other.ProtectionType &&
        _providerRating == other.ProviderRating &&
        _providerStyle == other.ProviderStyle &&
        _publisher == other.Publisher &&
        _subtitle == other.Subtitle &&
        Duration == other.Duration &&
        FrameCount == other.FrameCount &&
        EqualityComparer<MultiStringValue>.Default.Equals(Producer, other.Producer) &&
        EqualityComparer<MultiStringValue>.Default.Equals(Writer, other.Writer) &&
        Year == other.Year;
    //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
    //LastSynchronizedOn == other.LastSynchronizedOn &&
    //CreatedOn == other.CreatedOn &&
    //ModifiedOn == other.ModifiedOn;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract bool Equals(IMediaPropertiesRow other);

    public abstract bool Equals(IMediaProperties other);

    public override int GetHashCode()
    {
        if (TryGetId(out Guid id)) return id.GetHashCode();
        HashCode hash = new();
        hash.Add(_contentDistributor);
        hash.Add(_creatorApplication);
        hash.Add(_creatorApplicationVersion);
        hash.Add(_dateReleased);
        hash.Add(_dvdID);
        hash.Add(_protectionType);
        hash.Add(_providerRating);
        hash.Add(_providerStyle);
        hash.Add(_publisher);
        hash.Add(_subtitle);
        hash.Add(Duration);
        hash.Add(FrameCount);
        hash.Add(Producer);
        hash.Add(Writer);
        hash.Add(Year);
        hash.Add(UpstreamId);
        hash.Add(LastSynchronizedOn);
        hash.Add(CreatedOn);
        hash.Add(ModifiedOn);
        return hash.ToHashCode();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
