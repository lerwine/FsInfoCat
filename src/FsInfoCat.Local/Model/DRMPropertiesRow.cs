using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Base class for entities containing extended file DRM information properties.
/// </summary>
/// <seealso cref="DRMPropertiesListItem" />
/// <seealso cref="DRMPropertySet" />
/// <seealso cref="LocalDbContext.DRMPropertySets" />
/// <seealso cref="LocalDbContext.DRMPropertiesListing" />
public abstract class DRMPropertiesRow : PropertiesRow, ILocalDRMPropertiesRow
{
    private string _description = string.Empty;

    /// <summary>
    /// Indicates when play expires for digital rights management.
    /// </summary>
    /// <value>Indicates when play rights expire.</value>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Date Play Expires</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>6</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-dateplayexpires">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.DatePlayExpires), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public DateTime? DatePlayExpires { get; set; }

    /// <summary>
    /// Indicates when play starts for digital rights management.
    /// </summary>
    /// <value>Indicates when play rights begin.</value>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Date Play Starts</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>5</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-dateplaystarts">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.DatePlayStarts), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public DateTime? DatePlayStarts { get; set; }

    /// <summary>
    /// Displays the description for digital rights management.
    /// </summary>
    /// <value>Displays the description for Digital Rights Management (DRM).</value>
    /// <remarks>
    /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>License Description</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>3</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-description">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Description), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_description))]
    public string Description { get => _description; set => _description = value.EmptyIfNullOrWhiteSpace(); }

    /// <summary>
    /// Indicates whether the content is protected.
    /// </summary>
    /// <value><see langword="true" /> if the content of the file is protected; <see langword="false" /> if the file content is unprotected;
    /// otherwise, <see langword="null" /> if this value is not specified.</value>
    /// <remarks>
    /// Indicates whether the file is protected under Digital Rights Management (DRM).
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Is Protected</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>2</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-isprotected">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.IsProtected), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public bool? IsProtected { get; set; }

    /// <summary>
    /// Indicates the play count for digital rights management.
    /// </summary>
    /// <value>Indicates the number of times the file has been played.</value>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Plays Remaining</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>4</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-playcount">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.PlayCount), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public uint? PlayCount { get; set; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    protected virtual string PropertiesToString() => $@"IsProtected={IsProtected}, PlayCount={PlayCount}, DatePlayStarts={DatePlayStarts:yyyy-mm-ddTHH:mm:ss.fffffff}, DatePlayExpires={DatePlayExpires:yyyy-mm-ddTHH:mm:ss.fffffff},
    _description=""{ExtensionMethods.EscapeCsString(_description)}""";

    public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ILocalDRMPropertiesRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] ILocalDRMPropertiesRow other) => ArePropertiesEqual((IDRMPropertiesRow)other) &&
        EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        LastSynchronizedOn == other.LastSynchronizedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IDRMPropertiesRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] IDRMPropertiesRow other) => ArePropertiesEqual((IDRMProperties)other) &&
        CreatedOn == other.CreatedOn &&
        ModifiedOn == other.ModifiedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IDRMProperties" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected virtual bool ArePropertiesEqual([DisallowNull] IDRMProperties other) => _description == other.Description &&
        DatePlayExpires == other.DatePlayExpires &&
        DatePlayStarts == other.DatePlayStarts &&
        IsProtected == other.IsProtected &&
        PlayCount == other.PlayCount;
    //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
    //LastSynchronizedOn == other.LastSynchronizedOn &&
    //CreatedOn == other.CreatedOn &&
    //ModifiedOn == other.ModifiedOn;

    public abstract bool Equals(IDRMPropertiesRow other);

    public abstract bool Equals(IDRMProperties other);

    public override int GetHashCode()
    {
        if (TryGetId(out Guid id)) return id.GetHashCode();
        HashCode hash = new();
        hash.Add(_description);
        hash.Add(DatePlayExpires);
        hash.Add(DatePlayStarts);
        hash.Add(IsProtected);
        hash.Add(PlayCount);
        hash.Add(UpstreamId);
        hash.Add(LastSynchronizedOn);
        hash.Add(CreatedOn);
        hash.Add(ModifiedOn);
        return hash.ToHashCode();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
