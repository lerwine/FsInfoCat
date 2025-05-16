using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Base class for entities containing extended file properties for document files.
/// </summary>
/// <seealso cref="DocumentPropertiesListItem" />
/// <seealso cref="DocumentPropertySet" />
/// <seealso cref="LocalDbContext.DocumentPropertySets" />
/// <seealso cref="LocalDbContext.DocumentPropertiesListing" />
public abstract class DocumentPropertiesRow : PropertiesRow, ILocalDocumentPropertiesRow
{
    #region Fields

    private string _clientID = string.Empty;
    private string _lastAuthor = string.Empty;
    private string _revisionNumber = string.Empty;
    private string _division = string.Empty;
    private string _documentID = string.Empty;
    private string _manager = string.Empty;
    private string _presentationFormat = string.Empty;
    private string _version = string.Empty;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the Client ID.
    /// </summary>
    /// <value>The Client ID.</value>
    /// <remarks>
    /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Client ID</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{276D7BB0-5B34-4FB0-AA4B-158ED12A1809} (Format)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>100</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-clientid">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.ClientID), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public string ClientID { get => _clientID; set => _clientID = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Contributor.
    /// </summary>
    /// <value>The document contributor.</value>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Contributor</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{F334115E-DA1B-4509-9B3D-119504DC7ABB} (Format)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>100</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-contributor">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Contributor), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public MultiStringValue Contributor { get; set; }

    /// <summary>
    /// Gets the Date Created.
    /// </summary>
    /// <value>The date and time that a document was created.</value>
    /// <remarks>
    /// This property is stored in the document, not obtained from the file system.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Date Created</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>12</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-datecreated">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.DateCreated), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public DateTime? DateCreated { get; set; }

    /// <summary>
    /// Gets the Last Author.
    /// </summary>
    /// <value>The last person to save the document, as stored in the document.</value>
    /// <remarks>
    /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Last Author</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>8</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-lastauthor">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.LastAuthor), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_lastAuthor))]
    public string LastAuthor { get => _lastAuthor; set => _lastAuthor = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Revision Number.
    /// </summary>
    /// <value>The revision number.</value>
    /// <remarks>
    /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Revision Number</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>9</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-revisionnumber">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.RevisionNumber), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_revisionNumber))]
    public string RevisionNumber { get => _revisionNumber; set => _revisionNumber = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Access control information, from SummaryInfo propset.
    /// </summary>
    /// <value>Access control information, from SummaryInfo propset.</value>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Security</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>19</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-security">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Security), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public int? Security { get; set; }

    /// <summary>
    /// Gets the Division.
    /// </summary>
    /// <value>The Division</value>
    /// <remarks>
    /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Division</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{1E005EE6-BF27-428B-B01C-79676ACD2870} (Format)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>100</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-division">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Division), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_division))]
    public string Division { get => _division; set => _division = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Document ID.
    /// </summary>
    /// <value>The Document ID</value>
    /// <remarks>
    /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Document ID</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{E08805C8-E395-40DF-80D2-54F0D6C43154} (Format)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>100</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-documentid">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.DocumentID), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_documentID))]
    public string DocumentID { get => _documentID; set => _documentID = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Manager.
    /// </summary>
    /// <value>The Manager</value>
    /// <remarks>
    /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Manager</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>14</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-manager">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Manager), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_manager))]
    public string Manager { get => _manager; set => _manager = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Presentation Format.
    /// </summary>
    /// <value>The Presentation Format</value>
    /// <remarks>
    /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Presentation Format</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>3</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-presentationformat">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.PresentationFormat), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_presentationFormat))]
    public string PresentationFormat { get => _presentationFormat; set => _presentationFormat = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Version.
    /// </summary>
    /// <value>The Version</value>
    /// <remarks>
    /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Version Number</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>29</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-version">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Version), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_version))]
    public string Version { get => _version; set => _version = value.AsWsNormalizedOrEmpty(); }

    #endregion

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ILocalDocumentPropertiesRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] ILocalDocumentPropertiesRow other) => ArePropertiesEqual((IDocumentPropertiesRow)other) &&
        EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        LastSynchronizedOn == other.LastSynchronizedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IDocumentPropertiesRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] IDocumentPropertiesRow other) => ArePropertiesEqual((IDocumentProperties)other) &&
        CreatedOn == other.CreatedOn &&
        ModifiedOn == other.ModifiedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IDocumentProperties" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] IDocumentProperties other) => _clientID == other.ClientID &&
        EqualityComparer<MultiStringValue>.Default.Equals(Contributor, other.Contributor) &&
        DateCreated == other.DateCreated &&
        _lastAuthor == other.LastAuthor &&
        _revisionNumber == other.RevisionNumber &&
        Security == other.Security &&
        _division == other.Division &&
        _documentID == other.DocumentID &&
        _manager == other.Manager &&
        _presentationFormat == other.PresentationFormat &&
        _version == other.Version;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract bool Equals(IDocumentPropertiesRow other);

    public abstract bool Equals(IDocumentProperties other);

    public override int GetHashCode()
    {
        if (TryGetId(out Guid id)) return id.GetHashCode();
        HashCode hash = new();
        hash.Add(_clientID);
        hash.Add(Contributor);
        hash.Add(DateCreated);
        hash.Add(_lastAuthor);
        hash.Add(_revisionNumber);
        hash.Add(Security);
        hash.Add(_division);
        hash.Add(_documentID);
        hash.Add(_manager);
        hash.Add(_presentationFormat);
        hash.Add(_version);
        hash.Add(UpstreamId);
        hash.Add(LastSynchronizedOn);
        hash.Add(CreatedOn);
        hash.Add(ModifiedOn);
        return hash.ToHashCode();
    }
    protected virtual string PropertiesToString() => $@"Security={Security}, DocumentID=""{ExtensionMethods.EscapeCsString(_documentID)}"", ClientID=""{ExtensionMethods.EscapeCsString(_clientID)}"", PresentationFormat=""{ExtensionMethods.EscapeCsString(_presentationFormat)}"",
    DateCreated={DateCreated:yyyy-mm-ddTHH:mm:ss.fffffff}, Version=""{ExtensionMethods.EscapeCsString(_version)}"", RevisionNumber=""{ExtensionMethods.EscapeCsString(_revisionNumber)}"",
    LastAuthor=""{ExtensionMethods.EscapeCsString(_lastAuthor)}"", Division=""{ExtensionMethods.EscapeCsString(_division)}"", Manager=""{ExtensionMethods.EscapeCsString(_manager)}"",
    Contributor={Contributor.ToCsString()}";

    public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
