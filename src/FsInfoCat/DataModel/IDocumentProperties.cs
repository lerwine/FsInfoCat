using FsInfoCat.Collections;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for document files.
    /// </summary>
    [EntityInterface]
    public interface IDocumentProperties : IEquatable<IDocumentProperties>
    {
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
        [Display(Name = nameof(Properties.Resources.DisplayName_ClientID), ResourceType = typeof(Properties.Resources))]
        string ClientID { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_Contributor), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Contributor { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_DateCreated), ResourceType = typeof(Properties.Resources))]
        DateTime? DateCreated { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_LastAuthor), ResourceType = typeof(Properties.Resources))]
        string LastAuthor { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_RevisionNumber), ResourceType = typeof(Properties.Resources))]
        string RevisionNumber { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_Security), ResourceType = typeof(Properties.Resources))]
        int? Security { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_Division), ResourceType = typeof(Properties.Resources))]
        string Division { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_DocumentID), ResourceType = typeof(Properties.Resources))]
        string DocumentID { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_Manager), ResourceType = typeof(Properties.Resources))]
        string Manager { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_PresentationFormat), ResourceType = typeof(Properties.Resources))]
        string PresentationFormat { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_Version), ResourceType = typeof(Properties.Resources))]
        string Version { get; }
    }
}
